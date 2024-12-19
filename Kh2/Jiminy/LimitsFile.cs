using KhLib.Kh2.Structs;
using Xe.BinaryMapper;

namespace KhLib.Kh2.Jiminy
{
    public class LimitsFile : BaseTableFile<LimitsFile.Entry>
    {
        public LimitsFile() : base(4, 4, 18, "JMLI", 4, 16) { }

        public static LimitsFile Read(byte[] byteFile)
        {
            LimitsFile file = new LimitsFile();
            file.ReadFile(byteFile);

            return file;
        }

        // JmLimitDataHead | JmLimitDataInfo
        public class Entry
        {
            [Data] public ushort CommandId { get; set; }
            [Data] public ushort TitleId { get; set; }
            [Data] public ushort TextId { get; set; }
            [Data] public ushort Filler { get; set; }
        }
    }
}
