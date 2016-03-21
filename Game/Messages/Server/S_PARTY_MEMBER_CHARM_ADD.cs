namespace Tera.Game.Messages
{
    public class SPartyMemberCharmAdd : ParsedMessage
    {
        internal SPartyMemberCharmAdd(TeraMessageReader reader) : base(reader)
        {
            ServerId = reader.ReadUInt32();
            PlayerId = reader.ReadUInt32();
            CharmId = reader.ReadUInt32();
            Duration = reader.ReadInt32();
            Status = reader.ReadByte();
        }

        public uint ServerId { get; }
        public uint PlayerId { get; }
        public uint CharmId { get; }
        public byte Status { get; }
        public int Duration { get; }
    }
}