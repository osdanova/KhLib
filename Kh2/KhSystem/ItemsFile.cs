using KhLib.Kh2.Dictionaries;
using KhLib.Kh2.Structs;
using Xe.BinaryMapper;

namespace KhLib.Kh2.KhSystem
{
    public class ItemsFile
    {
        public List<Entry> Items;
        public List<Param> Params;

        /******************************************
         * Constructors
         ******************************************/
        public ItemsFile()
        {
            Items = new List<Entry>();
            Params = new List<Param>();
        }

        /******************************************
         * Functions - Static
         ******************************************/
        public static ItemsFile Read(byte[] byteFile)
        {
            ItemsFile file = new ItemsFile();

            using (MemoryStream stream = new MemoryStream(byteFile))
            {
                file.Items = BaseTable<Entry>.Read(stream, 8).Entries;
                file.Params = BaseTable<Param>.Read(stream, 8).Entries;
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
                BaseTable<Entry>.Write(stream, 6, 8, Items);
                BaseTable<Param>.Write(stream, 0, 8, Params);

                return stream.ToArray();
            }
        }

        // ITEM_TABLE
        public class Entry
        {
            [Data] public ushort Id { get; set; }
            [Data] public Type Type { get; set; }
            [Data] public byte Flag { get; set; } // Special/Normal
            [Data(Count=4)] public byte[] CategoryParams { get; set; }
            [Data] public ushort NameId { get; set; }
            [Data] public ushort DescriptionId { get; set; }
            [Data] public ushort BuyPrice { get; set; }
            [Data] public ushort SellPrice { get; set; }
            [Data] public ushort CommandId { get; set; }
            [Data] public ushort Slot { get; set; }
            [Data] public short Picture { get; set; }
            [Data] public PrizeboxType Prizebox { get; set; }
            [Data] public byte Icon { get; set; }

            public Entry()
            {
                CategoryParams = new byte[4];
            }

            // PARAMS BY CATEGORY
            // Ability
            public short AbilityId
            {
                get => (short)(CategoryParams[0] | (CategoryParams[1] << 8));
                set
                {
                    CategoryParams[0] = (byte)(value & 0xFF);
                    CategoryParams[1] = (byte)((value >> 8) & 0xFF);
                }
            }
            public byte AbilityType
            {
                get => CategoryParams[2];
                set => CategoryParams[2] = value;
            }
            public byte AbilityAp
            {
                get => CategoryParams[3];
                set => CategoryParams[3] = value;
            }
            // Consumable
            public short ConsumableCureRate
            {
                get => (short)(CategoryParams[0] | (CategoryParams[1] << 8));
                set
                {
                    CategoryParams[0] = (byte)(value & 0xFF);
                    CategoryParams[1] = (byte)((value >> 8) & 0xFF);
                }
            }
            public short ConsumableEffect
            {
                get => (short)(CategoryParams[2] | (CategoryParams[3] << 8));
                set
                {
                    CategoryParams[2] = (byte)(value & 0xFF);
                    CategoryParams[3] = (byte)((value >> 8) & 0xFF);
                }
            }
            // Equipment
            public short EquipmentParam
            {
                get => (short)(CategoryParams[0] | (CategoryParams[1] << 8));
                set
                {
                    CategoryParams[0] = (byte)(value & 0xFF);
                    CategoryParams[1] = (byte)((value >> 8) & 0xFF);
                }
            }
            // Magic
            public short MagicId
            {
                get => (short)(CategoryParams[0] | (CategoryParams[1] << 8));
                set
                {
                    CategoryParams[0] = (byte)(value & 0xFF);
                    CategoryParams[1] = (byte)((value >> 8) & 0xFF);
                }
            }
            // Synthesis
            public byte SynthesisType
            {
                get => CategoryParams[0];
                set => CategoryParams[0] = value;
            }
            public byte SynthesisRank
            {
                get => CategoryParams[1];
                set => CategoryParams[1] = value;
            }
            // Report
            public short ReportId
            {
                get => (short)(CategoryParams[0] | (CategoryParams[1] << 8));
                set
                {
                    CategoryParams[0] = (byte)(value & 0xFF);
                    CategoryParams[1] = (byte)((value >> 8) & 0xFF);
                }
            }
            // Weapon
            public short WeaponId
            {
                get => (short)(CategoryParams[0] | (CategoryParams[1] << 8));
                set
                {
                    CategoryParams[0] = (byte)(value & 0xFF);
                    CategoryParams[1] = (byte)((value >> 8) & 0xFF);
                }
            }
            public short WeaponParam
            {
                get => (short)(CategoryParams[2] | (CategoryParams[3] << 8));
                set
                {
                    CategoryParams[2] = (byte)(value & 0xFF);
                    CategoryParams[3] = (byte)((value >> 8) & 0xFF);
                }
            }
            // Map
            public World_Enum MapWorld
            {
                get => (World_Enum)CategoryParams[0];
                set => CategoryParams[0] = (byte)value;
            }
        }

        // ITEM_PARAM
        public class Param
        {
            [Data] public ushort Id { get; set; }
            [Data] public ushort AbilityId { get; set; }
            [Data] public byte Strength { get; set; }
            [Data] public byte Magic { get; set; }
            [Data] public byte Defense { get; set; }
            [Data] public byte AbilityPoints { get; set; }
            [Data] public byte PhysicalResistance { get; set; }
            [Data] public byte FireResistance { get; set; }
            [Data] public byte IceResistance { get; set; }
            [Data] public byte LightningResistance { get; set; }
            [Data] public byte DarkResistance { get; set; }
            [Data] public byte NeutralResistance { get; set; }
            [Data] public byte GeneralResistance { get; set; }
            [Data] public byte Padding { get; set; }
        }

        public enum Type : byte
        {
            Consumable,
            Boost,
            Keyblade,
            Staff,
            Shield,
            PingWeapon,
            AuronWeapon,
            BeastWeapon,
            JackWeapon,
            DummyWeapon,
            RikuWeapon,
            SimbaWeapon,
            JackSparrowWeapon,
            TronWeapon,
            Armor,
            Accessory,
            Synthesis,
            Recipe,
            Magic,
            Ability,
            Summon,
            Form,
            Map,
            Report,
        }

        public enum Rank : byte
        {
            C,
            B,
            A,
            S
        }

        public enum AbilityType : byte
        {
            Support,
            Limit,
            Action,
            Growth
        }

        public enum PrizeboxType : byte
        {
            RedS,
            RedL,
            RedXL,
            BlueS,
            BlueL,
            BlueXL,
            HornedS,
            HornedL,
            HornedXL,
            PurpleS,
            PurpleL,
            PurpleXL
        }
    }
}
