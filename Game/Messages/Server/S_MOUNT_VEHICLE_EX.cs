namespace Tera.Game.Messages
{
    public class S_MOUNT_VEHICLE_EX : ParsedMessage
    {
        internal S_MOUNT_VEHICLE_EX(TeraMessageReader reader)
            : base(reader)
        {
            Owner = reader.ReadEntityId();
            Id = reader.ReadEntityId();
        }

        public EntityId Id { get; }
        public EntityId Owner { get; }
    }
}