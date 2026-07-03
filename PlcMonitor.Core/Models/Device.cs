namespace PlcMonitor.Core
{
    public class Device
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        public string Name { get; set; } = string.Empty;
        public DeviceType DeviceType { get; set; }
        public string IpAddress { get; set; } = "127.0.0.1";
        public int Port { get; set; }
        /// <summary>
        /// Modbus站号
        /// </summary>
        public byte SlaveId { get; set; } = 1;

        #region 串口专用
        public SerialMode SerialMode { get; set; } = SerialMode.RTU;
        public string PortName { get; set; } = "COM1";  // COM3
        public int BaudRate { get; set; } = 9600;
        public System.IO.Ports.Parity? Parity { get; set; } = System.IO.Ports.Parity.None;
        public int? DataBits { get; set; } = 8;
        public System.IO.Ports.StopBits? StopBits { get; set; } = System.IO.Ports.StopBits.One; 
        #endregion

        #region S7
        /// <summary>
        /// S7插槽：S7-300/400CPU=2；1200/1500=1；200SMART=1
        /// </summary>
        public int Slot { get; set; } = 1;
        /// <summary>
        /// S7机架：S7-300/400=0；S7-1200/1500/200SMART=0
        /// </summary>
        public int Rack { get; set; } = 0; 
        #endregion

        #region OPC UA
        public string OpcEndpointUrl { get; set; } = string.Empty;
        public string OpcUserName { get; set; } = string.Empty;
        public string OpcPassword { get; set; } = string.Empty; 
        #endregion

        /// <summary>
        /// 采集周期ms
        /// </summary>
        public int ScanInterval { get; set; } = 500;
        public List<DataPoint> DataPoints { get; set; } = [];
        public bool IsConnected { get; set; }
        public DateTime LastUpdateTime { get; set; }
    }
}
