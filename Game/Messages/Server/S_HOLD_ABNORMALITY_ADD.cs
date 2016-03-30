using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Tera.Game.Messages
{

    public class S_HOLD_ABNORMALITY_ADD : ParsedMessage
    {
        internal S_HOLD_ABNORMALITY_ADD(TeraMessageReader reader) : base(reader)
        {
            PrintRaw();
        }
    }
}
