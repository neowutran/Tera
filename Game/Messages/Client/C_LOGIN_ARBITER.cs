using System;
using System.Linq;

namespace Tera.Game.Messages
{
    public enum LangEnum : UInt32
    {
        INT = 0,
        KR = 1,
        NA = 2,
        JP = 3,
        EU_GER = 4,
        EU_FR = 5,
        EU_EN = 6,
        TW = 7,
        RU = 8,
        CH = 9,
        THA = 10,
        SE = 11
    }


    public class C_LOGIN_ARBITER : ParsedMessage
    {
        internal C_LOGIN_ARBITER(TeraMessageReader reader) : base(reader)
        {
            reader.Skip(11);
            Language = (LangEnum)reader.ReadUInt32();
            Version = reader.Factory.Region.Contains("C") ? 2707 : reader.ReadInt32();//hardcoded EUC/KRC, since its version is not sent via network
            if (!Environment.GetCommandLineArgs().Contains("--toolbox"))
            {
                reader.Factory.ReleaseVersion = Version;
                reader.Factory.ReloadSysMsg();
            }
        }

        public LangEnum Language { get; set; }
        public int Version { get; set; }
    }
}