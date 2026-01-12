using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CupaGoovno;

public class BaronessJawbreaker
{
    public void Init()
    {
        On.BaronessLevelJawbreaker.killminis_cr += killminis_cr;
    }

    private IEnumerator killminis_cr(On.BaronessLevelJawbreaker.orig_killminis_cr orig, BaronessLevelJawbreaker self)
    {
        //self.prefabsList.Reverse();
        for (int i = 0; i < self.prefabsList.Count; i++)
        {
            self.prefabsList[i].StartDying();
            //yield return CupheadTime.WaitForSeconds(self, 0.8f);
        }
        yield break;
    }
}
