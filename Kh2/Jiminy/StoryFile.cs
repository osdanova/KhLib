using KhLib.Kh2.Dictionaries;
using KhLib.Kh2.Structs;
using Xe.BinaryMapper;

namespace KhLib.Kh2.Jiminy
{
    public class StoryFile : BaseTableFile<StoryFile.Entry>
    {
        public StoryFile() : base(4, 4, 18, "JMST", 4) { }

        public static StoryFile Read(byte[] byteFile)
        {
            StoryFile file = new StoryFile();
            file.ReadFile(byteFile);

            return file;
        }

        // JmStoryDataHead | JmStoryDataInfo
        public class Entry
        {
            [Data] public JmWorld_Enum WorldId { get; set; }
            [Data(Count = 3)] public byte[] Filler { get; set; }
            [Data] public ushort LogTextId { get; set; }
            [Data] public ushort NextTextId { get; set; }
            [Data] public ushort StoryTextId { get; set; }
            [Data] public ushort ProgressId { get; set; }

            public Entry()
            {
                Filler = new byte[3];
            }
        }
    }
}
