namespace Tera.Game.Messages
{
    public class S_PLAYER_STAT_UPDATE : ParsedMessage
    {
        internal S_PLAYER_STAT_UPDATE(TeraMessageReader reader) : base(reader)
        {
            HpRemaining = reader.ReadInt32();
            MpRemaining = reader.ReadInt32();
            reader.Skip(4);
            TotalHp = reader.ReadInt32();
            TotalMp = reader.ReadInt32();
            /*
            BasePower = reader.ReadInt32();
            BaseEndurance = reader.ReadInt32();
            BaseImpactFactor = reader.ReadInt32();
            BaseBalanceFactor = reader.ReadInt32();
            BaseMovementSpeed = reader.ReadInt16();
            reader.Skip(2);
            BaseAttackSpeed = reader.ReadInt16();
            BaseCritRate = reader.ReadSingle();
            BaseCritResist = reader.ReadSingle();
            BaseCritPower = reader.ReadSingle();
            BaseAttack = reader.ReadInt32();
            BaseAttack2 = reader.ReadInt32();
            BaseDefence = reader.ReadInt32();
            BaseImpcat = reader.ReadInt32();
            BaseBalance = reader.ReadInt32();
            BaseResistWeakening = reader.ReadSingle();
            BaseResistPeriodic = reader.ReadSingle();
            BaseResistStun = reader.ReadSingle();
            BonusPower = reader.ReadInt32();
            BonusEndurance = reader.ReadInt32();
            BonusImpactFactor = reader.ReadInt32();
            BonusBalanceFactor = reader.ReadInt32();
            BonusMovementSpeed = reader.ReadInt16();
            reader.Skip(2);
            BonusAttackSpeed = reader.ReadInt16();
            BonusCritRate = reader.ReadSingle();
            BonusCritResist = reader.ReadSingle();
            BonusCritPower = reader.ReadSingle();
            BonusAttack = reader.ReadInt32();
            BonusAttack2 = reader.ReadInt32();
            BonusDefence = reader.ReadInt32();
            BonusImpcat = reader.ReadInt32();
            BonusBalance = reader.ReadInt32();
            BonusResistWeakening = reader.ReadSingle();
            BonusResistPeriodic = reader.ReadSingle();
            BonusResistStun = reader.ReadSingle();
            Level = reader.ReadInt32();
            Vitality = reader.ReadInt16();
            Status = reader.ReadByte();
            BonusHp = reader.ReadInt32();
            BonusMp = reader.ReadInt32();
            Stamina = reader.ReadInt32();
            TotalStamina = reader.ReadInt32();
            ReRemaining = reader.ReadInt32();
            TotalRe = reader.ReadInt32();
            BonusRe = reader.ReadInt32();
            reader.Skip(4);
            ItemLevelInventory = reader.ReadInt32();
            ItemLevel = reader.ReadInt32();
            Edge = reader.ReadInt32();
            reader.Skip(16);
            FlightEnergy = reader.ReadSingle();
            */
            // Something else unknown later
        }

        public bool Slaying => TotalHp > HpRemaining * 2 && HpRemaining > 0;
        public int BaseAttack { get; }
        public int BaseAttack2 { get; }
        public short BaseAttackSpeed { get; }
        public int BaseBalance { get; }
        public int BaseBalanceFactor { get; }
        public float BaseCritPower { get; }
        public float BaseCritRate { get; }
        public float BaseCritResist { get; }
        public int BaseDefence { get; }
        public int BaseEndurance { get; }
        public int BaseImpactFactor { get; }
        public int BaseImpcat { get; }
        public short BaseMovementSpeed { get; }
        public int BasePower { get; }
        public float BaseResistPeriodic { get; }
        public float BaseResistStun { get; }
        public float BaseResistWeakening { get; }
        public int BonusAttack { get; }
        public int BonusAttack2 { get; }
        public short BonusAttackSpeed { get; }
        public int BonusBalance { get; }
        public int BonusBalanceFactor { get; }
        public float BonusCritPower { get; }
        public float BonusCritRate { get; }
        public float BonusCritResist { get; }
        public int BonusDefence { get; }
        public int BonusEndurance { get; }
        public int BonusHp { get; }
        public int BonusImpactFactor { get; }
        public int BonusImpcat { get; }
        public short BonusMovementSpeed { get; }
        public int BonusMp { get; }
        public int BonusPower { get; }
        public float BonusResistPeriodic { get; }
        public float BonusResistStun { get; }
        public float BonusResistWeakening { get; }
        public int HpRemaining { get; }
        public int ItemLevel { get; }
        public int ItemLevelInventory { get; }
        public int Level { get; }
        public int MpRemaining { get; }
        public int ReRemaining { get; }
        public int Stamina { get; }
        public byte Status { get; }
        public int TotalHp { get; }
        public int TotalMp { get; }
        public int TotalRe { get; }
        public int BonusRe { get; }
        public int TotalStamina { get; }
        public int Vitality { get; }
        public int Edge { get; }
        public float FlightEnergy { get; }
    }
}