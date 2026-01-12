using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CupaGoovno;

public class Flower
{
    public void Init()
    {
        new FlowerEnemySeed().Init();
        new FlowerMiniFlowerSpawn().Init();
        new FlowerVenusSpawn().Init();
        On.FlowerLevel.nextPattern_cr += nextPattern_cr;
        On.FlowerLevel.OnLevelStart += OnLevelStart;
        On.FlowerLevel.OnDestroy += OnDestroy;
        On.FlowerLevelFlower.PollenShotEnd += PollenShotEnd;
        On.FlowerLevelFlower.launchPollen += launchPollen;
        On.FlowerLevelFlower.SpawnEnemySeed += SpawnEnemySeed;
        On.FlowerLevelFlower.PhaseTwoTrigger += PhaseTwoTrigger;
    }

    private IEnumerator nextPattern_cr(On.FlowerLevel.orig_nextPattern_cr orig, FlowerLevel self)
    {
			yield return self.StartCoroutine(self.gattlingGun_cr());
    }

    protected void OnLevelStart(On.FlowerLevel.orig_OnLevelStart orig, FlowerLevel self)
    {
        orig(self);
        CupheadRenderer.Instance.TouchFuzzy(30f, 10f, 2137f);//15,8
        SpawnSeedsOnStart(self.flower);
        phase2 = false;
    }

    protected void OnDestroy(On.FlowerLevel.orig_OnDestroy orig, FlowerLevel self)
    {
        orig(self);
        FlowerEffect.StopFuzzy(CupheadRenderer.Instance);//new
    }

    private void PollenShotEnd(On.FlowerLevelFlower.orig_PollenShotEnd orig, FlowerLevelFlower self)
    {
        //self.currentPollenShot.StartMoving();
    }

    private void launchPollen(On.FlowerLevelFlower.orig_launchPollen orig, FlowerLevelFlower self)
    {
        string[] pollenStrings = self.properties.CurrentState.pollenSpit.pollenType.Split(new char[]{','});
        string text = pollenStrings[self.currentPollenType];
        int type;
        if (text[0].Equals('R'))
        {
            type = 0;
        }
        else
        {
            type = 1;
        }
        self.currentPollenType++;
        if (self.currentPollenType >= pollenStrings.Length)
        {
            self.currentPollenType = 0;
        }

        List<GameObject> gameObjectList = new List<GameObject>();//new start
        List<FlowerLevelPollenProjectile> pollenList = new List<FlowerLevelPollenProjectile>();
        List<Vector3> moveVectors = new List<Vector3>() { new Vector3(0, -25), new Vector3(0, 225) };
        Vector3 originalPosition = new Vector3(0, 0) + self.topProjectileSpawnPoint.position;
        Vector3 position;
        for (int i = 0; i < 2; i++)
        {
            position = originalPosition + moveVectors[i];
            int type2 = i == 0 ? type : 0;
            self.topProjectileSpawnPoint.position = position;
            gameObjectList.Add(UnityEngine.Object.Instantiate<GameObject>(self.pollenProjectile, self.topProjectileSpawnPoint.position, Quaternion.identity));
            pollenList.Add(gameObjectList[i].GetComponent<FlowerLevelPollenProjectile>());
            pollenList[i].InitPollen((float)self.properties.CurrentState.pollenSpit.pollenSpeed, self.properties.CurrentState.pollenSpit.pollenUpDownStrength, type2, self.topProjectileSpawnPoint);
            pollenList[i].StartMoving();
        }//new end
        /*
        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(self.pollenProjectile, self.topProjectileSpawnPoint.position, Quaternion.identity);
        self.currentPollenShot = gameObject.GetComponent<FlowerLevelPollenProjectile>();
        self.currentPollenShot.InitPollen((float)self.properties.CurrentState.pollenSpit.pollenSpeed, self.properties.CurrentState.pollenSpit.pollenUpDownStrength, type, self.topProjectileSpawnPoint);
        */
        AudioManager.Play("flower_phase2_spit_projectile");
        self.attackCount++;
        if (self.attackCount > self.attackCountTarget)
        {
            self.animator.SetBool("OnPollenAttack", false);
            self.attackCount = 1;
            self.projectileSpawned = false;
        }
        else
        {
            self.projectileSpawned = true;
        }
    }

    public void PhaseTwoTrigger(On.FlowerLevelFlower.orig_PhaseTwoTrigger orig, FlowerLevelFlower self)
    {
        orig(self);
        phase2 = true;
    }

    private void SpawnEnemySeed(On.FlowerLevelFlower.orig_SpawnEnemySeed orig, FlowerLevelFlower self, int xPos, char t, bool a = true)
    {
        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(self.enemySeedPrefab);
        gameObject.transform.position = new Vector3((float)(-700 + xPos), (float)Level.Current.Height, 0f);//new
        //gameObject.transform.position = new Vector3((float)(-600 + xPos), (float)Level.Current.Height, 0f);
        gameObject.GetComponent<FlowerLevelEnemySeed>().OnSeedSpawn(self.properties, self, t, a);
    }

    private void SpawnEnemySeed(FlowerLevelFlower self, int xPos, int yPos, char t, bool a = true)//new
    {
        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(self.enemySeedPrefab);
        gameObject.transform.position = new Vector3((float)(-700 + xPos), (float)yPos, 0f);
        gameObject.GetComponent<FlowerLevelEnemySeed>().OnSeedSpawn(self.properties, self, t, a);
    }

    private void SpawnSeedsOnStart(FlowerLevelFlower self)//new
    {
        int seedsCount = 0;
        int seedsCountMax = 10;
        int yTop = 500;
        int yDelta = 50;
        for(; ; )
        {
		    self.currentGattlingGunAttackPattern.Clear();
            string[] projectileAttributes = self.properties.CurrentState.gattlingGun.seedSpawnString[self.currentGattlingGunAttackString].Split(new char[]{','});
            projectileAttributes.Shuffle<string>();
            for (int j = 0; j < projectileAttributes.Length; j++)
            {
                string[] array = projectileAttributes[j].Split(new char[] { '-' });
                if (array.Length > 1)
                {
                    self.AddAttackTypes(array);
                }
                else
                {
                    self.currentGattlingGunAttackPattern.Add(projectileAttributes[j]);
                }
                self.currentGattlingGunAttackPattern.Add("D" + self.properties.CurrentState.gattlingGun.fallingSeedDelay.ToStringInvariant());
            }
            for (int i = 0; i < self.currentGattlingGunAttackPattern.Count; i++)
            {
                char t = self.currentGattlingGunAttackPattern[i][0];
                if(t != 'D')
                {
                    int x = Parser.IntParse(self.currentGattlingGunAttackPattern[i].Substring(1));
                    int y = yTop - seedsCount * yDelta;
                    bool a = t != 'C' || !self.miniFlowerSpawned;
                    SpawnEnemySeed(self, x, y, t, a);
                    if (t == 'C')
                    {
                        self.miniFlowerSpawned = true;
                    }
                    seedsCount++;
                    if (seedsCount >= seedsCountMax)
                    {
                        return;
                    }
                }
            }
        }
    }

    public static bool phase2;
}
