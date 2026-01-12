using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class WeaponMangetsunamiExProjectile : BasicProjectile
{
    public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
    {
        base.OnCollisionPlayer(hit, phase);
        if (!speedUp)
        {
            LevelPlayerMotor player = hit.GetComponent<LevelPlayerMotor>();
            if (player != null && player.Dashing)
            {
                speedUp = true;
                Damage = CustomWeaponProperties.Mangetsunami.Ex.damageAfterDash;
                Speed = CustomWeaponProperties.Mangetsunami.Ex.speedAfterDash;
                if (Mathf.Sign(direction.x) != Math.Sign(player.transform.localScale.x))
                {
                    direction.x *= -1f;
                }
            }
        }
    }

    public override void Move()
    {
        //base.transform.position += this.Direction * this.Speed * CupheadTime.FixedDelta - new Vector3(0f, this._accumulativeGravity * CupheadTime.FixedDelta, 0f);
        //this._accumulativeGravity += this.Gravity * CupheadTime.FixedDelta;
        transform.position += direction * Speed * CupheadTime.FixedDelta;
        transform.AddEulerAngles(0f, 0f, Speed * CupheadTime.FixedDelta);
    }

    public override float DestroyLifetime => 2137f;
    public Vector3 direction;

    private bool speedUp = false;
}
