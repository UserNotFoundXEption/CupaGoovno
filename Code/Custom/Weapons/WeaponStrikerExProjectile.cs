using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Diagnostics;

namespace CupaGoovno;

public class WeaponStrikerExProjectile : AbstractProjectile
{
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (dead)
        {
            return;
        }

        if (timeUntilUnfreeze > 0f)
        {
            timeUntilUnfreeze -= CupheadTime.FixedDelta;
            currentSpeed = 0f;
        }
        else
        {
            currentSpeed = speed;
        }

        damageCooldown -= CupheadTime.FixedDelta;//new
        Vector2 vector = MathUtils.AngleToDirection(transform.eulerAngles.z) * currentSpeed;
        transform.AddPosition(vector.x * CupheadTime.FixedDelta, vector.y * CupheadTime.FixedDelta, 0f);
    }

    public override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
    {
        base.OnCollisionEnemy(hit, phase);
        float num = damageDealer.DealDamage(hit);
        /*totalDamage += num;
        if (totalDamage > this.maxDamage)
        {
            this.Die();
        }
        if (num > 0f)*/
        if(damageCooldown <= 0f)//new
        {
            totalDamage += Damage;//new
            damageCooldown = damageDealer.damageRate;//new
            //this.hitFXPrefab.Create(this.hitFxRoot.position);
            AudioManager.Play("player_ex_impact_hit");
            emitAudioFromObject.Add("player_ex_impact_hit");
            timeUntilUnfreeze = hitFreezeTime;
        }

        if (totalDamage > CustomWeaponProperties.Striker.Ex.maxDamage)//new start
        {
            Die();
        }//new end
    }

    public override void OnCollisionOther(GameObject hit, CollisionPhase phase)
    {
        if (hit.tag == "Parry")
        {
            return;
        }
        base.OnCollisionOther(hit, phase);
    }

    float timeUntilUnfreeze = 0f;
    float totalDamage = 0f;
    float speed = WeaponProperties.LevelWeaponPeashot.Ex.speed;
    float currentSpeed = WeaponProperties.LevelWeaponPeashot.Ex.speed;
    float damageCooldown = 0f;
    public static float hitFreezeTime = 0.05f;
}
