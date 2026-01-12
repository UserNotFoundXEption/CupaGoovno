using System.Collections;
using UnityEngine;

namespace CupaGoovno;

public class BlimpMoonLady
{
    public void Init()
    {
        On.FlyingBlimpLevelMoonLady.ufo_cr += ufo_cr;
        On.FlyingBlimpLevelMoonLady.intro_cr += intro_cr;
    }

    private IEnumerator ufo_cr(On.FlyingBlimpLevelMoonLady.orig_ufo_cr orig, FlyingBlimpLevelMoonLady self)
    {
        self.state = FlyingBlimpLevelMoonLady.State.Attack;
        bool reversedUfo = false;//new
        float volume = 0.1f;
        LevelProperties.FlyingBlimp.UFO p = self.properties.CurrentState.uFO;
        string[] typePattern = p.UFOString.GetRandom<string>().Split(new char[]
        {
        ','
        });
        int index = UnityEngine.Random.Range(0, typePattern.Length);
        AudioManager.Play("level_flying_blimp_moon_anticipation");
        self.animator.SetTrigger("To ATK");
        yield return CupheadTime.WaitForSeconds(self, p.moonATKAnticipation);
        self.gears.Play();
        self.gears.volume = volume;
        AudioManager.Play("level_flying_blimp_moon_face_extend");
        self.animator.SetTrigger("Continue");
        yield return self.animator.WaitForAnimationToStart(self, "Moon_Attack", false);//new start
        yield return CupheadTime.WaitForSeconds(self, 0.2f);
        Vector3 newPosition = new Vector3(self.transform.position.x + 400, self.transform.position.y);
        while (self.transform.position != newPosition)
        {
            self.transform.position = Vector3.MoveTowards(self.transform.position, newPosition, 3000 * CupheadTime.FixedDelta);
            yield return new WaitForFixedUpdate();
        }//new end
         //yield return self.animator.WaitForAnimationToEnd(self, "Moon_Attack", false, true);
        self.time = 0f;
        self.startTimer = true;
        self.StartCoroutine(self.timer_cr());
        while (self.time < p.moonATKDuration)
        {
            self.pedal.volume = volume;
            if (volume < 1f)
            {
                volume += 0.1f;
            }
            if (self.state != FlyingBlimpLevelMoonLady.State.Death)
            {
                if (typePattern[index][0] == 'A')
                {
                    SpawnUFO(self, self.ufoPrefabA, reversedUfo);
                }
                else if (typePattern[index][0] == 'B')
                {
                    SpawnUFO(self, self.ufoPrefabB, reversedUfo);
                }
                reversedUfo = !reversedUfo;//new
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
        self.pedal.volume = 1f;
        self.animator.SetTrigger("End");
        self.startTimer = false;
        self.gears.Stop();
        AudioManager.Play("level_flying_blimp_moon_gears_idle");
        yield return self.animator.WaitForAnimationToEnd(self, "Moon_Attack_To_Idle", false, true);
        self.StartCoroutine(self.ufo_attack_handler_cr());
        yield break;
    }

    private IEnumerator intro_cr(On.FlyingBlimpLevelMoonLady.orig_intro_cr orig, FlyingBlimpLevelMoonLady self)
    {
        AudioManager.Play("level_flying_blimp_transform_moon");
        self.state = FlyingBlimpLevelMoonLady.State.Morph;
        LevelProperties.FlyingBlimp.Morph p = self.properties.CurrentState.morph;
        PlanePlayerController playerOne = PlayerManager.GetPlayer<PlanePlayerController>(PlayerId.PlayerOne);
        PlanePlayerController playerTwo = PlayerManager.GetPlayer<PlanePlayerController>(PlayerId.PlayerTwo);
        if (playerOne != null && playerOne.isActiveAndEnabled)
        {
            playerOne.animationController.SetColorOverTime(self.dimColor.GetComponent<SpriteRenderer>().color, 15f);
        }
        if (playerTwo != null && playerTwo.isActiveAndEnabled)
        {
            playerTwo.animationController.SetColorOverTime(self.dimColor.GetComponent<SpriteRenderer>().color, 15f);
        }
        yield return null;
        /*
        while (self.transform.position != self.transformMorphEndPoint.position)
        {
            self.transform.position = Vector3.MoveTowards(self.transform.position, self.transformMorphEndPoint.position, 300f * CupheadTime.Delta);
            yield return CupheadTime.WaitForSeconds(self, 0.1f);
        }*/
        self.transform.position = self.transformMorphEndPoint.position;//new
        yield return CupheadTime.WaitForSeconds(self, p.crazyAHold);
        self.pedal.Stop();
        self.animator.SetTrigger("To B");
        yield return CupheadTime.WaitForSeconds(self, p.crazyBHold);
        self.animator.SetTrigger("End");
        self.StartCoroutine(self.stars_cr());
        yield return self.animator.WaitForAnimationToEnd(self, "Morph_End", false, true);
        self.state = FlyingBlimpLevelMoonLady.State.Idle;
        foreach (CollisionChild collisionChild in self.childColliders)
        {
            collisionChild.GetComponent<Collider2D>().enabled = true;
        }
        Level.Current.SetBounds(null, new int?(Level.Current.Right - 250), null, null);
        self.StartCoroutine(self.ufo_attack_handler_cr());
        yield return null;
        yield break;
    }

    private void SpawnUFO(FlyingBlimpLevelMoonLady self, FlyingBlimpLevelUFO prefab, bool reverse)
    {
        LevelProperties.FlyingBlimp.UFO uFO = self.properties.CurrentState.uFO;
        FlyingBlimpLevelUFO flyingBlimpLevelUFO = UnityEngine.Object.Instantiate<FlyingBlimpLevelUFO>(prefab);
        //flyingBlimpLevelUFO.Init(this.ufoStartPoint.position, this.ufoMidPoint.position, this.ufoStopPoint.position, uFO.UFOSpeed, uFO.UFOHP, uFO);
        Vector3 spawnPoint = new Vector3(1000f, self.ufoStopPoint.position.y);//new start
        Vector3 spawnPointReversed = new Vector3(-1000f, self.ufoStopPoint.position.y);
        if (reverse)
        {
            flyingBlimpLevelUFO.Init(spawnPointReversed, spawnPointReversed, spawnPointReversed, -uFO.UFOSpeed, uFO.UFOHP, uFO);
        }
        else
        {
            flyingBlimpLevelUFO.Init(spawnPoint, spawnPoint, spawnPoint, uFO.UFOSpeed, uFO.UFOHP, uFO);
        }//new end
    }

    public static IEnumerator waitAndLoad_cr(FlyingBlimpLevelMoonLady self, Levels level, bool final)//new
    {
        yield return CupheadTime.WaitForSeconds(self, 1f);
        if (YoMamaFat.loadUFOForBird || YoMamaFat.loadUFOForTrain)
        {
            YoMamaFat.ufoA = self.ufoPrefabA;
            YoMamaFat.ufoB = self.ufoPrefabB;
            YoMamaFat.ufoProperties = self.properties.CurrentState.uFO;
            UnityEngine.GameObject.DontDestroyOnLoad(YoMamaFat.ufoA);
            UnityEngine.GameObject.DontDestroyOnLoad(YoMamaFat.ufoB);
            Awake.Load(level, final);
        }
        yield break;
    }
}
