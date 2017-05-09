namespace Tera.Game.Messages
{
    public class SAbnormalityRefresh : ParsedMessage
    {
        internal SAbnormalityRefresh(TeraMessageReader reader) : base(reader)
        {
            TargetId = reader.ReadEntityId();
            AbnormalityId = reader.ReadInt32();
            Duration = reader.ReadUInt32();
            Unknow = reader.ReadInt32();
            StackCounter = reader.ReadInt32();

//            Debug.WriteLine("Target:"+TargetId+";Abnormality:"+AbnormalityId+";Duration:"+Duration+";Uknow:"+Unknow+";Stack:"+StackCounter);
        }

        public uint Duration { get; }

        public int Unknow { get; }


        public int StackCounter { get; }

        public int AbnormalityId { get; }

        public EntityId TargetId { get; }
    }
}