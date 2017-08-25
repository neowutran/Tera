using System.Diagnostics;

namespace Tera.Game.Messages
{
    public class SCreatureChangeHp : ParsedMessage
    {
        internal SCreatureChangeHp(TeraMessageReader reader) : base(reader)
        {
            if (reader.Version < 319000 || reader.Version > 319900)
            {
                HpRemaining = reader.ReadInt32();
                TotalHp = reader.ReadInt32();
                HpChange = reader.ReadInt32();
            }
            else
            {
                HpRemaining = reader.ReadInt64();
                TotalHp = reader.ReadInt64();
                HpChange = reader.ReadInt64();
            }
            Type = reader.ReadInt32();
            //Unknow3 = reader.ReadInt16();
            TargetId = reader.ReadEntityId();
            SourceId = reader.ReadEntityId();
            Critical = reader.ReadInt16();


            //Debug.WriteLine("target = " + TargetId + ";Source:" + SourceId + ";Critical:" + Critical + ";Hp left:" + HpRemaining + ";Max HP:" + TotalHp+";HpLost/Gain:"+ HpChange + ";Type:"+ Type + ";Unknow3:"+Unknow3);
        }

        public int Unknow3 { get; }
        public long HpChange { get; }

        public int Type { get; }


        public long HpRemaining { get; }

        public long TotalHp { get; }

        public int Critical { get; }


        public EntityId TargetId { get; }
        public EntityId SourceId { get; }
        public bool Slaying => TotalHp > HpRemaining*2 && HpRemaining > 0;
    }
}