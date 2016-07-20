using System;

namespace Tera.Game.Abnormality
{
    public class Abnormality
    {
        private readonly AbnormalityTracker _abnormalityTracker;
        private bool _buffRegistered;

        private bool _enduranceDebuffRegistered;

        public Abnormality(HotDot hotdot, EntityId source, EntityId target, int duration, int stack, long ticks,
            AbnormalityTracker abnormalityTracker)
        {
            HotDot = hotdot;
            Source = source;
            Target = target;
            Duration = duration/1000;
            Stack = stack == 0 ? 1 : stack;
            FirstHit = ticks;
            if (HotDot.Name == "") return;
            _abnormalityTracker = abnormalityTracker;
            RegisterBuff();
            RegisterEnduranceDebuff();
        }

        public HotDot HotDot { get; }
        public EntityId Source { get; }
        public int Stack { get; private set; }

        public EntityId Target { get; }

        public int Duration { get; private set; }

        public long LastApply { get; private set; }

        public long FirstHit { get; }

        public long TimeBeforeApply => DateTime.UtcNow.Ticks - LastApply - HotDot.Tick*10000000;

        public void Apply(int amount, bool critical, bool isHp, long time)
        {
            if (_abnormalityTracker.UpdateDamageTracker != null)
            {
                var skillResult = new SkillResult(
                    amount,
                    critical,
                    isHp,
                    amount > 0,
                    HotDot,
                    Source,
                    Target,
                    new DateTime(time),
                    _abnormalityTracker.EntityTracker,
                    _abnormalityTracker.PlayerTracker
                    );

                _abnormalityTracker.UpdateDamageTracker(skillResult);
            }
            LastApply = time;
        }

        public void ApplyBuffDebuff(long tick)
        {
            if (HotDot.Name == "") return;
            ApplyBuff(tick);
            ApplyEnduranceDebuff(tick);
        }

        private void ApplyEnduranceDebuff(long lastTicks)
        {
            if (!HotDot.Debuff) return;
            if (_enduranceDebuffRegistered == false) return;
            var entity = _abnormalityTracker.EntityTracker.GetOrPlaceholder(Target);
            var game = entity as NpcEntity;
            if (game == null)
            {
                return;
            }

            _abnormalityTracker.AbnormalityStorage.AbnormalityTime(game)[HotDot].End(lastTicks);
        }


        private void RegisterEnduranceDebuff()
        {
            if (!HotDot.Debuff) return;
            var entityGame = _abnormalityTracker.EntityTracker.GetOrPlaceholder(Target);
            var game = entityGame as NpcEntity;
            if (game == null)
            {
                return;
            }

            if (!_abnormalityTracker.AbnormalityStorage.AbnormalityTime(game).ContainsKey(HotDot))
            {
                var userEntity = _abnormalityTracker.EntityTracker.GetOrPlaceholder(Source);
                var user = userEntity as UserEntity;
                if (user == null)
                {
                    return;
                }
                var abnormalityInitDuration = new AbnormalityDuration(user.RaceGenderClass.Class, FirstHit);
                _abnormalityTracker.AbnormalityStorage.AbnormalityTime(game).Add(HotDot, abnormalityInitDuration);
                _enduranceDebuffRegistered = true;
                return;
            }

            _abnormalityTracker.AbnormalityStorage.AbnormalityTime(game)[HotDot].Start(FirstHit);
            _enduranceDebuffRegistered = true;
        }

        private void RegisterBuff()
        {
            if (HotDot.HPMPChange) return;
            var userEntity = _abnormalityTracker.EntityTracker.GetOrNull(Target);
            var user = userEntity as UserEntity;
            if (user == null)
            {
                return;
            }
            var player = _abnormalityTracker.PlayerTracker.GetOrUpdate(user);

            if (!_abnormalityTracker.AbnormalityStorage.AbnormalityTime(player).ContainsKey(HotDot))
            {
                var npcEntity = _abnormalityTracker.EntityTracker.GetOrPlaceholder(Source);
                PlayerClass playerClass;
                if (!(npcEntity is UserEntity))
                {
                    playerClass = PlayerClass.Common;
                }
                else
                {
                    playerClass = ((UserEntity) npcEntity).RaceGenderClass.Class;
                }
                var abnormalityInitDuration = new AbnormalityDuration(playerClass, FirstHit);
                _abnormalityTracker.AbnormalityStorage.AbnormalityTime(player).Add(HotDot, abnormalityInitDuration);
                _buffRegistered = true;
                return;
            }
            _abnormalityTracker.AbnormalityStorage.AbnormalityTime(player)[HotDot].Start(FirstHit);
            _buffRegistered = true;
        }

        private void ApplyBuff(long lastTicks)
        {
            if (HotDot.HPMPChange) return;
            if (_buffRegistered == false) return;
            var userEntity = _abnormalityTracker.EntityTracker.GetOrNull(Target);
            if (!(userEntity is UserEntity))
            {
                return;
            }
            var player = _abnormalityTracker.PlayerTracker.GetOrUpdate((UserEntity) userEntity);
            _abnormalityTracker.AbnormalityStorage.AbnormalityTime(player)[HotDot].End(lastTicks);
        }

        public void Refresh(int stackCounter, int duration, long time)
        {
            Stack = stackCounter;
            Duration = duration/1000;
        }
    }
}