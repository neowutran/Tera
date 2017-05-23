using System;
using System.Collections.Generic;
using System.Linq;

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
        public enum AbnormalityType
        {
            Debuff = 1,
            DOT = 2,
            Stun = 3,
            Buff = 4
        }
        public enum Types
        {
            Unknown = 0,
            MaxHP = 1,
            MaxMP = 2,
            Power = 3,
            Endurance = 4,
            MovSpd = 5,
            Crit = 6,
            CritResist = 7,
            ImpactEffective = 8,
            Ballance = 9,
            WeakResist = 14,
            DotResist = 15,
            StunResist = 16, //something strange, internal itemname sleep_protect, but user string is stun resist, russian user string is "control effect resist"
            AllResist = 18,
            CritPower = 19,
            CritPower1 = 36,
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
            //29 = something strange when using Lethal Strike, cdr
            Stamina = 30,
            Gathering = 31,
            RidingSpeed = 34,
            GatheringTime = 35,
            HPChange = 51,
            MPChange = 52,
            RageChange = 53,
            KnockDownChance = 103,
            DefPotion = 104, //or glyph: - incoming damage %
            IncreasedHeal = 105,
            PVPDef = 108,
            Reflect = 110,
            RestoreHpOnHit = 151, //restore x% hp after successfull hit with Y skill
            AtkPotion = 162, //or glyph: + outgoing damage %
            DamageAura = 163, // increase skills damage + X
            CritChance = 167,
            PVPAtk = 168,
            ChangeBody = 192,//5=boobs,6=height,7=thighs
            BigHead = 193,
            NoHeal = 202,
            Noctenium = 203, //different values for different kinds of Noctenium, not sure what for =)
            StaminaDecay = 207,
            CDR = 208,
            Block = 210, //frontal block ? Not sure, the ability to use block, or blocking stance
            Stun = 211,
            Stun1 = 61,
            Chaos = 213, //movement keys messed up
            AllowAggro = 216, //no aggro
            HPLoss = 221, //loss hp at the end of debuff
            DelayedEffect = 223, //redirects at the end of effect if not canceled
            BuffNearbyParty = 226, //buffs with redirected abnormality
            Absorb = 227, //or may be I've messed it with 281
            AbsorbToMP = 228, //absorbs damage, each X damage costs Y MP
            Resurrect = 229,
            ReducedMPCost = 230,
            Mark = 231, // Velik's Mark/Curse of Kaprima = increase received damage when marked
            Fear = 232,
            ShareDamageWithNearbyTank = 233,
            ResistNegative = 234,
            SkillCastSpeed = 235, //Increases Shield Barrage speed after 8 successful hits with Combo Attack.
            CastSpeed = 236,
            CrystalBind = 237,
            Drunk1 = 241,
            Drunk = 244,
            Immune = 245, //or may be just status immune too...
            StunKnockDown = 246,
            CCrystalBind = 249,
            Visibility = 254, //0 = invisible
            DropUp = 255,
            StaminaRegen = 258,
            Range = 259, //increase melee range? method 0 value 0.1= +10%
            HPChange2 = 260, //used by instant death on Curse stacks.
            ReloadingSkill = 261,
            //264 = redirect abnormality, value= new abnormality, bugged due to wrong float format in xml.
            Rage = 280, //tick - RageChange, notick (one change) - Rage 
            SuperArmor = 283,
            StatusImmune = 287,
            StaggerImmune = 288,
            EnchantChance = 291,
            ForceCrit = 316, //archer's Find Weakness = next hit will trigger critpower crystalls
            Charm = 65535
        }

        public struct Effect
        {
            public Types Type;
            public DotType Method;
            public double Amount;

        }
        public HotDot(int id, string type, double hp, double mp, double amount, DotType method, uint time, int tick,
            string name, string itemName, string tooltip, string iconName, AbnormalityType abType, bool isBuff, string effectIcon)
        {
            Id = id;
            Types rType;
            rType = Enum.TryParse(type, out rType) ? rType : Types.Unknown;
            AbType = abType;
            IsBuff = isBuff;
            Hp = hp;
            Mp = mp;
            Time = time;
            Tick = tick;
            Effects.Add(new Effect
            {
                Type = rType,
                Amount = amount,
                Method = method,
            });
            Name = name;
            ShortName = name;
            ItemName = itemName;
            Tooltip = tooltip;
            IconName = String.Intern(iconName);
            EffectIcon = String.Intern(effectIcon);
            Debuff = (rType == Types.Endurance || rType == Types.CritResist) && amount <= 1 || rType == Types.Mark || (rType == Types.DefPotion && amount > 1);
            HPMPChange = rType == Types.HPChange || rType == Types.MPChange;
            Buff = rType != Types.HPChange;// && rType != Types.MPChange;//try to show MPChange abnormals
        }

        public void Update(int id, string type, double hp, double mp, double amount, DotType method, uint time, int tick,
            string name, string itemName, string tooltip, string iconName)
        {
            Types rType;
            rType = Enum.TryParse(type, out rType) ? rType : Types.Unknown;
            if (Effects.Any(x => x.Type == rType)) return; // already added - duplicate strings with the same id and type in tsv (different item names - will be deleted soon)
            Hp = rType==Types.HPChange ? hp : Hp;
            Mp = rType==Types.MPChange ? mp : Mp;
            Tick = rType == Types.MPChange|| rType == Types.HPChange ? tick : Tick; //assume that hp and mp tick times should be the same for one abnormality id
            Effects.Add(new Effect
            {
                Type = rType,
                Amount = amount,
                Method = method
            });
            Debuff = Debuff || (rType == Types.Endurance || rType == Types.CritResist) && amount < 1 || rType == Types.Mark || (rType == Types.DefPotion && amount > 1);
            HPMPChange = HPMPChange || rType == Types.HPChange || rType == Types.MPChange;
            Buff = Buff || (rType != Types.HPChange); // && rType != Types.MPChange);//try to show MPChange abnormals
        }


        public List<Effect> Effects = new List<Effect>();
        public int Id { get; }
        public double Hp { get; private set; }
        public double Mp { get; private set; }
        public uint Time { get; }
        public int Tick { get; private set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string ItemName { get; }
        public string Tooltip { get; set; }
        public string IconName { get; }
        public string EffectIcon { get; }
        public bool Buff { get; private set; }
        public bool Debuff { get; private set; }
        public bool HPMPChange { get; private set; }
        public AbnormalityType AbType { get; set; }
        public bool IsBuff { get; set; }//if false => purple hp bar

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((HotDot) obj);
        }


        public bool Equals(HotDot other)
        {
            return Id == other.Id;
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
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return $"{Name} {Id}";
        }
    }
}