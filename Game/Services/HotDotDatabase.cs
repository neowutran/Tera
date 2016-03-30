using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Data
{
    public class HotDotDatabase
    {
        public enum HotDot
        {
            Dot = 131071,
            Hot = 65536
            //  SystemHot = 655360, // natural regen
            //  CrystalHpHot = 196608,   Not 
            //  StuffMpHot = 262144,
            //  NaturalMpRegen = 0
        }

        private readonly Dictionary<int, Data.HotDot> _hotdots =
            new Dictionary<int, Data.HotDot>();


        public HotDotDatabase(string folder, string language)
        {
            var reader = new StreamReader(File.OpenRead(Path.Combine(folder, $"hotdot\\hotdot-{language}.tsv")));
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (line == null) continue;
                var values = line.Split('\t');
                var id = int.Parse(values[0]);
                var type = values[1];
                var hp = double.Parse(values[2], CultureInfo.InvariantCulture);
                var mp = double.Parse(values[3], CultureInfo.InvariantCulture);
                var method = (Data.HotDot.DotType) Enum.Parse(typeof (Data.HotDot.DotType), values[4]);
                var time = int.Parse(values[5]);
                var tick = int.Parse(values[6]);
                var amount = double.Parse(values[7], CultureInfo.InvariantCulture);
                var name = values[8];
                var itemName = values[10];
                var tooltip = values[11];
                var iconName = values[12];
                _hotdots[id] = new Data.HotDot(id, type, hp, mp, amount, method, time, tick, name, itemName , tooltip, iconName);
            }
            _hotdots[8888888] = new Data.HotDot(8888888, "Endurance", 0, 0, 0, 0, 0, 0, "Enrage","","","enraged");
        }

        public Data.HotDot Get(int skillId)
        {
            return !_hotdots.ContainsKey(skillId) ? null : _hotdots[skillId];
        }
    }
}