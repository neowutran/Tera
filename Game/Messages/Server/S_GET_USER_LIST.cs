using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;

namespace Tera.Game.Messages
{
    public class S_GET_USER_LIST : ParsedMessage
    {
        internal S_GET_USER_LIST(TeraMessageReader reader) : base(reader)
        {
            var count = reader.ReadUInt16();
            var offset = reader.ReadUInt16();
            for (var i = 1; i <= count; i++)
            {
                reader.BaseStream.Position = offset-4;
                var pointer = reader.ReadUInt16();
                Debug.Assert(pointer==offset);//should be the same
                var nextOffset = reader.ReadUInt16();
                reader.Skip(14);
                var gNameOffset = reader.ReadUInt16();
                var playerId = reader.ReadUInt32();
                //if (reader.Factory.ReleaseVersion >= 4000)
                //{ //no guildid on classic, will get id from userguildlogo
                //    reader.Skip(reader.Factory.ReleaseVersion < 6200 ? 286 : 294);
                //    if (reader.Factory.ReleaseVersion >= 6603) reader.Skip(121); //added accessory transformation
                //    var guildId = reader.ReadUInt32();
                //    PlayerGuilds.Add(playerId, guildId);
                //    reader.BaseStream.Position = gNameOffset - 4;
                //    var gName = reader.ReadTeraString();
                //    PlayerGuildNames.Add(playerId, gName);
                //}
                offset = nextOffset;
            }
        }

        public Dictionary<uint, uint> PlayerGuilds { get; } = new Dictionary<uint, uint>();
        public Dictionary<uint, string> PlayerGuildNames { get; } = new Dictionary<uint, string>();
    }
}