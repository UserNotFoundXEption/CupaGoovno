using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class ClownHorse
{
    public void Init()
    {
        //On.ClownLevelClownHorse.FireWaveBullets += FireWaveBullets;
        On.ClownLevelClownHorse.horse_cr += horse_cr;
        On.ClownLevelClownHorse.clown_fall_cr += clown_fall_cr;
        On.ClownLevelClownHorse.horse_death_cr += horse_death_cr;
    }

    private IEnumerator horse_cr(On.ClownLevelClownHorse.orig_horse_cr orig, ClownLevelClownHorse self, ClownLevelClownHorse.HorseType horseType, string[] positionPattern, float ATKAmount)
    {
        bool isPink = false;
        float hesitate = 0f;
        float posOffset = 0f;
        float ATKcounter = 0f;
        float YSpeed = 0f;
        LevelProperties.Clown.Horse p = self.properties.CurrentState.horse;
        self.SelectStartPos();
        while (ATKcounter < ATKAmount)
        {
            Parser.FloatTryParse(positionPattern[self.positionIndex], out posOffset);
            float getPos = 360f - posOffset;
            while (self.transform.position.y != getPos)
            {
                self.pos = self.transform.position;
                self.pos.y = Mathf.MoveTowards(self.transform.position.y, getPos, p.HorseSpeed * CupheadTime.Delta);
                self.transform.position = self.pos;
                yield return null;
            }
            self.StartCoroutine(self.spit_fx_cr());
            if (horseType != ClownLevelClownHorse.HorseType.Wave)
            {
                if (horseType == ClownLevelClownHorse.HorseType.Drop)
                {
                    hesitate = p.DropHesitate;
                    float spawnY = 0f;
                    float nextSpawnY = 0f;
                    int i = 0;
                    self.dropBulletPositionPattern = p.DropBulletPositionString[self.dropMainIndex].Split(new char[]
                    {
                        ','
                    });
                    string[] droppattern = self.dropBulletPositionPattern[self.dropBulletIndex].Split(new char[]
                    {
                        '-'
                    });
                    self.SpitSFX();
                    self.animator.SetBool("Spit", true);
                    float[] durationBeforeDrops = new float[droppattern.Length];
                    List<int> indexPatterns = new List<int>(droppattern.Length);
                    for (int l = 0; l < droppattern.Length; l++)
                    {
                        indexPatterns.Add(l);
                    }
                    float currentDuration = self.properties.CurrentState.horse.DropBulletDelay;
                    bool dropTwo = true;
                    while (indexPatterns.Count > 0)
                    {
                        if (indexPatterns.Count > 1 && dropTwo)
                        {
                            currentDuration += self.properties.CurrentState.horse.DropBulletTwoDelay.RandomFloat();
                            int index = UnityEngine.Random.Range(0, indexPatterns.Count);
                            durationBeforeDrops[indexPatterns[index]] = currentDuration;
                            indexPatterns.RemoveAt(index);
                            index = UnityEngine.Random.Range(0, indexPatterns.Count);
                            durationBeforeDrops[indexPatterns[index]] = currentDuration;
                            indexPatterns.RemoveAt(index);
                            dropTwo = false;
                        }
                        else
                        {
                            currentDuration += self.properties.CurrentState.horse.DropBulletOneDelay.RandomFloat();
                            int index2 = UnityEngine.Random.Range(0, indexPatterns.Count);
                            durationBeforeDrops[indexPatterns[index2]] = currentDuration;
                            indexPatterns.RemoveAt(index2);
                            dropTwo = true;
                        }
                    }
                    for (int j = 0; j < droppattern.Length; j++)
                    {
                        if (j < droppattern.Length - 1)
                        {
                            i = j + 1;
                        }
                        else
                        {
                            i = 0;
                        }
                        Parser.FloatTryParse(droppattern[j], out spawnY);
                        Parser.FloatTryParse(droppattern[i], out nextSpawnY);
                        float dist = nextSpawnY - spawnY;
                        self.FireDropBullets(spawnY, durationBeforeDrops[j]);
                        float halfSpeed = p.DropBulletInitalSpeed / 2f;
                        yield return CupheadTime.WaitForSeconds(self, dist / halfSpeed / 2f);
                    }
                    self.animator.SetBool("Spit", false);
                    if (self.dropBulletIndex < self.dropBulletPositionPattern.Length - 1)
                    {
                        self.dropBulletIndex++;
                    }
                    else
                    {
                        self.dropMainIndex = (self.dropMainIndex + 1) % p.DropBulletPositionString.Length;
                        self.dropBulletIndex = 0;
                    }
                    yield return CupheadTime.WaitForSeconds(self, p.DropATKDelay);
                }
            }
            else
            {
                hesitate = p.WaveHesitate;
                Vector3 pos = self.projectileRoot.transform.position;
                if (Rand.Bool())
                {
                    YSpeed = -p.WaveBulletWaveSpeed;
                }
                else
                {
                    YSpeed = p.WaveBulletWaveSpeed;
                }
                self.SpitSFX();
                self.animator.SetBool("Spit", true);
                for (int k = 0; k < p.WaveBulletCount; k++)
                {
                    self.wavePinkPattern = p.WavePinkString[self.pinkMainIndex].Split(new char[]
                    {
                        ','
                    });
                    if (self.wavePinkPattern[self.pinkIndex][0] == 'R')
                    {
                        isPink = false;
                    }
                    else if (self.wavePinkPattern[self.pinkIndex][0] == 'P')
                    {
                        isPink = true;
                    }
                    pos = new Vector3(pos.x, -200f);//new
                    self.FireWaveBullets(k, isPink, YSpeed, pos);
                    if (self.pinkIndex < self.wavePinkPattern.Length - 1)
                    {
                        self.pinkIndex++;
                    }
                    else
                    {
                        self.pinkMainIndex = (self.pinkMainIndex + 1) % p.WavePinkString.Length;
                        self.pinkIndex = 0;
                    }
                    yield return CupheadTime.WaitForSeconds(self, p.WaveBulletDelay);
                }
                self.animator.SetBool("Spit", false);
                yield return CupheadTime.WaitForSeconds(self, p.WaveATKDelay);
            }
            self.positionIndex %= positionPattern.Length;
            ATKcounter += 1f;
        }
        yield return CupheadTime.WaitForSeconds(self, hesitate);
        while (self.transform.position.y != self.startPos.y)
        {
            self.pos = self.transform.position;
            self.pos.y = Mathf.MoveTowards(self.transform.position.y, self.startPos.y, p.HorseSpeed * CupheadTime.Delta);
            self.transform.position = self.pos;
            yield return null;
        }
        self.StartCoroutine(self.select_horse_cr());
        yield return null;
        yield break;
    }

    private IEnumerator horse_death_cr(On.ClownLevelClownHorse.orig_horse_death_cr orig, ClownLevelClownHorse self)
    {
        self.clownSwing.StartSwing();
        yield return orig(self);
    }

    private IEnumerator clown_fall_cr(On.ClownLevelClownHorse.orig_clown_fall_cr orig, ClownLevelClownHorse self)
    {
        float fallGravity = -100f;
        float fallAccumulatedGravity = 0f;
        Vector3 fallVelocity = Vector3.zero;
        self.FallHorseSFXOff();
        self.droppedClown = true;
        while (self.transform.position.y > -660f)
        {
            if (CupheadTime.Delta != 0f)
            {
                self.transform.position += (fallVelocity + new Vector3(-300f, fallAccumulatedGravity)) * CupheadTime.FixedDelta;
                fallAccumulatedGravity += fallGravity;
            }
            yield return null;
        }
        yield return CupheadTime.WaitForSeconds(self, 0.1f);
        while (self.moveObject != null)
        {
            yield return null;
        }
        //self.clownSwing.StartSwing();
        UnityEngine.Object.Destroy(self.gameObject);
        yield return null;
        yield break;
    }

    private void FireWaveBullets(On.ClownLevelClownHorse.orig_FireWaveBullets orig, ClownLevelClownHorse self, int index, bool isPink, float YSpeed, object pos)
    {
        /*if (pos is Vector3 pos2)
        {
            pos2 = new Vector3(pos2.x, 0f);
            orig(self, index, isPink, YSpeed, pos2);
        }
        else
        {*/
            orig(self, index, isPink, YSpeed, pos);
        //}
    }
}
