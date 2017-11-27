using System.Diagnostics;

namespace Tera.Game.Messages
{
    public class SCreatureChangeHp : ParsedMessage
    {
        internal SCreatureChangeHp(TeraMessageReader reader) : base(reader)
        {
            if (reader.Version < 321550 || reader.Version > 321600)
            {
                HpRemaining = reader.ReadInt64();
                TotalHp = reader.ReadInt64();
                HpChange = reader.ReadInt64();
            }
            else
            {
                HpRemaining = reader.ReadInt32();
                TotalHp = reader.ReadInt32();
                HpChange = reader.ReadInt32();
            }
            Type = reader.ReadInt32();
            //Unknow3 = reader.ReadInt16();
            TargetId = reader.ReadEntityId();
            SourceId = reader.ReadEntityId();
            Critical = reader.ReadByte();
            AbnormalId = reader.ReadInt32();
            //Debug.WriteLine("target = " + TargetId + ";Source:" + SourceId + ";Critical:" + Critical + ";Hp left:" + HpRemaining + ";Max HP:" + TotalHp+";HpLost/Gain:"+ HpChange + ";Type:"+ Type + ";dot:"+AbnormalId);
        }

        public int Unknow3 { get; }
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