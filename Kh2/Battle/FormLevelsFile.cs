using KhLib.Kh2.Structs;
using Xe.BinaryMapper;

namespace KhLib.Kh2.Battle
{
    public class FormLevelsFile : BaseTableFile<FormLevelsFile.Entry>
    {
        public FormLevelsFile() : base(4, 4, 2) { }

        public static FormLevelsFile Read(byte[] byteFile)
        {
            FormLevelsFile file = new FormLevelsFile();
            file.ReadFile(byteFile);

            return file;
        }

        // SUMMON_TABLE
        public class Entry
        {
            [Data] public byte Id_Level { get; set; }
            [Data] public byte AntiRate_AbiLevel { get; set; }
            [Data] public ushort AbilityId { get; set; }
            [Data] public int Exp { get; set; }

            public byte Id
            {
                get => (byte)(Id_Level >> 4);
                set => Id_Level = (byte)((Id_Level & 0x0F) | (value << 4));
            }

            public byte Level
            {
                get => (byte)(Id_Level & 0x0F);
                set => Id_Level = (byte)((Id_Level & 0xF0) | (value & 0x0F));
            }

            public byte AntiRate
            {
                get => (byte)(AntiRate_AbiLevel >> 4);
                set => AntiRate_AbiLevel = (byte)((AntiRate_AbiLevel & 0x0F) | (value << 4));
            }

            public byte AbilityLevel
            {
                get => (byte)(AntiRate_AbiLevel & 0x0F);
                set => AntiRate_AbiLevel = (byte)((AntiRate_AbiLevel & 0xF0) | (value & 0x0F));
            }
        }
    }
}
