using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Tera.Game.Messages
{
    public class S_REQUEST_CONTRACT : ParsedMessage
    {
        internal S_REQUEST_CONTRACT(TeraMessageReader reader) : base(reader)
        {
            PrintRaw();
            Type = reader.ReadInt16();
            reader.Skip(20);
            Sender = reader.ReadTeraString();
            Receiver = reader.ReadTeraString();
            Console.WriteLine("Sender:"+Sender+";Receiver:"+Receiver+";type:"+Type);
        }

        public short Type { get; private set; }
        public string Sender { get; private set; }
        public string Receiver { get; private set; }
    }
}

