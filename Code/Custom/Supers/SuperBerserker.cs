using System.Collections;
using UnityEngine;

namespace CupaGoovno;

public class SuperBerserker
{
    public void Init()
    {
        Other.LoadCustomAsset(laughSfxPath);
    }

    public static void StartSuper(LevelPlayerWeaponManager self)
    {
        AbstractPlayerController player = PlayerManager.GetPlayer(self.player.id);
        if (!active)
        {
            player.StartCoroutine(berserker_cr(player));
            player.StartCoroutine(emptySuper_cr(player));
        }
    }

    private static IEnumerator berserker_cr(AbstractPlayerController player)
    {
        active = true;
        StatsManager.UpdateDamageCounter(PlayerId.PlayerOne);
        Other.PlayCustomSfx(laughSfxPath, 1.2f);
        UnityEngine.UI.Image overlay = Other.CreateScreenOverlay();

        float t = 0f;
        float normalSpeed = player.stats.Loadout.charm == Charm.charm_curse ? 1.2f : 1f;
        float targetSpeed = normalSpeed * p.timeSpeedMultiplier;

        while(t < p.timeIn)
        {
            t += CupheadTime.delta;
            Time.timeScale = Mathf.Lerp(1f, targetSpeed, t / p.timeIn);
            overlay.color = new Color(1f, 0.6f, 0f, t / p.timeIn / 15f);
            yield return null;
        }
        Time.timeScale = targetSpeed;

        while (active)
        {
            yield return null;
        }

        t = 0f;
        while (t < p.timeOut)
        {
            t += CupheadTime.delta;
            Time.timeScale = Mathf.Lerp(targetSpeed, normalSpeed, t / p.timeOut);
            overlay.color = new Color(1f, 0.6f, 0f, (1f - t / p.timeOut) / 15f);
            yield return null;
        }
        Time.timeScale = normalSpeed;
        GameObject.Destroy(overlay);
    }

    private static IEnumerator emptySuper_cr(AbstractPlayerController player)
    {
        float t = 0f;
        achievementTimer = 0f;
        while (player.stats.SuperMeter > 0f)
        {
            player.stats.SuperMeter -= 50f * CupheadTime.Delta / p.duration;
            player.stats.OnSuperChanged(true);
            t += CupheadTime.delta;
            achievementTimer += CupheadTime.delta;

            if (achievementTimer > 20f)
            {
                achievementTimer = 0f;
                CustomAchievements.Unlock(CustomAchievements.Achievements.Butcher);
            }
            yield return null;
        }
        active = false;
        StatsManager.UpdateDamageCounter(PlayerId.PlayerOne);
        player.stats.SuperMeter = 0f;
        player.stats.OnSuperChanged(true);
    }

    public static float achievementTimer;
    public static bool active = false;
    public static bool fired = false;

    private static readonly string laughSfxPath = "CupaGoovno:cupagoovno\\berserker_laugh";
    private static CustomSuperProperties.Berserker p = new();
}
