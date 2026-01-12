using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class FrogTall
{
    public void Init()
    {
        On.FrogsLevelTall.fan_cr += fan_cr;
        On.FrogsLevelTall.fanAccelerate_cr += fanAccelerate_cr;
        On.FrogsLevelTall.fanDecelerate_cr += fanDecelerate_cr;
        On.FrogsLevelTall.StartFireflies += StartFireflies;
        On.FrogsLevelTall.ShootFirefly += ShootFirefly;
        On.FrogsLevelTall.fireflies_cr += fireflies_cr;
    }

    private IEnumerator fan_cr(On.FrogsLevelTall.orig_fan_cr orig, FrogsLevelTall self)
    {
        bool changeDirection = Rand.Bool();//new start
        if (changeDirection)
        {
            pushDirection = (int)-self.transform.localScale.x;
        }
        else
        {
            pushDirection = Rand.Bool() ? 1 : -1;
        }//new end
        LevelProperties.Frogs.TallFan p = self.properties.CurrentState.tallFan;
        float time = p.duration;
        self.animator.Play("Fan");
        yield return CupheadTime.WaitForSeconds(self, 0.5f);
        AudioManager.Play("level_frogs_tall_fan_start");
        self.emitAudioFromObject.Add("level_frogs_tall_fan_start");
        self.StartCoroutine(self.fanAccelerate_cr(p));//direction
        //yield return CupheadTime.WaitForSeconds(self, 2f);
        AudioManager.PlayLoop("level_frogs_tall_fan_attack_loop");
        self.emitAudioFromObject.Add("level_frogs_tall_fan_attack_loop");
        /*if (self.firstFan)
        {
            self.firstFan = false;
            float startX = self.transform.position.x;
            yield return CupheadTime.WaitForSeconds(self, 0.25f);
            float t = 0f;
            while (t < 0.5f)
            {
                float val = t / 0.5f;
                float x = EaseUtils.Ease(EaseUtils.EaseType.easeInSine, startX, startX + 60f, val);
                self.transform.SetPosition(new float?(x), null, null);
                t += CupheadTime.Delta;
                yield return null;
            }
            self.transform.SetPosition(new float?(startX + 60f), null, null);
            yield return CupheadTime.WaitForSeconds(self, p.duration.RandomFloat() - 0.75f);
        }
        else
        {*/
        yield return CupheadTime.WaitForSeconds(self, p.duration.RandomFloat());
        //}
        //yield return CupheadTime.WaitForSeconds(self, time);
        AudioManager.Play("level_frogs_tall_fan_end");
        self.emitAudioFromObject.Add("level_frogs_tall_fan_end");
        yield return CupheadTime.WaitForSeconds(self, 0.2f);
        AudioManager.Stop("level_frogs_tall_fan_attack_loop");
        self.animator.SetTrigger("OnFanEnd");
        self.StartCoroutine(self.fanDecelerate_cr(p));//direction
        yield return CupheadTime.WaitForSeconds(self, 0.5f);
        self.state = FrogsLevelTall.State.Complete;
        yield break;
    }

    private IEnumerator fanAccelerate_cr(On.FrogsLevelTall.orig_fanAccelerate_cr orig, FrogsLevelTall self, LevelProperties.Frogs.TallFan p)
    {
        self.fanForce.enabled = true;
        self.transform.SetScale(pushDirection, null, null);//new
        self.transform.position = new Vector3(525 + pushDirection * 100f, self.transform.position.y);
        yield return self.StartCoroutine(self.fanPowerTween_cr(0f, p.power * pushDirection, (float)p.accelerationTime));
        yield break;
    }

    private IEnumerator fanDecelerate_cr(On.FrogsLevelTall.orig_fanDecelerate_cr orig, FrogsLevelTall self, LevelProperties.Frogs.TallFan p)
    {
        yield return self.StartCoroutine(self.fanPowerTween_cr(p.power * pushDirection, 0f, 0.75f));
        self.transform.SetScale(pushDirection, null, null);//new
        self.transform.position = new Vector3(625, self.transform.position.y);
        self.fanForce.enabled = false;
        yield break;
    }

    public void StartFireflies(On.FrogsLevelTall.orig_StartFireflies orig, FrogsLevelTall self)
    {
        self.layer = 0;
        if ((self.state != FrogsLevelTall.State.Idle && self.state != FrogsLevelTall.State.Complete) || self.properties.CurrentHealth / self.properties.TotalHealth <= 0.74f)//new
        //if (self.state != FrogsLevelTall.State.Idle && self.state != FrogsLevelTall.State.Complete)
        {
            self.state = FrogsLevelTall.State.Complete;
            self.animator.Play("Idle");
            return;
        }
        self.state = FrogsLevelTall.State.Fireflies;
        self.fireflyCount = 0;
        self.animator.SetBool("EndFirefly", false);
        self.StartCoroutine(self.fireflies_cr());
    }

    private void ShootFirefly(On.FrogsLevelTall.orig_ShootFirefly orig, FrogsLevelTall self)
    {
        AudioManager.Play("level_frogs_tall_spit_shoot");
        self.emitAudioFromObject.Add("level_frogs_tall_spit_shoot");
        FrogsLevelTallFireflyRoot frogsLevelTallFireflyRoot = self.tempRoots[UnityEngine.Random.Range(0, self.tempRoots.Count)];
        self.tempRoots.Remove(frogsLevelTallFireflyRoot);
        Vector2 vector = frogsLevelTallFireflyRoot.transform.position;
        Vector2 a = vector;
        Vector2 vector2 = new Vector2(UnityEngine.Random.value * (float)((!Rand.Bool()) ? -1 : 1), UnityEngine.Random.value * (float)((!Rand.Bool()) ? -1 : 1));
        //		vector = a + vector2.normalized * frogsLevelTallFireflyRoot.radius * UnityEngine.Random.value;
        vector = a + vector2.normalized * frogsLevelTallFireflyRoot.radius * UnityEngine.Random.value / 100 + new Vector2(300, 0);//new
        self.fireflyPrefab.Create(self.spitRoot.position, vector, self.fireflyProperties.speed, self.fireflyProperties.hp, self.fireflyProperties.followDelay, self.fireflyProperties.followTime, self.fireflyProperties.followDistance, self.fireflyProperties.invincibleDuration, PlayerManager.GetNext(), self.layer++);
        self.fireflyCount--;
    }

    private IEnumerator fireflies_cr(On.FrogsLevelTall.orig_fireflies_cr orig, FrogsLevelTall self)
    {
        self.fireflyProperties = self.properties.CurrentState.tallFireflies;
        string patternString = self.fireflyProperties.patterns[UnityEngine.Random.Range(0, self.fireflyProperties.patterns.Length)];
        KeyValue[] pattern = KeyValue.ListFromString(patternString, new char[]
        {
        'S',
        'D'
        });
        self.animator.SetTrigger("OnFirefly");
        yield return CupheadTime.WaitForSeconds(self, 2f);
        for (int i = 0; i < pattern.Length; i++)
        {
            if (self.properties.CurrentHealth / self.properties.TotalHealth <= 0.74f)//new start
            {
                self.animator.Play("Idle");
                self.animator.SetInteger("FireflyCount", 0);
                self.state = FrogsLevelTall.State.Complete;
                break;
            }//new end
            else
            {
                if (pattern[i].key == "S")
                {
                    self.ResetFireflyRoots();
                    self.fireflyCount = (int)pattern[i].value;
                    self.animator.SetInteger("FireflyCount", self.fireflyCount);
                    self.animator.SetTrigger("OnFireflyStart");
                    self.animator.SetBool("EndFirefly", i >= pattern.Length - 1);
                    while (self.fireflyCount > 0)
                    {
                        self.animator.SetInteger("FireflyCount", self.fireflyCount);
                        yield return null;
                    }
                    self.animator.SetInteger("FireflyCount", self.fireflyCount);
                }
                else
                {
                    yield return CupheadTime.WaitForSeconds(self, pattern[i].value);
                }
            }
        }
        yield return CupheadTime.WaitForSeconds(self, self.fireflyProperties.hesitate);
        self.state = FrogsLevelTall.State.Complete;
        yield break;
    }

    int pushDirection;
}
