using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tera.Game.Messages;

namespace Tera.Game
{
    public class GuildQuest
    {
        public S_GUILD_QUEST_LIST.GuildQuestType GuildQuestType1 { get; private set; }
        public string DescriptionLabel { get; private set; }
        public string TitleLabel { get; private set; }
        public string GuildName { get; private set; }
        public uint ZoneId { get; private set; }
        public ushort Total { get; private set; }
        public ulong MonsterId { get; private set; }
        public bool Active { get; private set; }
        public ulong Gold { get; private set; }
        public ushort Count { get; private set; }
        public ulong XP { get; private set; }
        public GuildQuest(
            S_GUILD_QUEST_LIST.GuildQuestType guildQuestType1,
            string descriptionLabel,
            string titleLabel,
            string guildName,
            uint zoneId,
            ulong monsterId,
            ushort total,
            ushort count,
            bool active,
            ulong gold,
            ulong xP

            )
        {
            GuildQuestType1 = guildQuestType1;
            DescriptionLabel = descriptionLabel;
            TitleLabel = titleLabel;
            GuildName = guildName;
            ZoneId = zoneId;
            MonsterId = monsterId;
            Total = total;
            Active = active;
            Gold = gold;
            Count = count;
            XP = xP;
        

        }

        public override string ToString()
        {
            return "GuildQuestType1: " + GuildQuestType1 + "\n" +
                "GuildQuestDescriptionLabel:" + DescriptionLabel + "\n" +
                "GuildQuestTitleLabel:" + TitleLabel + "\n" +
                "GuildName:" + GuildName + "\n" +
                "ZoneId:" + ZoneId + "\n" +
                "MonsterId:" + MonsterId + "\n" +
                "Count:" + Count +"/"+ Total + "\n" +
                "Active:" + Active + "\n" +
                "Gold:" + Gold + "\n" +
                "XP:" + XP + "\n"
                ;
                
        }

    }
}
