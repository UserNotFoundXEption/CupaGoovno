using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class WeaponSkyticklerExProjectile : WeaponUpshotExProjectile
{
    public override void FixedUpdate()
    {
        //abstractprojectile fixedupdate start
        if (this.firingHitbox != null)
        {
            if (this.firstUpdate)
            {
                this.firstUpdate = false;
            }
            else
            {
                if (!this.dead)
                {
                    base.GetComponent<Collider2D>().enabled = true;
                }
                UnityEngine.Object.Destroy(this.firingHitbox.gameObject);
                this.firingHitbox = null;
            }
        }
        if (this.damageDealer != null)
        {
            this.damageDealer.FixedUpdate();
        }
        //abstractprojectile fixedupdate end
        if (base.dead)
        {
            return;
        }
        if (this.timeUntilUnfreeze > 0f)
        {
            this.timeUntilUnfreeze -= CupheadTime.FixedDelta;
            return;
        }
        this.time += CupheadTime.FixedDelta;
        this.angle += Mathf.Lerp(WeaponProperties.LevelWeaponUpshot.Ex.minRotationSpeed, WeaponProperties.LevelWeaponUpshot.Ex.maxRotationSpeed, this.time / WeaponProperties.LevelWeaponUpshot.Ex.rotationRampTime) * CupheadTime.FixedDelta * this.rotateDir;
        this.radius += Mathf.Lerp(WeaponProperties.LevelWeaponUpshot.Ex.minRadiusSpeed, WeaponProperties.LevelWeaponUpshot.Ex.maxRadiusSpeed, this.time / WeaponProperties.LevelWeaponUpshot.Ex.radiusRampTime) * CupheadTime.FixedDelta;
        Vector2 vec = MathUtils.AngleToDirection(this.angle) * this.radius;
        base.transform.position = this.startPos + new Vector3(vec.x, vec.y);
        float num = Mathf.Round(this.time * 24f) / 24f;
        base.transform.localScale = Vector3.Lerp(this.startScale, this.endScale, num * 5f);
        //num *= 0.2f;
        //this.trail1.color = new Color(1f, 1f, 1f, 0.5f - num);
        //this.trail2.color = new Color(1f, 1f, 1f, 0.25f - num);
        //this.UpdateTrails();
    }
}
