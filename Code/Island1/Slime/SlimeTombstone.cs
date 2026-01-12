using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class SlimeTombstone
{
    public void Init()
    {
        On.SlimeLevelTombstone.move_cr += move_cr;
    }

    private IEnumerator move_cr(On.SlimeLevelTombstone.orig_move_cr orig, SlimeLevelTombstone self)
    {
        YieldInstruction wait = new WaitForFixedUpdate();
        self.direction = ((!MathUtils.RandomBool()) ? SlimeLevelTombstone.Direction.Right : SlimeLevelTombstone.Direction.Left);
        string[] offsets = self.properties.CurrentState.tombstone.attackOffsetString.Split(new char[]
        {
        ','
        });
        /*self.offsetIndex = (self.offsetIndex + 1) % offsets.Length;
        float offset = 0f;
        Parser.FloatTryParse(offsets[self.offsetIndex], out offset);*/
        float offset1 = UnityEngine.Random.Range(0f, 2.5f);//new start
        float multiplier = Rand.Bool() ? 1 : -1;
        float offset = Mathf.Log(offset1) / Mathf.Log(2) * multiplier * 100;//new end
        bool justStarted = true;
        while (!self.wantsToSmash)
        {
            self.animator.SetTrigger((self.direction != SlimeLevelTombstone.Direction.Right) ? "MoveLeft" : "MoveRight");
            yield return self.animator.WaitForAnimationToStart(self, (self.direction != SlimeLevelTombstone.Direction.Right) ? "Move_Left" : "Move_Right", false);
            self.animator.Play("Dirt");
            if (justStarted)
            {
                self.animator.Play("Dust_Start");
            }
            else
            {
                self.animator.Play("Dust_Start_End");
            }
            AudioManager.Play("slime_tombstone_slide");
            self.emitAudioFromObject.Add("slime_tombstone_slide");
            float startX = self.transform.position.x;
            float endX = (self.direction != SlimeLevelTombstone.Direction.Right) ? -500f : 500f;
            float moveTime = Mathf.Abs(startX - endX) / self.properties.CurrentState.tombstone.moveSpeed;
            yield return self.TweenPositionX(startX, endX, moveTime, EaseUtils.EaseType.easeInOutSine);
            self.direction = ((self.direction != SlimeLevelTombstone.Direction.Right) ? SlimeLevelTombstone.Direction.Right : SlimeLevelTombstone.Direction.Left);
            justStarted = false;
        }
        self.animator.SetTrigger((self.direction != SlimeLevelTombstone.Direction.Right) ? "MoveLeft" : "MoveRight");
        yield return self.animator.WaitForAnimationToStart(self, (self.direction != SlimeLevelTombstone.Direction.Right) ? "Move_Left" : "Move_Right", false);
        self.animator.Play("Dust_Start_End");
        AudioManager.Play("slime_tombstone_slide");
        self.emitAudioFromObject.Add("slime_tombstone_slide");
        AbstractPlayerController player = PlayerManager.GetNext();
        float startX2 = self.transform.position.x;
        float endX2 = (self.direction != SlimeLevelTombstone.Direction.Right) ? -500f : 500f;
        float moveTime2 = Mathf.Abs(startX2 - endX2) / self.properties.CurrentState.tombstone.moveSpeed;
        float targetX = 0f;
        float t = 0f;
        bool centeredOnPlayer = false;
        while (!centeredOnPlayer && t < moveTime2)
        {
            yield return wait;
            t += CupheadTime.FixedDelta * self.hitPauseCoefficient();
            self.transform.SetPosition(new float?(EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, startX2, endX2, t / moveTime2)), null, null);
            if (player == null || player.IsDead)
            {
                player = PlayerManager.GetNext();
            }
            targetX = player.center.x + offset;
            if ((self.direction == SlimeLevelTombstone.Direction.Right && self.transform.position.x > targetX) || (self.direction == SlimeLevelTombstone.Direction.Left && self.transform.position.x < targetX))
            {
                centeredOnPlayer = true;
            }
        }
        self.transform.SetPosition(new float?(Mathf.Clamp(targetX, -500f, 500f)), null, null);
        self.animator.Play("Dust_End");
        self.animator.Play("Dirt_Off");
        self.StartSmash();
        yield break;
    }
}
