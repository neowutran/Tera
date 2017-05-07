using System;

namespace Tera.Game.Messages
{
    public class S_START_GUILD_QUEST : ParsedMessage
    {
        internal S_START_GUILD_QUEST(TeraMessageReader reader) : base(reader)
        {
            PrintRaw();
            var unkown = reader.ReadUInt16();
            var unknowByte = reader.ReadByte();
            QuestId = reader.ReadUInt32();
            Guildname = reader.ReadTeraString();
            Console.WriteLine("id:" + QuestId + ";guildname:" + Guildname + ";unkown:" + unkown + ";unknown:" +
                              unknowByte);
        }

        public uint QuestId { get; }
        public string Guildname { get; }
    }
}