using NModbus;

namespace PlcMonitor.Core
{
    /// <summary>
    /// Sparse storage for points.
    /// </summary>
    public class SparsePointSource<TPoint> : IPointSource<TPoint>
    {
        private readonly Dictionary<ushort, TPoint> _values = new Dictionary<ushort, TPoint>();

        public event EventHandler<StorageEventArgs<TPoint>> StorageOperationOccurred;

        /// <summary>
        /// Gets or sets the value of an individual point wih tout 
        /// </summary>
        /// <param name="registerIndex"></param>
        /// <returns></returns>
        public TPoint this[ushort registerIndex]
        {
            get
            {
                TPoint value;

                if (_values.TryGetValue(registerIndex, out value))
                    return value;

                return default(TPoint);
            }
            set { _values[registerIndex] = value; }
        }

        public TPoint[] ReadPoints(ushort startAddress, ushort numberOfPoints)
        {
            var points = new TPoint[numberOfPoints];

            for (ushort index = 0; index < numberOfPoints; index++)
            {
                points[index] = this[(ushort)(index + startAddress)];
            }

            StorageOperationOccurred?.Invoke(this, new StorageEventArgs<TPoint>(PointOperation.Read, startAddress, points, numberOfPoints));

            return points;
        }

        public void WritePoints(ushort startAddress, TPoint[] points)
        {
            for (ushort index = 0; index < points.Length; index++)
            {
                this[(ushort)(index + startAddress)] = points[index];
            }

            ; StorageOperationOccurred?.Invoke(this, new StorageEventArgs<TPoint>(PointOperation.Write, startAddress, points,default));
        }
    }

    public class StorageEventArgs<TPoint> : EventArgs
    {
        private readonly PointOperation _pointOperation;
        private readonly ushort _startingAddress;
        private readonly ushort _numberOfPoints;
        private readonly TPoint[] _points;

        public StorageEventArgs(PointOperation pointOperation, ushort startingAddress, TPoint[] points, ushort numberOfPoints)
        {
            _pointOperation = pointOperation;
            _startingAddress = startingAddress;
            _numberOfPoints = numberOfPoints;
            _points = points;
        }

        public ushort StartingAddress
        {
            get { return _startingAddress; }
        }
        public ushort NumberOfPoints
        {
            get { return _numberOfPoints; }
        }

        public TPoint[] Points
        {
            get { return _points; }
        }

        public PointOperation Operation
        {
            get { return _pointOperation; }
        }
    }

    public enum PointOperation
    {
        Read,
        Write
    }

    public class EventDrivenDataStore : ISlaveDataStore
    {
        private readonly SparsePointSource<bool> _coilDiscretes;
        private readonly SparsePointSource<bool> _coilInputs;
        private readonly SparsePointSource<ushort> _holdingRegisters;
        private readonly SparsePointSource<ushort> _inputRegisters;

        public EventDrivenDataStore()
        {
            _coilDiscretes = new SparsePointSource<bool>();
            _coilInputs = new SparsePointSource<bool>();
            _holdingRegisters = new SparsePointSource<ushort>();
            _inputRegisters = new SparsePointSource<ushort>();
        }
        /// <summary>
        /// 线圈事件 Discretes Output [Read/Write]
        /// </summary>
        public SparsePointSource<bool> CoilDiscretes
        {
            get { return _coilDiscretes; }
        }
        /// <summary>
        /// 离散输入事件 Discrete Inputs [Read Only]
        /// </summary>
        public SparsePointSource<bool> CoilInputs
        {
            get { return _coilInputs; }
        }
        /// <summary>
        /// 保持寄存器事件 [Read/Write]
        /// </summary>
        public SparsePointSource<ushort> HoldingRegisters
        {
            get { return _holdingRegisters; }
        }
        /// <summary>
        /// 输入寄存器事件 [Read Only]
        /// </summary>
        public SparsePointSource<ushort> InputRegisters
        {
            get { return _inputRegisters; }
        }

        IPointSource<bool> ISlaveDataStore.CoilDiscretes
        {
            get { return _coilDiscretes; }
        }

        IPointSource<bool> ISlaveDataStore.CoilInputs
        {
            get { return _coilInputs; }
        }

        IPointSource<ushort> ISlaveDataStore.HoldingRegisters
        {
            get { return _holdingRegisters; }
        }

        IPointSource<ushort> ISlaveDataStore.InputRegisters
        {
            get { return _inputRegisters; }
        }
    }
}
