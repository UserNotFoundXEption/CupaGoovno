using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class MoaiLevelBaseballBat : CustomProjectile
{
    public static new MoaiLevelBaseballBat Create()
    {
        MoaiLevelBaseballBat baseball = Create<MoaiLevelBaseballBat>(prefab);

        baseball.transform.SetScale(p.batScale, p.batScale);

        return baseball;
    }

    public static GameObject prefab;

    private static MoaiProperties.Baseball p = new();
}
