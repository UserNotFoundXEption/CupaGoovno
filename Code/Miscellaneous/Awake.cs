using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.IO;

namespace CupaGoovno;

public class Awake
{
    public void Init()
    {
        On.Level.Awake += LevelAwake;
        On.AbstractMapSceneStartUI.Awake += MapAwake;
        On.StartScreen.Awake += StartScreenAwake;
    }

    protected void LevelAwake(On.Level.orig_Awake orig, Level self)//new
    {
        if (self is PlatformingLevel)
        {
            RunNGunFollower.Create(true);
        }
        if(self is MausoleumLevel)
        {
            RunNGunFollower.Create(false);
        }

        instance = self;
        Other.levelSelf = self;
        orig(self);
        switch (self)
        {
            case VeggiesLevel:
                VeggiesAwake();
                break;
            case DicePalaceRabbitLevel rabbitSelf:
                RabbitAwake(rabbitSelf);
                break;
            case SlimeLevel:
                SlimeAwake();
                break;
            case FlyingBlimpLevel blimpSelf:
                BlimpAwake(blimpSelf);
                break;
            case ClownLevel clownSelf:
                ClownAwake(clownSelf);
                break;
            case DragonLevel dragonSelf:
                DragonAwake(dragonSelf);
                break;
            case FlyingBirdLevel birdSelf:
                BirdAwake(birdSelf);
                break;
            case BeeLevel beeSelf:
                BeeAwake(beeSelf);
                break;
            case RobotLevel:
                RobotAwake();
                break;
            case MouseLevel mouseSelf:
                MouseAwake(mouseSelf);
                break;
            case PirateLevel:
                PirateAwake();
                break;
            case SallyStagePlayLevel:
                SallyAwake();
                break;
            case TrainLevel:
                TrainAwake();
                break;
            case DicePalaceRouletteLevel rouletteSelf:
                RouletteAwake(rouletteSelf);
                break;
            case FlyingMermaidLevel mermaidSelf:
                MermaidAwake(mermaidSelf);
                break;
            case DicePalaceMainLevel:
                KingDiceAwake();
                break;
            case DicePalaceDominoLevel:
                DominoAwake();
                break;
            case DicePalaceCigarLevel cigarSelf:
                CigarAwake(cigarSelf);
                break;
            case DicePalaceFlyingMemoryLevel:
                MonkeyAwake();
                break;
            case DicePalaceFlyingHorseLevel horseSelf:
                HorseAwake(horseSelf);
                break;
            case DevilLevel devilSelf:
                DevilAwake(devilSelf);
                break;
            case FlyingCowboyLevel cowSelf:
                CowAwake(cowSelf);
                break;
            case TutorialLevel:
                TutorialAwake();
                break;
            case GraveyardLevel graveyardSelf:
                GraveyardAwake(graveyardSelf);
                break;
            default:
                break;
        }
    }

    public static void Load(Levels level, bool final)
    {
        instance.StartCoroutine(waitAndLoad_cr(level, final));
    }

    public static IEnumerator waitAndLoad_cr(Levels level, bool final)
    {
        while (SceneLoader.currentlyLoading)
        {
            yield return null;
        }
        SceneLoader.CurrentLevel = level;
        SceneLoader.Transition transitionStart = SceneLoader.Transition.None;
        SceneLoader.Transition transitionEnd = final ? SceneLoader.Transition.Iris : SceneLoader.Transition.None;
        SceneLoader.LoadScene(LevelProperties.GetLevelScene(level), transitionStart, transitionEnd);
    }

    private void VeggiesAwake()
    {
        if (YoMamaFat.flowerPlatform == null)
        {
            YoMamaFat.loadFlowerPlatform = true;
            Load(Levels.DicePalaceRabbit, false);
        }
        else
        {
            YoMamaFat.loadFlowerPlatform = false;
        }

        Veggies.SetPlatformX(-2137f);
        Carrot.mainCarrotDic = [];
        Carrot.carrotsList = [];
        UnityEngine.GameObject.Destroy(GameObject.Find("veggie_bg_0001"));
        UnityEngine.GameObject.Destroy(GameObject.Find("veggie_bg_0002"));
    }

    private void RabbitAwake(DicePalaceRabbitLevel self)
    {
        if (YoMamaFat.loadFlowerPlatform)
        {
            YoMamaFat.flowerPlatform = self.rabbit.platform1;
            GameObject.DontDestroyOnLoad(self.rabbit.platform1); 
            Load(Levels.Veggies, true);
        }
    }

    private void SlimeAwake()
    {
        if (YoMamaFat.parryOrb == null)
        {
            YoMamaFat.loadOrbForSlime = true;
            Load(Levels.Tutorial, false);
        }
        else
        {
            YoMamaFat.loadOrbForSlime = false;
        }
    }

