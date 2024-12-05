namespace KhLib.Kh2.System
{
    public class WeaponEntriesFile
    {
        public List<int> WeaponActorSetPointers = new List<int>(); // 70 entries
        public List<Set> Sets = new List<Set>();

        /******************************************
         * Constructors
         ******************************************/
        public WeaponEntriesFile()
        {
            WeaponActorSetPointers = new List<int>();
            Sets = new List<Set>();
        }

        /******************************************
         * Functions - Static
         ******************************************/
        public static WeaponEntriesFile Read(byte[] byteFile)
        {
            WeaponEntriesFile file = new WeaponEntriesFile();

            using (MemoryStream stream = new MemoryStream(byteFile))
            {
                using (BinaryReader br = new BinaryReader(stream))
                {
                    // Pointers
                    List<int> tmpPointers = new List<int>();
                    file.WeaponActorSetPointers = new List<int>();
                    for (int i = 0; i < 70; i++)
                    {
                        int pointer = br.ReadInt32();
                        if (pointer != 0 && !tmpPointers.Contains(pointer))
                        {
                            tmpPointers.Add(pointer);
                        }
                        file.WeaponActorSetPointers.Add(tmpPointers.IndexOf(pointer));
                    }

                    // Sets
                    file.Sets = new List<Set>();
                    for(int i = 0; i < tmpPointers.Count; i++)
                    {
                        Set set = new Set();
                        stream.Position = tmpPointers[i] * 4;
                        int setCount = br.ReadInt32() - 1;
                        for (int j = 0; j < setCount; j++)
                        {
                            set.Entries.Add(br.ReadInt32());
                        }
                        file.Sets.Add(set);
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
            List<int> pointers = new List<int>();
            int currentPointer = 70;
            foreach (Set set in Sets)
            {
                pointers.Add(currentPointer);
                currentPointer += (set.Entries.Count + 1);
            }

            using (MemoryStream stream = new MemoryStream())
            {
                using(BinaryWriter bw = new BinaryWriter(stream))
                {
                    foreach (int pointer in WeaponActorSetPointers)
                    {
                        if(pointer == -1)
                        {
                            bw.Write((int)0);
                        }
                        else
                        {
                            bw.Write(pointers[pointer]);
                        }
                    }

                    foreach (Set set in Sets)
                    {
                        bw.Write(set.Entries.Count + 1);
                        foreach (int value in set.Entries)
                        {
                            bw.Write(value);
                        }
                    }
                }

                return stream.ToArray();
            }
        }

        public class Set
        {
            public List<int> Entries { get; set; }

            public Set()
            {
                Entries = new List<int>();
            }
        }
    }
}
