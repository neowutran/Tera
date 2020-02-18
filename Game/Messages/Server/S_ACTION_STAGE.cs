namespace Tera.Game.Messages
{
    public class S_ACTION_STAGE : ParsedMessage
    {
        internal S_ACTION_STAGE(TeraMessageReader reader) : base(reader)
        {
            reader.Skip(4); //Effects array count and offset
            Entity = reader.ReadEntityId();
            Position = reader.ReadVector3f();
            Heading = reader.ReadAngle();
            Model = reader.ReadUInt32();
            SkillId = new SkillId(reader).Id;
            Stage = reader.ReadUInt32();
            Speed = reader.ReadSingle();
            if (reader.Factory.ReleaseVersion>=7500)reader.Skip(4);//projectilespeed
            Id = reader.ReadUInt32();
            if (reader.Factory.ReleaseVersion<=7100)return;//classic, not sure when following things were added
            EffectScale = reader.ReadSingle();
            Moving = reader.ReadBoolean();
            Destination = reader.ReadVector3f();
            Target = reader.ReadEntityId();
            //            Debug.WriteLine($"{Time.Ticks} {BitConverter.ToString(BitConverter.GetBytes(Entity.Id))}: {Start} {Heading}, S:{Speed}, {SkillId} {Stage} {Model} {unk} {Id}" );
        }

        public EntityId Entity { get; }
        public Vector3f Position { get; private set; }
        public Angle Heading { get; private set; }
        public uint Model { get; private set; }
        public int SkillId { get; private set; }
        public uint Stage { get; private set; }
        public float Speed { get; private set; }
        public uint Id { get; private set; }
        public float EffectScale { get; set; }
        public bool Moving { get; private set; }
        public Vector3f Destination { get; private set; }
        public EntityId Target { get; private set; }
    }
}