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
            Dungeon = 1,
            Battleground = 2,
            Gathering = 3

        }

        public ulong Gold { get; private set; }
        public uint NumberCharacters { get; private set; }
        public uint NumberAccount { get; private set; }
        public string GuildName { get; private set; }
        public string GuildMaster { get; private set; }
        public uint GuildLevel { get; private set; }

        public uint NumberQuestsDone { get; private set; }
        public uint NumberTotalDailyQuest { get; private set; }
        public uint MaxNumberAccount { get; private set; }

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
                "MaxNumberAccount:"+MaxNumberAccount+"\n"
                ;

        }

        public GuildQuest ActiveQuest()
        {
            return GuildQuests.Where(x => x.Active == true).FirstOrDefault();
        }


        public List<GuildQuest> GuildQuests { get; private set; }

        internal S_GUILD_QUEST_LIST(TeraMessageReader reader) : base(reader)
        {
            // PrintRaw();
            GuildQuests = new List<GuildQuest>();
            var counter = reader.ReadUInt16();
            var offset = reader.ReadUInt16();
            var pruk = reader.ReadBytes(12);
            Console.WriteLine(BitConverter.ToString(pruk));

            GuildLevel = reader.ReadUInt32();
             pruk = reader.ReadBytes(16);
            Console.WriteLine(BitConverter.ToString(pruk));

            Gold = reader.ReadUInt64();
            NumberCharacters = reader.ReadUInt32();
            NumberAccount = reader.ReadUInt32();
            pruk = reader.ReadBytes(12);
            Console.WriteLine(BitConverter.ToString(pruk));


            NumberQuestsDone = reader.ReadUInt32();
            NumberTotalDailyQuest = reader.ReadUInt32();
            GuildName = reader.ReadTeraString();
            GuildMaster = reader.ReadTeraString();
            MaxNumberAccount = reader.ReadUInt16();
            var baseNumberUnknowByte = (int)(offset - reader.BaseStream.Position);
            var baseunknow = reader.ReadBytes(baseNumberUnknowByte);
            Console.WriteLine(BitConverter.ToString(baseunknow));
            Console.WriteLine(ToString());



            for (var i = 1; i <= counter; i++)
            {
               
                reader.BaseStream.Position = offset - 4;
                var pointer = reader.ReadUInt16();
                //Debug.Assert(pointer == offset);//should be the same
                var nextOffset = reader.ReadUInt16();
                var hexnext = nextOffset.ToString("X");
                var unknow = reader.ReadBytes(43);
                var guildQuestType1 = (GuildQuestType) reader.ReadUInt32();
                var unknowXXX = reader.ReadBytes(4);

                Console.WriteLine(BitConverter.ToString(unknow));
                Console.WriteLine(BitConverter.ToString(unknowXXX));

                var active = reader.ReadByte();
                Console.WriteLine("Active:" + active);
                var activeBool = active == 1;

                var guildQuestDescriptionLabel = reader.ReadTeraString();
                var guildQuestTitleLabel = reader.ReadTeraString();
                var questguildname = reader.ReadTeraString();
              

                var uk = reader.ReadBytes(4);
                Console.WriteLine(BitConverter.ToString(uk));
                var zoneId = reader.ReadUInt32();
                var monsterId = reader.ReadUInt32();
                var countQuest = reader.ReadUInt16();
                var unknowshort = reader.ReadUInt16();
                Console.WriteLine("unknow short:" + unknowshort);

                var totalQuest = reader.ReadUInt16();
           

                uk = reader.ReadBytes(10);
                Console.WriteLine(BitConverter.ToString(uk));

                var questGold = reader.ReadUInt64();
                uk = reader.ReadBytes(8);
                Console.WriteLine(BitConverter.ToString(uk));
                var questXP = reader.ReadUInt64();

                var count = (int)(nextOffset - reader.BaseStream.Position);
                if (nextOffset == 0) {
                    count = (int)(reader.BaseStream.Length - reader.BaseStream.Position);
                }

                unknow = reader.ReadBytes(count);
                Console.WriteLine(BitConverter.ToString(unknow));
                offset = nextOffset;


                var quest = new GuildQuest(
               guildQuestType1,
               guildQuestDescriptionLabel,
               guildQuestTitleLabel,
               questguildname,
               zoneId,
               monsterId,
               totalQuest,
               countQuest,
               activeBool, 
               questGold, 
               questXP
               );
                GuildQuests.Add(quest);
                Console.WriteLine(quest);


            }
        }

    }
}

