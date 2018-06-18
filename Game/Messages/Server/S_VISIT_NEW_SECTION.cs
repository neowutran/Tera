using System.Diagnostics;

namespace Tera.Game.Messages
{
    public class S_VISIT_NEW_SECTION : ParsedMessage
    {
        internal S_VISIT_NEW_SECTION(TeraMessageReader reader)
            : base(reader)
        {
            IsFirsVisit = reader.ReadBoolean();
            MapId = reader.ReadUInt32();
            GuardId = reader.ReadUInt32();
            SectionId = reader.ReadUInt32();
            Debug.Print("S_VISIT_NEW_SECTION");
        }

        public bool IsFirsVisit { get; set; }
        public uint MapId { get; set; }
        public uint GuardId { get; set; }
        public uint SectionId { get; set; }
    }
}