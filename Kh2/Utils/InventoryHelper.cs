namespace KhLib.Kh2.Utils
{
    public class InventoryHelper
    {
        public static bool GetFlag(ushort item)
        {
            return (item & 0x8000) != 0;
        }
        public static void SetFlag(ref ushort item, bool flag)
        {
            if (flag)
            {
                item |= 0x8000;
            }
            else
            {
                item &= 0x7FFF;
            }
        }

        public static ushort GetId(ushort item)
        {
            return (ushort)(item & 0x7FFF);
        }
        public static void SetId(ref ushort item, ushort id)
        {
            if (id > 0x7FFF)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "ID must be in the range 0 to 32767.");
            }

            item = (ushort)((item & 0x8000) | (id & 0x7FFF)); // Preserve flag, set ID
        }
    }
}
