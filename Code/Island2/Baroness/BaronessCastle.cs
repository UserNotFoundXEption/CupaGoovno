using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class BaronessCastle
{
    public void Init()
    {
        On.BaronessLevelCastle.SpawnJellyBeans += SpawnJellyBeans;
        On.BaronessLevelCastle.spawnJellybeans_cr += spawnJellybeans_cr;
        On.BaronessLevelCastle.open_teeth_cr += open_teeth_cr;
        On.BaronessLevelCastle.peppermint_cr += peppermint_cr;
        On.BaronessLevelCastle.shoot_cr += shoot_cr;
    }

    private void SpawnJellyBeans(On.BaronessLevelCastle.orig_SpawnJellyBeans orig, BaronessLevelCastle self, BaronessLevelJellybeans prefab)
    {
        Vector3 position = self.emergePoint.position;
        self.emergePoint.position = position;
        position.y = self.emergePoint.position.y - 20f;
        LevelProperties.Baroness.Jellybeans jellybeans = self.properties.CurrentState.jellybeans;
        prefab.Create(self.properties.CurrentState.jellybeans, position + new Vector3(200, 0, 0), jellybeans.movementSpeed, (float)jellybeans.HP);
        //prefab.Create(self.properties.CurrentState.jellybeans, position, jellybeans.movementSpeed, (float)jellybeans.HP);
        self.emergePoint.position = self.originalEmergePos;
    }

    private IEnumerator spawnJellybeans_cr(On.BaronessLevelCastle.orig_spawnJellybeans_cr orig, BaronessLevelCastle self)
    {
        LevelProperties.Baroness.Jellybeans p = self.properties.CurrentState.jellybeans;
        string[] typePattern = p.typeArray.GetRandom<string>().Split(new char[]
        {
        ','
        });
        float change = 0f;
        while (self.state != BaronessLevelCastle.State.ChaseIntro)
        {
            for (int i = 0; i < typePattern.Length; i++)
            {
                BaronessLevelJellybeans toSpawn = null;
                float beanSpawnDelay = p.spawnDelay.RandomFloat();
                if (typePattern[i][0] == 'R' || (BaronessLevelCastle.CURRENT_MINI_BOSS != null && BaronessLevelCastle.CURRENT_MINI_BOSS.bossId == BaronessLevelCastle.BossPossibility.Waffle))
                //if (typePattern[i][0] == 'R')
                {
                    toSpawn = self.greenJellyPrefab;
                }
                else if (typePattern[i][0] == 'P')
                {
                    toSpawn = self.pinkJellyPrefab;
                }
                if ((BaronessLevelCastle.CURRENT_MINI_BOSS != null && self.state == BaronessLevelCastle.State.Idle) || self.state == BaronessLevelCastle.State.EasyFinal)
                {
                    self.SpawnJellyBeans(toSpawn);
                    if (BaronessLevelCastle.CURRENT_MINI_BOSS.bossId == BaronessLevelCastle.BossPossibility.Waffle)//new start
                    {
                        yield return CupheadTime.WaitForSeconds(self, 0.35f);
                    }
                    else
                    {//new end
                        yield return CupheadTime.WaitForSeconds(self, beanSpawnDelay - change);
                    }//new
                }
                else
                {
                    yield return null;
                }
                if (self.jellyChangeDelay)
                {
                    change += beanSpawnDelay - beanSpawnDelay * (1f - p.spawnDelayChangePercentage / 100f);
                    self.jellyChangeDelay = false;
                }
            }
        }
        yield break;
    }

    private IEnumerator open_teeth_cr(On.BaronessLevelCastle.orig_open_teeth_cr orig, BaronessLevelCastle self)
    {
        yield break;
    }

    private IEnumerator peppermint_cr(On.BaronessLevelCastle.orig_peppermint_cr orig, BaronessLevelCastle self)
    {
        for (; ; )
        {
            float seconds = self.properties.CurrentState.peppermint.peppermintSpawnDurationRange.RandomFloat();
            yield return CupheadTime.WaitForSeconds(self, seconds);
            BaronessLevelPeppermint peppermint = UnityEngine.Object.Instantiate<BaronessLevelPeppermint>(self.peppermintPrefab);//new start
            LevelProperties.Baroness.Peppermint p = self.properties.CurrentState.peppermint;
            peppermint.Init(self.emergePoint.position, p.peppermintSpeed);//new end
                                                                          //self.teethState = BaronessLevelCastle.TeethState.StartOpen;
            yield return null;
        }
    }

    private IEnumerator shoot_cr(On.BaronessLevelCastle.orig_shoot_cr orig, BaronessLevelCastle self)
    {
        self.state = BaronessLevelCastle.State.Idle;
        LevelProperties.Baroness.BaronessVonBonbon p = self.properties.CurrentState.baronessVonBonbon;
        string[] pattern = p.timeString.GetRandom<string>().Split(new char[]
        {
            ','
        });
        self.timeIndex = UnityEngine.Random.Range(0, pattern.Length);
        Collider2D collider = self.baronessPhase1.shootPoint.GetComponent<Collider2D>();
        for (; ; )
        {
            float timeShoot;
            Parser.FloatTryParse(pattern[self.timeIndex], out timeShoot);
            yield return CupheadTime.WaitForSeconds(self, timeShoot);
            self.baronessPhase1.shotEnough = false;
            if (self.castleOpen)
            {
                yield return self.animator.WaitForAnimationToEnd(self, "Castle_Close", false, true);
            }
            while (BaronessLevelCastle.CURRENT_MINI_BOSS.bossId == BaronessLevelCastle.BossPossibility.Waffle)//new start
            {
                yield return null;
            }//new end
            self.baronessPoppedUp = true;
            AudioManager.Play("level_baroness_stick_head_open");
            self.baronessPhase1.animator.Play("Baroness_Pop_Up");
            while (self.baronessPoppedUp)
            {
                collider.enabled = true;
                if ((float)self.baronessPhase1.shootCounter >= p.attackCount.RandomFloat() || self.baronessPhase1.shotEnough)
                {
                    break;
                }
                self.baronessPhase1.animator.SetTrigger("ToShoot");
                yield return CupheadTime.WaitForSeconds(self, p.attackDelay);
                yield return null;
            }
            self.baronessPoppedUp = false;
            collider.enabled = false;
            self.baronessPhase1.shootCounter = 0;
            AudioManager.Play("level_baroness_stick_head_closed");
            self.baronessPhase1.animator.SetTrigger("Leave");
            if (self.timeIndex < pattern.Length - 1)
            {
                self.timeIndex++;
            }
            else
            {
                self.timeIndex = 0;
            }
            yield return null;
        }
    }
}
