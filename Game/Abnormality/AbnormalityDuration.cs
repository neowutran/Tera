using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Tera.Game
{
    public class AbnormalityDuration : ICloneable
    {
        private List<Duration> _listDuration = new List<Duration>();

        public AbnormalityDuration(PlayerClass playerClass, long start)
        {
            InitialPlayerClass = playerClass;
            Start(start);
        }

        private AbnormalityDuration(PlayerClass playerClass)
        {
            InitialPlayerClass = playerClass;
        }

        public PlayerClass InitialPlayerClass { get; }

        public object Clone()
        {
            var newListDuration = _listDuration.Select(duration => (Duration) duration.Clone()).ToList();
            var abnormalityDuration = new AbnormalityDuration(InitialPlayerClass)
            {
                _listDuration = newListDuration
            };
            return abnormalityDuration;
        }

        public AbnormalityDuration Clone(long begin, long end)
        {
            if (begin == 0 || end == 0) return (AbnormalityDuration) Clone();
            var newListDuration =
                _listDuration.Where(x => x.Begin < end && x.End > begin)
                    .Select(duration => duration.Clone(begin, end))
                    .ToList();
            var abnormalityDuration = new AbnormalityDuration(InitialPlayerClass)
            {
                _listDuration = newListDuration
            };
            return abnormalityDuration;
        }
        public long Duration(long begin, long end)
        {
            long totalDuration = 0;
            foreach (var duration in _listDuration)
            {
                if (begin > duration.End || end < duration.Begin)
                {
                    continue;
                }

                var abnormalityBegin = duration.Begin;
                var abnormalityEnd = duration.End;

                if (begin > abnormalityBegin)
                {
                    abnormalityBegin = begin;
                }

                if (end < abnormalityEnd)
                {
                    abnormalityEnd = end;
                }

                totalDuration += abnormalityEnd - abnormalityBegin;
            }
            return totalDuration;
        }

        public void Start(long start)
        {
            if (_listDuration.Count != 0) {
                if (!Ended())
                {
                    //Console.WriteLine("Can't restart something that has not been ended yet");
                    return;
                }
           }
            _listDuration.Add(new Duration(start, long.MaxValue));
        }

        public void End(long end)
        {
            if (Ended())
            {
                //Console.WriteLine("Can't end something that has already been ended");
                return;
            }

            _listDuration[_listDuration.Count - 1].End = end;
        }

        public long LastStart()
        {
            return _listDuration[_listDuration.Count - 1].Begin;
        }

        public long LastEnd()
        {
            return _listDuration[_listDuration.Count - 1].End;
        }

        public int Count(long begin=0, long end=0)
        {
            return begin == 0 || end == 0 ? _listDuration.Count : _listDuration.Count(x => begin <= x.End && end >= x.Begin);
        }

        public List<Duration> AllDurations(long begin, long end) //for use only on cloned storages
        {
            return _listDuration.Where(x => begin <= x.End && end >= x.Begin)
                .Select(x => new Duration(begin > x.Begin ? begin : x.Begin, end < x.End ? end : x.End))
                .ToList();
        }
        public List<Duration> AllDurations() //for use only on filtered cloned storages
        {
            return _listDuration.ToList();
        }

        public bool Ended()
        {
            return _listDuration[_listDuration.Count - 1].End != long.MaxValue;
        }
    }
}