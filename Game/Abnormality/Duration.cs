using System;

namespace Tera.Game
{
    public class Duration : ICloneable
    {
        public Duration(long begin, long end)
        {
            End = end;
            Begin = begin;
        }

        public long Begin { get; }
        public long End { get; set; }

        public object Clone()
        {
            return new Duration(Begin, End);
        }
        public Duration Clone(long begin, long end)
        {
            return new Duration(Begin > begin ? Begin : begin, End < end ? End : end);
        }

    }
}