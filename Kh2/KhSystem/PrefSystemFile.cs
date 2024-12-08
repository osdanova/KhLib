using Xe.BinaryMapper;

namespace KhLib.Kh2.KhSystem
{
    public class PrefSystemFile
    {
        /******************************************
         * Properties
         ******************************************/
        public Entry SystemSettings = new Entry();

        /******************************************
         * Constructors
         ******************************************/
        public PrefSystemFile()
        {
        }

        /******************************************
         * Functions - Static
         ******************************************/
        // NOTE: Like the other files in pref this file uses pointers, but it onlly contains 1 entry so I'm not bothering with pointers
        public static PrefSystemFile Read(byte[] byteFile)
        {
            PrefSystemFile file = new PrefSystemFile();

            using (MemoryStream stream = new MemoryStream(byteFile))
            {
                stream.Position = 8; // pointer count + pointer to single entry
                file.SystemSettings = BinaryMapping.ReadObject<Entry>(stream);
            }

            return file;
        }

        /******************************************
         * Functions - Local
         ******************************************/
        public byte[] GetAsByteArray()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using(BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write((int)1);
                    writer.Write((int)8);
                    BinaryMapping.WriteObject<Entry>(stream, SystemSettings);
                }
                return stream.ToArray();
            }
        }

        // PREF_SYSTEM
        public class Entry
        {
            [Data] public float CeilingStop { get; set; }
            [Data] public float CeilingDisableCommandTime { get; set; }
            [Data] public float HangRangeH { get; set; }
            [Data] public float HangRangeL { get; set; }
            [Data] public float HangRangeXZ { get; set; }
            [Data] public float FallMax { get; set; }
            [Data] public float BlowBrakeXZ { get; set; }
            [Data] public float BlowMinXZ { get; set; }
            [Data] public float BlowBrakeUp { get; set; }
            [Data] public float BlowUp { get; set; }
            [Data] public float BlowSpeed { get; set; }
            [Data] public float BlowToHitBack { get; set; }
            [Data] public float HitBack { get; set; }
            [Data] public float HitBackSmall { get; set; }
            [Data] public float HitBackToJump { get; set; }
            [Data] public float FlyBlowBrake { get; set; }
            [Data] public float FlyBlowStop { get; set; }
            [Data] public float FlyBlowUpAdjust { get; set; }
            [Data] public float MagicJump { get; set; }
            [Data] public float LockOnRange { get; set; }
            [Data] public float LockOnReleaseRange { get; set; }
            [Data] public float StunRecov { get; set; }
            [Data] public float StunRecovHp { get; set; }
            [Data] public float StunRelax { get; set; }
            [Data] public float DriveZako { get; set; }
            [Data] public float ChangeTimeZako { get; set; }
            [Data] public float DriveTime { get; set; }
            [Data] public float DriveTimeRelax { get; set; }
            [Data] public float ChangeTimeAddRate { get; set; }
            [Data] public float ChangeTimeSubRate { get; set; }
            [Data] public float MpDriveRate { get; set; }
            [Data] public float MpToMpDrive { get; set; }
            [Data] public float SummonTimeRelax { get; set; }
            [Data] public float SummonPrayTime { get; set; }
            [Data] public float SummonPrayTimeSkip { get; set; }
            [Data] public int AntiFormDriveCount { get; set; }
            [Data] public int AntiFormSubCount { get; set; }
            [Data] public float AntiFormDamageRate { get; set; }
            [Data] public float FinalFormRate { get; set; }
            [Data] public float FinalFormMulRate { get; set; }
            [Data] public float FinalFormMaxRate { get; set; }
            [Data] public int FinalFormSubCount { get; set; }
            [Data] public float AttackDistToSpeed { get; set; }
            [Data] public float AlCarpetDashInner { get; set; }
            [Data] public float AlCarpetDashDelay { get; set; }
            [Data] public float AlCarpetDashAccel { get; set; }
            [Data] public float AlCarpetDashBrake { get; set; }
            [Data] public float LkDashDriftInner { get; set; }
            [Data] public float LkDashDriftTime { get; set; }
            [Data] public float LkDashAccelDrift { get; set; }
            [Data] public float LkDashAccelStop { get; set; }
            [Data] public float LkDashDriftSpeed { get; set; }
            [Data] public float LkMagicJump { get; set; }
            [Data] public float MickeyChargeWait { get; set; }
            [Data] public float MickeyDownRate { get; set; }
            [Data] public float MickeyMinRate { get; set; }
            [Data] public float LmSwimSpeed { get; set; }
            [Data] public float LmSwimControl { get; set; }
            [Data] public float LmSwimAccel { get; set; }
            [Data] public float LmDolphinAccel { get; set; }
            [Data] public float LmDolphinSpeedMax { get; set; }
            [Data] public float LmDolphinSpeedMin { get; set; }
            [Data] public float LmDolphinSpeedMaxDist { get; set; }
            [Data] public float LmDolphinSpeedMinDist { get; set; }
            [Data] public float LmDolphinRotMax { get; set; }
            [Data] public float LmDolphinRotDist { get; set; }
            [Data] public float LmDolphinRotMaxDist { get; set; }
            [Data] public float LmDolphinDistToTime { get; set; }
            [Data] public float LmDolphinTimeMax { get; set; }
            [Data] public float LmDolphinTimeMin { get; set; }
            [Data] public float LmDolphinNearSpeed { get; set; }
            [Data] public int DriveBerserkAttack { get; set; }
            [Data] public float MpHaste { get; set; }
            [Data] public float MpHasra { get; set; }
            [Data] public float MpHasga { get; set; }
            [Data] public float DrawRange { get; set; }
            [Data] public int ComboDamageUp { get; set; }
            [Data] public int ReactionDamageUp { get; set; }
            [Data] public float DamageDrive { get; set; }
            [Data] public float DriveBoost { get; set; }
            [Data] public float FormBoost { get; set; }
            [Data] public float ExpChance { get; set; }
            [Data] public int Defender { get; set; }
            [Data] public int ElementUp { get; set; }
            [Data] public float DamageAspil { get; set; }
            [Data] public float HyperHeal { get; set; }
            [Data] public float CombiBoost { get; set; }
            [Data] public float PrizeUp { get; set; }
            [Data] public float LuckUp { get; set; }
            [Data] public int ItemUp { get; set; }
            [Data] public float AutoHeal { get; set; }
            [Data] public float SummonBoost { get; set; }
            [Data] public float DriveConvert { get; set; }
            [Data] public float DefenceMaster { get; set; }
            [Data] public int DefenceMasterRatio { get; set; }
        }

    }
}
