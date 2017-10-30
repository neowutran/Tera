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
            reader.Skip(11);
            Username = reader.ReadTeraString();
            Text = reader.ReadTeraString();
        }

        public ushort UsernameOffset { get; set; }
        public ushort TextOffset { get; set; }
        public string Username { get; set; }

        public string Text { get; set; }

        public ChannelEnum Channel { get; set; }

        public enum ChannelEnum
        {
            Guild = 2,
            General = 27,
            Say = 0,
            Greetings = 9,
            Trading = 4,
            Emotes = 26,
            Alliance = 28,
            Area = 3,
            Group = 1,
            Raid = 32

        }
    }
}