// Copyright (c) Gothos
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Tera.Game.Messages
{
    public class SpawnNpcServerMessage : ParsedMessage
    {
        internal SpawnNpcServerMessage(TeraMessageReader reader)
            : base(reader)
        {
            reader.Skip(reader.Factory.ReleaseVersion >= 7100 ? 10 : 6);//classic staff, not sure when additional array appeared
            Id = reader.ReadEntityId();
            TargetId = reader.ReadEntityId();
            Position = reader.ReadVector3f();
            Heading = reader.ReadAngle();
            reader.Skip(4);
            NpcId = reader.ReadUInt32();
            NpcArea = reader.ReadUInt16();
            reader.Skip(reader.Factory.ReleaseVersion>=7900? 43 : //remainingEnrageTime  
                        reader.Factory.ReleaseVersion >= 6801 ? 39 : 35);//KR added 4 bytes (shapeId)
            OwnerId = reader.ReadEntityId();
        }

        public EntityId Id { get; private set; }
        public EntityId OwnerId { get; private set; }
        public EntityId TargetId { get; private set; }
        public Vector3f Position { get; private set; }
        public Angle Heading { get; private set; }
        public uint NpcId { get; private set; }
        public ushort NpcArea { get; private set; }
    }
}