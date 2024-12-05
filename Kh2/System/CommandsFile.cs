using KhLib.Kh2.Structs;
using KhLib.Kh2.Utils;
using Xe.BinaryMapper;

namespace KhLib.Kh2.System
{
    public class CommandsFile
    {
        /******************************************
         * Properties
         ******************************************/
        public int Version;
        public List<Entry> Entries = new List<Entry>();

        /******************************************
         * Constructors
         ******************************************/
        public CommandsFile()
        {
            Version = 2;
            Entries = new List<Entry>();
        }

        /******************************************
         * Functions - Static
         ******************************************/
        public static CommandsFile Read(byte[] byteFile)
        {
            CommandsFile file = new CommandsFile();

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

        // struct COMMAND_ELEM
        public class Entry
        {
            [Data] public ushort Id { get; set; }
            [Data] public ushort Execute { get; set; }
            [Data] public short Argument { get; set; } //this can be Argument, Form, Magic
            [Data] public sbyte SubMenu { get; set; }
            [Data] public Icon CmdIcon { get; set; }
            [Data] public int MessageId { get; set; }
            [Data] public int Flags { get; set; }
            [Data] public float Range { get; set; }
            [Data] public float Dir { get; set; }
            [Data] public float DirRange { get; set; }
            [Data] public byte Cost { get; set; }
            [Data] public Camera CmdCamera { get; set; }
            [Data] public byte Priority { get; set; }
            [Data] public Receiver CmdReceiver { get; set; }
            [Data] public ushort Time { get; set; }
            [Data] public ushort Require { get; set; }
            [Data] public byte Mark { get; set; }
            [Data] public Action CmdAction { get; set; }
            [Data] public ushort ReactionCount { get; set; }
            [Data] public ushort DistRange { get; set; }
            [Data] public ushort Score { get; set; }
            [Data] public ushort DisableForm { get; set; }
            [Data] public byte Group { get; set; }
            [Data] public byte Reserve { get; set; }

            public bool Cursor
            {
                get => BitUtils.Int.GetBit(Flags, 0);
                set => Flags = (ushort)BitUtils.Int.SetBit(Flags, 0, value);
            }
            public bool Land
            {
                get => BitUtils.Int.GetBit(Flags, 1);
                set => Flags = (ushort)BitUtils.Int.SetBit(Flags, 1, value);
            }
            public bool Force
            {
                get => BitUtils.Int.GetBit(Flags, 2);
                set => Flags = (ushort)BitUtils.Int.SetBit(Flags, 2, value);
            }
            public bool Combo
            {
                get => BitUtils.Int.GetBit(Flags, 3);
                set => Flags = (ushort)BitUtils.Int.SetBit(Flags, 3, value);
            }
            public bool Battle
            {
                get => BitUtils.Int.GetBit(Flags, 4);
                set => Flags = (ushort)BitUtils.Int.SetBit(Flags, 4, value);
            }
            public bool Secure
            {
                get => BitUtils.Int.GetBit(Flags, 5);
                set => Flags = (ushort)BitUtils.Int.SetBit(Flags, 5, value);
            }
            public bool RequireFlag
            {
                get => BitUtils.Int.GetBit(Flags, 6);
                set => Flags = (ushort)BitUtils.Int.SetBit(Flags, 6, value);
            }
            public bool NoCombo
            {
                get => BitUtils.Int.GetBit(Flags, 7);
                set => Flags = (ushort)BitUtils.Int.SetBit(Flags, 7, value);
            }
            public bool Drive
            {
                get => BitUtils.Int.GetBit(Flags, 8);
                set => Flags = (ushort)BitUtils.Int.SetBit(Flags, 8, value);
            }
            public bool Short
            {
                get => BitUtils.Int.GetBit(Flags, 9);
                set => Flags = (ushort)BitUtils.Int.SetBit(Flags, 9, value);
            }
            public bool DisableSora
            {
                get => BitUtils.Int.GetBit(Flags, 10);
                set => Flags = (ushort)BitUtils.Int.SetBit(Flags, 10, value);
            }
            public bool DisableRoxas
            {
                get => BitUtils.Int.GetBit(Flags, 11);
                set => Flags = (ushort)BitUtils.Int.SetBit(Flags, 11, value);
            }
            public bool DisableLionSora
            {
                get => BitUtils.Int.GetBit(Flags, 12);
                set => Flags = (ushort)BitUtils.Int.SetBit(Flags, 12, value);
            }
            public bool DisableLimitForm
            {
                get => BitUtils.Int.GetBit(Flags, 13);
                set => Flags = (ushort)BitUtils.Int.SetBit(Flags, 13, value);
            }
            // Flag bit 14 is unused
            public bool DisableSkateboard
            {
                get => BitUtils.Int.GetBit(Flags, 15);
                set => Flags = (ushort)BitUtils.Int.SetBit(Flags, 15, value);
            }
            public bool InBattleOnly
            {
                get => BitUtils.Int.GetBit(Flags, 16);
                set => Flags = (ushort)BitUtils.Int.SetBit(Flags, 16, value);
            }
        }

        /******************************************
         * Enums
         ******************************************/
        public enum Action : byte
        {
            Null = 0,
            Idle = 1,
            Jump = 2,
        }

        public enum Camera : byte
        {
            Null = 0,
            Watch = 1,
            LockOn = 2,
            WatchLockOn = 3,
        }

        public enum Icon : byte
        {
            Null = 0,
            Attack = 1,
            Magic = 2,
            Item = 3,
            Form = 4,
            Summon = 5,
            Friend = 6,
            Limit = 7,
        }

        public enum Receiver : byte
        {
            Player = 0,
            Target = 1,
            Both = 2,
        }
    }
}
