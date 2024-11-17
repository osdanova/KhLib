using KhLib.Kh2.Utils;
using KhLib.Ps2EmotionEngine;
using System.Numerics;
using Xe.BinaryMapper;
using static KhLib.Kh2.Model.ModelCommon;
using static KhLib.Kh2.Model.ModelSkeletalBin;

namespace KhLib.Kh2.Model
{
    public class ModelSkeletal
    {
        public ModelWrapperHeader WrapperHeader { get; set; }
        public List<ModelSkeletalMesh> Meshes { get; set; }
        public SkeletonData SkelData { get; set; }
        public List<Bone> Bones { get; set; }

        // ModelSKLRAW
        // ModelSKLRAWGroup
        // ModelSKLRAWAttribute
        // ModelMultiRAW


        

        /******************************************
         * Functions - Static
         ******************************************/
        public static ModelSkeletal Read(byte[] byteFile)
        {
            ModelSkeletal model = new ModelSkeletal();

            using (MemoryStream stream = new MemoryStream(byteFile))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    stream.Position += ReservedAreaSize;

                    BIN_ModelWrapperHeader binModelWrapperHeader = BinaryMapping.ReadObject<BIN_ModelWrapperHeader>(stream);
                    BIN_ModelSkeletalHeader binSkeletalModelHeader = BinaryMapping.ReadObject<BIN_ModelSkeletalHeader>(stream);

                    List<BIN_MeshHeader> binMeshHeaders = new List<BIN_MeshHeader>();
                    for (int i = 0; i < binSkeletalModelHeader.MeshCount; i++)
                    {
                        binMeshHeaders.Add(BinaryMapping.ReadObject<BIN_MeshHeader>(stream));
                    }

                    BIN_SkeletonData binSkeletonData = BinaryMapping.ReadObject<BIN_SkeletonData>(stream);

                    List<BIN_Bone> binBones = new List<BIN_Bone>();
                    for (int i = 0; i < binSkeletalModelHeader.BoneCount; i++)
                    {
                        binBones.Add(BinaryMapping.ReadObject<BIN_Bone>(stream));
                    }

                    // Load structures
                    model.WrapperHeader = new ModelWrapperHeader();
                    model.WrapperHeader.Type = (ModelType)binModelWrapperHeader.Type;
                    model.WrapperHeader.Subtype = (ModelSubtype)binModelWrapperHeader.Subtype;
                    model.WrapperHeader.Attributes = binModelWrapperHeader.Attributes;

                    model.Meshes = new List<ModelSkeletalMesh>();
                    foreach (BIN_MeshHeader binMesh in binMeshHeaders)
                    {
                        ModelSkeletalMesh mesh = new ModelSkeletalMesh();
                        mesh.AttributesBitfield = binMesh.AttributesBitfield;
                        mesh.TextureIndex = binMesh.TextureIndex;
                        mesh.HasVertexBuffer = binMesh.HasVertexBuffer;
                        mesh.Alt = binMesh.Alt;
                        model.Meshes.Add(mesh);

                        EmotionEngineUtils.DmaVifMesh packagedMesh = EmotionEngineUtils.ReadDmaVifMesh(stream, ReservedAreaSize, binMesh.DmaPacketOffset, binMesh.PacketLength, binMesh.BoneMatrixOffset);
                        //EmotionEngineUtils.VifMeshKh2Skeletal vifMesh = EmotionEngineUtils.ProcessVifSkeletal(packagedMesh.DmaChain[0].VifCode);
                        packagedMesh.ProcessDmaVifMesh();
                    }

                    model.SkelData = new SkeletonData();
                    model.SkelData.BoundingBoxMin = new Vector4(binSkeletonData.BoundingBoxMinX, binSkeletonData.BoundingBoxMinY, binSkeletonData.BoundingBoxMinZ, binSkeletonData.BoundingBoxMinW);
                    model.SkelData.BoundingBoxMax = new Vector4(binSkeletonData.BoundingBoxMaxX, binSkeletonData.BoundingBoxMaxY, binSkeletonData.BoundingBoxMaxZ, binSkeletonData.BoundingBoxMaxW);
                    model.SkelData.InverseKinematicsBoneBias = new Vector4(binSkeletonData.InverseKinematicsBoneBiasX, binSkeletonData.InverseKinematicsBoneBiasY, binSkeletonData.InverseKinematicsBoneBiasZ, binSkeletonData.InverseKinematicsBoneBiasW);
                    model.SkelData.BoneReferences = binSkeletonData.BoneReferences;
                    model.SkelData.DistanceFromSkeleton = new Vector4(binSkeletonData.DistanceFromSkeletonX, binSkeletonData.DistanceFromSkeletonY, binSkeletonData.DistanceFromSkeletonZ, binSkeletonData.DistanceFromSkeletonW);

                    model.Bones = new List<Bone>();
                    foreach (BIN_Bone binBone in binBones)
                    {
                        Bone bone = new Bone();
                        bone.Index = binBone.Index;
                        bone.SiblingIndex = binBone.SiblingIndex;
                        bone.ParentIndex = binBone.ParentIndex;
                        bone.ChildIndex = binBone.ChildIndex;
                        bone.Flags = binBone.Flags;
                        bone.Scale = new Vector4(binBone.ScaleX, binBone.ScaleY, binBone.ScaleZ, binBone.ScaleW);
                        bone.Rotation = new Vector4(binBone.RotationX, binBone.RotationY, binBone.RotationZ, binBone.RotationW);
                        bone.Translation = new Vector4(binBone.TranslationX, binBone.TranslationY, binBone.TranslationZ, binBone.TranslationW);
                        model.Bones.Add(bone);
                    }
                }
            }
            return model;
        }

        /******************************************
         * Subclasses
         ******************************************/

        public class ModelSkeletalMesh
        {
            public int AttributesBitfield { get; set; }
            public uint TextureIndex { get; set; }
            public ushort HasVertexBuffer { get; set; }
            public ushort Alt { get; set; }
            public byte[] VifPacket { get; set; }

            public bool DrawAlphaPhase
            {
                get => BitUtils.Int.GetBit(AttributesBitfield, 0);
                set => AttributesBitfield = (int)BitUtils.Int.SetBit(AttributesBitfield, 0, value);
            }
            public bool Alpha
            {
                get => BitUtils.Int.GetBit(AttributesBitfield, 1);
                set => AttributesBitfield = (int)BitUtils.Int.SetBit(AttributesBitfield, 1, value);
            }
            public bool Multi
            {
                get => BitUtils.Int.GetBit(AttributesBitfield, 2);
                set => AttributesBitfield = (int)BitUtils.Int.SetBit(AttributesBitfield, 2, value);
            }
            public int Part
            {
                get => BitUtils.Int.GetBits(AttributesBitfield, 6, 10);
                set => AttributesBitfield = BitUtils.Int.SetBits(AttributesBitfield, 6, 10, value);
            }
            public int Mesh
            {
                get => BitUtils.Int.GetBits(AttributesBitfield, 11, 15);
                set => AttributesBitfield = BitUtils.Int.SetBits(AttributesBitfield, 11, 15, value);
            }
            public int Priority
            {
                get => BitUtils.Int.GetBits(AttributesBitfield, 16, 20);
                set => AttributesBitfield = BitUtils.Int.SetBits(AttributesBitfield, 16, 20, value);
            }
            public bool AlphaEx
            {
                get => BitUtils.Int.GetBit(AttributesBitfield, 21);
                set => AttributesBitfield = (int)BitUtils.Int.SetBit(AttributesBitfield, 21, value);
            }
            public int UVScroll
            {
                get => BitUtils.Int.GetBits(AttributesBitfield, 22, 26);
                set => AttributesBitfield = BitUtils.Int.SetBits(AttributesBitfield, 22, 26, value);
            }
            public bool AlphaAdd
            {
                get => BitUtils.Int.GetBit(AttributesBitfield, 27);
                set => AttributesBitfield = (int)BitUtils.Int.SetBit(AttributesBitfield, 27, value);
            }
            public bool AlphaSub
            {
                get => BitUtils.Int.GetBit(AttributesBitfield, 28);
                set => AttributesBitfield = (int)BitUtils.Int.SetBit(AttributesBitfield, 28, value);
            }
            public bool Specular
            {
                get => BitUtils.Int.GetBit(AttributesBitfield, 29);
                set => AttributesBitfield = (int)BitUtils.Int.SetBit(AttributesBitfield, 29, value);
            }
            public bool NoLight
            {
                get => BitUtils.Int.GetBit(AttributesBitfield, 30);
                set => AttributesBitfield = (int)BitUtils.Int.SetBit(AttributesBitfield, 30, value);
            }
        }

        public class SkeletonData
        {
            public Vector4 BoundingBoxMin { get; set; }
            public Vector4 BoundingBoxMax { get; set; }
            public Vector4 InverseKinematicsBoneBias { get; set; }
            public BoneReference BoneReferences { get; set; }
            public Vector4 DistanceFromSkeleton { get; set; }
        }

        // struct PART_NAME
        // Ids of the bones
        public class BoneReference
        {
            [Data] public int HEAD { get; set; }
            [Data] public int RF_HEAD { get; set; }
            [Data] public int LB_HEAD { get; set; }
            [Data] public int RB_HEAD { get; set; }
            [Data] public int NECK { get; set; }
            [Data] public int RF_NECK { get; set; }
            [Data] public int LB_NECK { get; set; }
            [Data] public int RB_NECK { get; set; }
            [Data] public int CHEST { get; set; }
            [Data] public int RF_CHEST { get; set; }
            [Data] public int LB_CHEST { get; set; }
            [Data] public int RB_CHEST { get; set; }
            [Data] public int HIP { get; set; }
            [Data] public int RF_HIP { get; set; }
            [Data] public int LB_HIP { get; set; }
            [Data] public int RB_HIP { get; set; }
            [Data] public int COLLAR { get; set; }
            [Data] public int RF_COLLAR { get; set; }
            [Data] public int LB_COLLAR { get; set; }
            [Data] public int RB_COLLAR { get; set; }
            [Data] public int UPARM { get; set; }
            [Data] public int RF_UPARM { get; set; }
            [Data] public int LB_UPARM { get; set; }
            [Data] public int RB_UPARM { get; set; }
            [Data] public int FOARM { get; set; }
            [Data] public int RF_FOARM { get; set; }
            [Data] public int LB_FOARM { get; set; }
            [Data] public int RB_FOARM { get; set; }
            [Data] public int HAND { get; set; }
            [Data] public int RF_HAND { get; set; }
            [Data] public int LB_HAND { get; set; }
            [Data] public int RB_HAND { get; set; }
            [Data] public int FEMUR { get; set; }
            [Data] public int RF_FEMUR { get; set; }
            [Data] public int LB_FEMUR { get; set; }
            [Data] public int RB_FEMUR { get; set; }
            [Data] public int LF_TIBIA { get; set; }
            [Data] public int RF_TIBIA { get; set; }
            [Data] public int LB_TIBIA { get; set; }
            [Data] public int RB_TIBIA { get; set; }
            [Data] public int LF_FOOT { get; set; }
            [Data] public int RF_FOOT { get; set; }
            [Data] public int LB_FOOT { get; set; }
            [Data] public int RB_FOOT { get; set; }
            [Data] public int LF_TOES { get; set; }
            [Data] public int RF_TOES { get; set; }
            [Data] public int LB_TOES { get; set; }
            [Data] public int RB_TOES { get; set; }
            [Data] public int WEAPON_L_LINK { get; set; }
            [Data] public int WEAPON_L { get; set; }
            [Data] public int WEAPON_R_LINK { get; set; }
            [Data] public int WEAPON_R { get; set; }
            [Data] public int SPECIAL0 { get; set; }
            [Data] public int SPECIAL1 { get; set; }
            [Data] public int SPECIAL2 { get; set; }
            [Data] public int SPECIAL3 { get; set; }
            [Data] public int MAX { get; set; }
        }

        public class Bone
        {
            public short Index { get; set; }
            public short SiblingIndex { get; set; } // Calculated ingame
            public short ParentIndex { get; set; }
            public short ChildIndex { get; set; } // Calculated ingame
            public int Flags { get; set; }
            public Vector4 Scale { get; set; }
            public Vector4 Rotation { get; set; }
            public Vector4 Translation { get; set; }

            /* Bone BitfLags:
             * no_envelop
             * not_joint > On when the bone has no rigged vertices
             * The rest is unused
             */

            public override string ToString()
            {
                return "[" + Index + "|" + ParentIndex + "] T" + Translation + " R" + Rotation;
            }
        }
    }
}
