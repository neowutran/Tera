using System;
using System.Collections;
using System.Collections.Generic;

namespace Tera.Game
{
    public class PlayerTracker : IEnumerable<Player>
    {
        private readonly Dictionary<uint, Player> _playerById = new Dictionary<uint, Player>();
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
            if (!_playerById.TryGetValue(user.PlayerId, out player))
            {
                player = new Player(user,_serverDatabase);
                _playerById.Add(player.PlayerId, player);
            }
            else
            {
                player.User = user;
            }
        }

        public Player Get(uint playerId)
        {
            return _playerById[playerId];
        }
    }
}