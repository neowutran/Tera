namespace Tera.Game.Messages
{
    public class S_GET_USER_GUILD_LOGO : ParsedMessage
    {
        internal S_GET_USER_GUILD_LOGO(TeraMessageReader reader) : base(reader)
        {
            var iconOffset = reader.ReadUInt16();
            var iconsize = reader.ReadUInt16();
            PlayerId = reader.ReadUInt32();
            GuildId = reader.ReadUInt32();
            Logo = reader.ReadBytes(iconsize);
            //System.IO.File.WriteAllBytes($"q:\\{Time.Ticks}.bin", Logo);
        }

        public uint GuildId { get; }
        public uint PlayerId { get; }
        public byte[] Logo { get; }
    }
}