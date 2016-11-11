using System;
using System.Diagnostics;

namespace Tera.Game.Messages
{
    public class SAbnormalityBegin : ParsedMessage
    {
        internal SAbnormalityBegin(TeraMessageReader reader) : base(reader)
        {
            TargetId = reader.ReadEntityId();
            SourceId = reader.ReadEntityId();
            AbnormalityId = reader.ReadInt32();
            Duration = reader.ReadInt32();
            Stack = reader.ReadInt32();

            if (AbnormalityId == 700600 || AbnormalityId == 700601 || AbnormalityId == 700602 || AbnormalityId == 700603 || AbnormalityId == 700700 || AbnormalityId == 700701 || AbnormalityId == 700630 || AbnormalityId == 700631 || AbnormalityId == 601 || AbnormalityId == 603 || AbnormalityId == 602)
            {
                Console.WriteLine("Good: " + AbnormalityId);
            }
        }

        public int Duration { get; }

        public int Stack { get; }

        public int AbnormalityId { get; }


        public EntityId TargetId { get; }
        public EntityId SourceId { get; }
    }
}