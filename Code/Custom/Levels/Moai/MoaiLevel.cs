using Blender.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace CupaGoovno;

public class MoaiLevel : Level
{
    public override Levels CurrentLevel => (Levels)Enum.Parse(typeof(Levels), "MoaiLevel");
    public override Scenes CurrentScene => (Scenes)Enum.Parse(typeof(Scenes), "scene_level_moai");
    public override Sprite BossPortrait => portrait.GetComponent<SpriteRenderer>().sprite;
    public override string BossQuote {
        get 
        {
            return "QuoteMoai" + UnityEngine.Random.Range(1, 34).ToString();
        } 
    }

    public override void PartialInit()
    {
        timeline = new();
        timeline.health = ultra ? p.hp * 10f : p.hp;
        if (ultra)
        {
            for(int i = 1; i < 10; i++)
            {
                timeline.AddEventAtHealth("", (int)p.hp * i);
            }
        }

        float time = ultra ? 2000f : 200f;
        goalTimes = new GoalTimes(time, time, time);

        base.PartialInit();
        Level.ScoringData.difficulty = Level.Mode.Hard;
    }

    public override void OnLevelStart()
    {
        base.OnLevelStart();
        DestroyNonGroundColliders();

        GetPrefabs();
        SetupLaser();

        GetNewHeartDelays();
        PlayMusic();
        instance = this;

        if(PlayerManager.Count > 1 && bossPlayerId > 0)
        {
            PlayerMoai.bossPlayerController.gameObject.SetActive(false);
            StartCoroutine(PlayerMoai.PlayerBoss_cr());
        }
        else
        {
            MoaiUtils.CsvLogMessage("Starting new battle", true, true);

            StartCoroutine(moaiPattern_cr(
                MoaiChoosers.Softmax,
                MoaiLearnersDiscrete.QLearning,
                MoaiLearnersContinous.PolicyGradientBeta));

            /*StartCoroutine(moaiPatternMock_cr(
                null,
                MoaiAttacks.Support.Crackhead));*/
        }
    }

    public override void Awake()
    {
        base.Awake();
        FixSpriteShaders();
        AddMoaiBehaviour();
    }

