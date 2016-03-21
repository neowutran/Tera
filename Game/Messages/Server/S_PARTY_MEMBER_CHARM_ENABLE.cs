namespace Tera.Game.Messages
{
    public class SPartyMemberCharmEnable : ParsedMessage
    {
        internal SPartyMemberCharmEnable(TeraMessageReader reader) : base(reader)
        {
            TargetId = reader.ReadEntityId();
            CharmId = reader.ReadUInt32();
        }

        public EntityId TargetId { get; }
        public uint CharmId { get; }
    }
}