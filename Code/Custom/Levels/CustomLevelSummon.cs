using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public abstract class CustomLevelSummon : CustomProjectile
{
    public static new T Create<T>(GameObject prefab) where T : CustomLevelSummon
    {
        T summon = CustomProjectile.Create<T>(prefab);
        summon.tag = "Enemy";
        return summon;
    }

    public override void Awake()
    {
        base.Awake();
        GetComponent<DamageReceiver>().OnDamageTaken += OnDamageTaken;
    }

    public virtual void OnDamageTaken(DamageDealer.DamageInfo info)
    {
        hp -= info.damage;
        if (hp <= 0f)
        {
            Die();
        }
    }

    public float hp;
}
