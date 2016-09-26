using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
namespace Tera.Game.Messages
{
    public class S_GUILD_QUEST_LIST : ParsedMessage
    {
        internal S_GUILD_QUEST_LIST(TeraMessageReader reader) : base(reader)
        {
            PrintRaw();
            var counter = reader.ReadUInt16();
            var offset = reader.ReadUInt16();
            Console.WriteLine("counter:" + counter+";offset:"+offset+"; position:"+reader.BaseStream.Position);
            var baseNumberUnknowByte = (int)(offset - reader.BaseStream.Position);
            var baseunknow = reader.ReadBytes(baseNumberUnknowByte);
            Console.WriteLine(BitConverter.ToString(baseunknow));

            
            for (var i = 1; i <= counter; i++)
            {
               
                reader.BaseStream.Position = offset - 4;
                Console.WriteLine("position:" + reader.BaseStream.Position);
                var pointer = reader.ReadUInt16();
                //Debug.Assert(pointer == offset);//should be the same
                var nextOffset = reader.ReadUInt16();
                var hexnext = nextOffset.ToString("X");
                Console.WriteLine("pointer:"+pointer+";nextOffset:"+nextOffset+"-"+ hexnext);
                var unknow = reader.ReadBytes(52);
                Console.WriteLine(BitConverter.ToString(unknow));
                var guildQuestDescriptionLabel = reader.ReadTeraString();
                var guildQuestTitleLabel = reader.ReadTeraString();
                var guildname = reader.ReadTeraString();
              

                var uk = reader.ReadBytes(4);
                Console.WriteLine(BitConverter.ToString(uk));
                var zoneId = reader.ReadUInt32();
                var monsterId = reader.ReadUInt64();
                var total = reader.ReadUInt16();

                Console.WriteLine(
                 guildQuestDescriptionLabel + "\t"
                 + guildQuestTitleLabel + "\t"
                 + guildname + "\t"
                 + total + "\t"
                 + monsterId + "\t"
                 + zoneId + "\t"
                 );
                var count = (int)(nextOffset - reader.BaseStream.Position);
                if (nextOffset == 0) {
                    count = (int)(reader.BaseStream.Length - reader.BaseStream.Position);
                }

                unknow = reader.ReadBytes(count);
                Console.WriteLine(BitConverter.ToString(unknow));
                offset = nextOffset;

            }
        }

    }
}

