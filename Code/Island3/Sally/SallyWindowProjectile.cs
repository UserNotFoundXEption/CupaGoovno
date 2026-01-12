using Rewired;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class SallyWindowProjectile
{
    public void Init()
    {
        On.SallyStagePlayLevelWindowProjectile.OnCollisionPlayer += OnCollisionPlayer;
        On.SallyStagePlayLevelWindowProjectile.move_cr += move_cr;
        On.SallyStagePlayLevelWindowProjectile.on_ground_hit_cr += on_ground_hit_cr;
    }

    protected void OnCollisionPlayer(On.SallyStagePlayLevelWindowProjectile.orig_OnCollisionPlayer orig, SallyStagePlayLevelWindowProjectile self, object hit, CollisionPhase phase)
    {
        if (self.move)//new
        {//new
            orig(self, hit, phase);
        }//new
    }

    private IEnumerator move_cr(On.SallyStagePlayLevelWindowProjectile.orig_move_cr orig, SallyStagePlayLevelWindowProjectile self)
    {
        self.transform.GetChild(0).tag = "EnemyProjectile";
        self.move = true;
        Vector3 dir = MathUtils.AngleToDirection(self.rotation);
        self.transform.SetEulerAngles(null, null, self.rotation);//new
        while (self.move)
        {
            if (playerDic.ContainsKey(self))//new start
            {
                self.StartCoroutine(followPlayer_cr(self));
                yield break;
            }
            if (!waitDic.ContainsKey(self) || !waitDic[self])
            {
                self.transform.position += dir * self.speed * CupheadTime.FixedDelta;
            }
            else
            {
                yield return delay_cr(self);
            }//new end
             //self.transform.position += dir * self.speed * CupheadTime.FixedDelta;
            yield return new WaitForFixedUpdate();
        }
        yield break;
    }

    private IEnumerator on_ground_hit_cr(On.SallyStagePlayLevelWindowProjectile.orig_on_ground_hit_cr orig, SallyStagePlayLevelWindowProjectile self)
    {
        while (self.transform.position.y > -270f)//new
                                                 //while (self.transform.position.y > (float)Level.Current.Ground)
        {
            yield return null;
        }
        self.move = false;
        if (self.isBaby)
        {
            self.animator.SetTrigger("OnSmash");
            AudioManager.Play("sally_bottle_smash");
            self.emitAudioFromObject.Add("sally_bottle_smash");
        }
        else
        {
            self.animator.Play("Death");
        }
        yield return null;
        yield break;
    }

    public static SallyStagePlayLevelWindowProjectile Create(SallyStagePlayLevelWindowProjectile self, Vector2 pos, float rotation, float speed, AbstractPlayerController player)//new
    {
        SallyStagePlayLevelWindowProjectile sallyStagePlayLevelWindowProjectile = self.Create() as SallyStagePlayLevelWindowProjectile;
        sallyStagePlayLevelWindowProjectile.transform.position = pos;
        sallyStagePlayLevelWindowProjectile.rotation = rotation;
        sallyStagePlayLevelWindowProjectile.speed = speed;
        playerDic[sallyStagePlayLevelWindowProjectile] = player;
        return sallyStagePlayLevelWindowProjectile;
    }

    public static SallyStagePlayLevelWindowProjectile CreateAndWait(SallyStagePlayLevelWindowProjectile self, Vector2 pos, float rotation, float speed)//new
    {
        SallyStagePlayLevelWindowProjectile sallyStagePlayLevelWindowProjectile = self.Create() as SallyStagePlayLevelWindowProjectile;
        sallyStagePlayLevelWindowProjectile.transform.position = pos;
        sallyStagePlayLevelWindowProjectile.rotation = rotation;
        sallyStagePlayLevelWindowProjectile.transform.SetScale(3f, null, null);
        sallyStagePlayLevelWindowProjectile.speed = speed;
        waitDic[sallyStagePlayLevelWindowProjectile] = true;
        return sallyStagePlayLevelWindowProjectile;
    }

    private IEnumerator followPlayer_cr(SallyStagePlayLevelWindowProjectile self)//new
    {
        float accelerationSpeed = 5000f;
        float maxSpeed = 2000f;
        Vector3 velocity = MathUtils.AngleToDirection(self.rotation) * 100f;
        for (; ; )
        {
            Vector3 toPlayer = (playerDic[self].transform.position - self.transform.position).normalized;
            Vector3 acceleration = toPlayer * accelerationSpeed * CupheadTime.Delta;
            velocity += acceleration;
            float exceedRatio = (Mathf.Abs(velocity.x + acceleration.x) + Mathf.Abs(velocity.y + acceleration.y)) / maxSpeed;
            if (exceedRatio > 1)
            {
                velocity /= exceedRatio;
            }
            if (self.move)
            {
                self.transform.position += velocity * CupheadTime.Delta;
            }
            yield return null;
        }
    }

    private IEnumerator delay_cr(SallyStagePlayLevelWindowProjectile self)//new
    {
        float delay = 0.3f;
        yield return CupheadTime.WaitForSeconds(self, delay);
        waitDic[self] = false;
        yield break;
    }

    private static Dictionary<SallyStagePlayLevelWindowProjectile, AbstractPlayerController> playerDic = new Dictionary<SallyStagePlayLevelWindowProjectile, AbstractPlayerController>();
    private static Dictionary<SallyStagePlayLevelWindowProjectile, bool> waitDic = new Dictionary<SallyStagePlayLevelWindowProjectile, bool>();
}
