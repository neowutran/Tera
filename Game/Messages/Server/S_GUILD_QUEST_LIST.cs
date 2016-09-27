using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
namespace Tera.Game.Messages
{
    public class S_GUILD_QUEST_LIST : ParsedMessage
    {
        public enum GuildQuestType
        {
            Hunt = 1,
            Battleground = 2,
            Gathering = 3

        }

        public enum GuildQuestType2
        {
            Hunt = 0,
            Battleground = 2,
            Gathering = 4

        }

        public enum QuestSizeType {
            Small = 0,
            Medium = 1,
            Big = 2
        }



        public ulong Gold { get; private set; }
        public uint NumberCharacters { get; private set; }
        public uint NumberAccount { get; private set; }
        public string GuildName { get; private set; }
        public string GuildMaster { get; private set; }
        public uint GuildLevel { get; private set; }

        public uint NumberQuestsDone { get; private set; }
        public uint NumberTotalDailyQuest { get; private set; }

        public override string ToString()
        {
            return "Guild name:" + GuildName + "\n" +
                "Guild master:" + GuildMaster + "\n" +
                "Guild level:" + GuildLevel + "\n" +
                "Number accounts:" + NumberAccount + "\n" +
                "Number characters:" + NumberCharacters + "\n" +
                "Gold:" + Gold + "\n"+
                "NumberTotalDailyQuest:"+NumberTotalDailyQuest+"\n"+
                "NumberQuestsDone:"+NumberQuestsDone+"\n"+
                "Current XP:"+ GuildXpCurrent+"\n"+
                "XP for next level:"+ GuildXpNextLevel+"\n"+
                "XP to gain for next level:"+ (GuildXpNextLevel - GuildXpCurrent)+"\n"+
                "Guild size:"+ GuildSize+"\n"+
                "Guild creation date:"+ EpochGuildCreationTime+"\n"
                ;

        }

        public GuildQuest ActiveQuest()
        {
            return GuildQuests.Where(x => x.Active == true).FirstOrDefault();
        }


        public List<GuildQuest> GuildQuests { get; private set; }

        public ulong GuildXpCurrent { get; private set; }
        public ulong GuildXpNextLevel { get; private set; }

        public GuildSizeType GuildSize { get; private set; }
        public ulong EpochGuildCreationTime { get; private set; }

        public enum GuildSizeType
        {
            Small = 0,
            Medium = 1,
            Big = 2
        }

