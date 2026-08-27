using Opc.Ua;
using Opc.Ua.Client;
using System.Text;

namespace PlcMonitor.Core
{
    public class OpcUaClient : ICommunicationClient
    {
        private ApplicationConfiguration? _config;
        private ISession? _session;
        private bool _disposed;

        public bool IsConnected => _session?.Connected ?? false;

        public event Action<string>? OnLog;
        public event Action? OnConnectionStateChanged;

        public Device DeviceInfo { get; }
        public OpcUaClient(Device device)
        {
            DeviceInfo = device;
        }

        public async Task<CommunicationResult<bool>> ConnectAsync()
        {
            try
            {
                _config = new ApplicationConfiguration
                {
                    ApplicationName = "ScadaSystem Client",
                    ApplicationUri = $"urn:{System.Net.Dns.GetHostName()}:ScadaSystem.Client",
                    ApplicationType = ApplicationType.Client,
                    SecurityConfiguration = new SecurityConfiguration
                    {
                        AutoAcceptUntrustedCertificates = true,//自动信任证书
                        AddAppCertToTrustedStore = true,
                        ApplicationCertificate = new CertificateIdentifier
                        {
                            StoreType = "Directory",
                            // 关键：使用 %CommonApplicationData% 路径，确保有写权限
                            StorePath = @"%CommonApplicationData%\OPC Foundation\CertificateStores\MachineDefault",
                            SubjectName = "CN=OpcUaClientDemo"
                        },
                        TrustedIssuerCertificates = new CertificateTrustList
                        {
                            StoreType = "Directory",
                            StorePath = @"%CommonApplicationData%\OPC Foundation\CertificateStores\UA Certificate Authorities"
                        },
                        TrustedPeerCertificates = new CertificateTrustList
                        {
                            StoreType = "Directory",
                            StorePath = @"%CommonApplicationData%\OPC Foundation\CertificateStores\UA Applications"
                        },
                        RejectedCertificateStore = new CertificateStoreIdentifier
                        {
                            StoreType = "Directory",
                            StorePath = @"%CommonApplicationData%\OPC Foundation\CertificateStores\RejectedCertificates"
                        },
                        RejectSHA1SignedCertificates = false,
                        MinimumCertificateKeySize = 1024
                    },
                    TransportConfigurations = new TransportConfigurationCollection(),
                    TransportQuotas = new TransportQuotas { OperationTimeout = 1000 },
                    ClientConfiguration = new ClientConfiguration
                    {
                        DefaultSessionTimeout = 60000,
                        MinSubscriptionLifetime = 10000
                    },
                    TraceConfiguration = new TraceConfiguration
                    {
                        TraceMasks = Utils.TraceMasks.Error,
                    }
                };
                await _config.ValidateAsync(ApplicationType.Client);

                // 身份认证
                IUserIdentity identity = string.IsNullOrWhiteSpace(DeviceInfo.OpcUserName) ? new UserIdentity()
                    : new UserIdentity(DeviceInfo.OpcUserName, Encoding.UTF8.GetBytes(DeviceInfo.OpcPassword));

                // 创建会话
                var telemetry = DefaultTelemetry.Create((dd) => { });
                var sessionFactory = new DefaultSessionFactory(telemetry);
                _session = await sessionFactory.CreateAsync(
                    configuration:_config,
                    endpoint: new ConfiguredEndpoint(null, new EndpointDescription(DeviceInfo.OpcEndpointUrl)),
                    updateBeforeConnect: true,
                    sessionName: string.Empty,
                    sessionTimeout: (uint)_config.ClientConfiguration.DefaultSessionTimeout,
                    identity: identity,
                    preferredLocales: null,
                    ct: default);

                _session.SessionClosing += (s, e) => OnConnectionStateChanged?.Invoke();
                _session.KeepAlive += (sender, e) =>
                {
                    if (!ServiceResult.IsGood(e.Status))
                    {
                        OnConnectionStateChanged?.Invoke();// 断线事件
                    }
                };
                OnConnectionStateChanged?.Invoke();
                return CommunicationResult<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                return CommunicationResult<bool>.Fail($"连接失败：{ex.Message}");
            }
        }

        public Task<CommunicationResult<bool>> DisconnectAsync()
        {
            try
            {
                _session?.CloseAsync();
                _session?.Dispose();
                OnConnectionStateChanged?.Invoke();
                return Task.FromResult(CommunicationResult<bool>.Ok(true));
            }
            catch (Exception ex)
            {
                return Task.FromResult(CommunicationResult<bool>.Fail($"断开异常: {ex.Message}"));
            }
        }

