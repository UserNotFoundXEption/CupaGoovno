using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class MoaiLevelHeart : CustomProjectile
{
    public static new MoaiLevelHeart Create()
    {
        MoaiLevelHeart heart = Create<MoaiLevelHeart>(prefab);

        heart.transform.SetPosition(p.startX, p.y.min);
        heart.transform.SetScale(p.scale, p.scale);

        heart.t = 0f;

        heart.DamagesType.Player = false;
        heart.SetParryable(true);

        return heart;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        t += CupheadTime.FixedDelta;
        if(t < p.initialDelay)
        {
            return;
        }

        transform.AddPosition(p.speed * CupheadTime.FixedDelta);

        float yAmplitude = (p.y.max - p.y.min) / 2f;
        float yCenter = (p.y.max + p.y.min) / 2f;
        float y = Mathf.Sin(Mathf.PI * 2 * t / p.sinusTime) * yAmplitude + yCenter;

        transform.SetPosition(null, y);
    }

    public override void OnParry(AbstractPlayerController player)
    {
        base.OnParry(player);
        if(player.stats.Loadout.charm != Charm.charm_health_up_1)
        {
            player.stats.HealerHP++;
            player.stats.SetHealth(player.stats.Health + 1);

            LevelPlayerController levelPlayerController = player.stats.basePlayer as LevelPlayerController;
            levelPlayerController?.animationController.OnHealerCharm();
        }
    }

    public static GameObject prefab;

    private float t;

    private static MoaiProperties.Heart p = new();
}
