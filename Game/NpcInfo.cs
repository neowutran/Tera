using System;

namespace Tera.Game
{
    public class NpcInfo
    {
        public ushort HuntingZoneId { get;private set; }
        public uint TemplateId { get;private set; }
        public string Name { get; private set; }
        public string Area { get; private set; }
        public bool Boss { get; internal set; }
        public int HP { get; private set; }

        public NpcInfo(ushort huntingZoneId, uint templateId,bool boss, int hp, string name, string area)
        {
            HuntingZoneId = huntingZoneId;
            TemplateId = templateId;
            Name = name;
            Area = area;
            Boss = boss;
            HP = hp;
        }
    }
}
