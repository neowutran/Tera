// Copyright (c) Gothos
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Tera.Game
{
    public class ProjectileEntity : Entity, IHasOwner
    {
        public ProjectileEntity(EntityId id, EntityId ownerId, Entity owner, Vector3f position, Angle heading,
            Vector3f finish, int speed, long time)
            : base(id, position, heading, finish, speed, time)
        {
            OwnerId = ownerId;
            Owner = owner;
        }

        public EntityId OwnerId { get; set; }
        public Entity Owner { get; set; }
    }
}