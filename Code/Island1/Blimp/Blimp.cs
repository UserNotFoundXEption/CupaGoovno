using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class Blimp
{
    public void Init()
    {
        new BlimpEnemy().Init();
        new BlimpMoonLady().Init();
        new BlimpUFO().Init();
        new BlimpGeminiShoot().Init();
        On.FlyingBlimpLevelBlimpLady.spawnProjectile += spawnProjectile;
        On.FlyingBlimpLevelBlimpLady.spawnEnemy_cr += spawnEnemy_cr;
        On.FlyingBlimpLevelBlimpLady.FireArrowsStars += FireArrowsStars;
        On.FlyingBlimpLevelBlimpLady.taurus_cr += taurus_cr;
        On.FlyingBlimpLevelBlimpLady.spawn_moon_lady_cr += spawn_moon_lady_cr;
        On.FlyingBlimpLevelBlimpLady.select_constellation_cr += select_constellation_cr;
        On.FlyingBlimpLevelBlimpLady.SpawnGemini += SpawnGemini;
        On.FlyingBlimpLevel.enemies_cr += enemies_cr;
    }

    private void spawnProjectile(On.FlyingBlimpLevelBlimpLady.orig_spawnProjectile orig, FlyingBlimpLevelBlimpLady self)
    {
        //FlyingBlimpLevelShootProjectile p = self.shootProjectilePrefab.Create(self.projectileRoot.position, 0f, self.properties.CurrentState.shoot);
        FlyingBlimpLevelShootProjectile p = self.shootProjectilePrefab.Create(self.projectileRoot.position + new Vector3(200f, 0f), 0f, self.properties.CurrentState.shoot);//new
        p.transform.SetScale(5f, null, null);//new
    }

    public IEnumerator spawnEnemy_cr(On.FlyingBlimpLevelBlimpLady.orig_spawnEnemy_cr orig, FlyingBlimpLevelBlimpLady self)
    {
        blimpLady = self;//new
        LevelProperties.FlyingBlimp.Enemy p = self.properties.CurrentState.enemy;
        string[] spawnPattern = p.spawnString.GetRandom<string>().Split(new char[]
        {
        ','
        });
        string[] typePattern = p.typeString.GetRandom<string>().Split(new char[]
        {
        ','
        });
        bool AParryable = false;
        float waitTime = 0f;
        //int counter = 0;
        int typeIndex = 0;
        int spawnIndex = UnityEngine.Random.Range(0, spawnPattern.Length);
        Vector3 spawnPos = Vector3.zero;
        for (; ; )
        {
            for (int i = spawnIndex; i < spawnPattern.Length; i++)
            {
                if (waitTime > 0f)
                {
                    yield return CupheadTime.WaitForSeconds(self, waitTime);
                }
                if (spawnPattern[i][0] == 'D')
                {
                    Parser.FloatTryParse(spawnPattern[i].Substring(1), out waitTime);
                }
                else
                {
                    string[] array = spawnPattern[i].Split(new char[]
                    {
                    '-'
                    });
                    foreach (string s in array)
                    {
                        float y = 0f;
                        float stopPoint = 0f;
                        Parser.FloatTryParse(s, out y);
                        Parser.FloatTryParse(s, out stopPoint);
                        FlyingBlimpLevelEnemy prefab = null;
                        /*if (typePattern[typeIndex][0] == 'A')
                        {
                            prefab = self.enemyPrefabA;
                            if ((float)counter >= p.APinkOccurance.RandomFloat())
                            {
                                AParryable = true;
                                counter = 0;
                            }
                            else
                            {
                                AParryable = false;
                                counter++;
                            }
                        }
                        else if (typePattern[typeIndex][0] == 'B')
                        {
                            prefab = self.enemyPrefabB;
                            AParryable = false;
                        }*/
                        bool randomChoice = UnityEngine.Random.Range(0, 2) == 0;//new start
                        bool gemini = self.properties.CurrentState.stateName == LevelProperties.FlyingBlimp.States.Gemini;
                        bool sag = self.properties.CurrentState.stateName == LevelProperties.FlyingBlimp.States.Sagittarius;
                        if (!sag && (gemini || randomChoice))
                        {
                            prefab = self.enemyPrefabA;
                        }
                        else
                        {
                            prefab = self.enemyPrefabB;
                        }//new end
                        spawnPos.y = y;
                        if (self.state != FlyingBlimpLevelBlimpLady.State.Death)
                        {
                            self.SummonEnemy(prefab, spawnPos, stopPoint, AParryable);
                        }
                        typeIndex = (typeIndex + 1) % typePattern.Length;
                    }
                    waitTime = p.stringDelay;
                }
                i %= spawnPattern.Length;
            }
            spawnIndex = 0;
        }
    }

    private void FireArrowsStars(On.FlyingBlimpLevelBlimpLady.orig_FireArrowsStars orig, FlyingBlimpLevelBlimpLady self)
    {
        LevelProperties.FlyingBlimp.Sagittarius sagittarius = self.properties.CurrentState.sagittarius;
        int num = 6;//3
        AbstractPlayerController next = PlayerManager.GetRandom();//new
        for (int i = 0; i < num; i++)
        {
            //AbstractPlayerController next = PlayerManager.GetNext();
            float num2 = sagittarius.homingSpreadAngle.GetFloatAt((float)i / ((float)num - 1f));
            float num3 = sagittarius.homingSpreadAngle.max / 2f;
            num2 -= num3;
            float num4 = Mathf.Atan2(0f, -360f) * 57.29578f;
            //self.sagittariusStarPrefab.Create(self.arrowEffectRoot.transform.position, num4 + num2, sagittarius.arrowInitialSpeed, sagittarius.homingSpeed, sagittarius.homingRotation, sagittarius.homingDurationRange.RandomFloat(), sagittarius.homingDelay, next, (float)sagittarius.arrowHP);
            self.sagittariusArrowPrefab.Create(self.arrowRoot.position, num4 + num2, sagittarius.arrowInitialSpeed);//new
        }
        self.arrowEffect.Create(self.arrowEffectRoot.transform.position);
        //self.sagittariusArrowPrefab.Create(self.arrowRoot.position, 180f, sagittarius.arrowInitialSpeed);
    }

    private IEnumerator taurus_cr(On.FlyingBlimpLevelBlimpLady.orig_taurus_cr orig, FlyingBlimpLevelBlimpLady self)
    {
        self.pivotPoint.AddPosition(-150f, 0f, 0f);//new
        LevelProperties.FlyingBlimp.Taurus p = self.properties.CurrentState.taurus;
        self.waitLoopTime = p.attackDelayRange.RandomFloat();
        self.moving = true;
        self.state = FlyingBlimpLevelBlimpLady.State.Taurus;
        self.movementSpeed = p.movementSpeed;
        do
        {
            float t = 0f;
            while (t < self.waitLoopTime)
            {
                t += CupheadTime.Delta;
                if (!self.isLooping)
                {
                    break;
                }
                yield return null;
            }
            t = 0f;
            self.moving = false;
            //self.animator.SetTrigger("TaurusATK");
            //yield return self.animator.WaitForAnimationToStart(self, "Taurus_Attack", false);
            if (self.isLooping)//new
            {//new
                self.animator.Play("Taurus_Attack", -1, 0.5f);//new
                AudioManager.Play("level_flying_blimp_taurus_attack");
                AudioManager.Stop("level_flying_blimp_taurus_idle");
                self.emitAudioFromObject.Add("level_flying_blimp_taurus_attack");
                yield return self.animator.WaitForAnimationToEnd(self, "Taurus_Attack", false, true);
                self.moving = true;
            }//new
            yield return null;
        }
        while (self.isLooping);
        self.animator.Play("Big_Cloud");
        self.pivotPoint.AddPosition(150f, 0f, 0f);//new
        self.movementSpeed = self.originalSpeed;
        self.StartCoroutine(self.final_fade_cr());
        yield return null;
        yield break;
    }

    private IEnumerator spawn_moon_lady_cr(On.FlyingBlimpLevelBlimpLady.orig_spawn_moon_lady_cr orig, FlyingBlimpLevelBlimpLady self)
    {
        DestroyTornados();
        yield return orig(self);
    }

    private IEnumerator select_constellation_cr(On.FlyingBlimpLevelBlimpLady.orig_select_constellation_cr orig, FlyingBlimpLevelBlimpLady self)
    {
        DestroyTornados();
        yield return orig(self);
    }

    private void SpawnGemini(On.FlyingBlimpLevelBlimpLady.orig_SpawnGemini orig, FlyingBlimpLevelBlimpLady self)
    {
        self.geminiTarget = self.objectSpawnRoot.transform.position;
        Vector2 a = self.geminiTarget;
        //Vector2 vector = new Vector2(UnityEngine.Random.value * (float)((!Rand.Bool()) ? -1 : 1), UnityEngine.Random.value * (float)((!Rand.Bool()) ? -1 : 1));
        Vector2 vector = Vector2.zeroVector;
        self.geminiTarget = a + vector.normalized * self.objectSpawnRoot.radius * UnityEngine.Random.value;
        self.geminiObject = UnityEngine.Object.Instantiate<FlyingBlimpLevelGeminiShoot>(self.geminiObjectPrefab);
        self.geminiObject.Init(self.properties.CurrentState.gemini, self.geminiTarget);
    }

    private IEnumerator enemies_cr(On.FlyingBlimpLevel.orig_enemies_cr orig, FlyingBlimpLevel self)
    {
        yield return CupheadTime.WaitForSeconds(self, 5f);
        if (self.properties.CurrentState.stateName != LevelProperties.FlyingBlimp.States.Moon)
        {
            yield return orig(self);
        }
    }

    private void DestroyTornados()//new
    {
        FlyingBlimpLevelTornado[] tornados = UnityEngine.Object.FindObjectsOfType<FlyingBlimpLevelTornado>();
        foreach (FlyingBlimpLevelTornado tornado in tornados)
        {
            tornado.transform.position = new Vector3(-2137f, 0f);
        }
    }

    public static FlyingBlimpLevelBlimpLady blimpLady;
}
