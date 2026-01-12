using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class BaronessCupcake
{
    public void Init()
    {
        On.BaronessLevelCupcake.splash_cr += splash_cr;
    }

    private IEnumerator splash_cr(On.BaronessLevelCupcake.orig_splash_cr orig, BaronessLevelCupcake self, bool onLeft, float posX)
    {
        float originalOffset = (!onLeft) ? (-self.properties.splashOriginalOffset) : self.properties.splashOriginalOffset;
        float offset = (!onLeft) ? (-self.properties.splashOffset) : self.properties.splashOffset;
        float delay = 0.4f;
        int value = 0;
        //for (int i = 0; i < 3; i++)
        for (int i = 0; i < 10; i++)
        {
            /*if (onLeft)
            {
                value = i;
            }
            else*/
            if (i == 0)
            {
                value = 2;
            }
            else if (i == 1)
            {
                value = 0;
            }
            else
            {
                value = 1;
            }
            Effect splash = self.splashPrefab.Create(new Vector2(posX + originalOffset + offset * (float)i, (float)Level.Current.Ground));
            float scale = (!onLeft) ? splash.transform.localScale.x : (-splash.transform.localScale.x);
            splash.animator.SetInteger("SplashType", value);
            splash.transform.SetScale(new float?(scale), null, null);
            yield return CupheadTime.WaitForSeconds(self, delay);
        }
        yield break;
    }
}
