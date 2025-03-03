namespace KhLib.Kh2.LocalSet
{
    public class LocalSetFile
    {
        private const int FINAL_PADDING = 400;

        public List<LocalSetWorld> Files = new List<LocalSetWorld>();

        public static LocalSetFile Read(byte[] byteFile)
        {
            LocalSetFile thisFile = new LocalSetFile();

            BinaryArchive archive = BinaryArchive.Read(byteFile);

            foreach(var a in archive.Entries)
            {
                thisFile.Files.Add(LocalSetWorld.Read(a.File));
            }

            return thisFile;
        }

        public byte[] GetAsByteArray()
        {
            BinaryArchive bar = new BinaryArchive();

            foreach (LocalSetWorld world in Files)
            {
                BinaryArchive.Entry entry = new BinaryArchive.Entry();
                entry.Name = "loca";
                entry.Type = BinaryArchive.EntryType.List;
                entry.File = world.GetAsByteArray();
                bar.Entries.Add(entry);
            }

            byte[] byteFile = bar.GetAsByteArray();
            Array.Resize(ref byteFile, byteFile.Length + FINAL_PADDING);
            return byteFile;
        }
    }
}
