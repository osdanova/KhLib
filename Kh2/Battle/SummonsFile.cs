using KhLib.Kh2.Structs;
using Xe.BinaryMapper;

namespace KhLib.Kh2.Battle
{
    public class SummonsFile : BaseTableFile<SummonsFile.Entry>
    {
        public SummonsFile() : base(4, 4, 2) { }

        public static SummonsFile Read(byte[] byteFile)
        {
            SummonsFile file = new SummonsFile();
            file.ReadFile(byteFile);

            return file;
        }

        // SUMMON_TABLE
        public class Entry
        {
            [Data] public ushort CommandId{ get; set; }
            [Data] public ushort ItemId{ get; set; }
            [Data] public uint Object1Id { get; set; }
            [Data] public uint Object2Id { get; set; }
            [Data] public ushort LimitCommandId { get; set; }
            [Data(Count = 50)] public byte[] Padding { get; set; }

            public Entry()
            {
                Padding = new byte[50];
            }
        }
    }
}
