using Xe.BinaryMapper;

namespace KhLib.Kh2
{
    /******************************************
     * Helper classes for reading and writing
     ******************************************/
    public class BinaryArchive_BIN
    {
        // struct BINARC
        public class BIN_Header
        {
            public static int SIZE = 0x10;
            [Data(Count = 3)] public string Signature { get; set; } // BAR
            [Data] public byte ExternalFlagsAndVersion { get; set; } // 4b external flags; 4b Version
            [Data] public int EntryCount { get; set; }
            [Data] public int Address { get; set; } // Always 0 unless ingame
            [Data] public int MotionSetType { get; set; } // 30b Replace; 2b Flag (MotionSet Type)
        }
        // struct INFO
        public class BIN_Entry
        {
            public static int SIZE = 0x10;
            [Data] public short Type { get; set; }
            [Data] public short DuplicateFlag { get; set; } // Bitflag
            [Data(Count = 4)] public string Name { get; set; }
            [Data] public int Offset { get; set; }
            [Data] public int Size { get; set; }

            // For visualizing easier in Debug Locals
            public override string ToString()
            {
                return "[" + Name + "] Type: " + Type + "; DupFlag: " + DuplicateFlag + "; Offset: " + Offset + "; Size: " + Size;
            }
        }
    }
}
