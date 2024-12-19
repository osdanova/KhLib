using KhLib.Kh2.Structs;
using Xe.BinaryMapper;

namespace KhLib.Kh2.Battle
{
    public class PrizeTableFile : BaseTableFile<PrizeTableFile.Entry>
    {
        public PrizeTableFile() : base(4, 4, 2) { }

        public static PrizeTableFile Read(byte[] byteFile)
        {
            PrizeTableFile file = new PrizeTableFile();
            file.ReadFile(byteFile);

            return file;
        }

        // PRIZETABLE
        public class Entry
        {
            [Data] public ushort Id { get; set; }
            [Data] public byte HpS { get; set; }
            [Data] public byte HpL { get; set; }
            [Data] public byte MunnyL { get; set; }
            [Data] public byte MunnyM { get; set; }
            [Data] public byte MunnyS { get; set; }
            [Data] public byte MpS { get; set; }
            [Data] public byte MpL { get; set; }
            [Data] public byte DriveS { get; set; }
            [Data] public byte DriveL { get; set; }
            [Data] public byte Padding { get; set; }
            [Data] public ushort Drop1Id { get; set; }
            [Data] public ushort Drop1Chance { get; set; }
            [Data] public ushort Drop2Id { get; set; }
            [Data] public ushort Drop2Chance { get; set; }
            [Data] public ushort Drop3Id { get; set; }
            [Data] public ushort Drop3Chance { get; set; }
        }
    }
}
