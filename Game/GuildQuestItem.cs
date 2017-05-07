namespace Tera.Game
{
    public class GuildQuestItem
    {
        public GuildQuestItem(uint itemId, ulong amount)
        {
            ItemId = itemId;
            Amount = amount;
        }

        public uint ItemId { get; }
        public ulong Amount { get; }


        public override string ToString()
        {
            return "ItemId:" + ItemId + ";Amount:" + Amount;
        }
    }
}