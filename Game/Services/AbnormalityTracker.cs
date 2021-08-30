using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using Tera.Game.Abnormality;
using Tera.Game.Messages;
using static Tera.Game.HotDotDatabase;

namespace Tera.Game
{
    public class AbnormalityTracker
    {
        private Dictionary<EntityId, List<Abnormality.Abnormality>> _abnormalities = new Dictionary<EntityId, List<Abnormality.Abnormality>>();

        internal AbnormalityStorage AbnormalityStorage;
        internal EntityTracker EntityTracker;
        internal HotDotDatabase HotDotDatabase;
        internal PlayerTracker PlayerTracker;
        internal CharmTracker CharmTracker;
        public Action<SkillResult> UpdateDamageTracker;

        public AbnormalityTracker(EntityTracker entityTracker, PlayerTracker playerTracker,
            HotDotDatabase hotDotDatabase, AbnormalityStorage abnormalityStorage, Action<SkillResult> update = null)
        {
            EntityTracker = entityTracker;
            PlayerTracker = playerTracker;
            HotDotDatabase = hotDotDatabase;
            UpdateDamageTracker = update;
            CharmTracker = new CharmTracker(this);
            AbnormalityStorage = abnormalityStorage;
        }

        public List<Abnormality.Abnormality> BuffList(EntityId entity) // use only in processing thread
        {
            if (!_abnormalities.ContainsKey(entity))return new List<Abnormality.Abnormality>();
            return _abnormalities[entity];
        }

        public void Update(SNpcStatus npcStatus)
        {

            RegisterAggro(npcStatus);
            if (npcStatus.Enraged)
            {
                AddAbnormality(npcStatus.Npc, npcStatus.Target, npcStatus.RemainingEnrageTime, 0, (int)StaticallyUsedBuff.Enraged, npcStatus.Time.Ticks);
                var abnormalityUser = _abnormalities[npcStatus.Npc];
                foreach (var abnormality in abnormalityUser)
                {
                    if (abnormality.HotDot.Id != (int)StaticallyUsedBuff.Enraged) continue;
                    abnormality.Refresh(0, npcStatus.RemainingEnrageTime, npcStatus.Time.Ticks);
                    AbnormalityAdded?.Invoke(abnormality, false);
                    return;
                }
            }
            else
            {
                DeleteAbnormality(npcStatus);
            }
        }

        public void RegisterSlaying(UserEntity user, bool slaying, long ticks)
        {
            if (user == null) return;
            if (slaying)
            {
                if (!AbnormalityStorage.Death(PlayerTracker.GetOrUpdate(user)).Dead)
                {
                    AddAbnormality(user.Id, user.Id, 0, 0, (int)StaticallyUsedBuff.Slaying, ticks);
                }
            }
            else
            {
                DeleteAbnormality(user.Id, (int)StaticallyUsedBuff.Slaying, ticks);
            }
        }

        public void RegisterDead(EntityId id, long ticks, bool dead)
        {
            var user = EntityTracker.GetOrNull(id) as UserEntity;
            if (user == null) return;
            var player = PlayerTracker.GetOrUpdate(user);
            if (dead)
            {
                AbnormalityStorage.Death(player).Start(ticks);
                DeleteAbnormality(user.Id, (int)StaticallyUsedBuff.Slaying, ticks);
            }
            else
                AbnormalityStorage.Death(player).End(ticks);
        }

        public void Update(SCreatureLife message)
        {
            RegisterDead(message.User, message.Time.Ticks, message.Dead);
        }

        private List<EntityId> AggroRegistered = new List<EntityId>();

        public void Update(SNpcOccupierInfo message)
        {
            if (message.HasReset){
                AggroRegistered.Remove(message.NPC);
                return;
            }

            if (!AggroRegistered.Contains(message.NPC))
            {
                // Add a arbitrary dummy damage to auto start fight timer on aggro 
                AggroRegistered.Add(message.NPC);
                UpdateDamageTracker(new SkillResult(0, false, true, false, HotDotDatabase.Enraged, message.Target, message.NPC, message.Time, EntityTracker, PlayerTracker));
            }

        }


