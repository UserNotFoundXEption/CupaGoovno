using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class YoMamaFat
{
    public static bool loadFlowerPlatform = false;
    public static FlowerLevelPlatform flowerPlatform;

    public static bool loadDragonPotion = false;
    public static DragonLevelPotion dragonPotion;
    public static LevelProperties.Dragon.Potions dragonPotionProperties;

    public static bool loadDragofly = false;
    public static TreePlatformingLevelDragonfly dragonfly;

    public static bool loadUFOForBird = false;
    public static bool loadUFOForTrain = false;
    public static FlyingBlimpLevelUFO ufoA;
    public static FlyingBlimpLevelUFO ufoB;
    public static LevelProperties.FlyingBlimp.UFO ufoProperties;

    public static bool loadOrbForSlime = false;
    public static bool loadOrbForSally = false;
    public static bool loadOrbForDevil = false;
    public static TutorialLevelParryNext parryOrb;
    public static Transform parryOrbTransform;

    public static RobotLevelGemProjectile robotGemProjectile;
    public static GameObject robotShotBot;

    public static bool loadCowboyUFO = false;
    public static FlyingCowboyLevelUFO cowboyUfo;
    public static LevelProperties.FlyingCowboy.UFOEnemy cowboyUfoProperties;

    public static bool loadMouseFallingObject = false;
    public static MouseLevelFallingObject fallingObject;
    public static LevelProperties.Mouse.Claw fallingObjectProperties;

    public static SallyStagePlayLevelWindowProjectile sallyBottle;
    public static SallyStagePlayLevelWindowProjectile sallyRuler;
    public static SallyStagePlayLevel sallyParent;

    public static bool loadFollowerOrb = false;
    public static BeeLevelQueenFollower followerOrb;
    public static LevelProperties.Bee.Follower followerOrbProperties;

    public static bool loadCigarSpitForDomino = false;
    public static bool loadCigarSpitForKing = false;
    public static DicePalaceCigarLevelCigarSpit cigarSpit;
    public static LevelProperties.DicePalaceCigar cigarProperties;

    public static bool loadMermaidLaser = false;
    public static FlyingMermaidLevelLaser mermaidLaser;

    public static ClownLevelHorseshoe clownHorseshoe;
    public static LevelProperties.Clown clownProperties;

    public static PlanePlayerWeaponManager planeWeaponManager;

    public static int healerParryCounter;

    public static int oneHeartDamageBoost;
}
