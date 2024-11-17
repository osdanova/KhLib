using System.Drawing;
using System.Numerics;
using Xe.BinaryMapper;

namespace KhLib.Ps2EmotionEngine
{
    public class EmotionEngineUtils
    {
        /*
         * Data is packaged in DMA code to be sent by the DMAC (Data Memory Access Controller).
         * The data contained is vector data packaged as VIF code.
         * The VIF (Vector Interface) unpacks this data for the VU (Vector Unit) to process
         * 
         * EE (Emotion Engine) => The EE is the main CPU of the PS2. Also known as the R5900, it is a custom MIPS core designed by Sony and Toshiba.
         * Scratchpad (SPRAM) => 16KB of memory integrated directly into the EmotionEngine
         * DMA (Direct Memory Access) => Circuits that work in parallel to the CPU to send data to and from peripherals
         * DMAC (DMA Controller) => Connects the PS2's 32 MB of Main Memory (RAM) to various peripheral interfaces, such as VIF (VPU), SIF (IOP), GIF (GS), and IPU (mpeg decoder). Each peripheral (VIF, GIF, SIF, IPU, etc) has a 128 or 256 byte FIFO
         * GIF (Graphical Interface) => Transmits graphical data to the GS (Graphic synthesizer, draws polygons)
         * VIF (Vector Interface) => Decompresses vector data, uploads microprograms to the VUs, and sends graphical data to the GIF
         * VU0/1 (Vector Units) => Custom DSPs used to process vertex data, physics calculations, and other related tasks
         */

        /* A mesh is contained within a DmaVifMesh.
         * Due to memory limitations the mesh is split into multiple smaller DMA packets which together form a DMA chain.
         * Each DMA packet contains the DMA tags for the controller, the VIF code to be processed and the bone list that it uses.
         */
        public class DmaVifMesh
        {
            // Packed
            public List<DmaPacket> DmaChain { get; set; }
            // Unpacked
            public List<VifVertex> Vertices { get; set; }
            public List<List<int>> Faces { get; set; }

            public DmaVifMesh()
            {
                DmaChain = new List<DmaPacket>();
            }

