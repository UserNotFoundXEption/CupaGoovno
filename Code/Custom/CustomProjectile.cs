using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public abstract class CustomProjectile : AbstractProjectile
{
    public static T Create<T>(GameObject prefab) where T : CustomProjectile
    {
        GameObject projObj = Instantiate<GameObject>(prefab);
        T proj = projObj.AddComponent<T>();
        Other.FixShader(proj.gameObject);

        proj.DamagesType = new DamageDealer.DamageTypesManager();
        proj.DamagesType.OnlyPlayer();
        proj.CollisionDeath = new CollisionProperties();
        proj.CollisionDeath.None();
        proj.tag = "EnemyProjectile";

        return proj;
    }

    public override void Start()
    {
        base.Start();
        damageDealer.damageRate = 0.2f;
    }

    public override void Update()
    {
        base.Update();
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

    public override void SetParryable(bool parryable)
    {
        _canParry = parryable;
        //SetBool(AbstractProjectile.Parry, parryable);
    }

    public override void RandomizeVariant()
    {
    }

    public override void SetTrigger(string trigger)
    {
    }

    public override void Die()
    {
        StopAllCoroutines();
        Destroy(gameObject);
    }

    public override float DestroyLifetime => 60f;
}
