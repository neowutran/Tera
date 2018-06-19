using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Tera.Game.Messages
{
    public enum MatchingType
    {
        Dungeon, Battleground
    }
    
    public class S_CHANGE_EVENT_MATCHING_STATE : ParsedMessage
    {
        internal S_CHANGE_EVENT_MATCHING_STATE(TeraMessageReader reader) : base(reader)
        {
            var count = reader.ReadUInt16();
            var offset = reader.ReadUInt16();
            
            Searching = reader.ReadBoolean();
            Type = reader.ReadBoolean() ? MatchingType.Dungeon : MatchingType.Battleground;
            
            for (var i = 1; i <= count; i++)
            {
                reader.BaseStream.Position = offset - 4;
                var pointer = reader.ReadUInt16();
                Debug.Assert(pointer == offset);//should be the same
                
                var nextOffset = reader.ReadUInt16();
                offset = nextOffset;
                MatchingEvents.Add(reader.ReadUInt32());
            }
        }

        public bool Searching { get; }
        public MatchingType Type { get; }
        public List<uint> MatchingEvents = new List<uint>();
    }
}