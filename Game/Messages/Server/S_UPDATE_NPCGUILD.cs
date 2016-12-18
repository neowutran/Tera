using System.Diagnostics;
namespace Tera.Game.Messages
{
    public class S_UPDATE_NPCGUILD : ParsedMessage
    {
        public enum NpcGuildType
        {
		    Vanguard=609
        }

        internal S_UPDATE_NPCGUILD(TeraMessageReader reader) : base(reader)
        {
            User=reader.ReadEntityId();
            reader.Skip(8);
            int type = reader.ReadInt32();
            Type = (NpcGuildType)type;
            reader.Skip(8);
            Credits = reader.ReadInt32();
            Debug.WriteLine("type:"+type+";translated:"+Type + "; Credits:"+Credits);
        }

        public EntityId User { get; private set; }
        public int Credits { get; private set; }
        public NpcGuildType Type { get; private set; }
    }
}

