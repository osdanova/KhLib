using KhLib.Kh2.Dictionaries;
using KhLib.Kh2.Structs;
using Xe.BinaryMapper;

namespace KhLib.Kh2.Battle
{
    public class LimitsFile : BaseTableFile<LimitsFile.Entry>
    {
        public LimitsFile() : base(4, 4, 1) { }

        public static LimitsFile Read(byte[] byteFile)
        {
            LimitsFile file = new LimitsFile();
            file.ReadFile(byteFile);

            return file;
        }

        // LIMIT_TABLE
        public class Entry
        {
            [Data] public byte Id { get; set; }
            [Data] public byte Character1Id { get; set; }
            [Data] public byte Character2Id { get; set; }
            [Data] public byte Character3Id { get; set; }
            [Data(Count = 32)] public string Filename { get; set; }
            [Data] public uint ObjectId { get; set; }
            [Data] public ushort CommandId { get; set; }
            [Data] public ushort ItemId { get; set; }
            [Data] public World_Enum WorldId { get; set; }
            [Data(Count = 19)] public byte[] Padding { get; set; }

            public Entry()
            {
                Padding = new byte[19];
            }
        }
    }
}
