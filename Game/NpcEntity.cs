// Copyright (c) Gothos
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Tera.Game
{
    // NPCs and Mosters - Tera doesn't distinguish these
    public class NpcEntity : Entity, IHasOwner
    {
        public NpcEntity(EntityId id, EntityId ownerId, uint categoryId, uint npcId, ushort npcArea, Entity owner,NpcInfo info)
            : base(id)
        {
            OwnerId = ownerId;
            Owner = owner;
            CategoryId = categoryId;
            NpcId = npcId;
            NpcArea = npcArea;
            Info = info ?? new NpcInfo(npcArea,npcId,false,0,$"{npcArea} {npcId}",npcArea.ToString());
        }

        public uint CategoryId { get; }
        public uint NpcId { get; }
        public ushort NpcArea { get; }
        public NpcInfo Info { get; private set; }

        public EntityId OwnerId { get; }
        public Entity Owner { get; }
    }
}