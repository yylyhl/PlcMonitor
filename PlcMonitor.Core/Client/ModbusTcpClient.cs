using NModbus;
using System.Net.Sockets;

namespace PlcMonitor.Core
{
    /// <summary>
    /// Modbus TCP 客户端适配器（基于NModbus 3.0.83）
    /// 地址格式：HR:100 保持寄存器 / IR:100 输入寄存器 / Coil:0 线圈 / DI:0 离散输入
    /// </summary>
    public class ModbusTcpClient : ICommunicationClient
    {
        private TcpClient? _tcpClient;
        private IModbusMaster? _master;
        private readonly object _lockObj = new();

        public bool IsConnected { get; private set; }

        public event Action<string>? OnLog;
        public event Action? OnConnectionStateChanged;

        public Device DeviceInfo { get; }
        public ModbusTcpClient(Device device)
        {
            DeviceInfo = device;
        }

        public async Task<CommunicationResult<bool>> ConnectAsync()
        {
            try
            {
                _tcpClient = new TcpClient();
                await _tcpClient.ConnectAsync(DeviceInfo.IpAddress, DeviceInfo.Port == 0 ? 502 : DeviceInfo.Port);
                var factory = new ModbusFactory();
                _master = factory.CreateMaster(_tcpClient);
                _master.Transport.ReadTimeout = 1700;
                _master.Transport.WriteTimeout = 1700;
                IsConnected = true;
                OnConnectionStateChanged?.Invoke();
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
                _master?.Dispose();
                _tcpClient?.Close();
                _tcpClient?.Dispose();
                IsConnected = false;
                OnConnectionStateChanged?.Invoke();
            }
            return Task.FromResult(CommunicationResult<bool>.Ok(true));
        }

        public async Task<CommunicationResult<object?>> ReadAsync(string address, DataPointType dataType)
        {
            if (!IsConnected || _tcpClient == null || !_tcpClient.Connected || _master == null) 
                return CommunicationResult<object?>.Fail("设备未连接");

            try
            {
                //数据存储区
                //保持寄存器（Holding Registers）：地址从 0 开始，用于存储可读写的 16 位数据（如传感器值）。
                //输入寄存器（Input Registers）：地址从 0 开始，仅支持读取（如设备状态）。
                //线圈（Coils）：布尔值（0/1），地址从 0 开始。
                //离散输入（Discrete Inputs）：只读布尔值。
                var (area, addr) = ParseAddress(address);
                ushort startAddr = ushort.Parse(addr);
                //var parts = address.Split(':');
                //if (parts.Length != 2) return CommunicationResult<object?>.Fail("地址格式错误，示例：HR:100、C:0、DI:0、IR:0");

                //string area = parts[0].ToUpper();
                //ushort startAddr = ushort.Parse(parts[1]);

                ushort[] registers;
                switch (area)
                {
                    case "HR": // 保持寄存器
                        ushort len = dataType switch
                        {
                            DataPointType.Bool or DataPointType.Int16 or DataPointType.UInt16 => 1,
                            DataPointType.Int32 or DataPointType.UInt32 or DataPointType.Float => 2,
                            DataPointType.Double => 4,
                            _ => 1
                        };
                        registers = await _master.ReadHoldingRegistersAsync(DeviceInfo.SlaveId, startAddr, len);
                        return CommunicationResult<object?>.Ok(ConvertRegisters(registers, dataType));
                    case "IR": // 输入寄存器
                        ushort lenIr = dataType switch
                        {
                            DataPointType.Int16 or DataPointType.UInt16 => 1,
                            DataPointType.Int32 or DataPointType.Float => 2,
                            _ => 1
                        };
                        registers = await _master.ReadInputRegistersAsync(DeviceInfo.SlaveId, startAddr, lenIr);
                        return CommunicationResult<object?>.Ok(ConvertRegisters(registers, dataType));
                    case "C": // 线圈
                    case "COIL": // 线圈
                        bool[] coils = await _master.ReadCoilsAsync(DeviceInfo.SlaveId, startAddr, 1);
                        return CommunicationResult<object?>.Ok(coils[0]);
                    case "DI": // 离散输入
                        bool[] dis = await _master.ReadInputsAsync(DeviceInfo.SlaveId, startAddr, 1);
                        return CommunicationResult<object?>.Ok(dis[0]);
                    default:
                        return CommunicationResult<object?>.Fail("不支持的寄存器区域");
                }
            }
            catch (Exception ex)
            {
                if (!_tcpClient.Connected) { await DisconnectAsync(); }
                return CommunicationResult<object?>.Fail($"读取失败：[{address}]{ex.Message}");
            }
        }
        private static (string type, string addr) ParseAddress(string address)
        {
            int i = 0;//第一个数字的位置
            while (i < address.Length && !char.IsDigit(address[i])) i++;
            if (i == 0) throw new ArgumentException("地址格式示例: HR0、C0、DI0、IR0");
            if (!int.TryParse(address[i..], out _)) throw new ArgumentException("地址数字非法");//防止如：HR001
            return (address[..i], address[i..]);
        }
        public async Task<CommunicationResult<bool>> WriteAsync(string address, DataPointType dataType, object value)
        {
            if (!IsConnected || _tcpClient == null || !_tcpClient.Connected || _master == null) 
                return CommunicationResult<bool>.Fail("设备未连接");

            try
            {
                var (area, addr) = ParseAddress(address);
                ushort startAddr = ushort.Parse(addr);

                //var parts = address.Split(':');
                //if (parts.Length != 2) return CommunicationResult<bool>.Fail("地址格式错误，示例：HR:100、C:0、DI:0、IR:0");

                //string area = parts[0].ToUpper();
                //ushort startAddr = ushort.Parse(parts[1]);

                if (area.Equals("C", StringComparison.OrdinalIgnoreCase) || area.Equals("COIL", StringComparison.OrdinalIgnoreCase))
                {
                    await _master.WriteSingleCoilAsync(DeviceInfo.SlaveId, startAddr, Convert.ToBoolean(value));
                    return CommunicationResult<bool>.Ok(true);
                }

                if (area.Equals("HR", StringComparison.OrdinalIgnoreCase))
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
            catch (Exception ex)
            {
                if (!_tcpClient.Connected) { await DisconnectAsync(); }
                return CommunicationResult<bool>.Fail($"写入失败: [{address}]{ex.Message}");
            }
        }

        private static float ConvertRegisters(ushort[] registers, DataPointType dataType)
        {
            return dataType switch
            {
                DataPointType.UInt16 => registers[0],
                DataPointType.Int16 => DataConvertHelper.ToInt16(registers[0]),
                DataPointType.Float => DataConvertHelper.RegistersToFloat(registers),
                DataPointType.Int32 => DataConvertHelper.RegistersToInt32(registers),
                _ => registers[0]
            };
        }

        public void Dispose()
        {
            DisconnectAsync().Wait();
            GC.SuppressFinalize(this);
        }
    }
}
