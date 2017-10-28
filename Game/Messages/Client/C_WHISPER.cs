using System.Collections.Generic;
using System.Diagnostics;

namespace Tera.Game.Messages
{
    public class C_WHISPER : ParsedMessage
    {
        internal C_WHISPER(TeraMessageReader reader) : base(reader)
        {
            TargetOffset = reader.ReadUInt16();
            TextOffset = reader.ReadUInt16();
            Target = reader.ReadTeraString();
            Text = reader.ReadTeraString();


        }

        public ushort TargetOffset { get; set; }
        public ushort TextOffset { get; set; }
        public string Target { get; set; }
        public string Text { get; set; }

    }
}