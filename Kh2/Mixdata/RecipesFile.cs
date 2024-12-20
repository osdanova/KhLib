using KhLib.Kh2.Structs;
using Xe.BinaryMapper;

namespace KhLib.Kh2.Mixdata
{
    public class RecipesFile : BaseTableFile<RecipesFile.Entry>
    {
        public RecipesFile() : base(4, 4, 3, "MIRE", 4, 16) { }

        public static RecipesFile Read(byte[] byteFile)
        {
            RecipesFile file = new RecipesFile();
            file.ReadFile(byteFile);

            return file;
        }

        // MixRecipeDataHead | MixRecipeDataInfo
        public class Entry
        {
            [Data] public ushort RecipeId { get; set; }
            [Data] public UnlockType UnlockType { get; set; }
            [Data] public Rank Rank { get; set; }
            [Data] public ushort Item1Id { get; set; }
            [Data] public ushort Item2Id { get; set; }
            [Data] public ushort Material1Id { get; set; }
            [Data] public ushort Material1Count { get; set; }
            [Data] public ushort Material2Id { get; set; }
            [Data] public ushort Material2Count { get; set; }
            [Data] public ushort Material3Id { get; set; }
            [Data] public ushort Material3Count { get; set; }
            [Data] public ushort Material4Id { get; set; }
            [Data] public ushort Material4Count { get; set; }
            [Data] public ushort Material5Id { get; set; }
            [Data] public ushort Material5Count { get; set; }
            [Data] public ushort Material6Id { get; set; }
            [Data] public ushort Material6Count { get; set; }
        }

        public enum UnlockType : byte
        {
            Recipe,
            FreeDev1,
            FreeDev2,
            FreeDev3
        }

        public enum Rank : byte
        {
            C,
            B,
            A,
            S
        }
    }
}
