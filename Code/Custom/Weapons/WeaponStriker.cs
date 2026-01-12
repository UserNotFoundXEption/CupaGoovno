using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CupaGoovno;

public class WeaponStriker : AbstractLevelWeapon
{
    public override bool rapidFire => true;
    public override float rapidFireRate => 0.16F;

    public override AbstractProjectile fireBasic()
    {
        BasicProjectile projectile = base.fireBasic() as BasicProjectile;
        projectile.Speed = WeaponProperties.LevelWeaponPeashot.Basic.speed;
        projectile.Damage = CustomWeaponProperties.Striker.Basic.damage;
        projectile.PlayerId = player.id;
        projectile.DamagesType.PlayerProjectileDefault();
        projectile.CollisionDeath.PlayerProjectileDefault();
        CustomWeapons.projectilesBasic.Add(projectile);

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
        WeaponStrikerExProjectile projectile = base.fireEx() as WeaponStrikerExProjectile;
        projectile.Damage = CustomWeaponProperties.Striker.Ex.damage;
        projectile.DamageRate = WeaponStrikerExProjectile.hitFreezeTime + WeaponProperties.LevelWeaponPeashot.Ex.damageDistance / WeaponProperties.LevelWeaponPeashot.Ex.speed;
        projectile.PlayerId = player.id;
        projectile.CollisionDeath.Ground = false;//new
        MeterScoreTracker meterScoreTracker = new(MeterScoreTracker.Type.Ex);
        projectile.DamagesType.PlayerProjectileDefault();
        meterScoreTracker.Add(projectile);
        return projectile;
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

    private float[] yPositions =
    [
        0f,
        20f,
        40f,
        20f
    ];
    private int currentY = 0;
}
