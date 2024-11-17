using Xe.BinaryMapper;
using static KhLib.Kh2.Model.ModelSkeletal;

namespace KhLib.Kh2.Model
{
    /******************************************
     * Helper classes for reading and writing
     ******************************************/
    public class ModelSkeletalBin
    {
        // struct ModelSKLRAW
        public class BIN_ModelSkeletalHeader
        {
            [Data] public ushort BoneCount { get; set; }
            [Data] public ushort TextureCount { get; set; }
            [Data] public uint BoneOffset { get; set; }
            [Data] public uint SkeletonDataOffset { get; set; }
            [Data] public int MeshCount { get; set; }
        }

        // struct ModelHRC
        public class BIN_Bone
        {
            [Data] public short Index { get; set; }
            [Data] public short SiblingIndex { get; set; }
            [Data] public short ParentIndex { get; set; }
            [Data] public short ChildIndex { get; set; }
            [Data] public int Reserved { get; set; }
            [Data] public int Flags { get; set; }
            [Data] public float ScaleX { get; set; }
            [Data] public float ScaleY { get; set; }
            [Data] public float ScaleZ { get; set; }
            [Data] public float ScaleW { get; set; }
            [Data] public float RotationX { get; set; }
            [Data] public float RotationY { get; set; }
            [Data] public float RotationZ { get; set; }
            [Data] public float RotationW { get; set; }
            [Data] public float TranslationX { get; set; }
            [Data] public float TranslationY { get; set; }
            [Data] public float TranslationZ { get; set; }
            [Data] public float TranslationW { get; set; }

            /* Bone BitfLags:
             * no_envelop
             * not_joint > On when the bone has no rigged vertices
             * The rest is unused
             */

            public override string ToString()
            {
                return "[" + Index + "|" + ParentIndex + "] <" + RotationX + "," + RotationY + "," + RotationZ + "> <" + TranslationX + "," + TranslationY + "," + TranslationZ + ">";
            }
        }
        // struct ModelSKLRAWBone
        public class BIN_SkeletonData
        {
            [Data] public float BoundingBoxMinX { get; set; }
            [Data] public float BoundingBoxMinY { get; set; }
            [Data] public float BoundingBoxMinZ { get; set; }
            [Data] public float BoundingBoxMinW { get; set; }
            [Data] public float BoundingBoxMaxX { get; set; }
            [Data] public float BoundingBoxMaxY { get; set; }
            [Data] public float BoundingBoxMaxZ { get; set; }
            [Data] public float BoundingBoxMaxW { get; set; }
            [Data] public float InverseKinematicsBoneBiasX { get; set; }
            [Data] public float InverseKinematicsBoneBiasY { get; set; }
            [Data] public float InverseKinematicsBoneBiasZ { get; set; }
            [Data] public float InverseKinematicsBoneBiasW { get; set; }
            [Data] public BoneReference BoneReferences { get; set; }
            [Data] public float DistanceFromSkeletonX { get; set; }
            [Data] public float DistanceFromSkeletonY { get; set; }
            [Data] public float DistanceFromSkeletonZ { get; set; }
            [Data] public float DistanceFromSkeletonW { get; set; }
        }

        //public class BIN_SkeletalGroup
        //{
        //    public BIN_SkeletalGroupHeader Header { get; set; }
        //    public byte[] VifData { get; set; }
        //    public List<DmaPacket> DmaData { get; set; }
        //    public List<int> BoneMatrix { get; set; }
        //    public SkeletalMesh Mesh { get; set; }
        //
        //    public SkeletalGroup()
        //    {
        //        Header = new SkeletalGroupHeader();
        //        DmaData = new List<DmaPacket>();
        //        BoneMatrix = new List<int>();
        //    }
        //}

        // struct ModelSKLRAWGroup
        public class BIN_MeshHeader
        {
            [Data] public int AttributesBitfield { get; set; }
            [Data] public uint TextureIndex { get; set; }
            [Data] public uint PolygonCount { get; set; }
            [Data] public ushort HasVertexBuffer { get; set; }
            [Data] public ushort Alt { get; set; }
            [Data] public uint DmaPacketOffset { get; set; } // packettag
            [Data] public uint BoneMatrixOffset { get; set; } // matrixinfo_offset
            [Data] public uint PacketLength { get; set; } // In QW (16bytes)
            [Data] public uint Reserved { get; set; } // Padding
        }
    }
}
