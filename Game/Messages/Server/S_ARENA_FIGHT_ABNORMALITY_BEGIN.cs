using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tera.Game.Messages
{
    public class S_ARENA_FIGHT_ABNORMALITY_BEGIN : ParsedMessage
    {
        internal S_ARENA_FIGHT_ABNORMALITY_BEGIN(TeraMessageReader reader) : base(reader)
        {
            PrintRaw();
        }
    }
}
