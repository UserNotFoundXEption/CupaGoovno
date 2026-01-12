using Blender.Utility;
using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;

namespace CupaGoovno;

public class SuperSandevistan
{
    public void Init()
    {
        Other.LoadCustomAsset(enterSfxPath);
        Other.LoadCustomAsset(exitSfxPath);
    }

    public static void StartSuper(LevelPlayerWeaponManager self)
    {
        AbstractPlayerController player = PlayerManager.GetPlayer(self.player.id);
        if (state == State.Inactive)
        {
            player.StartCoroutine(sandevistan_cr(player));
            player.StartCoroutine(emptySuper_cr(player));
        }
    }

    private static IEnumerator sandevistan_cr(AbstractPlayerController player)
    {
        state = State.Active;
        Other.PlayCustomSfx(enterSfxPath, 0.6f);
        UnityEngine.UI.Image overlay = Other.CreateScreenOverlay();

        float t = 0f;
        float normalSpeed = player.stats.Loadout.charm == Charm.charm_curse ? 1.2f : 1f;
        float targetMultiplier = p.timeSpeedMultiplier;

        while (t < p.timeIn)
        {
            t += CupheadTime.delta;
            multiplier = Mathf.Lerp(1f, targetMultiplier, t / p.timeIn);
            CupheadTime.SetLayerSpeed(CupheadTime.Layer.Default, multiplier);
            CupheadTime.SetLayerSpeed(CupheadTime.Layer.Enemy, multiplier);
            overlay.color = new Color(0f, 0f, 1f, t / p.timeIn / 15f);
            yield return null;
        }
        CupheadTime.SetLayerSpeed(CupheadTime.Layer.Default, targetMultiplier);
        CupheadTime.SetLayerSpeed(CupheadTime.Layer.Enemy, targetMultiplier);

        while (state == State.Active)
        {
            yield return null;
        }

        Other.PlayCustomSfx(exitSfxPath, 0.6f);
        t = 0f;
        while (t < p.timeOut)
        {
            t += CupheadTime.delta;
            multiplier = Mathf.Lerp(targetMultiplier, 1f, t / p.timeIn);
            CupheadTime.SetLayerSpeed(CupheadTime.Layer.Default, multiplier); 
            CupheadTime.SetLayerSpeed(CupheadTime.Layer.Enemy, multiplier);
            overlay.color = new Color(0f, 0f, 1f, (1f - t / p.timeOut) / 15f);
            yield return null;
        }

        state = State.Inactive;
        CupheadTime.SetLayerSpeed(CupheadTime.Layer.Default, 1f); 
        CupheadTime.SetLayerSpeed(CupheadTime.Layer.Enemy, 1f); 
        GameObject.Destroy(overlay);
    }

    private static IEnumerator emptySuper_cr(AbstractPlayerController player)
    {
        bool isRunNGun = Level.platformingLevels.Contains(Level.Current.CurrentLevel);
        float duration = isRunNGun ? p.durationRunNGun : p.duration;
        while (player.stats.SuperMeter > 0f)
        {
            player.stats.SuperMeter -= 50f * CupheadTime.Delta / duration / multiplier;
            player.stats.OnSuperChanged(true);
            yield return null;
        }
        state = State.Exiting;
        player.stats.SuperMeter = 0f;
        player.stats.OnSuperChanged(true);
    }

    public static bool Active
    {
        get { return state == State.Active || state == State.Exiting; }
    }

    public static State state;
    public static float multiplier;

    private static readonly string enterSfxPath = "CupaGoovno:cupagoovno\\sandevistan_enter";
    private static readonly string exitSfxPath = "CupaGoovno:cupagoovno\\sandevistan_exit";
    private static CustomSuperProperties.Sandevistan p = new();

    public enum State
    {
        Inactive,
        Active,
        Exiting
    }
}
