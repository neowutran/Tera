namespace Tera.Game.Messages
{
    public class SRemoveCharmStatus : ParsedMessage
    {
        internal SRemoveCharmStatus(TeraMessageReader reader) : base(reader)
        {
            CharmId = reader.ReadUInt32();
        }

        public uint CharmId { get; }
    }
}