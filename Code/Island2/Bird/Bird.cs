using System.Collections;
using UnityEngine;

namespace CupaGoovno;

public class Bird
{
    public void Init()
    {
        new BirdSmall().Init();
        new BirdEnemy().Init();
        new BirdEgg().Init();
        On.FlyingBirdLevel.OnDestroy += OnDestroy;
        On.FlyingBirdLevelBird.OnDamageTaken += OnDamageTaken;
        On.FlyingBirdLevelBird.Awake += Awake;
        On.FlyingBirdLevelBird.OnStateChange += OnStateChange;
        On.FlyingBirdLevelBird.FireLasers += FireLasers;
        On.FlyingBirdLevelBird.FireEgg += FireEgg;
        On.FlyingBirdLevelBird.stretcherMove_cr += stretcherMove_cr;
    }

    private void OnDamageTaken(On.FlyingBirdLevelBird.orig_OnDamageTaken orig, FlyingBirdLevelBird self, DamageDealer.DamageInfo info)
    {
        if (info.damageSource == DamageDealer.DamageSource.Super || info.damageSource == DamageDealer.DamageSource.Ex)
        {
            orig(self, info);
        }
    }

    protected void Awake(On.FlyingBirdLevelBird.orig_Awake orig, FlyingBirdLevelBird self)
    {
        orig(self);
        wallCr = self.StartCoroutine(wall_block_cr(self));//new
    }

    private void OnStateChange(On.FlyingBirdLevelBird.orig_OnStateChange orig, FlyingBirdLevelBird self)
    {
        orig(self);
        switch (self.properties.CurrentState.stateName)
        {
            case LevelProperties.FlyingBird.States.Whistle:
                self.StartCoroutine(ufo_cr(self));
                self.StopCoroutine(wallCr);
                break;
            case LevelProperties.FlyingBird.States.HouseDeath:
                Dragonfly.enter = true;
                break;
            case LevelProperties.FlyingBird.States.BirdRevival:
                YoMamaFat.dragonfly.transform.position = new Vector3(2137f, 2137f);
                UnityEngine.Object.Destroy(YoMamaFat.dragonfly.gameObject);
                YoMamaFat.dragonfly = null;
                break;
            default:
                break;
        }
    }

    private void FireLasers(On.FlyingBirdLevelBird.orig_FireLasers orig, FlyingBirdLevelBird self)
    {
        AudioManager.Play("level_flyingbird_bird_laser_fire");
        self.emitAudioFromObject.Add("level_flyingbird_bird_laser_fire");
        self.laserEffect.Create(self.laserRoots[0].position);
        float angleDelta = UnityEngine.Random.Range(-30f, 30f);//new
        foreach (Transform transform in self.laserRoots)
        {
            self.laserPrefab.Create(transform.position, -transform.eulerAngles.z + angleDelta, -self.properties.CurrentState.lasers.speed);//new
            //self.laserPrefab.Create(transform.position, -transform.eulerAngles.z, -self.properties.CurrentState.lasers.speed);
        }
    }

    private void FireEgg(On.FlyingBirdLevelBird.orig_FireEgg orig, FlyingBirdLevelBird self)
    {
        //this.eggPrefab.Create(base.properties.CurrentState.feathers.speed, this.eggRoot.position);
        FlyingBirdLevelBirdEgg egg = self.eggPrefab.Create(self.properties.CurrentState.feathers.speed, self.eggRoot.position) as FlyingBirdLevelBirdEgg;//new
        BirdEgg.eggPrefabDic[egg] = self.eggPrefab;//new
    }

