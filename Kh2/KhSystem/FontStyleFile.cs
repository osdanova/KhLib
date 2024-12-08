using KhLib.Kh2.Structs;
using Xe.BinaryMapper;

namespace KhLib.Kh2.KhSystem
{
    public class FontStyleFile
    {
        /******************************************
         * Properties
         ******************************************/
        public int Version;
        public List<Entry> Entries = new List<Entry>();

        /******************************************
         * Constructors
         ******************************************/
        public FontStyleFile()
        {
            Version = 1;
            Entries = new List<Entry>();
        }

        /******************************************
         * Functions - Static
         ******************************************/
        public static FontStyleFile Read(byte[] byteFile)
        {
            FontStyleFile file = new FontStyleFile();

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

        // struct FONTSTYLE_LIST
        public class Entry
        {
            [Data] public int Id { get; set; }
            [Data(Count=19)] public uint[] Colors { get; set; }
        }
    }
}
