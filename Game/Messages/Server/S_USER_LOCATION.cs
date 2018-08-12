namespace Tera.Game.Messages
{
    public class S_USER_LOCATION : ParsedMessage
    {
        internal S_USER_LOCATION(TeraMessageReader reader) : base(reader)
        {
            Entity = reader.ReadEntityId();
            Start = reader.ReadVector3f();
            Heading = reader.ReadAngle();
            HeadingLook = reader.Factory.ReleaseVersion < 6200 ? Heading : reader.ReadAngle();//not sure when it appeared
            Speed = reader.ReadInt16();
            Finish = reader.ReadVector3f();
            Ltype = reader.ReadInt32();
            InShuttle = reader.ReadBoolean();
//            Debug.WriteLine($"{Time.Ticks} {BitConverter.ToString(BitConverter.GetBytes(Entity.Id))}: {Start} {Heading} -> {Finish}, S:{Speed} ,{Ltype} {unk1} {unk2}" );
        }

        public bool InShuttle { get; set; }
        public Angle HeadingLook { get; set; }
        public EntityId Entity { get; }
        public Vector3f Start { get; private set; }
        public Angle Heading { get; private set; }
        public short Speed { get; private set; }
        public Vector3f Finish { get; private set; }
        public int Ltype { get; private set; }
    }
}