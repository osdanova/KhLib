using Xe.BinaryMapper;

namespace KhLib.Kh2.KhSystem
{
    public class ShopsFile_BIN
    {
        // ShopTblHead
        public class BIN_ShopTableHeader
        {
            [Data] public int Id { get; set; }
            [Data] public short Version { get; set; }
            [Data] public short ShopCount { get; set; }
            [Data] public short InventoryCount { get; set; } //ItemList
            [Data] public short ItemCount { get; set; }
            [Data] public uint LogOffset { get; set; }
        }

        // struct ShopInfo
        public class BIN_Shop
        {
            [Data] public ushort CommandId { get; set; } // Signal - Bitfield?
            [Data] public ushort MenuFlags { get; set; } // The id of the bit/flag to be set for this menu

            [Data] public ushort NameId { get; set; }
            [Data] public ushort ShopkeeperObjectId { get; set; }
            [Data] public ushort PosX { get; set; }
            [Data] public ushort PosY { get; set; }
            [Data] public ushort PosZ { get; set; }
            [Data] public ushort StatusFlags { get; set; } // ushort Stat
            [Data] public ushort InventoryCount { get; set; }
            [Data] public byte ShopId { get; set; }
            [Data] public byte AllItemCount { get; set; }

            [Data] public ushort InvOffset { get; set; }
            [Data] public ushort Padding { get; set; }
        }

        // ShopItemInfo
        public class BIN_Inventory
        {
            [Data] public short UnlockEventId { get; set; } // AddMenuFlag
            [Data] public ushort ProductCount { get; set; }
            [Data] public ushort ProductOffset { get; set; }
            [Data] public ushort Padding { get; set; }
        }
    }
}