            public void ProcessDmaVifMesh()
            {
                Vertices = new List<VifVertex>();
                List<VifTriFlag> triFlags = new List<VifTriFlag>();
                for (int i = 0; i < DmaChain.Count; i++)
                {
                    DmaPacket dmaPacket = DmaChain[i];
                    if (dmaPacket.UnpackedVifMesh == null)
                    {
                        dmaPacket.UnpackedVifMesh = ProcessVifSkeletal(dmaPacket.VifCode);
                    }

                    // Set bones for weights
                    int currentVertexCount = 0;
                    for (int j = 0; j < dmaPacket.UnpackedVifMesh.WeightVertexCounts.Count; j++)
                    {
                        for (int k = 0; k < dmaPacket.UnpackedVifMesh.WeightVertexCounts[j]; k++)
                        {
                            if (dmaPacket.UnpackedVifMesh.Header.IsSingleWeight)
                            {
                                dmaPacket.UnpackedVifMesh.PositionsV3[currentVertexCount + k].BoneIndex = dmaPacket.BoneIndices[j];
                            }
                            else
                            {
                                dmaPacket.UnpackedVifMesh.PositionsV4[currentVertexCount + k].BoneIndex = dmaPacket.BoneIndices[j];
                            }
                        }
                        currentVertexCount += dmaPacket.UnpackedVifMesh.WeightVertexCounts[j];
                    }

                    // Vertices
                    for (int j = 0; j < dmaPacket.UnpackedVifMesh.PositionIndices.Count; j++)
                    {
                        VifVertex vertex = new VifVertex();
                        vertex.UvCoordinates = dmaPacket.UnpackedVifMesh.UvCoordinates[j];

                        if(dmaPacket.UnpackedVifMesh.Colors.Count > 0)
                        {
                            vertex.Color = Color.FromArgb(dmaPacket.UnpackedVifMesh.Colors[j].A, dmaPacket.UnpackedVifMesh.Colors[j].R, dmaPacket.UnpackedVifMesh.Colors[j].G, dmaPacket.UnpackedVifMesh.Colors[j].B);
                        }

                        if (dmaPacket.UnpackedVifMesh.Normals.Count > 0)
                        {
                            vertex.Normal = new Vector3(dmaPacket.UnpackedVifMesh.Normals[j].X, dmaPacket.UnpackedVifMesh.Normals[j].Y, dmaPacket.UnpackedVifMesh.Normals[j].Z);
                        }

                        vertex.Weights = new List<VifWeight>();
                        if (dmaPacket.UnpackedVifMesh.Header.IsSingleWeight)
                        {
                            VifWeight weight = new VifWeight();
                            VifV3 position = dmaPacket.UnpackedVifMesh.PositionsV3[dmaPacket.UnpackedVifMesh.PositionIndices[j]];
                            weight.Position = new Vector3(position.X, position.Y, position.Z);
                            weight.BoneIndex = position.BoneIndex;
                            weight.Weight = 1;
                            vertex.Weights.Add(weight);
                        }
                        else
                        {
                            List<int> weightIndices = dmaPacket.UnpackedVifMesh.GetWeights(dmaPacket.UnpackedVifMesh.PositionIndices[j]);
                            foreach(int weightIndex in weightIndices)
                            {
                                VifV4 vifWeight = dmaPacket.UnpackedVifMesh.PositionsV4[weightIndex];
                                VifWeight weight = new VifWeight();
                                weight.Position = new Vector3(vifWeight.X, vifWeight.Y, vifWeight.Z);
                                weight.BoneIndex = vifWeight.BoneIndex;
                                weight.Weight = vifWeight.W;
                                vertex.Weights.Add(weight);
                            }
                        }
                        Vertices.Add(vertex);
                    }
                    // Tri Flags
                    triFlags.AddRange(dmaPacket.UnpackedVifMesh.TriFlags);
                }

                // Faces
                Faces = new List<List<int>>();
                for (int j = 0; j < triFlags.Count; j++)
                {
                    VifTriFlag flag = triFlags[j];
                    if (flag == VifTriFlag.TRIANGLE ||
                        flag == VifTriFlag.TRIANGLE_DOUBLE_SIDE)
                    {
                        Faces.Add(new List<int> { j, j - 2, j - 1 });
                    }
                    if (flag == VifTriFlag.TRIANGLE_INVERTED ||
                        flag == VifTriFlag.TRIANGLE_DOUBLE_SIDE)
                    {
                        Faces.Add(new List<int> { j, j - 1, j - 2 });
                    }
                }
            }
        }

        public class DmaPacket
        {
            public byte[] VifCode { get; set; }
            public SimpleDmaTag HeaderTag { get; set; }
            public List<SimpleDmaTag> DmaTags { get; set; }
            public List<int> BoneIndices { get; set; }
            public VifMeshKh2Skeletal UnpackedVifMesh { get; set; }

            public DmaPacket()
            {
                DmaTags = new List<SimpleDmaTag>();
                BoneIndices = new List<int>();
            }
        }

