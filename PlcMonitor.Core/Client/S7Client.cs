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

        public event Action<string, string>? OnError;
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
                CpuType cpu = CpuType.S71500; // 默认S7-1500，可根据设备配置修改
                _plc = new Plc(cpu, DeviceInfo.IpAddress, (short)DeviceInfo.StationNo, (short)DeviceInfo.Slot);
                await _plc.OpenAsync();
                IsConnected = _plc.IsConnected;
                OnConnectionStateChanged?.Invoke();
                return CommunicationResult<bool>.Ok(IsConnected);
            }
            catch (Exception ex)
            {
                OnError?.Invoke(DeviceInfo.Name, $"连接失败: {ex.Message}");
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
            if (!IsConnected || _plc == null)
                return CommunicationResult<object?>.Fail("设备未连接");

            try
            {
                object result = await _plc.ReadAsync(address);
                // 类型统一转换
                object? finalValue = dataType switch
                {
                    DataPointType.Bool => Convert.ToBoolean(result),
                    DataPointType.Int16 => Convert.ToInt16(result),
                    DataPointType.UInt16 => Convert.ToUInt16(result),
                    DataPointType.Int32 => Convert.ToInt32(result),
                    DataPointType.Float => Convert.ToSingle(result),
                    _ => result
                };
                return CommunicationResult<object?>.Ok(finalValue);
            }
            catch (Exception ex)
            {
                OnError?.Invoke(DeviceInfo.Name, $"读取失败: [{address}]{ex.Message}");
                return CommunicationResult<object?>.Fail($"读取失败：[{address}]{ex.Message}");
            }
        }

        public async Task<CommunicationResult<bool>> WriteAsync(string address, DataPointType dataType, object value)
        {
            if (!IsConnected || _plc == null)
                return CommunicationResult<bool>.Fail("设备未连接");

            try
            {
                //_plc.Write(address, value);
                await _plc.WriteAsync(address, value);
                return CommunicationResult<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                OnError?.Invoke(DeviceInfo.Name, $"写入失败: [{address}]{ex.Message}");
                return CommunicationResult<bool>.Fail($"写入失败: [{address}]{ex.Message}");
            }
        }

        public void Dispose()
        {
            _plc?.Close();
            //_plc?.Dispose();
            IsConnected = false;
            GC.SuppressFinalize(this);
        }
    }
}
