using KhLib.Kh2.Structs;
using Xe.BinaryMapper;

namespace KhLib.Kh2.Mixdata
{
    public class ExperienceFile : BaseTableFile<ExperienceFile.Entry>
    {
        public ExperienceFile() : base(4, 4, 3, "MIEX", 4, 16) { }

        public static ExperienceFile Read(byte[] byteFile)
        {
            ExperienceFile file = new ExperienceFile();
            file.ReadFile(byteFile);

            return file;
        }

        // MixExpDataHead | MixExpDataInfo
        public class Entry
        {
            [Data] public ushort RankC { get; set; }
            [Data] public ushort RankB { get; set; }
            [Data] public ushort RankA { get; set; }
            [Data] public ushort RankS { get; set; }
            [Data] public float ExpRate { get; set; }
            [Data] public ushort ProgressId { get; set; }
            [Data] public ushort Func { get; set; }
        }
    }
}
