using KhLib.Kh2.Structs;
using KhLib.Kh2.Utils;
using Xe.BinaryMapper;

namespace KhLib.Kh2.KhSystem
{
    public class AreaInfoFile
    {
        /******************************************
         * Properties
         ******************************************/
        public int Version;
        public List<List<AreaInfo>> WorldInfos = new List<List<AreaInfo>>();

        /******************************************
         * Constructors
         ******************************************/
        public AreaInfoFile()
        {
            Version = 1;
            WorldInfos = new List<List<AreaInfo>>();
        }

        /******************************************
         * Functions - Static
         ******************************************/
        public static AreaInfoFile Read(byte[] byteFile)
        {
            AreaInfoFile file = new AreaInfoFile();

            using (MemoryStream stream = new MemoryStream(byteFile))
            {
                BaseTable<WorldHeader> worldHeaders = BaseTable<WorldHeader>.Read(stream, 8);

                foreach(WorldHeader worldHeader in worldHeaders.Entries)
                {
                    List<AreaInfo> areaInfos = new List<AreaInfo>();
                    stream.Position = worldHeader.Offset;
                    for (int i = 0; i < worldHeader.EntryCount; i++)
                    {
                        areaInfos.Add(BinaryMapping.ReadObject<AreaInfo>(stream));
                    }
                    file.WorldInfos.Add(areaInfos);
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
                    writer.Write(Version);
                    writer.Write(WorldInfos.Count);

                    stream.Position += (WorldInfos.Count * 4);

                    List<WorldHeader> headers = new List<WorldHeader>();
                    foreach (List<AreaInfo> worldInfo in WorldInfos)
                    {
                        headers.Add(new WorldHeader { EntryCount = (ushort)worldInfo.Count, Offset = (ushort)stream.Position });
                        foreach (AreaInfo arif in worldInfo)
                        {
                            BinaryMapping.WriteObject<AreaInfo>(stream, arif);
                        }
                    }

                    stream.Position = 8;
                    foreach(WorldHeader header in headers)
                    {
                        BinaryMapping.WriteObject<WorldHeader>(stream, header);
                    }

                    return stream.ToArray();
                }
            }
        }

        public class WorldHeader
        {
            [Data] public ushort EntryCount { get; set; }
            [Data] public ushort Offset { get; set; }
        }

        // AREAINFO
        public class AreaInfo
        {
            [Data] public ArifFlags Flags { get; set; }
            [Data] public int Reverb { get; set; }
            [Data] public int BackgroundSoundEffect1 { get; set; }
            [Data] public int BackgroundSoundEffect2 { get; set; }
            [Data(Count = 8)] public BgmSet[] BackgroundMusic { get; set; }
            [Data] public ushort Voice { get; set; }
            [Data] public ushort NavigationMapItem { get; set; }
            [Data] public byte Command { get; set; }
            [Data(Count = 11)] public byte[] Reserved { get; set; }

            public bool IsKnownArea
            {
                get => BitFlag.IsFlagSet(Flags, ArifFlags.IsKnownArea);
                set => Flags = BitFlag.SetFlag(Flags, ArifFlags.IsKnownArea, value);
            }
            public bool IndoorArea
            {
                get => BitFlag.IsFlagSet(Flags, ArifFlags.IndoorArea);
                set => Flags = BitFlag.SetFlag(Flags, ArifFlags.IndoorArea, value);
            }
            public bool Monochrome
            {
                get => BitFlag.IsFlagSet(Flags, ArifFlags.Monochrome);
                set => Flags = BitFlag.SetFlag(Flags, ArifFlags.Monochrome, value);
            }
            public bool NoShadow
            {
                get => BitFlag.IsFlagSet(Flags, ArifFlags.NoShadow);
                set => Flags = BitFlag.SetFlag(Flags, ArifFlags.NoShadow, value);
            }
            public bool HasGlow
            {
                get => BitFlag.IsFlagSet(Flags, ArifFlags.HasGlow);
                set => Flags = BitFlag.SetFlag(Flags, ArifFlags.HasGlow, value);
            }

            public AreaInfo()
            {
                BackgroundMusic = new BgmSet[8];
                Reserved = new byte[11];
            }
        }

        public class BgmSet
        {
            [Data] public ushort BgmField { get; set; }
            [Data] public ushort BgmBattle { get; set; }
        }

        [Flags]
        public enum ArifFlags : uint
        {
            IsKnownArea = 0x01,
            IndoorArea = 0x02,
            Monochrome = 0x04,
            NoShadow = 0x08,
            HasGlow = 0x10
        }
    }
}
