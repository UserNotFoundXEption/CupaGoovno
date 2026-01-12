using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class FrogTigerBullet
{
    public void Init()
    {
        On.FrogsLevelTigerBullet.bullet_cr += bullet_cr;
    }

    private IEnumerator bullet_cr(On.FrogsLevelTigerBullet.orig_bullet_cr orig, FrogsLevelTigerBullet self)
    {
        float t = 0f;
        Transform trans = self.bullet.transform;
        float start = trans.localPosition.y;
        float end = start + 500f;
        float time = 0.6f;
        for (; ; )
        {
            t = 0f;
            AudioManager.Play("level_frogs_ball_platform_ball_launch");
            while (t < time)//new
            //while (t < 0.5f)
            {
                float val = t / time;//new
                //float val = t / 0.5f;
                float y = EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, start, end, val);
                trans.SetLocalPosition(null, new float?(y), null);
                t += CupheadTime.Delta;
                yield return null;
            }
            t = 0f;
            while (t < time)//new
            //while (t < 0.5f)
            {
                float val2 = t / time;//new
                //float val2 = t / 0.5f;
                float y2 = EaseUtils.Ease(EaseUtils.EaseType.easeInSine, end, start, val2);
                trans.SetLocalPosition(null, new float?(y2), null);
                t += CupheadTime.Delta;
                yield return null;
            }
        }
    }
}
