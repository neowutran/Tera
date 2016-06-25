using System.Collections.Generic;

namespace Tera.Game.Messages
{
    public class SPartyMemberCharmReset : ParsedMessage
    {
        internal SPartyMemberCharmReset(TeraMessageReader reader) : base(reader)
        {
            var count = reader.ReadUInt16();
            var offset = reader.ReadUInt16();
            ServerId = reader.ReadUInt32();
            PlayerId = reader.ReadUInt32();
            for (var i = 1; i <= count; i++)
            {
                reader.Skip(4); //offset pointer & next member offset
                var charmId = reader.ReadUInt32();
                var duration = reader.ReadUInt32();
                var status = reader.ReadByte();
                Charms.Add(new CharmStatus {Status = status, CharmId = charmId, Duration = duration});
            }
            ;
            //    Debug.WriteLine($"target:{BitConverter.ToString(BitConverter.GetBytes(PlayerId))}, Charms:");
            //    foreach (CharmStatus charm in Charms)
            //    {
            //        Debug.WriteLine($"charmid:{charm.CharmId} duration: {charm.Duration} Status: {charm.Status}");
            //    }
        }

        public uint ServerId { get; }
        public uint PlayerId { get; }
        public List<CharmStatus> Charms { get; } = new List<CharmStatus>();
    }
}