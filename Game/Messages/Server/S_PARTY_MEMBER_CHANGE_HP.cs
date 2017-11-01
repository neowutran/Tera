namespace Tera.Game.Messages
{
    public class SPartyMemberChangeHp : ParsedMessage
    {
        internal SPartyMemberChangeHp(TeraMessageReader reader) : base(reader)
        {
            ServerId = reader.ReadUInt32();
            PlayerId = reader.ReadUInt32();
            HpRemaining = reader.ReadInt32();
            if (reader.Version < 321150) reader.Skip(4);
            TotalHp = reader.ReadInt32();
            // Debug.WriteLine("target = " + TargetId + ";Hp left:" + HpRemaining + ";Max HP:" + TotalHp + ");
        }

        public int Unknow3 { get; }

        public int HpRemaining { get; }

        public int TotalHp { get; }

        public uint ServerId { get; }
        public uint PlayerId { get; }
        public bool Slaying => TotalHp > HpRemaining*2 && HpRemaining > 0;
    }
}