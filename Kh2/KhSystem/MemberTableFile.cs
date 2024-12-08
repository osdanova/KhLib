using KhLib.Kh2.Structs;
using Xe.BinaryMapper;

namespace KhLib.Kh2.KhSystem
{
    public class MemberTableFile
    {
        /******************************************
         * Properties
         ******************************************/
        public List<Entry> Entries = new List<Entry>();
        public List<Set> Sets = new List<Set>();

        /******************************************
         * Constructors
         ******************************************/
        public MemberTableFile()
        {
            Entries = new List<Entry>();
            Sets = new List<Set>();
        }

        /******************************************
         * Functions - Static
         ******************************************/
        public static MemberTableFile Read(byte[] byteFile)
        {
            MemberTableFile file = new MemberTableFile();

            using (MemoryStream stream = new MemoryStream(byteFile))
            {
                BaseTable<Entry> table = BaseTable<Entry>.Read(stream, 8);
                file.Entries = table.Entries;

                file.Sets = new List<Set>();
                while (stream.Position < byteFile.Length)
                {
                    file.Sets.Add(BinaryMapping.ReadObject<Set>(stream));
                }
                if(file.Sets.Count > 6)
                {
                    file.Sets[0].Description = "NO_FRIEND";
                    file.Sets[1].Description = "DEFAULT";
                    file.Sets[2].Description = "W_FRIEND";
                    file.Sets[3].Description = "W_FRIEND_IN";
                    file.Sets[4].Description = "W_FRIEND_FIX";
                    file.Sets[5].Description = "W_FRIEND_ONLY";
                    file.Sets[6].Description = "DONALD_ONLY";
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
                BaseTable<Entry>.Write(stream, 5, 8, Entries);

                foreach(Set set in Sets)
                {
                    BinaryMapping.WriteObject(stream, set);
                }

                return stream.ToArray();
            }
        }

        // MemberTable
        public class Entry
        {
            [Data] public ushort WorldId { get; set; }
            [Data] public ushort CheckStoryFlag { get; set; }
            [Data] public ushort CheckStoryFlagNegation { get; set; }
            [Data] public byte CheckArea { get; set; }
            [Data] public byte Padding { get; set; }
            [Data] public int PlayerSize { get; set; }
            [Data] public int FriendSize { get; set; }
            [Data] public ushort Player {  get; set; }
            [Data] public ushort Party1 {  get; set; }
            [Data] public ushort Party2 {  get; set; }
            [Data] public ushort Party3 {  get; set; }
            [Data] public ushort Valor { get; set; }
            [Data] public ushort Wisdom { get; set; }
            [Data] public ushort Limit { get; set; }
            [Data] public ushort Master { get; set; }
            [Data] public ushort Final { get; set; }
            [Data] public ushort Anti { get; set; }
            [Data] public ushort Mickey { get; set; }
            [Data] public ushort PlayerHp { get; set; }
            [Data] public ushort ValorHp { get; set; }
            [Data] public ushort WisdomHp { get; set; }
            [Data] public ushort LimitHp { get; set; }
            [Data] public ushort MasterHp { get; set; }
            [Data] public ushort FinalHp { get; set; }
            [Data] public ushort PlayerHp2 { get; set; }
        }

        // MemberSet
        public class Set
        {
            [Data] public byte Player { get; set; }
            [Data] public byte Party1 { get; set; }
            [Data] public byte Party2 { get; set; }
            [Data] public byte Party3 { get; set; }
            public string Description { get; set; }
        }
    }
}
