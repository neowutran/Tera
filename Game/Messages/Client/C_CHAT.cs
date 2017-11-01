using System.Collections.Generic;
using System.Diagnostics;

namespace Tera.Game.Messages
{
    public class C_CHAT : ParsedMessage
    {
        internal C_CHAT(TeraMessageReader reader) : base(reader)
        {
            TextOffset = reader.ReadUInt16();
            Channel = reader.ReadInt32();
            Text = reader.ReadTeraString();


        }

        public ushort TextOffset { get; set; }
        public int Channel { get; set; }
        public string Text { get; set; }

    }
}