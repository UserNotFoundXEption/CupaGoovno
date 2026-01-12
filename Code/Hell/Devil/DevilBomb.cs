using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class DevilBomb
{
    public void Init()
    {
        On.DevilLevelBomb.Start += Start;
    }

    protected void Start(On.DevilLevelBomb.orig_Start orig, DevilLevelBomb self)
    {
        orig(self);
        self.StartCoroutine(turnRed_cr(self));
    }

    private IEnumerator turnRed_cr(DevilLevelBomb self)
    {
        float time = self.properties.explodeDelay;
        float t = 0;
        while (t < time)
        {
            t += CupheadTime.delta;
            float percent = t / time;
            Color color = new Color(1, 1 - percent, 1 - percent);
            SpriteRenderer renderer = self.GetComponent<SpriteRenderer>();
            if(renderer != null)
            {
                renderer.color = color;
            }
            yield return null;
        }
        yield break;
    }
}
