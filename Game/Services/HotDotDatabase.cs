using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Tera.Game
{
    public class HotDotDatabase
    {
        public enum HotOrDot
        {
            Dot = 131071,
            Hot = 65536
            //  SystemHot = 655360, // natural regen
            //  CrystalHpHot = 196608,   Not 
            //  StuffMpHot = 262144,
            //  NaturalMpRegen = 0
        }

        private readonly Dictionary<int, HotDot> _hotdots =
            new Dictionary<int, HotDot>();


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
                var method = (HotDot.DotType) Enum.Parse(typeof(HotDot.DotType), values[4]);
                var time = int.Parse(values[5]);
                var tick = int.Parse(values[6]);
                var amount = double.Parse(values[7], CultureInfo.InvariantCulture);
                var name = values[8];
                var itemName = values[10];
                var tooltip = values[11];
                var iconName = values[12];
                if (_hotdots.ContainsKey(id))
                    _hotdots[id].Update(id, type, hp, mp, amount, method, time, tick, name, itemName, tooltip, iconName);
                else
                    _hotdots[id] = new HotDot(id, type, hp, mp, amount, method, time, tick, name, itemName, tooltip, iconName);
            }
            _hotdots[8888888] = new HotDot(8888888, "Endurance", 0, 0, 0, 0, 0, 0, "Enrage", "", "", "enraged");
            _hotdots[8888889] = new HotDot(8888889, "CritPower", 0, 0, 0, 0, 0, 0, "Slaying", "",
                "'Slaying' crystal is working (if equipped) when player in this state.", "slaying");
        }

        public void Add(HotDot dot)
        {
            _hotdots[dot.Id] = dot;
        }

        public HotDot Get(int skillId)
        {
            return !_hotdots.ContainsKey(skillId) ? null : _hotdots[skillId];
        }
    }
}