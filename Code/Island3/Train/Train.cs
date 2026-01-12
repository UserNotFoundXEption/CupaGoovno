using System.Collections;
using UnityEngine;

namespace CupaGoovno;

public class Train
{
    public void Init()
    {
        new TrainPumpkinProjectile().Init();
        new TrainLollipopGhoulsManager().Init();
        new TrainEngineBoss().Init();
        new TrainPlatform().Init();
        new TrainLollipopGhoul().Init();
        new TrainDropperProjectile().Init();
        On.TrainLevel.Start += Start;
        On.TrainLevel.setPhase += setPhase;
    }

    protected void Start(On.TrainLevel.orig_Start orig, TrainLevel self)
    {
        orig(self);
        extraSoapCoroutine = self.StartCoroutine(extraSoap_cr(self));
        TrainLollipopGhoulsManager.parent = self;
        player = PlayerManager.GetFirst();
    }

    private void setPhase(On.TrainLevel.orig_setPhase orig, TrainLevel self, int phase)
    {
        orig(self, phase);
        if(phase == 2)
        {
            self.StopCoroutine(extraSoapCoroutine);
            dropperCoroutine = self.StartCoroutine(dropper_cr(self));
        }
        if(phase == 3)
        {
            self.StopCoroutine(dropperCoroutine);
        }
        if(phase == 4)
        {
            self.StartCoroutine(ufo_cr(self));
            self.StartCoroutine(platformBullshit_cr(self));
        }
    }

    private IEnumerator extraSoap_cr(TrainLevel self)//new
    {
        bool horizontal = false;
        for(; ; )
        {
            yield return CupheadTime.WaitForSeconds(self, 6f);
            TrainLevelPumpkinProjectile soap = self.pumpkinPrefab.brickPrefab.Create() as TrainLevelPumpkinProjectile;
            if (horizontal)
            {
                TrainPumpkinProjectile.horizontalDic[soap] = true;
            }
            else
            {
                TrainPumpkinProjectile.diagonalDic[soap] = true;
            }
            horizontal = !horizontal;
        }
    }

    private IEnumerator dropper_cr(TrainLevel self)//new
    {
        for(; ; )
        {
            yield return CupheadTime.WaitForSeconds(self, 1f);
            Vector2 pos = player.transform.position;
            while(Mathf.Abs(pos.x - player.transform.position.x) < 200f)
            {
                pos = new Vector2(UnityEngine.Random.Range(-600f, 600f), 400f);
            }
            TrainLevelEngineBossDropperProjectile proj = self.engine.dropperPrefab.Create(pos, self.properties.CurrentState.engine.projectileUpSpeed, self.properties.CurrentState.engine.projectileXSpeed, self.properties.CurrentState.engine.projectileGravity);
            SpriteRenderer spriteRenderer = proj.GetComponent<SpriteRenderer>();
            if(spriteRenderer != null)
            {
                spriteRenderer.sortingLayerName = "UI";
                spriteRenderer.sortingOrder = int.MaxValue;
            }
        }
    }

    private IEnumerator ufo_cr(TrainLevel self)//new
    {
        yield return CupheadTime.WaitForSeconds(self, 5f);
        float delay = 2.5f;
        LevelProperties.FlyingBlimp.UFO p = YoMamaFat.ufoProperties;
        string[] typePattern = p.UFOString.GetRandom<string>().Split(new char[] { ',' });
        int index = UnityEngine.Random.Range(0, typePattern.Length);
        bool left = false;
        for (; ; )
        {
            if (typePattern[index][0] == 'A')
            {
                SpawnUFO(YoMamaFat.ufoA, left);
            }
            else if (typePattern[index][0] == 'B')
            {
                SpawnUFO(YoMamaFat.ufoB, left);
            }
            yield return CupheadTime.WaitForSeconds(self, delay);
            if (index < typePattern.Length - 1)
            {
                index++;
            }
            else
            {
                index = 0;
            }
            left = !left;
        }
    }

    private void SpawnUFO(FlyingBlimpLevelUFO prefab, bool left)//new
    {
        LevelProperties.FlyingBlimp.UFO p = YoMamaFat.ufoProperties;
        float startX = left ? -700f : 700f;
        float speed = left ? -p.UFOSpeed : p.UFOSpeed;
        FlyingBlimpLevelUFO flyingBlimpLevelUFO = UnityEngine.Object.Instantiate<FlyingBlimpLevelUFO>(prefab);
        flyingBlimpLevelUFO.Init(new Vector3(startX, 300f), new Vector3(startX, 300f), new Vector3(startX, 300f), speed, p.UFOHP, p);
    }

    private IEnumerator platformBullshit_cr(TrainLevel self)
    {
        for (; ; )
        {
            if(player == null)
            {
                yield break;
            }
            if(player.transform.position.x > platform.transform.position.x)
            {
                platform.OnRight();
            }
            else
            {
                platform.OnLeft();
            }
            yield return CupheadTime.WaitForSeconds(self, 0.1f);
        }
    }

    public static TrainLevelPlatform platform;
    Coroutine extraSoapCoroutine;
    Coroutine dropperCoroutine;
    AbstractPlayerController player;
}