    private void FixSpriteShaders()
    {
        GameObject[] gameObjects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject gameObject in gameObjects)
        {
            SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            Shader errorShader = Shader.Find("Hidden/InternalErrorShader");
            Shader defaultShader = Shader.Find("Sprites/Default");
            if (spriteRenderer != null && spriteRenderer.material.shader == errorShader)
            {
                spriteRenderer.material = new Material(defaultShader);
            }
        }
    }

    private void AddMoaiBehaviour()
    {
        moai = GameObject.Find("Moai").AddComponent<MoaiLevelMoai>();
        GameObject.Find("BlueLaserLeft").AddComponent<MoaiLevelLaser>();
        GameObject.Find("BlueLaserRight").AddComponent<MoaiLevelLaser>();
    }

    private void DestroyNonGroundColliders()
    {
        GameObject.Destroy(GameObject.Find("Level_Wall_Left").gameObject);
        GameObject.Destroy(GameObject.Find("Level_Wall_Right").gameObject);
        GameObject.Destroy(GameObject.Find("Level_Ceiling").gameObject);
    }

    private void GetPrefabs()
    {
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            switch (obj.name)
            {
                case "Pollen":
                    MoaiLevelPollen.prefab = obj;
                    break;
                case "WarningGradient":
                    MoaiLevelWarning.prefab = obj;
                    break;
                case "Spike":
                    MoaiLevelSpike.prefab = obj;
                    break;
                case "BlueEyeLeft":
                    MoaiAttackLaser.eyeLeft = obj;
                    break;
                case "BlueEyeRight":
                    MoaiAttackLaser.eyeRight = obj;
                    break;
                case "BlueLaserLeft":
                    MoaiAttackLaser.laserLeft = obj;
                    break;
                case "BlueLaserRight":
                    MoaiAttackLaser.laserRight = obj;
                    break;
                case "BlueLaserWarningLeft":
                    MoaiAttackLaser.laserWarningLeft = obj;
                    break;
                case "BlueLaserWarningRight":
                    MoaiAttackLaser.laserWarningRight = obj;
                    break;
                case "BlueLaserPivotLeft":
                    MoaiAttackLaser.laserPivotLeft = obj;
                    break;
                case "BlueLaserPivotRight":
                    MoaiAttackLaser.laserPivotRight = obj;
                    break;
                case "LaserSpark":
                    MoaiLevelLaserSpark.prefab = obj;
                    break;
                case "RedLaser":
                    MoaiAttackLaser.redLaserSprite = obj.GetComponent<SpriteRenderer>().sprite;
                    break;
                case "RedLaserWarning":
                    MoaiAttackLaser.redLaserWarningSprite = obj.GetComponent<SpriteRenderer>().sprite;
                    break;
                case "RedEye":
                    MoaiAttackLaser.redEyeSprite = obj.GetComponent<SpriteRenderer>().sprite;
                    break;
                case "RedLaserSpark":
                    MoaiLevelLaserSpark.redSprite = obj.GetComponent<SpriteRenderer>().sprite;
                    break;
                case "GiantStone":
                    MoaiLevelGiantStone.prefab = obj;
                    break;
                case "Bird":
                    MoaiLevelBird.prefab = obj;
                    break;
                case "MoaiShitling":
                    MoaiLevelShitling.prefab = obj;
                    break;
                case "Crackhead":
                    MoaiLevelCrackhead.prefab = obj;
                    break;
                case "Rocket":
                    MoaiLevelRocket.prefab = obj;
                    break;
                case "Bouncer":
                    MoaiLevelBouncer.prefab = obj;
                    break;
                case "Pusher":
                    MoaiLevelPusher.prefab = obj;
                    break;
                case "BaseballMoai":
                    MoaiLevelBaseball.prefab = obj;
                    break;
                case "BaseballBat":
                    MoaiLevelBaseballBat.prefab = obj;
                    break;
                case "Heart":
                    MoaiLevelHeart.prefab = obj;
                    break;
                case "SfxRocketLoop":
                    MoaiAttackRockets.loop = obj.GetComponent<AudioSource>().clip;
                    break;
                case "SfxPollenWarning":
                    MoaiAttackPollen.warning = obj.GetComponent<AudioSource>().clip;
                    break;
                case "SfxPollenLaunch":
                    MoaiAttackPollen.launch = obj.GetComponent<AudioSource>().clip;
                    break;
                case "SfxShitlingFalling":
                    MoaiAttackShitlings.falling = obj.GetComponent<AudioSource>().clip;
                    break;
                case "SfxShitlingDeath1":
                case "SfxShitlingDeath2":
                case "SfxShitlingDeath3":
                case "SfxShitlingDeath4":
                    MoaiLevelShitling.deaths.Add(obj.GetComponent<AudioSource>().clip);
                    break;
                case "SfxGiantStoneLoop":
                    MoaiAttackGiantStone.loop = obj.GetComponent<AudioSource>().clip;
                    break;
                case "SfxBirdDeath":
                    MoaiLevelBird.death = obj.GetComponent<AudioSource>().clip;
                    break;
                case "SfxBouncerDeath":
                    MoaiLevelBouncer.death = obj.GetComponent<AudioSource>().clip;
                    break;
                case "SfxBouncerBounce1":
                case "SfxBouncerBounce2":
                case "SfxBouncerBounce3":
                case "SfxBouncerBounce4":
                    MoaiLevelBouncer.bounces.Add(obj.GetComponent<AudioSource>().clip);
                    break;
                case "SfxLaserWarning":
                    MoaiAttackLaser.warning = obj.GetComponent<AudioSource>().clip;
                    break;
                case "SfxLaserFire":
                    MoaiAttackLaser.fire = obj.GetComponent<AudioSource>().clip;
                    break;
                case "SfxLaserEyes":
                    MoaiAttackLaser.eyes = obj.GetComponent<AudioSource>().clip;
                    break;
                case "SfxUltrakillTheme":
                    ultrakillThemeClip = obj.GetComponent<AudioSource>().clip;
                    break;
                case "SfxUltrakillCrush":
                    MoaiAttackGiantStone.ultrakillCrush = obj.GetComponent<AudioSource>().clip;
                    break;
                case "SfxUltrakillDie":
                    MoaiAttackShitlings.ultrakillDie = obj.GetComponent<AudioSource>().clip;
                    break;
                case "SfxUltrakillJudgement":
                    MoaiAttackPusher.ultrakillJudgement = obj.GetComponent<AudioSource>().clip;
                    break;
                case "SfxUltrakillPrepare":
                    MoaiAttackRockets.ultrakillPrepare = obj.GetComponent<AudioSource>().clip;
                    break;
                case "SfxUltrakillThyEnd":
                    MoaiAttackLaser.ultrakillThyEnd = obj.GetComponent<AudioSource>().clip;
                    break;
                case "MUS_BotanicPanic":
                    themeClip = obj.GetComponent<AudioSource>().clip;
                    break;
                case "Portrait":
                    portrait = obj;
                    break;
                case "PlayerTwoArrowMain":
                    PlayerMoai.arrowMainTransform = obj.GetComponent<RectTransform>();
                    break;
                case "PlayerTwoArrowSupport":
                    PlayerMoai.arrowSupportTransform = obj.GetComponent<RectTransform>();
                    break;
                case "PlayerTwoSliderMain":
                    PlayerMoai.sliderMain = obj.GetComponent<Slider>();
                    break;
                case "PlayerTwoSliderSupport":
                    PlayerMoai.sliderSupport = obj.GetComponent<Slider>();
                    break;
                case "PlayerTwoStaminaBarFill":
                    PlayerMoai.stamina = obj.GetComponent<Image>();
                    PlayerMoai.stamina.fillAmount = 1f;
                    break;
                case "PlayerTwoCanvas":
                    if (PlayerManager.Count > 1 && bossPlayerId > 0)
                    {
                        obj.SetActive(true);
                    }
                    break;
            }

            if (obj.name.Contains("Bliss"))
            {
                SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
                if(sr != null)
                {
                    sr.sortingLayerName = "Background";
                }
            }
        }
    }

    private void SetupLaser()
    {
        MoaiAttackLaser.laserLeft.tag = "EnemyProjectile";
        MoaiAttackLaser.laserRight.tag = "EnemyProjectile";
        MoaiAttackLaser.laserLeft.layer = LayerMask.NameToLayer("Projectile");
        MoaiAttackLaser.laserRight.layer = LayerMask.NameToLayer("Projectile");
        if (redLaser)
        {
            MoaiAttackLaser.eyeLeft.GetComponent<SpriteRenderer>().sprite = MoaiAttackLaser.redEyeSprite;
            MoaiAttackLaser.eyeRight.GetComponent<SpriteRenderer>().sprite = MoaiAttackLaser.redEyeSprite;
            MoaiAttackLaser.laserLeft.GetComponent<SpriteRenderer>().sprite = MoaiAttackLaser.redLaserSprite;
            MoaiAttackLaser.laserRight.GetComponent<SpriteRenderer>().sprite = MoaiAttackLaser.redLaserSprite;
            MoaiAttackLaser.laserWarningRight.GetComponent<SpriteRenderer>().sprite = MoaiAttackLaser.redLaserWarningSprite;
            MoaiAttackLaser.laserWarningLeft.GetComponent<SpriteRenderer>().sprite = MoaiAttackLaser.redLaserWarningSprite;
            MoaiLevelLaserSpark.prefab.GetComponent<SpriteRenderer>().sprite = MoaiLevelLaserSpark.redSprite;
        }
    }

    private IEnumerator moaiPatternMock_cr(MoaiAttacks.Main? main, MoaiAttacks.Support? support)
    {
        yield return CupheadTime.WaitForSeconds(this, 2f);

        IMoaiLearnerDiscrete learnerDiscrete = new MoaiQLearning();
        MoaiContinousLearningManager learnerContinousManager = new(MoaiLearnersContinous.PolicyGradientBeta, learnerDiscrete);

        for(; ; )
        {
            learnerDiscrete.SaveHealthInfo();
            float mainParameter = 0f;
            float supportParameter = 0f;

            if (main is MoaiAttacks.Main main2)
            {
                mainParameter = learnerContinousManager.GetNext(main2);
                moai.Attack(main2, mainParameter);

            }
            if (support is MoaiAttacks.Support support2)
            {
                supportParameter = learnerContinousManager.GetNext(support2);
                moai.Attack(support2, supportParameter);
            }

            yield return CupheadTime.WaitForSeconds(this, 15f);

            if (main is MoaiAttacks.Main main3)
            {
                float reward = learnerDiscrete.GetReward(false);
                learnerContinousManager.Update(main3, reward, mainParameter);

            }
            if (support is MoaiAttacks.Support support3)
            {
                float reward = learnerDiscrete.GetReward(false);
                learnerContinousManager.Update(support3, reward, supportParameter);
            }
        }
    }

    private IEnumerator moaiPattern_cr(MoaiChoosers chooserEnum, MoaiLearnersDiscrete learnerDEnum, MoaiLearnersContinous learnerCEnum)
    {
        IMoaiLearnerDiscrete learnerDiscrete;
        switch (learnerDEnum)
        {
            case MoaiLearnersDiscrete.QLearning:
                learnerDiscrete = new MoaiQLearning();
                break;
            default:
                Plugin.Log("moaiParrern_cr couldnt find discrete learner");
                yield break;
        }

        MoaiContinousLearningManager learnerContinousManager = new(learnerCEnum, learnerDiscrete);

        IMoaiChooser chooser;
        switch (chooserEnum)
        {
            case MoaiChoosers.EpsilonGreedy:
                chooser = new MoaiEpsilonGreedy(learnerDiscrete);
                break;
            case MoaiChoosers.Softmax:
                chooser = new MoaiChooserSoftmax(learnerDiscrete);
                break;
            case MoaiChoosers.Random:
                chooser = new MoaiChooserRandom();
                break;
            default:
                Plugin.Log("moaiParrern_cr couldnt find chooser");
                yield break;
        }

        for (; ; )
        {
            learnerDiscrete.SaveHealthInfo();

            MoaiAttacks attacks = chooser.GetNext();
            float mainParameter = learnerContinousManager.GetNext(attacks.main);
            float supportParameter = learnerContinousManager.GetNext(attacks.support);
            moai.Attack(attacks, mainParameter, supportParameter);
            bool heart = CheckForHeart();

            while (moai.state == MoaiLevelMoai.States.Attacking)
            {
                yield return null;
            }

            learnerDiscrete.Update(attacks, heart);

            float reward = learnerDiscrete.GetReward(heart);
            learnerContinousManager.Update(attacks.main, reward, mainParameter);
            learnerContinousManager.Update(attacks.support, reward, supportParameter);
        }
    }

    private bool CheckForHeart()
    {
        if (possibleHeartDelays[0] == 0)
        {
            MoaiLevelHeart.Create();
            possibleHeartDelays.RemoveAt(0);
            if(possibleHeartDelays.Count == 0)
            {
                GetNewHeartDelays();
            }
            return true;
        }
        else
        {
            possibleHeartDelays[0]--;
            return false;
        }
    }

    private void GetNewHeartDelays()
    {
        MoaiProperties.Heart p = new();
        List<int> delays = [];

        for(int i = (int)p.delay.min; i <= p.delay.max; i++)
        {
            delays.Add(i - 1);
        }

        delays.Shuffle();
        possibleHeartDelays = delays;
    }

    private void PlayMusic()
    {
        theme = moai.gameObject.AddComponent<AudioSource>();
        theme.clip = ultra ? ultrakillThemeClip : themeClip;
        theme.volume = themeVolume * Other.GetMusicVolumeMultiplier();
        theme.loop = true;
        theme.ignoreListenerPause = true;
        theme.Play();
    }

    public override void OnLevelEnd()
    {
        if (this != null)
        {
            StopAllCoroutines();
            StartCoroutine(CustomLevelUtils.endMusic_cr(theme, themeVolume));
        }
    }

    public static MoaiLevelMoai moai;
    public static MoaiLevel instance;
    public static bool redLaser;
    public static bool ultra;
    public static int bossPlayerId;

    private List<int> possibleHeartDelays = [];
    private GameObject portrait;
    private MoaiProperties.Moai p = new();
    private AudioSource theme;
    private AudioClip themeClip;
    private AudioClip ultrakillThemeClip;
    private float themeVolume = 0.6f;
}
