namespace Tera.Game.Messages
{
    public class S_INSTANT_DASH : ParsedMessage
    {
        internal S_INSTANT_DASH(TeraMessageReader reader) : base(reader)
        {
            Entity = reader.ReadEntityId();
            Target = reader.ReadEntityId();
            if (reader.Factory.ReleaseVersion>=6000)reader.Skip(4);//0, not sure when it was added
            Position = reader.ReadVector3f();
            Heading = reader.ReadAngle();
//            Debug.WriteLine($"{Time.Ticks} {BitConverter.ToString(BitConverter.GetBytes(Entity.Id))}: {Finish} {Heading}");
        }

        public EntityId Entity { get; }
        public EntityId Target { get; }
        public Vector3f Position { get; private set; }
        public Angle Heading { get; private set; }
    }
}