using KhLib.Kh2.Structs;
using Xe.BinaryMapper;

namespace KhLib.Kh2.Battle
{
    public class PlayerParamsFile : BaseTableFile<PlayerParamsFile.Entry>
    {
        public PlayerParamsFile() : base(4, 4, 1) { }

        public static PlayerParamsFile Read(byte[] byteFile)
        {
            PlayerParamsFile file = new PlayerParamsFile();
            file.ReadFile(byteFile);

            return file;
        }

        // ???
        public class Entry
        {
            [Data] public ushort Version { get; set; }
            [Data] public byte CharacterId { get; set; }
            [Data] public byte Hp { get; set; }
            [Data] public byte Mp { get; set; }
            [Data] public byte Ap { get; set; }
            [Data] public byte Strength { get; set; }
            [Data] public byte Magic { get; set; }
            [Data] public byte Defense { get; set; }
            [Data] public byte ArmorSlots { get; set; }
            [Data] public byte AccessorySlots { get; set; }
            [Data] public byte ItemSlots { get; set; }
            [Data(Count = 50)] public ushort[] Inventory { get; set; }
            [Data(Count = 8)] public ushort[] Padding { get; set; } // I have no idea what's the max items

            public Entry()
            {
                Inventory = new ushort[50];
                Padding = new ushort[8];
            }
        }
    }
}