    private void BlimpAwake(FlyingBlimpLevel self)
    {
        if (YoMamaFat.dragonPotion == null && !YoMamaFat.loadUFOForBird && !YoMamaFat.loadUFOForTrain)
        {
            YoMamaFat.loadDragonPotion = true;
            Load(Levels.Dragon, false);
        }
        else
        {
            YoMamaFat.loadDragonPotion = false;
        }

        if (YoMamaFat.loadUFOForBird)
        {
            self.StartCoroutine(BlimpMoonLady.waitAndLoad_cr(self.moonLady, Levels.FlyingBird, false));
        }
        if (YoMamaFat.loadUFOForTrain)
        {
            self.StartCoroutine(BlimpMoonLady.waitAndLoad_cr(self.moonLady, Levels.Train, true));
        }

        self.blimpLady.loopSize = 150f;
        self.blimpLady.pivotOffset = Vector3.up * 2f * self.blimpLady.loopSize;
        self.blimpLady.transform.AddPosition(150f, 0f, 0f);
        self.blimpLady.pivotPoint.position = self.blimpLady.transform.position;
        self.blimpLady.startPos = self.blimpLady.transform.position;
        GameObject.Destroy(UnityEngine.GameObject.Find("TreeGroup"));
    }

    private void ClownAwake(ClownLevel self)
    {
        YoMamaFat.clownHorseshoe = self.clownHorse.regularHorseshoe;
        YoMamaFat.clownProperties = self.properties;
    }

    private void DragonAwake(DragonLevel self)
    {
        YoMamaFat.dragonPotion = self.leftSideDragon.bothPotionPrefab;
        YoMamaFat.dragonPotionProperties = self.properties.CurrentState.potions;
        GameObject.DontDestroyOnLoad(YoMamaFat.dragonPotion);
        if (YoMamaFat.loadDragonPotion)
        {
            Load(Levels.FlyingBlimp, true);
        }
    }

    private void BirdAwake(FlyingBirdLevel self)
    {
        if (YoMamaFat.ufoA == null)
        {
            YoMamaFat.loadUFOForBird = true;
            Load(Levels.FlyingBlimp, false);
        }
        else
        {
            YoMamaFat.loadUFOForBird = false;
        }

        if (YoMamaFat.dragonfly == null)
        {
            YoMamaFat.loadDragofly = true;
            Load(Levels.Platforming_Level_1_2, false);
        }
        else
        {
            YoMamaFat.loadDragofly = false;
            GameObject.Destroy(GameObject.Find("birdhouse_bg_0001"));
            GameObject.Destroy(GameObject.Find("birdhouse_bg_0002"));
            GameObject.Destroy(GameObject.Find("birdhouse_bg_0003"));
            self.StartCoroutine(Bird.warning_cr(self));
        }
    }

    private void BeeAwake(BeeLevel self)
    {
        YoMamaFat.followerOrb = self.queen.followerPrefab;
        YoMamaFat.followerOrbProperties = self.properties.CurrentState.follower;
        GameObject.DontDestroyOnLoad(YoMamaFat.followerOrb);

        if (YoMamaFat.loadFollowerOrb)
        {
            Load(Levels.SallyStagePlay, true);
        }
    }

    private void RobotAwake()
    {
        if (YoMamaFat.cowboyUfo == null || YoMamaFat.cowboyUfoProperties == null)
        {
            YoMamaFat.loadCowboyUFO = true;
            Load(Levels.FlyingCowboy, false);
        }
        else
        {
            YoMamaFat.loadCowboyUFO = false;
        }

        GameObject.Destroy(GameObject.Find("Foreground_Miscellaneous"));
        GameObject.Destroy(GameObject.Find("junkyard_fg_crane_b"));
    }

    private void MouseAwake(MouseLevel self)
    {
        YoMamaFat.fallingObject = self.cat.fallingObjectPrefabs[0];
        YoMamaFat.fallingObjectProperties = self.properties.CurrentState.claw;
        GameObject.DontDestroyOnLoad(YoMamaFat.fallingObject);
        if (YoMamaFat.loadMouseFallingObject)
        {
            Load(Levels.Pirate, true);
        }
    }

    private void PirateAwake()
    {
        if (YoMamaFat.fallingObject == null)
        {
            YoMamaFat.loadMouseFallingObject = true;
            Load(Levels.Mouse, false);
        }
        else
        {
            YoMamaFat.loadMouseFallingObject = false;
        }
    }

