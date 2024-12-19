using KhLib.Kh2.Structs;
using Xe.BinaryMapper;

namespace KhLib.Kh2.Jiminy
{
    public class QuestsFile : BaseTableFile<QuestsFile.Entry>
    {
        public QuestsFile() : base(4, 4, 18, "JMQU", 4, 16) { }

        public static QuestsFile Read(byte[] byteFile)
        {
            QuestsFile file = new QuestsFile();
            file.ReadFile(byteFile);

            return file;
        }

        // JmQuestDataHead | JmQuestDataInfo
        public class Entry
        {
            [Data] public ushort WorldId { get; set; }
            [Data] public ushort CategoryId { get; set; }
            [Data] public ushort TitleId { get; set; }
            [Data] public ushort Stat { get; set; }
            [Data] public ushort DrawConditionId { get; set; }
            [Data] public ushort MinigameId { get; set; }
            [Data] public ushort Score { get; set; }
            [Data] public ushort ClearConditionId { get; set; }
        }
    }
}