        public static DmaVifMesh ReadDmaVifMesh(Stream stream, int baseOffset, uint dmaOffset, uint dmaTagCount, uint? boneMatrixOffset)
        {
            if (dmaTagCount < 1)
            {
                throw new System.Exception("DMA Packet is empty");
            }

            DmaVifMesh mesh = new DmaVifMesh();

            using (BinaryReader reader = new BinaryReader(stream))
            {
                // DMA Tags
                stream.Position = baseOffset + dmaOffset;
                DmaPacket currentPacket = new DmaPacket();
                for (int i = 0; i < dmaTagCount; i++)
                {
                    SimpleDmaTag currentTag = BinaryMapping.ReadObject<SimpleDmaTag>(stream);
                    // Header tag
                    if (currentTag.Qwc != 0 && currentTag.Address != 0 && currentTag.Data[0] == 0)
                    {
                        currentPacket = new DmaPacket();
                        currentPacket.HeaderTag = currentTag;
                    }
                    // End tag (Not stored)
                    else if (currentTag.Qwc == 0)
                    {
                        mesh.DmaChain.Add(currentPacket);
                    }
                    // Data tag
                    else
                    {
                        currentPacket.DmaTags.Add(currentTag);
                    }
                }

                // VIF code
                foreach (DmaPacket packet in mesh.DmaChain)
                {
                    int vifCodeLength = packet.HeaderTag.Qwc * 16;
                    packet.VifCode = new byte[vifCodeLength];
                    stream.Position = baseOffset + packet.HeaderTag.Address;
                    stream.Read(packet.VifCode, 0, vifCodeLength);
                }

                // Bone indices
                if (boneMatrixOffset != null)
                {
                    stream.Position = baseOffset + boneMatrixOffset.Value;
                    int count = reader.ReadInt32();
                    int currentPacketIndex = 0;
                    for (int i = 0; i < count; i++)
                    {
                        int boneIndex = reader.ReadInt32();
                        if (boneIndex == -1)
                        {
                            currentPacketIndex++;
                        }
                        else
                        {
                            mesh.DmaChain[currentPacketIndex].BoneIndices.Add(boneIndex);
                        }
                    }
                }
            }

            return mesh;
        }

        /******************************************
        * DMA
        ******************************************/
        // Dma Tag structure simplified for Kh2
        public class SimpleDmaTag
        {
            [Data] public ushort Qwc { get; set; } // Quadword Count. How much data is in the packet, measured in quadwords (16 bytes)
            [Data] public ushort Param { get; set; }
            [Data] public int Address { get; set; } //  For models either memory location or bone matrix data
            [Data(Count = 8)] public byte[] Data { get; set; }
        }
        // Real Dma Tag
        // 0-15    QWC to transfer
        // 16-25   Unused
        // 26-27   Priority control
        //         0=No effect
        //         1=Reserved
        //         2=Priority control disabled(D_PCR.31 = 0)
        //         3=Priority control enabled(D_PCR.31 = 1)
        // 28-30   Tag ID
        // 31      IRQ
        // 32-62   ADDR field(lower 4 bits must be zero)
        // 63      Memory selection for ADDR(0=RAM, 1=scratchpad)
        // 64-127  Data to transfer(only if Dn_CHCR.TTE==1)

        public enum DmaOps : byte
        {
            NOP = 0x0,
            STCYCL = 0x1,
            OFFSET = 0x2,
            BASE = 0x3,
            ITOP = 0x4,
            STMOD = 0x5,
            MSKPATH3 = 0x6,
            MARK = 0x7,
            FLUSHE = 0x10,
            FLUSHH = 0x11,
            FLUSHA = 0x13,
            MSCAL = 0x14,
            MSCALF = 0x15,
            MSCNT = 0x17,
            STMASK = 0x20,
            STROW = 0x30,
            STCOL = 0x31,
            MPG = 0x4A,
            DIRECT = 0x50,
            DIRECTHL = 0x51,
            UNPACK_60 = 0x60,
            UNPACK_7F = 0x7F,
        }

        /******************************************
        * VIF
        ******************************************/

        public static uint VIF_OP_UNPACK = 0x01000101;

        public const uint VIF_OP_SET_MASK = 0x20000000;
        public const uint VIF_OP_INDICES_MASK = 0xCFCFCFCF;
        public const uint VIF_OP_FLAGS_MASK = 0x3F3F3F3F;
        public const uint VIF_OP_NORMALS_MASK = 0xC0C0C0C0;
        public const uint VIF_OP_NORMALS_RESERVE_MASK = 0x3F3F3F3F;
        public const uint VIF_OP_COORDS_MASK = 0x80808080;

        public const uint VIF_OP_SET_COLUMN = 0x31000000;

