using S7.Net;

namespace PlcMonitor.Core
{
    /// <summary>
    /// 西门子S7 PLC 客户端适配器（基于S7netplus）
    /// 地址格式：DB1.DBD0 / DB1.DBW0 / DB1.DBX0.0 / I0.0 / Q0.0 / MW0
    /// </summary>
    public class S7Client : ICommunicationClient
    {
        private Plc? _plc;
        public bool IsConnected { get; private set; }

        public event Action<string>? OnLog;
        public event Action? OnConnectionStateChanged;

        public Device DeviceInfo { get; }
        public S7Client(Device device)
        {
            DeviceInfo = device;
        }

        public async Task<CommunicationResult<bool>> ConnectAsync()
        {
            try
            {
                //CpuType cpu = CpuType.S71500; // 默认S7-1500，可根据设备配置修改
                CpuType cpu = string.IsNullOrWhiteSpace(DeviceInfo.CpuType) ? CpuType.S71500 : (CpuType)Enum.Parse(typeof(CpuType), DeviceInfo.CpuType, ignoreCase: true);
                _plc = new Plc(cpu, DeviceInfo.IpAddress, DeviceInfo.Port, (short)DeviceInfo.Rack, (short)DeviceInfo.Slot);
                await _plc.OpenAsync();
                IsConnected = _plc.IsConnected;
                OnConnectionStateChanged?.Invoke();
                return CommunicationResult<bool>.Ok(IsConnected);
            }
            catch (Exception ex)
            {
                return CommunicationResult<bool>.Fail($"S7连接失败：{ex.Message}");
            }
        }

        public Task<CommunicationResult<bool>> DisconnectAsync()
        {
            _plc?.Close();
            IsConnected = false;
            OnConnectionStateChanged?.Invoke();
            return Task.FromResult(CommunicationResult<bool>.Ok(true));
        }

        public async Task<CommunicationResult<object?>> ReadAsync(string address, DataPointType dataType)
        {
            if (!IsConnected || _plc == null || !_plc.IsConnected)
                return CommunicationResult<object?>.Fail("设备未连接");
            try
            {
                string[] arr = (address.ToUpper()).Split('.');//如：db2.dbx0.1，db2.dbd4
                string valuetype = arr[1].Substring(0, 3);//取数组中的第二个元素的前三位，用以确认读取的PLC数据类型
                if (valuetype != "DBX" & valuetype != "DBW" & valuetype != "DBD")
                {
                    return CommunicationResult<object?>.Fail("请检查地址是否输入错误");//数据类型：DBX(位，bool） DBB(字节,byte） DBW(字/short,word） DBD(双字/double,dword）
                }
                object result = await _plc.ReadAsync(address.ToUpper());
                //object? finalValue = dataType switch
                //{
                //    DataPointType.Bool => Convert.ToBoolean(result),
                //    DataPointType.Int16 => Convert.ToInt16(result),
                //    DataPointType.UInt16 => Convert.ToUInt16(result),
                //    DataPointType.Int32 => Convert.ToInt32(result),
                //    DataPointType.Float => Convert.ToSingle(result),
                //    _ => result
                //};
                return CommunicationResult<object?>.Ok(result);
            }
            catch (Exception ex)
            {
                if (!_plc.IsConnected) { await DisconnectAsync(); }
                return CommunicationResult<object?>.Fail($"读取失败：[{address}]{ex.Message}");
            }
        }

        public async Task<CommunicationResult<bool>> WriteAsync(string address, DataPointType dataType, object value)
        {
            if (!IsConnected || _plc == null || !_plc.IsConnected)
                return CommunicationResult<bool>.Fail("设备未连接");

            try
            {
                string[] arr = (address.ToUpper()).Split('.');//如：db2.dbx0.1，db2.dbd4
                string valuetype = arr[1].Substring(0, 3);//取数组中的第二个元素的前三位，用以确认读取的PLC数据类型
                if (valuetype != "DBX" & valuetype != "DBW" & valuetype != "DBD")
                {
                    return CommunicationResult<bool>.Fail("请检查地址是否输入错误");//数据类型：DBX(位，bool） DBB(字节,byte） DBW(字/short,word） DBD(双字/double,dword）
                }
                await _plc.WriteAsync(address.ToUpper(), value);
                return CommunicationResult<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                if (!_plc.IsConnected) { await DisconnectAsync(); }
                return CommunicationResult<bool>.Fail($"写入失败: [{address}]{ex.Message}");
            }
        }

        public void Dispose()
        {
            _plc?.Close();
            IsConnected = false;
            GC.SuppressFinalize(this);
        }
    }
}