    private void SallyAwake()
    {
        if (YoMamaFat.parryOrb == null || YoMamaFat.parryOrbTransform == null)
        {
            YoMamaFat.loadOrbForSally = true;
            Load(Levels.Tutorial, false);
        }
        else
        {
            YoMamaFat.loadOrbForSally = false;
        }

        if (YoMamaFat.followerOrb == null || YoMamaFat.followerOrbProperties == null)
        {
            YoMamaFat.loadFollowerOrb = true;
            Load(Levels.Bee, false);
        }
        else
        {
            YoMamaFat.loadFollowerOrb = false;
        }

        GameObject.Destroy(GameObject.Find("Cupid_Right"));
        GameObject.Destroy(GameObject.Find("Cupid_Left"));
    }

    private void TrainAwake()
    {
        if (YoMamaFat.ufoA == null)
        {
            YoMamaFat.loadUFOForTrain = true;
            Load(Levels.FlyingBlimp, false);
        }
        else
        {
            YoMamaFat.loadUFOForTrain = false;
        }
    }

    private void MermaidAwake(FlyingMermaidLevel self)
    {
        if (YoMamaFat.loadMermaidLaser)
        {
            YoMamaFat.mermaidLaser = self.merdusa.laser;
            self.merdusa.laser.transform.parent = null;
            GameObject.DontDestroyOnLoad(YoMamaFat.mermaidLaser);
            Load(Levels.DicePalaceFlyingMemory, true);
        }
    }

    private void RouletteAwake(DicePalaceRouletteLevel self)
    {
        Roulette.platforms = self.platforms;
    }

    private void KingDiceAwake()
    {
        if (YoMamaFat.cigarSpit == null)
        {
            YoMamaFat.loadCigarSpitForKing = true;
            Load(Levels.DicePalaceCigar, false);
        }
        else
        {
            YoMamaFat.loadCigarSpitForKing = false;
        }
    }

    private void DominoAwake()
    {
        if(YoMamaFat.cigarSpit == null)
        {
            YoMamaFat.loadCigarSpitForDomino = true;
            Load(Levels.DicePalaceCigar, false);
        }
        else
        {
            YoMamaFat.loadCigarSpitForDomino = false;
        }
    }

    private void CigarAwake(DicePalaceCigarLevel self)
    {
        YoMamaFat.cigarSpit = self.cigar.spitPrefab;
        YoMamaFat.cigarProperties = self.properties;
        GameObject.DontDestroyOnLoad(YoMamaFat.cigarSpit);
        if (YoMamaFat.loadCigarSpitForDomino)
        {
            Load(Levels.DicePalaceDomino, true);
        }
        if (YoMamaFat.loadCigarSpitForKing)
        {
            Load(Levels.DicePalaceMain, true);
        }
    }

    private void MonkeyAwake()
    {
        if (YoMamaFat.mermaidLaser == null)
        {
            YoMamaFat.loadMermaidLaser = true;
            Load(Levels.FlyingMermaid, false);
        }
        else
        {
            YoMamaFat.loadMermaidLaser = false;
        }

        GameObject.Find("ForegroundPlush").GetComponent<SpriteRenderer>().enabled = false;
    }

    private void HorseAwake(DicePalaceFlyingHorseLevel self)
    {
        self.horse.transform.AddPosition(200f);
        GameObject.Destroy(GameObject.Find("Foreground"));
        HorsePresent.horseshoes = [];
        Horse.horse = self.horse;
    }

    private void DevilAwake(DevilLevel self)
    {
        if (YoMamaFat.parryOrb == null || YoMamaFat.parryOrbTransform == null)
        {
            YoMamaFat.loadOrbForDevil = true;
            Load(Levels.Tutorial, false);
        }
        else
        {
            YoMamaFat.loadOrbForDevil = false;
        }

        Devil.devil = self;
        GameObject.Find("devil_bg_ph_1_foreground").GetComponent<SpriteRenderer>().enabled = false;
    }

    private void CowAwake(FlyingCowboyLevel self)
    {
        YoMamaFat.cowboyUfo = self.cowboy.ufoPrefab;
        YoMamaFat.cowboyUfoProperties = self.properties.CurrentState.uFOEnemy;
        GameObject.DontDestroyOnLoad(YoMamaFat.cowboyUfo); 
        if (YoMamaFat.loadCowboyUFO)
        {
            Load(Levels.Robot, true);
        }
    }

    protected void TutorialAwake()
    {
        if (YoMamaFat.loadOrbForSlime && Tutorial.GetOrb())
        {
            Load(Levels.Slime, true);
        }
        if (YoMamaFat.loadOrbForSally && Tutorial.GetOrb())
        {
            Load(Levels.SallyStagePlay, true);
        }
        if (YoMamaFat.loadOrbForDevil && Tutorial.GetOrb())
        {
            Load(Levels.Devil, true);
        }
    }

    private void GraveyardAwake(GraveyardLevel self)
    {
        self.StartCoroutine(Graveyard.warning_cr(self));
    }