        private void RegisterAggro(SNpcStatus aggro)
        {
            var time = aggro.Time.Ticks;
            var entity = EntityTracker.GetOrNull(aggro.Npc) as NpcEntity;
            if (entity == null) return; //not sure why, but sometimes it fails
            var user = EntityTracker.GetOrNull(aggro.Target) as UserEntity;
            if (user != null)
            {
                var player = PlayerTracker.GetOrUpdate(user);
                if (AbnormalityStorage.Last(entity) != player)
                {
                    if (AbnormalityStorage.Last(entity) != null)
                    {
                        AbnormalityStorage.AggroEnd(AbnormalityStorage.Last(entity), entity, time);
                    }
                    else
                    {
                        if (!AggroRegistered.Contains(aggro.Npc))
                        {
                            // Add a arbitrary dummy damage to auto start fight timer on aggro 
                            AggroRegistered.Add(aggro.Npc);
                            UpdateDamageTracker(new SkillResult(0, false, true, false, HotDotDatabase.Enraged, aggro.Target, aggro.Npc, aggro.Time, EntityTracker, PlayerTracker));
                        }
                    }

                    AbnormalityStorage.AggroStart(player, entity, time);
                    AbnormalityStorage.LastAggro[entity] = player;
                    
                }
            }
            else
            {
                if (AbnormalityStorage.Last(entity) != null)
                {
                    AbnormalityStorage.AggroEnd(AbnormalityStorage.Last(entity), entity, time);
                    AbnormalityStorage.LastAggro[entity] = null;
                }
            }
        }

        public void StopAggro(SDespawnNpc aggro)
        {
            var time = aggro.Time.Ticks;
            var entity = EntityTracker.GetOrNull(aggro.Npc) as NpcEntity;
            if (entity == null) return; // Strange, but seems there are not only NPC or something wrong with trackers
            if (AbnormalityStorage.Last(entity) != null)
            {
                AbnormalityStorage.AggroEnd(AbnormalityStorage.Last(entity), entity, time);
                AbnormalityStorage.LastAggro[entity] = null;
            }
        }


        public event AbnormalityAddEvent AbnormalityAdded;
        public event AbnormalityRemoveEvent AbnormalityRemoved;

        public delegate void AbnormalityAddEvent(Abnormality.Abnormality abnormality, bool newStack);
        public delegate void AbnormalityRemoveEvent(EntityId target, int abnormalityId);

        public void Update(SAbnormalityBegin message)
        {
            AddAbnormality(message.TargetId, message.SourceId, message.Duration, message.Stack, message.AbnormalityId,
                message.Time.Ticks);
        }

        public void AddAbnormality(EntityId target, EntityId source, long duration, int stack, int abnormalityId,
            long ticks)
        {
            if (!_abnormalities.ContainsKey(target))
            {
                _abnormalities.Add(target, new List<Abnormality.Abnormality>());
            }
            var hotdot = HotDotDatabase.Get(abnormalityId);
            if (hotdot == null || hotdot.IsShow == false)
            {
                return;
            }

            if (_abnormalities[target].Count(x => x.HotDot.Id == abnormalityId) == 0)
            {
                var ab = new Abnormality.Abnormality(hotdot, source, target, duration, stack, ticks, this);
                //dont add existing abnormalities since we don't delete them all, that may cause many untrackable issues.
                _abnormalities[target].Add(ab);
                AbnormalityAdded?.Invoke(ab, true);
            }
        }

