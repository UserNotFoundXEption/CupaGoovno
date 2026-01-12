using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class WeaponMangetsunami : AbstractLevelWeapon
{
    public override bool rapidFire => false;
    public override bool isChargeWeapon => true;
    public override float rapidFireRate => CustomWeaponProperties.Mangetsunami.Basic.fireRate;

    public override void StartCharging()
    {
        base.StartCharging();
        this.BasicSoundOneShot("player_weapon_charge_start", "player_weapon_charge_start_p2");
        timeCharged = 0f;
        playedFullyChargedSound = false;
    }

    public override void StopCharging()
    {
        timeCharged = 0f;
    }


    public void FixedUpdate()
    {
        timeCharged += CupheadTime.FixedDelta / SuperSandevistan.multiplier;
        if(!playedFullyChargedSound && FullyCharged)
        {
            AudioManager.Play("player_weapon_charge_ready");
            playedFullyChargedSound = true;
        }
    }

    public override AbstractProjectile fireBasic()
    {
        if(FullyCharged)
        {
            FireCharged();
			BasicSoundOneShot("player_weapon_charge_full_fireball", "player_weapon_charge_full_fireball_p2");
        }
        else
        {
            FireUncharged();
			BasicSoundOneShot("player_weapon_charge_fire_small", "player_weapon_charge_fire_small_p2");
        }
        timeCharged = 0f;
        return null;
    }

    private void FireUncharged()
    {
        WeaponMangetsunamiProjectile projectile = FireOneProjectile();
        float scale = CustomWeaponProperties.Mangetsunami.Basic.unchargedScale;
        projectile.transform.SetScale(scale, scale);
        projectile.Charged = false;
    }

    private void FireCharged()
    {
        for (int i = 0; i < 3; i++)
        {
            WeaponMangetsunamiProjectile projectile = FireOneProjectile();

            float spreadAngle = CustomWeaponProperties.Mangetsunami.Basic.spreadAngle;
            projectile.transform.AddEulerAngles(0f, 0f, -spreadAngle + i * spreadAngle);

            float scale = CustomWeaponProperties.Mangetsunami.Basic.chargedScale;
            projectile.transform.SetScale(scale, scale);

            projectile.Charged = true;
        }
    }

    private WeaponMangetsunamiProjectile FireOneProjectile()
    {
        WeaponMangetsunamiProjectile projectile = base.fireBasic() as WeaponMangetsunamiProjectile;
        projectile.Speed = CustomWeaponProperties.Mangetsunami.Basic.speed;
        projectile.Damage = CustomWeaponProperties.Mangetsunami.Basic.chargedScale;
        projectile.PlayerId = player.id;
        projectile.DamagesType.PlayerProjectileDefault();
        projectile.CollisionDeath.SetAll(false);
        projectile.CollisionDeath.Enemies = true;
        CustomWeapons.projectilesBasic.Add(projectile);

        return projectile;
    }

    public override AbstractProjectile fireEx()
    {
        WeaponMangetsunamiExProjectile projectile = base.fireEx() as WeaponMangetsunamiExProjectile;
        projectile.Speed = CustomWeaponProperties.Mangetsunami.Ex.startingSpeed;
        projectile.Damage = CustomWeaponProperties.Mangetsunami.Ex.startingDamage;
        projectile.PlayerId = player.id;
        projectile.DamagesType.PlayerProjectileDefault();
        projectile.CollisionDeath.SetAll(false);
        projectile.CollisionDeath.Enemies = true;
        CustomWeapons.projectilesBasic.Add(projectile);

        projectile.direction = projectile.transform.right;

        MeterScoreTracker meterScoreTracker = new(MeterScoreTracker.Type.Ex);
        meterScoreTracker.Add(projectile);
        return projectile;
    }

    public override void BeginBasic()
    {
        if (FullyCharged)
        {
            BeginBasicCheckAttenuation("player_weapon_charge_full_fireball", "player_weapon_charge_full_fireball_p2");
        }
        else
        {
            BeginBasicCheckAttenuation("player_weapon_charge_fire_small", "player_weapon_charge_fire_small_p2");
        }
        base.BeginBasic();
    }

    public override void EndBasic()
    {
        if (FullyCharged)
        {
            EndBasicCheckAttenuation("player_weapon_charge_full_fireball", "player_weapon_charge_full_fireball_p2");
        }
        else
        {
            EndBasicCheckAttenuation("player_weapon_charge_fire_small", "player_weapon_charge_fire_small_p2");
        }
        base.EndBasic();
    }

    private float timeCharged;
    private bool playedFullyChargedSound;

    private bool FullyCharged
    {
        get
        {
            return timeCharged >= CustomWeaponProperties.Mangetsunami.Basic.chargeTime;
        }
    }
}
