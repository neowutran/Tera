namespace Tera.Game.Messages
{
    public class SPartyMemberCharmAdd : ParsedMessage
    {
        internal SPartyMemberCharmAdd(TeraMessageReader reader) : base(reader)
        {
            TargetId = reader.ReadEntityId();
            CharmId = reader.ReadUInt32();
            Duration = reader.ReadInt32();
            Status = reader.ReadByte();
            //   Console.WriteLine("target = "+TargetId+";Charm:"+CharmId+";Duration:"+Duration+";Status:"+Status);
        }

        public EntityId TargetId { get; }
        public uint CharmId { get; }
        public byte Status { get; }
        public int Duration { get; }
    }
}