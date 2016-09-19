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
            //PrintRaw();
            reader.Skip(24);
            short type = reader.ReadInt16();
            Type = (RequestType)type;
            //Console.WriteLine("type:"+type+";translated:"+Type);
        }

        public enum RequestType
        {
            DungeonTeleporter = 15,
            Mailbox = 8,
            MapTeleporter =  14,
            TeraClubMapTeleporter = 53,
            TeraClubTravelJournalTeleporter = 54,
            OpenBox = 43,
            LootBox = 52,
            ChooseLootDialog = 20, //(aka: goldfinger + elion token + ...)
            BankOpen = 26,
            TeraClubDarkanFlameUse = 33, // or merge multiple item together
            PartyInvite = 4,
            TradeRequest = 3 
        }

        public RequestType Type { get; private set; }
    }
}

