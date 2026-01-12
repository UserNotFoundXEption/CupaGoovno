using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class Genie
{
    public void Init()
    {
        new GenieHead().Init();
        new GenieHelixProjectile().Init();
        new GenieTransform().Init();
        new GenieGoop().Init();
        new GeniePyramid().Init();
        On.FlyingGenieLevel.Awake += Awake;
        On.FlyingGenieLevelGenie.Awake += Awake;
        On.FlyingGenieLevelGenie.StartTreasure += StartTreasure;
        On.FlyingGenieLevelGenie.StartSphinx += StartSphinx;
        On.FlyingGenieLevelGenie.StartGems += StartGems;
        On.FlyingGenieLevelGenie.StartSwords += StartSwords;
        On.FlyingGenieLevelGenie.swords_cr += swords_cr;
        On.FlyingGenieLevelGenie.StartObelisk += StartObelisk;
        On.FlyingGenieLevelGenie.obelisk_cr += obelisk_cr;
        On.FlyingGenieLevelGenie.gems_cr += gems_cr;
        On.FlyingGenieLevelGenie.small_gems_cr += small_gems_cr;
        On.FlyingGenieLevelGenie.big_gems_cr += big_gems_cr;
        On.FlyingGenieLevelGenie.coffin_cr += coffin_cr;
    }

    protected void Awake(On.FlyingGenieLevel.orig_Awake orig, FlyingGenieLevel self)
    {
        orig(self);
        GameObject.Destroy(GameObject.Find("TreeSpawner").gameObject);
    }
    
    protected void Awake(On.FlyingGenieLevelGenie.orig_Awake orig, FlyingGenieLevelGenie self)
    {
        orig(self);
        self.transform.AddPosition(150f, 0f, 0f);//new
    }

    public void StartTreasure(On.FlyingGenieLevelGenie.orig_StartTreasure orig, FlyingGenieLevelGenie self)
    {
        self.state = FlyingGenieLevelGenie.State.Treasure;
        self.skullCounter = 0;
        self.animator.SetBool("OnTreasure", true);
        self.attackLooping = true;
        int num = UnityEngine.Random.Range(0, self.treasureAttacks.Count);
        self.treasureCounter = self.treasureAttacks[num];
        /*
        switch (self.treasureCounter)
        {
        case 0:
            self.StartSwords();
            break;
        case 1:
            self.StartGems();
            break;
        case 2:
            self.StartSphinx();
            break;
        default:
            global::Debug.LogError("The counter is messed up: " + self.treasureCounter, null);
            break;
        }*/
        self.StartSwords();//new start
        self.StartGems();
        self.StartSphinx();//new end
        self.treasureAttacks.Remove(num);
    }

    public void StartSphinx(On.FlyingGenieLevelGenie.orig_StartSphinx orig, FlyingGenieLevelGenie self)
    {
        /*if (self.patternCoroutine1 != null)
        {
            self.StopCoroutine(self.patternCoroutine1);
        }*/
        sphinxCoroutine = self.StartCoroutine(self.sphinx_cr());
    }

    public void StartGems(On.FlyingGenieLevelGenie.orig_StartGems orig, FlyingGenieLevelGenie self)
    {/*
		if (self.patternCoroutine1 != null)
		{
			self.StopCoroutine(self.patternCoroutine1);
		}*/
        gemsCoroutine = self.StartCoroutine(self.gems_cr());
    }

    public void StartSwords(On.FlyingGenieLevelGenie.orig_StartSwords orig, FlyingGenieLevelGenie self)
    {/*
		if (self.patternCoroutine1 != null)
		{
			self.StopCoroutine(self.patternCoroutine1);
		}*/
        swordsCoroutine = self.StartCoroutine(self.swords_cr());
    }

    private IEnumerator swords_cr(On.FlyingGenieLevelGenie.orig_swords_cr orig, FlyingGenieLevelGenie self)
    {
        self.attackLooping = true;
        yield return self.animator.WaitForAnimationToEnd(self, "Chest_Intro", false, true);
        LevelProperties.FlyingGenie.Swords p = self.properties.CurrentState.swords;
        int positionIndex = UnityEngine.Random.Range(0, p.patternPositionStrings.Length);
        string[] positionPattern = p.patternPositionStrings[positionIndex].Split(new char[]
        {
            ','
        });
        Vector3 endPosition = Vector3.zero;
        float location = 0f;
        while (self.attackLooping)
        {
            positionPattern = p.patternPositionStrings[positionIndex].Split(new char[]
            {
                ','
            });
            for (int i = 0; i < positionPattern.Length; i++)
            {
                string[] coordinates = positionPattern[i].Split(new char[]
                {
                    '-'
                });
                for (int j = 0; j < coordinates.Length; j++)
                {
                    Parser.FloatTryParse(coordinates[j], out location);
                    if (j % 2 == 0)
                    {
                        endPosition.x = -640f + location;
                    }
                    else
                    {
                        endPosition.y = 360f - location;
                    }
                }
                self.SpawnSwords(endPosition);
                yield return CupheadTime.WaitForSeconds(self, p.spawnDelay);//new
                if (!self.attackLooping)
                {
                    break;
                }
                //yield return CupheadTime.WaitForSeconds(self, p.spawnDelay);
            }
            if (!self.attackLooping)
            {
                break;
            }
            yield return CupheadTime.WaitForSeconds(self, p.repeatDelay);
            positionIndex = (positionIndex + 1) % p.patternPositionStrings.Length;
            yield return null;
        }
        self.animator.SetBool("OnTreasure", false);
        yield return self.animator.WaitForAnimationToEnd(self, "Chest_Outro", false, true);
        yield return CupheadTime.WaitForSeconds(self, p.hesitate);
        self.state = FlyingGenieLevelGenie.State.Idle;
        yield return null;
        yield break;
    }

    public void StartObelisk(On.FlyingGenieLevelGenie.orig_StartObelisk orig, FlyingGenieLevelGenie self)
    {
        if (sphinxCoroutine != null)
        {
            self.StopCoroutine(sphinxCoroutine);
        }
        if (swordsCoroutine != null) //new start
        {
            self.StopCoroutine(swordsCoroutine);
        }
        if (gemsCoroutine != null)
        {
            self.StopCoroutine(gemsCoroutine);
        }//new end
        if (self.bigGemsRoutine != null)
        {
            self.StopCoroutine(self.bigGemsRoutine);
        }
        if (self.smallGemsRoutine != null)
        {
            self.StopCoroutine(self.smallGemsRoutine);
        }
        self.state = FlyingGenieLevelGenie.State.Disappear;
        self.animator.SetBool("OnDisappear", true);
        self.StartCoroutine(self.obelisk_cr());
        self.StartCoroutine(self.genie_laugh_sound_cr());
    }

    private IEnumerator obelisk_cr(On.FlyingGenieLevelGenie.orig_obelisk_cr orig, FlyingGenieLevelGenie self)
    {
        LevelProperties.FlyingGenie.Obelisk p = self.properties.CurrentState.obelisk;
        self.attackLooping = true;
        Vector3 startPos = Vector3.zero;
        startPos.x = 1340f;
        startPos.y = 360f;
        float t = 0f;
        float time = 1f;
        float angle = 0f;
        bool firstPillar = true;
        self.obelisks = new List<FlyingGenieLevelObelisk>();
        int obelisksListIndex = 0;
        int obeliskPoolSize = 6;
        int obeliskCounter = 0;
        int mainObeliskIndex = UnityEngine.Random.Range(0, p.obeliskGeniePos.Length);
        string[] blockOrderPattern = p.obeliskGeniePos[mainObeliskIndex].Split(new char[]
        {
        ','
        });
        int obeliskIndex = UnityEngine.Random.Range(0, blockOrderPattern.Length);
        int mainBouncerIndex = UnityEngine.Random.Range(0, p.bouncerAngleString.Length);
        string[] bouncerPattern = p.bouncerAngleString[mainBouncerIndex].Split(new char[]
        {
        ','
        });
        int bouncerIndex = UnityEngine.Random.Range(0, bouncerPattern.Length);
        for (int i = 0; i < obeliskPoolSize; i++)
        {
            FlyingGenieLevelObelisk flyingGenieLevelObelisk = UnityEngine.Object.Instantiate<FlyingGenieLevelObelisk>(self.obeliskPrefab);
            flyingGenieLevelObelisk.Init(startPos, p, self, i == 0);
            self.obelisks.Add(flyingGenieLevelObelisk);
        }
        yield return self.animator.WaitForAnimationToStart(self, "Genie_Meditate", false);
        self.sawMask.gameObject.SetActive(true);
        while (t < time)
        {
            Vector3 pos = self.hieroBG.position;
            Vector3 pos2 = self.brickBG.position;
            float val = EaseUtils.Ease(EaseUtils.EaseType.easeInBounce, 0f, 1f, t / time);
            pos.y = Mathf.Lerp(self.hieroBG.position.y, 340f, val);
            pos2.y = Mathf.Lerp(self.brickBG.position.y, -320f, val);
            self.hieroBG.position = pos;
            self.brickBG.position = pos2;
            t += CupheadTime.Delta;
            yield return null;
        }
        self.treasureRoot.AddPosition(500f, 0f, 0f);//new
        self.StartGems();//new
        while (obeliskCounter < p.obeliskCount)
        {
            Parser.FloatTryParse(bouncerPattern[bouncerIndex], out angle);
            string[] headLocations = blockOrderPattern[obeliskIndex].Split(new char[]
            {
            '-'
            });
            self.obelisks[obelisksListIndex].ActivateObelisk(headLocations);
            if (p.bounceShotOn)
            {
                if (!firstPillar)
                {
                    int index;
                    if (obelisksListIndex <= 0)
                    {
                        index = self.obelisks.Count - 1;
                    }
                    else
                    {
                        index = obelisksListIndex - 1;
                    }
                    self.SpawnBouncer(self.obelisks[obelisksListIndex], self.obelisks[index], angle);
                }
                else
                {
                    float num = Vector3.Distance(self.obelisks[obelisksListIndex + 1].transform.position, self.transform.position);
                    self.obelisks[obelisksListIndex].SetColliders((self.obelisks[obelisksListIndex + 1].transform.position.x + Mathf.Abs(num / 2f)) / 2f, self.transform.position.x - num / 2f);
                    firstPillar = false;
                }
            }
            obelisksListIndex = (obelisksListIndex + 1) % self.obelisks.Count;
            yield return null;
            yield return CupheadTime.WaitForSeconds(self, p.obeliskAppearDelay);
            if (obeliskIndex < blockOrderPattern.Length - 1)
            {
                obeliskIndex++;
            }
            else
            {
                mainObeliskIndex = (mainObeliskIndex + 1) % p.obeliskGeniePos.Length;
                obeliskIndex = 0;
            }
            if (bouncerIndex < bouncerPattern.Length - 1)
            {
                bouncerIndex++;
            }
            else
            {
                mainBouncerIndex = (mainBouncerIndex + 1) % p.bouncerAngleString.Length;
                bouncerIndex = 0;
            }
            blockOrderPattern = p.obeliskGeniePos[mainObeliskIndex].Split(new char[]
            {
            ','
            });
            bouncerPattern = p.bouncerAngleString[mainBouncerIndex].Split(new char[]
            {
            ','
            });
            obeliskCounter++;
            yield return null;
        }
        foreach (FlyingGenieLevelObelisk obelisk in self.obelisks)
        {
            if (obelisk.isOn)
            {
                while (obelisk.transform.position.x > -640f)
                {
                    yield return null;
                }
            }
        }
        AudioManager.Stop("genie_pillar_main_loop");
        AudioManager.Stop("genie_pillar_destructable_loop");
        self.sawMask.gameObject.SetActive(false);
        self.StartCoroutine(self.delete_obelisks_cr(self.obelisks));
        self.state = FlyingGenieLevelGenie.State.Idle;
        self.StartCoffin();
        if (gemsCoroutine != null)//new start
        {
            self.StopCoroutine(gemsCoroutine);
        }//new end
        yield return null;
        yield break;
    }

    private IEnumerator gems_cr(On.FlyingGenieLevelGenie.orig_gems_cr orig, FlyingGenieLevelGenie self)
    {
        self.attackLooping = true;
        if (self.state != FlyingGenieLevelGenie.State.Disappear)//new
        {//new
            yield return self.animator.WaitForAnimationToEnd(self, "Chest_Intro", false, true);
            AudioManager.Play("genie_chest_jewel_escape");
            self.emitAudioFromObject.Add("genie_chest_jewel_escape");
            AudioManager.PlayLoop("genie_chest_magic_loop");
            self.emitAudioFromObject.Add("genie_chest_magic_loop");
        }//new
        while (self.attackLooping)
        {
            self.smallGemTimerUp = false;
            self.bigGemTimerUp = false;
            if (self.bigGemsRoutine != null)
            {
                self.StopCoroutine(self.bigGemsRoutine);
            }
            self.bigGemsRoutine = self.StartCoroutine(self.big_gems_cr());
            if (self.smallGemsRoutine != null)
            {
                self.StopCoroutine(self.smallGemsRoutine);
            }
            self.smallGemsRoutine = self.StartCoroutine(self.small_gems_cr());
            while (!self.smallGemTimerUp && !self.bigGemTimerUp)
            {
                if (!self.attackLooping)
                {
                    break;
                }
                yield return null;
            }
            if (self.attackLooping)
            {
                yield return CupheadTime.WaitForSeconds(self, self.properties.CurrentState.gems.repeatDelay);
            }
            yield return null;
        }
        self.animator.SetBool("OnTreasure", false);
        yield return self.animator.WaitForAnimationToStart(self, "Chest_Outro", false);
        AudioManager.Stop("genie_chest_magic_loop");
        AudioManager.Play("genie_chest_magic_loop_end");
        self.emitAudioFromObject.Add("genie_chest_magic_loop_end");
        yield return self.animator.WaitForAnimationToEnd(self, "Chest_Outro", false, true);
        yield return CupheadTime.WaitForSeconds(self, self.properties.CurrentState.gems.hesitate);
        if (self.state != FlyingGenieLevelGenie.State.Disappear)//new
        {//new
            self.state = FlyingGenieLevelGenie.State.Idle;
        }//new
        yield return null;
        yield break;
    }

    private IEnumerator small_gems_cr(On.FlyingGenieLevelGenie.orig_small_gems_cr orig, FlyingGenieLevelGenie self)
    {
        LevelProperties.FlyingGenie.Gems p = self.properties.CurrentState.gems;
        self.smallGemTimerUp = false;
        int mainOffsetIndex = UnityEngine.Random.Range(0, p.gemSmallAimOffset.Length);
        string[] smallOffsetString = p.gemSmallAimOffset[mainOffsetIndex].Split(new char[]
        {
        ','
        });
        int offsetIndex = UnityEngine.Random.Range(0, smallOffsetString.Length);
        float offset = 0f;
        self.StartCoroutine(self.small_gem_timer_cr());
        while (!self.smallGemTimerUp && self.attackLooping)
        {
            smallOffsetString = p.gemSmallAimOffset[mainOffsetIndex].Split(new char[]
            {
            ','
            });
            Parser.FloatTryParse(smallOffsetString[offsetIndex], out offset);
            AbstractPlayerController player = PlayerManager.GetNext();
            //self.gemPrefab.Create(self.treasureRoot.position, player, offset, p.gemSmallSpeed, self.gemPinkPattern[self.gemPinkIndex][0] == 'P', false);
            FlyingGenieLevelGem gem = self.gemPrefab.Create(self.treasureRoot.position, player, offset, p.gemSmallSpeed, self.gemPinkPattern[self.gemPinkIndex][0] == 'P', false);//new start
            if (self.properties.CurrentState.stateName == LevelProperties.FlyingGenie.States.Disappear)
            {
                gem.transform.SetScale(0.7f, 0.7f, null);
            }//new end
            self.gemPinkIndex = (self.gemPinkIndex + 1) % self.gemPinkPattern.Length;
            yield return CupheadTime.WaitForSeconds(self, p.gemSmallDelayRange.RandomFloat());
            if (offsetIndex < smallOffsetString.Length - 1)
            {
                offsetIndex++;
            }
            else
            {
                mainOffsetIndex = (mainOffsetIndex + 1) % p.gemSmallAimOffset.Length;
                offsetIndex = 0;
            }
        }
        yield return null;
        yield break;
    }

    private IEnumerator big_gems_cr(On.FlyingGenieLevelGenie.orig_big_gems_cr orig, FlyingGenieLevelGenie self)
    {
        LevelProperties.FlyingGenie.Gems p = self.properties.CurrentState.gems;
        self.bigGemTimerUp = false;
        int mainOffsetIndex = UnityEngine.Random.Range(0, p.gemBigAimOffset.Length);
        string[] bigOffsetString = p.gemBigAimOffset[mainOffsetIndex].Split(new char[]
        {
        ','
        });
        int offsetIndex = UnityEngine.Random.Range(0, bigOffsetString.Length);
        float offset = 0f;
        self.StartCoroutine(self.big_gems_timer_cr());
        while (!self.bigGemTimerUp && self.attackLooping)
        {
            Parser.FloatTryParse(bigOffsetString[offsetIndex], out offset);
            AbstractPlayerController player = PlayerManager.GetNext();
            //self.gemPrefab.Create(self.treasureRoot.position, player, offset, p.gemBigSpeed, false, true);
            FlyingGenieLevelGem gem = self.gemPrefab.Create(self.treasureRoot.position, player, offset, p.gemBigSpeed, false, true);//new start
            if (self.properties.CurrentState.stateName == LevelProperties.FlyingGenie.States.Disappear)
            {
                gem.transform.SetScale(0.7f, 0.7f, null);
            }//new end
            yield return CupheadTime.WaitForSeconds(self, p.gemBigDelayRange.RandomFloat());
            if (offsetIndex < bigOffsetString.Length - 1)
            {
                offsetIndex++;
            }
            else
            {
                mainOffsetIndex = (mainOffsetIndex + 1) % p.gemBigAimOffset.Length;
                offsetIndex = 0;
            }
            yield return null;
        }
        yield return null;
        yield break;
    }

    private IEnumerator coffin_cr(On.FlyingGenieLevelGenie.orig_coffin_cr orig, FlyingGenieLevelGenie self)
    {
        Coroutine spamCr = self.StartCoroutine(ghost_spam_cr(self));//new
        yield return orig(self);
        self.StopCoroutine(spamCr);//new
    }


    private IEnumerator ghost_spam_cr(FlyingGenieLevelGenie self)//new
    {
        yield return CupheadTime.WaitForSeconds(self, 5f);
        for (; ; )
        {
            LevelProperties.FlyingGenie.Coffin p = self.properties.CurrentState.coffin;
            self.mummyClassic.Create(new Vector3(500f, 320f), -p.mummyASpeed, 0f, p, FlyingGenieLevelMummy.MummyType.Classic, 2137f, 100);
            self.mummyClassic.Create(new Vector3(500f, -320f), -p.mummyASpeed, 0f, p, FlyingGenieLevelMummy.MummyType.Classic, 2137f, 100);
            yield return CupheadTime.WaitForSeconds(self, 1f);
        }
    }

    Coroutine sphinxCoroutine;
    Coroutine gemsCoroutine;
    Coroutine swordsCoroutine;
}
