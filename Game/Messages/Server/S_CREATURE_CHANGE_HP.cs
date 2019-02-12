using System.Diagnostics;

namespace Tera.Game.Messages
{
    public class SCreatureChangeHp : ParsedMessage
    {
        internal SCreatureChangeHp(TeraMessageReader reader) : base(reader)
        {
            HpRemaining = reader.Factory.ReleaseVersion < 6200 ? reader.ReadInt32() : reader.ReadInt64();
            TotalHp = reader.Factory.ReleaseVersion < 6200 ? reader.ReadInt32() : reader.ReadInt64();
            HpChange = reader.Factory.ReleaseVersion < 6200 ? reader.ReadInt32() : reader.ReadInt64();
            Type = reader.ReadInt32();
            TargetId = reader.ReadEntityId();
            SourceId = reader.ReadEntityId();
            Critical = reader.ReadByte();
            if (reader.Factory.ReleaseVersion>=6200) AbnormalId = reader.ReadInt32(); // not sure when it was added, wasn't there on classic
            //Debug.WriteLine("target = " + TargetId + ";Source:" + SourceId + ";Critical:" + Critical + ";Hp left:" + HpRemaining + ";Max HP:" + TotalHp+";HpLost/Gain:"+ HpChange + ";Type:"+ Type + ";dot:"+AbnormalId);
        }

        public long HpChange { get; }

        public int Type { get; }


        public long HpRemaining { get; }

        public long TotalHp { get; }

        public int Critical { get; }

        public int AbnormalId { get; }

        public EntityId TargetId { get; }
        public EntityId SourceId { get; }
        public bool Slaying => TotalHp > HpRemaining*2 && HpRemaining > 0;
    }
}