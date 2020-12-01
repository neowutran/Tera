namespace Tera.Game
{
    public class NpcInfo
    {
        public NpcInfo(ushort huntingZoneId, uint templateId, bool boss, long hp, string name, string area, long enrageThreshold=0)
        {
            HuntingZoneId = huntingZoneId;
            TemplateId = templateId;
            Name = name;
            Area = area;
            Boss = boss;
            HP = hp;
            EnrageThreshold = enrageThreshold;
        }

        public ushort HuntingZoneId { get; private set; }
        public uint TemplateId { get; private set; }
        public string Name { get; private set; }
        public string Area { get; private set; }
        public bool Boss { get; internal set; }
        public long HP { get; internal set; }
        public long EnrageThreshold { get; internal set; }
    }
}