        // struct SKL_HEADER
        // Offsets are for offsets in memory in QWC
        public class VifHeaderSkeletal
        {
            [Data] public int Type { get; set; }
            [Data] public int VertexColorPtrInc { get; set; }
            [Data] public int MagicNumber { get; set; }
            [Data] public int VertexBufferPointer { get; set; }
            [Data] public int TriStripNodeCount { get; set; }
            [Data] public int TriStripNodeOffset { get; set; }
            [Data] public int MatrixCountOffset { get; set; }
            [Data] public int MatrixOffset { get; set; }
            [Data] public int ColorCount { get; set; }
            [Data] public int ColorOffset { get; set; }
            [Data] public int WeightGroupCount { get; set; }
            [Data] public int WeightGroupCountOffset { get; set; }
            [Data] public int VertexCoordCount { get; set; }
            [Data] public int VertexCoordOffset { get; set; }
            [Data] public int VertexIndexOffset { get; set; }
            [Data] public int MatrixCount { get; set; }
            public int NormalCount { get; set; }
            public int NormalOffset { get; set; }

            public bool HasNormals
            {
                get
                {
                    return (VertexCoordOffset != TriStripNodeOffset + TriStripNodeCount + ColorCount);
                }
            }

            public bool IsSingleWeight
            {
                get
                {
                    return WeightGroupCount <= 1;
                }
            }
        }

        // Count: Amount of items
        // Size: Item size in bits
        //public enum UnpackCountSize : byte
        //{
        //    C1_S32 = 0x60,
        //    C1_S16 = 0x61,
        //    C1_S8  = 0x62,
        //
        //    C2_S32 = 0x64,
        //    C2_S16 = 0x65,
        //    C2_S8  = 0x66,
        //
        //    C3_S32 = 0x68,
        //    C3_S16 = 0x69,
        //    C3_S8  = 0x6A,
        //
        //    C4_S32 = 0x6C,
        //    C4_S16 = 0x6D,
        //    C4_S8  = 0x6E,
        //    C4_S5  = 0x6F, // (5+5+5+1)
        //
        //    // With Mask (Fourth Bit)
        //    C1_S32_M = 0x70,
        //    C1_S16_M = 0x71,
        //    C1_S8_M  = 0x72,
        //    C2_S32_M = 0x74,
        //    C2_S16_M = 0x75,
        //    C2_S8_M  = 0x76,
        //}

        public class VifUnpackTag
        {
            [Data] public byte Address { get; set; }
            [Data] public byte Options { get; set; }
            [Data] public byte Count { get; set; }
            [Data] public byte Size { get; set; }
        }

        public class VifUv
        {
            [Data] public short U { get; set; }
            [Data] public short V { get; set; }

            public override string ToString()
            {
                return "<" + U + ", " + V + ">";
            }
        }

        public class VifColor
        {
            [Data] public byte R { get; set; }
            [Data] public byte G { get; set; }
            [Data] public byte B { get; set; }
            [Data] public byte A { get; set; }

            public override string ToString()
            {
                return "<" + R + ", " + G + ", " + B + ", " + A + ">";
            }
        }

        public class VifV3
        {
            [Data] public float X { get; set; }
            [Data] public float Y { get; set; }
            [Data] public float Z { get; set; }
            public int BoneIndex { get; set; }

            public override string ToString()
            {
                return "["+BoneIndex+"] <" + X + ", " + Y + ", " + Z + ">";
            }
        }
        public class VifV4
        {
            [Data] public float X { get; set; }
            [Data] public float Y { get; set; }
            [Data] public float Z { get; set; }
            [Data] public float W { get; set; }
            public int BoneIndex { get; set; }

            public override string ToString()
            {
                return "["+BoneIndex+"] <"+X+", "+Y+ ", "+Z+", "+W+">";
            }
        }

        public enum VifTriFlag : byte
        {
            TRIANGLE_DOUBLE_SIDE = 0x00,
            VERTEX = 0x10,
            TRIANGLE = 0x20,
            TRIANGLE_INVERTED = 0x30
        }

        public enum VifUnpackOptions : byte
        {
            UNPACK_OPTIONS_SIGN = 0x80, // First bit: TOPS is added to starting address (File address + unpack address)
            UNPACK_OPTIONS_ZERO = 0xC0  // Second bit: Zero extended (If unset sign extended)
        }

