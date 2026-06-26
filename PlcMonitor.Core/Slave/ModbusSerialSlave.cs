using NModbus.Serial;
using System.IO.Ports;
using System.Net.Sockets;

namespace PlcMonitor.Core.Slave
{
    /// <summary>
    /// Modbus TCP从站服务，支持多从站、读写事件、数据模拟、受控启停
    /// </summary>
    public class ModbusSerialSlave : IDisposable
    {
        public Device? DeviceInfo { get; }
        private SerialPort? _serialPort;

        private NModbus.IModbusSlaveNetwork? _slaveNetwork;
        private CancellationTokenSource? _cts;
        private Task? _listenTask;
        private bool _disposed;

        /// <summary>
        /// 从站字典：站号,存储区事件
        /// </summary>
        public Dictionary<byte, EventDrivenDataStore> SlaveStores { get; } = new();

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
        
        public ModbusSerialSlave(Device device)
        {
            DeviceInfo = device;
        }
        //public ModbusSerialPortSlaveServer(string portName, int baudRate = 9600, int dataBits = 8, Parity parity = Parity.None, StopBits stopBits = StopBits.One, ProtocolType Protocol = ProtocolType.RTU)
        //{
        //    DeviceInfo = new Device { Protocol = ProtocolType.RTU };
        //    _serialPort = new SerialPort
        //    {
        //        PortName = portName,
        //        BaudRate = baudRate,
        //        Parity = parity,
        //        DataBits = dataBits,
        //        StopBits = stopBits,
        //        ReadTimeout = 3000,
        //        WriteTimeout = 3000
        //    };
        //}

        /// <summary>
        /// 添加一个从站
        /// </summary>
        public void AddSlave(byte slaveId)
        {
            if (SlaveStores.ContainsKey(slaveId))
                throw new InvalidOperationException($"从站 {slaveId} 已存在");

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
            OnLog?.Invoke($"已添加从站，站号: {slaveId}");
        }

        /// <summary>
        /// 启动从站服务
        /// </summary>
        public async Task StartAsync()
        {
            if (_listenTask != null && !_cts!.IsCancellationRequested)
                throw new InvalidOperationException("从站服务已在运行");

            try
            {
                _cts = new CancellationTokenSource();
                _serialPort ??= new SerialPort
                {
                    PortName = DeviceInfo.PortName,
                    BaudRate = DeviceInfo.BaudRate,
                    Parity = DeviceInfo.Parity ?? Parity.None,
                    DataBits = DeviceInfo.DataBits ?? 8,
                    StopBits = DeviceInfo.StopBits ?? StopBits.One,
                    ReadTimeout = 3000,
                    WriteTimeout = 3000
                };
                _serialPort.Open();

                // 注册取消回调：停止监听以中断ListenAsync
                _cts.Token.Register(() => _serialPort.Close());

                var factory = new NModbus.ModbusFactory();
                if (DeviceInfo.SerialMode == SerialMode.ASCII)
                {
                    _slaveNetwork = factory.CreateAsciiSlaveNetwork(_serialPort);
                }
                else { _slaveNetwork = factory.CreateRtuSlaveNetwork(_serialPort); }
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
                        OnLog?.Invoke($"监听异常: {ex.Message}");
                    }
                }, _cts.Token);
                OnLog?.Invoke($"Modbus SerialRtu从站服务已启动，监听:{_serialPort.PortName}, 站号[{string.Join(',', SlaveStores.Keys)}]");
            }
            catch (SocketException ex)
            {
                OnLog?.Invoke($"启动失败，端口 {DeviceInfo?.PortName} 被占用或无权限: {ex.Message}");
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
                _serialPort?.Close();
                _listenTask = null;
                _slaveNetwork = null;
                _serialPort = null;
                _cts?.Dispose();
                _cts = null;
                OnLog?.Invoke("从站服务已停止");
            }
        }

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

        ~ModbusSerialSlave() => Dispose(false);
        #endregion
    }
}
