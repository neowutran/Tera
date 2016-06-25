using System.Collections.Generic;

namespace Tera.Game.Messages
{
    public class SResetCharmStatus : ParsedMessage
    {
        internal SResetCharmStatus(TeraMessageReader reader) : base(reader)
        {
            var count = reader.ReadUInt16();
            var offset = reader.ReadUInt16();
            TargetId = reader.ReadEntityId();
            for (var i = 1; i <= count; i++)
            {
                reader.Skip(2); // offset pointer
                reader.Skip(2); // next member offset
                var charmId = reader.ReadUInt32();
                var duration = reader.ReadUInt32();
                var status = reader.ReadByte();
                Charms.Add(new CharmStatus {Status = status, CharmId = charmId, Duration = duration});
            }
            ;
            //Debug.WriteLine($"target:{BitConverter.ToString(BitConverter.GetBytes(TargetId.Id))}, Charms:");
            //foreach (CharmStatus charm in Charms)
            //{
            //    Debug.WriteLine($"charmid:{charm.CharmId} duration: {charm.Duration} Status: {charm.Status}");
            //}
        }

        public EntityId TargetId { get; }
        public List<CharmStatus> Charms { get; } = new List<CharmStatus>();
    }
}