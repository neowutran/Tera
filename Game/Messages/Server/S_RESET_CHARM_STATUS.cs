using System;
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
            reader.Skip(2);//unknown 10-00
            for (var i = 1; i <= count; i++)
            {
                var unk1 = reader.ReadUInt16();
                var charmId = reader.ReadUInt32();
                var duration = reader.ReadUInt32();
                var status = reader.ReadByte();
                var unk2 = reader.ReadUInt16();
                Charms.Add(new CharmStatus { Unk1=unk1, Status=status, CharmId=charmId, Duration=duration,Unk2=unk2 } );
            };
            //Console.WriteLine($"target:{BitConverter.ToString(BitConverter.GetBytes(TargetId.Id))}, Charms:");
            //foreach (CharmStatus charm in Charms)
            //{
            //    Console.WriteLine($"{charm.Unk1} {charm.Unk2} charmid:{charm.CharmId} duration: {charm.Duration} Status: {charm.Status}");
            //}
        }
        public EntityId TargetId { get; }
        public List<CharmStatus> Charms { get; } = new List<CharmStatus>();
    }
}