using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class WeaponMangetsunamiProjectile : BasicProjectile
{
    private void Bounce(bool vertical)
    {
        float z = transform.eulerAngles.z;
        if (vertical)
        {
            transform.SetEulerAngles(0f, 0f, -z);
        }
        else
        {
            transform.SetEulerAngles(0f, 0f, 180f - z);
        }

        float multiplier = CustomWeaponProperties.Mangetsunami.Basic.bounceMultiplier;
        Damage *= multiplier;
        transform.localScale *= Mathf.Sqrt(multiplier);
        bounces++;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if(!Charged || bounces >= CustomWeaponProperties.Mangetsunami.Basic.maxBounces)
        {
            return;
        }

        float x = transform.position.x;
        float y = transform.position.y;
        if(CupheadLevelCamera.Current != null)
        {
            x -= CupheadLevelCamera.Current.transform.position.x;
            y -= CupheadLevelCamera.Current.transform.position.y;
        }

        float angle = transform.eulerAngles.z;
        while(angle > 180f)
        {
            angle -= 360f;
        }
        bool goingRight = Mathf.Abs(angle) < 90f;
        bool goingUp = Mathf.Sign(angle) > 0f;

        if ((goingUp && y > 360f) || (!goingUp && y < -360f))
        {
            Bounce(true);
        }
        if((goingRight && x > 640f) || (!goingRight && x < -640f))
        {
            Bounce(false);
        }
    }

    public bool Charged
    {
        set
        {
            _charged = value;
            if (value)
            {
                Damage = CustomWeaponProperties.Mangetsunami.Basic.damageCharged;
                transform.SetScale(CustomWeaponProperties.Mangetsunami.Basic.chargedScale, CustomWeaponProperties.Mangetsunami.Basic.chargedScale);
            }
            else
            {
                Damage = CustomWeaponProperties.Mangetsunami.Basic.damageUncharged;
                transform.SetScale(CustomWeaponProperties.Mangetsunami.Basic.unchargedScale, CustomWeaponProperties.Mangetsunami.Basic.unchargedScale);
            }
        }
        private get
        {
            return _charged;
        }
    }

    public override float DestroyLifetime => 10f;

    private int bounces = 0;
    private bool _charged;
}
