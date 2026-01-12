using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class MermaidFishSpreadProjectile
{
    public static IEnumerator RejectSpreadshotEmbraceSpinner(BasicProjectile self)
    {
        self.GetComponentInChildren<SpriteRenderer>().color = Color.cyan;
        self.transform.SetScale(2f, 2f, null);
        startXDic[self] = self.transform.position.x;
        startYDic[self] = self.transform.position.y;
        while(self.transform.position.x > -750f)
        {
            float xDelta = startXDic[self] - self.transform.position.x;
            float ySin = Mathf.Sin(xDelta/100f) * 300f;
            self.transform.SetPosition(null, startYDic[self] + ySin + xDelta/5f, null);
            yield return null;
        }
        yield break;
    }

    private static Dictionary<BasicProjectile, float> startXDic = new Dictionary<BasicProjectile, float>();
    private static Dictionary<BasicProjectile, float> startYDic = new Dictionary<BasicProjectile, float>();
}
