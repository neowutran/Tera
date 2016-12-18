using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Tera.Game.Messages
{
    public class S_SYSTEM_MESSAGE : ParsedMessage
    {
        internal S_SYSTEM_MESSAGE(TeraMessageReader reader) : base(reader)
        {
           // PrintRaw();
        }

    }
}