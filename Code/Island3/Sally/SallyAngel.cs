using System.Collections;
using UnityEngine;

namespace CupaGoovno;

public class SallyAngel
{
    public void Init()
    {
        On.SallyStagePlayLevelAngel.intro_cr += intro_cr;
        On.SallyStagePlayLevelAngel.meteor_cr += meteor_cr;
        On.SallyStagePlayLevelAngel.tidal_wave_cr += tidal_wave_cr;
        On.SallyStagePlayLevelAngel.OnPhase4 += OnPhase4;
    }

    private IEnumerator intro_cr(On.SallyStagePlayLevelAngel.orig_intro_cr orig, SallyStagePlayLevelAngel self)
    {
        self.SpawnUmbrella();//new
        float t = 0f;
        float time = 3f;
        Vector3 endPos = new Vector3(self.transform.position.x - 300f, self.phase3Root.position.y);//new
        //Vector3 endPos = new Vector3(self.transform.position.x, self.phase3Root.position.y);
        Vector2 start = self.transform.position;
        self.GetComponent<Collider2D>().enabled = true;
        self.StartCoroutine(self.sally_angel_intro_sound_cr());
        if (self.killedHusband)
        {
            self.StartCoroutine(self.spawn_husband_cr());
        }
        while (t < time)
        {
            float val = EaseUtils.Ease(EaseUtils.EaseType.easeInOutBounce, 0f, 1f, t / time);
            self.transform.position = Vector2.Lerp(start, endPos, val);
            t += CupheadTime.Delta;
            yield return null;
        }
        self.transform.position = endPos;
        self.nextAttack = 1;
        self.StartCoroutine(self.sign_slide_cr());
        yield return CupheadTime.WaitForSeconds(self, 1f);
        self.StartCoroutine(self.main_cr());
        yield return null;
        yield break;
    }

    private IEnumerator meteor_cr(On.SallyStagePlayLevelAngel.orig_meteor_cr orig, SallyStagePlayLevelAngel self)
    {
        self.state = SallyStagePlayLevelAngel.State.Meteor;
        LevelProperties.SallyStagePlay.Meteor p = self.properties.CurrentState.meteor;
        string[] meteorSpawnString = p.meteorSpawnString.Split(new char[]
        {
        ','
        });
        int index = 0;
        float spawn = 0f;
        Parser.FloatTryParse(meteorSpawnString[self.meteorSpawnIndex], out spawn);
        bool lockedPosition = false;
        for (int i = 0; i < self.meteors.Count; i++)
        {
            if (self.meteors[i].state == SallyStagePlayLevelMeteor.State.Leaving)
            {
                self.meteors.Remove(self.meteors[i]);
                i++;
            }
        }
        yield return null;
        for (int j = 0; j < self.meteors.Count; j++)
        {
            if (spawn == self.meteors[j].spawnPosition)
            {
                index = j;
                lockedPosition = true;
                break;
            }
        }
        bool positionTaken = false;
        int meteorCounter = 0;
        int spawnStringCounter = 0;
        while (lockedPosition)
        {
            while (meteorCounter < self.meteors.Count)
            {
                meteorCounter++;
                if (spawn == self.meteors[index].spawnPosition)
                {
                    positionTaken = true;
                }
                index = (index + 1) % self.meteors.Count;
            }
            if (!positionTaken)
            {
                lockedPosition = false;
                break;
            }
            self.meteorSpawnIndex = (self.meteorSpawnIndex + 1) % meteorSpawnString.Length;
            Parser.FloatTryParse(meteorSpawnString[self.meteorSpawnIndex], out spawn);
            spawnStringCounter++;
            if (spawnStringCounter >= meteorSpawnString.Length)
            {
                break;
            }
            meteorCounter = 0;
            positionTaken = false;
            yield return null;
        }
        lastMeteorX = spawn - 640f;//new
        if (self.meteors.Count <= 0)
        {
            lockedPosition = false;
        }
        if (!lockedPosition)
        {
            self.meteors.Add(self.meteorPrefab.Create(spawn, (float)p.meteorHP, p));
            self.meteorSpawnIndex = (self.meteorSpawnIndex + 1) % meteorSpawnString.Length;
        }
        self.animator.SetBool("OnPh3Attack", false);
        yield return CupheadTime.WaitForSeconds(self, self.properties.CurrentState.general.attackDelayRange.RandomFloat());
        self.state = SallyStagePlayLevelAngel.State.Idle;
        yield return null;
        yield break;
    }

    private IEnumerator tidal_wave_cr(On.SallyStagePlayLevelAngel.orig_tidal_wave_cr orig, SallyStagePlayLevelAngel self)
    {
        self.state = SallyStagePlayLevelAngel.State.Wave;
        LevelProperties.SallyStagePlay.Tidal p = self.properties.CurrentState.tidal;
        self.wave.StartWave(p);
        self.StartCoroutine(waveHorizontalAttack_cr(self));//new
        while (self.wave.isMoving)
        {
            yield return null;
        }
        self.animator.SetBool("OnPh3Attack", false);
        yield return CupheadTime.WaitForSeconds(self, self.properties.CurrentState.tidal.tidalHesitate);
        yield return self.animator.WaitForAnimationToEnd(self, "Phase3_Attack", false, false);
        self.state = SallyStagePlayLevelAngel.State.Idle;
        yield return null;
        yield break;
    }

