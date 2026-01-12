using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;

namespace CupaGoovno;

public class Mermaid
{
    public void Init()
    {
        new MermaidYellProjectile().Init();
        new MermaidHead().Init();
        new MermaidEel().Init();
        new MermaidMerdusa().Init();
        On.FlyingMermaidLevelMermaid.Awake += Awake;
        On.FlyingMermaidLevelMermaid.fish_cr += fish_cr;
        On.FlyingMermaidLevelMermaid.summon_cr += summon_cr;
        On.FlyingMermaidLevelMermaid.fishSpinner += fishSpinner;
        On.FlyingMermaidLevelMermaid.LaunchFish += LaunchFish;
    }

    protected void Awake(On.FlyingMermaidLevelMermaid.orig_Awake orig, FlyingMermaidLevelMermaid self)
    {
        orig(self);
        self.transform.AddPosition(150f, 0f, 0f);
        self.walkingPositions[0].AddPosition(150f, 0f, 0f);
        self.walkingPositions[1].AddPosition(150f, 0f, 0f);
        self.fishPattern =
        [
            FlyingMermaidLevelMermaid.FishPossibility.Spinner,
            FlyingMermaidLevelMermaid.FishPossibility.Spreadshot,
            FlyingMermaidLevelMermaid.FishPossibility.Homer
        ];
        self.fishPattern.Shuffle();
    }

    private IEnumerator fish_cr(On.FlyingMermaidLevelMermaid.orig_fish_cr orig, FlyingMermaidLevelMermaid self)
    {
        self.state = FlyingMermaidLevelMermaid.State.Fish;
        //self.animator.SetTrigger("StartFish");
        //yield return self.animator.WaitForAnimationToEnd(self, "Tuckdown_Start", false, true);
        self.animator.Play("Tuckdown_Start");//new
        float t = 0f;
        FlyingMermaidLevelSplashManager.Instance.SpawnMegaSplashLarge(self.gameObject, 0f, false, 0f);
        while (t < self.tuckdownMoveTime)
        {
            t += CupheadTime.Delta;
            self.transform.SetPosition(null, new float?(Mathf.Lerp(self.regularY, self.fishUnderwaterY, t / self.tuckdownMoveTime)), null);
            yield return null;
        }
        yield return CupheadTime.WaitForSeconds(self, self.tuckdownWaitTime);
        self.fish = self.nextFish();
        self.spreadshotFishSprite.enabled = (self.fish == FlyingMermaidLevelMermaid.FishPossibility.Spreadshot);
        self.spreadshotFishOverlaySprite.enabled = (self.fish == FlyingMermaidLevelMermaid.FishPossibility.Spreadshot);
        self.spinnerFishSprite.enabled = (self.fish == FlyingMermaidLevelMermaid.FishPossibility.Spinner);
        self.spinnerFishOverlaySprite.enabled = (self.fish == FlyingMermaidLevelMermaid.FishPossibility.Spinner);
        self.homerFishSprite.enabled = (self.fish == FlyingMermaidLevelMermaid.FishPossibility.Homer);
        self.homerFishOverlaySprite.enabled = (self.fish == FlyingMermaidLevelMermaid.FishPossibility.Homer);
        self.animator.SetTrigger("Continue");
        yield return self.animator.WaitForAnimationToEnd(self, "Tuckdown_Loop", false, true);
        t = 0f;
        FlyingMermaidLevelSplashManager.Instance.SpawnMegaSplashLarge(self.gameObject, 50f, true, 0f);
        while (t < self.tuckdownRiseTime)
        {
            t += CupheadTime.Delta;
            self.transform.SetPosition(null, new float?(Mathf.Lerp(self.fishUnderwaterY, self.regularY, t / self.tuckdownRiseTime)), null);
            yield return null;
        }
        self.animator.SetBool("Repeat", true);
        string[] pattern = self.nextFishPatternString().Split(new char[]
        {
        ','
        });
        float waitTime = self.properties.CurrentState.fish.delayBeforeFirstAttack;
        for (int i = 0; i < pattern.Length; i++)
        {
            if (pattern[i][0] == 'D')
            {
                Parser.FloatTryParse(pattern[i].Substring(1), out waitTime);
            }
            else
            {
                yield return CupheadTime.WaitForSeconds(self, waitTime);
                self.animator.SetTrigger("Continue");
                yield return self.animator.WaitForAnimationToEnd(self, "Fish_Attack_Start", false, true);
                self.doFishAttack(pattern[i]);
                if (i < pattern.Length - 1)
                {
                    yield return self.animator.WaitForAnimationToEnd(self, "Fish_Attack_Repeat", false, true);
                    waitTime = self.waitTimeBetweenFishAttacks();
                }
            }
        }
        self.animator.SetBool("Repeat", false);
        //yield return self.animator.WaitForAnimationToEnd(self, "Fish_Attack", false, true);
        //yield return CupheadTime.WaitForSeconds(self, self.properties.CurrentState.fish.delayBeforeFly);
        //self.animator.SetTrigger("Continue");
        //yield return self.animator.WaitForAnimationToEnd(self, "Fish_Launch", false, true);
        self.animator.Play("Fish_Launch");//new
        yield return CupheadTime.WaitForSeconds(self, self.properties.CurrentState.fish.hesitateAfterAttack);
        self.state = FlyingMermaidLevelMermaid.State.Idle;
        yield break;
    }

