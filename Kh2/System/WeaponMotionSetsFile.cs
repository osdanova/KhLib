using Xe.BinaryMapper;

namespace KhLib.Kh2.System
{
    public class WeaponMotionSetsFile
    {
        public List<Entry> Entries; // 70 entries

        /******************************************
         * Constructors
         ******************************************/
        public WeaponMotionSetsFile()
        {
            Entries = new List<Entry>();
        }

        /******************************************
         * Functions - Static
         ******************************************/
        public static WeaponMotionSetsFile Read(byte[] byteFile)
        {
            WeaponMotionSetsFile file = new WeaponMotionSetsFile();

            using (MemoryStream stream = new MemoryStream(byteFile))
            {
                for(int i = 0; i < 70; i++)
                {
                    file.Entries.Add(BinaryMapping.ReadObject<Entry>(stream));
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
                foreach (Entry entry in Entries)
                {
                    BinaryMapping.WriteObject(stream, entry);
                }

                return stream.ToArray();
            }
        }

        public class Entry
        {
            [Data(Count = 32)] public string RightWeapon { get; set; }
            [Data(Count=32)] public string LeftWeapon { get; set; }
        }
    }
}
