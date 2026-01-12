using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CupaGoovno;

public class Bee
{
    public void Init()
    {
        new BeeSecurity().Init();
        new BeeSpitProjectile().Init();
        new BeePlatforms().Init();
        On.BeeLevel.Start += Start;
        On.BeeLevel.follower_cr += follower_cr;
        On.BeeLevelQueen.OnDestroy += OnDestroy;
        On.BeeLevelQueen.StartFollower += StartFollower;
        On.BeeLevelQueen.triangle_cr += triangle_cr;
        On.BeeLevelQueen.chain_cr += chain_cr;
        On.BeeLevelQueen.morph_cr += morph_cr;
    }

    protected void Start(On.BeeLevel.orig_Start orig, BeeLevel self)
    {
        orig(self);
        beeLevel = self;//new
        self.StartCoroutine(self.follower_cr());//new
    }

    private IEnumerator follower_cr(On.BeeLevel.orig_follower_cr orig, BeeLevel self)
    {
        yield return CupheadTime.WaitForSeconds(self, 5f);//new
        self.queen.StartFollower();/*
		    while (self.queen.state != BeeLevelQueen.State.Idle)
		    {
			    yield return null;
		    }*/
        yield break;
    }

    protected void OnDestroy(On.BeeLevelQueen.orig_OnDestroy orig, BeeLevelQueen self)
    {
        if(follower != null)
        {
            follower.Die();
            GameObject.Destroy(follower.gameObject);
        }
        UpdateForAirplane();
        orig(self);
    }

    public void StartFollower(On.BeeLevelQueen.orig_StartFollower orig, BeeLevelQueen self)
    {
        //self.state = BeeLevelQueen.State.Triangle;
        //self.StartCoroutine(self.follower_cr());
        LevelProperties.Bee.Follower properties = self.properties.CurrentState.follower;//new start
        BeeLevelQueenFollower.Properties p = new BeeLevelQueenFollower.Properties(PlayerManager.GetNext(), properties.introTime, properties.homingSpeed, properties.homingRotation, properties.homingTime, properties.health, properties.childDelay, properties.childHealth, properties.parryable);
        Vector3 pos = new Vector3(800f, 0f);
        follower = self.followerPrefab.Create(pos, p);
        follower.transform.SetScale(0.7f, 0.7f, null);//new end
    }

    private IEnumerator triangle_cr(On.BeeLevelQueen.orig_triangle_cr orig, BeeLevelQueen self)
    {
        self.transform.ResetLocalTransforms();
        self.transform.SetScale(new float?((float)MathUtils.PlusOrMinus()), new float?(1f), new float?(1f));
        self.transform.SetPosition(new float?(290f * self.transform.localScale.x), null, null);
        self.animator.Play("Warning");
        yield return self.animator.WaitForAnimationToEnd(self, "Warning", false, true);
        self.ClearTrigger(BeeLevelQueen.Triggers.Continue);
        self.SetAttackAnim(BeeLevelQueen.AttackAnimations.Triangle);
        self.animator.Play("Spell_Start");
        self.SetBool(BeeLevelQueen.Bools.Repeat, false);
        LevelProperties.Bee.Triangle properties = self.properties.CurrentState.triangle;
        yield return self.animator.WaitForAnimationToEnd(self, "Spell_Start", false, true);
        AudioManager.PlayLoop("bee_queen_spell_shake_loop");
        self.emitAudioFromObject.Add("bee_queen_spell_shake_loop");
        int i = 0;
        while (i < properties.count)
        {
            yield return CupheadTime.WaitForSeconds(self, properties.chargeTime);
            self.SetTrigger(BeeLevelQueen.Triggers.Continue);
            yield return self.animator.WaitForAnimationToEnd(self, "Spell_Charge_End", false, true);
            yield return self.animator.WaitForAnimationToEnd(self, "Spell_Attack_Start", false, true);
            BeeLevelQueenTriangle.Properties p = new BeeLevelQueenTriangle.Properties(PlayerManager.GetNext(), properties.introTime, properties.speed, properties.rotationSpeed, properties.health, properties.childSpeed, properties.childDelay, properties.childHealth, properties.childCount, properties.damageable);
            if (properties.damageable)
            {
                self.trianglePrefab.Create(p);
            }
            else
            {
                //self.triangleInvinciblePrefab.Create(p);
                self.triangleInvinciblePrefab.Create(p).transform.SetScale(0.8f, 0.8f, null);//new
            }
            yield return CupheadTime.WaitForSeconds(self, properties.attackTime);
            i++;
            self.SetBool(BeeLevelQueen.Bools.Repeat, i != properties.count);
            self.SetTrigger(BeeLevelQueen.Triggers.Continue);
        }
        yield return self.animator.WaitForAnimationToEnd(self, "Spell_End", false, true);
        AudioManager.Stop("bee_queen_spell_shake_loop");
        self.transform.SetPosition(new float?(0f), null, null);
        yield return CupheadTime.WaitForSeconds(self, properties.hesitate);
        self.state = BeeLevelQueen.State.Idle;
        yield break;
    }

