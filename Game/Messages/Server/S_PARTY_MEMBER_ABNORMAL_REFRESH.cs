namespace Tera.Game.Messages
{
    public class SPartyMemberAbnormalRefresh : ParsedMessage
    {
        internal SPartyMemberAbnormalRefresh(TeraMessageReader reader) : base(reader)
        {
            //   PrintRaw();
            ServerId = reader.ReadUInt32();
            PlayerId = reader.ReadUInt32();
            AbnormalityId = reader.ReadInt32();
            Duration = reader.ReadInt64();
            StackCounter = reader.ReadInt32();

            //Debug.WriteLine("Target:"+TargetId+";Abnormality:"+AbnormalityId+";Duration:"+Duration+";Uknow:"+Unknow+";Stack:"+StackCounter);
        }

        public long Duration { get; }

        public int Unknow { get; }


        public int StackCounter { get; }

        public int AbnormalityId { get; }

        public uint ServerId { get; }
        public uint PlayerId { get; }
    }
}