        public void Update(SAbnormalityRefresh message)
        {
            if (!_abnormalities.ContainsKey(message.TargetId))
            {
                return;
            }
            var abnormalityUser = _abnormalities[message.TargetId];
            foreach (var abnormality in abnormalityUser)
            {
                if (abnormality.HotDot.Id != message.AbnormalityId) continue;
                var newStack = abnormality.Stack < message.StackCounter;
                abnormality.Refresh(message.StackCounter, message.Duration, message.Time.Ticks);
                AbnormalityAdded?.Invoke(abnormality, newStack);
                return;
            }
        }

        public bool HaveAbnormalities(EntityId target) => _abnormalities.ContainsKey(target) && _abnormalities[target].Any();

        public bool AbnormalityExist(EntityId target, int abnormalityid) => _abnormalities.ContainsKey(target) && _abnormalities[target].Any(x=>x.HotDot.Id==abnormalityid);
        /**
         * Return time left for the abnormality. Or -1 if no abnormality found
         */
        public long AbnormalityTimeLeft(EntityId target, HotDot.Types dotype)
        {
            if (!_abnormalities.ContainsKey(target))
            {
                return -1;
            }
            var abnormalityTarget = _abnormalities[target];
            var abnormalities = abnormalityTarget.Where(t => t.HotDot.Effects.Any(x => x.Type == dotype));
            var enumerable = abnormalities as IList<Abnormality.Abnormality> ?? abnormalities.ToList();
            if(!enumerable.Any())
            {
                return -1;
            }
            return enumerable.Max(x => x.TimeBeforeEnd);
       }

        /**
         * Return time left for the abnormality. Or -1 if no abnormality found
         */
        public long AbnormalityTimeLeft(EntityId target, int abnormalityId, int stack=0)
        {
            if (!_abnormalities.ContainsKey(target))
            {
                return -1;
            }
            var abnormalityTarget = _abnormalities[target];
            var i = abnormalityTarget.FindIndex(t => t.HotDot.Id == abnormalityId && t.Stack>=stack);
            if (i == -1)
            {
                return -1;
            }
            return abnormalityTarget[i].TimeBeforeEnd;
        }

        /**
         * Return current stack count for the abnormality. Or -1 if no abnormality found
         */
        public int Stack(EntityId target, int abnormalityId)
        {
            if (!_abnormalities.ContainsKey(target))
            {
                return -1;
            }
            var abnormalityTarget = _abnormalities[target];
            var i = abnormalityTarget.FindIndex(t => t.HotDot.Id == abnormalityId);
            if (i==-1)
            {
                return -1;
            }
            return abnormalityTarget[i].Stack;

        }
        public void DeleteAbnormality(EntityId target, int abnormalityId, long ticks)
        {
            if (!_abnormalities.ContainsKey(target))
            {
                return;
            }

            var abnormalityUser = _abnormalities[target];

            for (var i = 0; i < abnormalityUser.Count; i++)
            {
                if (abnormalityUser[i].HotDot.Id != abnormalityId) continue;
                abnormalityUser[i].ApplyBuffDebuff(ticks);
                abnormalityUser.Remove(abnormalityUser[i]);
                AbnormalityRemoved?.Invoke(target, abnormalityId);
                break;
            }

            if (abnormalityUser.Count == 0)
            {
                _abnormalities.Remove(target);
                return;
            }
            _abnormalities[target] = abnormalityUser;
        }

        public void Update(SAbnormalityEnd message)
        {
            DeleteAbnormality(message.TargetId, message.AbnormalityId, message.Time.Ticks);
        }

        public void DeleteAbnormality(SDespawnNpc message)
        {
            DeleteAbnormality(message.Npc, message.Time.Ticks);
        }

        public void DeleteAbnormality(SNpcStatus message)
        {
            DeleteAbnormality(message.Npc, (int)StaticallyUsedBuff.Enraged, message.Time.Ticks);
        }

        public void DeleteAbnormality(SDespawnUser message)
        {
            DeleteAbnormality(message.User, message.Time.Ticks);
            RegisterDead(message.User, message.Time.Ticks, false);
        }

