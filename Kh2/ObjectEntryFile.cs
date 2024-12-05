using KhLib.Kh2.Dictionaries;
using KhLib.Kh2.Structs;
using KhLib.Kh2.Utils;
using Xe.BinaryMapper;

namespace KhLib.Kh2
{
    public class ObjectEntryFile
    {
        /******************************************
         * Properties
         ******************************************/
        public int Version;
        public List<Entry> Entries = new List<Entry>();

        /******************************************
         * Constructors
         ******************************************/
        public ObjectEntryFile()
        {
            Version = 3;
            Entries = new List<Entry>();
        }

        /******************************************
         * Functions - Static
         ******************************************/
        public static ObjectEntryFile Read(byte[] byteFile)
        {
            ObjectEntryFile file = new ObjectEntryFile();

            using (MemoryStream stream = new MemoryStream(byteFile))
            {
                BaseTable<Entry> table = BaseTable<Entry>.Read(stream, 8);
                file.Version = table.Version;
                file.Entries = table.Entries;
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
                BaseTable<Entry>.Write(stream, Version, 8, Entries);
                return stream.ToArray();
            }
        }

        // struct OBJENTRY
        public class Entry
        {
            [Data] public uint Id { get; set; }
            [Data] public EntryType Type { get; set; }
            [Data] public byte SubType { get; set; }
            [Data] public byte DrawPriority { get; set; }
            [Data] public byte WeaponJoint { get; set; } // Skeleton
            [Data(Count = 32)] public string ModelName { get; set; } // EntryName
            [Data(Count = 32)] public string AnimationName { get; set; } // MsetFile
            [Data] public ushort Flags { get; set; }
            [Data] public TargetType ObjectTargetType { get; set; }
            [Data] public byte Padding { get; set; }
            [Data] public ushort NeoStatus { get; set; } // Part
            [Data] public ushort NeoMoveset { get; set; } // WeaponPart
            [Data] public float Weight { get; set; }
            [Data] public byte SpawnLimiter { get; set; } // Cost
            [Data] public byte Page { get; set; }
            [Data] public ShadowSize ObjectShadowSize { get; set; }
            [Data] public Form ObjectForm { get; set; }
            [Data] public ushort SpawnObject1 { get; set; }
            [Data] public ushort SpawnObject2 { get; set; }
            [Data] public ushort SpawnObject3 { get; set; }
            [Data] public ushort SpawnObject4 { get; set; }

            public bool NoApdx
            {
                get => BitUtils.Int.GetBit(Flags, 0);
                set => Flags = (ushort)BitUtils.Int.SetBit(Flags, 0, value);
            }

            public bool Before
            {
                get => BitUtils.Int.GetBit(Flags, 1);
                set => Flags = (ushort)BitUtils.Int.SetBit(Flags, 1, value);
            }

            public bool FixColor
            {
                get => BitUtils.Int.GetBit(Flags, 2);
                set => Flags = (ushort)BitUtils.Int.SetBit(Flags, 2, value);
            }

            public bool Fly
            {
                get => BitUtils.Int.GetBit(Flags, 3);
                set => Flags = (ushort)BitUtils.Int.SetBit(Flags, 3, value);
            }

            public bool Scissoring
            {
                get => BitUtils.Int.GetBit(Flags, 4);
                set => Flags = (ushort)BitUtils.Int.SetBit(Flags, 4, value);
            }

            public bool IsPirate
            {
                get => BitUtils.Int.GetBit(Flags, 5);
                set => Flags = (ushort)BitUtils.Int.SetBit(Flags, 5, value);
            }

            public bool WallOcclusion
            {
                get => BitUtils.Int.GetBit(Flags, 6);
                set => Flags = (ushort)BitUtils.Int.SetBit(Flags, 6, value);
            }

            public bool Hift
            {
                get => BitUtils.Int.GetBit(Flags, 7);
                set => Flags = (ushort)BitUtils.Int.SetBit(Flags, 7, value);
            }

            public string ObjectDescription
            {
                get => Objects_Dictionary.Instance[ModelName];
            }
        }

        /******************************************
         * Enums
         ******************************************/
        public enum EntryType : byte
        {
            PLAYER = 0,
            FRIEND = 1,
            NPC = 2,
            BOSS = 3,
            ZAKO = 4,
            WEAPON = 5,
            E_WEAPON = 6,
            SP = 7, // Save Point
            F_OBJ = 8,
            BTLNPC = 9,
            TREASURE = 10,
            SUBMENU = 11,
            L_BOSS = 12,
            G_OBJ = 13,
            MEMO = 14,
            RTN = 15,
            MINIGAME = 16,
            WORLDMAP = 17,
            PRIZEBOX = 18,
            SUMMON = 19,
            SHOP = 20,
            L_ZAKO = 21,
            MASSEFFECT = 22,
            E_OBJ = 23,
            JIGSAW = 24,
        }

        public enum TargetType : byte
        {
            M = 0,
            S = 1,
            L = 2
        }

        public enum ShadowSize : byte
        {
            NoShadow = 0,
            SmallShadow = 1,
            MiddleShadow = 2,
            LargeShadow = 3,
            SmallMovingShadow = 4,
            MiddleMovingShadow = 5,
            LargeMovingShadow = 6
        }

        public enum Form : byte
        {
            SoraRoxasDefault = 0,
            ValorForm = 1,
            WisdomForm = 2,
            LimitForm = 3,
            MasterForm = 4,
            FinalForm = 5,
            AntiForm = 6,
            LionKingSora = 7,
            AtlanticaSora = 8,
            SoraCarpet = 9,
            RoxasDualWield = 10,
            Default = 11, // used on Enemies
            CubeCardForm = 12
        }
    }
}
