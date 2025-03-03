using KhLib.Kh2.Structs;
using Xe.BinaryMapper;

namespace KhLib.Kh2.LocalSet
{
    public class LocalSetWorld : BaseTableFile<LocalSetWorld.Entry>
    {
        public LocalSetWorld() : base(4, 4) { }

        public static LocalSetWorld Read(byte[] byteFile)
        {
            LocalSetWorld file = new LocalSetWorld();
            file.ReadFile(byteFile);

            return file;
        }

        // LOCALSET
        public class Entry
        {
            [Data] public short LocalSetId { get; set; }
            [Data] public short AreaId { get; set; }
        }
    }
}
