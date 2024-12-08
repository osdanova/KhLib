using Xe.BinaryMapper;

namespace KhLib.Kh2.KhSystem
{
    public class PrefMagicFile
    {
        /******************************************
         * Properties
         ******************************************/
        public List<Entry> Entries = new List<Entry>();
        public List<MagicEntry> MagicEntries = new List<MagicEntry>();

        /******************************************
         * Constructors
         ******************************************/
        public PrefMagicFile()
        {
            Entries = new List<Entry>();
            MagicEntries = new List<MagicEntry>();
        }

        /******************************************
         * Functions - Static
         ******************************************/
        public static PrefMagicFile Read(byte[] byteFile)
        {
            PrefMagicFile file = new PrefMagicFile();

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

                    file.MagicEntries = new List<MagicEntry>();
                    Dictionary<int, int> pointerMap = new Dictionary<int, int>();
                    for (int i = 0; i < pointerSet.Count; i++)
                    {
                        pointerMap.Add(pointerSet[i], i);

                        stream.Position = pointerSet[i];
                        MagicEntry preference = BinaryMapping.ReadObject<MagicEntry>(stream);
                        preference.Id = i;
                        file.MagicEntries.Add(preference);
                    }

                    file.Entries = new List<Entry>();
                    for (int i = 0; i < pointerList.Count; i++)
                    {
                        Entry entry = new Entry();
                        entry.Id = i;
                        entry.MagicEntryId = (pointerList[i] == 0) ? -1 : pointerMap[pointerList[i]];
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
                    for (int i = 0; i < MagicEntries.Count; i++)
                    {
                        offsets.Add(i, (int)stream.Position);
                        BinaryMapping.WriteObject<MagicEntry>(stream, MagicEntries[i]);
                    }

                    stream.Position = 0;
                    writer.Write((int)Entries.Count);
                    foreach (Entry entry in Entries)
                    {
                        if (entry.MagicEntryId == -1)
                        {
                            writer.Write((int)0);
                        }
                        else
                        {
                            writer.Write(offsets[entry.MagicEntryId]);
                        }
                    }
                }

                return stream.ToArray();
            }
        }

        public class Entry
        {
            public int Id { get; set; }
            public int MagicEntryId { get; set; }
        }

        // PREF_MAGIC
        public class MagicEntry
        {
            public int Id { get; set; }
            [Data] public float FireRadius { get; set; }
            [Data] public float FireHeight { get; set; }
            [Data] public float FireTime { get; set; }
            [Data] public float BlizzardFadeTime { get; set; }
            [Data] public float BlizzardTime { get; set; }
            [Data] public float BlizzardSpeed { get; set; }
            [Data] public float BlizzardRadius { get; set; }
            [Data] public float BlizzardHitback { get; set; }
            [Data] public float ThunderNoTargetDistance { get; set; }
            [Data] public float ThunderBorderHeight { get; set; }
            [Data] public float ThunderNoTargetHeight { get; set; }
            [Data] public float ThunderCheckHeight { get; set; }
            [Data] public float ThunderBurstRadius { get; set; }
            [Data] public float ThunderHeight { get; set; }
            [Data] public float ThunderRadius { get; set; }
            [Data] public float ThunderAttackWait { get; set; }
            [Data] public float ThunderTime { get; set; }
            [Data] public float CureRange { get; set; }
            [Data] public float MagnetMinYOffset { get; set; }
            [Data] public float MagnetLargeTime { get; set; }
            [Data] public float MagnetStayTime { get; set; }
            [Data] public float MagnetSmallTime { get; set; }
            [Data] public float MagnetRadius { get; set; }
            [Data] public float ReflectRadius { get; set; }
            [Data] public float ReflectLaserTime { get; set; }
            //[Data] public float ReflectFinishTime { get; set; }
            //[Data] public float ReflectLv1Radius { get; set; }
            //[Data] public float ReflectLv1Height { get; set; }
            [Data] public int ReflectLv2Count { get; set; }
            [Data] public float ReflectLv2Radius { get; set; }
            [Data] public float ReflectLv2Height { get; set; }
            [Data] public int ReflectLv3Count { get; set; }
            [Data] public float ReflectLv3Radius { get; set; }
            [Data] public float ReflectLv3Height { get; set; }
        }
    }
}
