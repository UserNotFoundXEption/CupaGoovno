using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class Booze
{
    public void Init()
    {
        new BoozeDecanter().Init();
        new BoozeTumbler().Init();
        new BoozeMartini().Init();
        new BoozeOlive().Init();
        On.DicePalaceBoozeLevel.Start += Start;
    }

    protected void Start(On.DicePalaceBoozeLevel.orig_Start orig, DicePalaceBoozeLevel self)
    {
        orig(self);
        self.StartCoroutine(beams_cr(self));
    }

    private IEnumerator beams_cr(DicePalaceBoozeLevel self)
    {
        float[] decanterPossibleX = [-500f, -300f, -100f, 100f];
        float initialDelay = 3f;
        //float delay = 0.1f;
        yield return CupheadTime.WaitForSeconds(self, initialDelay);
        for (; ; )
        {
            float decanterX1 = decanterPossibleX[UnityEngine.Random.Range(0, decanterPossibleX.Length)];
            float decanterX2 = decanterX1;
            while(decanterX1 == decanterX2 || Mathf.Abs(decanterX1  -decanterX2) > 400f)
            {
                decanterX2 = decanterPossibleX[UnityEngine.Random.Range(0, decanterPossibleX.Length)];
            }
            self.StartCoroutine(warningBeams_cr(self, decanterX1, decanterX2));
            
            BoozeDecanter.AttackOnce(decanterX1, decanterX2);
            BoozeTumbler.AttackOnce();
            while(BoozeDecanter.attacking || BoozeTumbler.attacking)
            {
                yield return null;
            }
            //yield return CupheadTime.WaitForSeconds(self, delay);
        }
    }

    private IEnumerator warningBeams_cr(DicePalaceBoozeLevel self, float decanterX1, float decanterX2)
    {
        float warningTime = 0.7f;
        GameObject decanterRectangle1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        decanterRectangle1.transform.localScale = new Vector3(200f, 1000f, 1f);
        decanterRectangle1.transform.position = new Vector3(decanterX1, 0f, 0f);
        Other.SetTransparentMaterial(decanterRectangle1, new UnityEngine.Color(1f, 0f, 0f, 0.5f));
        GameObject decanterRectangle2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        decanterRectangle2.transform.localScale = new Vector3(200f, 1000f, 1f);
        decanterRectangle2.transform.position = new Vector3(decanterX2, 0f, 0f);
        Other.SetTransparentMaterial(decanterRectangle2, new UnityEngine.Color(1f, 0f, 0f, 0.5f));
        yield return CupheadTime.WaitForSeconds(self, warningTime);
        GameObject.Destroy(decanterRectangle1);
        GameObject.Destroy(decanterRectangle2);
    }
}
