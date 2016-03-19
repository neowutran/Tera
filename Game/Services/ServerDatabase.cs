using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Tera.Game
{
    public class ServerDatabase
    {
        private readonly Dictionary<Tuple<uint,string>, Server> _servers = new Dictionary<Tuple<uint, string>, Server>();
        private readonly IEnumerable<Server> _serverlist = new List<Server>();

        public ServerDatabase(string folder)
        {
            _serverlist = File.ReadAllLines(Path.Combine(folder, $"data/servers.txt"))
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => s.Split(new[] { ' ' }, 4))
                .Select(parts => new Server(parts[3], parts[1], parts[0], !string.IsNullOrEmpty(parts[2])?uint.Parse(parts[2]):uint.MaxValue));
            _servers =_serverlist.Where(x=>x.ServerId!=uint.MaxValue).ToDictionary(x=>Tuple.Create(x.ServerId,x.Region));
        }

        public string GetServerName(uint ServerId,string region="") // not sure, whether Servers in different regions have unique ServerId's, if so - delete Tuple and lookup only by ServerId
        {
            var key = Tuple.Create(ServerId, region);
            return _servers.ContainsKey(key) ? _servers[key].Name : $"{region}: {ServerId.ToString()}";
        }
        public Dictionary<string,Server> GetServersByIp()
        {
            return _serverlist.GroupBy(x => x.Ip).ToDictionary(x=>x.Key,x=>x.First());
        }

    }
}