namespace PlcMonitor.Core
{
    /// <summary>
    /// 工业通信服务端统一接口
    /// </summary>
    public interface ICommunicationServer : IDisposable
    {
        bool IsStarted { get; }
        Device DeviceInfo { get; }

        Task StartAsync();
        Task StopAsync();

        /// <summary>
        /// 读取单个数据点
        /// </summary>
        bool AddSlave(byte slaveId, out string msg);

        event Action<string>? OnLog;
        event Action<byte, PointOperation, ushort, ushort[], ushort>? HoldingRegistersStorageOperationOccurred;//OnHoldingRegistersRead
        event Action<byte, PointOperation, ushort, ushort[]>? InputRegistersStorageOperationOccurred;//OnHoldingRegistersWritten
        event Action<byte, PointOperation, ushort, bool[], ushort>? CoilDiscretesStorageOperationOccurred;//OnCoilsRead
        event Action<byte, PointOperation, ushort, bool[]>? CoilInputsStorageOperationOccurred;//OnCoilsWritten
    }
}
