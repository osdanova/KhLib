using KhLib.Kh2.Dictionaries;
using KhLib.Kh2.Structs;
using Xe.BinaryMapper;

namespace KhLib.Kh2.Jiminy
{
    public class AlbumFile : BaseTableFile<AlbumFile.Entry>
    {
        public AlbumFile() : base(4, 4, 18, "JMAL", 4) { }

        public static AlbumFile Read(byte[] byteFile)
        {
            AlbumFile file = new AlbumFile();
            file.ReadFile(byteFile);

            return file;
        }

        // JmAlbumDataHead | JmAlbumDataInfo
        public class Entry
        {
            [Data] public JmWorld_Enum WorldId { get; set; }
            [Data] public byte Graphic1Id { get; set; }
            [Data] public byte Graphic2Id { get; set; }
            [Data] public byte Graphic3Id { get; set; }
            [Data] public byte Graphic4Id { get; set; }
            [Data] public byte Graphic5Id { get; set; }
            [Data] public ushort ProgressId { get; set; }
            [Data] public ushort TitleId { get; set; }
            [Data] public ushort TextId { get; set; }
        }
    }
}
