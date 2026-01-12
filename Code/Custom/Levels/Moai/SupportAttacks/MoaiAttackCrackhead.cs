using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public static class MoaiAttackCrackhead
{
    public static IEnumerator attack_cr(float parameter)
    {
        yield return CupheadTime.WaitForSeconds(MoaiLevel.moai, p.initialDelay);

        MoaiLevelCrackhead.Create(new Vector2(-700f, -150f), parameter);
    }

    private static MoaiProperties.Crackhead p = new();
}
