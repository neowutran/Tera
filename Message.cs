using System;
using System.Linq;
using System.Text;

namespace Tera
{
    public class Message
    {
        public Message(DateTime time, MessageDirection direction, ArraySegment<byte> data)
        {
            Time = time;
            Direction = direction;
            Data = data;
        }

        public DateTime Time { get; private set; }
        public MessageDirection Direction { get; private set; }
        public ArraySegment<byte> Data { get; set; }

        private static char RandomLetter()
        {
            Random random = new Random();
            return (char)('a' + random.Next(0, 26));
        }

        public void ReplaceStringWithGarbage(int offset)
        {
            char utf16char = Encoding.Unicode.GetChars(new byte[] { Data.Array[offset], Data.Array[offset + 1] })[0];
            if(utf16char == 0) { return; }
            var byteArray = Encoding.Unicode.GetBytes(new char[] { RandomLetter() });
            Data.Array[offset] = byteArray[0];
            Data.Array[offset + 1] = byteArray[1];
            ReplaceStringWithGarbage(offset + 2);
        }

        public ushort OpCode => (ushort) (Data.Array[Data.Offset] | Data.Array[Data.Offset + 1] << 8);
        public ArraySegment<byte> Payload => new ArraySegment<byte>(Data.Array, Data.Offset + 2, Data.Count -2);
    }
}