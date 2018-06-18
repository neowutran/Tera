using System.Collections.Generic;
using System.Diagnostics;

namespace Tera.Game.Messages
{
    public struct LfgListing
    {
        public string Message;
        public string LeaderName;
        public uint LeaderId;
        public bool IsRaid;
        public uint PlayerCount;
    }

    public class S_SHOW_PARTY_MATCH_INFO : ParsedMessage
    {
        internal S_SHOW_PARTY_MATCH_INFO(TeraMessageReader reader) : base(reader)
        {
            var count = reader.ReadUInt16();
            var offset = reader.ReadUInt16();

            PageCurrent = reader.ReadUInt16();
            PageCount = reader.ReadUInt16();
            
            for (var i = 1; i <= count; i++)
            {
                reader.BaseStream.Position = offset - 4;
                var pointer = reader.ReadUInt16();
                Debug.Assert(pointer == offset);//should be the same
                var nextOffset = reader.ReadUInt16();
                
                var messageoffset = reader.ReadUInt16();
                var leaderoffset = reader.ReadUInt16();
                var leaderId = reader.ReadUInt32();
                var isRaid = reader.ReadBoolean();
                var playerCount = reader.ReadUInt16();
                
                reader.BaseStream.Position = messageoffset - 4;
                var message = reader.ReadTeraString();

                reader.BaseStream.Position = leaderoffset - 4;
                var leaderName = reader.ReadTeraString();

                offset = nextOffset;
                Listings.Add(new LfgListing()
                {
                    Message = message,
                    LeaderName = leaderName,
                    LeaderId = leaderId,
                    IsRaid = isRaid,
                    PlayerCount = playerCount,
                });
            }
        }

        public uint PageCurrent { get; }
        public uint PageCount { get; }

        public List<LfgListing> Listings { get; } = new List<LfgListing>();
        
    }
}