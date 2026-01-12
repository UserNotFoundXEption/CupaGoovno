using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class DragonCloudPlatform
{
    public void Init()
    {
        On.DragonLevelCloudPlatform.move_cr += move_cr;
    }

    private IEnumerator move_cr(On.DragonLevelCloudPlatform.orig_move_cr orig, DragonLevelCloudPlatform self)
    {
        if (!stopIndexDic.ContainsKey(self))
        {
            stopIndexDic[self] = 0;
        }
        Vector3 center = new Vector3(self.transform.position.x, self.transform.position.y, 0f);
        float[] stopValue = new float[] { 1000f, -300f, 150f, 300 };
        for (; ; )
        {
            MoveInSinusPattern(self, center);
            while (center.x > stopValue[stopIndexDic[self]])//new start?
            {
                MoveInSinusPattern(self, center);
                yield return null;
            }
            center += new Vector3(-CupheadTime.Delta * self.speed, 0f, 0f);//new end?
            //self.transform.AddPosition(-DragonLevel.SPEED * self.speed * CupheadTime.Delta, 0f, 0f);
            yield return null;
            if (self.properties.movingRight)
            {
                if (self.transform.position.x >= self.maxX)
                {
                    if (self.manager != null)
                    {
                        self.manager.DestroyObjectPool(self);
                    }
                    else
                    {
                        UnityEngine.Object.Destroy(self.gameObject);
                    }
                }
            }
            else if (self.transform.position.x <= self.minX)
            {
                if (self.manager != null)
                {
                    self.manager.DestroyObjectPool(self);
                }
                else
                {
                    UnityEngine.Object.Destroy(self.gameObject);
                }
            }
        }
    }

    private void MoveInSinusPattern(DragonLevelCloudPlatform self, Vector3 center)//new
    {
        if (!timeDic.ContainsKey(self))
        {
            timeDic[self] = 0f;
        }
        timeDic[self] += CupheadTime.Delta;
        float sinusMax = 100f;
        float sinX = Mathf.Sin(2 * timeDic[self]) * sinusMax;
        float sinY = Mathf.Sin(timeDic[self]) * sinusMax;
        self.transform.position = center + new Vector3(sinX, sinY, 0f);
    }

    public static void ChangeStopX(DragonLevelCloudPlatform self)//new
    {
        if (!stopIndexDic.ContainsKey(self))
        {
            stopIndexDic[self] = 0;
        }
        if (stopIndexDic[self] < 3)
        {
            stopIndexDic[self]++;
        }
    }

    public static Dictionary<DragonLevelCloudPlatform, int> stopIndexDic = new Dictionary<DragonLevelCloudPlatform, int>();
    public static Dictionary<DragonLevelCloudPlatform, float> timeDic = new Dictionary<DragonLevelCloudPlatform, float>();
}
