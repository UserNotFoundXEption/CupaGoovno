using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class CigarSpit
{
    public static void InitProjectileVertical(DicePalaceCigarLevelCigarSpit self, LevelProperties.DicePalaceCigar properties, bool clockwise, bool onRight)
    {
        self.time = 0f;
        self.centerPoint = self.transform.position;
        self.onRight = onRight;
        if (!clockwise)
        {
            self.circleSpeed = -properties.CurrentState.spiralSmoke.circleSpeed;
        }
        else
        {
            self.circleSpeed = properties.CurrentState.spiralSmoke.circleSpeed;
        }
        self.properties = properties;
        //self.StartCoroutine(self.move_cr());
        self.StartCoroutine(moveVertical_cr(self));//new
        self.StartCoroutine(self.bullet_trail_cr());
    }

    private static IEnumerator moveVertical_cr(DicePalaceCigarLevelCigarSpit self)
    {
        YieldInstruction wait = new WaitForFixedUpdate();
        for (; ; )
        {
            //self.centerPoint += -self.transform.right * self.properties.CurrentState.spiralSmoke.horizontalSpeed * CupheadTime.FixedDelta;
            self.centerPoint += -self.transform.up * self.properties.CurrentState.spiralSmoke.horizontalSpeed * CupheadTime.FixedDelta;//new
            Vector3 newPos = self.centerPoint;
            newPos.y = self.centerPoint.y + Mathf.Sin(self.time * self.circleSpeed) * self.properties.CurrentState.spiralSmoke.spiralSmokeCircleSize;
            if (self.onRight)
            {
                newPos.x = self.centerPoint.x + Mathf.Cos(self.time * self.circleSpeed) * self.properties.CurrentState.spiralSmoke.spiralSmokeCircleSize;
            }
            else
            {
                newPos.x = self.centerPoint.x + -Mathf.Cos(self.time * self.circleSpeed) * self.properties.CurrentState.spiralSmoke.spiralSmokeCircleSize;
            }
            self.transform.position = newPos;
            self.time += CupheadTime.FixedDelta;
            yield return wait;
        }
    }
}