        public enum VifUnpackSize : byte
        {
            READ_SIZE_16 = 0x6C,
            READ_SIZE_12 = 0x78,
            READ_SIZE_4 = 0x65,
            READ_SIZE_4_2 = 0x6E,
            READ_SIZE_1 = 0x72
        }

        public class VifVertex
        {
            public List<VifWeight> Weights { get; set; }
            public VifUv UvCoordinates { get; set; }
            public Color? Color { get; set; }
            public Vector3? Normal { get; set; }
        }

        public class VifWeight
        {
            public Vector3 Position { get; set; }
            public int BoneIndex { get; set; }
            public float Weight { get; set; }
        }

        // Unpacked VIF data
        public class VifMeshKh2Skeletal
        {
            public VifHeaderSkeletal Header {  get; set; }
            public List<byte> PositionIndices { get; set; } // One per vertex. Point to Positions V3/4 if single or weightGroups if multi
            public List<VifUv> UvCoordinates { get; set; } // // One per vertex. X out of 4096.0f
            public List<VifTriFlag> TriFlags { get; set; } // One per vertex. 
            public List<VifColor> Colors { get; set; } // One per vertex. 
            public List<VifV3> Normals { get; set; } // One per vertex. 
            public List<byte> NormalReserve { get; set; } // Purpose unknown
            public List<VifV3> PositionsV3 { get; set; } // Ordered/grouped by bone assignment
            public List<VifV4> PositionsV4 { get; set; }
            public List<int> WeightVertexCounts { get; set; } // Amount of Positions assigned to the bone in that position (First entry is how many Positions apply to the first bone in DmaPacket.BoneIndices)
            public List<List<List<int>>> WeightGroups { get; set; } // [0] are assignments to a single bone, [1] are assignments to 2 bones (2 indices each), etc... | These point to Positions V3/4

            public VifMeshKh2Skeletal()
            {
                PositionIndices = new List<byte>();
                UvCoordinates = new List<VifUv>();
                TriFlags = new List<VifTriFlag>();
                Colors = new List<VifColor>();
                Normals = new List<VifV3>();
                NormalReserve = new List<byte>();
                PositionsV3 = new List<VifV3>();
                PositionsV4 = new List<VifV4>();
                WeightVertexCounts = new List<int>();
                WeightGroups = new List<List<List<int>>>();
            }

            public List<int> GetWeights(int index)
            {
                int tempIndex = index;
                int currentList = 0;
                while (tempIndex >= WeightGroups[currentList].Count)
                {
                    tempIndex -= WeightGroups[currentList].Count;
                    currentList++;
                }
                return WeightGroups[currentList][tempIndex];
            }
        }

