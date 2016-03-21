namespace Tera.Game.Messages
{
    public class SPartyMemberCharmEnable : ParsedMessage
    {
        internal SPartyMemberCharmEnable(TeraMessageReader reader) : base(reader)
        {
            ServerId = reader.ReadUInt32();
            PlayerId = reader.ReadUInt32();
            CharmId = reader.ReadUInt32();
        }

        public uint ServerId { get; }
        public uint PlayerId { get; }
        public uint CharmId { get; }
    }
}