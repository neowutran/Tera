namespace Tera.Game.Messages
{
    public class S_START_COOLTIME_SKILL : ParsedMessage
    {
        internal S_START_COOLTIME_SKILL(TeraMessageReader reader) : base(reader)
        {
            SkillId = reader.ReadInt32() & 0x3FFFFFF;
            Cooldown = reader.ReadInt32();
        }

        public int SkillId { get; private set; }
        public int Cooldown { get; private set; }
    }
}