using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class GenieTransform
{
    public void Init()
    {
        On.FlyingGenieLevelGenieTransform.phase2_intro_cr += phase2_intro_cr;
        On.FlyingGenieLevelGenieTransform.SpawnTurban += SpawnTurban;
        On.FlyingGenieLevelGenieTransform.shoot_cr += shoot_cr;
        On.FlyingGenieLevelGenieTransform.WaitWhileShooting += WaitWhileShooting;
        On.FlyingGenieLevelGenieTransform.move_cr += move_cr;
        On.FlyingGenieLevelGenieTransform.genie_intro_cr += genie_intro_cr;
        On.FlyingGenieLevelGenieTransform.SpawnPyramids += SpawnPyramids;
        On.FlyingGenieLevelGenieTransform.attack_cr += attack_cr;
    }

    private IEnumerator phase2_intro_cr(On.FlyingGenieLevelGenieTransform.orig_phase2_intro_cr orig, FlyingGenieLevelGenieTransform self)
    {
        AudioManager.Play("genie_return");
        self.emitAudioFromObject.Add("genie_return");/*
		LevelProperties.FlyingGenie.Scan p = self.properties.CurrentState.scan;
		float timer = 0f;
		float P1ShrinkTimer = 0f;
		float P2ShrinkTimer = 0f;
		PlanePlayerController player = PlayerManager.GetPlayer(PlayerId.PlayerOne) as PlanePlayerController;
		PlanePlayerController player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo) as PlanePlayerController;
		bool player2In = player2 != null;
		while (timer < p.scanDuration)
		{
			timer += CupheadTime.Delta;
			if (Level.Current.mode != Level.Mode.Easy)
			{
				if (player.Shrunk)
				{
					P1ShrinkTimer += CupheadTime.Delta;
				}
				if (player2In && player2.Shrunk)
				{
					P2ShrinkTimer += CupheadTime.Delta;
				}
			}
			yield return null;
		}
		if (P1ShrinkTimer >= p.miniDuration)
		{
			if (player2In)
			{
				if (P2ShrinkTimer >= p.miniDuration)
				{
					self.skipMarionette = true;
				}
				else
				{
					self.skipMarionette = false;
				}
			}
			else
			{
				self.skipMarionette = true;
			}
		}*/
        self.animator.SetTrigger("Continue");
        self.pyramidsGoingClockwise = Rand.Bool();/*
		if (self.skipMarionette)
		{
			self.transitionHP = p.transitionDamage;
			self.animator.SetBool("IsPuppet", true);
			self.state = FlyingGenieLevelGenieTransform.State.Giant;
			self.StartCoroutine(self.move_up_puppet_cr());
			self.properties.DealDamageToNextNamedState();
		}
		else
		{*/
        self.animator.SetBool("IsPuppet", false);
        self.state = FlyingGenieLevelGenieTransform.State.Marionette;
        yield return self.animator.WaitForAnimationToEnd(self, "Marionette_Intro", false, true);
        self.startPos = self.transform.position;
        self.StartCoroutine(self.move_cr());
        self.StartCoroutine(self.shoot_cr());
        //}
        yield return null;
        yield break;
    }

    private void SpawnTurban(On.FlyingGenieLevelGenieTransform.orig_SpawnTurban orig, FlyingGenieLevelGenieTransform self)
    {/*
		if (!self.skipMarionette)
		{
			self.spawner = self.spawnerPrefab.Create(new Vector3(self.transform.position.x, (float)Level.Current.Height + 100f), PlayerManager.GetNext(), self.properties.CurrentState.bullets);
			self.spawner.isDead = false;
		}*/
    }

    private IEnumerator shoot_cr(On.FlyingGenieLevelGenieTransform.orig_shoot_cr orig, FlyingGenieLevelGenieTransform self)
    {
        LevelProperties.FlyingGenie.Bullets p = self.properties.CurrentState.bullets;
        int mainShotIndex = UnityEngine.Random.Range(0, p.shotCount.Length);
        string[] shotCount = p.shotCount[mainShotIndex].Split(new char[]
        {
        ','
        });
        int shotIndex = 0;
        string[] pinkCount = p.pinkString.Split(new char[]
        {
        ','
        });
        int pinkIndex = 0;
        Coroutine bulletSpam = self.StartCoroutine(FireBulletSpam(self));//new
        while (self.state == FlyingGenieLevelGenieTransform.State.Marionette)
        {
            self.isShooting = false;
            shotCount = p.shotCount[mainShotIndex].Split(new char[]
            {
            ','
            });
            yield return CupheadTime.WaitForSeconds(self, p.hesitateRange.RandomFloat());
            self.isShooting = true;
            self.animator.SetBool("IsAttacking", true);
            yield return self.animator.WaitForAnimationToEnd(self, "Marionette_Attack_Start", false, true);
            AudioManager.Play("genie_voice_laugh_reverb");
            AbstractPlayerController player = PlayerManager.GetNext();
            self.animator.Play("Marionette_Spark");
            for (int i = 0; i < shotCount.Length; i++)
            {
                for (int j = 0; j < Parser.IntParse(shotCount[shotIndex]); j++)
                {
                    if (player == null || player.IsDead)
                    {
                        player = PlayerManager.GetNext();
                    }
                    Vector3 dir = player.transform.position - self.marionetteShootRoot.transform.position;
                    if (dir.x > 0f)
                    {
                        dir.x = 0f;
                    }
                    if (pinkCount[pinkIndex][0] == 'P')
                    {
                        //self.pinkBullet.Create(self.marionetteShootRoot.transform.position, MathUtils.DirectionToAngle(dir), p.shotSpeed);
                        yield return FireRandom(self, dir);//new
                        AudioManager.Play("genie_puppet_shoot");
                        self.emitAudioFromObject.Add("genie_puppet_shoot");
                    }
                    else if (pinkCount[pinkIndex][0] == 'R')
                    {
                        //self.shotBullet.Create(self.marionetteShootRoot.transform.position, MathUtils.DirectionToAngle(dir), p.shotSpeed);
                        yield return FireRandom(self, dir);//new
                        AudioManager.Play("genie_puppet_shoot");
                        self.emitAudioFromObject.Add("genie_puppet_shoot");
                    }
                    yield return self.WaitWhileShooting(p.shotDelay, p.shotSpeed);
                    pinkIndex = (pinkIndex + 1) % pinkCount.Length;
                }
                if (player == null || player.IsDead)
                {
                    player = PlayerManager.GetNext();
                }
                yield return self.WaitWhileShooting(p.shotDelay, p.shotSpeed);
                if (shotIndex < shotCount.Length - 1)
                {
                    shotIndex++;
                }
                else
                {
                    mainShotIndex = (mainShotIndex + 1) % p.shotCount.Length;
                    shotIndex = 0;
                }
                yield return null;
            }
            yield return null;
            self.animator.SetBool("IsAttacking", false);
        }
        yield return null;
        self.StopCoroutine(bulletSpam);//new
        yield break;
    }

    private IEnumerator WaitWhileShooting(On.FlyingGenieLevelGenieTransform.orig_WaitWhileShooting orig, FlyingGenieLevelGenieTransform self, float time, float shootSpeed)
    {
        bool pointingUp = false;
        float timeEsalpsed = 0f;
        float timeSinceSubShot = 0f;
        List<Vector3> vectors = new List<Vector3>();//new start
        vectors.Add(new Vector3(0, 0));
        vectors.Add(new Vector3(100, 0));
        vectors.Add(new Vector3(200, 0));
        vectors.Add(new Vector3(300, 0));
        vectors.Add(new Vector3(400, 0));//new end
        while (timeEsalpsed <= time)
        {
            if (timeSinceSubShot >= 0.12f)
            {
                //self.shootBullet.Create(self.marionetteShootRoot.transform.position, (float)((!pointingUp) ? -100 : 100), shootSpeed);
                for (int i = 0; i < 5; i++)//new start
                {
                    self.shootBullet.Create(self.marionetteShootRoot.transform.position + vectors[i], (float)((!pointingUp) ? -100 : 100), shootSpeed);
                }//new end
                pointingUp = !pointingUp;
                timeSinceSubShot = 0f;
            }
            timeEsalpsed += CupheadTime.Delta;
            timeSinceSubShot += CupheadTime.Delta;
            yield return null;
        }
        yield break;
    }

    // Token: 0x06003F3B RID: 16187 RVA: 0x0015D154 File Offset: 0x0015B354
    private IEnumerator move_cr(On.FlyingGenieLevelGenieTransform.orig_move_cr orig, FlyingGenieLevelGenieTransform self)
    {
        YieldInstruction wait = new WaitForFixedUpdate();
        while (self.state == FlyingGenieLevelGenieTransform.State.Marionette)
        {
            if (!self.isShooting)
            {
                if (self.transform.position.x > -self.startPos.x + 300)
                //if (self.transform.position.x > -self.startPos.x)
                {
                    self.transform.AddPosition(-self.properties.CurrentState.bullets.marionetteMoveSpeed * CupheadTime.FixedDelta, 0f, 0f);
                }
            }
            else if (self.transform.position.x < self.startPos.x)
            {
                self.transform.AddPosition(self.properties.CurrentState.bullets.marionetteReturnSpeed * CupheadTime.FixedDelta, 0f, 0f);
            }
            yield return wait;
        }
        yield break;
    }

    private IEnumerator genie_intro_cr(On.FlyingGenieLevelGenieTransform.orig_genie_intro_cr orig, FlyingGenieLevelGenieTransform self)
    {
        float pullSpeed = 700f;
        float size = self.GetComponent<SpriteRenderer>().bounds.size.x;
        float angle = 120f;
        int number = 1;
        if (!self.skipMarionette)
        {
            self.animator.SetTrigger("MarionetteDeath");
            self.GetComponent<LevelBossDeathExploder>().StartExplosion();
        }
        yield return CupheadTime.WaitForSeconds(self, 1f);
        while (self.transform.position.y < 960f)
        {
            self.transform.AddPosition(0f, pullSpeed * CupheadTime.Delta, 0f);
            yield return null;
        }
        if (!self.skipMarionette)
        {
            self.GetComponent<LevelBossDeathExploder>().StopExplosions();
        }
        yield return CupheadTime.WaitForSeconds(self, 0.7f);
        self.animator.Play("Giant_Intro");
        self.transform.position = new Vector3(640f + size / 3f, 0f);
        Vector3 startPos = self.transform.position;
        float t = 0f;
        float time = 1f;
        while (t < time)
        {
            float val = EaseUtils.Ease(EaseUtils.EaseType.linear, 0f, 1f, t / time);
            self.transform.position = Vector2.Lerp(startPos, self.giantRoot.position, val);
            t += CupheadTime.Delta;
            yield return null;
        }
        self.transform.position = self.giantRoot.position;
        self.pyramidPivotPoint.transform.AddPosition(100f);//new start
        for (int i = 0; i < 9; i++)
        {
            self.SpawnPyramids(angle * 0.0174532924f * (float)i / 3, number);
            number++;
        }//new end
        /*for (int i = 0; i < 3; i++)
        {
            self.SpawnPyramids(angle * 0.0174532924f * (float)i, number);
            number++;
        }*/
        self.StartCoroutine(self.attack_cr());
        yield return null;
        yield break;
    }

    private void SpawnPyramids(On.FlyingGenieLevelGenieTransform.orig_SpawnPyramids orig, FlyingGenieLevelGenieTransform self, float startingAngle, int number)
    {
        LevelProperties.FlyingGenie.Pyramids pyramids = self.properties.CurrentState.pyramids;
        FlyingGenieLevelPyramid flyingGenieLevelPyramid = UnityEngine.Object.Instantiate<FlyingGenieLevelPyramid>(self.pyramidPrefab);
        flyingGenieLevelPyramid.Init(pyramids, self.transform.position, startingAngle, pyramids.speedRotation, self.pyramidPivotPoint, number, self.pyramidsGoingClockwise);
        flyingGenieLevelPyramid.GetComponent<Collider2D>().enabled = false;
        flyingGenieLevelPyramid.transform.SetScale(0.4f, 0.4f, 0.4f);//new
        self.pyramids.Add(flyingGenieLevelPyramid);
    }

    private IEnumerator attack_cr(On.FlyingGenieLevelGenieTransform.orig_attack_cr orig, FlyingGenieLevelGenieTransform self)
    {
        LevelProperties.FlyingGenie.Pyramids p = self.properties.CurrentState.pyramids;
        string[] delayString = p.attackDelayString.GetRandom<string>().Split(',');
        string[] attackString = p.pyramidAttackString.GetRandom<string>().Split(',');
        int delayIndex = UnityEngine.Random.Range(0, delayString.Length);
        int attackIndex = UnityEngine.Random.Range(0, attackString.Length);
        //float delay = 0f;
        //int numberReceived = 0;
        float t = 0f;
        float time = 2.5f;
        foreach (FlyingGenieLevelPyramid flyingGenieLevelPyramid in self.pyramids)
        {
            flyingGenieLevelPyramid.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
        }
        while (t < time)
        {
            t += CupheadTime.Delta;
            foreach (FlyingGenieLevelPyramid flyingGenieLevelPyramid2 in self.pyramids)
            {
                flyingGenieLevelPyramid2.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, t / time);
            }
            yield return null;
        }
        foreach (FlyingGenieLevelPyramid flyingGenieLevelPyramid3 in self.pyramids)
        {
            flyingGenieLevelPyramid3.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
            flyingGenieLevelPyramid3.GetComponent<Collider2D>().enabled = true;
        }
        for (; ; )
        {/*
			for (int i = attackIndex; i < attackString.Length; i++)
			{
				Parser.FloatTryParse(delayString[delayIndex], out delay);
				yield return CupheadTime.WaitForSeconds(self, delay);
				string[] attackOrder = attackString[i].Split(new char[]
				{
					'-'
				});
				foreach (string s in attackOrder)
				{
					Parser.IntTryParse(s, out numberReceived);
					for (int l = 0; l < self.pyramids.Count; l++)
					{
						if (self.pyramids[l].number == numberReceived)
						{
							self.StartCoroutine(self.pyramids[l].beam_cr());
						}
					}
				}
				for (int j = 0; j < self.pyramids.Count; j++)
				{
					if (self.pyramids[j].number == numberReceived)
					{
						while (!self.pyramids[j].finishedATK)
						{
							yield return null;
						}
					}
				}
				attackIndex = 0;
				i %= attackString.Length;
				delayIndex = (delayIndex + 1) % delayString.Length;
			}
			yield return null;*/
            int pyramidNumber = UnityEngine.Random.Range(0, self.pyramids.Count);//new start
            self.StartCoroutine(self.pyramids[pyramidNumber].beam_cr());
            while (!self.pyramids[pyramidNumber].finishedATK)
            {
                yield return null;
            }//new end 
        }
    }

    private IEnumerator FireRandom(FlyingGenieLevelGenieTransform self, Vector3 dir)//new
    {
        float angle = MathUtils.DirectionToAngle(dir) + UnityEngine.Random.Range(-30f, 30f);
        switch (UnityEngine.Random.Range(0, 3))
        {
            case 0:
                FireShotgun(self, angle);
                break;
            case 1:
                FireReverseShotgun(self, angle);
                break;
            case 2:
                yield return CupheadTime.WaitForSeconds(self, 0.1f);
                FireGiantBullet(self, UnityEngine.Random.Range(0, 2) == 0);
                yield return CupheadTime.WaitForSeconds(self, 0.1f);
                break;
        }
        yield break;
    }

    private void FireShotgun(FlyingGenieLevelGenieTransform self, float angle)//new
    {
        LevelProperties.FlyingGenie.Bullets p = self.properties.CurrentState.bullets;
        for (int i = -2; i < 3; i++)
        {
            self.shotBullet.Create(self.marionetteShootRoot.transform.position, angle + 5 * i, p.shotSpeed * 0.6f);
        }
    }

    private void FireReverseShotgun(FlyingGenieLevelGenieTransform self, float angle)//new
    {
        while (angle < -200)
        {
            angle += UnityEngine.Random.Range(5f, 10f);
        }
        while (angle > -150)
        {
            angle -= UnityEngine.Random.Range(5f, 10f);
        }
        LevelProperties.FlyingGenie.Bullets p = self.properties.CurrentState.bullets;
        for (int i = 4; i < 20; i++)
        {
            self.shotBullet.Create(self.marionetteShootRoot.transform.position, angle + 5 * i, p.shotSpeed * 0.6f);
            self.shotBullet.Create(self.marionetteShootRoot.transform.position, angle - 5 * i, p.shotSpeed * 0.6f);
        }
    }

    private void FireGiantBullet(FlyingGenieLevelGenieTransform self, bool up)//new
    {
        LevelProperties.FlyingGenie.Bullets p = self.properties.CurrentState.bullets;
        float y = up ? 200 : -200;
        self.shotBullet.Create(new Vector3(800f, y), -180f, p.shotSpeed * 1.2f).transform.SetScale(6f, 6f, 6f);
    }

    private IEnumerator FireBulletSpam(FlyingGenieLevelGenieTransform self)//new
    {
        LevelProperties.FlyingGenie.Bullets p = self.properties.CurrentState.bullets;

        float startY = -350f;
        int possibleYCount = 15;
        float[] possibleY = new float[possibleYCount];
        for(int i = 0; i <= possibleYCount; i ++)
        {
            possibleY[i] = startY + i * 50f;
        }
        possibleY.Shuffle();

        int j = 0;
        for (; ; )
        {
            j++;
            if(j == possibleYCount)
            {
                j = 0;
                possibleY.Shuffle();
            }
            float y = possibleY[j];
            self.shotBullet.Create(new Vector3(650f, y), -180f, p.shotSpeed * 1.5f);
            yield return CupheadTime.WaitForSeconds(self, 0.15f);
        }
    }
}
