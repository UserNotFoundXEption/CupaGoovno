using Blender.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine.U2D;
using UnityEngine;
using Blender.Utility;
using Rewired;
using Blender;

namespace CupaGoovno;

public class CustomCharms
{
    public void Init()
    {
        CharmMilk.Init();
        CharmBalance.Init();
        CasualMode();
        Balance();
        Milk();
        Gravity();
    }

    private void CasualMode()
    {
        string charmId = "charm_casual_mode";
        casualMode = EquipRegistries.Charms.Register(charmId, new EquipInfo()
            .SetAtlasPath(CustomSprites.spriteAtlas)
            .SetNormalIcons(["casual_mode_0001", "casual_mode_0002", "casual_mode_0003"]));
        charms.Add(casualMode);
    }

    private void Balance()
    {
        string charmId = "charm_balance";
        balance = EquipRegistries.Charms.Register(charmId, new EquipInfo()
            .SetAtlasPath(CustomSprites.spriteAtlas)
            .SetNormalIcons(["balance0", "balance1", "balance2"]));
        charms.Add(balance);
    }

    private void Milk()
    {
        string charmId = "charm_milk";
        milk = EquipRegistries.Charms.Register(charmId, new EquipInfo()
            .SetAtlasPath(CustomSprites.spriteAtlas)
            .SetNormalIcons(["milk0", "milk1", "milk2"]));
        charms.Add(milk);
    }

    private void Gravity()
    {
        string charmId = "charm_gravity";
        gravity = EquipRegistries.Charms.Register(charmId, new EquipInfo()
            .SetAtlasPath(CustomSprites.spriteAtlas)
            .SetNormalIcons(["gravity_globe0", "gravity_globe1", "gravity_globe2"]));
        charms.Add(gravity);
    }

    public static void GiftCharms()
    {
        PlayerId[] playerIds = [PlayerId.PlayerOne, PlayerId.PlayerTwo];
        foreach (Charm charm in charms)
        {
            foreach(PlayerId playerId in playerIds)
            {
                bool isUnlocked = PlayerData.Data.IsUnlocked(playerId, charm);
                bool shouldBeUnlocked = false;

                CustomAchievements.Achievements achievement = CustomAchievements.Achievements.None;
                foreach(var ach in CustomUnlockables.charms.Keys)
                {
                    if(ach == charm)
                    {
                        achievement = CustomUnlockables.charms[charm];
                        break;
                    }
                }

                if(achievement == CustomAchievements.Achievements.None)
                {
                    shouldBeUnlocked = true;
                }
                else
                {
                    foreach(var ach in CustomAchievements.achievements)
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
                    PlayerData.Data.Gift(playerId, charm);
                    PlayerData.SaveCurrentFile();
                }

                if(isUnlocked && !shouldBeUnlocked)
                {
                    PlayerData.Data.inventories.GetPlayer(playerId)._charms.Remove(charm);
                    PlayerData.SaveCurrentFile();
                }
            }
        }
    }

    public static Charm casualMode;
    public static Charm balance;
    public static Charm milk;
    public static Charm gravity;

    private static List<Charm> charms = [];
}
