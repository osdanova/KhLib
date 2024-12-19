using KhLib.Kh2.Dictionaries;
using KhLib.Kh2.Structs;
using Xe.BinaryMapper;

namespace KhLib.Kh2.Battle
{
    public class MagicFile : BaseTableFile<MagicFile.Entry>
    {
        public MagicFile() : base(4, 4, 1) { }

        public static MagicFile Read(byte[] byteFile)
        {
            MagicFile file = new MagicFile();
            file.ReadFile(byteFile);

            return file;
        }

        // MAGIC_TABLE
        public class Entry
        {
            [Data] public byte Id { get; set; }
            [Data] public byte Level { get; set; }
            [Data] public World_Enum WorldId { get; set; }
            [Data] public byte Padding1 { get; set; }
            [Data(Count = 32)] public string Filename { get; set; }
            [Data] public ushort ItemId { get; set; }
            [Data] public ushort CommandId { get; set; }
            [Data] public short MotionId { get; set; }
            [Data] public short Blend { get; set; }
            [Data] public short MotionFinishId { get; set; }
            [Data] public short BlendFinish { get; set; }
            [Data] public short MotionAirId { get; set; }
            [Data] public short BlendAir { get; set; }
            [Data] public sbyte VoiceId { get; set; }
            [Data] public sbyte VoiceFinishId { get; set; }
            [Data] public sbyte VoiceSelfId { get; set; }
            [Data] public byte Padding2 { get; set; }
        }
    }
}
