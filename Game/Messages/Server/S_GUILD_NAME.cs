namespace Tera.Game.Messages
{
    public class S_GUILD_NAME : ParsedMessage
    {
        internal S_GUILD_NAME(TeraMessageReader reader) : base(reader)
        {
            reader.Skip(8);
            UserId = reader.ReadEntityId();
            GuildName = reader.ReadTeraString();
            //we don't need the rest now, uncomment if needed
            //GuildRank = reader.ReadTeraString();
            //GuildTitle = reader.ReadTeraString();
            //GuildLogo = reader.ReadTeraString();
        }

        public EntityId UserId { get; private set; }
        public string GuildName { get; private set; }
        public string GuildRank { get; private set; }
        public string GuildTitle { get; private set; }
        public string GuildLogo { get; private set; }
    }
}