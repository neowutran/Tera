namespace Tera.Game.Messages
{
    public class SEnableCharmStatus : ParsedMessage
    {
        internal SEnableCharmStatus(TeraMessageReader reader) : base(reader)
        {
            CharmId = reader.ReadUInt32();
        }

        public uint CharmId { get; }
    }
}