using KhLib.Kh2.Structs;
using Xe.BinaryMapper;

namespace KhLib.Kh2.Battle
{
    public class EnemyParamsFile : BaseTableFile<EnemyParamsFile.Entry>
    {
        public EnemyParamsFile() : base(4, 4, 2) { }

        public static EnemyParamsFile Read(byte[] byteFile)
        {
            EnemyParamsFile file = new EnemyParamsFile();
            file.ReadFile(byteFile);

            return file;
        }

        // ENEMYPARAM
        public class Entry
        {
            [Data] public ushort Id { get; set; }
            [Data] public ushort Level { get; set; }
            [Data(Count=32)] public ushort[] Hp { get; set; }
            [Data] public ushort DamageMax { get; set; }
            [Data] public ushort DamageMin { get; set; }
            [Data] public ushort ResPhysical { get; set; }
            [Data] public ushort ResFire { get; set; }
            [Data] public ushort ResBlizzard{ get; set; }
            [Data] public ushort ResThunder { get; set; }
            [Data] public ushort ResDark { get; set; }
            [Data] public ushort ResSpecial { get; set; }
            [Data] public ushort ResAbsolute { get; set; }
            [Data] public ushort Exp { get; set; }
            [Data] public ushort Prize { get; set; }
            [Data] public ushort BonusLevel { get; set; }

            public Entry()
            {
                Hp = new ushort[32];
            }
        }
    }
}
