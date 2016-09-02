namespace Tera.Game.Messages
{
    public class S_CHAT : ParsedMessage
    {
        internal S_CHAT(TeraMessageReader reader) : base(reader)
        {
            reader.Skip(4);//offsets
            Channel = reader.ReadUInt32();
            reader.Skip(11);
            Username = reader.ReadTeraString();
            Text = reader.ReadTeraString();
        }

        public string Username { get; set; }

        public string Text { get; set; }

        public uint Channel { get; set; }
    }
}