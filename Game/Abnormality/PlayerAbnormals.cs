using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tera.Game
{
    public class PlayerAbnormals
    {
        public Dictionary<HotDot, AbnormalityDuration> Times;
        public Death Death;
        private Dictionary<NpcEntity, Death> _aggro;

        public Death Aggro(NpcEntity entity)
        {
            Death death = null;
            if (entity != null) _aggro.TryGetValue(entity, out death);
            if (death == null) death = new Death();
            return death;
        }
        public PlayerAbnormals()
        {
            Times = new Dictionary<HotDot, AbnormalityDuration>();
            Death = new Death();
            _aggro = new Dictionary<NpcEntity, Death>();
        }
        public PlayerAbnormals(Dictionary<HotDot, AbnormalityDuration> times, Death death, Dictionary<NpcEntity, Death> aggro)
        {
            Times = times;
            Death = death;
            _aggro = aggro;
        }
    }
}
