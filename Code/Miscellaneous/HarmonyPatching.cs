using DialoguerCore;
using HarmonyLib;
using System.Collections.Generic;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;


namespace CupaGoovno;

public class HarmonyPatching
{
    public void Init()
    {
        var harmony = new Harmony("com.example.hook");
        harmony.PatchAll();
    }
}

[HarmonyPatch]
public class LevelPlayerControllerPatch
{
    [HarmonyPatch(typeof(LevelPlayerController))]
    [HarmonyPatch("CanTakeDamage", MethodType.Getter)]
    [HarmonyPrefix]
    public static bool Prefix(LevelPlayerController __instance, ref bool __result)
    {
        __result = __instance.damageReceiver.state == PlayerDamageReceiver.State.Vulnerable;
        return false;
    }
}

[HarmonyPatch]
public class AbstractProjectilePatch
{
    [HarmonyPatch(typeof(AbstractProjectile))]
    [HarmonyPatch("DamageMultiplier", MethodType.Getter)]
    [HarmonyPrefix]
    public static bool Prefix(AbstractProjectile __instance, ref float __result)
    {
        if(__instance.tag == "PlayerProjectile")
        {
            __result = Other.GetDamageMultiplier();
        }
        else
        {
            __result = 1f;
        }

        try
        {
            bool isEx = __instance.damageSource == DamageDealer.DamageSource.Ex;
            Charm charm = PlayerManager.GetFirst().stats.Loadout.charm;
            bool isPractiseMode = charm == Charm.charm_smoke_dash;
            if (isEx && isPractiseMode)
            {
                __result = 0f;
            }
        }
        catch { }

        return false;
    }
}

[HarmonyPatch]
public class PlayerManagerPatch
{
    [HarmonyPatch(typeof(PlayerManager))]
    [HarmonyPatch("DamageMultiplier", MethodType.Getter)]
    [HarmonyPrefix]
    public static bool Prefix(ref float __result)
    {
        __result = Other.GetDamageMultiplier();

        return false;
    }
}

[HarmonyPatch]
public class PlatformHelperPatch
{
    [HarmonyPatch(typeof(PlatformHelper))]
    [HarmonyPatch("ShowAchievements", MethodType.Getter)]
    [HarmonyPrefix]
    public static bool Prefix(ref bool __result)
    {
        __result = true;
        return false;
    }
}

[HarmonyPatch]
public class CupheadTimePatch
{
    [HarmonyPatch(typeof(CupheadTime))]
    [HarmonyPatch("FixedDelta", MethodType.Getter)]
    [HarmonyPrefix]
    public static bool Prefix(ref float __result)
    {
        __result = Time.fixedDeltaTime * CupheadTime.GlobalSpeed * SuperSandevistan.multiplier;
        return false;
    }
}

[HarmonyPatch]
public class PlayerStatsManagerPatch
{
    [HarmonyPatch(typeof(PlayerStatsManager))]
    [HarmonyPatch("CanGainSuperMeter", MethodType.Getter)]
    [HarmonyPrefix]
    public static bool Prefix(PlayerStatsManager __instance, ref bool __result)
    {
        bool originalCanGain = !__instance.SuperInvincible || __instance.ChaliceShieldOn;

        string[] noParryObjects = [
            "Clown_Dog_Balloon_Pink",
            "Devil_Tear",
            "FlyingHorse_Bullet",
            "Frogs_Short_Fireball",
            "ParryBox",
            "Veggies_Potato_Bullet",
            "DicePalaceMain_Dice",
            "DicePalaceMain_Pink_Card"];
        bool isNoParryObject = false;
        foreach (string noParryObject in noParryObjects)
        {
            if (ParryStuff.lastParriedName.Contains(noParryObject))
            {
                isNoParryObject = true;
                break;
            }
        }

        __result = !isNoParryObject && originalCanGain && !SuperSandevistan.Active;

        ParryStuff.lastParriedName = "";

        return false;
    }
}

/*[HarmonyPatch]
public class TextPhasePatch
{
    [HarmonyPatch(typeof(TextPhase))]
    [HarmonyPatch("onStart", MethodType.Normal)]
    [HarmonyPrefix]
    public static bool Prefix(TextPhase __instance)
    {
        Plugin.Log(__instance.ToString());
        Plugin.Log(__instance.GetType());
        return true;
    }
}*/
