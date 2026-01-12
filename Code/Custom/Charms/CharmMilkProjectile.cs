using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class CharmMilkProjectile : CustomProjectile
{
    public static new CharmMilkProjectile Create(Vector2 pos, float rotation)
    {
        if (prefab == null)
        {
            prefab = AssetLoader<UnityEngine.Object>.GetCachedAsset(prefabPath) as GameObject;
            if (prefab == null)
            {
                Plugin.Log("milkProjectile prefab not found: " + prefabPath);
            }
        }
        
        CharmMilkProjectile milk = Create<CharmMilkProjectile>(prefab);

        milk.transform.position = pos;
        milk.transform.SetEulerAngles(0f, 0f, rotation);
        milk.transform.SetScale(p.scale, p.scale);

        float velX = p.speed * Mathf.Cos(rotation * Mathf.Deg2Rad);
        float velY = p.speed * Mathf.Sin(rotation * Mathf.Deg2Rad);
        velY += p.bonusVelY;
        milk.velocity = new Vector2(velX, velY);

        milk.DamagesType.OnlyEnemies();
        milk.CollisionDeath.PlayerProjectileDefault();
        milk.PlayerId = PlayerId.PlayerOne;
        milk.tag = "PlayerProjectile";
        milk.Damage = p.damage;

        SpriteRenderer sr = milk.GetComponent<SpriteRenderer>();
        sr.sortingLayerName = "UI";
        sr.sortingOrder = 2137;

        return milk;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        velocity += Vector2.down * p.gravity * CupheadTime.FixedDelta;
        transform.position += (Vector3)velocity * CupheadTime.FixedDelta;

        float angle = MathUtilities.DirectionToAngle(velocity);
        transform.SetEulerAngles(0f, 0f, angle);
    }

    public override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
    {
        damageDealer.DealDamage(hit);
        base.OnCollisionEnemy(hit, phase);
    }

    
    public const string prefabPath = "CupaGoovno:cupagoovno\\milkProjectilePrefab";

    private static GameObject prefab;
    private static CustomCharmProperties.Milk p = new();

    private Vector2 velocity;
}
