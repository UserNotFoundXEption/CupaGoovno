using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static UnityEngine.GridBrushBase;
using UnityEngine;
using System.Collections;

namespace CupaGoovno;

public class GenieHelixProjectile
{
    public void Init()
    {
        On.FlyingGenieLevelHelixProjectile.moveY_cr += moveY_cr;
    }

    private IEnumerator moveY_cr(On.FlyingGenieLevelHelixProjectile.orig_moveY_cr orig, FlyingGenieLevelHelixProjectile self)
    {
        float angle = 0f;
        float xSpeed = self.properties.heartShotXSpeed;
        float ySpeed = self.properties.heartShotYSpeed;
        Vector3 moveX = self.transform.position;
        float t = 0f;//new start
        float mainY = self.transform.position.y;
        float mainX = self.transform.position.x;
        float sinDelta;
        float loopSize;
        if (self.topOne)
        {
            loopSize = self.properties.heartLoopYSize;
            ySpeed = self.properties.heartShotYSpeed;
            sinDelta = 75f;
            mainY -= 75f;
        }
        else
        {
            loopSize = -self.properties.heartLoopYSize;
            ySpeed = -self.properties.heartShotYSpeed;
            sinDelta = -75f;
            mainY += 75f;
        }//new end
        while (self.transform.position.x != -640f)
        {
            /*float loopSize;
            if (self.topOne)
            {
                loopSize = self.properties.heartLoopYSize;
                ySpeed = self.properties.heartShotYSpeed;
            }
            else
            {
                loopSize = -self.properties.heartLoopYSize;
                ySpeed = -self.properties.heartShotYSpeed;
            }
            angle += ySpeed * CupheadTime.Delta;
            Vector3 moveY = new Vector3(0f, Mathf.Sin(angle + self.properties.heartLoopYSize) * CupheadTime.Delta * 60f * loopSize / 2f);
            moveX = -self.transform.right * xSpeed * CupheadTime.Delta;
            self.transform.position += moveX + moveY;*/
            t += CupheadTime.Delta;//new start
            angle = t * ySpeed;
            float sin = Mathf.Sin(angle + self.properties.heartLoopYSize) * 18f * loopSize / 2f + sinDelta;
            float circleY = sin * Mathf.Cos(t * rotationSpeedMultiplierDic[self]) * rotationDirectionDic[self];
            float circleX = sin * Mathf.Sin(t * rotationSpeedMultiplierDic[self]);
            mainX -= xSpeed * CupheadTime.Delta;
            float newX = mainX + circleX;
            float newY = mainY + circleY;
            self.transform.position = new Vector3(newX, newY);//new end
            yield return null;
        }
        self.Die();
        yield return null;
        yield break;
    }

    public static void SetRotationProperties(FlyingGenieLevelHelixProjectile self, float speed, int direction)//new
    {
        SpriteRenderer spriteRenderer = self.GetComponent<SpriteRenderer>();
        if(spriteRenderer != null)
        {
            spriteRenderer.sortingLayerName = "UI";
            spriteRenderer.sortingOrder = int.MaxValue;
        }
        rotationSpeedMultiplierDic[self] = speed;
        rotationDirectionDic[self] = direction;
    }

    public static Dictionary<FlyingGenieLevelHelixProjectile, float> rotationSpeedMultiplierDic = new Dictionary<FlyingGenieLevelHelixProjectile, float>();
    public static Dictionary<FlyingGenieLevelHelixProjectile, int> rotationDirectionDic = new Dictionary<FlyingGenieLevelHelixProjectile, int>();
}
