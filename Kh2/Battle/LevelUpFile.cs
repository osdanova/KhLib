using Xe.BinaryMapper;

namespace KhLib.Kh2.Battle
{
    public class LevelUpFile
    {
        /******************************************
         * Properties
         ******************************************/
        private int Version {  get; set; }
        public List<EntryPointer> Pointers = new List<EntryPointer>();
        public List<EntrySet> EntrySets = new List<EntrySet>();

        /******************************************
         * Constructors
         ******************************************/
        public LevelUpFile()
        {
            Pointers = new List<EntryPointer>();
            EntrySets = new List<EntrySet>();
        }

        /******************************************
         * Functions - Static
         ******************************************/
        public static LevelUpFile Read(byte[] byteFile)
        {
            LevelUpFile file = new LevelUpFile();

            using (MemoryStream stream = new MemoryStream(byteFile))
            using (BinaryReader br = new BinaryReader(stream))
            {
                file.Version = br.ReadInt32();
                int count = br.ReadInt32();

                List<int> pointerList = new List<int>();
                List<int> pointerSet = new List<int>();
                for (int i = 0; i < count; i++)
                {
                    int pointer = br.ReadInt32();
                    pointerList.Add(pointer);
                    if (pointer != 0 && !pointerSet.Contains(pointer))
                    {
                        pointerSet.Add(pointer);
                    }
                }
                pointerSet.Sort();

                file.EntrySets = new List<EntrySet>();
                Dictionary<int, int> pointerMap = new Dictionary<int, int>();
                for (int i = 0; i < pointerSet.Count; i++)
                {
                    pointerMap.Add(pointerSet[i], i);
                    stream.Position = pointerSet[i] * 4;

                    EntrySet set = new EntrySet();
                    set.Id = i;

                    int entryCount = br.ReadInt32();
                    for (int j = 0; j < entryCount; j++)
                    {
                        Entry entry = BinaryMapping.ReadObject<Entry>(stream);
                        entry.Level = j + 1;
                        set.Entries.Add(entry);
                    }

                    file.EntrySets.Add(set);
                }

                file.Pointers = new List<EntryPointer>();
                for (int i = 0; i < pointerList.Count; i++)
                {
                    EntryPointer pointer = new EntryPointer();
                    pointer.Id = i;
                    pointer.EntrySetId = (pointerList[i] == 0) ? -1 : pointerMap[pointerList[i]];
                    file.Pointers.Add(pointer);
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
                stream.Position = 8 + (Pointers.Count * 4);

                Dictionary<int, int> offsets = new Dictionary<int, int>();
                for (int i = 0; i < EntrySets.Count; i++)
                {
                    offsets.Add(i, (int)stream.Position);
                    writer.Write((int)EntrySets[i].Entries.Count);
                    foreach (Entry entry in EntrySets[i].Entries)
                    {
                        BinaryMapping.WriteObject<Entry>(stream, entry);
                    }
                }

                stream.Position = 0;
                writer.Write((int)Version);
                writer.Write((int)Pointers.Count);
                foreach (EntryPointer pointer in Pointers)
                {
                    if (pointer.EntrySetId == -1)
                    {
                        writer.Write((int)0);
                    }
                    else
                    {
                        writer.Write(offsets[pointer.EntrySetId] / 4);
                    }
                }
                return stream.ToArray();
            }
        }

        public class EntryPointer
        {
            public int Id { get; set; }
            public int EntrySetId { get; set; }
        }

        public class EntrySet
        {
            public int Id { get; set; }
            public List<Entry> Entries { get; set; }

            public EntrySet()
            {
                Entries = new List<Entry>();
            }
        }

        // LEVELUP
        public class Entry
        {
            public int Level { get; set; }
            [Data] public int Exp { get; set; }
            [Data] public byte Strength { get; set; }
            [Data] public byte Magic { get; set; }
            [Data] public byte Defense { get; set; }
            [Data] public byte Ap { get; set; }
            [Data] public ushort AbilitySwordId { get; set; }
            [Data] public ushort AbilityShieldId { get; set; }
            [Data] public ushort AbilityStaffId { get; set; }
            [Data(Count = 2)] public byte[] Padding { get; set; }

            public Entry()
            {
                Padding = new byte[4];
            }
    }
    }
}
