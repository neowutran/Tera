using System.Diagnostics;
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
            reader.Skip(14);
            //int unk3 = reader.ReadInt32();
            //int time = reader.ReadInt32();
            Sender = reader.ReadTeraString();
            Recipient = reader.ReadTeraString();
            Debug.WriteLine("type:"+type+";translated:"+Type+"; Sender:"+Sender+";Recipient"+Recipient);
        }

        public string Sender { get; private set; }
        public string Recipient { get; private set; }
        public enum RequestType
        {
            TradeRequest = 3,
            PartyInvite = 4,
            Mailbox = 8,
            ShopOpen = 9,
            MapTeleporter = 14,
            DungeonTeleporter = 15,
            UnStuck = 16,
            VanguardShop = 17,
            ChooseLootDialog = 20, //(aka: goldfinger + elion token + ...)
            BankOpen = 26,
            Craft = 31,
            Personalize = 32,
            TeraClubDarkanFlameUse = 33, // or merge multiple item together
            Enchant = 34,
            OpenBox = 43,
            ContinentMapTeleporter = 48,
            LootBox = 52,
            TeraClubMapTeleporter = 53,
            TeraClubTravelJournalTeleporter = 54,
            TeleporterToNearestCity = 71,
            Dressroom = 76
        }

        public RequestType Type { get; private set; }
    }
}

