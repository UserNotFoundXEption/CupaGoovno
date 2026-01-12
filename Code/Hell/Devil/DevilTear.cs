using System;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;
public class DevilTear
{
    public void Init()
    {
        On.DevilLevelTear.FixedUpdate += FixedUpdate;
    }

    protected void FixedUpdate(On.DevilLevelTear.orig_FixedUpdate orig, DevilLevelTear self)
    {
        AbstractProj.FixedUpdate(self);
        if (self.dead)
        {
            return;
        }
        //self.transform.AddPosition(0f, -self.speed * CupheadTime.FixedDelta, 0f);
        self.transform.AddPosition(self.speed * CupheadTime.FixedDelta, 0f, 0f);//new
    }

    public static IEnumerator parryCooldown_cr()
    {
        canParry = false;
        yield return CupheadTime.WaitForSeconds(Devil.devil, 0.2f);
        canParry = true;
    }

    public static bool canParry;
}

