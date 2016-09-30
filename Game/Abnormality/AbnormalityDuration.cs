using System;
using System.Collections.Generic;
using System.Linq;

namespace Tera.Game.Abnormality
{
    public class AbnormalityDuration : ICloneable
    {
        private List<Duration> _listDuration = new List<Duration>();

        public AbnormalityDuration(PlayerClass playerClass, long start, int stack)
        {
            InitialPlayerClass = playerClass;
            Start(start, stack);
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

        public long Duration(long begin, long end, int stack=0)
        {
            long totalDuration = 0;
            foreach (var duration in stack==0
                                    ? _listDuration.Where(x => end >= x.Begin && begin <= x.End)
                                    : _listDuration.Where(x => end >= x.Begin && begin <= x.End && stack == x.Stack)
                                    )
            {
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

        public void Start(long start,int stack)
        {
            if (_listDuration.Count != 0)
            {
                if (!Ended())
                {
                    if (LastStack()==stack)
                    //Debug.WriteLine("Can't restart something that has not been ended yet");
                        return;
                    if (_listDuration[_listDuration.Count - 1].Begin == start)
                        _listDuration.RemoveAt(_listDuration.Count - 1);//no need to store zero-time stacks
                    else
                        _listDuration[_listDuration.Count - 1].End = start;
                }
            }
            _listDuration.Add(new Duration(start, long.MaxValue, stack));
        }

        public void End(long end)
        {
            if (Ended())
            {
                //Debug.WriteLine("Can't end something that has already been ended");
                return;
            }

            _listDuration[_listDuration.Count - 1].End = end;
        }

        public int LastStack()
        {
            return _listDuration[_listDuration.Count - 1].Stack;
        }

        public long LastStart()
        {
            return _listDuration[_listDuration.Count - 1].Begin;
        }

        public long LastEnd()
        {
            return _listDuration[_listDuration.Count - 1].End;
        }

        public int Count(long begin = 0, long end = 0, int stack = 0)
        {
            if (stack==0)
                return begin == 0 || end == 0
                    ? _listDuration.Count
                    : _listDuration.Count(x => begin <= x.End && end >= x.Begin);
            return begin == 0 || end == 0
                ? _listDuration.Count(x => x.Stack == stack)
                : _listDuration.Count(x => x.Stack == stack && begin <= x.End && end >= x.Begin);
        }

        public List<Duration> AllDurations(long begin = 0, long end = 0) //for use only on cloned storages
        {
            return begin == 0 || end == 0
                ? _listDuration.ToList()
                : _listDuration.Where(x => begin <= x.End && end >= x.Begin)
                    .Select(x => new Duration(begin > x.Begin ? begin : x.Begin, end < x.End ? end : x.End, x.Stack))
                    .ToList();
        }

        public bool Ended()
        {
            return _listDuration[_listDuration.Count - 1].End != long.MaxValue;
        }
    }
}