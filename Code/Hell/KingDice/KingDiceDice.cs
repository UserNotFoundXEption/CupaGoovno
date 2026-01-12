using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class KingDiceDice
{
    public void Init()
    {
        On.DicePalaceMainLevelDice.move_cr += move_cr;
    }

    private IEnumerator move_cr(On.DicePalaceMainLevelDice.orig_move_cr orig, DicePalaceMainLevelDice self)
    {
        self.pivotPoint.AddPosition(0f, 350f, 0f);
        self.transform.AddPosition(0f, 350f, 0f);
        SpriteRenderer spriteRenderer = self.GetComponent<SpriteRenderer>();
        while(spriteRenderer == null)
        {
            yield return null;
            spriteRenderer = self.GetComponent<SpriteRenderer>();
        }
        spriteRenderer.sortingLayerName = "UI";
        spriteRenderer.sortingOrder = int.MaxValue;
        yield return orig(self);
    }

}
