namespace Tera.Game
{
    public class Server
    {
        public Server(string name, string region, string ip, uint serverId = uint.MaxValue)
        {
            Ip = ip;
            Name = name;
            Region = region;
            ServerId = serverId;
        }

        public string Ip { get; }
        public string Name { get; }
        public string Region { get; }
        public uint ServerId { get; }

        public override string ToString()
        {
            return "IP:" + Ip + ";Name:" + Name + ";Region:" + Region + ";ServerId:" + ServerId;
        }
    }
}