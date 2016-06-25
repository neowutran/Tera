namespace Tera.Game.Messages
{
    public class SAddCharmStatus : ParsedMessage
    {
        internal SAddCharmStatus(TeraMessageReader reader) : base(reader)
        {
            TargetId = reader.ReadEntityId();
            CharmId = reader.ReadUInt32();
            Status = reader.ReadByte();
            Duration = reader.ReadInt32();
//            Debug.WriteLine("target = "+TargetId+";Charm:"+CharmId+";Duration:"+Duration+";Status:"+Status);
        }

        public EntityId TargetId { get; }
        public uint CharmId { get; }
        public byte Status { get; }
        public int Duration { get; }
    }
}