using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class DevilArm
{
    public void Init()
    {
        On.DevilLevelDevilArm.move_away_cr += move_away_cr;
        On.DevilLevelDevilArm.Awake += Awake;
    }

    protected void Awake(On.DevilLevelDevilArm.orig_Awake orig, DevilLevelDevilArm self)
    {
        orig(self);
        self.transform.SetPosition(null, -300f, null);
    }

    private IEnumerator move_away_cr(On.DevilLevelDevilArm.orig_move_away_cr orig, DevilLevelDevilArm self)
    {
        yield return CupheadTime.WaitForSeconds(self, 5f);
        yield return orig(self);
    }
}
