using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class WeaponSkytickler : AbstractLevelWeapon
{
    public override bool rapidFire => true;
    public override float rapidFireRate => WeaponProperties.LevelWeaponUpshot.Basic.fireRate;

    public override AbstractProjectile fireBasic()
    {
        animationCycleCount++;
        for (int i = 0; i < 3; i++)
        {
            WeaponUpshotProjectile weaponUpshotProjectile = i != 0 ? fireBasicNoEffect() as WeaponUpshotProjectile : base.fireBasic() as WeaponUpshotProjectile;
            if (i == 1)
            {
                weaponUpshotProjectile.GetComponent<SpriteRenderer>().sortingOrder = 1;
            }
            weaponUpshotProjectile.Damage = WeaponProperties.LevelWeaponUpshot.Basic.damage;
            weaponUpshotProjectile.PlayerId = player.id;
            weaponUpshotProjectile.DamagesType.PlayerProjectileDefault();
            weaponUpshotProjectile.CollisionDeath.PlayerProjectileDefault();
            weaponUpshotProjectile.CollisionDeath.Other = false;
            //weaponUpshotProjectile.xSpeed = WeaponProperties.LevelWeaponUpshot.Basic.xSpeed[i];
            weaponUpshotProjectile.xSpeed = CustomWeaponProperties.Skytickler.Basic.xSpeed[i];//new
            weaponUpshotProjectile.ySpeedMinMax = WeaponProperties.LevelWeaponUpshot.Basic.ySpeed[i];
            weaponUpshotProjectile.timeToArc = WeaponProperties.LevelWeaponUpshot.Basic.timeToMaxSpeed[i];
            weaponUpshotProjectile.animator.Play(((animationCycleCount + i) % 3).ToString(), 0, UnityEngine.Random.Range(0f, 1f));
            CustomWeapons.projectilesUpshot.Add(weaponUpshotProjectile);//new
        }
        return null;
    }

    public override AbstractProjectile fireEx()
    {
        WeaponUpshotExProjectile weaponUpshotExProjectile = base.fireEx() as WeaponUpshotExProjectile;
        weaponUpshotExProjectile.Damage = WeaponProperties.LevelWeaponUpshot.Ex.damage;
        weaponUpshotExProjectile.DamageRate = WeaponProperties.LevelWeaponUpshot.Ex.damageRate;
        weaponUpshotExProjectile.PlayerId = player.id;
        weaponUpshotExProjectile.rotateDir = Mathf.Sign(player.gameObject.transform.localScale.x);
        weaponUpshotExProjectile.CollisionDeath.Ground = false;
        MeterScoreTracker meterScoreTracker = new MeterScoreTracker(MeterScoreTracker.Type.Ex);
        meterScoreTracker.Add(weaponUpshotExProjectile);
        return weaponUpshotExProjectile;
    }

    public override void BeginBasic()
    {
        base.BeginBasic();
        AudioManager.Play("player_weapon_upshot_start");
        emitAudioFromObject.Add("player_weapon_upshot_start");
        BasicSoundLoop("player_weapon_upshot_loop_p1", "player_weapon_upshot_loop_p2");
    }

    public override void EndBasic()
    {
        base.EndBasic();
        StopLoopSound("player_weapon_upshot_loop_p1", "player_weapon_upshot_loop_p2");
    }

    private int animationCycleCount = 0;
}
