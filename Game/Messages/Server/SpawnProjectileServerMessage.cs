namespace Tera.Game.Messages
{
    public class SpawnProjectileServerMessage : ParsedMessage
    {
        internal SpawnProjectileServerMessage(TeraMessageReader reader)
            : base(reader)
        {
            Id = reader.ReadEntityId();
            reader.Skip(4);
            SkillId = new SkillId(reader).Id;
            Start = reader.ReadVector3f();
            Finish = reader.ReadVector3f();
            Moving = reader.ReadBoolean();
            Speed = reader.ReadSingle();
            OwnerId = reader.ReadEntityId();
            TemplateId = reader.ReadInt32();
            //PrintRaw();
            //Debug.WriteLine($"{Time.Ticks} {BitConverter.ToString(BitConverter.GetBytes(Id.Id))} {Start} - > {Finish} {Speed}");
        }

        public float Speed { get; set; }
        public int TemplateId { get; private set; }
        public bool Moving { get; private set; }
        public EntityId Id { get; private set; }
        public int SkillId { get; private set; }
        public Vector3f Start { get; private set; }
        public Vector3f Finish { get; private set; }
        public EntityId OwnerId { get; private set; }
    }
}