using System.Diagnostics;

namespace Tera.Game.Messages
{
    public class S_PRIVATE_CHAT : ParsedMessage
    {
        internal S_PRIVATE_CHAT(TeraMessageReader reader) : base(reader)
        {
            AuthorNameOffset = reader.ReadUInt16();
            TextOffset = reader.ReadUInt16();
            Channel = reader.ReadUInt32();
            AuthorId = reader.ReadUInt64();
            AuthorName = reader.ReadTeraString();
            Text = reader.ReadTeraString();
            Debug.WriteLine("Channel:"+Channel+";Username:"+AuthorName+";Text:"+Text+";AuthorId:"+AuthorId);
        }
        public ushort AuthorNameOffset { get; set; }
        public ushort TextOffset { get; set; }
        public string AuthorName { get; set; }

        public ulong AuthorId { get; set; }

        public string Text { get; set; }

        public uint Channel { get; set; }
    }
}