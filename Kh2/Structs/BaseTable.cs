using System.Text;
using Xe.BinaryMapper;

namespace KhLib.Kh2.Structs
{
    public class BaseTable<T> where T : class
    {
        public byte HeaderSize { get; set; }
        public int Version { get; set; }
        public int Count { get; set; }
        public List<T> Entries { get; set; }

        public static BaseTable<T> Read(Stream stream, byte headerSize)
        {
            BaseTable<T> baseTable = new BaseTable<T>();
            using (BinaryReader br = new BinaryReader(stream, Encoding.Default, true))
            {
                switch(headerSize)
                {
                    case 2:
                        baseTable.Version = br.ReadByte();
                        baseTable.Count = br.ReadByte();
                        break;
                    case 4:
                        baseTable.Version = br.ReadInt16();
                        baseTable.Count = br.ReadInt16();
                        break;
                    case 8:
                        baseTable.Version = br.ReadInt32();
                        baseTable.Count = br.ReadInt32();
                        break;
                    default:
                        throw new Exception("BaseTable size not valid");
                }
            }
            baseTable.Entries = Enumerable.Range(0, baseTable.Count)
                                          .Select(_ => BinaryMapping.ReadObject<T>(stream))
                                          .ToList();
            return baseTable;
        }

        public static void Write(Stream stream, int version, byte headerSize, List<T> items)
        {
            var itemList = items as IList<T> ?? items.ToList();

            using (BinaryWriter bw = new BinaryWriter(stream, Encoding.Default, true))
            {
                switch (headerSize)
                {
                    case 2:
                        bw.Write((byte)version);
                        bw.Write((byte)itemList.Count);
                        break;
                    case 4:
                        bw.Write((short)version);
                        bw.Write((short)itemList.Count);
                        break;
                    case 8:
                        bw.Write(version);
                        bw.Write(itemList.Count);
                        break;
                    default:
                        throw new Exception("BaseTable size not valid");
                }
            }

            foreach (var item in itemList)
                BinaryMapping.WriteObject(stream, item);
        }
    }
}
