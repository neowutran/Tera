namespace Tera.Game.Messages
{
    public class StartUserProjectileServerMessage : ParsedMessage
    {
        internal StartUserProjectileServerMessage(TeraMessageReader reader)
            : base(reader)
        {
            OwnerId = reader.ReadEntityId();
            reader.Skip(4);
            Id = reader.ReadEntityId();
            SkillId = new SkillId(reader).Id;
            Start = reader.ReadVector3f();
            Finish = reader.ReadVector3f();
            Speed = reader.ReadSingle();
            //Debug.WriteLine($"{Time.Ticks} {BitConverter.ToString(BitConverter.GetBytes(Id.Id))} {Start} - > {Finish} {Speed} {SkillId}");
        }

        public float Speed { get; set; }
        public Vector3f Finish { get; set; }
        public Vector3f Start { get; set; }
        public int SkillId { get; set; }
        public EntityId Id { get; private set; }
        public EntityId OwnerId { get; private set; }
    }
}