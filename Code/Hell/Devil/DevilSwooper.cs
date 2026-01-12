using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class DevilSwooper
{
    public void Init()
    {
        On.DevilLevelSwooper.swoop_cr += swoop_cr;
    }

    private IEnumerator swoop_cr(On.DevilLevelSwooper.orig_swoop_cr orig, DevilLevelSwooper self)
    {
        /*float bestDistance = float.MaxValue;
        Vector2 bestVelocity = Vector2.zero;
        Vector3 target = PlayerManager.GetNext().center;
        Vector2 relativeTargetPos = target - self.transform.position;
        relativeTargetPos.x = Mathf.Abs(relativeTargetPos.x);
        if (target.x > self.transform.position.x)
        {
            self.animator.SetTrigger("OnTurn");
            yield return self.animator.WaitForAnimationToEnd(self, "Turn", false, true);
        }*/
        self.animator.SetBool("Spinning", true);
        self.AttackSFX();
        /*for (float num = 0f; num < 1f; num += 0.01f)
        {
            float angle = -self.properties.launchAngle.GetFloatAt(num);
            float floatAt = self.properties.launchSpeed.GetFloatAt(num);
            Vector2 vector = MathUtils.AngleToDirection(angle) * floatAt;
            float num2 = relativeTargetPos.x / vector.x;
            float num3 = vector.y * num2 + 0.5f * self.properties.gravity * num2 * num2;
            float num4 = Mathf.Abs(relativeTargetPos.y - num3);
            float num5 = vector.y + self.properties.gravity * num2;
            if (num5 >= 0f)
            {
                if (num4 < bestDistance)
                {
                    bestDistance = num4;
                    bestVelocity = vector;
                }
            }
        }
        if (target.x < self.transform.position.x)
        {
            bestVelocity.x *= -1f;
        }
        Vector2 velocity = bestVelocity;*/
        Vector2 velocity = new Vector2(0f, -200f);
        while (self.transform.position.y < (float)(Level.Current.Ceiling + 150))
        {
            velocity.y += self.properties.gravity * CupheadTime.FixedDelta;
            self.transform.AddPosition(velocity.x * CupheadTime.FixedDelta, velocity.y * CupheadTime.FixedDelta, 0f);
            yield return new WaitForFixedUpdate();
        }
        self.state = DevilLevelSwooper.State.Returning;
        float xPos = self.parent.PutSwooperInSlot(self);
        self.transform.SetPosition(new float?(xPos), null, null);
        yield return CupheadTime.WaitForSeconds(self, 0.5f);
        float moveTime = 1.5f;
        float t = 0f;
        while (t < moveTime)
        {
            self.transform.SetPosition(null, new float?(EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, (float)(Level.Current.Ceiling + 150), self.yPos, t / moveTime)), null);
            t += CupheadTime.FixedDelta;
            yield return new WaitForFixedUpdate();
        }
        self.state = DevilLevelSwooper.State.Idle;
        self.transform.SetPosition(null, new float?(self.yPos), null);
        self.animator.SetBool("Spinning", false);
        self.AttackSFXEnd();
        yield break;
    }
}
