using System;
using System.Diagnostics;

namespace Tera.Game.Messages
{
    // Base class for parsed messages
    public abstract class ParsedMessage : Message
    {
        internal ParsedMessage(TeraMessageReader reader)
            : base(reader.Message.Time, reader.Message.Direction, reader.Message.Data)
        {
            Raw = reader.Message.Payload.Array;
            OpCodeName = reader.OpCodeName;
        }

        public byte[] Raw { get; protected set; }

        public string OpCodeName { get; }

        public void PrintRaw()
        {
            Debug.WriteLine(OpCodeName + " : " + OpCode + " : " + Direction + " : Size " + Payload.Count + " : Time " + Time);
            Debug.WriteLine(BitConverter.ToString(Payload.Array));
        }
    }
}