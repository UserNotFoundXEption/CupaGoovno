using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class FrogShort
{
    public void Init()
    {
        On.FrogsLevelShort.StartRage += StartRage;
        On.FrogsLevelShort.rage_cr += rage_cr;
        On.FrogsLevelShort.roll_cr += roll_cr;
        On.FrogsLevelShort.morphRoll_cr += morphRoll_cr;
        On.FrogsLevelShort.clap_cr += clap_cr;
    }

    public void StartRage(On.FrogsLevelShort.orig_StartRage orig, FrogsLevelShort self)
    {
        if ((self.state != FrogsLevelShort.State.Idle && self.state != FrogsLevelShort.State.Complete) || self.properties.CurrentHealth / self.properties.TotalHealth <= 0.74f)//new
         //if (self.state != FrogsLevelShort.State.Idle && self.state != FrogsLevelShort.State.Complete)
        {
            return;
        }
        self.state = FrogsLevelShort.State.Rage;
        self.StartCoroutine(self.rage_cr());
    }

    private IEnumerator rage_cr(On.FrogsLevelShort.orig_rage_cr orig, FrogsLevelShort self)
    {
        LevelProperties.Frogs.ShortRage p = self.properties.CurrentState.shortRage;
        self.animator.SetTrigger("OnRage");
        yield return self.animator.WaitForAnimationToEnd(self, "Rage", false, true);
        yield return CupheadTime.WaitForSeconds(self, p.anticipationDelay);
        self.animator.SetTrigger("OnRageAttack");
        yield return self.animator.WaitForAnimationToEnd(self, "Rage_Anticipate_End", false, true);
        AudioManager.PlayLoop("level_frogs_short_ragefist_attack_loop");
        self.emitAudioFromObject.Add("level_frogs_short_ragefist_attack_loop");
        int shotCount = p.shotCount;
        int root = 0;
        string parryString = p.parryPatterns[UnityEngine.Random.Range(0, p.parryPatterns.Length)].ToLower();
        int parryIndex = 0;
        while (shotCount > 0)
        {
            if (self.properties.CurrentHealth / self.properties.TotalHealth <= 0.74f)//new start
            {
                break;
            }//new end
            yield return CupheadTime.WaitForSeconds(self, p.shotDelay);
            shotCount--;
            self.Shoot(p, self.rageRoots[root].position, parryString[parryIndex] == 'p');
            root = (int)Mathf.Repeat((float)(root + 1), (float)self.rageRoots.Length);
            parryIndex = (int)Mathf.Repeat((float)(parryIndex + 1), (float)parryString.Length);

        }
        yield return CupheadTime.WaitForSeconds(self, p.shotDelay);
        self.animator.SetTrigger("OnRageEnd");
        AudioManager.Stop("level_frogs_short_ragefist_attack_loop");
        yield return CupheadTime.WaitForSeconds(self, p.hesitate);
        self.state = FrogsLevelShort.State.Complete;
        yield break;
    }

    private IEnumerator roll_cr(On.FrogsLevelShort.orig_roll_cr orig, FrogsLevelShort self)
    {
        yield return null;
        float startX = self.transform.position.x;
        float endX = -(startX + 240f);
        LevelProperties.Frogs.ShortRoll p = self.properties.CurrentState.shortRoll;
        self.animator.SetTrigger("OnRoll");
        yield return CupheadTime.WaitForSeconds(self, 1.2f + p.delay);
        self.animator.SetTrigger("OnRollContinue");
        yield return CupheadTime.WaitForSeconds(self, 1f);
        //        CupheadLevelCamera.Current.StartShake(4f);
        yield return self.animator.WaitForAnimationToStart(self, "Roll_Loop", false);
        CupheadLevelCamera.Current.StartShake(4f);//new
        float t = 0f;
        while (t < p.time)
        {
            float val = t / p.time;
            float x = EaseUtils.Ease(EaseUtils.EaseType.easeInSine, startX, endX, val);
            self.transform.SetPosition(new float?(x), null, null);
            t += CupheadTime.Delta;
            yield return null;
        }
        self.transform.SetPosition(new float?(endX), null, null);
        yield return null;
        CupheadLevelCamera.Current.EndShake(0.5f);
        AudioManager.Stop("level_frogs_short_rolling_loop");
        AudioManager.Play("level_frogs_short_rolling_crash");
        self.emitAudioFromObject.Add("level_frogs_short_rolling_crash");
        self.spriteRenderer.enabled = false;
        self.direction = FrogsLevelShort.Direction.Right;
        yield return CupheadTime.WaitForSeconds(self, p.returnDelay);
        self.transform.SetScale(new float?(-1f), null, null);
        self.transform.SetPosition(new float?(-(startX + 140f)), null, null);
        self.animator.SetTrigger("OnRollContinue");
        AudioManager.Play("level_frogs_short_rolling_end");
        self.emitAudioFromObject.Add("level_frogs_short_rolling_end");
        self.spriteRenderer.enabled = true;
        yield return CupheadTime.WaitForSeconds(self, 1f + p.hesitate);
        self.state = FrogsLevelShort.State.Complete;
        AudioManager.Stop("level_frogs_short_rolling_loop");
        yield break;
    }

    public IEnumerator morphRoll_cr(On.FrogsLevelShort.orig_morphRoll_cr orig, FrogsLevelShort self)
    {
        FrogsLevelTall.Current.transform.SetPosition(625f);
        FrogsLevelTall.Current.transform.SetScale(1f);
        yield return orig(self);
    }

    private IEnumerator clap_cr(On.FrogsLevelShort.orig_clap_cr orig, FrogsLevelShort self)
    {
        self.clapProperties = self.properties.CurrentState.shortClap;
        self.clapDirection = ((!Rand.Bool()) ? FrogsLevelShortClapBullet.Direction.Up : FrogsLevelShortClapBullet.Direction.Down);
        self.clapRoot.SetEulerAngles(new float?(0f), new float?(0f), new float?(self.clapProperties.angles.GetRandom<float>()));
        string patternString = self.clapProperties.patterns[UnityEngine.Random.Range(0, self.clapProperties.patterns.Length)];
        KeyValue[] pattern = KeyValue.ListFromString(patternString, new char[]
        {
            'S',
            'D'
        });
        self.animator.SetTrigger("OnClap");
        self.animator.SetBool("Clapping", true);
        yield return CupheadTime.WaitForSeconds(self, 1f + self.clapProperties.shotDelay);
        for (int i = 0; i < pattern.Length; i++)
        {
            if (self.properties.CurrentHealth / self.properties.TotalHealth <= 0.35f)//new start
            {
                self.animator.Play("Clap_End");
                yield return self.animator.WaitForAnimationToEnd(self, "Clap_End", false, true);
                break;
            }//new end

            if (pattern[i].key == "S")
            {
                int ii = 0;
                while ((float)ii < pattern[i].value)
                {
                    self.clapDirection = ((self.clapDirection != FrogsLevelShortClapBullet.Direction.Down) ? FrogsLevelShortClapBullet.Direction.Down : FrogsLevelShortClapBullet.Direction.Up);
                    if (i >= pattern.Length - 1 && (float)ii >= pattern[i].value - 1f)
                    {
                        self.animator.Play("Clap_End");
                        yield return self.animator.WaitForAnimationToEnd(self, "Clap_End", false, true);
                    }
                    else
                    {
                        self.animator.Play("Clap_Shoot");
                    }
                    yield return CupheadTime.WaitForSeconds(self, 0.5f);
                    ii++;
                }
            }
            else
            {
                yield return CupheadTime.WaitForSeconds(self, pattern[i].value);
            }
        }
        self.animator.Play("Idle");
        self.animator.ResetTrigger("OnClap");
        self.animator.SetBool("Clapping", false);
        yield return CupheadTime.WaitForSeconds(self, self.clapProperties.hesitate);
        self.state = FrogsLevelShort.State.Complete;
        yield break;
    }
}