        internal S_GUILD_QUEST_LIST(TeraMessageReader reader) : base(reader)
        {
            //PrintRaw();
            GuildQuests = new List<GuildQuest>();
            var counter = reader.ReadUInt16();
            var questOffset = reader.ReadUInt16();
            var guildNameOffset = reader.ReadUInt16();
            var guildMasterOffset = reader.ReadUInt16();

            var guildId = reader.ReadUInt32();
            var guildMasterId = reader.ReadInt32();

            GuildLevel = reader.ReadUInt32();
            GuildXpCurrent = reader.ReadUInt64();
            GuildXpNextLevel = reader.ReadUInt64();
            Gold = reader.ReadUInt64();
            NumberCharacters = reader.ReadUInt32();
            NumberAccount = reader.ReadUInt32();
            GuildSize = (GuildSizeType)reader.ReadUInt32();
            EpochGuildCreationTime = reader.ReadUInt64();
            NumberQuestsDone = reader.ReadUInt32();
            NumberTotalDailyQuest = reader.ReadUInt32();
            GuildName = reader.ReadTeraString();
            GuildMaster = reader.ReadTeraString();
            
            for (var i = 1; i <= counter; i++)
            {
               
                reader.BaseStream.Position = questOffset - 4;
                var pointer = reader.ReadUInt16();
                Debug.Assert(pointer == questOffset);//should be the same

                var nextOffset = reader.ReadUInt16();
                var countTargets = reader.ReadUInt16();
                var offsetTargets = reader.ReadUInt16();

                var countUnk2 = reader.ReadUInt16();
                var offsetUnk2 = reader.ReadUInt16();
                var countRewards = reader.ReadUInt16();
                var offsetRewards = reader.ReadUInt16();
                var offsetName = reader.ReadUInt16();
                var offsetDescription = reader.ReadUInt16();
                var offsetGuildName = reader.ReadUInt16();

                var id = reader.ReadUInt32();
                var questType2 = (GuildQuestType2)reader.ReadUInt32();
                var questSize = (QuestSizeType)reader.ReadUInt32();
                var unk3 = reader.ReadByte();
                var unk4 = reader.ReadUInt32();
                var unk5 = reader.ReadUInt32();

                //in seconds
                var timeRemaining = reader.ReadUInt32();

                var guildQuestType = (GuildQuestType) reader.ReadUInt32();
                var unk6 = reader.ReadInt32();
                var active = reader.ReadByte();
                var activeBool = active == 1;
                var guildQuestDescriptionLabel = reader.ReadTeraString();
                var guildQuestTitleLabel = reader.ReadTeraString();
                var questguildname = reader.ReadTeraString();
                var targetOffsetAgain = reader.ReadUInt16();

                var unk7 = reader.ReadUInt16();
                Debug.WriteLine(
                 ";unk3:" + unk3 +
                 ";unk4:" + unk4 +
                 ";unk5:" + unk5 +
                 ";countUnk2:" + countUnk2 +
                 ";offsetUnk2:" + offsetUnk2 +
                 ";unk6:" + unk6+
                 ";unk7:"+ unk7
                 );
                List<GuildQuestTarget> targets = new List<GuildQuestTarget>();
                for (var j = 1; j <= countTargets; j++)
                {
                    var zoneId = reader.ReadUInt32();
                    var targetId = reader.ReadUInt32();
                    var countQuest = reader.ReadUInt32();
                    var totalQuest = reader.ReadUInt32();
                    targets.Add(new GuildQuestTarget(zoneId, targetId, countQuest, totalQuest));
                    var currentPosition = reader.ReadUInt16();

                    //If 3 targets
                    //1rst iteration: offset of the "currentPosition" of the next iteration of this loop
                    //2nd iteration: offset = 0 (= continue the loop normally?)
                    //3nd iteration: offset of the "currentPosition" of reward
                    //fuck you

                    //If 1 target:
                    //offset of the "currentPosition" of the first reward element
                    var shitTargetOffset = reader.ReadUInt16();
                }

                for(var j = 1; j <= countUnk2; j++)
                {
                    Debug.WriteLine("unk2:" + reader.ReadByte().ToString("X")+" ;"+j+"/"+countUnk2);
                }

                List<GuildQuestItem> rewards = new List<GuildQuestItem>();
                for(var j = 1; j <= countRewards; j++)
                {
                    var item = reader.ReadUInt32();
                    var amount = reader.ReadUInt64();

                    rewards.Add(new GuildQuestItem(item, amount));

                    //IF IT S THE LAST QUEST:
                    //1rst iteration of the loop: currentPosition
                    //2nd iteration of the loop: = 0
                    var currentPosition = reader.ReadUInt16();
                 
                    if (j == countRewards && i == counter) { break; }

                    //IF IT S NOT THE LAST QUEST 
                    //1rst iteration of the loop: offset = 0 (= continue the loop normally?)
                    //2nd iteration of the loop: offset to the "currentPosition" of the next reward (from the next quest)

                    //IF IT S THE LAST QUEST: 
                    //1rst iteration of the loop: offset = 0 (= continue the loop normally)
                    //2nd iteration of the loop: DOES NOT EXIST
                    var shitRewardOffset = reader.ReadUInt16();
                }
           
           
                questOffset = nextOffset;

                var quest = new GuildQuest(
               guildQuestType,
               questType2,
               guildQuestDescriptionLabel,
               guildQuestTitleLabel,
               questguildname,
               targets,
               activeBool,
               rewards,
               timeRemaining,
               questSize

               );
                GuildQuests.Add(quest);
             
            }

            Debug.WriteLine(ToString());
        }

    }
}

