using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tera.Game.Messages
{
    public enum RunemarksActionType
    {
        Normal = 0,
        Detonate = 1,
        Expired = 2,
        Reclaimed = 3
    }
    /// <summary>
    /// Packet used for valkyrie's runemarks
    /// </summary>
    public class S_WEAK_POINT : ParsedMessage
    {
        private uint type;

        public EntityId Target { get; private set; }
        public uint RunemarksBefore { get; private set; }
        public uint RunemarksAfter { get; private set; }
        public RunemarksActionType Type => (RunemarksActionType) type;
        public uint SkillId { get; private set; }

        internal S_WEAK_POINT(TeraMessageReader reader) : base(reader)
        {
            Target = reader.ReadEntityId();
            RunemarksBefore = reader.ReadUInt32();
            RunemarksAfter = reader.ReadUInt32();
            type = reader.ReadUInt32();
            SkillId = reader.ReadUInt32();
        }
    }
}
