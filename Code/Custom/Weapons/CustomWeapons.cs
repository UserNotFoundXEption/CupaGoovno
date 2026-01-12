using Blender.Content;
using Blender.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using static PlanePlayerWeaponManager.States;

namespace CupaGoovno;

public class CustomWeapons
{
    public void Init()
    {
        Striker();
        Skytickler();
        Mangetsunami();
        Peeshooter();
    }

    private void Striker()
    {
        string weaponId = "level_weapon_striker";
        WeaponInfo info = new WeaponInfo(typeof(WeaponStriker), bundle);
        striker = EquipRegistries.Weapons.Register(weaponId,
            new WeaponInfo(typeof(WeaponStriker), bundle)
                .SetExType(typeof(WeaponStrikerExProjectile))
                .SetAtlasPath(CustomSprites.spriteAtlas)
                .SetNormalIcons(["striker0", "striker1", "striker2"])
                .SetGreyIcons(["striker0_grey", "striker1_grey", "striker2_grey"])
                .AsWeaponInfo());
        weapons.Add(striker);
    }

    private void Skytickler()
    {
        string weaponId = "level_weapon_skytickler";
        WeaponInfo info = new WeaponInfo(typeof(WeaponSkytickler), bundle);
        skytickler = EquipRegistries.Weapons.Register(weaponId,
            new WeaponInfo(typeof(WeaponSkytickler), bundle)
                .SetBasicType(typeof(WeaponUpshotProjectile))
                .SetExType(typeof(WeaponSkyticklerExProjectile))
                .SetAtlasPath(CustomSprites.spriteAtlas)
                .SetNormalIcons(["skytickler0", "skytickler1", "skytickler2"])
                .SetGreyIcons(["skytickler0_grey", "skytickler1_grey", "skytickler2_grey"])
                .AsWeaponInfo());
        weapons.Add(skytickler);
    }

    private void Mangetsunami()
    {
        string weaponId = "level_weapon_mangetsunami";
        WeaponInfo info = new WeaponInfo(typeof(WeaponMangetsunami), bundle);
        mangetsunami = EquipRegistries.Weapons.Register(weaponId,
            new WeaponInfo(typeof(WeaponMangetsunami), bundle)
                .SetBasicType(typeof(WeaponMangetsunamiProjectile))
                .SetExType(typeof(WeaponMangetsunamiExProjectile))
                .SetAtlasPath(CustomSprites.spriteAtlas)
                .SetNormalIcons(["mangetsunami0", "mangetsunami1", "mangetsunami2"])
                .SetGreyIcons(["mangetsunami0_grey", "mangetsunami1_grey", "mangetsunami2_grey"])
                .AsWeaponInfo());
        weapons.Add(mangetsunami);
    }

    private void Peeshooter()
    {
        string weaponId = "level_weapon_pee";
        WeaponInfo info = new WeaponInfo(typeof(WeaponPeeshooter), bundle);
        peeshooter = EquipRegistries.Weapons.Register(weaponId,
            new WeaponInfo(typeof(WeaponPeeshooter), bundle)
                .SetExType(typeof(WeaponPeeShooterExProjectile))
                .SetAtlasPath(CustomSprites.spriteAtlas)
                .SetNormalIcons(["peeshooter0", "peeshooter1", "peeshooter2"])
                .SetGreyIcons(["peeshooter0_grey", "peeshooter1_grey", "peeshooter2_grey"])
                .AsWeaponInfo());
        weapons.Add(peeshooter);

        AssetHelper.AddPersistentPath(AssetHelper.LoaderType.Single, WeaponPeeshooter.lemonPartySoundPath);
    }

    public static void GiftWeapons()
    {
        PlayerId[] playerIds = [PlayerId.PlayerOne, PlayerId.PlayerTwo];
        foreach (Weapon weapon in weapons)
        {
            foreach (PlayerId playerId in playerIds)
            {
                bool isUnlocked = PlayerData.Data.IsUnlocked(playerId, weapon);
                bool shouldBeUnlocked = false;

                CustomAchievements.Achievements achievement = CustomAchievements.Achievements.None;
                foreach (var ach in CustomUnlockables.weapons.Keys)
                {
                    if (ach == weapon)
                    {
                        achievement = CustomUnlockables.weapons[weapon];
                        break;
                    }
                }

                if (achievement == CustomAchievements.Achievements.None)
                {
                    shouldBeUnlocked = true;
                }
                else
                {
                    foreach (var ach in CustomAchievements.achievements)
                    {
                        if (ach.ach == achievement)
                        {
                            shouldBeUnlocked = ach.unlocked;
                            break;
                        }
                    }
                }

                if (!isUnlocked && shouldBeUnlocked)
                {
                    PlayerData.Data.Gift(playerId, weapon);
                    PlayerData.SaveCurrentFile();
                }

                if (isUnlocked && !shouldBeUnlocked)
                {
                    PlayerData.Data.inventories.GetPlayer(playerId)._weapons.Remove(weapon);
                    PlayerData.SaveCurrentFile();
                }
            }
        }
    }

    public static Weapon striker;
    public static Weapon skytickler;
    public static Weapon mangetsunami;
    public static Weapon peeshooter;
    public static List<BasicProjectile> projectilesBasic = [];
    public static List<WeaponUpshotProjectile> projectilesUpshot = [];

    private static List<Weapon> weapons = [];
    private string bundle = "CupaGoovno:weapons";
}
