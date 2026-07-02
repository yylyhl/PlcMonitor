using System.Net;
using System.Net.Sockets;

namespace PlcMonitor.Core
{
    /// <summary>
    /// Modbus TCP从站服务，支持多从站、读写事件、数据模拟、受控启停
    /// </summary>
    public class ModbusTcpSlave : ICommunicationServer//: IDisposable//: ICommunicationServer
    {
        public bool IsStarted { get; private set; }
        public Device DeviceInfo { get; }
        //private readonly int _port;
        //private readonly IPAddress _listenIp;
        private TcpListener? _tcpListener;
        private NModbus.IModbusSlaveNetwork? _slaveNetwork;
        private CancellationTokenSource? _cts;
        private Task? _listenTask;
        private bool _disposed;

        /// <summary>
        /// 从站字典：站号,存储区事件
        /// </summary>
        private Dictionary<byte, EventDrivenDataStore> SlaveStores { get; } = new();

        /// <summary>
        /// 日志事件
        /// </summary>
        public event Action<string>? OnLog;

        #region 对外暴露的转发事件
        public event Action<byte, PointOperation, ushort, ushort[], ushort>? HoldingRegistersStorageOperationOccurred;//OnHoldingRegistersRead
        public event Action<byte, PointOperation, ushort, ushort[]>? InputRegistersStorageOperationOccurred;//OnHoldingRegistersWritten
        public event Action<byte, PointOperation, ushort, bool[], ushort>? CoilDiscretesStorageOperationOccurred;//OnCoilsRead
        public event Action<byte, PointOperation, ushort, bool[]>? CoilInputsStorageOperationOccurred;//OnCoilsWritten
        #endregion
        public ModbusTcpSlave(Device device)
        {
            DeviceInfo = device;
        }

        /// <summary>
        /// 添加一个从站
        /// </summary>
        public bool AddSlave(byte slaveId, out string msg)
        {
            msg = string.Empty;
            if (SlaveStores.ContainsKey(slaveId))
            {
                msg = $"[ModbusSerialSlave]从站 {slaveId} 已存在";
                return false;
            }

            var dataStore = new EventDrivenDataStore();

            dataStore.HoldingRegisters.StorageOperationOccurred += (sender, args)
                => HoldingRegistersStorageOperationOccurred?.Invoke(slaveId, args.Operation, args.StartingAddress, args.Points, args.NumberOfPoints);

            dataStore.InputRegisters.StorageOperationOccurred += (sender, args)
                => InputRegistersStorageOperationOccurred?.Invoke(slaveId, args.Operation, args.StartingAddress, args.Points);

            dataStore.CoilDiscretes.StorageOperationOccurred += (sender, args)
                => CoilDiscretesStorageOperationOccurred?.Invoke(slaveId, args.Operation, args.StartingAddress, args.Points, args.NumberOfPoints);

            dataStore.CoilInputs.StorageOperationOccurred += (sender, args)
                => CoilInputsStorageOperationOccurred?.Invoke(slaveId, args.Operation, args.StartingAddress, args.Points);

            SlaveStores[slaveId] = dataStore;
            OnLog?.Invoke($"[ModbusTcpSlave]已添加从站 {slaveId}");
            return true;
        }
        private string SlaveIds { get { return string.Join(',', SlaveStores.Keys); } }

