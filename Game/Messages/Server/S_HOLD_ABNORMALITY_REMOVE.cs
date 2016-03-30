using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tera.Game.Messages
{
 
    public class S_HOLD_ABNORMALITY_REMOVE : ParsedMessage
    {
        internal S_HOLD_ABNORMALITY_REMOVE(TeraMessageReader reader) : base(reader)
        {
            PrintRaw();
        }
    }
}

