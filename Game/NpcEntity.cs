// Copyright (c) Gothos
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Tera.Game
{
    // NPCs and Mosters - Tera doesn't distinguish these
    public class NpcEntity : Entity, IHasOwner
    {
        public NpcEntity(EntityId id, EntityId ownerId, Entity owner, NpcInfo info, Vector3f position, Angle heading)
            : base(id, position, heading)
        {
            OwnerId = ownerId;
            Owner = owner;
            Info = info;
        }


        public NpcInfo Info { get; }

        public EntityId OwnerId { get; set; }
        public Entity Owner { get; set; }

        public override string ToString()
        {
            return Info.Name + " : " + Info.Area;
        }
    }
}