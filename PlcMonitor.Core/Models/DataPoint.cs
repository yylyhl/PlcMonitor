namespace PlcMonitor.Core
{
    public class DataPoint
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public DataPointType DataType { get; set; }
        public object? CurrentValue { get; set; }
        public DateTime UpdateTime { get; set; }
        public string Unit { get; set; } = string.Empty;


        public bool EnableAlarm { get; set; }
        public double HighLimit { get; set; }
        public double LowLimit { get; set; }
    }
}
