namespace Tera.Game.Messages
{
    public class SNpcLocation : ParsedMessage
    {
        internal SNpcLocation(TeraMessageReader reader) : base(reader)
        {
            Entity = reader.ReadEntityId();
            Start = reader.ReadVector3f();
            Heading = reader.ReadAngle();
            Speed = reader.ReadInt16();
            Finish = reader.ReadVector3f();
            Ltype = reader.ReadInt32(); //0 = Move, 7= Rotate standing
//            Debug.WriteLine($"{Time.Ticks} {BitConverter.ToString(BitConverter.GetBytes(Entity.Id))}: {Start} {Heading} -> {Finish}, S:{Speed} ,{Ltype}");
        }

        public EntityId Entity { get; }
        public Vector3f Start { get; }
        public Angle Heading { get; }
        public short Speed { get; }
        public Vector3f Finish { get; }
        public int Ltype { get; }
    }
}