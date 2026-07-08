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
                // 客户端配置（开发环境自动信任证书，生产环境需替换）
                _config = new ApplicationConfiguration
                {
                    ApplicationName = "ScadaSystem Client",
                    ApplicationType = ApplicationType.Client,
                    SecurityConfiguration = new SecurityConfiguration
                    {
                        AutoAcceptUntrustedCertificates = true,
                        RejectSHA1SignedCertificates = false,
                        MinimumCertificateKeySize = 1024
                    },
                    TransportQuotas = new TransportQuotas { OperationTimeout = 1000 },
                    ClientConfiguration = new ClientConfiguration { DefaultSessionTimeout = 60000 }
                };
                //await _config.Validate(ApplicationType.Client);
                await _config.ValidateAsync(ApplicationType.Client);

                // 身份认证
                IUserIdentity identity = string.IsNullOrWhiteSpace(DeviceInfo.OpcUserName) ? new UserIdentity()
                    : new UserIdentity(DeviceInfo.OpcUserName, Encoding.UTF8.GetBytes(DeviceInfo.OpcPassword));

                // 创建会话
                var telemetry = DefaultTelemetry.Create(default);
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

        public async Task<CommunicationResult<object?>> ReadAsync(string address, DataPointType dataType)
        {
            if (!IsConnected || _session == null)
                return CommunicationResult<object?>.Fail("设备未连接");

            try
            {
                // address为NodeId字符串，如 ns=2;s=Temperature
                NodeId nodeId = NodeId.Parse(address);
                ReadValueIdCollection nodes =
                [
                    new ReadValueId { NodeId = nodeId, AttributeId = Attributes.Value }
                ];

                var results = await _session.ReadAsync(null, 0, TimestampsToReturn.Neither, nodes, default);
                DataValue value = results.Results[0];
                if (StatusCode.IsBad(value.StatusCode)) return CommunicationResult<object?>.Fail("状态码: {value.StatusCode}");

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
                return CommunicationResult<object?>.Fail($"读取失败: [{address}]{ex.Message}");
            }
        }

        public async Task<CommunicationResult<bool>> WriteAsync(string address, DataPointType dataType, object value)
        {
            if (!IsConnected || _session == null)
                return CommunicationResult<bool>.Fail("设备未连接");

            try
            {
                NodeId nodeId = NodeId.Parse(address);
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
                return CommunicationResult<bool>.Fail($"写入失败: [{address}]{ex.Message}");
            }
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