        /// <summary>
        /// 启动从站服务
        /// </summary>
        public async Task StartAsync()
        {
            if (_listenTask != null && !_cts!.IsCancellationRequested)
                throw new InvalidOperationException($"[ModbusTcpSlave]从站 [{SlaveIds}] 服务已在运行");

            try
            {
                _cts = new CancellationTokenSource();
                var ipAddress = string.IsNullOrWhiteSpace(DeviceInfo.IpAddress) ? IPAddress.Any : IPAddress.Parse(DeviceInfo.IpAddress);
                _tcpListener = new TcpListener(ipAddress, DeviceInfo.Port);
                _tcpListener.Start();

                // 注册取消回调：停止监听以中断ListenAsync
                _cts.Token.Register(() => _tcpListener.Stop());

                var factory = new NModbus.ModbusFactory();
                _slaveNetwork = factory.CreateSlaveNetwork(_tcpListener);

                // 将所有从站加入网络
                foreach (var (slaveId, dataStore) in SlaveStores)
                {
                    var slave = factory.CreateSlave(slaveId, dataStore);
                    _slaveNetwork.AddSlave(slave);
                }
                // 后台运行监听循环
                _listenTask = Task.Run(async () =>
                {
                    try
                    {
                        await _slaveNetwork.ListenAsync();
                    }
                    catch (ObjectDisposedException)
                    {
                        // 正常停止导致的对象释放，忽略
                    }
                    catch (SocketException ex) when (ex.SocketErrorCode == SocketError.Interrupted)
                    {
                        // 取消监听导致的中断，忽略
                    }
                    catch (Exception ex)
                    {
                        OnLog?.Invoke($"[ModbusTcpSlave]从站 [{SlaveIds}] 监听异常: {ex.Message}");
                    }
                }, _cts.Token);
                OnLog?.Invoke($"[ModbusTcpSlave]从站 [{SlaveIds}] 服务已启动，监听地址:{DeviceInfo.IpAddress}:{DeviceInfo.Port}, 站号[{string.Join(',', SlaveStores.Keys)}]");
            }
            catch (SocketException ex)
            {
                OnLog?.Invoke($"[ModbusTcpSlave]从站 [{SlaveIds}] 启动失败，端口 {DeviceInfo.Port} 被占用或无权限: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 停止从站服务
        /// </summary>
        public async Task StopAsync()
        {
            try
            {
                _cts?.Cancel();
                if (_listenTask != null)
                    await _listenTask.WaitAsync(TimeSpan.FromSeconds(3));
            }
            catch { }
            finally
            {
                _slaveNetwork?.Dispose();
                _tcpListener?.Stop();
                _listenTask = null;
                _slaveNetwork = null;
                _tcpListener = null;
                _cts?.Dispose();
                _cts = null;
                OnLog?.Invoke($"[ModbusTcpSlave]从站 [{SlaveIds}] 服务已停止");
            }
        }

        #region 外部数据模拟方法（模拟硬件传感器数值变化）
        ///// <summary>
        ///// 设置单个保持寄存器的值
        ///// </summary>
        //public void SetHoldingRegister(byte slaveId, ushort address, ushort value)
        //{
        //    if (!SlaveStores.TryGetValue(slaveId, out var store))
        //        throw new ArgumentException($"从站 {slaveId} 不存在");
        //    store.WriteHoldingRegisters(slaveId, address, [value]);
        //}

        ///// <summary>
        ///// 设置保持寄存器的Float值（大端字节序，与主站逻辑完全对应）
        ///// </summary>
        //public void SetHoldingRegisterFloat(byte slaveId, ushort address, float value)
        //{
        //    if (!SlaveStores.TryGetValue(slaveId, out var store))
        //        throw new ArgumentException($"从站 {slaveId} 不存在");

        //    // 与主站读取逻辑完全一致：小端转大端
        //    byte[] bytes = BitConverter.GetBytes(value);
        //    Array.Reverse(bytes);

        //    ushort[] regs = new ushort[2];
        //    regs[0] = (ushort)(bytes[0] << 8 | bytes[1]);
        //    regs[1] = (ushort)(bytes[2] << 8 | bytes[3]);

        //    store.WriteHoldingRegisters(slaveId, address, regs);
        //}

        ///// <summary>
        ///// 设置单个线圈状态
        ///// </summary>
        //public void SetCoil(byte slaveId, ushort address, bool value)
        //{
        //    if (!SlaveStores.TryGetValue(slaveId, out var store))
        //        throw new ArgumentException($"从站 {slaveId} 不存在");
        //    store.WriteCoils(slaveId, address, [value]);
        //}
        #endregion

        #region Dispose模式
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            {
                StopAsync().GetAwaiter().GetResult();
            }
            _disposed = true;
        }

        ~ModbusTcpSlave() => Dispose(false);
        #endregion
    }
}
