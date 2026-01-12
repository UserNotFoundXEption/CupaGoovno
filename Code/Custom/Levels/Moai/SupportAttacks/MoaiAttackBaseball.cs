using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public static class MoaiAttackBaseball
{
    public static IEnumerator attack_cr(float parameter)
    {
        yield return CupheadTime.WaitForSeconds(MoaiLevel.moai, p.initialDelay);

        MoaiLevelBaseball.Create(parameter);
    }

    private static MoaiProperties.Baseball p = new();
}
