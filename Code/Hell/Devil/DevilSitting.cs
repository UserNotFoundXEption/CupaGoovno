using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking.Match;
using static TrainLevelSkeleton;

namespace CupaGoovno;

public class DevilSitting
{
    public void Init()
    {
        On.DevilLevelSittingDevil.Awake += Awake;
        On.DevilLevelSittingDevil.pitchforkFiveFlameSpinner_cr += pitchforkFiveFlameSpinner_cr;
        On.DevilLevelSittingDevil.spider_cr += spider_cr;
        On.DevilLevelSittingDevil.clap_cr += clap_cr;
        On.DevilLevelSittingDevil.demon_cr += demon_cr;
        On.DevilLevelSittingDevil.OnDestroy += OnDestroy;
    }

    protected void Awake(On.DevilLevelSittingDevil.orig_Awake orig, DevilLevelSittingDevil self)
    {
        orig(self);
        self.StartCoroutine(self.pitchforkFiveFlameSpinner_cr());
        Devil.orbiter = self.spinnerOrbitingProjectilePrefab;
    }

    private IEnumerator pitchforkFiveFlameSpinner_cr(On.DevilLevelSittingDevil.orig_pitchforkFiveFlameSpinner_cr orig, DevilLevelSittingDevil self)
    {
        yield return CupheadTime.WaitForSeconds(self, 5f);//new start
        float spawnRadius = self.properties.CurrentState.pitchfork.spawnRadius;
        float dormantDuration = self.properties.CurrentState.pitchfork.dormantDuration;
        float spawnCenterY = self.properties.CurrentState.pitchfork.spawnCenterY;//new end
        LevelProperties.Devil.PitchforkFiveFlameSpinner p = self.properties.CurrentState.pitchforkFiveFlameSpinner;
        DevilLevelPitchforkSpinnerProjectile centerProjectile = self.spinnerProjectilePrefab.Create(new Vector2(0f, spawnCenterY), p.maxSpeed, p.acceleration, p.attackDuration, self, dormantDuration);
        spinnerCenter = centerProjectile;//new
        float rotationSpeed = (float)Rand.PosOrNeg() * p.rotationSpeed;
        foreach (float angle in self.pitchforkFiveFlameSpinnerSpawner.getSpawnAngles())
        {
            
            DevilLevelPitchforkOrbitingProjectile proj = self.spinnerOrbitingProjectilePrefab.Create(centerProjectile, angle, rotationSpeed, spawnRadius, self, dormantDuration);//new
            spinnerList.Add(proj);//new end
            //self.spinnerOrbitingProjectilePrefab.Create(centerProjectile, angle, rotationSpeed, self.properties.CurrentState.pitchfork.spawnRadius, self, self.properties.CurrentState.pitchfork.dormantDuration);
        }
        centerProjectile.SetParryable(false);//new start
        centerProjectile.transform.SetScale(3.5f, 3.5f, 3.5f);
        SpriteRenderer centerSprite = centerProjectile.GetComponent<SpriteRenderer>();
        while(centerSprite == null)
        {
            yield return null;
            centerSprite = centerProjectile.GetComponent<SpriteRenderer>();
        }
        centerSprite.sortingLayerName = "Player";
        centerSprite.sortingOrder = int.MinValue;
        YoMamaFat.parryOrbTransform.position = new Vector2(0f, 30f);
        YoMamaFat.parryOrbTransform.parent = centerProjectile.transform;
        SpriteRenderer renderer = YoMamaFat.parryOrbTransform.GetComponent<SpriteRenderer>();
        while(renderer == null)
        {
            yield return null;
            renderer = YoMamaFat.parryOrbTransform.GetComponent<SpriteRenderer>();
        }
        renderer.enabled = false;
        YoMamaFat.parryOrbTransform.SetScale(3f, 3f, 3f);
        YoMamaFat.parryOrb.parrySwitch.enabled = true;//new end
        yield break;
    }

