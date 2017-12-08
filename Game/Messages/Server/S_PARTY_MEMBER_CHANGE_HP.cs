namespace Tera.Game.Messages
{
    public class SPartyMemberChangeHp : ParsedMessage
    {
        internal SPartyMemberChangeHp(TeraMessageReader reader) : base(reader)
        {
            ServerId = reader.ReadUInt32();
            PlayerId = reader.ReadUInt32();
            HpRemaining = reader.ReadInt64();
            TotalHp = reader.ReadInt64();
            // Debug.WriteLine("target = " + TargetId + ";Hp left:" + HpRemaining + ";Max HP:" + TotalHp + ");
        }

        public int Unknow3 { get; }

        public long HpRemaining { get; }

        public long TotalHp { get; }

        public uint ServerId { get; }
        public uint PlayerId { get; }
        public bool Slaying => TotalHp > HpRemaining*2 && HpRemaining > 0;
    }
}