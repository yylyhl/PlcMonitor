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
    public enum ModbusMode
    {
        TCP,
        UDP,
        RTU,
        ASCII,
        RTUoverTCP,
        RTUoverUDP,
    }
    public enum SerialMode
    {
        RTU,
        ASCII,
    }
    public enum ModbusFunction
    {
        /// <summary>
        /// Coils [Read/Write]
        /// </summary>
        Coil = 0,
        /// <summary>
        /// Discrete inputs 
        /// </summary>
        DI = 1,
        /// <summary>
        /// Holding registers [Read/Write]
        /// </summary>
        HR = 4,
        /// <summary>
        /// Input registers [Read Only]
        /// </summary>
        IR = 3,
    }
}
