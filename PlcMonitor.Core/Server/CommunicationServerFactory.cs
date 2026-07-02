namespace PlcMonitor.Core
{
    /// <summary>
    /// 通信服务端工厂，根据设备类型创建对应适配器
    /// </summary>
    public class CommunicationServerFactory
    {
        public static ICommunicationServer CreateServer(Device device)
        {
            return device.DeviceType switch
            {
                DeviceType.ModbusTcp => new ModbusTcpSlave(device),
                DeviceType.ModbusSerialPort => new ModbusSerialSlave(device),
                _ => throw new NotSupportedException($"不支持的设备类型：{device.DeviceType}")
            };
        }
    }
}
