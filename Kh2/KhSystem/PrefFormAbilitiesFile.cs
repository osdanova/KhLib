using Xe.BinaryMapper;

namespace KhLib.Kh2.KhSystem
{
    public class PrefFormAbilitiesFile
    {
        /******************************************
         * Properties
         ******************************************/
        public List<Entry> Entries = new List<Entry>();

        /******************************************
         * Constructors
         ******************************************/
        public PrefFormAbilitiesFile()
        {
            Entries = new List<Entry>();
        }

        /******************************************
         * Functions - Static
         ******************************************/
        public static PrefFormAbilitiesFile Read(byte[] byteFile)
        {
            PrefFormAbilitiesFile file = new PrefFormAbilitiesFile();

            using (MemoryStream stream = new MemoryStream(byteFile))
            {
                using (BinaryReader br = new BinaryReader(stream))
                {
                    int count = br.ReadInt32();
                    List<int> pointers = new List<int>();
                    for (int i = 0; i < count; i++)
                    {
                        pointers.Add(br.ReadInt32());
                    }

                    foreach(int pointer in pointers)
                    {
                        stream.Position = pointer;
                        file.Entries.Add(BinaryMapping.ReadObject<Entry>(stream));
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
                using (BinaryWriter bw = new BinaryWriter(stream))
                {
                    List<int> pointers = new List<int>();
                    stream.Position = 4 + (Entries.Count * 4);
                    foreach(Entry entry in Entries)
                    {
                        pointers.Add((int)stream.Position);
                        BinaryMapping.WriteObject<Entry>(stream, entry);
                    }

                    stream.Position = 0;
                    bw.Write((int)Entries.Count);
                    foreach (int pointer in pointers)
                    {
                        bw.Write(pointer);
                    }
                }

                return stream.ToArray();
            }
        }

        // PREF_FORMABILITY
        public class Entry
        {
            [Data] public float HighJumpHeight { get; set; }
            [Data] public float AirDodgeHeight { get; set; }
            [Data] public float AirDodgeSpeed { get; set; }
            [Data] public float AirSlideTime { get; set; }
            [Data] public float AirSlideSpeed { get; set; }
            [Data] public float AirSlideBrake { get; set; }
            [Data] public float AirSlideStopBrake { get; set; }
            [Data] public float AirSlideStarTime { get; set; }
            [Data] public float GlideSpeed { get; set; }
            [Data] public float GlideFallRatio { get; set; }
            [Data] public float GlideFallHeight { get; set; }
            [Data] public float GlideFallMax { get; set; }
            [Data] public float GlideAccel { get; set; }
            [Data] public float GlideStartHeight { get; set; }
            [Data] public float GlideEndHeight { get; set; }
            [Data] public float GlideTurnSpeed { get; set; }
            [Data] public float DodgeRollStarTime { get; set; }
        }
    }
}
