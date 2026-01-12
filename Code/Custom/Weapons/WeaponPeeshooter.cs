using Rewired;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static TrainLevelSkeleton;

namespace CupaGoovno;

public class WeaponPeeshooter : AbstractLevelWeapon
{
    public override bool rapidFire => true;
    public override float rapidFireRate => 0.16F;

    public override AbstractProjectile fireBasic()
    {
        BasicProjectile projectile = base.fireBasic() as BasicProjectile;
        projectile.Speed = WeaponProperties.LevelWeaponPeashot.Basic.speed;
        projectile.Damage = CustomWeaponProperties.Peeshooter.Basic.damage;
        projectile.PlayerId = player.id;
        projectile.DamagesType.PlayerProjectileDefault();
        projectile.CollisionDeath.PlayerProjectileDefault();
        CustomWeapons.projectilesBasic.Add(projectile);

        float strayChance = CustomWeaponProperties.Peeshooter.Basic.strayBulletChance;
        if (UnityEngine.Random.Range(0f, 1f) < strayChance)
        {
            MinMax angleMinMax = CustomWeaponProperties.Peeshooter.Basic.strayBulletAngleDiff;
            float strayAngle = angleMinMax.RandomFloat();
            if (Rand.Bool())
            {
                strayAngle *= -1f;
            }
            projectile.transform.AddEulerAngles(0f, 0f, strayAngle);
        }

        float y = yPositions[currentY];
        currentY++;
        if (currentY >= yPositions.Length)
        {
            currentY = 0;
        }
        projectile.transform.AddPosition(0f, y, 0f);

        return projectile;
    }

    public override AbstractProjectile fireEx()
    {
        WeaponPeeShooterExProjectile projectile = base.fireEx() as WeaponPeeShooterExProjectile;
        projectile.Speed = 0f;
        projectile.PlayerId = player.id;
        projectile.DamagesType.PlayerProjectileDefault();
        projectile.CollisionDeath.None();

        projectile.transform.SetPosition(null, Level.Current.Ground - 60f);
        projectile.transform.SetEulerAngles(null, null, 0f);

        float scale = CustomWeaponProperties.Peeshooter.Ex.scale;
        projectile.transform.SetScale(scale, scale);

        SpriteRenderer spriteRenderer = projectile.GetComponent<SpriteRenderer>();
        spriteRenderer.sortingLayerName = "Default";
        spriteRenderer.sortingOrder = 2137;

        CustomWeapons.projectilesBasic.Add(projectile);

        AudioSource audioSource = projectile.gameObject.AddComponent<AudioSource>();
        AudioClip audioClip = AssetLoader<UnityEngine.Object>.GetCachedAsset(lemonPartySoundPath) as AudioClip;
        if (audioSource != null && audioClip != null)
        {
            audioSource.clip = audioClip;
            audioSource.volume = 0.6f * Other.GetSfxVolumeMultiplier();
            audioSource.Play();
        }

        return null;
    }

    public override void BeginBasic()
    {
        OneShotCooldown("player_default_fire_start");
        BasicSoundLoop("player_default_fire_loop", "player_default_fire_loop_p2");
        base.BeginBasic();
    }

    public override void EndBasic()
    {
        ActivateCooldown();
        base.EndBasic();
        StopLoopSound("player_default_fire_loop", "player_default_fire_loop_p2");
    }

    public static string lemonPartySoundPath = "CupaGoovno:cupagoovno\\lemon_party";

    private float[] yPositions =
    [
        -15f,
        0f,
        15f,
        0f
    ];
    private int currentY = 0;
}
