using KhLib.Kh2.Structs;
using Xe.BinaryMapper;

namespace KhLib.Kh2.Mixdata
{
    public class ConditionsFile : BaseTableFile<ConditionsFile.Entry>
    {
        public ConditionsFile() : base(4, 4, 3, "MICO", 4, 16) { }

        public static ConditionsFile Read(byte[] byteFile)
        {
            ConditionsFile file = new ConditionsFile();
            file.ReadFile(byteFile);

            return file;
        }

        // MixConditionDataHead | MixConditionDataInfo
        public class Entry
        {
            [Data] public ushort MessageId { get; set; }
            [Data] public ushort RewardId { get; set; }
            [Data] public RewardType RewardType { get; set; }
            [Data] public byte ConditionType { get; set; }
            [Data] public byte Rank { get; set; }
            [Data] public CountType CountType { get; set; }
            [Data] public ushort Count { get; set; }
            [Data] public short MenuFlagId { get; set; }
        }

        public enum RewardType : byte
        {
            Item,
            Upgrade,
        }

        public enum CountType : byte
        {
            Stack,
            Collect,
        }

        public enum Rank : byte
        {
            C,
            B,
            A,
            S
        }
    }
}
