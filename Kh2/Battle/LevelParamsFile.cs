using Xe.BinaryMapper;

namespace KhLib.Kh2.Battle
{
    public class LevelParamsFile
    {
        /******************************************
         * Properties
         ******************************************/
        public List<Entry> Entries = new List<Entry>();

        /******************************************
         * Constructors
         ******************************************/
        public LevelParamsFile()
        {
            Entries = new List<Entry>();
        }

        /******************************************
         * Functions - Static
         ******************************************/
        public static LevelParamsFile Read(byte[] byteFile)
        {
            LevelParamsFile file = new LevelParamsFile();

            using (MemoryStream stream = new MemoryStream(byteFile))
            using (BinaryReader reader = new BinaryReader(stream))
            {
                for (int i = 0; i < 99; i++)
                {
                    Entry entry = BinaryMapping.ReadObject<Entry>(stream);
                    entry.Level = i + 1;
                    file.Entries.Add(entry);
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
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                foreach (Entry entry in Entries)
                {
                    BinaryMapping.WriteObject<Entry>(stream, entry);
                }

                return stream.ToArray();
            }
        }

        // LEVELPARAM
        public class Entry
        {
            public int Level { get; set; }
            [Data] public ushort Hp { get; set; }
            [Data] public ushort Strength { get; set; }
            [Data] public ushort Defense { get; set; }
            [Data] public ushort AttackMax { get; set; }
            [Data] public ushort AttackMin { get; set; }
            [Data] public ushort Exp { get; set; }
        }
    }
}
