using KhLib.Kh2.Structs;
using Xe.BinaryMapper;

namespace KhLib.Kh2.Battle
{
    public class VoiceTableFile : BaseTableFile<VoiceTableFile.Entry>
    {
        public VoiceTableFile() : base(4, 4, 2) { }

        public static VoiceTableFile Read(byte[] byteFile)
        {
            VoiceTableFile file = new VoiceTableFile();
            file.ReadFile(byteFile);

            return file;
        }

        // ???
        public class Entry
        {
            [Data] public byte CharacterId { get; set; }
            [Data] public byte Action { get; set; }
            [Data] public byte Priority { get; set; }
            [Data] public byte Reserve { get; set; }
            [Data] public sbyte Voice1Id { get; set; }
            [Data] public byte Voice1Chance { get; set; }
            [Data] public sbyte Voice2Id { get; set; }
            [Data] public byte Voice2Chance { get; set; }
            [Data] public sbyte Voice3Id { get; set; }
            [Data] public byte Voice3Chance { get; set; }
            [Data] public sbyte Voice4Id { get; set; }
            [Data] public byte Voice4Chance { get; set; }
            [Data] public sbyte Voice5Id { get; set; }
            [Data] public byte Voice5Chance { get; set; }
        }
    }
}
