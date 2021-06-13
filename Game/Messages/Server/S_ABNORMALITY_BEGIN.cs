using System;
using System.Diagnostics;

namespace Tera.Game.Messages
{
    public class SAbnormalityBegin : ParsedMessage
    {
        internal SAbnormalityBegin(TeraMessageReader reader) : base(reader) {
            if (reader.Factory.ReleaseVersion >= 10700 ||
                reader.Factory.ReleaseVersion==9901 && reader.Factory.Version!=381995) reader.ReadInt32();//offset & counter to effects list, 381995 - last 106.02 presented as 99.01
            TargetId = reader.ReadEntityId();
            SourceId = reader.ReadEntityId();
            AbnormalityId = reader.ReadInt32();
            Duration = reader.ReadInt64();
            Stack = reader.ReadInt32();
        }

        public long Duration { get; }

        public int Stack { get; }

        public int AbnormalityId { get; }


        public EntityId TargetId { get; }
        public EntityId SourceId { get; }
    }
}