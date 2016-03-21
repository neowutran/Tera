using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tera.Game.Messages;

namespace Tera.Game
{
    // Contains information about skills
    // Currently this is limited to the name of the skill
    public class CharmsDatabase
    {
        private readonly Dictionary<uint, string> _charms = new Dictionary <uint, string>();

        public CharmsDatabase(string directory, string reg_lang)
        {
            var lines = File.ReadLines(Path.Combine(directory, $"hotdot\\charms-{reg_lang}.tsv"));
            var listOfParts = lines.Select(s => s.Split(new[] { '\t' }, 2));
            foreach (var parts in listOfParts)
            {
                _charms.Add(uint.Parse(parts[0]), parts[1]);
            }
        }

        public string GetCharmName(uint charmId)
        {
            string result = string.Empty;
            _charms.TryGetValue(charmId, out result);
            return result;
        }
    }
}
