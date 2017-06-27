using System.Diagnostics;

namespace Tera.Game.Messages
{
    public class S_AVAILABLE_EVENT_MATCHING_LIST : ParsedMessage

    {
        internal S_AVAILABLE_EVENT_MATCHING_LIST(TeraMessageReader reader) : base(reader)
        {
            reader.Skip(63);
            //Badges = reader.ReadInt32();
            Credits = reader.ReadInt16();
        }

        public int Badges { get; private set; }
        public int Credits { get; private set; }
    }
}