        private void DeleteAbnormality(EntityId entity, long ticks)
        {
            if (!_abnormalities.ContainsKey(entity))
            {
                return;
            }
            foreach (var abno in _abnormalities[entity])
            {
                abno.ApplyBuffDebuff(ticks);
            }
            _abnormalities.Remove(entity);
        }


        public void Update(SPlayerChangeMp message)
        {
            Update(message.TargetId, message.SourceId, message.MpChange, message.Type, message.Critical == 1, false,
                message.Time.Ticks);
        }

        private void Update(EntityId target, EntityId source, long change, int type, bool critical, bool isHp, long time, int abnotmalId = 0)
        {
            if (!_abnormalities.ContainsKey(target))
            {
                return;
            }

            if ((int)HotOrDot.Dot != type)
            {
                return;
            }

            var abnormalities = _abnormalities[target];
            var ab = abnotmalId==0 ? null : abnormalities.FirstOrDefault(x => x.HotDot.Id == abnotmalId);
            if (ab != null) { ab.Apply(change, critical, isHp, time); return; } //visible dots
            if (abnotmalId != 0) { //invisible dots
                if (UpdateDamageTracker != null)
                {
                    var skillResult = new SkillResult(
                        change,
                        critical,
                        isHp,
                        change > 0,
                        HotDotDatabase.Get(abnotmalId),
                        source,
                        target,
                        new DateTime(time),
                        EntityTracker,
                        PlayerTracker
                    );
                    UpdateDamageTracker(skillResult);
                }
                return;
            }
            /// backward compatibility with classic servers whithout abnormal id in hp change packet.
            abnormalities =
                abnormalities.Where(
                    x => x.Source == EntityTracker.MeterUser.Id || x.Target == EntityTracker.MeterUser.Id)
                    .OrderByDescending(o => o.TimeBeforeApply)
                    .ToList();

            foreach (var abnormality in abnormalities)
            {
                if (abnormality.Source != source && abnormality.Source != abnormality.Target)
                {
                    continue;
                }

                if (isHp)
                {
                    if ((!(abnormality.HotDot.Hp > 0) || change <= 0) &&
                        (!(abnormality.HotDot.Hp < 0) || change >= 0)
                        ) continue;
                }
                else
                {
                    if ((!(abnormality.HotDot.Mp > 0) || change <= 0) &&
                        (!(abnormality.HotDot.Mp < 0) || change >= 0)
                        ) continue;
                }

                abnormality.Apply(change, critical, isHp, time);
                return;
            }
        }

        public void Update(SCreatureChangeHp message)
        {
            Update(message.TargetId, message.SourceId, message.HpChange, message.Type, message.Critical == 1, true,
                message.Time.Ticks, message.AbnormalId);
            var user = EntityTracker.GetOrPlaceholder(message.TargetId) as UserEntity;
            RegisterSlaying(user, message.Slaying, message.Time.Ticks);
        }

        public void Update(SPartyMemberCharmAdd message)
        {
            var player = PlayerTracker.GetOrNull(message.ServerId, message.PlayerId);
            if (player == null) return;
            CharmTracker.CharmAdd(player.User.Id, message.CharmId, message.Status, message.Time.Ticks);
        }

        public void Update(SResetCharmStatus message)
        {
            CharmTracker.CharmReset(message.TargetId, message.Charms, message.Time.Ticks);
        }

        public void Update(SAddCharmStatus message)
        {
            CharmTracker.CharmAdd(message.TargetId, message.CharmId, message.Status, message.Time.Ticks);
        }

        public void Update(SEnableCharmStatus message)
        {
            CharmTracker.CharmEnable(EntityTracker.MeterUser.Id, message.CharmId, message.Time.Ticks);
        }

        public void Update(SPartyMemberCharmDel message)
        {
            var player = PlayerTracker.GetOrNull(message.ServerId, message.PlayerId);
            if (player == null) return;
            CharmTracker.CharmDel(player.User.Id, message.CharmId, message.Time.Ticks);
        }

