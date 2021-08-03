using System;

namespace Tera.Game.Messages
{
    public class S_CHAT : ParsedMessage
    {
        internal S_CHAT(TeraMessageReader reader) : base(reader)
        {
            UsernameOffset = reader.ReadUInt16();
            TextOffset = reader.ReadUInt16();
            var channel = reader.ReadInt32();
            Channel = (ChannelEnum)channel;
            reader.BaseStream.Position = UsernameOffset - 4;
            Username = reader.ReadTeraString();
            reader.BaseStream.Position = TextOffset - 4;
            Text = reader.ReadTeraString();
        }

        public ushort UsernameOffset { get; set; }
        public ushort TextOffset { get; set; }
        public string Username { get; set; }

        public string Text { get; set; }

        public ChannelEnum Channel { get; set; }

        public enum ChannelEnum
        {
            Say = 0,
            Group = 1,
            Guild = 2,
            Area = 3,
            Trading = 4,
            Whisper = 7,
            Greetings = 9,
            Bargain = 19,
            LFG = 20,
            TeamAlert = 22,
            System = 24,
            Emotes = 26,
            General = 27,
            Alliance = 28,
            Echelon = 29,
            Vanarch = 30,
            Raid = 32,
            RP = 212
        }
    }
}