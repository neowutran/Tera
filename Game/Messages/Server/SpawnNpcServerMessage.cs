// Copyright (c) Gothos
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Tera.Game.Messages
{
    public class SpawnNpcServerMessage : ParsedMessage
    {
        internal SpawnNpcServerMessage(TeraMessageReader reader)
            : base(reader)
        {
            reader.Skip(10);
            Id = reader.ReadEntityId();
            TargetId = reader.ReadEntityId();
            Position = reader.ReadVector3f();
            Heading = reader.ReadAngle();
            reader.Skip(4);
            NpcId = reader.ReadUInt32();
            NpcArea = reader.ReadUInt16();
            CategoryId = reader.ReadUInt32();
            reader.Skip(31);
            OwnerId = reader.ReadEntityId();
        }

        public EntityId Id { get; }
        public EntityId OwnerId { get; }
        public EntityId TargetId { get; }
        public Vector3f Position { get; }
        public Angle Heading { get; }
        public uint NpcId { get; }
        public ushort NpcArea { get; }
        public uint CategoryId { get; }
    }
}