    private IEnumerator summon_cr(On.FlyingMermaidLevelMermaid.orig_summon_cr orig, FlyingMermaidLevelMermaid self)
    {
        LevelProperties.FlyingMermaid.Summon p = self.properties.CurrentState.summon;
        //self.animator.SetBool("Summon", true);
        //yield return self.animator.WaitForAnimationToEnd(self, "Summon_Start", false, true);
        //AudioManager.Play("level_mermaid_summon_loop_start");
        //yield return CupheadTime.WaitForSeconds(self, p.holdBeforeCreature);
        FlyingMermaidLevelMermaid.SummonPossibility summon = self.nextSummon();
        //AudioManager.Play("level_mermaid_summon_loop");
        if (summon != FlyingMermaidLevelMermaid.SummonPossibility.Seahorse)
        {
            if (summon != FlyingMermaidLevelMermaid.SummonPossibility.Pufferfish)
            {
                if (summon == FlyingMermaidLevelMermaid.SummonPossibility.Turtle)
                {
                    self.SummonTurtle();
                }
            }
            else
            {
                AudioManager.Play("level_mermaid_merdusa_puffer_fish_bubble_up");
                self.StartCoroutine(self.summonPufferFish_cr());
            }
        }
        else
        {
            self.SummonSeahorse();
        }
        /* yield return CupheadTime.WaitForSeconds(self, p.holdAfterCreature);
         AudioManager.Stop("level_mermaid_summon_loop");
         AudioManager.Play("level_mermaid_summon_loop_end");
         self.animator.SetBool("Summon", false);
         yield return self.animator.WaitForAnimationToEnd(self, "Summon_End", false, true);
         yield return CupheadTime.WaitForSeconds(self, p.hesitateAfterAttack);*/
         self.state = FlyingMermaidLevelMermaid.State.Idle;
        yield break;
    }

    private void fishSpinner(On.FlyingMermaidLevelMermaid.orig_fishSpinner orig, FlyingMermaidLevelMermaid self)
    {
        Vector2 pos = self.fishProjectileRoot.position;
        float speed = self.properties.CurrentState.spinnerFish.bulletSpeed;
        BasicProjectile projectile = self.fishSpreadshotBulletPrefab.Create(pos, -180f, speed);
        self.StartCoroutine(MermaidFishSpreadProjectile.RejectSpreadshotEmbraceSpinner(projectile));
    }

    public void LaunchFish(On.FlyingMermaidLevelMermaid.orig_LaunchFish orig, FlyingMermaidLevelMermaid self)
    {
        //FlyingMermaidLevelFish flyingMermaidLevelFish = null;
        FlyingMermaidLevelMermaid.FishPossibility fishPossibility = self.fish;
        if (fishPossibility != FlyingMermaidLevelMermaid.FishPossibility.Spreadshot)
        {
            if (fishPossibility != FlyingMermaidLevelMermaid.FishPossibility.Spinner)
            {
                if (fishPossibility == FlyingMermaidLevelMermaid.FishPossibility.Homer)
                {
                    //flyingMermaidLevelFish = self.homerFishPrefab;
                    self.homerFishPrefab.Create(self.fishLaunchRoot.position, self.properties.CurrentState.fish);//new
                }
            }
            else
            {
                //flyingMermaidLevelFish = self.spinnerFishPrefab;
                FlyingMermaidLevelFish fish = self.spreadshotFishPrefab.Create(self.fishLaunchRoot.position, self.properties.CurrentState.fish);//new start
                SpriteRenderer renderer = fish.GetComponent<SpriteRenderer>();
                if(renderer != null)
                {
                    renderer.color = Color.cyan;
                }//new end
            }
        }
        else
        {
            //flyingMermaidLevelFish = self.spreadshotFishPrefab;
            self.spreadshotFishPrefab.Create(self.fishLaunchRoot.position, self.properties.CurrentState.fish);//new
        }
        //flyingMermaidLevelFish.Create(self.fishLaunchRoot.position, self.properties.CurrentState.fish);
    }
}