        public void Update(SPartyMemberCharmEnable message)
        {
            var player = PlayerTracker.GetOrNull(message.ServerId, message.PlayerId);
            if (player == null) return;
            CharmTracker.CharmEnable(player.User.Id, message.CharmId, message.Time.Ticks);
        }

        public void Update(SPartyMemberCharmReset message)
        {
            var player = PlayerTracker.GetOrNull(message.ServerId, message.PlayerId);
            if (player == null) return;
            CharmTracker.CharmReset(player.User.Id, message.Charms, message.Time.Ticks);
        }
        public void Update(SRemoveCharmStatus message)
        {
            CharmTracker.CharmDel(EntityTracker.MeterUser.Id, message.CharmId, message.Time.Ticks);
        }

        public void Update(SDespawnUser message)
        {
            DeleteAbnormality(message);
            CharmTracker.CharmReset(message.User, new List<CharmStatus>(), message.Time.Ticks);

        }
        public void Update(SDespawnNpc message)
        {
            StopAggro(message);
            DeleteAbnormality(message);
        }
        public void Update(SpawnUserServerMessage message)
        {
            RegisterDead(message.Id, message.Time.Ticks, message.Dead);
        }
        public void Update(SpawnMeServerMessage message)
        {
            AggroRegistered = new List<EntityId>();
            AbnormalityStorage.EndAll(message.Time.Ticks);
            _abnormalities = new Dictionary<EntityId, List<Abnormality.Abnormality>>();
            RegisterDead(message.Id, message.Time.Ticks, message.Dead);
            CharmTracker = new CharmTracker(this);
        }
        public void Update(S_PLAYER_STAT_UPDATE message)
        {
            RegisterSlaying(EntityTracker.MeterUser, message.Slaying, message.Time.Ticks);
        }
        public void Update(S_PARTY_MEMBER_STAT_UPDATE message)
        {
            var user = PlayerTracker.GetOrNull(message.ServerId, message.PlayerId);
            if (user == null) return;
            RegisterSlaying(user.User, message.Slaying, message.Time.Ticks);
        }
        public void Update(SPartyMemberChangeHp message)
        {
            var user = PlayerTracker.GetOrNull(message.ServerId, message.PlayerId);
            if (user == null) return;
            RegisterSlaying(user.User, message.Slaying, message.Time.Ticks);
        }

     
        public void Update(ParsedMessage message)
        {
            message.On<S_PLAYER_STAT_UPDATE>(x => Update(x));
            message.On<S_PARTY_MEMBER_STAT_UPDATE>(x => Update(x));
            message.On<SPartyMemberChangeHp>(x => Update(x));
            message.On<SCreatureChangeHp>(x => Update(x));
            message.On<SPlayerChangeMp>(x => Update(x));
            message.On<SDespawnUser>(x => Update(x));
            message.On<SRemoveCharmStatus>(x => Update(x));
            message.On<SPartyMemberCharmReset>(x => Update(x));
            message.On<SPartyMemberCharmEnable>(x => Update(x));
            message.On<SPartyMemberCharmDel>(x => Update(x));
            message.On<SEnableCharmStatus>(x => Update(x));
            message.On<SAddCharmStatus>(x => Update(x));
            message.On<SResetCharmStatus>(x => Update(x));
            message.On<SPartyMemberCharmAdd>(x => Update(x));
            message.On<SAbnormalityEnd>(x => Update(x));
            message.On<SAbnormalityRefresh>(x => Update(x));
            message.On<SAbnormalityBegin>(x => Update(x));
            message.On<SpawnMeServerMessage>(x => Update(x));
            message.On<SDespawnNpc>(x => Update(x));
            message.On<SCreatureLife>(x => Update(x));
            message.On<SNpcStatus>(x => Update(x));
            message.On<SpawnUserServerMessage>(x => Update(x));
            message.On<SNpcOccupierInfo>(x => Update(x));

        }
    }
}