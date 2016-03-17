// Copyright (c) Gothos
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tera.Game.Messages;

namespace Tera.Game
{
    // Contains information about skills
    // Currently this is limited to the name of the skill
    public class SkillDatabase
    {
        private readonly Dictionary<RaceGenderClass, Dictionary<int, UserSkill>> _userSkilldata = new Dictionary<RaceGenderClass, Dictionary<int, UserSkill>>();

        public SkillDatabase(string directory,string reg_lang)
        {
            InitializeSkillDatabase(Path.Combine(directory, $"skills\\skills-override-{reg_lang}.tsv"));
            InitializeSkillDatabase(Path.Combine(directory, $"skills\\skills-{reg_lang}.tsv" ));
        }

        private void InitializeSkillDatabase(string filename)
        {
            var lines = File.ReadLines(filename);
            var listOfParts = lines.Select(s => s.Split(new[] { '\t' }, 7));
            foreach (var parts in listOfParts)
            {
                var skill = new UserSkill(int.Parse(parts[0]), new RaceGenderClass(parts[1], parts[2], parts[3]), parts[4], parts[5] != "" && bool.Parse(parts[5]), parts[6]);
                if (!_userSkilldata.ContainsKey(skill.RaceGenderClass))
                    _userSkilldata[skill.RaceGenderClass] = new Dictionary<int, UserSkill>();
                if (!_userSkilldata[skill.RaceGenderClass].ContainsKey(skill.Id))
                    _userSkilldata[skill.RaceGenderClass].Add(skill.Id,skill);
            }
        }

        public Skill Get(UserEntity user, EachSkillResultServerMessage message)
        {
            return GetOrNull(user, message.SkillId);
        }

        // skillIds are reused across races and class, so we need a RaceGenderClass to disambiguate them
        public Skill GetOrNull(UserEntity user, int skillId)
        {
            var raceGenderClass = user.RaceGenderClass;
            foreach (var rgc2 in raceGenderClass.Fallbacks())
            {
                if (!_userSkilldata.ContainsKey(rgc2))
                    continue;

                UserSkill skill;
                if (!_userSkilldata[rgc2].TryGetValue(skillId, out skill))
                     continue;
                return skill;
            }
            return null;
        }
    }
}
