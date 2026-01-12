using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public static class MoaiAttackBouncers
{
    public static IEnumerator attack_cr(float parameter)
    {
        yield return CupheadTime.WaitForSeconds(MoaiLevel.moai, p.initialDelay);

        for(int i = 0; i < p.count; i++)
        {
            float x = p.startX.RandomFloat();
            float y = p.startY.RandomFloat();
            MoaiLevelBouncer.Create(new Vector2(x, y), parameter);
            yield return CupheadTime.WaitForSeconds(MoaiLevel.moai, p.delay);
        }
    }

    private static MoaiProperties.Bouncers p = new();
}
