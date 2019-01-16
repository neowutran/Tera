using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tera.Game.Messages;

namespace Tera.Game
{
    public class PlayerTracker : IEnumerable<Player>
    {
        private readonly EntityTracker _entityTracker;
        private readonly Dictionary<Tuple<uint, uint>, Player> _playerById = new Dictionary<Tuple<uint, uint>, Player>();
        private readonly ServerDatabase _serverDatabase;
        private Player _unknownDamage;
        private List<Tuple<uint, uint>> _currentParty = new List<Tuple<uint, uint>>();
        public bool IsRaid { get; private set; }

        public int PartySize => _currentParty.Count;
        
        public PlayerTracker(EntityTracker entityTracker, ServerDatabase serverDatabase = null)
        {
            _serverDatabase = serverDatabase;
            _entityTracker = entityTracker;
            IsRaid = false;
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

        public delegate void PlayerIdChangedEvent(EntityId oldId, EntityId newId);
        public event PlayerIdChangedEvent PlayerIdChangedAction;

        public delegate void PartyChange();
        public event PartyChange PartyChangedEvent;
        
        public void Update(UserEntity user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            Player player;
            var tup = Tuple.Create(user.ServerId, user.PlayerId);
            if (!_playerById.TryGetValue(tup, out player))
            {
                player = new Player(user, _serverDatabase);
                _playerById.Add(tup, player);
            }
            else
            {
                if (player.User == user) return;
                OnPlayerIdChangedAction(player.User.Id, user.Id);
                player.User = user;
            }
        }

        public UserEntity GetUnknownPlayer()
        {
            if ((_unknownDamage?.ServerId ?? 0) != (_entityTracker?.MeterUser.ServerId ?? 0))
            {
                var user = new UserEntity(_entityTracker?.MeterUser.ServerId ?? 0);
                _entityTracker?.Register(user);
                _unknownDamage=GetOrUpdate(user);
            }
            return _unknownDamage?.User;
        }

        public Player Get(uint serverId, uint playerId)
        {
            return _playerById[Tuple.Create(serverId, playerId)];
        }

        public Player GetOrNull(uint serverId, uint playerId)
        {
            Player result;
            _playerById.TryGetValue(Tuple.Create(serverId, playerId), out result);
            return result;
        }

        public Player GetOrUpdate(UserEntity user)
        {
            Update(user);
            return _playerById[Tuple.Create(user.ServerId, user.PlayerId)];
        }

        public void UpdateParty(S_BAN_PARTY message)
        {
            _currentParty = new List<Tuple<uint, uint>>();
            PartyChangedEvent?.Invoke();
        }

        public void UpdateParty(LoginServerMessage message)
        {
            _currentParty = new List<Tuple<uint, uint>>();
            PartyChangedEvent?.Invoke();
        }

        public void UpdateParty(S_LEAVE_PARTY m )
        {
            _currentParty = new List<Tuple<uint, uint>>();
            PartyChangedEvent?.Invoke();
        }

        public void UpdateParty(S_LEAVE_PARTY_MEMBER m)
        {
            _currentParty.Remove(Tuple.Create(m.ServerId, m.PlayerId));
            PartyChangedEvent?.Invoke();
        }

        public void UpdateParty(S_BAN_PARTY_MEMBER m)
        {
            _currentParty.Remove(Tuple.Create(m.ServerId, m.PlayerId));
            PartyChangedEvent?.Invoke();
        }

        public void UpdateParty(S_PARTY_MEMBER_LIST m)
        {
            _currentParty = m.Party.ConvertAll(x => Tuple.Create(x.ServerId, x.PlayerId));
            IsRaid = m.Raid;
            PartyChangedEvent?.Invoke();
        }

        public void UpdateParty(ParsedMessage message)
        {
            message.On<S_BAN_PARTY>(m => UpdateParty(m));
            message.On<S_LEAVE_PARTY>(m => UpdateParty(m));
            message.On<S_LEAVE_PARTY_MEMBER>(m => UpdateParty(m));
            message.On<S_BAN_PARTY_MEMBER>(m => UpdateParty(m));
            message.On<S_PARTY_MEMBER_LIST>(m => UpdateParty(m));
            message.On<LoginServerMessage>(m => UpdateParty(m));
        }

        public bool MyParty(Player player)
        {
            if (player == null) return false;
            return _currentParty.Contains(Tuple.Create(player.ServerId, player.PlayerId)) ||
                   player.User == _entityTracker.MeterUser;
        }

        public bool MyParty(uint serverId, uint playerId)
        {
            return _currentParty.Contains(Tuple.Create(serverId, playerId)) ||
                   (playerId == _entityTracker.MeterUser.PlayerId && serverId == _entityTracker.MeterUser.ServerId);
        }
        
        public List<UserEntity> PartyList()
        {
            List<UserEntity> list = new List<UserEntity>();
            _currentParty.ForEach(x =>
                {
                    if (_playerById.TryGetValue(x, out Player player)) list.Add(player.User);
                });
            if (_entityTracker.MeterUser != null && !list.Contains(_entityTracker.MeterUser)) list.Add(_entityTracker.MeterUser);
            return list;
        }

        public Player Me()
        {
            var user = _entityTracker.MeterUser;
            if (user != null) return Get(user.ServerId, user.PlayerId);
            return null;
        }

        protected virtual void OnPlayerIdChangedAction(EntityId oldid, EntityId newid)
        {
            PlayerIdChangedAction?.Invoke(oldid, newid);
        }

    }
}