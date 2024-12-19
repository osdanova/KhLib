using KhLib.Kh2.Structs;
using Xe.BinaryMapper;

namespace KhLib.Kh2.Jiminy
{
    public class WorldsFile : BaseTableFile<WorldsFile.Entry>
    {
        public WorldsFile() : base(4, 4, 18, "JMWO", 4) { }

        public static WorldsFile Read(byte[] byteFile)
        {
            WorldsFile file = new WorldsFile();
            file.ReadFile(byteFile);

            return file;
        }

        // JmWorldDataHead | JmWorldDataInfo
        public class Entry
        {
            [Data] public byte Id { get; set; }
            [Data(Count = 3)] public string Name { get; set; }
            [Data] public ushort TitleId { get; set; }
            [Data] public ushort MenuId { get; set; }
            [Data] public ushort OpenProgressId { get; set; }
            [Data] public ushort AltTitleId { get; set; }
            [Data] public ushort AltMenuId { get; set; }
            [Data] public ushort AltProgressId { get; set; }
        }
    }
}
