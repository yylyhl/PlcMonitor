using NModbus;
using NModbus.Serial;
using System.IO.Ports;

namespace PlcMonitor.Core
{
    /// <summary>
    /// Modbus RTU 串口客户端适配器（基于 NModbus 3.x）
    /// 地址格式同 RTU：HR:100 / IR:100 / C:0 / DI:0
    /// </summary>
    public class ModbusSerialClient : ICommunicationClient
    {
        private SerialPort _serialPort;
        private IModbusSerialMaster? _master;
        private readonly object _lockObj = new();

        public bool IsConnected { get; private set; }

        public event Action<string>? OnLog;
        public event Action? OnConnectionStateChanged;

        public Device DeviceInfo { get; }

        public ModbusSerialClient(Device device)
        {
            DeviceInfo = device;
        }
        #region Heart Beat Check
        private CancellationTokenSource _cts;
        private bool _isRunning = false;
        private Task _heartBeatTask;
        private int _slaveFailCount = 0;
        private void StopHeartBeat()
        {
            if (!_isRunning) return;
            _cts?.Cancel();
            if (_heartBeatTask.IsCanceled) _heartBeatTask?.Wait(700);
            _cts?.Dispose();
            _isRunning = false;
            _slaveFailCount = 0;
        }
        private async Task StartHeartBeat(byte slaveId)
        {
            if (_isRunning) return;
            _cts = new CancellationTokenSource();
            _isRunning = true;
            _heartBeatTask = Task.Run(async () =>
            {
                while (!_cts.Token.IsCancellationRequested)
                {
                    var result = await TryReadTestRegAsync(slaveId);
                    if (result)
                    {
                        _slaveFailCount = 0;// 通信正常：清零失败计数，标记在线
                    }
                    else
                    {
                        Interlocked.Increment(ref _slaveFailCount);// 通信失败：计数+1
                        if (_slaveFailCount >= 2)
                        {
                            await DisconnectAsync();// 连续失败达到阈值，判定离线
                            break;
                        }
                    }
                    await Task.Delay(500, _cts.Token);
                }
            }, _cts.Token);
        }
        private async Task<bool> TryReadTestRegAsync(byte slaveId)
        {
            if (_master == null || _serialPort == null || !_serialPort.IsOpen) return false;
            //using var timeCts = new CancellationTokenSource(200);
            try
            {
                var delayTask = Task.Delay(200);
                var completed = await Task.WhenAny(_master.ReadHoldingRegistersAsync(slaveId, 0, 1), delayTask);
                return completed != delayTask;
            }
            catch (TimeoutException)
            {
                return false;
            }
            catch (Exception ex)
            {
                OnLog?.Invoke($"CheckHeartBeat-err[{ex.Message}]");
                return true;
            }
        } 
        #endregion
        public async Task<CommunicationResult<bool>> ConnectAsync()
        {
            try
            {
                _serialPort = new SerialPort
                {
                    PortName = DeviceInfo.PortName,
                    BaudRate = DeviceInfo.BaudRate,
                    Parity = DeviceInfo.Parity ?? Parity.None,
                    DataBits = DeviceInfo.DataBits ?? 8,
                    StopBits = DeviceInfo.StopBits ?? StopBits.One,
                    ReadTimeout = 1700,
                    WriteTimeout = 1700
                };
                _serialPort.DataReceived += (sender, args) =>
                {
                    OnLog?.Invoke($"[ModbusSerialClient]DataReceived-{sender} [{args.EventType}]");
                };
                _serialPort.ErrorReceived += (sender, args) =>
                {
                    OnLog?.Invoke($"[ModbusSerialClient]ErrorReceived-{sender} [{args.EventType}]");
                };
                _serialPort.Open();
                var factory = new ModbusFactory();
                var serialPort = new SerialPortAdapter(_serialPort);//serialPort = new SerialPort(DeviceInfo.PortName);
                if (DeviceInfo.SerialMode == SerialMode.ASCII)
                {
                    _master = factory.CreateAsciiMaster(serialPort);
                }
                else { _master = factory.CreateRtuMaster(serialPort); }
                _master.Transport.Retries = 3;
                if (!await TryReadTestRegAsync(DeviceInfo.SlaveId))
                {
                    await DisconnectAsync();
                    return CommunicationResult<bool>.Fail($"连接失败");
                }
                IsConnected = true;
                OnConnectionStateChanged?.Invoke();
                await StartHeartBeat(DeviceInfo.SlaveId);
                //var modbusData = await ReadAsync("HR4", DataPointType.Float);
                //if (!modbusData.Success) throw new Exception("连接失败。。。");
                return CommunicationResult<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                return CommunicationResult<bool>.Fail($"err：{ex.Message}");
            }
        }

        public Task<CommunicationResult<bool>> DisconnectAsync()
        {
            lock (_lockObj)
            {
                try
                {
                    StopHeartBeat();
                    _master?.Dispose();
                    if (_serialPort?.IsOpen == true)
                        _serialPort.Close();

                    _serialPort?.Dispose();
                    _master?.Dispose();
                    _master = null;
                }
                finally
                {
                    IsConnected = false;
                    OnConnectionStateChanged?.Invoke();
                }
            }

            return Task.FromResult(CommunicationResult<bool>.Ok(true));
        }

