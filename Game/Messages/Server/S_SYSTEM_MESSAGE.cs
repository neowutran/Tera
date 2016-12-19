
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Tera.Game.Messages
{
    public class S_SYSTEM_MESSAGE : ParsedMessage
    {
        internal S_SYSTEM_MESSAGE(TeraMessageReader reader) : base(reader)
        {
            reader.Skip(2);//offset
		    RawMessage = reader.ReadTeraString();
            var parts=RawMessage.Split(new[] {'\v'});
            Type = (MessageType) int.Parse(parts[0].Replace("@", ""));//todo add mapping id=>message name via SysMsgNamer, like OpCodeNamer, mapping is different between game versions, use smt_<version>.txt
            int i = 1;
            while (i + 2 <= parts.Length)
            {
                Parameters[parts[i]] = parts[i + 1];
                i = i + 2;
            }
            //todo add various strsheet_*.xml to reconstruct game message as it seen by user (if needed?)
            Debug.WriteLine(Type + ":   "+string.Join(";\t",Parameters.Select(x=>x.Key+": "+x.Value)));
        }

        public string RawMessage { get; private set; }
        public MessageType Type { get; private set; }
        public Dictionary<string,string> Parameters=new Dictionary<string, string>();

        public enum MessageType
        {

            GuildMemberLoginWithoutComment = 1770,
            GuildMemberLoginWithComment = 1769,
            GuildMemberLogout = 1969,
            PartyMemberPickupLoot = 679,
            AccountBenefit = 827,
            NewVandguardRequest = 3054
        }
    }
}