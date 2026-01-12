using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class SallyLightning
{
    public void Init()
    {
        On.SallyStagePlayLevelLightning.move_cr += move_cr;
        On.SallyStagePlayLevelLightning.OnCollisionGround += OnCollisionGround;
    }

    protected IEnumerator move_cr(On.SallyStagePlayLevelLightning.orig_move_cr orig, SallyStagePlayLevelLightning self)
    {
        bumpsLeftDic[self] = 3;//new
        self.StartCoroutine(bumpInitialDelay_cr(self));//new
        self.velocity = self.transform.right;
        for (; ; )
        {
            float x = self.transform.position.x;//new start
            float y = self.transform.position.y;
            if (canBumpDic.ContainsKey(self) && canBumpDic[self])
            {
                if (y < (float)Level.Current.Ground || y > (float)Level.Current.Ceiling)
                {
                    self.velocity = new Vector3(self.velocity.x, -self.velocity.y);
                    OnBump(self);
                }
                else if (x > (float)Level.Current.Right || x < (float)Level.Current.Left)
                {
                    self.velocity = new Vector3(-self.velocity.x, self.velocity.y);
                    OnBump(self);
                }
            }
            else
            {
                float offset = 200f;
                bool left = x < (float)Level.Current.Left - offset;
                bool right = x > (float)Level.Current.Right + offset;
                bool down = y < (float)Level.Current.Ground - offset;
                bool up = y > (float)Level.Current.Ceiling + offset;
                bool outBounds = left || right || up || down;
                if (outBounds && bumpsLeftDic[self] <= 0)
                {
                    if (self.lightningLast)
                    {
                        AudioManager.Stop("sally_sally_lightning_move_loop");
                        AudioManager.Play("sally_thunder_end");
                    }
                    self.Die();
                }
            }//new end
            self.transform.position += self.velocity * self.speed * CupheadTime.FixedDelta;
            yield return new WaitForFixedUpdate();
        }
    }

    protected void OnCollisionGround(On.SallyStagePlayLevelLightning.orig_OnCollisionGround orig, SallyStagePlayLevelLightning self, object hit, CollisionPhase phase)
    {

    }

    private void OnBump(SallyStagePlayLevelLightning self)//new
    {
        canBumpDic[self] = false;
        self.StartCoroutine(bumpDelay_cr(self));
        bumpsLeftDic[self]--;
    }

    private IEnumerator bumpDelay_cr(SallyStagePlayLevelLightning self)//new
    {
        yield return CupheadTime.WaitForSeconds(self, 0.5f);
        if (bumpsLeftDic[self] > 0)
        {
            canBumpDic[self] = true;
        }
        yield break;
    }

    private IEnumerator bumpInitialDelay_cr(SallyStagePlayLevelLightning self)//new
    {
        while (self.transform.position.y > 300)
        {
            yield return null;
        }
        canBumpDic[self] = true;
        yield break;
    }

    private static Dictionary<SallyStagePlayLevelLightning, bool> canBumpDic = new Dictionary<SallyStagePlayLevelLightning, bool>();
    private static Dictionary<SallyStagePlayLevelLightning, int> bumpsLeftDic = new Dictionary<SallyStagePlayLevelLightning, int>();
}