        public async Task<CommunicationResult<object?>> ReadAsync(string address, DataPointType dataType)
        {
            if (!IsConnected || _master == null || !_serialPort.IsOpen) return CommunicationResult<object?>.Fail("设备未连接");
            try
            {
                var (area, addr) = ParseAddress(address);
                ushort startAddr = ushort.Parse(addr);

                ushort[] registers;
                switch (area)
                {
                    case "HR":
                        ushort len = GetRegisterLength(dataType);
                        registers = await _master.ReadHoldingRegistersAsync(DeviceInfo.SlaveId, startAddr, len);
                        return CommunicationResult<object?>.Ok(ConvertRegisters(registers, dataType));

                    case "IR":
                        ushort lenIr = GetRegisterLength(dataType);
                        registers = await _master.ReadInputRegistersAsync(DeviceInfo.SlaveId, startAddr, lenIr);
                        return CommunicationResult<object?>.Ok(ConvertRegisters(registers, dataType));

                    case "C":
                    case "COIL":
                        bool[] coils = await _master.ReadCoilsAsync(DeviceInfo.SlaveId, startAddr, 1);
                        return CommunicationResult<object?>.Ok(coils[0]);

                    case "DI":
                        bool[] dis = await _master.ReadInputsAsync(DeviceInfo.SlaveId, startAddr, 1);
                        return CommunicationResult<object?>.Ok(dis[0]);

                    default:
                        return CommunicationResult<object?>.Ok(CommunicationResult<object?>.Fail("不支持的寄存器区域"));
                }
            }
            catch (TimeoutException ex)
            {
                await DisconnectAsync();
                return CommunicationResult<object?>.Fail($"Read-TimeoutException：[{address}]{ex.Message}");
            }
            catch (Exception ex)
            {
                return CommunicationResult<object?>.Fail($"Read-Exception：[{address}]{ex.Message}");
            }
        }

        public async Task<CommunicationResult<bool>> WriteAsync(string address, DataPointType dataType, object value)
        {
            if (!IsConnected || _master == null || !_serialPort.IsOpen) return CommunicationResult<bool>.Fail("设备未连接");
            try
            {
                var (area, addr) = ParseAddress(address);
                ushort startAddr = ushort.Parse(addr);

                if (area is "C" or "COIL")
                {
                    await _master.WriteSingleCoilAsync(DeviceInfo.SlaveId, startAddr, Convert.ToBoolean(value));
                    return CommunicationResult<bool>.Ok(true);
                }

                if (area == "HR")
                {
                    ushort[] regs = dataType switch
                    {
                        DataPointType.Int16 or DataPointType.UInt16 => [Convert.ToUInt16(value)],

                        DataPointType.Float => DataConvertHelper.FloatToRegisters(Convert.ToSingle(value)),

                        _ => [Convert.ToUInt16(value)]
                    };

                    await _master.WriteMultipleRegistersAsync(DeviceInfo.SlaveId, startAddr, regs);
                    return CommunicationResult<bool>.Ok(true);
                }

                return CommunicationResult<bool>.Fail("该区域不支持写入");
            }
            catch (TimeoutException ex)
            {
                await DisconnectAsync();
                return CommunicationResult<bool>.Fail($"Write-TimeoutException：[{address}]{ex.Message}");
            }
            catch (Exception ex)
            {
                return CommunicationResult<bool>.Fail($"Write-Exception：[{address}]{ex.Message}");
            }
        }

        private static (string type, string addr) ParseAddress(string address)
        {
            int i = 0;
            while (i < address.Length && !char.IsDigit(address[i])) i++;

            if (i == 0) throw new ArgumentException("地址格式示例: HR0、C0、DI0、IR0");
            if (!int.TryParse(address[i..], out _)) throw new ArgumentException("地址数字非法");

            return (address[..i], address[i..]);
        }

        private static ushort GetRegisterLength(DataPointType dataType) =>
            dataType switch
            {
                DataPointType.Bool or
                DataPointType.Int16 or
                DataPointType.UInt16 => 1,
                DataPointType.Int32 or
                DataPointType.UInt32 or
                DataPointType.Float => 2,
                DataPointType.Double => 4,
                _ => 1
            };

        private static object ConvertRegisters(ushort[] registers, DataPointType dataType) =>
            dataType switch
            {
                DataPointType.UInt16 => registers[0],
                DataPointType.Int16 => DataConvertHelper.ToInt16(registers[0]),
                DataPointType.Float => DataConvertHelper.RegistersToFloat(registers),
                DataPointType.Int32 => DataConvertHelper.RegistersToInt32(registers),
                _ => registers[0]
            };

        public void Dispose()
        {
            DisconnectAsync().Wait();
            GC.SuppressFinalize(this);
        }
    }
}