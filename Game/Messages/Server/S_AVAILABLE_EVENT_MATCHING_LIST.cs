using System.Diagnostics;

namespace Tera.Game.Messages
{
    public class S_AVAILABLE_EVENT_MATCHING_LIST : ParsedMessage

    {
        internal S_AVAILABLE_EVENT_MATCHING_LIST(TeraMessageReader reader) : base(reader)
        {
            reader.Skip(24);
            Badges = reader.ReadInt32();
            Credits = reader.ReadInt32();
            Debug.WriteLine(Type+" Badges:"+Badges+" Credits:"+Credits);
        }

        public int Badges { get; private set; }
        public int Credits { get; private set; }
        public static S_UPDATE_NPCGUILD.NpcGuildType Type = S_UPDATE_NPCGUILD.NpcGuildType.Vanguard;
    }
}