    private IEnumerator chain_cr(On.BeeLevelQueen.orig_chain_cr orig, BeeLevelQueen self)
    {
        self.currentChain = self.properties.CurrentState.chain;
        self.transform.ResetLocalTransforms();
        if (Rand.Bool())//new start
        {
            self.transform.SetScale(-1f);
            self.transform.SetPosition(250f, 0f, 0f);
        }
        else
        {
            self.transform.SetPosition(new float?(-250f), new float?(0f), new float?(0f));
        }//new end
        //self.transform.SetPosition(new float?(-250f), new float?(0f), new float?(0f));
        self.animator.Play("Warning");
        yield return self.animator.WaitForAnimationToEnd(self, "Warning", false, true);
        self.transform.SetPosition(new float?(0f), new float?(550f), new float?(0f));
        self.EnableBody(true);
        self.SetBool(BeeLevelQueen.Bools.Repeat, true);
        self.animator.Play("Chain_Idle");
        self.animator.Play("Head_Closed_Idle", self.animator.GetLayerIndex("Head"));
        yield return self.StartCoroutine(self.tween_cr(self.transform, self.transform.position, new Vector2(0f, 300f), EaseUtils.EaseType.easeOutQuart, 0.6f));
        AudioManager.Play("bee_queen_chain_ascend_vocal");
        self.emitAudioFromObject.Add("bee_queen_chain_ascend_vocal");
        AudioManager.Play("bee_queen_chain_head_ascend");
        self.emitAudioFromObject.Add("bee_queen_chain_head_ascend");
        self.StartCoroutine(self.tween_cr(self.chain.transform, self.chain.transform.position, new Vector2(0f, -100f), EaseUtils.EaseType.easeInQuart, 0.6f));
        yield return self.StartCoroutine(self.tween_cr(self.head.transform, self.head.transform.position, new Vector2(0f, -100f), EaseUtils.EaseType.easeInQuart, 0.6f));
        CupheadLevelCamera.Current.Shake(20f, 0.7f, false);
        yield return CupheadTime.WaitForSeconds(self, 0.7f);
        self.animator.Play("Spit_Start", self.animator.GetLayerIndex("Head"));
        yield return CupheadTime.WaitForSeconds(self, 1f);
        if (!self.properties.CurrentState.chain.chainForever)
        {
            for (int i = 0; i < self.currentChain.count; i++)
            {
                AudioManager.Play("bee_chain_head_spit_delay");
                self.emitAudioFromObject.Add("bee_chain_head_spit_delay");
                yield return CupheadTime.WaitForSeconds(self, self.currentChain.delay);
                if (i >= self.currentChain.count - 1)
                {
                    self.SetBool(BeeLevelQueen.Bools.Repeat, false);
                }
                self.SetTrigger(BeeLevelQueen.Triggers.Continue);
            }
            yield return self.animator.WaitForAnimationToEnd(self, "Spit_Attack_End", self.animator.GetLayerIndex("Head"), false, true);
            AudioManager.Play("bee_queen_chain_head_decend");
            self.emitAudioFromObject.Add("bee_queen_chain_head_decend");
            self.StartCoroutine(self.tween_cr(self.chain.transform, self.chain.transform.position, new Vector2(0f, 300f), EaseUtils.EaseType.easeInQuart, 0.6f));
            yield return self.StartCoroutine(self.tween_cr(self.head.transform, self.head.transform.position, new Vector2(0f, 300f), EaseUtils.EaseType.easeInQuart, 0.6f));
            CupheadLevelCamera.Current.Shake(20f, 0.7f, false);
            yield return CupheadTime.WaitForSeconds(self, 0.7f);
            yield return self.StartCoroutine(self.tween_cr(self.transform, self.transform.position, new Vector2(0f, 550f), EaseUtils.EaseType.easeInQuart, 0.6f));
            self.EnableBody(false);
            yield return CupheadTime.WaitForSeconds(self, self.currentChain.hesitate);
            self.state = BeeLevelQueen.State.Idle;
            self.transform.SetScale(1f);//new
            yield break;
        }
        for (; ; )
        {
            AudioManager.Play("bee_chain_head_spit_delay");
            self.emitAudioFromObject.Add("bee_chain_head_spit_delay");
            yield return CupheadTime.WaitForSeconds(self, self.currentChain.delay);
            self.SetTrigger(BeeLevelQueen.Triggers.Continue);
            yield return null;
        }
    }

