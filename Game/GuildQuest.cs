using System;
using System.Collections.Generic;
using Tera.Game.Messages;

namespace Tera.Game
{
    public class GuildQuest
    {
        public GuildQuest(
            S_GUILD_QUEST_LIST.GuildQuestType guildQuestType1,
            S_GUILD_QUEST_LIST.GuildQuestType2 guildQuestType2,
            string descriptionLabel,
            string titleLabel,
            string guildName,
            List<GuildQuestTarget> targets,
            bool active,
            List<GuildQuestItem> rewards,
            ulong timeRemaining,
            S_GUILD_QUEST_LIST.QuestSizeType questSize
        )
        {
            GuildQuestType1 = guildQuestType1;
            GuildQuestType2 = guildQuestType2;
            DescriptionLabel = descriptionLabel;
            TitleLabel = titleLabel;
            GuildName = guildName;
            Active = active;
            Rewards = rewards;
            Targets = targets;
            TimeRemaining = TimeSpan.FromSeconds(timeRemaining);
            QuestSize = questSize;
        }

        public S_GUILD_QUEST_LIST.GuildQuestType GuildQuestType1 { get; }
        public S_GUILD_QUEST_LIST.GuildQuestType2 GuildQuestType2 { get; }
        public string DescriptionLabel { get; }

        public TimeSpan TimeRemaining { get; }

        public string TitleLabel { get; }
        public string GuildName { get; }
        public bool Active { get; }
        public S_GUILD_QUEST_LIST.QuestSizeType QuestSize { get; }

        public List<GuildQuestItem> Rewards { get; }

        public List<GuildQuestTarget> Targets { get; }

        public override string ToString()
        {
            var str = "GuildQuestType1: " + GuildQuestType1 + "\n" +
                      "GuildQuestType2:" + GuildQuestType2 + "\n" +
                      "GuildQuestDescriptionLabel:" + DescriptionLabel + "\n" +
                      "GuildQuestTitleLabel:" + TitleLabel + "\n" +
                      "GuildName:" + GuildName + "\n" +
                      "Active:" + Active + "\n" +
                      "Time remaining:" + TimeRemaining + "\n" +
                      "Quest size:" + QuestSize;

            foreach (var target in Targets)
                str += "\n-----\n" + target;

            foreach (var reward in Rewards)
                str += "\n-----\n" + reward;
            return str;
        }
    }
}