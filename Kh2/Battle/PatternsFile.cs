using KhLib.Kh2.Structs;
using Xe.BinaryMapper;

namespace KhLib.Kh2.Battle
{
    public class PatternsFile : BaseTableFile<PatternsFile.Entry>
    {
        public PatternsFile() : base(4, 4, 2) { }

        public static PatternsFile Read(byte[] byteFile)
        {
            PatternsFile file = new PatternsFile();
            file.ReadFile(byteFile);

            return file;
        }

        // PATTERN
        public class Entry
        {
            [Data] public byte Id { get; set; } // Id of the bit in the pattern int of an entity, each bit corresponds to a pattern entry and can be enabled and disabled
            [Data(Count = 19)] public sbyte[] Reactions { get; set; }
            [Data(Count = 12)] public byte[] Padding { get; set; }

            public Entry()
            {
                Reactions = new sbyte[19];
                Padding = new byte[12];
            }
        }
    }
}
