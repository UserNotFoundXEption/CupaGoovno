using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public static class CharmBalance
{
    public static void Init()
    {
        boostDifference = 0f;
    }

    public static void OnDamage(float damage, AbstractPlayerController player)
    {
        bool main = IsMain(player);
        if (!main)
        {
            damage *= -1f;
        }

        float newBoost = boostDifference + damage * p.percentPerDamage;
        newBoost = Mathf.Clamp(newBoost, -100f, 100f);
        boostDifference = newBoost;
    }

    public static float GetDamageMultiplier(AbstractPlayerController player)
    {
        bool main = IsMain(player);
        float activeBoost = 1f;
        if (main)
        {
            activeBoost = 1f - boostDifference / 100f;
        }
        else
        {
            activeBoost = 1f + boostDifference / 100f;
        }

        return activeBoost + p.passiveBoost;
    }

    private static bool IsMain(AbstractPlayerController player)
    {
        if (player is LevelPlayerController levelPlayer)
        {
            Weapon primaryWeapon = player.stats.Loadout.primaryWeapon;
            Weapon currentWeapon = levelPlayer.weaponManager.currentWeapon;
            return primaryWeapon == currentWeapon;
        }
        if (player is PlanePlayerController planePlayer)
        {
            Weapon peashot = Weapon.plane_weapon_peashot;
            Weapon currentWeapon = planePlayer.weaponManager.currentWeapon;
            return peashot == currentWeapon;
        }
        return true;
    }

    public static void TryUpdate(float damage)
    {
        AbstractPlayerController player = Other.Player();
        if (player.stats.Loadout.charm == CustomCharms.balance)
        {
            OnDamage(damage, player);
            StatsManager.UpdateDamageCounter(player.id);
        }
    }

    public static float boostDifference;

    private static CustomCharmProperties.Balance p = new();
}
