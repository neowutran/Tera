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

            //if (OpCodeName.Contains("S_DUNGEON_STATUS") || OpCodeName.Contains("S_DUNGEON_RANK_END_POINT") || OpCodeName.Contains("S_DUNGEON_CLEAR") || OpCodeName.Contains("S_DUNGEON_UI_HIGHLIGHT") || OpCodeName.Contains("S_DUNGEON_EVENT_MESSAGE") || OpCodeName.Contains("S_DUNGEON_EVENT_GAGE"))
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