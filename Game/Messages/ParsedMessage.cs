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

            //if (OpCodeName.Contains("S_BAN_PARTY") || OpCodeName == "PARTY_INFO" || OpCodeName == "S_BAN_PARTY_MEMBER")
            //{
            //    PrintRaw();
            //}

            //    Debug.WriteLine(OpCodeName);

            //if (OpCodeName == "S_SKILL_TARGETING_AREA" )
            //{
            //    PrintRaw();
            //}

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