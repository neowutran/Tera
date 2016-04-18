using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Tera.Game
{
    public struct Charm
    {
        public uint CharmId { get; }
        public string Name { get; }
        public string IconName { get; }
        public string Tooltip { get; }
        public Charm(uint charmId, string name, string iconName, string tooltip) { CharmId = charmId;Name = name;IconName = iconName; Tooltip = tooltip; }
    }
    public class CharmsDatabase
    {
        private readonly Dictionary<uint, Charm> _charms = new Dictionary <uint, Charm>();

        public CharmsDatabase(string directory, string reg_lang, HotDotDatabase hotdots)
        {
            var lines = File.ReadLines(Path.Combine(directory, $"hotdot\\charms-{reg_lang}.tsv"));
            var listOfParts = lines.Select(s => s.Split(new[] { '\t' }));
            foreach (var parts in listOfParts)
            {
                _charms.Add(uint.Parse(parts[0]), new Charm(uint.Parse(parts[0]), parts[1],parts[2], parts[3] ));
                hotdots.Add(new HotDot(int.Parse(parts[0]), "Charm", 0, 0, 0, 0, 0, 0, parts[1],"",parts[3],parts[2]));
            }
        }

        public string GetCharmName(uint charmId)
        {
            Charm result = new Charm();
            _charms.TryGetValue(charmId, out result);
            return result.Name;
        }
        public string GetCharmIconName(uint charmId)
        {
            Charm result = new Charm();
            _charms.TryGetValue(charmId, out result);
            return result.IconName;
        }
    }
}
