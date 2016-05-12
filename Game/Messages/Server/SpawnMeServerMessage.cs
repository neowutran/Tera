namespace Tera.Game.Messages
{
    public class SpawnMeServerMessage : ParsedMessage
    {
        internal SpawnMeServerMessage(TeraMessageReader reader)
            : base(reader)
        {
            Id = reader.ReadEntityId();
            Position = reader.ReadVector3f();
            Heading = reader.ReadAngle();
            Alive = (reader.ReadByte() & 1) == 1;
            unk1 = reader.ReadByte();
        }

        public byte unk1 { get; set; }
        public bool Alive { get; set; }
        public Angle Heading { get; set; }
        public Vector3f Position { get; set; }
        public EntityId Id { get; private set; }
    }
}