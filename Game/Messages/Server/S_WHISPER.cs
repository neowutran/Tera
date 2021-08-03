namespace Tera.Game.Messages
{
    public class S_WHISPER : ParsedMessage
    {
        internal S_WHISPER(TeraMessageReader reader) : base(reader)
        {
            SenderOffset = reader.ReadUInt16();
            ReceiverOffset = reader.ReadUInt16();
            TextOffset = reader.ReadUInt16();
            reader.BaseStream.Position = SenderOffset - 4;
            Sender = reader.ReadTeraString();
            reader.BaseStream.Position = ReceiverOffset - 4;
            Receiver = reader.ReadTeraString();
            reader.BaseStream.Position = TextOffset - 4;
            Text = reader.ReadTeraString();
        }
        public ushort SenderOffset { get; set; }
        public ushort ReceiverOffset { get; set; }
        public ushort TextOffset { get; set; }
        public string Sender { get; set; }
        public string Receiver { get; set; }

        public string Text { get; set; }

        public byte[] Canal { get; set; }
    }
}