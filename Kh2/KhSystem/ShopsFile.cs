using KhLib.Kh2.Utils;
using Xe.BinaryMapper;
using static KhLib.Kh2.KhSystem.ShopsFile_BIN;

namespace KhLib.Kh2.KhSystem
{
    public class ShopsFile
    {
        /******************************************
         * Properties
         ******************************************/
        public int FileSignature;
        public short Version;

        public List<Shop> Shops { get; set; }

        /******************************************
         * Constructors
         ******************************************/
        public ShopsFile()
        {
            FileSignature = 0x48535A54; // TZSH
            Version = 7;

            Shops = new List<Shop>();
        }

        /******************************************
         * Functions - Static
         ******************************************/
        public static ShopsFile Read(byte[] byteFile)
        {
            ShopsFile file = new ShopsFile();
        
            using (MemoryStream stream = new MemoryStream(byteFile))
            {
                using (BinaryReader br = new BinaryReader(stream))
                {
                    BIN_ShopTableHeader bHeader = BinaryMapping.ReadObject<BIN_ShopTableHeader>(stream);

                    List<BIN_Shop> bShops = new List<BIN_Shop>();
                    for (int i = 0; i < bHeader.ShopCount; i++)
                    {
                        bShops.Add(BinaryMapping.ReadObject<BIN_Shop>(stream));
                    }

                    file.Shops = new List<Shop>();
                    foreach (BIN_Shop bShop in bShops)
                    {
                        Shop shop = new Shop(bShop);
                        shop.Inventories = new List<Inventory>();
                        for (int i = 0; i < bShop.InventoryCount; i++)
                        {
                            stream.Position = bShop.InvOffset + (i * 8);

                            Inventory inventory = new Inventory();
                            BIN_Inventory bInventory = BinaryMapping.ReadObject<BIN_Inventory>(stream);
                            inventory.AddMenuFlagId = bInventory.UnlockEventId;

                            inventory.ItemIds = new List<ushort>();
                            stream.Position = bInventory.ProductOffset;
                            for (int j = 0; j < bInventory.ProductCount; j++)
                            {
                                inventory.ItemIds.Add(br.ReadUInt16());
                            }

                            shop.Inventories.Add(inventory);
                        }

                        file.Shops.Add(shop);
                    }

                    return file;
                }
            }
        }

