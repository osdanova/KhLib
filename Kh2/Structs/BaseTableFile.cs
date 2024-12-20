using KhLib.Kh2.Utils;
using System.Text;
using Xe.BinaryMapper;

namespace KhLib.Kh2.Structs
{
    /*
     * Holds a list of entries that have the same structure.
     */
    public class BaseTableFile<T> where T : class
    {
        /******************************************
         * Properties
         ******************************************/
        private string? _Identifier;
        private int? _Version;
        private int _VersionSize;
        private int _CountSize;
        private int _HeaderPaddingSize;
        private int _AlignFileTo;
        public List<T> Entries { get; set; }

        /******************************************
         * Constructors
         ******************************************/
        public BaseTableFile()
        {
            // Most common
            _CountSize = 4;
            Entries = new List<T>();
        }
        public BaseTableFile(int versionSize, int countSize, int? version = null, string? identifier = null, int headerPaddingSize = 0, int alignFileTo = 0)
        {
            _VersionSize = versionSize;
            _CountSize = countSize;

            _Identifier = identifier;
            _Version = version;

            Entries = new List<T>();
            _HeaderPaddingSize = headerPaddingSize;

            _AlignFileTo = alignFileTo;
        }

        /******************************************
         * Functions - Static
         ******************************************/
        public void ReadFile(byte[] byteFile)
        {

            using (MemoryStream stream = new MemoryStream(byteFile))
            using(BinaryReader br = new BinaryReader(stream))
            {
                if (_Identifier != null)
                {
                    string identifier = Encoding.UTF8.GetString(byteFile, 0, _Identifier.Length);
                    if (identifier != _Identifier)
                    {
                        throw new Exception("[BaseTableFile] identifier read (" + identifier + ") should be (" + _Identifier + ")");
                    }
                    stream.Position = _Identifier.Length;
                }

                if(_VersionSize > 0)
                {
                    switch (_VersionSize)
                    {
                        case 1:
                            _Version = br.ReadByte();
                            break;
                        case 2:
                            _Version = br.ReadInt16();
                            break;
                        case 4:
                            _Version = br.ReadInt32();
                            break;
                        default:
                            throw new Exception("[BaseTableFile] version size not valid");
                    }
                }

                int entryCount = 0;
                switch (_CountSize)
                {
                    case 1:
                        entryCount = br.ReadByte();
                        break;
                    case 2:
                        entryCount = br.ReadInt16();
                        break;
                    case 4:
                        entryCount = br.ReadInt32();
                        break;
                    default:
                        throw new Exception("[BaseTableFile] count size not valid");
                }

                if(_HeaderPaddingSize > 0)
                {
                    stream.Position += _HeaderPaddingSize;
                }

                Entries = new List<T>();
                for (int i = 0; i < entryCount; i++)
                {
                    Entries.Add(BinaryMapping.ReadObject<T>(stream));
                }
            }
        }

        /******************************************
         * Functions - Local
         ******************************************/
        public byte[] GetAsByteArray()
        {
            using (MemoryStream stream = new MemoryStream())
            using (BinaryWriter bw = new BinaryWriter(stream))
            {
                if (_Identifier != null)
                {
                    byte[] idBytes = Encoding.UTF8.GetBytes(_Identifier);
                    stream.Write(idBytes, 0, idBytes.Length);
                }

                if (_VersionSize > 0)
                {
                    switch (_VersionSize)
                    {
                        case 1:
                            bw.Write((byte)_Version);
                            break;
                        case 2:
                            bw.Write((short)_Version);
                            break;
                        case 4:
                            bw.Write((int)_Version);
                            break;
                        default:
                            throw new Exception("[BaseTableFile] version size not valid");
                    }
                }
                    
                switch (_CountSize)
                {
                    case 1:
                        bw.Write((byte)Entries.Count);
                        break;
                    case 2:
                        bw.Write((short)Entries.Count);
                        break;
                    case 4:
                        bw.Write((int)Entries.Count);
                        break;
                    default:
                        throw new Exception("[BaseTableFile] count size not valid");
                }

                if (_HeaderPaddingSize > 0)
                {
                    stream.Position += _HeaderPaddingSize;
                }

                foreach (var entry in Entries)
                {
                    BinaryMapping.WriteObject<T>(stream, entry);
                }

                if(_AlignFileTo != null && _AlignFileTo != 0)
                {
                    ReadWriteUtils.AlignStreamToByte(stream, _AlignFileTo);
                }

                return stream.ToArray();
            }
        }
    }
}
