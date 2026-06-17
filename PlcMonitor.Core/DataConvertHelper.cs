namespace PlcMonitor.Core
{
    /// <summary>
    /// 工业数据类型转换工具（处理大小端、寄存器拼接）
    /// </summary>
    public static class DataConvertHelper
    {
        /// <summary>
        /// ushort寄存器数组转Float（ABCD顺序，标准Modbus大端）
        /// </summary>
        public static float RegistersToFloat(ushort[] registers, int startIndex = 0)
        {
            byte[] bytes = new byte[4];
            BitConverter.GetBytes(registers[startIndex]).CopyTo(bytes, 2);
            BitConverter.GetBytes(registers[startIndex + 1]).CopyTo(bytes, 0);
            return BitConverter.ToSingle(bytes, 0);
        }

        /// <summary>
        /// Float转ushort寄存器数组
        /// </summary>
        public static ushort[] FloatToRegisters(float value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            return [BitConverter.ToUInt16(bytes, 2), BitConverter.ToUInt16(bytes, 0)];
        }

        /// <summary>
        /// ushort转Int16
        /// </summary>
        public static short ToInt16(ushort value) => (short)value;

        /// <summary>
        /// 两个ushort转Int32
        /// </summary>
        public static int RegistersToInt32(ushort[] registers, int startIndex = 0)
        {
            byte[] bytes = new byte[4];
            BitConverter.GetBytes(registers[startIndex]).CopyTo(bytes, 2);
            BitConverter.GetBytes(registers[startIndex + 1]).CopyTo(bytes, 0);
            return BitConverter.ToInt32(bytes, 0);
        }
    }
}