    private IEnumerator spider_cr(On.DevilLevelSittingDevil.orig_spider_cr orig, DevilLevelSittingDevil self)
    {
        self.animator.SetBool("StartSpider", true);
        yield return self.animator.WaitForAnimationToStart(self, "Spider_Start", false);
        AudioManager.Play("devil_spider_head_intro");
        self.emitAudioFromObject.Add("devil_spider_head_intro");
        yield return self.animator.WaitForAnimationToEnd(self, "Spider_Start", false, true);
        LevelProperties.Devil.Spider p = self.properties.CurrentState.spider;
        int numAttacks = p.numAttacks.RandomInt();
        for (int i = 0; i < numAttacks; i++)
        {
            yield return CupheadTime.WaitForSeconds(self, p.entranceDelay.RandomFloat());
            self.spiderOffsetIndex = (self.spiderOffsetIndex + 1) % self.spiderOffsets.Length;
            float offset = 0f;
            Parser.FloatTryParse(self.spiderOffsets[self.spiderOffsetIndex], out offset);
            float xPos = Mathf.Clamp(PlayerManager.GetNext().center.x + offset, -620f, 620f);
            self.spiderHead.Attack(xPos, p.downSpeed, p.upSpeed);
            self.StartCoroutine(demonsAfterSpider_cr(self, xPos));//new
            while (self.spiderHead.state != DevilLevelSpiderHead.State.Idle)
            {
                yield return null;
            }
        }
        self.animator.SetBool("StartSpider", false);
        yield return CupheadTime.WaitForSeconds(self, p.hesitate);
        self.state = DevilLevelSittingDevil.State.Idle;
        yield break;
    }

    private IEnumerator clap_cr(On.DevilLevelSittingDevil.orig_clap_cr orig, DevilLevelSittingDevil self)
    {
        clapBouncers_cr(self);
        yield return orig(self);
    }

    private IEnumerator demon_cr(On.DevilLevelSittingDevil.orig_demon_cr orig, DevilLevelSittingDevil self)
    {
        yield break;
    }

    protected void OnDestroy(On.DevilLevelSittingDevil.orig_OnDestroy orig, DevilLevelSittingDevil self)
    {
        DestroySpinner(self);
        orig(self);
    }

    private IEnumerator demonsAfterSpider_cr(DevilLevelSittingDevil self, float xPos)//new
    {
        yield return CupheadTime.WaitForSeconds(self, 0.8f);
        for (int i = 0; i < 3; i++)
        {
            Vector2 pos = new Vector2(xPos, -200f);
            float speed = self.properties.CurrentState.demons.speed;
            float hp = self.properties.CurrentState.demons.hp;
            DevilDemon.Create(self.demonPrefab, pos, 1, speed, hp, self);
            DevilDemon.Create(self.demonPrefab, pos, -1, speed, hp, self);
            yield return CupheadTime.WaitForSeconds(self, 0.1f);
        }
    }

    private void clapBouncers_cr(DevilLevelSittingDevil self)//new
    {
        LevelProperties.Devil.PitchforkFourFlameBouncer p = self.properties.CurrentState.pitchforkFourFlameBouncer;
        foreach (float angle in self.pitchforkFourFlameBouncerSpawner.getSpawnAngles())
        {
            self.bouncingProjectilePrefab.Create(self.getPitchforkFiringPos(angle), 1f, p.speed, angle, p.numBounces, self, self.properties.CurrentState.pitchfork.dormantDuration);
        }
    }

    public static void DestroySpinner(DevilLevelSittingDevil self)
    {
        if(spinnerCenter != null)
        {
            GameObject.Destroy(spinnerCenter.gameObject);
        }
        while(spinnerList.Count > 0)
        {
            if(spinnerList[0] != null)
            {
                GameObject.Destroy(spinnerList[0].gameObject);
            }
            spinnerList.RemoveAt(0);
        }
        if (YoMamaFat.parryOrbTransform != null)
        {
            SpriteRenderer renderer = YoMamaFat.parryOrbTransform.GetComponent<SpriteRenderer>();
            if(renderer != null)
            {
                renderer.enabled = true;
            }
            YoMamaFat.parryOrbTransform.parent = null;
            YoMamaFat.parryOrbTransform.SetScale(1f, 1f, 1f);
            YoMamaFat.parryOrbTransform.position = new Vector2(2137f, 2137f);
        }
    }

    private static DevilLevelPitchforkSpinnerProjectile spinnerCenter;
    private static List<DevilLevelPitchforkOrbitingProjectile> spinnerList = new List<DevilLevelPitchforkOrbitingProjectile>();
}