    public void OnPhase4(On.SallyStagePlayLevelAngel.orig_OnPhase4 orig, SallyStagePlayLevelAngel self)
    {
        AudioManager.Stop("sally_sally_lightning_move_loop");
        self.StopAllCoroutines();
        self.StartCoroutine(self.slide_out_cr());
        self.GetComponent<LevelBossDeathExploder>().StartExplosion();
        self.animator.SetTrigger("OnPh3Death");
        self.StartCoroutine(start_phase_4_cr(self));
        LevelProperties.Bee.Follower prop = YoMamaFat.followerOrbProperties;//new start
        BeeLevelQueenFollower.Properties p = new BeeLevelQueenFollower.Properties(PlayerManager.GetNext(), prop.introTime, prop.homingSpeed, prop.homingRotation, prop.homingTime, prop.health, prop.childDelay, prop.childHealth, false);
        BeeLevelQueenFollower follower = YoMamaFat.followerOrb.Create(new Vector3(0, 500), p);
        SpriteRenderer followerSprite = follower.GetComponent<SpriteRenderer>();
        if(followerSprite != null)
        {
            followerSprite.material = new Material(Shader.Find("Sprites/Default"));
            followerSprite.color = Color.red;
        }
        GameObject.Destroy(GameObject.Find("0_sally_bg_stage-side_curtains"));//new end
    }

    private IEnumerator start_phase_4_cr(SallyStagePlayLevelAngel self)
    {
        GameObject[] objs = GameObject.FindObjectsOfType<GameObject>();//new start
        foreach(GameObject obj in objs)
        {
            if(obj.name == "Phase3Wave" || obj.name == "SallyStagePlay_Bottle(Clone)")
            {
                GameObject.Destroy(obj);
            }
        }//new end
        self.GetComponent<SpriteRenderer>().material = self.phase4Material;
        for (int i = 0; i < self.meteors.Count; i++)
        {
            if (self.meteors[i] != null)
            {
                self.meteors[i].MeteorChangePhase();
            }
        }
        float t = 0f;
        float time = 2.5f;
        Vector3 endPos = new Vector3(self.transform.position.x, 860f);
        Vector2 start = self.transform.position;
        yield return CupheadTime.WaitForSeconds(self, 0.5f);
        if (self.killedHusband)
        {
            self.husband.Dead();
            self.StartCoroutine(self.husband.move_cr());
        }
        while (t < time)
        {
            float val = EaseUtils.Ease(EaseUtils.EaseType.linear, 0f, 1f, t / time);
            self.transform.position = Vector2.Lerp(start, endPos, val);
            t += CupheadTime.Delta;
            yield return null;
        }
        self.GetComponent<LevelBossDeathExploder>().StopExplosions();
        foreach (GameObject gameObject in self.shadows)
        {
            gameObject.SetActive(false);
        }
        self.animator.Play("Phase4_Idle");
        yield return CupheadTime.WaitForSeconds(self, 1f);
        t = 0f;
        time = 1f;
        Vector3 pos = self.transform.position;
        pos.x = -640f + self.transform.GetComponent<Renderer>().bounds.size.x / 2f;
        self.transform.position = pos;
        endPos = new Vector3(self.transform.position.x, self.phase4Root.position.y);
        start = self.transform.position;
        while (t < time)
        {
            float val2 = EaseUtils.Ease(EaseUtils.EaseType.linear, 0f, 1f, t / time);
            self.transform.position = Vector2.Lerp(start, endPos, val2);
            t += CupheadTime.Delta;
            yield return null;
        }
        self.StartCoroutine(self.move_cr());
        self.StartCoroutine(self.spawn_roses_cr());
        //self.SpawnUmbrella();
        yield return null;
        yield break;
    }

    private IEnumerator waveHorizontalAttack_cr(SallyStagePlayLevelAngel self)//new
    {
        float speed = 1000f;
        int emptySpaceInt = UnityEngine.Random.Range(-2, 3);
        float emptySpace = emptySpaceInt * 80f + 40f;
        GameObject rectangle = GameObject.CreatePrimitive(PrimitiveType.Cube);
        rectangle.transform.localScale = new Vector3(1500f, 40f, 1f);
        rectangle.transform.position = new Vector3(0f, emptySpace, 0f);

        MeshRenderer rectangleRenderer = Other.SetTransparentMaterial(rectangle, new Color(1f, 0f, 0f, 0f));

        float waveStartX = -640f;
        float waveDistance = lastMeteorX - waveStartX;
        float horizontalTime = waveDistance / speed;
        float deltaDistance = self.properties.CurrentState.tidal.tidalSpeed * horizontalTime;
        float offset = 200f;
        float fireX = lastMeteorX - deltaDistance + offset; ;
        while (self.wave.transform.position.x < fireX)
        {
            float percent = (self.wave.transform.position.x - waveStartX) / (fireX - waveStartX);
            if(percent < 0f)
            {
                percent = 0f;
            }
            rectangleRenderer.material.color = new Color(0f, 1f, 0f, percent * percent);
            yield return null;
        }

        for (float y = -360f; y < 800f; y += 80f)
        {
            if (y != emptySpace)
            {
                SallyStagePlayLevelWindowProjectile projectile = YoMamaFat.sallyBottle.Create(new Vector3(-750f, y), 180f, -speed, YoMamaFat.sallyParent);
                projectile.transform.SetScale(-4f, null, null);
            }
        }
        GameObject.Destroy(rectangle);
        yield break;
    }

    float lastMeteorX;
}
