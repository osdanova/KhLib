using KhLib.Kh2.Dictionaries;
using KhLib.Kh2.Utils;
using System.Text;
using Xe.BinaryMapper;

namespace KhLib.Kh2.Jiminy
{
    public class DiagramsFile
    {
        /******************************************
         * Properties
         ******************************************/
        private static readonly string _Identifier = "JMDI";
        public int Version;
        public List<Entry> Entries = new List<Entry>();

        /******************************************
         * Constructors
         ******************************************/
        public DiagramsFile()
        {
            Version = 18;
            Entries = new List<Entry>();
        }

        /******************************************
         * Functions - Static
         ******************************************/
        public static DiagramsFile Read(byte[] byteFile)
        {
            DiagramsFile file = new DiagramsFile();

            using (MemoryStream stream = new MemoryStream(byteFile))
            using (BinaryReader reader = new BinaryReader(stream))
            {
                string identifier = Encoding.UTF8.GetString(byteFile, 0, 4);
                if (identifier != _Identifier)
                {
                    throw new Exception("[DiagramsFile] identifier read (" + identifier + ") should be (" + _Identifier + ")");
                }

                stream.Position = 4;
                file.Version = reader.ReadInt32();
                int entryCount = reader.ReadInt32();


                stream.Position = 16;
                file.Entries = new List<Entry>();
                for (int i = 0; i < entryCount; i++)
                {
                    file.Entries.Add(BinaryMapping.ReadObject<Entry>(stream));
                }

                foreach (Entry entry in file.Entries)
                {
                    entry.CharacterList = new List<Character>();
                    stream.Position = entry.CharacterOffset;
                    for(int i = 0; i < entry.CharacterCount; i++)
                    {
                        entry.CharacterList.Add(BinaryMapping.ReadObject<Character>(stream));
                    }
                }
            }

            return file;
        }

        /******************************************
         * Functions - Local
         ******************************************/
        public byte[] GetAsByteArray()
        {
            using (MemoryStream stream = new MemoryStream())
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                byte[] idBytes = Encoding.UTF8.GetBytes(_Identifier);
                stream.Write(idBytes, 0, idBytes.Length);
                writer.Write((int)Version);
                writer.Write((int)Entries.Count);
                stream.Position += 4;

                int entryListOffset = (int)stream.Position;
                int characterOffset = 16 + (Entries.Count * 12);
                characterOffset += ReadWriteUtils.BytesRequiredToAlignToByte(characterOffset, 16); // Align to 16 after entry list.
                stream.Position = characterOffset;
                foreach (Entry entry in Entries)
                {
                    entry.CharacterCount = (byte)entry.CharacterList.Count;
                    entry.CharacterOffset = (uint)stream.Position;

                    foreach(Character chara in entry.CharacterList)
                    {
                        BinaryMapping.WriteObject<Character>(stream, chara);
                    }
                }
                ReadWriteUtils.AlignStreamToByte(stream, 16); // Align to 16 after character list.

                stream.Position = entryListOffset;
                foreach (Entry entry in Entries)
                {
                    BinaryMapping.WriteObject<Entry>(stream, entry);
                }

                return stream.ToArray();
            }
        }

        // JmDiagramDataHead | JmDiagramDataInfo | JmDiagramCharaInfo
        // JmDiagramDataHead is the same as the other files in jiminy.
        // The list of JmDiagramDataInfo follows. Whenever it ends it aligns to 16 bytes.
        // The list of JmDiagramCharaInfo follows. Whenever it ends it aligns to 16 bytes.
        public class Entry
        {
            [Data] public ushort ProgressDrawId { get; set; }
            [Data] public ushort ProgressHideId { get; set; }
            [Data] public JmWorld_Enum WorldId { get; set; }
            [Data] public byte CharacterCount { get; set; }
            [Data] public byte Type { get; set; }
            [Data] public byte Filler { get; set; }
            [Data] public uint CharacterOffset { get; set; }
            public List<Character> CharacterList { get; set; }

            public Entry()
            {
                CharacterList = new List<Character>();
            }
        }

        public class Character
        {
            [Data] public ushort LabelId { get; set; }
            [Data] public short Xpos { get; set; }
            [Data] public short Ypos { get; set; }
            [Data] public byte Draw { get; set; }
            [Data] public byte Filler { get; set; }
        }
    }
}
