namespace Tera.Game
{
    public class NpcInfo
    {
        public NpcInfo(ushort huntingZoneId, uint templateId, bool boss, long hp, string name, string area)
        {
            HuntingZoneId = huntingZoneId;
            TemplateId = templateId;
            Name = name;
            Area = area;
            Boss = boss;
            HP = hp;
        }

        public ushort HuntingZoneId { get; }
        public uint TemplateId { get; }
        public string Name { get; }
        public string Area { get; }
        public bool Boss { get; internal set; }
        public long HP { get; internal set; }
    }
}