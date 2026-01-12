using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class MermaidHead
{
    public void Init()
    {
        On.FlyingMermaidLevelMerdusaHead.StartBubble += StartBubble;
    }

    public void StartBubble(On.FlyingMermaidLevelMerdusaHead.orig_StartBubble orig, FlyingMermaidLevelMerdusaHead self)
    {
        if (self.patternCoroutine != null)
        {
            self.StopCoroutine(self.patternCoroutine);
        }
        self.patternCoroutine = self.StartCoroutine(bubble_cr(self));
    }

    private IEnumerator bubble_cr(FlyingMermaidLevelMerdusaHead self)
    {
        self.state = FlyingMermaidLevelMerdusaHead.State.Bubble;
        int counter = 0;//new start
        for(; ; )
        {
            if(counter == 7)
            {
                counter = 0;
                nextBubbleBig = true;
            }
            else
            {
                nextBubbleBig = false;
                counter++;
            }//new end
            //self.animator.SetTrigger("OnSnakeATK");
            //yield return self.animator.WaitForAnimationToEnd(self, "Snake_Attack", false, true);
            FlyingMermaidLevelSkullBubble prefab = self.bubblePrefab;
            float xDelta = UnityEngine.Random.Range(-200f, 200f);
            float yDelta = UnityEngine.Random.Range(-100f, 100f);
            Vector2 pos = self.transform.position + new Vector3(xDelta, yDelta);
            float velocity = -self.properties.CurrentState.bubbles.movementSpeed;
            float sinVelocity = self.properties.CurrentState.bubbles.waveSpeed;
            float sinSize = self.properties.CurrentState.bubbles.waveAmount;
            MermaidSkullBubble.CreateBubble(prefab, pos, velocity, sinVelocity, sinSize, 0f);
            yield return CupheadTime.WaitForSeconds(self, self.properties.CurrentState.bubbles.attackDelayRange.RandomFloat());
            if (nextBubbleBig)
            {
                yield return CupheadTime.WaitForSeconds(self, 3f);
            }
        }
        //self.state = FlyingMermaidLevelMerdusaHead.State.Idle;
        //yield return null;
        //yield break;
    }

    public static bool nextBubbleBig = false;
}
