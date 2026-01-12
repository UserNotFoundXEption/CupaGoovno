using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class SuperBooFistProjectile : CustomProjectile
{
    public static SuperBooFistProjectile Create(float x)
    {
        if (prefab == null)
        {
            prefab = AssetLoader<UnityEngine.Object>.GetCachedAsset(prefabPath) as GameObject;
            if (prefab == null)
            {
                Plugin.Log("booFistProjectile prefab not found: " + prefabPath);
            }
        }

        SuperBooFistProjectile fist = Create<SuperBooFistProjectile>(prefab);

        fist.transform.position = new(x, 1000f);
        fist.transform.SetScale(p.scale, p.scale);

        fist.tag = "Player";
        fist.PlayerId = PlayerId.PlayerOne;
        fist.Damage = 0f;
        fist.blockedProjectiles = 0;

        fist.DamagesType.SetAll(false);
        fist.CollisionDeath.None();

        SpriteRenderer sr = fist.GetComponent<SpriteRenderer>();
        sr.sortingLayerName = "Player";
        sr.sortingOrder = 0;

        return fist;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (transform.position.y > 0f)
        {
            transform.AddPosition(default, -p.fallingSpeed * CupheadTime.FixedDelta);
        }
    }

    public override void OnCollisionEnemyProjectile(GameObject hit, CollisionPhase phase)
    {
        if (blockedProjectiles < p.maxBlockedProjectiles && CheckListsProjectile(hit.name))
        {
            base.OnCollisionEnemyProjectile(hit, phase);
            OnObjectDestroyed(hit, phase);
        }
    }

    public override void OnCollision(GameObject hit, CollisionPhase phase)
    {
        if (blockedProjectiles < p.maxBlockedProjectiles && CheckListGeneral(hit.name))
        {
            base.OnCollision(hit, phase);
            OnObjectDestroyed(hit, phase);
        }
    }

    private void OnObjectDestroyed(GameObject hit, CollisionPhase phase)
    {
        blockedProjectiles++;
        AbstractProjectile proj = hit.GetComponent<AbstractProjectile>();
        DamageReceiver receiver = hit.GetComponent<DamageReceiver>();

        if (proj != null)
        {
            proj.Die();
        }

        if(receiver != null)
        {
            receiver.TakeDamage(new DamageDealer.DamageInfo(
                2137f, 
                DamageDealer.Direction.Neutral, 
                Vector2.zero, 
                damageSource));
        }

        if (hit != null)
        {
            Destroy(hit);
        }

        if (blockedProjectiles >= p.maxBlockedProjectiles)
        {
            Die();
        }
    }

    private bool CheckListsProjectile(string name)
    {
        return isProjectileOnForceList(name) || !isProjectileOnBlackList(name);
    }

    private bool CheckListGeneral(string name)
    {
        return isGeneralOnForceList(name);
    }

    private bool isProjectileOnForceList(string name)
    {
        return isOnList(name, forceListProjectileMoai, forceListProjectile);
    }

    private bool isProjectileOnBlackList(string name)
    {
        return isOnList(name, blackListProjectileMoai, blackListProjectile);
    }

    private bool isGeneralOnForceList(string name)
    {
        return isOnList(name, null, forceListGeneral);
    }

    private bool isOnList(string name, string[] moaiList, string[] list)
    {
        bool force = false;
        if(Level.Current is MoaiLevel && moaiList != null)
        {
            foreach(string listedName in moaiList)
            {
                if (name.Contains(listedName))
                {
                    force = true;
                    break;
                }
            }
        }

        foreach (string listedName in list)
        {
            if (name.Contains(listedName))
            {
                force = true;
                break;
            }
        }

        return force;
    }

    public const string prefabPath = "CupaGoovno:cupagoovno\\booFistProjectilePrefab";

    public override float DestroyLifetime => p.destroyLifetime;

    private static GameObject prefab;
    private static CustomSuperProperties.BooFist p = new();
    private static string[] forceListProjectileMoai =
    [
        "Bouncer",
        "Spark"
    ];
    private static string[] blackListProjectileMoai =
    [
        "Heart",
        "Baseball",
        "Laser"
    ];
    private static string[] forceListProjectile = 
    [

    ];
    private static string[] blackListProjectile = 
    [
        "runNGunFollower",
        "Devil_Spinner_Orbiting_Projectile",
        "Devil_Spinner_Projectile",
        "Devil_Skull",
        "Train_Lollipop_Lightning",
        "Bee_Follower",
        "Bee_Triangle_Invincible"
    ];
    private static string[] forceListGeneral =
    [
        "Dragon_FireMarcher_B",
        "Dragon_Potion_Both",
        "Dragon_Potion_Bullet",
        "Train_Eye_Projectile",
        "SallyStagePlay_Lightning",
        "Baroness_Baroness_Projectile"
    ];

    private int blockedProjectiles;
}
