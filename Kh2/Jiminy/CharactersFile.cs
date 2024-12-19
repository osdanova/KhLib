using KhLib.Kh2.Dictionaries;
using KhLib.Kh2.Structs;
using Xe.BinaryMapper;

namespace KhLib.Kh2.Jiminy
{
    public class CharactersFile : BaseTableFile<CharactersFile.Entry>
    {
        public CharactersFile() : base(4, 4, 18, "JMCH", 4) { }

        public static CharactersFile Read(byte[] byteFile)
        {
            CharactersFile file = new CharactersFile();
            file.ReadFile(byteFile);

            return file;
        }

        // JmCharaDataHead | JmCharaDataInfo
        public class Entry
        {
            [Data] public JmWorld_Enum WorldId { get; set; }
            [Data] public byte Graphic { get; set; }
            [Data] public byte GraphicBase { get; set; }
            [Data] public byte Filler { get; set; }
            [Data] public ushort Index { get; set; }
            [Data] public ushort NameId { get; set; }
            [Data] public ushort TextId { get; set; }
            [Data] public ushort SourceTitleId { get; set; }
            [Data] public ushort ObjectId { get; set; }
            [Data] public ushort MotionId { get; set; }
            [Data] public ushort Stat { get; set; }
            [Data] public short Xpos { get; set; }
            [Data] public short Ypos { get; set; }
            [Data] public short Yrot { get; set; }
            [Data] public short Xpos2 { get; set; }
            [Data] public short Ypos2 { get; set; }
            [Data] public float Scale { get; set; }
            [Data] public float Scale2 { get; set; }
        }
    }
}
