using KhLib.Kh2.Dictionaries;
using KhLib.Kh2.Structs;
using Xe.BinaryMapper;

namespace KhLib.Kh2.Jiminy
{
    public partial class MinigamesFile : BaseTableFile<MinigamesFile.Entry>
    {
        public MinigamesFile() : base(4, 4, 18, "JMMG", 4, 16) { }

        public static MinigamesFile Read(byte[] byteFile)
        {
            MinigamesFile file = new MinigamesFile();
            file.ReadFile(byteFile);

            return file;
        }

        // JmMiniGameDataHead | JmMiniGameDataInfo
        public class Entry
        {
            [Data] public ushort WorldId { get; set; }
            [Data] public ushort TitleId { get; set; }
            [Data] public ushort UnitId { get; set; }
            [Data] public ushort Index { get; set; }
        }
    }
}
