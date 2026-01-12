using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CupaGoovno;

public class DragonMeteor
{
    public void Init()
    {
        On.DragonLevelMeteor.smoke_cr += smoke_cr;
    }

    private IEnumerator smoke_cr(On.DragonLevelMeteor.orig_smoke_cr orig, DragonLevelMeteor self)
    {/*
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, 0.1f);
			this.smokePrefab.Create(base.transform.position).transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(base.transform.eulerAngles.z + UnityEngine.Random.Range(-45f, 45f)));
			yield return null;
		}*/
        yield break;
    }
}
