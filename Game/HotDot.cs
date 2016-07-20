using System;

namespace Tera.Game
{
    public class HotDot : IEquatable<object>
    {
        public enum DotType
        {
            swch = 0, // switch on for noctineum ? other strange uses.
            seta = 1, // ?set abs stat value
            abs = 2, // each tick  HP +=HPChange ; MP += MPChange
            perc = 3, // each tick  HP += MaxHP*HPChange; MP += MaxMP*MPChange
            setp = 4 // ?set % stat value
        }

        public enum Types
        {
            Unknown = 0,
            MaxHP = 1,
            Power = 3,
            Endurance = 4,
            MovSpd = 5,
            Crit = 6,
            CritResist = 7,
            ImpactEffective = 8,
            Ballance = 9,
            WeakResist = 14,
            DotResist = 15,
            StunResist = 16,
            //something strange, internal itemname sleep_protect, but user string is stun resist, russian user string is "control effect resist"
            AllResist = 18,
            CritPower = 19,
            Aggro = 20,
            NoMPDecay = 21, //slayer
            Attack = 22, //total damage modificator
            XPBoost = 23,
            ASpd = 24,
            MovSpdInCombat = 25,
            CraftTime = 26,
            OutOfCombatMovSpd = 27,
            HPDrain = 28, //drain hp on attack
            //28 = Something comming with MovSpd debuff skills, fxp 32% MovSpd debuff from Lockdown Blow IV, give also 12% of this kind
            //29 = something strange when using Lethal Strike
            Stamina = 30,
            Gathering = 31,
            HPChange = 51,
            MPChange = 52,
            RageChange = 53,
            KnockDownChance = 103,
            DefPotion = 104, //or glyph: - incoming damage %
            IncreasedHeal = 105,
            PVPDef = 108,
            AtkPotion = 162, //or glyph: + outgoing damage %
            CritChance = 167,
            PVPAtk = 168,
            Noctenium = 203, //different values for different kinds of Noctenium, not sure what for =)
            StaminaDecay = 207,
            CDR = 208,
            Block = 210, //frontal block ? Not sure, the ability to use block, or blocking stance
            HPLoss = 221, //loss hp at the and of debuff
            Mark = 231, // Velik's Mark/Curse of Kaprima = increase received damage when marked
            CastSpeed = 236,
            Range = 259, //increase melee range? method 0 value 0.1= +10%
            //264 = redirect abnormality, value= new abnormality, bugged due to wrong float format in xml.
            Rage = 280, //tick - RageChange, notick (one change) - Rage 
            SuperArmor = 283,
            Charm = 65535
        }

        public HotDot(int id, string type, double hp, double mp, double amount, DotType method, int time, int tick,
            string name, string itemName, string tooltip, string iconName)
        {
            Id = id;
            Types rType;
            Type = Enum.TryParse(type, out rType) ? rType : Types.Unknown;
            Hp = hp;
            Mp = mp;
            Amount = amount;
            Method = method;
            Time = time;
            Tick = tick;
            Name = name;
            ItemName = itemName;
            Tooltip = tooltip;
            IconName = iconName;
            Debuff = (Type == Types.Endurance || Type == Types.CritResist) && Amount<1 || Type == Types.Mark;
            HPMPChange = Type == Types.HPChange || Type == Types.MPChange;
        }

        public double Amount { get; }

        public int Id { get; }
        public Types Type { get; }
        public double Hp { get; }
        public double Mp { get; }
        public DotType Method { get; }
        public int Time { get; }
        public int Tick { get; }
        public string Name { get; }
        public string ItemName { get; }
        public string Tooltip { get; }
        public string IconName { get; }
        public bool Debuff { get; }
        public bool HPMPChange { get; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((HotDot) obj);
        }


        public bool Equals(HotDot other)
        {
            return Id == other.Id && Type == other.Type;
        }

        public static bool operator ==(HotDot a, HotDot b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object) a == null) || ((object) b == null))
            {
                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(HotDot a, HotDot b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return Type.GetHashCode() ^ Id.GetHashCode();
        }

        public override string ToString()
        {
            return $"{Name} {Id}";
        }
    }
}