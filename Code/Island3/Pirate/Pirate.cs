using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CupaGoovno;

public class Pirate
{
    public void Init()
    {
        new PirateSquid().Init();
        new PirateDogFish().Init();
        new PirateBoat().Init();
        new PirateBarrel().Init();
        On.PirateLevel.Start += Start;
        On.PirateLevel.shark_cr += shark_cr;
        On.PirateLevel.dogFish_cr += dogFish_cr;
        On.PirateLevel.peashot_cr += peashot_cr;
    }

    protected void Start(On.PirateLevel.orig_Start orig, PirateLevel self)
    {
        orig(self);
        self.StartCoroutine(self.squid_cr());//new
        self.StartCoroutine(self.dogFish_cr());//new
        self.StartCoroutine(self.peashot_cr());//new
    }

    private IEnumerator shark_cr(On.PirateLevel.orig_shark_cr orig, PirateLevel self)
    {
        //self.Whistle(PirateLevel.Creature.Shark);
        //yield return CupheadTime.WaitForSeconds(self, 1.5f);
        yield return CupheadTime.WaitForSeconds(self, self.properties.CurrentState.shark.startDelay);
        PirateLevelShark shark = self.prefabs.shark.InstantiatePrefab<PirateLevelShark>();
        shark.LevelInitWithGroup(self.properties.CurrentState.shark);
        while (shark.state != PirateLevelShark.State.Complete)
        {
            if (self.properties.CurrentState.stateName == LevelProperties.Pirate.States.Boat)//new start
            {
                yield break;
            }//new end
            yield return null;
        }
        yield return CupheadTime.WaitForSeconds(self, self.properties.CurrentState.shark.endDelay);
        yield break;
    }

    private IEnumerator dogFish_cr(On.PirateLevel.orig_dogFish_cr orig, PirateLevel self)
    {
        bool secretHitBox = false;
        self.Whistle(PirateLevel.Creature.DogFish);
        yield return CupheadTime.WaitForSeconds(self, 1.5f);
        self.scope.In();
        yield return CupheadTime.WaitForSeconds(self, self.properties.CurrentState.dogFish.startDelay);
        LevelProperties.Pirate.DogFish properties = self.properties.CurrentState.dogFish;
        for (int i = 0; i < 2137; i++)//new
        //for (int i = 0; i < properties.count; i++)
        {
            secretHitBox = (i == 3);
            /*PirateLevelDogFish dogFish = self.prefabs.dogFish.InstantiatePrefab<PirateLevelDogFish>();
            dogFish.transform.SetPosition(new float?(0f), new float?(-210f), new float?(0f));
            dogFish.Init(self.properties, secretHitBox);*/
            for (int j = 0; j < 2; j++)//new start
            {
                PirateLevelDogFish dogFish = self.prefabs.dogFish.InstantiatePrefab<PirateLevelDogFish>();
                dogFish.transform.SetPosition(new float?(0f), new float?(-210f), new float?(0f));
                dogFish.Init(self.properties, secretHitBox);
                yield return CupheadTime.WaitForSeconds(self, 1f);
            }//new end
            yield return CupheadTime.WaitForSeconds(self, properties.nextFishDelay);
        }
        yield return CupheadTime.WaitForSeconds(self, properties.endDelay);
        yield break;
    }

    private IEnumerator peashot_cr(On.PirateLevel.orig_peashot_cr orig, PirateLevel self)
    {
        for (; ; )
        {
            yield return orig(self);
        }
    }
}
