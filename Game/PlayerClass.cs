using System;

namespace Tera.Game
{
    public enum PlayerClass
    {   //todo change it to 0 based as in official meter. (require refactoring of low level code)
        Warrior = 1,
        Lancer = 2,
        Slayer = 3,
        Berserker = 4,
        Sorcerer = 5,
        Archer = 6,
        Priest = 7,
        Mystic = 8,
        Reaper = 9,
        Gunner = 10,
        Brawler = 11,
        Ninja = 12,
        Valkyrie = 13,

        Common = 100
    }   
}