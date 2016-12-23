using System.Diagnostics;

namespace Tera.Game.Messages
{
    public class SNpcStatus : ParsedMessage

    {
        internal SNpcStatus(TeraMessageReader reader) : base(reader)
        {
            Npc = reader.ReadEntityId();
            Enraged = (reader.ReadByte() & 1) == 1;
            Defeated = reader.ReadInt32()==5;//maybe, need test
            Target = reader.ReadEntityId();
            //Debug.WriteLine("NPC:" + Npc + ";Target:" + Target + (Enraged?" Enraged":""));
        }

        public EntityId Npc { get; }
        public bool Enraged { get; }
        public bool Defeated { get; }
        public EntityId Target { get; }
    }
}