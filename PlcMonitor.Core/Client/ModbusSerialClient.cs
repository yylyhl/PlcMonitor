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

        public event Action<string, string>? OnError;
        public event Action? OnConnectionStateChanged;

        public Device DeviceInfo { get; }

        public ModbusSerialClient(Device device)
        {
            DeviceInfo = device;
        }

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
                    ReadTimeout = 3000,
                    WriteTimeout = 3000
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
                IsConnected = true;
                OnConnectionStateChanged?.Invoke();
                //var modbusData = await ReadAsync("HR4", DataPointType.Float);
                //if (!modbusData.Success) throw new Exception("连接失败。。。");
                return CommunicationResult<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                OnError?.Invoke(DeviceInfo.Name, $"err: {ex.Message}");
                return CommunicationResult<bool>.Fail($"err：{ex.Message}");
            }
        }

        public Task<CommunicationResult<bool>> DisconnectAsync()
        {
            lock (_lockObj)
            {
                try
                {
                    _master?.Dispose();
                    if (_serialPort?.IsOpen == true)
                        _serialPort.Close();

                    _serialPort?.Dispose();
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
            if (!IsConnected || _master == null) return CommunicationResult<object?>.Fail("设备未连接");

            try
            {
                var (area, addr) = ParseAddress(address);
                ushort startAddr = ushort.Parse(addr);

                ushort[] registers;
                switch (area)
                {
                    case "HR":
                        ushort len = GetRegisterLength(dataType);
                        registers = await _master.ReadHoldingRegistersAsync(DeviceInfo.StationNo, startAddr, len);
                        return CommunicationResult<object?>.Ok(ConvertRegisters(registers, dataType));

                    case "IR":
                        ushort lenIr = GetRegisterLength(dataType);
                        registers = await _master.ReadInputRegistersAsync(DeviceInfo.StationNo, startAddr, lenIr);
                        return CommunicationResult<object?>.Ok(ConvertRegisters(registers, dataType));

                    case "C":
                    case "COIL":
                        bool[] coils = await _master.ReadCoilsAsync(DeviceInfo.StationNo, startAddr, 1);
                        return CommunicationResult<object?>.Ok(coils[0]);

                    case "DI":
                        bool[] dis = await _master.ReadInputsAsync(DeviceInfo.StationNo, startAddr, 1);
                        return CommunicationResult<object?>.Ok(dis[0]);

                    default:
                        return CommunicationResult<object?>.Ok(CommunicationResult<object?>.Fail("不支持的寄存器区域"));
                }
            }
            catch (Exception ex)
            {
                OnError?.Invoke(DeviceInfo.Name, $"读取失败: [{address}]{ex.Message}");
                return CommunicationResult<object?>.Fail($"读取失败：[{address}]{ex.Message}");
            }
        }

        public async Task<CommunicationResult<bool>> WriteAsync(
            string address, DataPointType dataType, object value)
        {
            if (!IsConnected || _master == null) return CommunicationResult<bool>.Fail("设备未连接");

            try
            {
                var (area, addr) = ParseAddress(address);
                ushort startAddr = ushort.Parse(addr);

                if (area is "C" or "COIL")
                {
                    await _master.WriteSingleCoilAsync(DeviceInfo.StationNo, startAddr, Convert.ToBoolean(value));
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

                    await _master.WriteMultipleRegistersAsync(DeviceInfo.StationNo, startAddr, regs);
                    return CommunicationResult<bool>.Ok(true);
                }

                return CommunicationResult<bool>.Fail("该区域不支持写入");
            }
            catch (Exception ex)
            {
                OnError?.Invoke(DeviceInfo.Name, $"写入失败: [{address}]{ex.Message}");
                return CommunicationResult<bool>.Fail($"写入失败：[{address}]{ex.Message}");
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