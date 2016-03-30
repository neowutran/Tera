using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tera.Game.Messages
{
    public class S_CLEAR_ALL_HOLDED_ABNORMALITY : ParsedMessage
    {
        internal S_CLEAR_ALL_HOLDED_ABNORMALITY(TeraMessageReader reader) : base(reader)
        {
            PrintRaw();
        }
    }
}
