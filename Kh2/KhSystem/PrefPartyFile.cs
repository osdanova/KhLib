using Xe.BinaryMapper;

namespace KhLib.Kh2.KhSystem
{
    public class PrefPartyFile
    {
        /******************************************
         * Properties
         ******************************************/
        public List<Entry> Entries = new List<Entry>();
        public List<PartyEntry> PartyEntries = new List<PartyEntry>();

        /******************************************
         * Constructors
         ******************************************/
        public PrefPartyFile()
        {
            Entries = new List<Entry>();
            PartyEntries = new List<PartyEntry>();
        }

        /******************************************
         * Functions - Static
         ******************************************/
        public static PrefPartyFile Read(byte[] byteFile)
        {
            PrefPartyFile file = new PrefPartyFile();

            using (MemoryStream stream = new MemoryStream(byteFile))
            {
                using (BinaryReader br = new BinaryReader(stream))
                {
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

                    file.PartyEntries = new List<PartyEntry>();
                    Dictionary<int, int> pointerMap = new Dictionary<int, int>();
                    for (int i = 0; i < pointerSet.Count; i++)
                    {
                        pointerMap.Add(pointerSet[i], i);

                        stream.Position = pointerSet[i];
                        PartyEntry preference = BinaryMapping.ReadObject<PartyEntry>(stream);
                        preference.Id = i;
                        file.PartyEntries.Add(preference);
                    }

                    file.Entries = new List<Entry>();
                    for (int i = 0; i < pointerList.Count; i++)
                    {
                        Entry entry = new Entry();
                        entry.Id = i;
                        entry.PartyEntryId = (pointerList[i] == 0) ? -1 : pointerMap[pointerList[i]];
                        file.Entries.Add(entry);
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
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    stream.Position = 4 + (Entries.Count * 4);

                    Dictionary<int, int> offsets = new Dictionary<int, int>();
                    for (int i = 0; i < PartyEntries.Count; i++)
                    {
                        offsets.Add(i, (int)stream.Position);
                        BinaryMapping.WriteObject<PartyEntry>(stream, PartyEntries[i]);
                    }

                    stream.Position = 0;
                    writer.Write((int)Entries.Count);
                    foreach (Entry entry in Entries)
                    {
                        if (entry.PartyEntryId == -1)
                        {
                            writer.Write((int)0);
                        }
                        else
                        {
                            writer.Write(offsets[entry.PartyEntryId]);
                        }
                    }
                }

                return stream.ToArray();
            }
        }

        public class Entry
        {
            public int Id { get; set; }
            public int PartyEntryId { get; set; }
        }

        // PREF_PARTY
        public class PartyEntry
        {
            public int Id { get; set; }
            [Data] public float WalkSpeed { get; set; }
            [Data] public float RunSpeed { get; set; }
            [Data] public float JumpHeight { get; set; }
            [Data] public float TurnSpeed { get; set; }
            [Data] public float HangHeight { get; set; }
            [Data] public float HangMargin { get; set; }
            [Data] public float StunTime { get; set; }
            [Data] public float MpDrive { get; set; }
            [Data] public float UpDownSpeed { get; set; }
            [Data] public float DashSpeed { get; set; }
            [Data] public float Accel { get; set; }
            [Data] public float Brake { get; set; }
            [Data] public float SubjectiveOffset { get; set; }
        }
    }
}
