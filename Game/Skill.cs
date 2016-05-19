namespace Tera.Game
{
    public class Skill
    {
        private static readonly string[] Lvls = { " I", " II", " III", " IV", " V", " VI", " VII", " VIII", " IX", " X",
            " XI", " XII", " XIII", " XIV", " XV", " XVI", " XVII", " XVIII", " XIX", " XX"};

        public static string RemoveLvl(string name)
        {
            foreach (string lvl in Lvls)
            {
                if (name.EndsWith(lvl) || name.Contains(lvl + " "))
                {
                    return name.Replace(lvl, string.Empty);
                }
            }
            return name;
        }

        internal Skill(int id, string name, bool? isChained = null, string detail = "",string iconName="", NpcInfo npcInfo=null)
        {
            Id = id;
            Name = name;
            ShortName = RemoveLvl(name);
            IsChained = isChained;
            Detail = detail;
            IconName = iconName;
            NpcInfo = npcInfo;
        }

        public int Id { get; }
        public string Name { get; private set; }
        public string ShortName { get; private set; }
        public bool? IsChained { get; private set; }
        public string Detail { get; private set; }
        public string IconName { get; private set; }
        public readonly NpcInfo NpcInfo;
    }

    public class UserSkill : Skill
    {
        public UserSkill(int id, PlayerClass playerClass, string name, string hit, bool? ischained, string iconName)
            : base(id, name, ischained, hit, iconName)
        {
            PlayerClass = playerClass;
            RaceGenderClass = new RaceGenderClass(Race.Common, Gender.Common, playerClass);
            Hit = hit;
        }
        public UserSkill(int id, RaceGenderClass raceGenderClass, string name, bool? isChained = null, string detail = "", string iconName = "", NpcInfo npcInfo = null)
            : base(id, name, isChained, detail,iconName, npcInfo)
        {
            RaceGenderClass = raceGenderClass;
            PlayerClass = raceGenderClass.Class;
            Hit = detail;
        }

        public string Hit { get; }
        public RaceGenderClass RaceGenderClass { get; private set; }
        public PlayerClass PlayerClass { get; }
        public override bool Equals(object obj)
        {
            var other = obj as UserSkill;
            if (other == null)
                return false;
            return (Id == other.Id) && (RaceGenderClass.Equals(other.RaceGenderClass));
        }

        public override int GetHashCode()
        {
            return Id ^ RaceGenderClass.GetHashCode() ^ NpcInfo.GetHashCode();
        }
    }
}