    protected void MapAwake(On.AbstractMapSceneStartUI.orig_Awake orig, AbstractMapSceneStartUI self)
    {
        orig(self);
        CustomCharms.GiftCharms();
        CustomWeapons.GiftWeapons();
        CustomSupers.GiftSupers();
        if (self is MapDifficultySelectStartUI)
        {
            DifficultySelectUIAwake(self as MapDifficultySelectStartUI);
        }
        if (!showedFeedbackNotification)
        {
            string tText = "Feedback";
            string mText = "If you have found a bug or have a suggestion, please contact waffelkamilatus on Discord. Attaching a video and logs from Cuphead/BepInEx/LogOutput.log would help a lot too. Have fun! (or else...)";
            self.StartCoroutine(Other.notification_cr(self, tText, mText, 10f, 210f, false));
            showedFeedbackNotification = true;
        }
    }

    private void DifficultySelectUIAwake(MapDifficultySelectStartUI self)
    {
        MapDifficultySelectStartUI.Current = self;
        /*switch (Level.CurrentMode)
        {
        case Level.Mode.Easy:
            self.index = 0;
            break;
        case Level.Mode.Normal:
            self.index = 1;
            break;
        case Level.Mode.Hard:
            self.index = 2;
            break;
        }*/
        self.index = 0;//new
        self.options = new Level.Mode[]
        {
			//Level.Mode.Easy,
			//Level.Mode.Normal,
			Level.Mode.Hard
        };
        self.SetDifficultyAvailability();
        self.difficulyTexts = new TMP_Text[3];
        self.easy.GetComponent<TMP_Text>().SetText("");//new
        self.difficulyTexts[0] = self.easy.GetComponent<TMP_Text>();
        self.normal.GetComponent<TMP_Text>().SetText("CupaGoovno");//new
        self.difficulyTexts[1] = self.normal.GetComponent<TMP_Text>();
        self.hard.GetComponent<TMP_Text>().SetText("");//new
        self.difficulyTexts[2] = self.hard.GetComponent<TMP_Text>();
        if (self.bossImage != null && self.bossImage.textComponent != null)
        {
            self.initialMaxFontSize = self.bossImage.textComponent.resizeTextMaxSize;
        }
        self.initialinImagePosX = self.inAnimated.rectTransform.offsetMin;
        self.initialinImagePosY = self.inAnimated.rectTransform.offsetMax;
        self.initialinDifficultyPos = self.difficultyImage.rectTransform.anchoredPosition;
        self.initialDifficultyPos = self.difficultySelectionText.rectTransform.anchoredPosition;
        self.initialBossNamePos = self.bossNameImage.rectTransform.anchoredPosition;
        self.UpdateCursor();
    }

    public void StartScreenAwake(On.StartScreen.orig_Awake orig, StartScreen self)
    {
        orig(self);

        if (showedCompatibilityWarning)
        {
            return;
        }

        string gameRoot = Directory.GetCurrentDirectory();
        string pluginsPath = Path.Combine(gameRoot, "BepInEx\\plugins");
        string[] allowedFolders = { "Blender", "CupaGoovno" };

        if (!Directory.Exists(pluginsPath))
        {
            pluginsPath = Path.Combine(gameRoot, "BepInEx/plugins");
            if (!Directory.Exists(pluginsPath))
            {
                Plugin.Log("Couldn't find plugins folder. Can't verify mods compatibility.");
                return;
            }
        }

        string[] foundDirectories = Directory.GetDirectories(pluginsPath);
        string[] foundFiles = Directory.GetFiles(pluginsPath);
        List<string> unwanted = new List<string>();
        bool foundUnwanted = false;

        foreach (string dir in foundDirectories)
        {
            string folderName = new DirectoryInfo(dir).Name;
            if (Array.IndexOf(allowedFolders, folderName) == -1)
            {
                Plugin.Log($"Found unwanted folder: {folderName}");
                unwanted.Add(folderName);
                foundUnwanted = true;
            }
        }

        foreach (string file in foundFiles)
        {
            string fileName = Path.GetFileName(file);
            Plugin.Log($"Found unwanted file: {fileName}");
            unwanted.Add(fileName);
            foundUnwanted = true;
        }

        if(foundUnwanted)
        {
            string tText = "Other mods detected!";
            string mText = "Most mods will cause compatibility issues and break the game. Please remove the following mods: ";
            foreach (string item in unwanted)
            {
                if (item == unwanted.Last())
                {
                    mText += item + ". Then restart your game.";
                }
                else
                {
                    mText += item + ", ";
                }
            }
            self.StartCoroutine(Other.notification_cr(self, tText, mText, 10f, 210f, false));
            showedCompatibilityWarning = true;
        }
    }
    
    public static bool showedCompatibilityWarning = false;

    private static Level instance;
    private bool showedFeedbackNotification = false;
}
