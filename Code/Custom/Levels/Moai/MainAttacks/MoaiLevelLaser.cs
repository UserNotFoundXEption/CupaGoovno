using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class MoaiLevelLaser : AbstractCollidableObject
{
    public override void Awake()
    {
        base.Awake();

        damageDealer = DamageDealer.NewEnemy();
        damageDealer.SetDirection(DamageDealer.Direction.Neutral, transform);
    }

    public void Update()
    {
        if (damageDealer != null)
        {
            damageDealer.Update();
        }
    }

    public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
    {
        base.OnCollisionPlayer(hit, phase);

        if (phase != CollisionPhase.Exit)
        {
            damageDealer.DealDamage(hit);
        }
    }

    public override void OnCollisionOther(GameObject hit, CollisionPhase phase)
    {
    }

    public override void OnCollisionPlayerProjectile(GameObject hit, CollisionPhase phase)
    {
    }

    private DamageDealer damageDealer;
}
