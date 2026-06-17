namespace PlcMonitor.Core
{
    /// <summary>
    /// 工业通信客户端统一接口
    /// </summary>
    public interface ICommunicationClient : IDisposable
    {
        bool IsConnected { get; }
        Device DeviceInfo { get; }

        Task<CommunicationResult<bool>> ConnectAsync();
        Task<CommunicationResult<bool>> DisconnectAsync();

        /// <summary>
        /// 读取单个数据点
        /// </summary>
        Task<CommunicationResult<object?>> ReadAsync(string address, DataPointType dataType);

        /// <summary>
        /// 写入单个数据点
        /// </summary>
        Task<CommunicationResult<bool>> WriteAsync(string address, DataPointType dataType, object value);

        event Action<string, string>? OnError;
        event Action? OnConnectionStateChanged;
    }
}
