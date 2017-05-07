namespace Tera.Game.Messages
{
    public class S_START_COOLTIME_SKILL : ParsedMessage
    {
        internal S_START_COOLTIME_SKILL(TeraMessageReader reader) : base(reader)
        {
            //PrintRaw();
            SkillId = reader.ReadInt32() & 0x3FFFFFF;
            Cooldown = reader.ReadInt32();
            //Debug.WriteLine("cooldown: SkillId = "+SkillId+"; Cooldown:"+Cooldown+"; hasResetted:"+HasResetted);
        }

        public int SkillId { get; }
        public int Cooldown { get; }

        public bool HasResetted => Cooldown == 0;
    }
}