    private IEnumerator morph_cr(On.BeeLevelQueen.orig_morph_cr orig, BeeLevelQueen self)
    {
        /*float t = 0f;
        float time = 2.5f;
        float moveSpeed = 0f;*/
        self.animator.Play("Warning_Trans");
        yield return self.animator.WaitForAnimationToEnd(self, "Warning_Trans", false, true);
        /*AudioManager.PlayLoop("bee_queen_spell_antic");
        self.emitAudioFromObject.Add("bee_queen_spell_antic");
        Vector3 endPos = new Vector3(0f, 230f);
        Vector3 startPos = self.transform.position;
        while (t < time)
        {
            float val = EaseUtils.Ease(EaseUtils.EaseType.linear, 0f, 1f, t / time);
            self.transform.position = Vector2.Lerp(startPos, endPos, val);
            t += CupheadTime.Delta;
            yield return null;
        }
        self.transform.position = endPos;
        AudioManager.Stop("bee_queen_spell_antic");
        self.animator.SetTrigger("Continue");
        yield return self.animator.WaitForAnimationToEnd(self, "Morph_Morph", false, true);
        yield return CupheadTime.WaitForSeconds(self, 0.54f);
        t = 0f;
        while (t < 0.76f)
        {
            moveSpeed = ((t >= 0.3f) ? 300f : 800f);
            self.transform.position += Vector3.up * moveSpeed * CupheadTime.Delta;
            t += CupheadTime.Delta;
            yield return null;
        }
        t = 0f;
        time = 0.67f;
        startPos = self.transform.position;
        endPos = new Vector3(0f, -960f);
        self.StartCoroutine(self.spawn_puffs_cr());
        while (t < time)
        {
            float val2 = EaseUtils.Ease(EaseUtils.EaseType.linear, 0f, 1f, t / time);
            self.transform.position = Vector2.Lerp(startPos, endPos, val2);
            t += CupheadTime.Delta;
            yield return null;
        }*/
        self.airplane.StartIntro();
        UnityEngine.Object.Destroy(self.gameObject);
        yield break;
    }

    public static void UpdateForAirplane()//new
    {
        if(beeLevel != null)
        {
            beeLevel.targetSpeed = -beeLevel.properties.CurrentState.movement.speed;
            beeLevel.missingPlatformCount = beeLevel.properties.CurrentState.movement.missingPlatforms;
            beeLevel.CheckGrunts();
        }
    }

    private static BeeLevel beeLevel;
    private static BeeLevelQueenFollower follower;

}