    private IEnumerator stretcherMove_cr(On.FlyingBirdLevelBird.orig_stretcherMove_cr orig, FlyingBirdLevelBird self)
    {
        bool movingRight = Rand.Bool();
        float time = self.properties.CurrentState.bigBird.speedXTime;
        float end = 0f;
        do
        {
            //if (self.state != FlyingBirdLevelBird.State.Heart)
            //{
            float t = 0f;
            float start = self.transform.position.x;
            if (movingRight)
            {
                end = 290f;
            }
            else
            {
                end = -240f;
            }
            while (t < time)
            {
                //if (self.state != FlyingBirdLevelBird.State.Heart)
                //{
                float value = t / time;
                self.transform.SetPosition(new float?(EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, start, end, value)), null, null);
                t += CupheadTime.Delta;
                //}
                yield return null;
            }
            self.transform.SetPosition(new float?(end), null, null);
            movingRight = !movingRight;
            FireWall(self, !movingRight);//new
                                   //}
            yield return null;
        }
        while (self.properties.CurrentHealth > 0f);
        yield break;
    }

    protected void OnDestroy(On.FlyingBirdLevel.orig_OnDestroy orig, FlyingBirdLevel self)
    {
        orig(self);
        if(YoMamaFat.dragonfly != null)
        {
            YoMamaFat.dragonfly.transform.position = new Vector3(2137f, 2137f);
            UnityEngine.Object.Destroy(YoMamaFat.dragonfly.gameObject);
            YoMamaFat.dragonfly = null;
        }
    }

    private IEnumerator wall_block_cr(FlyingBirdLevelBird self)//new
    {
        yield return CupheadTime.WaitForSeconds(self, 5f);
        for (; ; )
        {
            self.laserPrefab.Create(new Vector2(650f, 350f), 0f, -self.properties.CurrentState.lasers.speed);
            self.laserPrefab.Create(new Vector2(650f, -350f), 0f, -self.properties.CurrentState.lasers.speed);
            yield return CupheadTime.WaitForSeconds(self, 1f);
        }
    }

    private IEnumerator ufo_cr(FlyingBirdLevelBird self)//new
    {
        LevelProperties.FlyingBlimp.UFO p = YoMamaFat.ufoProperties;
        string[] typePattern = p.UFOString.GetRandom<string>().Split(new char[] { ',' });
        int index = UnityEngine.Random.Range(0, typePattern.Length);
        for (; ; )
        {
            if (typePattern[index][0] == 'A')
            {
                SpawnUFO(YoMamaFat.ufoA);
            }
            else if (typePattern[index][0] == 'B')
            {
                SpawnUFO(YoMamaFat.ufoB);
            }
            yield return CupheadTime.WaitForSeconds(self, p.UFODelay);
            if (index < typePattern.Length - 1)
            {
                index++;
            }
            else
            {
                index = 0;
            }
        }
    }

    private void SpawnUFO(FlyingBlimpLevelUFO prefab)//new
    {
        LevelProperties.FlyingBlimp.UFO p = YoMamaFat.ufoProperties;
        FlyingBlimpLevelUFO flyingBlimpLevelUFO = UnityEngine.Object.Instantiate<FlyingBlimpLevelUFO>(prefab);
        flyingBlimpLevelUFO.Init(new Vector3(700f, 300f), new Vector3(699f, 300f), new Vector3(698f, 300f), p.UFOSpeed, p.UFOHP, p);
    }

    private void FireWall(FlyingBirdLevelBird self, bool left)//new
    {
        float x = 650f;
        float speed = -self.properties.CurrentState.lasers.speed;
        float rotation = 0f;
        if (!left)
        {
            x = -x;
            rotation = 180f;
        }
        self.laserPrefab.Create(new Vector2(x, 350f), rotation, speed);
        self.laserPrefab.Create(new Vector2(x, 270f), rotation, speed);
        self.laserPrefab.Create(new Vector2(x, 190f), rotation, speed);
        self.laserPrefab.Create(new Vector2(x, 110f), rotation, speed);
        self.laserPrefab.Create(new Vector2(x, 30f), rotation, speed);
    }

    public static IEnumerator warning_cr(FlyingBirdLevel self)//new
    {
        if (!showedWarning)
        {
            yield return CupheadTime.WaitForSeconds(self, 1f);
            string tText = "WARNING";
            string mText = "Wally receives damage only from Ex and Super attacks!";
            self.StartCoroutine(Other.notification_cr(self, tText, mText, 5f, 125f, false));
            showedWarning = true;
        }
    }

    Coroutine wallCr;
    static bool showedWarning = false;
}
