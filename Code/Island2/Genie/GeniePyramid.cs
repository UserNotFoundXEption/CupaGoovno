using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class GeniePyramid
{
    public void Init()
    {
        On.FlyingGenieLevelPyramid.Awake += Awake;
        On.FlyingGenieLevelPyramid.PathMovement += PathMovement;
    }

    protected void Awake(On.FlyingGenieLevelPyramid.orig_Awake orig, FlyingGenieLevelPyramid self)
    {
        orig(self);
        spinSpeedDic[self] = UnityEngine.Random.Range(40f, 60f);//new
        foreach (GameObject gameObject in self.beams)
        {
            gameObject.transform.SetScale(3f, 3f);
        }
        self.StartCoroutine(spin_cr(self));//new
    }

    private IEnumerator spin_cr(FlyingGenieLevelPyramid self)//new
    {
        for (; ; )
        {
            if (self.isClockwise)
            {
                self.transform.AddEulerAngles(0f, 0f, spinSpeedDic[self] * CupheadTime.Delta);
            }
            else
            {
                self.transform.AddEulerAngles(0f, 0f, -spinSpeedDic[self] * CupheadTime.Delta);
            }
            yield return null;
        }
    }

    private void PathMovement(On.FlyingGenieLevelPyramid.orig_PathMovement orig, FlyingGenieLevelPyramid self)
    {
        self.angle += self.speed * CupheadTime.Delta;
        Vector3 a;
        if (self.isClockwise)
        {
            a = new Vector3(Mathf.Sin(self.angle) * self.properties.pyramidLoopSize, 0f, 0f);
        }
        else
        {
            a = new Vector3(-Mathf.Sin(self.angle) * self.properties.pyramidLoopSize, 0f, 0f);
        }
        a *= 1.35f;//new
        Vector3 b = new Vector3(0f, Mathf.Cos(self.angle) * self.properties.pyramidLoopSize, 0f);
        self.transform.position = self.pivotPoint.position;
        self.transform.position += a + b;
    }

    Dictionary<FlyingGenieLevelPyramid, float> spinSpeedDic = new Dictionary<FlyingGenieLevelPyramid, float>();
}
