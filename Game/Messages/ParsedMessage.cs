using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

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

            var regex = new Regex(@"^C_");
            var match = regex.Match(OpCodeName);
            if (match.Success)
                PrintRaw();
        }

        public byte[] Raw { get; protected set; }

        public string OpCodeName { get; }

        public void PrintRaw()
        {
            Debug.WriteLine(OpCodeName + ": ");
            Debug.WriteLine(BitConverter.ToString(Raw));
        }
    }
}