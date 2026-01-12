using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class BaronessJellyBeans
{
    public void Init()
    {
        On.BaronessLevelJellybeans.Start += Start;
        On.BaronessLevelJellybeans.move_cr += move_cr;
        On.BaronessLevelJellybeans.jump_cr += jump_cr;
    }

    protected void Start(On.BaronessLevelJellybeans.orig_Start orig, BaronessLevelJellybeans self)
    {
        AbstractProj.Start(self);
        self.GetComponent<Collider2D>().enabled = true;
        self.GetComponent<SpriteRenderer>().enabled = true;
        self.damageReceiver = self.GetComponent<DamageReceiver>();
        self.damageReceiver.OnDamageTaken += self.OnDamageTaken;
        self.state = BaronessLevelJellybeans.State.Run;
        //AudioManager.Play("level_baroness_jellybean_spawn");
        //self.emitAudioFromObject.Add("level_baroness_jellybean_spawn");
        self.StartCoroutine(self.fade_color_cr());
        self.StartCoroutine(self.beginning_offset_cr());
        self.StartCoroutine(self.move_cr());
    }

    private IEnumerator move_cr(On.BaronessLevelJellybeans.orig_move_cr orig, BaronessLevelJellybeans self)
    {
        YieldInstruction wait = new WaitForFixedUpdate();
        self.state = BaronessLevelJellybeans.State.Run;
        float offset = 200f;
        int i = 1;//new
        while (self.transform.position.x > -640f - offset)
        {
            if (self.state != BaronessLevelJellybeans.State.Jump)
            {
                if (200 * i > self.transform.position.x)//new start
                {
                    i--;
                    if (BaronessLevelCastle.CURRENT_MINI_BOSS.bossId != BaronessLevelCastle.BossPossibility.Waffle)
                    {
                        self.StartJump();
                    }
                }//new end
                Vector3 pos = self.transform.position;
                pos.x += -self.speed * CupheadTime.FixedDelta * self.hitPauseCoefficient();
                self.transform.position = pos;
            }
            yield return wait;
        }
        self.Die();
        yield break;
    }

    private IEnumerator jump_cr(On.BaronessLevelJellybeans.orig_jump_cr orig, BaronessLevelJellybeans self)
    {
        YieldInstruction wait = new WaitForFixedUpdate();
        float currentY;//new start
        float maxY = self.properties.heightDefault + self.properties.jumpHeight.RandomFloat();
        float minY = self.properties.heightDefault;
        self.velocity = self.properties.jumpSpeed;//new end
        //self.velocity = self.properties.jumpSpeed;
        //float decrement = 1f;
        Vector3 pos = self.transform.position;
        bool jumping = true;
        bool landing = false;
        self.animator.Play("Jellybean_Jump_Antic");
        yield return self.animator.WaitForAnimationToEnd(self, "Jellybean_Jump_Antic", false, true);
        while (jumping)
        {
            currentY = self.transform.position.y;//new start
            self.velocity = self.properties.jumpSpeed * (1 - (currentY - minY) / (maxY - minY)) + 10;
            if (landing)
            {
                self.velocity = -self.velocity;
            }//new end
            self.transform.AddPosition(0f, self.velocity * CupheadTime.FixedDelta * self.hitPauseCoefficient(), 0f);
            if (currentY >= maxY)//new start
            {
                landing = true;
            }
            if (landing && currentY < minY)
            {
                self.animator.SetTrigger("Land");
                jumping = false;
            }//new end

            /*
            if (self.transform.position.y >= self.properties.heightDefault + self.properties.jumpHeight.RandomFloat())
            {
                self.velocity -= decrement;
                if (!landing)
                {
                    self.velocity = -self.velocity;
                    self.animator.SetTrigger("Land");
                    landing = true;
                }
            }
            if (self.transform.position.y <= self.originalPos.y)
            {
                if (self.animator.GetCurrentAnimatorStateInfo(0).IsName("Jellybean_Jump_Land"))
                {
                    yield return self.animator.WaitForAnimationToEnd(self, "Jellybean_Jump_Land", false, true);
                }
                jumping = false;
            }*/
            yield return wait;
        }
        self.StartCoroutine(self.timer_cr());
        pos.y = self.originalPos.y;
        self.transform.position = pos;
        self.state = BaronessLevelJellybeans.State.Run;
        yield return null;
        yield break;
    }
}
