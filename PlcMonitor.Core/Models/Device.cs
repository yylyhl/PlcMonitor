namespace PlcMonitor.Core
{
    public class Device
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        public string Name { get; set; } = string.Empty;
        public DeviceType DeviceType { get; set; }
        public string IpAddress { get; set; } = "127.0.0.1";
        public int Port { get; set; }

        #region 串口专用
        public ProtocolType Protocol { get; set; } = ProtocolType.TCP;
        public string PortName { get; set; } = "COM1";  // COM3
        public int BaudRate { get; set; } = 9600;
        public System.IO.Ports.Parity? Parity { get; set; } = System.IO.Ports.Parity.None;
        public int? DataBits { get; set; } = 8;
        public System.IO.Ports.StopBits? StopBits { get; set; } = System.IO.Ports.StopBits.One; 
        #endregion

        /// <summary>
        /// Modbus站号 / S7机架号
        /// </summary>
        public byte StationNo { get; set; } = 1;

        /// <summary>
        /// S7槽号
        /// </summary>
        public int Slot { get; set; } = 1;
        public int Rack { get; set; } = 0;

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
