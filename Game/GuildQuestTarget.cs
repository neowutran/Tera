namespace Tera.Game
{
    public class GuildQuestTarget
    {
        public GuildQuestTarget(uint zoneId, uint targetId, uint countQuest, uint totalQuests)
        {
            ZoneId = zoneId;
            TargetId = targetId;
            CountQuest = countQuest;
            TotalQuest = totalQuests;
        }

        public uint ZoneId { get; }
        public uint TargetId { get; }
        public uint CountQuest { get; }
        public uint TotalQuest { get; }

        public override string ToString()
        {
            return "ZoneId:" + ZoneId + ";TargetId:" + TargetId + ";countQuest:" + CountQuest + ";totalQuest:" +
                   TotalQuest;
        }
    }
}