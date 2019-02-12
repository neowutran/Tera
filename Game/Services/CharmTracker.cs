using System.Collections.Generic;
using System.Linq;

namespace Tera.Game
{
    public class CharmTracker
    {
        private readonly AbnormalityTracker _abnormalityTracker;
        private readonly Dictionary<EntityId, List<uint>> _charms = new Dictionary<EntityId, List<uint>>();

        public CharmTracker(AbnormalityTracker tracker)
        {
            _abnormalityTracker = tracker;
        }

        public void CharmAdd(EntityId target, uint charmId, byte status, long ticks)
        {
            if (status == 1)
            {
                if (!_charms.ContainsKey(target)) _charms[target] = new List<uint>();
                _charms[target].Add(charmId);
                _abnormalityTracker.AddAbnormality(target, new EntityId(0), 0, 0, (int) charmId, ticks);
                //Debug.WriteLine(BitConverter.ToString(BitConverter.GetBytes(target.Id)) + " AAdd :" + charmId);
            }
            else
            {
                if (_charms.ContainsKey(target))
                    if (_charms[target].Contains(charmId)) _charms[target].Remove(charmId);
                _abnormalityTracker.DeleteAbnormality(target, (int) charmId, ticks);
                //Debug.WriteLine(BitConverter.ToString(BitConverter.GetBytes(target.Id)) + " ADel :" + charmId);
            }
        }

        public void CharmEnable(EntityId target, uint charmId, long ticks)
        {
            if (!_charms.ContainsKey(target)) _charms[target] = new List<uint>();
            _charms[target].Add(charmId);
            _abnormalityTracker.AddAbnormality(target, new EntityId(0), 0, 0, (int) charmId, ticks);
            //Debug.WriteLine(BitConverter.ToString(BitConverter.GetBytes(target.Id))+" Enb :"+charmId);
        }

        public void CharmReset(EntityId target, List<CharmStatus> charms, long ticks)
        {
            if (_charms.ContainsKey(target))
            {
                foreach (var charm in _charms[target])
                {
                    _abnormalityTracker.DeleteAbnormality(target, (int) charm, ticks);
                    //Debug.WriteLine(BitConverter.ToString(BitConverter.GetBytes(target.Id)) + " reset :" + charm);
                }
            }
            _charms[target] = new List<uint>();
            foreach (var charm in charms)
            {
                if (charm.Status == 1)
                {
                    _abnormalityTracker.AddAbnormality(target, new EntityId(0), charm.Duration, 0, (int) charm.CharmId, ticks);
                    _charms[target].Add(charm.CharmId);
                    //Debug.WriteLine($"{BitConverter.ToString(BitConverter.GetBytes(target.Id))} {charm.Status == 1} : {charm.CharmId}");
                }
            }
            if (!_charms[target].Any()) _charms.Remove(target);
        }

        public void CharmDel(EntityId target, uint charmId, long ticks)
        {
            //Debug.WriteLine(BitConverter.ToString(BitConverter.GetBytes(target.Id)) + " Del :" + charmId);
            if (_charms.ContainsKey(target))
                if (_charms[target].Contains(charmId)) _charms[target].Remove(charmId);
            _abnormalityTracker.DeleteAbnormality(target, (int) charmId, ticks);
        }
    }
}