        /******************************************
         * Functions - Local
         ******************************************/
        public byte[] GetAsByteArray()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(stream))
                {
                    List<ushort> logItems = new List<ushort>();

                    BIN_ShopTableHeader bHeader = new BIN_ShopTableHeader
                    {
                        Id = FileSignature,
                        Version = this.Version,
                        ShopCount = 0,
                        InventoryCount = 0,
                        ItemCount = 0
                    };

                    // Header Counts
                    foreach(Shop shop in Shops)
                    {
                        bHeader.ShopCount++;
                        foreach(Inventory inventory in shop.Inventories)
                        {
                            bHeader.InventoryCount++;
                            bHeader.ItemCount += (short)inventory.ItemIds.Count;
                        }
                    }

                    // Write products and store offsets and counts
                    List<BIN_Shop> bShops = new List<BIN_Shop>();
                    List<BIN_Inventory> bInventories = new List<BIN_Inventory>();
                    stream.Position = 16 + (bHeader.ShopCount * 24) + (bHeader.InventoryCount * 8);
                    ushort inventoryOffset = (ushort)(16 + (bHeader.ShopCount * 24));
                    foreach (Shop shop in Shops)
                    {
                        BIN_Shop bShop = new BIN_Shop
                        {
                            CommandId = shop.GameSignalId,
                            MenuFlags = shop.MenuFlagId,
                            NameId = shop.NameId,
                            ShopkeeperObjectId = shop.ShopkeeperObjectId,
                            PosX = shop.PosX,
                            PosY = shop.PosY,
                            PosZ = shop.PosZ,
                            StatusFlags = (ushort)shop.StatusFlags,
                            InventoryCount = (ushort)shop.Inventories.Count,
                            ShopId = shop.ShopId,
                            AllItemCount = 0,
                            InvOffset = inventoryOffset
                        };
                        bShops.Add(bShop);

                        foreach (Inventory inventory in shop.Inventories)
                        {
                            BIN_Inventory bInventory = new BIN_Inventory
                            {
                                UnlockEventId = inventory.AddMenuFlagId,
                                ProductCount = (ushort)inventory.ItemIds.Count,
                                ProductOffset = (ushort)stream.Position
                            };
                            bInventories.Add(bInventory);

                            foreach (ushort itemId in inventory.ItemIds)
                            {
                                bShop.AllItemCount++;
                                if (!logItems.Contains(itemId))
                                {
                                    logItems.Add(itemId);
                                }
                                bw.Write(itemId);
                            }
                            inventoryOffset += 8;
                        }
                    }

                    bHeader.LogOffset = (ushort)stream.Position;
                    // Log items
                    foreach(ushort logItemId in logItems)
                    {
                        bw.Write(logItemId);
                    }
                    // Possibly not needed but just in case
                    ReadWriteUtils.AlignStreamToByte(stream, 16);

                    // Write headers
                    stream.Position = 0;
                    BinaryMapping.WriteObject<BIN_ShopTableHeader>(stream, bHeader);
                    foreach(BIN_Shop bShop in bShops)
                    {
                        BinaryMapping.WriteObject<BIN_Shop>(stream, bShop);
                    }
                    foreach (BIN_Inventory bInventory in bInventories)
                    {
                        BinaryMapping.WriteObject<BIN_Inventory>(stream, bInventory);
                    }

                    return stream.ToArray();
                }
            }
        }

        // ShopInfo
        public class Shop
        {
            public ushort GameSignalId { get; set; }
            public ushort MenuFlagId { get; set; } // The id of the bit/flag to be set for this menu - Used to check if the shop has been visited
            public ushort NameId { get; set; }
            public ushort ShopkeeperObjectId { get; set; }
            public ushort PosX { get; set; }
            public ushort PosY { get; set; }
            public ushort PosZ { get; set; }
            public ShopType StatusFlags { get; set; }
            public byte ShopId { get; set; }
            public List<Inventory> Inventories { get; set; }
            public bool SellsWeapons
            {
                get => BitFlag.IsFlagSet(StatusFlags, ShopType.sellsWeapons);
                set => StatusFlags = BitFlag.SetFlag(StatusFlags, ShopType.sellsWeapons, value);
            }
            public bool SellsArmors
            {
                get => BitFlag.IsFlagSet(StatusFlags, ShopType.sellsArmors);
                set => StatusFlags = BitFlag.SetFlag(StatusFlags, ShopType.sellsArmors, value);
            }
            public bool SellsAccessories
            {
                get => BitFlag.IsFlagSet(StatusFlags, ShopType.sellsAccessories);
                set => StatusFlags = BitFlag.SetFlag(StatusFlags, ShopType.sellsAccessories, value);
            }
            public bool SellsItems
            {
                get => BitFlag.IsFlagSet(StatusFlags, ShopType.sellsItems);
                set => StatusFlags = BitFlag.SetFlag(StatusFlags, ShopType.sellsItems, value);
            }
            public bool SellsMaterials
            {
                get => BitFlag.IsFlagSet(StatusFlags, ShopType.sellsMaterials);
                set => StatusFlags = BitFlag.SetFlag(StatusFlags, ShopType.sellsMaterials, value);
            }
            public bool IsSpecialtyShop
            {
                get => BitFlag.IsFlagSet(StatusFlags, ShopType.isSpecialtyShop);
                set => StatusFlags = BitFlag.SetFlag(StatusFlags, ShopType.isSpecialtyShop, value);
            }
            public bool IsShop
            {
                get => BitFlag.IsFlagSet(StatusFlags, ShopType.isShop);
                set => StatusFlags = BitFlag.SetFlag(StatusFlags, ShopType.isShop, value);
            }
            public bool IsMoogleWorkshop
            {
                get => BitFlag.IsFlagSet(StatusFlags, ShopType.isMoogleWorkshop);
                set => StatusFlags = BitFlag.SetFlag(StatusFlags, ShopType.isMoogleWorkshop, value);
            }

            public Shop() { }
            public Shop(BIN_Shop bShop)
            {
                GameSignalId = bShop.CommandId;
                MenuFlagId = bShop.MenuFlags;
                NameId = bShop.NameId;
                ShopkeeperObjectId = bShop.ShopkeeperObjectId;
                PosX = bShop.PosX;
                PosY = bShop.PosY;
                PosZ = bShop.PosZ;
                StatusFlags = (ShopType)bShop.StatusFlags;
                ShopId = bShop.ShopId;
            }
        }

        // ShopItemInfo
        public class Inventory
        {
            public short AddMenuFlagId { get; set; }
            public List<ushort> ItemIds { get; set; }

            public Inventory()
            {
                ItemIds = new List<ushort>();
            }
        }

        [Flags]
        public enum ShopType : ushort
        {
            sellsWeapons = 0x01,
            sellsArmors = 0x02,
            sellsAccessories = 0x04,
            sellsItems = 0x08,
            sellsMaterials = 0x10,
            isSpecialtyShop = 0x80,
            isShop = 0x100,
            isMoogleWorkshop = 0x200
        }
    }
}
