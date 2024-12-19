using KhLib.Kh2.Utils;
using Xe.BinaryMapper;
using static KhLib.Kh2.Battle.AttackParamsFile;

namespace KhLib.Kh2.Battle
{
    public class PartyAttacksFile
    {
        /******************************************
         * Properties
         ******************************************/
        private int Version {  get; set; }
        public List<Entry> Entries = new List<Entry>();
        public List<AttackSet> AttackSets = new List<AttackSet>();

        /******************************************
         * Constructors
         ******************************************/
        public PartyAttacksFile()
        {
            Version = 2;
            Entries = new List<Entry>();
            AttackSets = new List<AttackSet>();
        }

        /******************************************
         * Functions - Static
         ******************************************/
        public static PartyAttacksFile Read(byte[] byteFile)
        {
            PartyAttacksFile file = new PartyAttacksFile();

            using (MemoryStream stream = new MemoryStream(byteFile))
            {
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

                    file.AttackSets = new List<AttackSet>();
                    Dictionary<int, int> pointerMap = new Dictionary<int, int>();
                    for (int i = 0; i < pointerSet.Count; i++)
                    {
                        pointerMap.Add(pointerSet[i], i);
                        stream.Position = pointerSet[i];

                        AttackSet set = new AttackSet();
                        set.Id = i;

                        int entryCount = br.ReadInt32();
                        for(int j = 0; j < entryCount; j++)
                        {
                            set.AttackEntries.Add(BinaryMapping.ReadObject<AttackEntry>(stream));
                        }

                        file.AttackSets.Add(set);
                    }

                    file.Entries = new List<Entry>();
                    for (int i = 0; i < pointerList.Count; i++)
                    {
                        Entry entry = new Entry();
                        entry.Id = i;
                        entry.AttackSetId = (pointerList[i] == 0) ? -1 : pointerMap[pointerList[i]];
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
                    stream.Position = 8 + (Entries.Count * 4);

                    Dictionary<int, int> offsets = new Dictionary<int, int>();
                    for (int i = 0; i < AttackSets.Count; i++)
                    {
                        offsets.Add(i, (int)stream.Position);
                        writer.Write((int)AttackSets[i].AttackEntries.Count);
                        foreach(AttackEntry entry in AttackSets[i].AttackEntries)
                        {
                            BinaryMapping.WriteObject<AttackEntry>(stream, entry);
                        }
                    }

                    stream.Position = 0;
                    writer.Write((int)Version);
                    writer.Write((int)Entries.Count);
                    foreach (Entry entry in Entries)
                    {
                        if (entry.AttackSetId == -1)
                        {
                            writer.Write((int)0);
                        }
                        else
                        {
                            writer.Write(offsets[entry.AttackSetId]);
                        }
                    }
                }

                return stream.ToArray();
            }
        }

        public class Entry
        {
            public int Id { get; set; }
            public int AttackSetId { get; set; }
        }

        // PARTYATTACK_TABLE
        public class AttackSet
        {
            public int Id { get; set; }
            public List<AttackEntry> AttackEntries { get; set; }

            public AttackSet()
            {
                AttackEntries = new List<AttackEntry>();
            }
        }

        // PARTYATTACK
        public class AttackEntry
        {
            [Data] public byte Id { get; set; }
            [Data] public byte Type { get; set; }
            [Data] public sbyte Sub { get; set; }
            [Data] public byte ComboOffset { get; set; }
            [Data] public PtyaFlags Flags { get; set; }
            [Data] public ushort Motion { get; set; }
            [Data] public ushort NextMotion { get; set; }
            [Data] public float Jump { get; set; }
            [Data] public float JumpMax { get; set; }
            [Data] public float JumpMin { get; set; }
            [Data] public float SpeedMin { get; set; }
            [Data] public float SpeedMax { get; set; }
            [Data] public float Near { get; set; }
            [Data] public float Far { get; set; }
            [Data] public float Low { get; set; }
            [Data] public float High { get; set; }
            [Data] public float InnerMin { get; set; }
            [Data] public float InnerMax { get; set; }
            [Data] public float BlendTime { get; set; }
            [Data] public float DistAdjust { get; set; }
            [Data] public ushort AbilityId { get; set; }
            [Data] public ushort Score { get; set; }

            public bool FlagAerial
            {
                get => BitFlag.IsFlagSet(Flags, PtyaFlags.Aerial);
                set => Flags = BitFlag.SetFlag(Flags, PtyaFlags.Aerial, value);
            }
            public bool FlagGroundToAir
            {
                get => BitFlag.IsFlagSet(Flags, PtyaFlags.GroundToAir);
                set => Flags = BitFlag.SetFlag(Flags, PtyaFlags.GroundToAir, value);
            }
            public bool FlagFinisher
            {
                get => BitFlag.IsFlagSet(Flags, PtyaFlags.Finisher);
                set => Flags = BitFlag.SetFlag(Flags, PtyaFlags.Finisher, value);
            }
            public bool FlagUnk
            {
                get => BitFlag.IsFlagSet(Flags, PtyaFlags.Unk);
                set => Flags = BitFlag.SetFlag(Flags, PtyaFlags.Unk, value);
            }

            [Flags]
            public enum PtyaFlags : uint
            {
                Aerial = 0x01,
                GroundToAir = 0x02,
                Finisher = 0x04,
                Unk = 0x08
            }
        }
    }
}
