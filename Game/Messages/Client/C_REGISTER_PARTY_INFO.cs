using System.Collections.Generic;
using System.Diagnostics;

namespace Tera.Game.Messages
{
    public class C_REGISTER_PARTY_INFO : ParsedMessage
    {
        internal C_REGISTER_PARTY_INFO(TeraMessageReader reader) : base(reader)
        {
            var offset = reader.ReadUInt16();
            
            IsRaid = reader.ReadBoolean();
            
            reader.BaseStream.Position = offset - 4;
            Message = reader.ReadTeraString();
        }

        public bool IsRaid { get; }
        public string Message { get; }
    }
}