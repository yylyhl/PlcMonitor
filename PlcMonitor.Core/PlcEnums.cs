namespace PlcMonitor.Core
{
    public enum DeviceType
    {
        Simulation,
        ModbusTcp,
        ModbusSerialPort,
        SiemensS7,
        OpcUa
    }

    public enum DataPointType
    {
        Bool,
        Int16,
        UInt16,
        Int32,
        UInt32,
        Float,
        Double
    }
    public enum ProtocolType
    {
        TCP,
        UDP,
        RTU,
        ASCII,
        RTUoverTCP,
        RTUoverUDP,
    }
}
