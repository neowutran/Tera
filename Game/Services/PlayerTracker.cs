using System;
using System.Collections;
using System.Collections.Generic;

namespace Tera.Game
{
    public class PlayerTracker : IEnumerable<Player>
    {
        private readonly Dictionary<Tuple<uint,uint>, Player> _playerById = new Dictionary<Tuple<uint,uint>, Player>();
        private readonly ServerDatabase _serverDatabase;
        public PlayerTracker(EntityTracker entityTracker,ServerDatabase serverDatabase=null)
        {
            _serverDatabase = serverDatabase;
            entityTracker.EntityUpdated += Update;
        }

        public IEnumerator<Player> GetEnumerator()
        {
            return _playerById.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void Update(Entity entity)
        {
            var user = entity as UserEntity;
            if (user != null)
                Update(user);
        }

        public void Update(UserEntity user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            Player player;
            var tup = Tuple.Create(user.ServerId, user.PlayerId);
            if (!_playerById.TryGetValue(tup, out player))
            {
                player = new Player(user,_serverDatabase);
                _playerById.Add(tup, player);
            }
            else
            {
                if (player.User != user)
                    player.User = user;
            }
        }

        public Player Get(uint serverId,uint playerId)
        {
            return _playerById[Tuple.Create(serverId, playerId)];
        }

        public Player GetOrNull(uint serverId,uint playerId)
        {
            Player result=null;
            _playerById.TryGetValue(Tuple.Create(serverId,playerId), out result);
            return result;
        }

        public Player GetOrUpdate(UserEntity user)
        {
            Update(user);
            return _playerById[Tuple.Create(user.ServerId, user.PlayerId)];
        }
    }
}