        public async Task<CommunicationResult<object?>> ReadAsync(string nodeIdAddress, DataPointType dataType)
        {
            if (!IsConnected || _session == null)
                return CommunicationResult<object?>.Fail("设备未连接");

            try
            {
                // nodeIdAddress为NodeId字符串，如 ns=2;s=Temperature
                NodeId nodeId = NodeId.Parse(nodeIdAddress);
                ReadValueIdCollection nodes =
                [
                    new ReadValueId { NodeId = nodeId, AttributeId = Attributes.Value }
                ];

                var results = await _session.ReadAsync(null, 0, TimestampsToReturn.Neither, nodes, default);
                DataValue value = results.Results[0];
                if (StatusCode.IsBad(value.StatusCode)) return CommunicationResult<object?>.Fail($"状态码: {value.StatusCode}");

                //return CommunicationResult<object?>.Ok(value.Value);
                var ress = dataType switch
                {
                    DataPointType.Bool => Convert.ToBoolean(value.Value),
                    DataPointType.Int16 => Convert.ToInt16(value.Value),
                    DataPointType.UInt16 => Convert.ToUInt16(value.Value),
                    DataPointType.Int32 => Convert.ToInt32(value.Value),
                    DataPointType.UInt32 => Convert.ToUInt32(value.Value),
                    DataPointType.Float => Convert.ToSingle(value.Value),
                    DataPointType.Double => Convert.ToDouble(value.Value),
                    _ => value.Value
                };
                return CommunicationResult<object?>.Ok(ress);
            }
            catch (Exception ex)
            {
                return CommunicationResult<object?>.Fail($"读取失败: [{nodeIdAddress}]{ex.Message}");
            }
        }

        public async Task<CommunicationResult<bool>> WriteAsync(string nodeIdAddress, DataPointType dataType, object value)
        {
            if (!IsConnected || _session == null)
                return CommunicationResult<bool>.Fail("设备未连接");

            try
            {
                NodeId nodeId = NodeId.Parse(nodeIdAddress);
                WriteValueCollection nodes =
                [
                    new WriteValue
                {
                    NodeId = nodeId,
                    AttributeId = Attributes.Value,
                    Value = new DataValue(new Variant(value))
                }
                ];

                var results = await _session.WriteAsync(null, nodes, default); 
                if (StatusCode.IsBad(results.Results[0])) return CommunicationResult<bool>.Fail($"状态码: {results.Results[0]}");
                return CommunicationResult<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                return CommunicationResult<bool>.Fail($"写入失败: [{nodeIdAddress}]{ex.Message}");
            }
        }


        /// <summary>
        /// 订阅单个节点（数据变化自动回调）
        /// </summary>
        /// <param name="nodeId">节点 ID</param>
        /// <param name="onChange">值变化回调</param>
        /// <param name="publishingInterval">发布周期(ms)</param>
        /// <returns>订阅对象，便于后续清理</returns>
        public async Task<Subscription> SubscribeAsync(string nodeId, Action<string, object?, string> onChange, int publishingInterval = 1000)
        {
            if (_session == null || !_session.Connected)
                throw new InvalidOperationException("未连接到 OPC UA 服务器");

            var subscription = new Subscription(_session.DefaultSubscription)
            {
                PublishingInterval = publishingInterval,
                KeepAliveCount = 10,
                LifetimeCount = 100,
                MaxNotificationsPerPublish = 1000,
                PublishingEnabled = true,
                Priority = 0
            };

            _session.AddSubscription(subscription);
            await subscription.CreateAsync();

            var monitoredItem = new MonitoredItem(subscription.DefaultItem)
            {
                StartNodeId = new NodeId(nodeId),
                AttributeId = Attributes.Value,
                DisplayName = nodeId,
                SamplingInterval = 500,
                QueueSize = 10,
                DiscardOldest = true
            };

            monitoredItem.Notification += (item, e) =>
            {
                foreach (var v in item.DequeueValues())
                {
                    if (StatusCode.IsGood(v.StatusCode))
                        onChange?.Invoke(item.DisplayName, v.Value, v.StatusCode.ToString());
                    else
                        onChange?.Invoke(item.DisplayName, null, v.StatusCode.ToString());
                }
            };

            subscription.AddItem(monitoredItem);
            await subscription.ApplyChangesAsync();

            return subscription;
        }
        /// <summary>
        /// 浏览地址空间（返回子节点）
        /// </summary>
        public async Task<ReferenceDescriptionCollection> BrowseAsync(string? nodeId = null)
        {
            if (_session == null || !_session.Connected)
                throw new InvalidOperationException("未连接到 OPC UA 服务器");

            var nodeToBrowse = nodeId == null ? ObjectIds.ObjectsFolder : new NodeId(nodeId);
            var browser = new Browser(_session)
            {
                BrowseDirection = BrowseDirection.Forward,
                NodeClassMask = (uint)(NodeClass.Variable | NodeClass.Object),
                ReferenceTypeId = ReferenceTypeIds.HierarchicalReferences
            };
            return await browser.BrowseAsync(nodeToBrowse);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing) _session?.Dispose();
            _disposed = true;
        }

        ~OpcUaClient() => Dispose(false);
    }
}
