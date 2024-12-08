using KhLib.Kh2.Dictionaries;
using KhLib.Kh2.Structs;
using Xe.BinaryMapper;

namespace KhLib.Kh2.KhSystem
{
    public class TreasuresFile
    {
        /******************************************
         * Properties
         ******************************************/
        public int Version;
        public List<Entry> Entries = new List<Entry>();

        /******************************************
         * Constructors
         ******************************************/
        public TreasuresFile()
        {
            Version = 3;
            Entries = new List<Entry>();
        }

        /******************************************
         * Functions - Static
         ******************************************/
        public static TreasuresFile Read(byte[] byteFile)
        {
            TreasuresFile file = new TreasuresFile();

            using (MemoryStream stream = new MemoryStream(byteFile))
            {
                BaseTable<Entry> table = BaseTable<Entry>.Read(stream, 4);
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
                BaseTable<Entry>.Write(stream, Version, 4, Entries);
                return stream.ToArray();
            }
        }

        // TREASURE
        public class Entry
        {
            [Data] public ushort Id { get; set; }
            [Data] public ushort ItemId { get; set; }
            [Data] public TrsrType Type { get; set; }
            [Data] public World_Enum World { get; set; }
            [Data] public byte Room { get; set; }
            [Data] public byte RoomChestIndex { get; set; }
            [Data] public short EventId { get; set; }
            [Data] public short OverallChestIndex { get; set; }
        }

        public enum TrsrType : byte
        {
            Chest,
            Event
        }
    }
}
