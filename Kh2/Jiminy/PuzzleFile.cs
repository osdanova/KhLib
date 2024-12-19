using KhLib.Kh2.Utils;
using Xe.BinaryMapper;
using static KhLib.Kh2.KhSystem.AreaInfoFile;

namespace KhLib.Kh2.Jiminy
{
    public class PuzzleFile
    {
        /******************************************
         * Properties
         ******************************************/
        public int Version;
        public List<Entry> Entries = new List<Entry>();

        /******************************************
         * Constructors
         ******************************************/
        public PuzzleFile()
        {
            Version = 18;
            Entries = new List<Entry>();
        }

        /******************************************
         * Functions - Static
         ******************************************/
        public static PuzzleFile Read(byte[] byteFile)
        {
            PuzzleFile file = new PuzzleFile();

            using (MemoryStream stream = new MemoryStream(byteFile))
            {
                using(BinaryReader reader = new BinaryReader(stream))
                {
                    Header header = BinaryMapping.ReadObject<Header>(stream);
                    file.Version = header.Version;
                    file.Entries = new List<Entry>();
                    for (int i = 0; i < header.EntryCount; i++)
                    {
                        file.Entries.Add(BinaryMapping.ReadObject<Entry>(stream));
                    }
                }
            }

            return file;
        }

        /******************************************
         * Functions - Local
         ******************************************/
        public byte[] GetAsByteArray()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using(BinaryWriter writer = new BinaryWriter(stream))
                {
                    Header header = new Header
                    {
                        MagicCode = "JMPZ",
                        Version = this.Version,
                        EntryCount = this.Entries.Count,
                    };

                    BinaryMapping.WriteObject<Header>(stream, header);
                    foreach (Entry entry in Entries)
                    {
                        BinaryMapping.WriteObject<Entry>(stream, entry);
                    }

                    return stream.ToArray();
                }
            }
        }
        public class Header
        {
            [Data(Count = 4)] public string MagicCode { get; set; }
            [Data] public int Version { get; set; }
            [Data] public int EntryCount { get; set; }
            [Data(Count = 4)] public byte[] Padding { get; set; }
        }

        // JmPuzzleDataHead - JmPuzzleDataInfo
        public class Entry
        {
            [Data] public byte Id { get; set; }
            [Data] public PuzzleType Type { get; set; }
            [Data] public ushort Title { get; set; }
            [Data] public ushort Item { get; set; }
            [Data(Count=10)] public string Name { get; set; }

            public bool TypeBorder
            {
                get => BitFlag.IsFlagSet(Type, PuzzleType.Border);
                set => Type = BitFlag.SetFlag(Type, PuzzleType.Border, value);
            }
            public bool TypeRotation
            {
                get => BitFlag.IsFlagSet(Type, PuzzleType.Rotation);
                set => Type = BitFlag.SetFlag(Type, PuzzleType.Rotation, value);
            }
        }

        [Flags]
        public enum PuzzleType : byte
        {
            Border = 0x01,
            Rotation = 0x10,
        }
    }
}
