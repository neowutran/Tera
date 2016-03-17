namespace Tera.Game
{
    // An object with an Id that can be spawned or deswpawned in the game world
    public class Entity
    {
        public Entity(EntityId id)
        {
            Id = id;
        }

        public EntityId Id { get; }

        public override string ToString()
        {
            var result = $"{GetType().Name} {Id}";
            if (RootOwner != this)
                result = $"{result} owned by {RootOwner}";
            return result;
        }

        public Entity RootOwner
        {
            get
            {
                var entity = this;
                var ownedEntity = entity as IHasOwner;
                while (ownedEntity != null && ownedEntity.Owner != null)
                {
                    entity = ownedEntity.Owner;
                    ownedEntity = entity as IHasOwner;
                }
                return entity;
            }
        }
    }
}