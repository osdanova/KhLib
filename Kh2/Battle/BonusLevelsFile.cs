using KhLib.Kh2.Structs;
using Xe.BinaryMapper;

namespace KhLib.Kh2.Battle
{
    public class BonusLevelsFile : BaseTableFile<BonusLevelsFile.Entry>
    {
        public BonusLevelsFile() : base(4, 4, 2) { }

        public static BonusLevelsFile Read(byte[] byteFile)
        {
            BonusLevelsFile file = new BonusLevelsFile();
            file.ReadFile(byteFile);

            return file;
        }

        // BONUSLEVEL
        public class Entry
        {
            [Data] public byte BonusEventId { get; set; }
            [Data] public byte CharacterId { get; set; }
            [Data] public byte Hp { get; set; }
            [Data] public byte Mp { get; set; }
            [Data] public byte Drive { get; set; }
            [Data] public byte ItemSlot { get; set; }
            [Data] public byte AccessorySlot { get; set; }
            [Data] public byte ArmorSlot { get; set; }
            [Data] public ushort Item1Id { get; set; }
            [Data] public ushort Item2Id { get; set; }
            [Data(Count = 4)] public byte[] Padding { get; set; }

            public Entry()
            {
                Padding = new byte[4];
            }
        }
    }
}
