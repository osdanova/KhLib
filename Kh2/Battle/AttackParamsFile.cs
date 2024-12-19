using KhLib.Kh2.Structs;
using KhLib.Kh2.Utils;
using Xe.BinaryMapper;

namespace KhLib.Kh2.Battle
{
    public class AttackParamsFile
    {
        /******************************************
         * Properties
         ******************************************/
        public int Version;
        public List<Entry> Entries = new List<Entry>();

        /******************************************
         * Constructors
         ******************************************/
        public AttackParamsFile()
        {
            Version = 6;
            Entries = new List<Entry>();
        }

        /******************************************
         * Functions - Static
         ******************************************/
        public static AttackParamsFile Read(byte[] byteFile)
        {
            AttackParamsFile file = new AttackParamsFile();

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

        // ATTACKPARAM
        public class Entry
        {
            [Data] public ushort Level { get; set; }
            [Data] public ushort Id { get; set; }
            [Data] public AttackType Type { get; set; }
            [Data] public byte BaseAdjust { get; set; } // Crit adjust
            [Data] public ushort Power { get; set; } // Power
            [Data] public TeamType Team { get; set; } // 1 = owner is player
            [Data] public ElementType Element { get; set; }
            [Data] public byte ReactionId { get; set; }
            [Data] public byte Effect { get; set; } // Hit effetc
            [Data] public short KnockbackStr1 { get; set; }
            [Data] public short KnockbackStr2 { get; set; }
            [Data] public ushort Unk16 { get; set; }
            [Data] public AtkpFlags Flags { get; set; }
            [Data] public RefactType RefactSelf { get; set; }
            [Data] public RefactType RefactOther { get; set; }
            [Data] public byte  ReflectMotion { get; set; }
            [Data] public short ReflectHitBack { get; set; }
            [Data] public int ReflectAct { get; set; } // reflect action
            [Data] public int HitSoundEffectId { get; set; } // Hit sound effect
            [Data] public ushort ReflectRC { get; set; }
            [Data] public byte RefRange { get; set; }
            [Data] public sbyte RefAngle { get; set; }
            [Data] public byte DamageEffect { get; set; }
            [Data] public byte SwitchValue { get; set; }
            [Data] public ushort FramesPerHit { get; set; } // Frames per hit
            [Data] public byte FloorCheck { get; set; }
            [Data] public byte AddDrive { get; set; }
            [Data] public byte Revenge { get; set; } // Karma
            [Data] public TrReactionType TrReaction { get; set; } // Terrain reaction?
            [Data] public byte ComboGroup { get; set; }
            [Data] public byte RandomEffect { get; set; }
            [Data] public KindType Kind { get; set; }
            [Data] public byte HpGain { get; set; } // Hp drain

            public bool FlagBgHit
            {
                get => BitFlag.IsFlagSet(Flags, AtkpFlags.BgHit);
                set => Flags = BitFlag.SetFlag(Flags, AtkpFlags.BgHit, value);
            }
            public bool FlagLimitPAX
            {
                get => BitFlag.IsFlagSet(Flags, AtkpFlags.LimitPAX);
                set => Flags = BitFlag.SetFlag(Flags, AtkpFlags.LimitPAX, value);
            }
            public bool FlagLand
            {
                get => BitFlag.IsFlagSet(Flags, AtkpFlags.Land);
                set => Flags = BitFlag.SetFlag(Flags, AtkpFlags.Land, value);
            }
            public bool FlagCapturePax
            {
                get => BitFlag.IsFlagSet(Flags, AtkpFlags.CapturePax);
                set => Flags = BitFlag.SetFlag(Flags, AtkpFlags.CapturePax, value);
            }
            public bool FlagThankYou
            {
                get => BitFlag.IsFlagSet(Flags, AtkpFlags.ThankYou);
                set => Flags = BitFlag.SetFlag(Flags, AtkpFlags.ThankYou, value);
            }
            public bool FlagKillBoss
            {
                get => BitFlag.IsFlagSet(Flags, AtkpFlags.KillBoss);
                set => Flags = BitFlag.SetFlag(Flags, AtkpFlags.KillBoss, value);
            }
        }

        [Flags]
        public enum AtkpFlags : byte
        {
            BgHit = 0x01,
            LimitPAX = 0x02,
            Land = 0x04,
            CapturePax = 0x08,
            ThankYou = 0x10,
            KillBoss = 0x20
        }

        public enum AttackType : byte
        {
            NormalAttack = 0,
            PierceArmor = 1,
            Guard = 2,
            SGuard = 3,
            Special = 4,
            Cure = 5,
            SCure = 6
        }

        public enum TeamType : byte
        {
            Unk0 = 0,
            Player = 1,
            Unk2 = 2,
            Unk3 = 3,
            Unk4 = 4
        }

        public enum ElementType : byte
        {
            Neutral = 0,
            Fire = 1,
            Blizzard = 2,
            Thunder = 3,
            Dark = 4,
            Special = 5,
            Absolute = 6,
        }

        public enum RefactType : byte
        {
            Reflect = 0,
            Guard = 1,
            Nothing = 2
        }

        public enum TrReactionType : byte
        {
            Attack = 0,
            Charge = 1,
            Crash = 2,
            Wall = 3
        }

        public enum KindType : byte
        {
            None = 0,
            ComboFinisher = 1,
            AirComboFinisher = 2,
            ReactionCommand = 4
        }
    }
}
