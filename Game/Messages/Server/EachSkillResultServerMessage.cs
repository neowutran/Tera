using System;
using System.Diagnostics;

namespace Tera.Game.Messages
{
    public class EachSkillResultServerMessage : ParsedMessage
    {
        [Flags]
        public enum SkillResultFlags
        {
            Bit0 = 1, // Usually 1 for attacks, 0 for blocks/dodges but I don't understand its exact semantics yet
            Heal = 2, // Bit0 == 1 + heal == 1 = mana
            Bit2 = 4,
            IsDfaResolve = 4,
            Bit16 = 0x10000,
            Bit18 = 0x40000
        } //0 = Hidden, 1 = Damage, 2 = Heal, 3 = MP, bit16+ = DataCenter.NocTanEffectData

        internal EachSkillResultServerMessage(TeraMessageReader reader)
            : base(reader)
        {
            reader.Skip(4);
            Source = reader.ReadEntityId();
            if (reader.Factory.ReleaseVersion >= 7402) {
                var owner = reader.ReadEntityId();
                if (owner.Id != 0) Source = owner;
            } // not sure, when it was added

            Target = reader.ReadEntityId();
            TemplateId = reader.ReadInt32();

            SkillId = new SkillId(reader).Id;
            if (reader.Factory.ReleaseVersion >= 11001 || reader.Factory.ReleaseVersion == 9901 ) {
                var originalSkillId = new SkillId(reader).Id;
                //if (originalSkillId != 0) SkillId = originalSkillId; //not sure what is originalSkillId, but ingame meter use it in this way. todo: look at it later if they implement something new.
                //seems it contains base skill id to simplify official meter DC queries, not needed here.
            }
            HitId = reader.ReadInt32(); //index in TargetingList (NOT id) - See DataCenter.SkillData
            Unknow2 = reader.ReadBytes(12); //index in area, id, time

            Amount = reader.Factory.ReleaseVersion < 6200 ? reader.ReadInt32() : reader.ReadInt64();// KR now use 64 bit
            FlagsDebug = reader.ReadInt32();
            Flags = (SkillResultFlags) FlagsDebug;
            IsCritical = (reader.ReadByte() & 1) != 0;
            ConsumeEdge = (reader.ReadByte() & 1) != 0;
            if (reader.Factory.ReleaseVersion >= 3707) {//brawler stuff
                Blocked = (reader.ReadByte() & 1) != 0; ///SuperArmor
                SuperArmorId = reader.ReadInt32();
                HitCylinderId = reader.ReadInt32();
                reader.Skip(4); // reaction bools: enable,push,air, airchain
            } else reader.Skip(2); //reaction bools: enable, push
            if (reader.Factory.ReleaseVersion>=8600) reader.Skip(2);//SkillDamageType

            Position = reader.ReadVector3f();
            Heading = reader.ReadAngle();
            //if (Position.X!=0)
            //    Debug.WriteLine($"{Time.Ticks} {BitConverter.ToString(BitConverter.GetBytes(Target.Id))} {SkillId} {Position} {Heading}");
        }

        //DEBUG
        public int FlagsDebug { get; }


        public int HitId { get; }

        //DEBUG
        public int TemplateId { get; }
        public int SuperArmorId { get; }
        public int HitCylinderId { get; }

        //DEBUG
        public byte[] Unknow2 { get; }


        public EntityId Source { get; private set; }
        public EntityId Target { get; }
        public long Amount { get; }
        public int SkillId { get; private set; }
        public SkillResultFlags Flags { get; }
        public bool IsCritical { get; private set; }
        public bool ConsumeEdge { get; private set; }
        public bool Blocked { get; private set; }
        public Vector3f Position { get; }
        public Angle Heading { get; }

        public bool IsMana => ((Flags & SkillResultFlags.Bit0) != 0) && ((Flags & SkillResultFlags.Heal) != 0);

        public bool IsHeal => ((Flags & SkillResultFlags.Bit0) == 0) && ((Flags & SkillResultFlags.Heal) != 0);
        public bool IsUseless => (Flags & SkillResultFlags.IsDfaResolve) != 0;
        public bool IsHp => !IsMana;
    }
}