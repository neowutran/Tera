using System;

namespace Tera.Game.Messages
{
    public class S_CHAT : ParsedMessage
    {
        internal S_CHAT(TeraMessageReader reader) : base(reader)
        {
            //    reader.Skip(2);
            Canal = reader.ReadBytes(6);
            reader.Skip(13);
            Username = reader.ReadTeraString();
            Text = reader.ReadTeraString();
        }

        public string Username { get; set; }

        public string Text { get; set; }

        public byte[] Canal { get; set; }
    }
}