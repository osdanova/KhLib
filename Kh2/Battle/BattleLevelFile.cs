using KhLib.Kh2.Structs;
using Xe.BinaryMapper;

namespace KhLib.Kh2.Battle
{
    public class BattleLevelFile : BaseTableFile<BattleLevelFile.Entry>
    {
        public BattleLevelFile() : base(4, 4, 1) { }

        public static BattleLevelFile Read(byte[] byteFile)
        {
            BattleLevelFile file = new BattleLevelFile();
            file.ReadFile(byteFile);

            return file;
        }

        // BATTLELEVEL_TABLE, BATTLELEVEL
        public class Entry
        {
            [Data] public int Id { get; set; }
            [Data] public int ProgressFlag { get; set; }
            [Data(Count=19)] public byte[] WorldBLs { get; set; }
            [Data(Count = 5)] public byte[] Padding { get; set; }

            public Entry()
            {
                WorldBLs = new byte[19];
                Padding = new byte[4];
            }
        }
    }
}
