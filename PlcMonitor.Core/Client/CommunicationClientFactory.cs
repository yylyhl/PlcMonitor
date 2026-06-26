namespace PlcMonitor.Core
{
    /// <summary>
    /// 通信客户端工厂，根据设备类型创建对应适配器
    /// </summary>
    public static class CommunicationClientFactory
    {
        public static ICommunicationClient CreateClient(Device device)
        {
            return device.DeviceType switch
            {
                DeviceType.ModbusTcp => new ModbusTcpClient(device),
                DeviceType.ModbusSerialPort => new ModbusSerialClient(device),
                DeviceType.SiemensS7 => new S7Client(device),
                DeviceType.OpcUa => new OpcUaClient(device),
                _ => throw new NotSupportedException($"不支持的设备类型：{device.DeviceType}")
            };
        }
    }
}
