// Copyright (c) Gothos
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Tera.Game.Messages;

namespace Tera.Game
{
    // Tracks which entities we have seen so far and what their properties are
    public class EntityTracker : IEnumerable<Entity>
    {
        private readonly Dictionary<EntityId, Entity> _dictionary = new Dictionary<EntityId, Entity>();
        private readonly NpcDatabase _npcDatabase;

        public EntityTracker(NpcDatabase npcDatabase)
        {
            _npcDatabase = npcDatabase;
        }

        public UserEntity MeterUser { get; private set; }

        public IEnumerator<Entity> GetEnumerator()
        {
            return _dictionary.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public event Action<Entity> EntityUpdated;

        protected virtual void OnEntityUpdated(Entity entity)
        {
            var handler = EntityUpdated;
            handler?.Invoke(entity);
        }

        public void Update(ParsedMessage message)
        {
            Entity newEntity = null;
            message.On<SpawnUserServerMessage>(m => newEntity = new UserEntity(m));
            message.On<LoginServerMessage>(m => newEntity = LoginMe(m));
            message.On<SpawnNpcServerMessage>(
                m =>
                    newEntity =
                        new NpcEntity(m.Id, m.OwnerId, GetOrPlaceholder(m.OwnerId),
                            _npcDatabase.GetOrPlaceholder(m.NpcArea, m.NpcId), m.Position, m.Heading));
            message.On<SpawnProjectileServerMessage>(
                m =>
                    newEntity =
                        new ProjectileEntity(m.Id, m.OwnerId, GetOrPlaceholder(m.OwnerId), m.Start,
                            m.Start.GetHeading(m.Finish), m.Finish, (int) m.Speed, m.Time.Ticks));
            message.On<StartUserProjectileServerMessage>(
                m =>
                    newEntity =
                        new ProjectileEntity(m.Id, m.OwnerId, GetOrPlaceholder(m.OwnerId), m.Start,
                            m.Start.GetHeading(m.Finish), m.Finish, (int) m.Speed, m.Time.Ticks));
            if (newEntity != null)
            {
                _dictionary[newEntity.Id] = newEntity;
                OnEntityUpdated(newEntity);
            }
            message.On<C_PLAYER_LOCATION>(m =>
            {
                if (MeterUser == null) return; //Don't know how, but sometimes this happens.
                MeterUser.Position = m.Position;
                MeterUser.Heading = m.Heading;
                MeterUser.Finish = m.Position;
                MeterUser.Speed = 0;
                MeterUser.StartTime = m.Time.Ticks;
                MeterUser.EndAngle = m.Heading;
                MeterUser.EndTime = 0;
                //Debug.WriteLine($"{MeterUser.Position} {MeterUser.Heading}");
            });
            message.On<S_CHANGE_DESTPOS_PROJECTILE>(m =>
            {
                var entity = GetOrNull(m.Id);
                if (entity == null) return;
                entity.Position = entity.Position.MoveForvard(entity.Finish, entity.Speed,
                    m.Time.Ticks - entity.StartTime);
                entity.Finish = m.Finish;
                entity.Heading = entity.Position.GetHeading(entity.Finish);
                entity.StartTime = m.Time.Ticks;
                entity.EndAngle = entity.Heading;
                entity.EndTime = 0;
                //Debug.WriteLine($"{entity.Position} {entity.Heading}");
            });
            message.On<S_ACTION_STAGE>(m =>
            {
                var entity = GetOrNull(m.Entity);
                if (entity == null) return;
                entity.Position = m.Position;
                entity.Finish = entity.Position;
                entity.Heading = m.Heading;
                entity.Speed = 0;
                entity.StartTime = 0;
                entity.EndAngle = entity.Heading;
                entity.EndTime = 0;
                //Debug.WriteLine($"{entity.Position} {entity.Heading}");
            });
            message.On<S_ACTION_END>(m =>
            {
                var entity = GetOrNull(m.Entity);
                if (entity == null) return;
                entity.Position = m.Position;
                entity.Finish = entity.Position;
                entity.Heading = m.Heading;
                entity.Speed = 0;
                entity.StartTime = 0;
                entity.EndAngle = entity.Heading;
                entity.EndTime = 0;
                //Debug.WriteLine($"{entity.Position} {entity.Heading}");
            });
            message.On<SCreatureLife>(m =>
            {
                var entity = GetOrNull(m.User);
                if (entity == null) return;
                entity.Position = m.Position;
                entity.Finish = entity.Position;
                entity.Speed = 0;
                entity.StartTime = 0;
                entity.EndAngle = entity.Heading;
                entity.EndTime = 0;
                //Debug.WriteLine($"{entity.Position} {entity.Heading}");
            });
            message.On<S_INSTANT_MOVE>(m =>
            {
                var entity = GetOrNull(m.Entity);
                if (entity == null) return;
                entity.Position = m.Position;
                entity.Finish = m.Position;
                entity.Heading = m.Heading;
                entity.Speed = 0;
                entity.StartTime = 0;
                entity.EndAngle = entity.Heading;
                entity.EndTime = 0;
                //Debug.WriteLine($"{entity.Position} {entity.Heading}");
            });
            message.On<S_CREATURE_ROTATE>(m =>
            {
                var entity = GetOrNull(m.Entity);
                if (entity == null) return;
                entity.Position = entity.Position.MoveForvard(entity.Finish, entity.Speed,
                    m.Time.Ticks - entity.StartTime);
                entity.Finish = entity.Position;
                entity.Speed = 0;
                entity.StartTime = m.Time.Ticks;
                if (entity.EndTime > 0 && entity.EndTime <= entity.StartTime)
                {
                    entity.Heading = entity.EndAngle;
                }
                else if (entity.EndTime > 0)
                {
                    Debug.WriteLine("New rotate started before old ended!");
                }
                entity.EndAngle = m.Heading;
                entity.EndTime = entity.StartTime + (m.NeedTime == 0 ? 0 : TimeSpan.TicksPerMillisecond*m.NeedTime);
                //Debug.WriteLine($"{entity.Position} {entity.Heading} {entity.EndAngle} {m.NeedTime}");
            });
            message.On<SNpcLocation>(m =>
            {
                var entity = GetOrNull(m.Entity);
                if (entity == null) return;
                entity.Position = m.Start;
                entity.Finish = m.Finish;
                entity.Speed = m.Speed;
                entity.StartTime = m.Time.Ticks;
                entity.Heading = m.Heading;
                entity.EndAngle = m.Heading;
                entity.EndTime = 0;
                //Debug.WriteLine($"{entity.Position} {entity.Heading} {entity.Finish} {entity.Speed}");
            });
            message.On<S_USER_LOCATION>(m =>
            {
                var entity = GetOrNull(m.Entity);
                if (entity == null) return;
                entity.Position = m.Start;
                entity.Finish = m.Finish;
                entity.Speed = m.Speed;
                entity.StartTime = m.Time.Ticks;
                entity.Heading = m.Heading;
                entity.EndAngle = m.Heading;
                entity.EndTime = 0;
                //Debug.WriteLine($"{entity.Position} {entity.Heading} {entity.Finish} {entity.Speed}");
            });
            message.On<S_BOSS_GAGE_INFO>(m =>
            {
                var entity = GetOrNull(m.EntityId) as NpcEntity;
                if (entity == null) return;
                _npcDatabase.AddDetectedBoss(entity.Info.HuntingZoneId, entity.Info.TemplateId);
                entity.Info.Boss = true;
            });
        }

        private Entity LoginMe(LoginServerMessage m)
        {
            MeterUser = new UserEntity(m);
            return MeterUser;
        }

        public Entity GetOrNull(EntityId id)
        {
            Entity entity;
            _dictionary.TryGetValue(id, out entity);
            return entity;
        }

        public Entity GetOrPlaceholder(EntityId id)
        {
            if (id == EntityId.Empty)
                return null;
            var entity = GetOrNull(id);
            if (entity != null)
                return entity;
            return new PlaceHolderEntity(id);
        }
    }
}