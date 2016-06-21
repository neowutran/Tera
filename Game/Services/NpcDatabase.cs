using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;


namespace Tera.Game
{
    public class NpcDatabase
    {
        private readonly Dictionary<Tuple<ushort, uint>, NpcInfo> _dictionary;
        private readonly Dictionary<ushort, string> _zoneNames = new Dictionary<ushort, string>();
        private readonly Func<Tuple<ushort, uint>, NpcInfo> _getPlaceholder;
        public bool DetectBosses;

        public NpcDatabase(Dictionary<Tuple<ushort, uint>, NpcInfo> npcInfo)
        {
            _zoneNames = (from npc in npcInfo.Values.GroupBy(x => x.HuntingZoneId).Select(x => x.FirstOrDefault())
                          let huntingzoneid = npc.HuntingZoneId
                          let area = npc.Area
                          select new { huntingzoneid, area }).ToDictionary(x => x.huntingzoneid, y => y.area);
            _dictionary = npcInfo;
            _getPlaceholder = Helpers.Memoize<Tuple<ushort, uint>, NpcInfo>(x => new NpcInfo(x.Item1, x.Item2, false, 0, $"Npc {x.Item1} {x.Item2}", GetAreaName(x.Item1)));
        }

        private static Dictionary<Tuple<ushort, uint>, NpcInfo> LoadNpcInfos(string directory, string reg_lang)
        {
            var xml = XDocument.Load(Path.Combine(directory, $"monsters\\monsters-{reg_lang}.xml"));
            var NPCs = (from zones in xml.Root.Elements("Zone")
                        let huntingzoneid = ushort.Parse(zones.Attribute("id").Value)
                        let area = zones.Attribute("name").Value
                        from monsters in zones.Elements("Monster")
                        let templateid = uint.Parse(monsters.Attribute("id").Value)
                        let boss = bool.Parse(monsters.Attribute("isBoss").Value)
                        let hp = int.Parse(monsters.Attribute("hp").Value)
                        let name = monsters.Attribute("name").Value
                        select new NpcInfo(huntingzoneid, templateid, boss, hp, name, area)).ToDictionary(x => Tuple.Create(x.HuntingZoneId, x.TemplateId));
            xml = XDocument.Load(Path.Combine(directory, "monsters\\monsters-override.xml"));
            var overs = (from zones in xml.Root.Elements("Zone")
                         let huntingzoneid = ushort.Parse(zones.Attribute("id").Value)
                         from monsters in zones.Elements("Monster")
                         let templateid = uint.Parse(monsters.Attribute("id").Value)
                         let boss = monsters.Attribute("isBoss")?.Value
                         let hp = monsters.Attribute("hp")?.Value
                         let name = monsters.Attribute("name")?.Value
                         select new { id = Tuple.Create(huntingzoneid, templateid), boss, hp, name });
            foreach (var over in overs)
            {
                if (NPCs.ContainsKey(over.id))
                {
                    NPCs[over.id] = new NpcInfo(NPCs[over.id].HuntingZoneId, NPCs[over.id].TemplateId,
                                                (over.boss == null) ? NPCs[over.id].Boss : bool.Parse(over.boss),
                                                (over.hp == null) ? NPCs[over.id].HP : int.Parse(over.hp),
                                                (over.name == null) ? NPCs[over.id].Name : over.name,
                                                NPCs[over.id].Area);
                }
                else
                {
                    NPCs.Add(over.id, new NpcInfo(over.id.Item1, over.id.Item2,
                                                (over.boss == null) ? false : bool.Parse(over.boss),
                                                (over.hp == null) ? 0 : int.Parse(over.hp),
                                                (over.name == null) ? $"Npc {over.id.Item1} {over.id.Item2}" : over.name,
                                                string.Empty));
                }
            }
            return NPCs;
        }

        public NpcDatabase(string directory, string reg_lang, bool detectBosses = false)
            : this(LoadNpcInfos(directory, reg_lang))
        {
            DetectBosses = detectBosses;
        }

        public NpcInfo GetOrNull(ushort huntingZoneId, uint templateId)
        {
            NpcInfo result;
            _dictionary.TryGetValue(Tuple.Create(huntingZoneId, templateId), out result);
            return result;
        }

        public string GetAreaName(ushort huntingZoneId)
        {
            string result;
            if (_zoneNames.TryGetValue(huntingZoneId, out result))
                return result;
            else
                return huntingZoneId.ToString();
        }

       
        public NpcInfo GetOrPlaceholder(ushort huntingZoneId, uint templateId)
        {
            var result = GetOrNull(huntingZoneId, templateId) ?? _getPlaceholder(Tuple.Create(huntingZoneId, templateId));
            if (DetectBosses) result.Boss = false;
            return result;
        }
    }
}
