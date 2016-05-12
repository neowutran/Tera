namespace Tera.Game.Messages
{
    public class SpawnProjectileServerMessage : ParsedMessage
    {
        internal SpawnProjectileServerMessage(TeraMessageReader reader)
            : base(reader)
        {
            Id = reader.ReadEntityId();
            reader.Skip(4);
            Model = reader.ReadInt32();
            Start = reader.ReadVector3f();
            Finish = reader.ReadVector3f();
            unk1 = reader.ReadByte();
            unk2 = reader.ReadInt32();
            OwnerId = reader.ReadEntityId();
            Speed = reader.ReadInt16();// ???
        }

        public int Speed { get; set; }
        public int unk2 { get; private set; }
        public byte unk1 { get; private set; }
        public EntityId Id { get; private set; }
        public int Model { get; private set; }
        public Vector3f Start { get; private set; }
        public Vector3f Finish { get; private set; }
        public EntityId OwnerId { get; private set; }
    }
}