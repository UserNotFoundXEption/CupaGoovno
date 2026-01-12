using Blender.Content;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class CustomSupers
{
    public void Init()
    {
        Berserker();
        Sandevistan();
        BooFist();
        new SuperBerserker().Init();
        new SuperSandevistan().Init();
        SuperBooFist.Init();
    }

    private void Berserker()
    {
        string superId = "level_super_berserker";
        berserker = EquipRegistries.Supers.Register(superId,
        new SuperInfo(typeof(SuperPlaceholder), bundle)
            .SetAtlasPath(CustomSprites.spriteAtlas)
            .SetNormalIcons(["berserker0", "berserker1", "berserker2"])
            .AsSuperInfo());
        supers.Add(berserker);
    }

    private void Sandevistan()
    {
        string superId = "level_super_sandevistan";
        sandevistan = EquipRegistries.Supers.Register(superId,
        new SuperInfo(typeof(SuperPlaceholder), bundle)
            .SetAtlasPath(CustomSprites.spriteAtlas)
            .SetNormalIcons(["sandevistan0", "sandevistan1", "sandevistan2"])
            .AsSuperInfo());
        supers.Add(sandevistan);
    }

    private void BooFist()
    {
        string superId = "level_super_boo_fist";
        booFist = EquipRegistries.Supers.Register(superId,
        new SuperInfo(typeof(SuperPlaceholder), bundle)
            .SetAtlasPath(CustomSprites.spriteAtlas)
            .SetNormalIcons(["boo_fist0", "boo_fist1", "boo_fist2"])
            .AsSuperInfo());
        supers.Add(booFist);
    }

    public static void GiftSupers()
    {
        PlayerId[] playerIds = [PlayerId.PlayerOne, PlayerId.PlayerTwo];
        foreach (Super super in supers)
        {
            foreach (PlayerId playerId in playerIds)
            {
                bool isUnlocked = PlayerData.Data.IsUnlocked(playerId, super);
                bool shouldBeUnlocked = false;

                CustomAchievements.Achievements achievement = CustomAchievements.Achievements.None;
                foreach (var ach in CustomUnlockables.supers.Keys)
                {
                    if (ach == super)
                    {
                        achievement = CustomUnlockables.supers[super];
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
                    PlayerData.Data.Gift(playerId, super);
                    PlayerData.SaveCurrentFile();
                }

                if (isUnlocked && !shouldBeUnlocked)
                {
                    PlayerData.Data.inventories.GetPlayer(playerId)._supers.Remove(super);
                    PlayerData.SaveCurrentFile();
                }
            }
        }
    }

    public static bool IsCustomSuper(Super super, LevelPlayerWeaponManager self)
    {
        if(super == berserker)
        {
            SuperBerserker.StartSuper(self);
            return true;
        }
        if(super == sandevistan)
        {
            SuperSandevistan.StartSuper(self);
            return true;
        }
        if(super == booFist)
        {
            SuperBooFist.StartSuper(self);
            return true;
        }
        return false;
    }

    public static Super berserker;
    public static Super sandevistan;
    public static Super booFist;

    private static List<Super> supers = [];
    private string bundle = "CupaGoovno:supers";
}
