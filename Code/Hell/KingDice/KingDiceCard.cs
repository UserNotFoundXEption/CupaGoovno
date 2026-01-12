using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CupaGoovno;

public class KingDiceCard
{
    public void Init()
    {
        On.DicePalaceMainLevelCard.parryCooldown_cr += parryCooldown_cr;
    }

    public IEnumerator parryCooldown_cr(On.DicePalaceMainLevelCard.orig_parryCooldown_cr orig, DicePalaceMainLevelCard self)
    {
        float t = 0f;
        while (t < self.coolDown)
        {
            t += CupheadTime.Delta / SuperSandevistan.multiplier;//new
            //t += CupheadTime.Delta;
            yield return null;
        }
        self.SetParryable(true);
        yield return null;
        yield break;
    }
}
