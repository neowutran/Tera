using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tera.Game
{
    public class Death
    {
        private AbnormalityDuration _death;

        public Death()
        {
        }
        public Death(AbnormalityDuration death)
        {
            _death = death;
        }

        public Death Clone()
        {
            return new Death((AbnormalityDuration)_death?.Clone());
        }
        public int Count(long begin, long end)
        {
            if(_death == null)
            {
                return 0;
            }
            return _death.Count(begin, end);
        }

        public long Duration(long begin, long end)
        {
            if(_death == null)
            {
                return 0;
            }
            return _death.Duration(begin, end);
        }

        public void Start(long begin)
        {
            if(_death == null)
            {
                _death = new AbnormalityDuration(PlayerClass.Common, begin);
                return;
            }
            _death.Start(begin);
        }

        public void End(long begin)
        {
            if(_death == null)
            {
                return;
            }
            _death.End(begin);
        }
        public Death Clear()
        {
            var death=_death.Ended() ? null : new AbnormalityDuration(PlayerClass.Common,_death.LastStart());
            return new Death(death);
        }
        public bool Dead => !_death.Ended();
    }
}
