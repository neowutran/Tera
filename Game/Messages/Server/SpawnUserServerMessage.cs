using System.Diagnostics.Eventing.Reader;

namespace Tera.Game.Messages
{
    public class SpawnUserServerMessage : ParsedMessage
    {
        internal SpawnUserServerMessage(TeraMessageReader reader)
            : base(reader)
        {
            reader.Skip(reader.Factory.ReleaseVersion>=4500 ? 8 : 4);//not sure when account benefits array appeared there
            var nameOffset = reader.ReadUInt16();
            reader.Skip(reader.Factory.ReleaseVersion>=4500 ? 14 : 16); //not sure when they deleted guild title
            ServerId = reader.ReadUInt32();
            PlayerId = reader.ReadUInt32();
            Id = reader.ReadEntityId();
            Position = reader.ReadVector3f();
            Heading = reader.ReadAngle();
            reader.Skip(4);//relation
            RaceGenderClass = new RaceGenderClass(reader.ReadInt32());
            reader.Skip(11); // huntingZoneId, walkSpeed, runSpeed, actionMode, status, bool visible
            Dead = (reader.ReadByte() & 1) == 0;
            reader.Skip(reader.Factory.ReleaseVersion>=4500 ? 121 : 105); // not sure when they added dye colors
            Level = reader.ReadInt16();
            reader.BaseStream.Position=nameOffset-4;
            Name = reader.ReadTeraString();
            GuildName = reader.ReadTeraString();
            //Debug.WriteLine(Name + ":" + BitConverter.ToString(BitConverter.GetBytes(Id.Id))+ ":"+ ServerId.ToString()+" "+ BitConverter.ToString(BitConverter.GetBytes(PlayerId))+" "+Dead);
        }

        public int Level { get; private set; }
        public bool Dead { get; set; }
        public Angle Heading { get; set; }
        public Vector3f Position { get; set; }
        public EntityId Id { get; private set; }
        public uint ServerId { get; private set; }
        public uint PlayerId { get; private set; }
        public string Name { get; private set; }
        public string GuildName { get; private set; }
        public RaceGenderClass RaceGenderClass { get; }
    }
}