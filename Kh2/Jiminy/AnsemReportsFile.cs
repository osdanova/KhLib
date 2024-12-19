using KhLib.Kh2.Structs;
using Xe.BinaryMapper;

namespace KhLib.Kh2.Jiminy
{
    public class AnsemReportsFile : BaseTableFile<AnsemReportsFile.Entry>
    {
        public AnsemReportsFile() : base(4, 4, 18, "JMAN", 4) { }

        public static AnsemReportsFile Read(byte[] byteFile)
        {
            AnsemReportsFile file = new AnsemReportsFile();
            file.ReadFile(byteFile);

            return file;
        }

        // JmAnsemDataHead | JmAnsemDataInfo
        public class Entry
        {
            [Data] public ushort ItemId { get; set; }
            [Data] public ushort TitleId { get; set; }
            [Data] public ushort TextId { get; set; }
            [Data] public ushort Filler { get; set; }
        }
    }
}
