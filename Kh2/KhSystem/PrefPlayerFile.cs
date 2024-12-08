using Xe.BinaryMapper;

namespace KhLib.Kh2.KhSystem
{
    public class PrefPlayerFile
    {
        /******************************************
         * Properties
         ******************************************/
        public List<Entry> Entries = new List<Entry>();
        public List<PlayerPreference> Preferences = new List<PlayerPreference>();

        /******************************************
         * Constructors
         ******************************************/
        public PrefPlayerFile()
        {
            Entries = new List<Entry>();
            Preferences = new List<PlayerPreference>();
        }

        /******************************************
         * Functions - Static
         ******************************************/
        public static PrefPlayerFile Read(byte[] byteFile)
        {
            PrefPlayerFile file = new PrefPlayerFile();

            using (MemoryStream stream = new MemoryStream(byteFile))
            {
                using(BinaryReader br = new BinaryReader(stream))
                {
                    int count = br.ReadInt32();

                    List<int> pointerList = new List<int>();
                    List<int> pointerSet = new List<int>();
                    for(int i = 0; i < count; i++)
                    {
                        int pointer = br.ReadInt32();
                        pointerList.Add(pointer);
                        if(pointer != 0 && !pointerSet.Contains(pointer)) {
                            pointerSet.Add(pointer);
                        }
                    }
                    pointerSet.Sort();

                    file.Preferences = new List<PlayerPreference>();
                    Dictionary<int,int> pointerMap = new Dictionary<int, int>();
                    for(int i = 0; i < pointerSet.Count; i++)
                    {
                        pointerMap.Add(pointerSet[i], i);

                        stream.Position = pointerSet[i];
                        PlayerPreference preference = BinaryMapping.ReadObject<PlayerPreference>(stream);
                        preference.Id = i;
                        file.Preferences.Add(preference);
                    }

                    file.Entries = new List<Entry>();
                    for (int i = 0; i < pointerList.Count; i++)
                    {
                        Entry entry = new Entry();
                        entry.Id = i;
                        entry.PlayerPreferenceId = (pointerList[i] == 0) ? -1 : pointerMap[pointerList[i]];
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

                    Dictionary<int,int> offsets = new Dictionary<int,int>();
                    for (int i = 0; i < Preferences.Count; i++)
                    {
                        offsets.Add(i, (int)stream.Position);
                        BinaryMapping.WriteObject<PlayerPreference>(stream, Preferences[i]);
                    }

                    stream.Position = 0;
                    writer.Write((int)Entries.Count);
                    foreach(Entry entry in Entries)
                    {
                        if(entry.PlayerPreferenceId == -1)
                        {
                            writer.Write((int)0);
                        }
                        else
                        {
                            writer.Write(offsets[entry.PlayerPreferenceId]);
                        }
                    }
                }

                return stream.ToArray();
            }
        }

        public class Entry
        {
            public int Id { get; set; }
            public int PlayerPreferenceId { get; set; }
        }

        // PREF_PLAYER
        public class PlayerPreference
        {
            public int Id { get; set; }
            [Data] public float AttackYOfs { get; set; }
            [Data] public float AttackRadius { get; set; }
            [Data] public float AttackMinH { get; set; }
            [Data] public float AttackMaxH { get; set; }
            [Data] public float AttackVAngle { get; set; }
            [Data] public float AttackVRange { get; set; }
            [Data] public float AttackSRange { get; set; }
            [Data] public float AttackHAngle { get; set; }
            [Data] public float AttackUMinH { get; set; }
            [Data] public float AttackUMaxH { get; set; }
            [Data] public float AttackURange { get; set; }
            [Data] public float AttackJFront { get; set; }
            [Data] public float AttackAirMinH { get; set; }
            [Data] public float AttackAirMaxH { get; set; }
            [Data] public float AttackAirBigHOfs { get; set; }
            [Data] public float AttackUV0 { get; set; }
            [Data] public float AttackJV0 { get; set; }
            [Data] public float AttackFirstV0 { get; set; }
            [Data] public float AttackComboV0 { get; set; }
            [Data] public float AttackFinishV0 { get; set; }
            [Data] public float BlowRecovH { get; set; }
            [Data] public float BlowRecovV { get; set; }
            [Data] public float BlowRecovTime { get; set; }
            [Data] public float AutoLockOnRange { get; set; }
            [Data] public float AutoLockOnMinH { get; set; }
            [Data] public float AutoLockOnMaxH { get; set; }
            [Data] public float AutoLockOnTime { get; set; }
            [Data] public float AutoLockOnHAdjust { get; set; }
            [Data] public float AutoLockOnInnerAdjust { get; set; }
        }
    }
}
