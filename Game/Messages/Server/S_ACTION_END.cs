using System;

namespace Tera.Game.Messages
{
    public class S_ACTION_END : ParsedMessage
    {
        internal S_ACTION_END(TeraMessageReader reader) : base(reader)
        {
            Entity = reader.ReadEntityId();
            Start = reader.ReadVector3f();
            Heading = reader.ReadAngle();
            Model = reader.ReadUInt32();
            SkillId = reader.ReadUInt32();
            unk = reader.ReadInt32();
            Id = reader.ReadUInt32();
//            Console.WriteLine($"{Time.Ticks} {BitConverter.ToString(BitConverter.GetBytes(Entity.Id))}: {Start} {Heading} -> {Finish}, S:{Speed} ,{Ltype} {unk1} {unk2}" );
        }

        public uint Id { get; set; }
        public int unk { get; set; }
        public uint SkillId { get; set; }
        public uint Model { get; set; }
        public EntityId Entity { get; }
        public Vector3f Start { get; private set; }
        public Angle Heading { get; private set; }
    }
}