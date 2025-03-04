﻿using Xe.BinaryMapper;

namespace KhLib.Kh2.Model
{
    public class ModelCommon
    {
        //-----------
        // STRUCTURES
        //-----------

        public const int ReservedAreaSize = 0x90;

        // struct ModelHeader
        public class ModelWrapperHeader
        {
            public ModelType Type { get; set; }
            public ModelSubtype Subtype { get; set; }
            public int Attributes { get; set; }
        }

        //// DMA packets contain the DMA tag with instructions for the DMAC as well as 2 extra parameters
        //public class DmaPacket
        //{
        //    [Data] public OpenKh.Common.Ps2.DmaTag DmaTag { get; set; }
        //    [Data] public OpenKh.Common.Ps2.VifCode VifCode { get; set; } // Param 0 (VIF code for models)
        //    [Data] public int Parameter { get; set; } // Param 1
        //
        //    public DmaPacket()
        //    {
        //        DmaTag = new Common.Ps2.DmaTag();
        //        VifCode = new Common.Ps2.VifCode();
        //    }
        //
        //    public override string ToString()
        //    {
        //        return (DmaTag.Qwc.ToString("X") + " " + DmaTag.Param.ToString("X") + " " + DmaTag.Address.ToString("X") + " " + VifCode.Cmd.ToString("X") + " " + VifCode.Num.ToString("X") + " " + VifCode.Immediate.ToString("X") + " " + Parameter.ToString("X"));
        //    }
        //}
        //
        //// Represents a vertex with a position in space (Absolute and relative to bones) and a UV coordinate
        //public class UVBVertex
        //{
        //    public Vector3 Position { get; set; } // Absolute position
        //    public List<BPosition> BPositions { get; set; } // Positions relative to bones
        //    public float U { get; set; }
        //    public float V { get; set; }
        //    public VifCommon.VertexColor Color { get; set; }
        //    public VifCommon.VertexNormal Normal { get; set; }
        //
        //    public UVBVertex() { }
        //    public UVBVertex(List<BPosition> BonePositions, float U = 0, float V = 0, Vector3 position = new Vector3(), VifCommon.VertexColor color = null, VifCommon.VertexNormal normal = null)
        //    {
        //        this.BPositions = BonePositions;
        //        this.U = U;
        //        this.V = V;
        //        Position = position;
        //        Color = color;
        //        Normal = normal;
        //    }
        //
        //    public override string ToString()
        //    {
        //        return "[" + BPositions.Count + "] <" + U + "," + V + "> " + Position;
        //    }
        //}
        //// Position relative to bone. The W coordinate represents the weight of the bone.
        //public class BPosition
        //{
        //    public Vector4 Position { get; set; }
        //    public int BoneIndex { get; set; }
        //    public BPosition(Vector4 Position = new Vector4(), int BoneIndex = -1)
        //    {
        //        this.Position = Position;
        //        this.BoneIndex = BoneIndex;
        //    }
        //    public override string ToString()
        //    {
        //        return "[" + BoneIndex + " | " + Position.W + "] <" + Position.X + ", " + Position.Y + ", " + Position.Z + ">";
        //    }
        //}
        //
        ////----------
        //// FUNCTIONS
        ////----------
        //
        //// Returns the ordered bone hierarchy from given bone to root
        //public static List<Bone> getBoneHierarchy(List<Bone> boneList, int boneIndex)
        //{
        //    List<Bone> boneHierarchy = new List<Bone>();
        //
        //    Bone currentBone = boneList[boneIndex];
        //    boneHierarchy.Add(currentBone);
        //
        //    while (currentBone.ParentIndex != null && currentBone.ParentIndex != -1 && !boneHierarchy.Contains(currentBone))
        //    {
        //        currentBone = boneList[currentBone.ParentIndex];
        //        boneHierarchy.Add(currentBone);
        //    }
        //
        //    return boneHierarchy;
        //}
        //// Returns the absolute SRT matrix for each bone
        //public static Matrix4x4[] GetBoneMatrices(List<Bone> boneList)
        //{
        //    Matrix4x4[] matrices = new Matrix4x4[boneList.Count];
        //    /* We compute relative matrices */
        //    for (int i = 0; i < boneList.Count; i++)
        //    {
        //        matrices[i] =
        //            Matrix4x4.CreateScale(boneList[i].ScaleX, boneList[i].ScaleY, boneList[i].ScaleZ) *
        //            Matrix4x4.CreateRotationX(boneList[i].RotationX) *
        //            Matrix4x4.CreateRotationY(boneList[i].RotationY) *
        //            Matrix4x4.CreateRotationZ(boneList[i].RotationZ) *
        //            Matrix4x4.CreateTranslation(boneList[i].TranslationX, boneList[i].TranslationY, boneList[i].TranslationZ);
        //    }
        //    /* We compute absolute matrices */
        //    for (int i = 0; i < boneList.Count; i++)
        //    {
        //        if (boneList[i].ParentIndex > -1)
        //            matrices[i] *= matrices[boneList[i].ParentIndex];
        //    }
        //    /* Done. */
        //    return matrices;
        //}
        //
        //// Returns the absolute position of the vertex given its position relative to bones
        //public static Vector3 getAbsolutePosition(List<BPosition> BPositions, Matrix4x4[] boneMatrices)
        //{
        //    Vector3 finalPos = Vector3.Zero;
        //
        //    foreach (BPosition bonePosition in BPositions)
        //    {
        //        // If only 1 bone is assigned per vertex the code is compressed and W has no value
        //        if (bonePosition.Position.W == 0)
        //        {
        //            bonePosition.Position = new Vector4(bonePosition.Position.X, bonePosition.Position.Y, bonePosition.Position.Z, 1);
        //        }
        //
        //        finalPos += ToVector3(Vector4.Transform(bonePosition.Position, boneMatrices[bonePosition.BoneIndex]));
        //    }
        //
        //    return finalPos;
        //}
        //
        //// Returns the position relative to the bone of the vertex given its absolute position and the weight to the bone
        //public static Vector3 getRelativePosition(Vector3 absolutePosition, Matrix4x4 boneMatrix, float weight)
        //{
        //    Matrix4x4 invertedMatrix = new Matrix4x4();
        //    Matrix4x4.Invert(boneMatrix, out invertedMatrix);
        //
        //    Vector3 relativePosition = Vector3.Transform(absolutePosition, invertedMatrix);
        //
        //    relativePosition *= weight;
        //
        //    return relativePosition;
        //}
        //
        //// Aligns the stream to the given byte
        //public static void alignStreamToByte(Stream stream, int alignByte)
        //{
        //    if (stream.Position % alignByte != 0)
        //    {
        //        byte[] extraBytes = new byte[(alignByte - (stream.Position % alignByte))];
        //        MemoryStream extraStream = new MemoryStream(extraBytes);
        //        extraStream.CopyTo(stream);
        //    }
        //}
        //
        //private static Vector3 ToVector3(Vector4 pos) => new Vector3(pos.X, pos.Y, pos.Z);
        //
        //// Returns the generic DMA packet that ends a DMA chain
        //public static DmaPacket getEndDmaPacket()
        //{
        //    ModelCommon.DmaPacket endDma = new ModelCommon.DmaPacket();
        //    endDma.DmaTag = new Common.Ps2.DmaTag();
        //    endDma.VifCode = new Common.Ps2.VifCode();
        //    endDma.DmaTag.Param = 0x1000;
        //    endDma.VifCode.Immediate = 0x1700;
        //
        //    return endDma;
        //}

        /******************************************
         * Enums
         ******************************************/
        // struct MODEL_TYPE
        public enum ModelType : int
        {
            Multi = 1,
            Background = 2,
            Skeletal = 3,
            Shadow = 4
        }

        // struct MODEL_SUBTYPE
        public enum ModelSubtype : int
        {
            ObjectCharacter_BackgroundMap = 0,
            ObjectBackground_BackgroundSky = 1
        }

        /******************************************
         * Helper classes for reading and writing
         ******************************************/
        // struct ModelHeader
        public class BIN_ModelWrapperHeader
        {
            [Data] public int Type { get; set; }
            [Data] public int Subtype { get; set; }
            [Data] public int Attributes { get; set; }
            [Data] public uint Size { get; set; } // Offset of shadow file. 0 if there are no more files
        }
    }
}
