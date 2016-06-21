using System;

namespace Tera.Game
{
    // An object with an Id that can be spawned or deswpawned in the game world
    public class Entity : IEquatable<object>
    {


        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Entity)obj);
        }

        public bool Equals(Entity other)
        {
            return Id == other.Id;
        }

        public static bool operator ==(Entity a, Entity b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(Entity a, Entity b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }


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