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
            Speed = reader.ReadSingle();
            OwnerId = reader.ReadEntityId();
            unk2 = reader.ReadInt16(); // ???
            //PrintRaw();
            //Debug.WriteLine($"{Time.Ticks} {BitConverter.ToString(BitConverter.GetBytes(Id.Id))} {Start} - > {Finish} {Speed}");
        }

        public float Speed { get; set; }
        public int unk2 { get; }
        public byte unk1 { get; }
        public EntityId Id { get; }
        public int Model { get; }
        public Vector3f Start { get; }
        public Vector3f Finish { get; }
        public EntityId OwnerId { get; }
    }
}