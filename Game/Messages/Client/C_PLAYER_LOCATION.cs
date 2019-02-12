namespace Tera.Game.Messages
{
    public class C_PLAYER_LOCATION : ParsedMessage
    {
        internal C_PLAYER_LOCATION(TeraMessageReader reader) : base(reader)
        {
            Position = reader.ReadVector3f();
            Heading = reader.ReadAngle();
            if (reader.Factory.ReleaseVersion >= 4000) LookDirection = reader.ReadAngle();// wasn't there on classic, but not sure when it's appeared.
            Finish = reader.ReadVector3f();
            Ltype = reader.ReadInt32();
            Speed = reader.ReadInt16();//jumping speed
            InShuttle = reader.ReadBoolean();
            TimeStamp = reader.ReadInt32();
            //Debug.WriteLine($"{Time.Ticks} {Start} {Heading} -> {Finish}, S:{Speed} ,{Ltype} {unk1} {unk2} {TimeStamp}" );
        }

        public int TimeStamp { get; set; }
        public bool InShuttle { get; set; }
        public Angle LookDirection { get; set; }
        public EntityId Entity { get; }
        public Vector3f Position { get; private set; }
        public Angle Heading { get; private set; }
        public short Speed { get; private set; }
        public Vector3f Finish { get; private set; }
        public int Ltype { get; private set; }
            // 0 = running, 1 = walking, 2 = falling, 5 = jumping,
            // 6 = jump intersection and end when something is blocking the path and the player can't
            // travel in the X and Y axis(it will then wait and resume if possible)
            // 7 = stop moving, landing
            // 8 = swimming, 9 = stop swimming, 10 = falling after jumping
    }
}