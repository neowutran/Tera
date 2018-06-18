using System.Collections.Generic;
using System.Diagnostics;

namespace Tera.Game.Messages
{
    public struct PartyMember
    {
        public uint ServerId;
        public uint PlayerId;
        public uint Level;
        public PlayerClass PlayerClass;
        public byte Status;
        public EntityId Id;
        public uint Order;
        public byte CanInvite;
        public uint unk1;
        public string Name;
    }

    public class S_PARTY_MEMBER_LIST : ParsedMessage
    {
        internal S_PARTY_MEMBER_LIST(TeraMessageReader reader) : base(reader)
        {
            var count = reader.ReadUInt16();
            var offset = reader.ReadUInt16();
            Ims = reader.ReadBoolean();
            Raid = reader.ReadBoolean();
            reader.Skip(12);
            LeaderServerId = reader.ReadUInt32();
            LeaderPlayerId = reader.ReadUInt32();
            reader.Skip(19);
            for (var i = 1; i <= count; i++)
            {
                reader.BaseStream.Position = offset - 4;
                var pointer = reader.ReadUInt16();
                Debug.Assert(pointer == offset);//should be the same
                var nextOffset = reader.ReadUInt16();
                var nameoffset = reader.ReadUInt16();
                var ServerId = reader.ReadUInt32();
                var PlayerId = reader.ReadUInt32();
                var Level = reader.ReadUInt32();
                var PlayerClass = (PlayerClass) (reader.ReadInt32() + 1);
                var Status = reader.ReadByte();
                var Id = reader.ReadEntityId();
                var Order = reader.ReadUInt32();
                var CanInvite = reader.ReadByte();
                var unk1 = reader.ReadUInt32();
                // var unk2 = reader.ReadUInt32(); //probably awakened status, appeared with KR awakening update
                reader.BaseStream.Position = nameoffset - 4;
                var Name = reader.ReadTeraString();
                offset = nextOffset;
                Party.Add(new PartyMember
                {
                    ServerId = ServerId,
                    PlayerId = PlayerId,
                    Level = Level,
                    PlayerClass = PlayerClass,
                    Status = Status,
                    Id = Id,
                    Order = Order,
                    CanInvite = CanInvite,
                    unk1 = unk1,
                    Name = Name
                });
            }
            ;
            //Debug.WriteLine($"leader:{BitConverter.ToString(BitConverter.GetBytes(LeaderPlayerId))}, party:");
            //foreach (PartyMember member in Party)
            //{
            //    Debug.WriteLine($"{member.PlayerClass} {BitConverter.ToString(BitConverter.GetBytes(member.PlayerId))} {member.Name} :{member.Id.ToString()} caninvite: {member.CanInvite} Status: {member.Status}");
            //}
        }

        public uint LeaderServerId { get; }
        public uint LeaderPlayerId { get; }
        public List<PartyMember> Party { get; } = new List<PartyMember>();
        public bool Ims;
        public bool Raid;
    }
}