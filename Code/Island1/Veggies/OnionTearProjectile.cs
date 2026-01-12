using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class OnionTearProjectile
{
    public void Init()
    {
        On.VeggiesLevelOnionTearProjectile.projectile_cr += projectile_cr;
    }

    private IEnumerator projectile_cr(On.VeggiesLevelOnionTearProjectile.orig_projectile_cr orig, VeggiesLevelOnionTearProjectile self)
    {
        YieldInstruction wait = new WaitForFixedUpdate();
        float startY = self.transform.position.y;
        float endY = (float)Level.Current.Ground - 200f;//200f nie bylo

        if (UnityEngine.Random.Range(0, 2) == 0)//new start
        {
            float tmp;
            tmp = startY;
            startY = endY;
            endY = tmp;
            self.transform.SetScale(1f, -1f, 1f);
        }
        bool horizontal = UnityEngine.Random.Range(0, 5) == 0;
        if (horizontal)
        {
            self.transform.SetScale(1f, 1f, 1f);
            self.transform.rotation = Quaternion.Euler(0f, 0f, -90f);
        }//new end
        float calculatedTime = self.time;
        float t = 0f;
        while (t < calculatedTime)
        {
            float val = t / calculatedTime;
            val *= val;//new start
            if (horizontal)
            {
                float x = EaseUtils.Ease(EaseUtils.EaseType.easeInQuad, 650f, -650f, val);
                self.transform.SetPosition(new float?(x), -60f, null);
            }
            else
            {//new end
                float y = EaseUtils.Ease(EaseUtils.EaseType.easeInQuad, startY, endY, val);
                self.transform.SetPosition(null, new float?(y), null);
            }//new
            t += CupheadTime.FixedDelta;
            yield return wait;
        }
        self.transform.SetPosition(null, new float?(endY), null);
        AudioManager.Play("level_veggies_onion_teardrop");
        self.Die();
        yield break;
    }
}
