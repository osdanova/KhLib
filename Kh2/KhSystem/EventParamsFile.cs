using KhLib.Kh2.Structs;
using Xe.BinaryMapper;

namespace KhLib.Kh2.KhSystem
{
    public class EventParamsFile
    {
        /******************************************
         * Properties
         ******************************************/
        public int Version;
        public List<Entry> Entries = new List<Entry>();

        /******************************************
         * Constructors
         ******************************************/
        public EventParamsFile()
        {
            Version = 1;
            Entries = new List<Entry>();
        }

        /******************************************
         * Functions - Static
         ******************************************/
        public static EventParamsFile Read(byte[] byteFile)
        {
            EventParamsFile file = new EventParamsFile();

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

        // ???
        public class Entry
        {
            [Data] public byte Id { get; set; }
            [Data] public byte UnkBitflag { get; set; }
            [Data] public int Unk1 { get; set; }
            [Data] public short Unk2 { get; set; }
        }
    }
}
