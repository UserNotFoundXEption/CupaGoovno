using System.Collections;
using UnityEngine;

namespace CupaGoovno;

public class BirdSmall
{
    public void Init()
    {
        On.FlyingBirdLevelSmallBird.OnDamageTaken += OnDamageTaken;
        On.FlyingBirdLevelSmallBird.ShootProjectile += ShootProjectile;
        On.FlyingBirdLevelSmallBird.Turn += Turn;
    }

    private void OnDamageTaken(On.FlyingBirdLevelSmallBird.orig_OnDamageTaken orig, FlyingBirdLevelSmallBird self, DamageDealer.DamageInfo info)
    {
        if (info.damageSource == DamageDealer.DamageSource.Super || info.damageSource == DamageDealer.DamageSource.Ex)
        {
            orig(self, info);
        }
    }

    private void ShootProjectile(On.FlyingBirdLevelSmallBird.orig_ShootProjectile orig, FlyingBirdLevelSmallBird self)
    {
        self.aim.LookAt2D(PlayerManager.Current.center);
        //self.bulletPrefab.Create(self.bulletRoot.position, self.aim.eulerAngles.z + 180f, -self.properties.CurrentState.smallBird.shotSpeed).SetParryable(true);
        Vector3 pos = self.bulletRoot.position;//new start
        float angle = self.aim.eulerAngles.z + 180f + UnityEngine.Random.Range(-15f, 15f);
        float speed = -self.properties.CurrentState.smallBird.shotSpeed;
        bool parryable = UnityEngine.Random.Range(0, 3) == 0;
        BasicProjectile projectile = self.bulletPrefab.Create(pos, angle, speed);
        projectile.SetParryable(parryable);
        if (!parryable)
        {
            SpriteRenderer spriteRenderer = projectile.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.color = Color.red;
            }
        }//new end
    }

    private Coroutine Turn(On.FlyingBirdLevelSmallBird.orig_Turn orig, FlyingBirdLevelSmallBird self, FlyingBirdLevelSmallBird.Direction d)
    {
        //return self.StartCoroutine(self.turn_cr(d));
        return null;
    }
}
