using System.Text;
using Xe.BinaryMapper;
using static KhLib.Kh2.BinaryArchive_BIN;

namespace KhLib.Kh2
{
    public class BinaryArchive
    {
        /******************************************
         * Constants
         ******************************************/
        private const string SIGNATURE = "BAR";

        /******************************************
         * Properties
         ******************************************/
        public byte ExternalFlags { get; set; } // 4b
        public byte Version { get; set; } // 4b
        public MotionsetType MSetType { get; set; }
        public List<Entry> Entries { get; set; }

        /******************************************
         * Constructors
         ******************************************/
        public BinaryArchive()
        {
            Entries = new List<Entry>();
        }

        /******************************************
         * Functions - Static
         ******************************************/
        public static BinaryArchive Read(byte[] byteFile)
        {
            BinaryArchive binaryArchive = new BinaryArchive();

            using (MemoryStream stream = new MemoryStream(byteFile))
            {
                // Read Header
                BIN_Header binHeader = BinaryMapping.ReadObject<BIN_Header>(stream);
                binaryArchive.ExternalFlags = (byte)((binHeader.ExternalFlagsAndVersion & 0xF0) >> 4);
                binaryArchive.Version = (byte)(binHeader.ExternalFlagsAndVersion & 0x0F);
                binaryArchive.MSetType = (MotionsetType)binHeader.MotionSetType;

                // Read Entries
                List<BIN_Entry> binEntries = new List<BIN_Entry>();
                for (int i = 0; i < binHeader.EntryCount; i++)
                {
                    binEntries.Add(BinaryMapping.ReadObject<BIN_Entry>(stream));
                }
                foreach(BIN_Entry binEntry in binEntries)
                {
                    Entry entry = new Entry();
                    entry.Type = (EntryType)binEntry.Type;
                    entry.Name = binEntry.Name;
                    if (binEntry.Size > 0)
                    {
                        stream.Position = binEntry.Offset;
                        byte[] fileBytes = new byte[binEntry.Size];
                        stream.Read(fileBytes, 0, binEntry.Size);
                        entry.File = fileBytes;
                    }
                    binaryArchive.Entries.Add(entry);
                }
            }

            return binaryArchive;
        }
        
        public static bool IsValid(byte[] byteFile)
        {
            if (byteFile.Length < 4)
            {
                return false;
            }

            byte[] signatureBytes = new byte[3];
            Array.Copy(byteFile, signatureBytes, 3);

            return (Encoding.ASCII.GetString(signatureBytes) == SIGNATURE) ? true : false;
        }

        /******************************************
         * Functions - Local
         ******************************************/

        // Returns the BinaryArchive as a byte array
        public byte[] GetAsByteArray()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                // Header
                BIN_Header binHeader = new BIN_Header();
                binHeader.Signature = SIGNATURE;
                binHeader.ExternalFlagsAndVersion = (byte)((ExternalFlags << 4) | (Version & 0x0F));
                binHeader.EntryCount = Entries.Count;
                binHeader.Address = 0;
                binHeader.MotionSetType = (int)MSetType;
                BinaryMapping.WriteObject(stream, binHeader);

                // Files
                List<BIN_Entry> binEntries = new List<BIN_Entry>();
                List<byte[]> byteFiles = new List<byte[]>();
                //int firstDummyPosition = -1;

                stream.Position += Entries.Count * BIN_Entry.SIZE;
                for (int i = 0; i < Entries.Count; i++)
                {
                    Entry entry = Entries[i];

                    BIN_Entry binEntry = new BIN_Entry();
                    binEntry.Type = (short)entry.Type;
                    binEntry.DuplicateFlag = 0;
                    binEntry.Name = entry.Name;
                    binEntry.Size = entry.File.Length;

                    // Check duplicate
                    for (int j = 0; j < byteFiles.Count; j++)
                    {
                        if (entry.File.SequenceEqual(byteFiles[j]))
                        {
                            binEntry.DuplicateFlag = 1;
                            binEntry.Offset = binEntries[j].Offset;
                            break;
                        }
                    }
                    if(binEntry.DuplicateFlag == 1)
                    {
                        binEntries.Add(binEntry);
                        byteFiles.Add(entry.File);
                        continue;
                    }

                    binEntry.DuplicateFlag = 0;
                    binEntry.Offset = (int)stream.Position;

                    binEntries.Add(binEntry);
                    byteFiles.Add(entry.File);

                    // Write file
                    stream.Write(entry.File, 0, entry.File.Length);
                    CommonUtils.PadStreamToBytes(stream, 16);
                }

                // Entries
                stream.Position = BIN_Header.SIZE;
                foreach (BIN_Entry binEntry in binEntries)
                {
                    BinaryMapping.WriteObject(stream, binEntry);
                }

                return stream.ToArray();
            }
        }

        public Entry GetFirstByName(string fileName)
        {
            foreach (BinaryArchive.Entry entry in Entries)
            {
                if (entry.Name == fileName)
                {
                    return entry;
                }
            }
            return null;
        }

        /******************************************
         * Enums
         ******************************************/
        public enum EntryType : short
        {
            Dummy = 0,
            Binary = 1,
            List = 2,
            Bdx = 3,
            Model = 4,
            DrawOctalTree = 5,
            CollisionOctalTree = 6,
            ModelTexture = 7,
            Dpx = 8,
            Motion = 9,
            Tim2 = 10,
            CameraOctalTree = 11,
            AreaDataSpawn = 12,
            AreaDataScript = 13,
            FogColor = 14,
            ColorOctalTree = 15,
            MotionTriggers = 16,
            Anb = 17,
            Pax = 18,
            MapCollision2 = 19,
            Motionset = 20,
            BgObjPlacement = 21,
            Event = 22,
            ModelCollision = 23,
            Imgd = 24,
            Seqd = 25,
            Unknown26 = 26,
            Unknown27 = 27,
            Layout = 28,
            Imgz = 29,
            AnimationMap = 30,
            Seb = 31,
            Wd = 32,
            Unknown33 = 33,
            IopVoice = 34,
            Unknown35 = 35,
            RawBitmap = 36,
            MemoryCard = 37,
            WrappedCollisionData = 38,
            Unknown39 = 39,
            Unknown40 = 40,
            Unknown41 = 41,
            Minigame = 42,
            JimiData = 43,
            Progress = 44,
            Synthesis = 45,
            BarUnknown = 46,
            Vibration = 47,
            Vag = 48,
        }
        public enum MotionsetType : int
        {
            Default = 0,
            Player = 1,
            Raw = 2
        }

        /******************************************
         * Subclasses
         ******************************************/
        public class Entry
        {
            public EntryType Type { get; set; }
            public byte[] File { get; set; }
            private string _name;
            public string Name
            {
                get
                {
                    return _name;
                }
                set
                {
                    value += "\0\0\0\0";
                    _name = value.Substring(0, 4);
                }
            }

            public Entry()
            {
                Name = "\0\0\0\0";
                File = new byte[0];
                Type = EntryType.Dummy;
            }

            public int FileSize
            {
                get
                {
                    return (int)File.Length;
                }
            }

            // For visualizing easier in Debug Locals
            public override string ToString()
            {
                return "[" + Name + "] Type: " + Type + "; Size: " + File.Length;
            }
        }
    }
}
