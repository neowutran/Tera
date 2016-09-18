using System.Diagnostics;

namespace Tera.Game.Messages
{
    public class S_BEGIN_THROUGH_ARBITER_CONTRACT : ParsedMessage
    {
        internal S_BEGIN_THROUGH_ARBITER_CONTRACT(TeraMessageReader reader)
            : base(reader)
        {
            reader.Skip(18);
            InviteName = reader.ReadTeraString();
            //reader.Skip(2);
            PlayerName = reader.ReadTeraString();

            Debug.WriteLine("InviteName:" + InviteName + " PlayerName:" + PlayerName);
        }

        public string InviteName { get; set; }
        public string PlayerName { get; set; }
    }
}
