using KhLib.Kh2.Structs;
using KhLib.Kh2.Utils;
using Xe.BinaryMapper;

namespace KhLib.Kh2.Battle
{
    public class StopFile : BaseTableFile<StopFile.Entry>
    {
        public StopFile() : base(4, 4, 1) { }

        public static StopFile Read(byte[] byteFile)
        {
            StopFile file = new StopFile();
            file.ReadFile(byteFile);

            return file;
        }

        // STOP_TABLE
        public class Entry
        {
            [Data] public ushort Id { get; set; }
            [Data] public StopFlags Flags { get; set; }

            public bool FlagStop
            {
                get => BitFlag.IsFlagSet(Flags, StopFlags.Stop);
                set => Flags = BitFlag.SetFlag(Flags, StopFlags.Stop, value);
            }
            public bool FlagDamageReaction
            {
                get => BitFlag.IsFlagSet(Flags, StopFlags.DamageReaction);
                set => Flags = BitFlag.SetFlag(Flags, StopFlags.DamageReaction, value);
            }
            public bool FlagStar
            {
                get => BitFlag.IsFlagSet(Flags, StopFlags.Star);
                set => Flags = BitFlag.SetFlag(Flags, StopFlags.Star, value);
            }

            [Flags]
            public enum StopFlags : ushort
            {
                Stop = 0x01,
                DamageReaction = 0x02,
                Star = 0x04
            }
        }
    }
}
