namespace Tera.Game.Messages
{
    public class StartUserProjectileServerMessage : ParsedMessage
    {
        internal StartUserProjectileServerMessage(TeraMessageReader reader)
            : base(reader)
        {
            OwnerId = reader.ReadEntityId();
            reader.Skip(8);
            Id = reader.ReadEntityId();
            SkillId = reader.ReadUInt32();
            Start = reader.ReadVector3f();
            SAngle = reader.ReadAngle();
            Finish = reader.ReadVector3f();
            FAngle = reader.ReadAngle();
        }

        public Angle FAngle { get; set; }
        public Vector3f Finish { get; set; }
        public Angle SAngle { get; set; }
        public Vector3f Start { get; set; }
        public uint SkillId { get; set; }
        public EntityId Id { get; private set; }
        public EntityId OwnerId { get; private set; }
    }
}