        public static VifMeshKh2Skeletal ProcessVifSkeletal(byte[] vifCode)
        {
            VifMeshKh2Skeletal vifMesh = new VifMeshKh2Skeletal();

            using (MemoryStream stream = new MemoryStream(vifCode))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    CheckVifOp(reader, VIF_OP_UNPACK);
                    VifUnpackTag headerTag = BinaryMapping.ReadObject<VifUnpackTag>(stream);
                    VifHeaderSkeletal headerSkeletal = BinaryMapping.ReadObject<VifHeaderSkeletal>(stream);

                    // Extra header: Normals
                    if (headerSkeletal.HasNormals)
                    {
                        headerSkeletal.NormalCount = reader.ReadInt32();
                        headerSkeletal.NormalOffset = reader.ReadInt32();
                        stream.Position += 8;
                    }

                    // UV
                    List<VifUv> uvs = new List<VifUv>();
                    CheckVifOp(reader, VIF_OP_UNPACK);
                    VifUnpackTag uvTag = BinaryMapping.ReadObject<VifUnpackTag>(stream);
                    for(int i = 0; i < uvTag.Count; i++)
                    {
                        uvs.Add(BinaryMapping.ReadObject<VifUv>(stream));
                    }

                    // Position indices
                    CheckVifOp(reader, VIF_OP_SET_MASK);
                    CheckVifOp(reader, VIF_OP_INDICES_MASK);
                    CheckVifOp(reader, VIF_OP_UNPACK);
                    List<byte> positionIndices = new List<byte>();
                    VifUnpackTag positionIndicesTag = BinaryMapping.ReadObject<VifUnpackTag>(stream);
                    for (int i = 0; i < positionIndicesTag.Count; i++)
                    {
                        positionIndices.Add(reader.ReadByte());
                    }
                    CommonUtils.AlignStreamPositionToBytes(stream, 4);

                    // Tri flags
                    CheckVifOp(reader, VIF_OP_SET_MASK);
                    CheckVifOp(reader, VIF_OP_FLAGS_MASK);
                    CheckVifOp(reader, VIF_OP_UNPACK);
                    VifUnpackTag triTag = BinaryMapping.ReadObject<VifUnpackTag>(stream);
                    List<VifTriFlag> triFlags = new List<VifTriFlag>();
                    for (int i = 0; i < triTag.Count; i++)
                    {
                        triFlags.Add((VifTriFlag)reader.ReadByte());
                    }
                    CommonUtils.AlignStreamPositionToBytes(stream, 4);

                    // Colors
                    List<VifColor> colors = new List<VifColor>();
                    if(headerSkeletal.ColorCount > 0)
                    {
                        CheckVifOp(reader, VIF_OP_UNPACK);
                        VifUnpackTag colorTag = BinaryMapping.ReadObject<VifUnpackTag>(stream);
                        for (int i = 0; i < colorTag.Count; i++)
                        {
                            colors.Add(BinaryMapping.ReadObject<VifColor>(stream));
                        }
                    }

                    // Normals
                    List<VifV3> normals = new List<VifV3>();
                    List<byte> normalReserve = new List<byte>(); // Should be all 0s
                    if (headerSkeletal.HasNormals)
                    {
                        CheckVifOp(reader, VIF_OP_SET_MASK);
                        CheckVifOp(reader, VIF_OP_NORMALS_MASK);
                        CheckVifOp(reader, VIF_OP_UNPACK);
                        VifUnpackTag normalTag = BinaryMapping.ReadObject<VifUnpackTag>(stream);
                        for (int i = 0; i < normalTag.Count; i++)
                        {
                            normals.Add(BinaryMapping.ReadObject<VifV3>(stream));
                        }

                        CheckVifOp(reader, VIF_OP_SET_MASK);
                        CheckVifOp(reader, VIF_OP_NORMALS_RESERVE_MASK);
                        CheckVifOp(reader, VIF_OP_UNPACK);
                        VifUnpackTag normalReserveTag = BinaryMapping.ReadObject<VifUnpackTag>(stream);
                        for (int i = 0; i < normalTag.Count; i++)
                        {
                            normalReserve.Add(reader.ReadByte());
                        }
                        CommonUtils.AlignStreamPositionToBytes(stream, 4);
                    }

                    if (headerSkeletal.IsSingleWeight)
                    {
                        CheckVifOp(reader, VIF_OP_SET_COLUMN);
                        VifV4 singleWeightV4 = BinaryMapping.ReadObject<VifV4>(stream); // All 1s
                        CheckVifOp(reader, VIF_OP_SET_MASK);
                        CheckVifOp(reader, VIF_OP_COORDS_MASK);
                    }

                    // Positions
                    List<VifV3> positionsV3 = new List<VifV3>();
                    List<VifV4> positionsV4 = new List<VifV4>();
                    CheckVifOp(reader, VIF_OP_UNPACK);
                    VifUnpackTag positionTag = BinaryMapping.ReadObject<VifUnpackTag>(stream);
                    if ((headerSkeletal.IsSingleWeight && positionTag.Size != (byte)VifUnpackSize.READ_SIZE_12) ||
                        (!headerSkeletal.IsSingleWeight && positionTag.Size != (byte)VifUnpackSize.READ_SIZE_16))
                    {
                        throw new System.Exception("VIF read error: Weight count detection missmatched!");
                    }
                    for (int i = 0; i < positionTag.Count; i++)
                    {
                        if(positionTag.Size == (byte)VifUnpackSize.READ_SIZE_12)
                        {
                            positionsV3.Add(BinaryMapping.ReadObject<VifV3>(stream));
                        }
                        else if(positionTag.Size == (byte)VifUnpackSize.READ_SIZE_16)
                        {
                            positionsV4.Add(BinaryMapping.ReadObject<VifV4>(stream));
                        }
                    }

                    // Bone counts
                    List<int> weightVertexCounts = new List<int>();
                    List<List<List<int>>> weightGroups = new List<List<List<int>>>();
                    // [0] are assignments to a single weight, [1] are assignments to 2 weight (2 indices each), etc...
                    // [1][0] is the first weight group for 2-weight assignments
                    // [1][0][1] is the second of the 2 weigths in that group
                    CheckVifOp(reader, VIF_OP_UNPACK);
                    VifUnpackTag boneVertexCountTag = BinaryMapping.ReadObject<VifUnpackTag>(stream);
                    for (int i = 0; i < headerSkeletal.MatrixCount; i++)
                    {
                        weightVertexCounts.Add(reader.ReadInt32());
                    }
                    // These paddings are to align the loaded data to a qwd in memory
                    int padding = CommonUtils.GetHowManyBytesToAlignTo(headerSkeletal.MatrixCount * 4, 16);
                    stream.Position += padding;
                    if (!headerSkeletal.IsSingleWeight)
                    {
                        List<int> weightGroupCounts = new List<int>(); // [0] are amount of assignment to a single bone, [1] are amount of assignments to 2 bones, etc...
                        for (int i = 0; i < headerSkeletal.WeightGroupCount; i++)
                        {
                            weightGroupCounts.Add(reader.ReadInt32());
                        }
                        padding = CommonUtils.GetHowManyBytesToAlignTo(headerSkeletal.WeightGroupCount * 4, 16);
                        stream.Position += padding;
                        for (int i = 0; i < weightGroupCounts.Count; i++) // i = How many assignments are in this group (Group being 1 bone, 2 bones, etc...)
                        {
                            int weightGroupCount = weightGroupCounts[i];
                            int bonesAssignedInThisGroup = i + 1;
                            int groupIntCount = 0;
                            List<List<int>> weightList = new List<List<int>>(); // Each list has "bonesAssignedInThisGroup" bone assignments
                            for (int j = 0; j < weightGroupCount; j++) // j = Amount of assignments in the group
                            {
                                List<int> weights = new List<int>();
                                for(int k = 0; k < bonesAssignedInThisGroup; k++) // k = Bone indices
                                {
                                    weights.Add(reader.ReadInt32());
                                    groupIntCount++;
                                }
                                weightList.Add(weights);
                            }
                            weightGroups.Add(weightList);
                            padding = CommonUtils.GetHowManyBytesToAlignTo(groupIntCount * 4, 16);
                            stream.Position += padding;
                        }
                    }

                    vifMesh.Header = headerSkeletal;
                    vifMesh.PositionIndices = positionIndices;
                    vifMesh.UvCoordinates = uvs;
                    vifMesh.TriFlags = triFlags;
                    vifMesh.Colors = colors;
                    vifMesh.Normals = normals;
                    vifMesh.NormalReserve = normalReserve;
                    vifMesh.PositionsV3 = positionsV3;
                    vifMesh.PositionsV4 = positionsV4;
                    vifMesh.WeightVertexCounts = weightVertexCounts;
                    vifMesh.WeightGroups = weightGroups;
                }
            }

            return vifMesh;
        }

        // Reads the next operation and makes sure it is the one given
        private static void CheckVifOp(BinaryReader reader, uint expectedOpCode)
        {
            uint op = reader.ReadUInt32();
            if (op != expectedOpCode)
            {
                throw new System.Exception("VIF read error: " + expectedOpCode + " operation expected but read: " + op);
            }
        }

        // Returns the amount of 4-byte entries left to fill the last group when grouping the entries in groups of the given size
        public static int getAmountRemainingIfGroupedBy(int entryCount, int groupSize)
        {
            int overflow = entryCount % groupSize;

            if (overflow == 0)
                return 0;

            return groupSize - overflow;
        }
        
    }
}
