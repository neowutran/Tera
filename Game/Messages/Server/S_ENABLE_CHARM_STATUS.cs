namespace Tera.Game.Messages
{
    public class SEnableCharmStatus : ParsedMessage
    {
        internal SEnableCharmStatus(TeraMessageReader reader) : base(reader)
        {
            TargetId = reader.ReadEntityId();
        }

        public EntityId TargetId { get; }
    }
}