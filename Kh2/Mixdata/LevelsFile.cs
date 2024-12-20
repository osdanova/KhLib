using KhLib.Kh2.Structs;
using Xe.BinaryMapper;

namespace KhLib.Kh2.Mixdata
{
    public class LevelsFile : BaseTableFile<LevelsFile.Entry>
    {
        public LevelsFile() : base(4, 4, 3, "MILV", 4, 16) { }

        public static LevelsFile Read(byte[] byteFile)
        {
            LevelsFile file = new LevelsFile();
            file.ReadFile(byteFile);

            return file;
        }

        // MixLevelDataHead | MixLevelDataInfo
        public class Entry
        {
            [Data] public ushort TitleId { get; set; }
            [Data] public ushort StatId { get; set; }
            [Data] public short Enable { get; set; }
            [Data] public ushort Filler { get; set; }
            [Data] public int Exp { get; set; }
        }
    }
}
