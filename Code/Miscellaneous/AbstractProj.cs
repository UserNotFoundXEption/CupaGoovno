using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static TrainLevelPlatform;

namespace CupaGoovno;

public class AbstractProj
{
    public static void FixedUpdate(AbstractProjectile self)
    {
        if (self.firingHitbox != null)
        {
            if (self.firstUpdate)
            {
                self.firstUpdate = false;
            }
            else
            {
                if (!self.dead)
                {
                    self.GetComponent<Collider2D>().enabled = true;
                }
                UnityEngine.Object.Destroy(self.firingHitbox.gameObject);
                self.firingHitbox = null;
            }
        }
        if (self.damageDealer != null)
        {
            self.damageDealer.FixedUpdate();
        }
    }

    public static void Update(AbstractProjectile self)
    {
        Vector3 position = self.transform.position;
        if (self.lifetime == 0f)
        {
            self.lastPosition = (self.startPosition = position);
        }

        if (self.DestroyDistance > 0f && Vector3.Distance(self.startPosition, position) >= self.DestroyDistance)
        {
            self.OnDieDistance();
        }

        self.distance += Vector3.Distance(self.lastPosition, position);
        self.lastPosition = position;
        if (self.DestroyLifetime > 0f && self.lifetime >= self.DestroyLifetime)
        {
            self.OnDieLifetime();
        }

        self.lifetime += Time.deltaTime;
        if (self.DestroyedAfterLeavingScreen)
        {
            bool flag = CupheadLevelCamera.Current.ContainsPoint(position, new Vector2(150f, self._setYPadding));
            if (self.hasBeenRendered && !flag)
            {
                UnityEngine.Object.Destroy(self.gameObject);
            }

            if (!self.hasBeenRendered)
            {
                self.hasBeenRendered = flag;
            }
        }
    }

    public static void Start(AbstractProjectile self)
    {
        self.damageDealer = new DamageDealer(self);
        self.damageDealer.OnDealDamage += self.OnDealDamage;
        self.damageDealer.SetStoneTime(self.StoneTime);
        self.damageDealer.PlayerId = self.PlayerId;
        if (self.tracker != null)
        {
            self.tracker.Add(self.damageDealer);
        }
    }
}
