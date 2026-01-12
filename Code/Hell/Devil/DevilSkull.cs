using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class DevilSkull
{
    public void Init()
    {
        On.DevilLevelSkull.main_cr += main_cr;
    }

    private IEnumerator main_cr(On.DevilLevelSkull.orig_main_cr orig, DevilLevelSkull self)
    {
        yield return self.animator.WaitForAnimationToEnd(self, "Start", false, true);
        AbstractPlayerController player = PlayerManager.GetNext();
        Vector2 moveDir = (player.transform.position - self.transform.position).normalized;
        Vector2 velocity = moveDir * self.properties.initialMoveSpeed;
        float rotation = MathUtils.DirectionToAngle(player.transform.position - self.transform.position);
        float t = 0f;
        while (t < self.properties.initialMoveDuration)
        {
            t += CupheadTime.FixedDelta;
            self.transform.AddPosition(velocity.x * CupheadTime.FixedDelta, velocity.y * CupheadTime.FixedDelta, 0f);
            yield return new WaitForFixedUpdate();
        }
        while(self.transform.position != Vector3.zero)//new start
        {
            self.transform.position = Vector3.Lerp(self.transform.position, Vector3.zero, CupheadTime.FixedDelta * 100f);
            yield return new WaitForFixedUpdate();
        }//new end
        float rotationSpeed = (float)Rand.PosOrNeg() * self.properties.swirlRotationSpeed;
        t = 0f;
        Vector2 spiralOrigin = self.transform.position;
        ready = true;//new
        for (; ; )
        {
            if(t < 2f)//new
            {//new
                t += CupheadTime.FixedDelta;
            }//new
            self.lifetime = 5f;//new
            rotation += rotationSpeed * CupheadTime.FixedDelta;
            self.transform.position = spiralOrigin + MathUtils.AngleToDirection(rotation) * self.properties.swirlMoveOutwardSpeed * t;
            yield return new WaitForFixedUpdate();
        }
    }

    public static bool ready;
}
