using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using static CupaGoovno.CustomAchievements;

namespace CupaGoovno;

public static class CustomUnlockables
{
    public static Dictionary<Weapon, Achievements> weapons = new()
    {
        {  CustomWeapons.striker, Achievements.None },
        {  CustomWeapons.skytickler, Achievements.None },
        {  CustomWeapons.mangetsunami, Achievements.Butcher },
        {  CustomWeapons.peeshooter, Achievements.BuiltDifferent }
    };

    public static Dictionary<Super, Achievements> supers = new()
    {
        {  CustomSupers.berserker, Achievements.None },
        {  CustomSupers.sandevistan, Achievements.Relic },
        {  CustomSupers.booFist, Achievements.Invincible }
    };

    public static Dictionary<Charm, Achievements> charms = new()
    {
        {  CustomCharms.casualMode, Achievements.None },
        { CustomCharms.balance, Achievements.DancersRift },
        { CustomCharms.milk, Achievements.Bird },
        { CustomCharms.gravity, Achievements.Dice }
    };
}
