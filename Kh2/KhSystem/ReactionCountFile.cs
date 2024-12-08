using KhLib.Kh2.Structs;
using Xe.BinaryMapper;

namespace KhLib.Kh2.KhSystem
{
    public class ReactionCountFile
    {
        /******************************************
         * Properties
         ******************************************/
        public int Version;
        public List<Entry> Entries = new List<Entry>();

        /******************************************
         * Constructors
         ******************************************/
        public ReactionCountFile()
        {
            Version = 1;
            Entries = new List<Entry>();
        }

        /******************************************
         * Functions - Static
         ******************************************/
        public static ReactionCountFile Read(byte[] byteFile)
        {
            ReactionCountFile file = new ReactionCountFile();

            using (MemoryStream stream = new MemoryStream(byteFile))
            {
                BaseTable<Entry> table = BaseTable<Entry>.Read(stream, 8);
                file.Version = table.Version;
                file.Entries = table.Entries;
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
                BaseTable<Entry>.Write(stream, Version, 8, Entries);
                return stream.ToArray();
            }
        }

        // struct RCCOUNT
        public class Entry
        {
            [Data] public ushort Id { get; set; }
            [Data] public ushort CommandId1 { get; set; }
            [Data] public ushort CommandId2 { get; set; }
            [Data] public ushort CommandId3 { get; set; }
            [Data] public ushort CommandId4 { get; set; }
            [Data] public ushort CommandId5 { get; set; }
        }
    }
}
