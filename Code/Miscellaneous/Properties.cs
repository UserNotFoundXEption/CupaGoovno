using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CupaGoovno;

public class Properties
{
    public void Init()
    {
        On.LevelProperties.Veggies.GetMode += VeggiesGetMode;
        On.LevelProperties.Slime.GetMode += SlimeGetMode;
        On.LevelProperties.Slime.CreateTimeline += SlimeCreateTimeline;
        On.LevelProperties.Frogs.GetMode += FrogsGetMode;
        On.LevelProperties.Flower.GetMode += FlowerGetMode;
        On.LevelProperties.Flower.CreateTimeline += FlowerCreateTimeline;
        On.LevelProperties.FlyingBlimp.GetMode += BlimpGetMode;
        On.LevelProperties.FlyingBlimp.CreateTimeline += BlimpCreateTimeline;
        On.LevelProperties.Baroness.GetMode += BaronessGetMode;
        On.LevelProperties.Clown.GetMode += ClownGetMode;
        On.LevelProperties.Clown.CreateTimeline += ClownCreateTimeline;
        On.LevelProperties.Dragon.GetMode += DragonGetMode;
        On.LevelProperties.Dragon.CreateTimeline += DragonCreateTimeline;
        On.LevelProperties.FlyingBird.GetMode += BirdGetMode;
        On.LevelProperties.FlyingBird.CreateTimeline += BirdCreateTimeline;
        On.LevelProperties.FlyingGenie.GetMode += GenieGetMode;
        On.LevelProperties.FlyingGenie.CreateTimeline += GenieCreateTimeline;
        On.LevelProperties.Bee.GetMode += BeeGetMode;
        On.LevelProperties.Robot.GetMode += RobotGetMode;
        On.LevelProperties.Robot.CreateTimeline += RobotCreateTimeline;
        On.LevelProperties.Mouse.GetMode += MouseGetMode;
        On.LevelProperties.Mouse.CreateTimeline += MouseCreateTimeline;
        On.LevelProperties.Pirate.GetMode += PirateGetMode;
        On.LevelProperties.Pirate.CreateTimeline += PirateCreateTimeline;
        On.LevelProperties.SallyStagePlay.GetMode += SallyGetMode;
        On.LevelProperties.FlyingMermaid.GetMode += MermaidGetMode;
        On.LevelProperties.FlyingMermaid.CreateTimeline += MermaidCreateTimeline;
        On.LevelProperties.Train.GetMode += TrainGetMode;
        On.LevelProperties.DicePalaceMain.GetMode += KingDiceGetMode;
        On.LevelProperties.DicePalaceMain.CreateTimeline += KingDiceCreateTimeline;
        On.LevelProperties.DicePalaceRabbit.GetMode += RabbitGetMode;
        On.LevelProperties.DicePalaceRabbit.CreateTimeline += RabbitCreateTimeline;
        On.LevelProperties.DicePalaceRoulette.GetMode += RouletteGetMode;
        On.LevelProperties.DicePalaceRoulette.CreateTimeline += RouletteCreateTimeline;
        On.LevelProperties.DicePalaceDomino.GetMode += DominoGetMode;
        On.LevelProperties.DicePalaceDomino.CreateTimeline += DominoCreateTimeline;
        On.LevelProperties.DicePalaceCigar.GetMode += CigarGetMode;
        On.LevelProperties.DicePalaceCigar.CreateTimeline += CigarCreateTimeline;
        On.LevelProperties.DicePalaceBooze.GetMode += BoozeGetMode;
        On.LevelProperties.DicePalaceBooze.CreateTimeline += BoozeCreateTimeline;
        On.LevelProperties.DicePalaceChips.GetMode += ChipsGetMode;
        On.LevelProperties.DicePalaceChips.CreateTimeline += ChipsCreateTimeline;
        On.LevelProperties.DicePalaceEightBall.GetMode += BallGetMode;
        On.LevelProperties.DicePalaceEightBall.CreateTimeline += BallCreateTimeline;
        On.LevelProperties.DicePalaceFlyingMemory.GetMode += MonkeyGetMode;
        On.LevelProperties.DicePalaceFlyingHorse.GetMode += HorseGetMode;
        On.LevelProperties.DicePalaceFlyingHorse.CreateTimeline += HorseCreateTimeline;
        On.LevelProperties.Devil.GetMode+= DevilGetMode;
        On.LevelProperties.Devil.CreateTimeline += DevilCreateTimeline;
        On.LevelProperties.FlyingCowboy.GetMode += CowGetMode;
        On.LevelProperties.Graveyard.GetMode += GraveyardGetMode;
    }

    public static LevelProperties.Veggies VeggiesGetMode(On.LevelProperties.Veggies.orig_GetMode orig, Level.Mode mode)
    {
        int hp = 100;
        Level.GoalTimes goalTimes = null;
        List<LevelProperties.Veggies.State> list = new List<LevelProperties.Veggies.State>();
        goalTimes = new Level.GoalTimes(120f, 120f, 120f);

        int potatoHp = 400;
        float potatoIdleTime = 0.1f;//2.5
        int potatoSeriesCount = 3;
        float potatoSeriesDelay = 0.1f;//1
        int potatoBulletCount = 7;
        MinMax potatoBulletDelay = new MinMax(0.2f, 0.6f);
        float potatoBulletSpeed = -700f;

        int onionHp = 700;//425
        float onionHappyTime = 6f;
        MinMax onionCryLoops = new MinMax(38f, 38f);
        string[] onionTearPatterns = new string[]{
            "280,500,330,630,280,400,350,550,280,630,300",
            "630,280,400,500,320,450,280,630,505,330,630",
            "280,630,350,550,405,460,280,635,535,280,600",
            "330,480,630,280,520,630,280,350,450,280,570",
            "450,630,350,280,400,560,460,325,630,280,420",
            "630,450,280,330,550,415,630,580,280,500,330",
            "330,280,505,630,450,630,280,580,330,400,550"};
        float onionTearAnticipate = 0.1f;
        float onionTearCommaDelay = 0.005f;
        float onionTearTime = 1.5f;//0.9
        MinMax onionPinkTearRange = new MinMax(2137f, 2137f);//new MinMax(5f, 7f)
        float onionHeartMaxSpeed = 775f;
        float onionHeartAcceleration = 915f;
        float onionHeartBounceRatio = 2f;
        int onionHeartHP = 2137;//260

        int carrotHp = 600;//475
        float carrotStartIdleTime = 0.5f;//1.5
        MinMax carrotIdleRange = new MinMax(1.5f, 2.5f);//new MinMax(5f, 6f)
        int carrotBulletCount = 1;//2
        float carrotBulletDelay = 1.5f;
        float carrotBulletSpeed = 1000f;//850
        float carrotHomingInitDelay = 1f;
        float carrotHomingSpeed = 260f;
        float carrotHomingRotation = 2.3f;
        int carrotHomingHP = 4;
        float carrotHomingDelay = 0.8f;
        MinMax carrotHomingDuration = new MinMax(0f, 0f);
        float carrotHomingBgSpeed = 550f;
        MinMax carrotHomingNumOfCarrots = new MinMax(0f, 0f);//new MinMax(5f, 7f)

        LevelProperties.Veggies.Potato potato = new LevelProperties.Veggies.Potato(potatoHp, potatoIdleTime, potatoSeriesCount, potatoSeriesDelay, potatoBulletCount, potatoBulletDelay, potatoBulletSpeed);
        LevelProperties.Veggies.Onion onion = new LevelProperties.Veggies.Onion(onionHp, onionHappyTime, onionCryLoops, onionTearPatterns, onionTearAnticipate, onionTearCommaDelay, onionTearTime, onionPinkTearRange, onionHeartMaxSpeed, onionHeartAcceleration, onionHeartBounceRatio, onionHeartHP);
        LevelProperties.Veggies.Beet beet = new LevelProperties.Veggies.Beet(2137, 0f, new string[0], 0f, 0, 0f, 0f, 0f, new MinMax(0f, 1f));
        LevelProperties.Veggies.Peas peas = new LevelProperties.Veggies.Peas(0);
        LevelProperties.Veggies.Carrot carrot = new LevelProperties.Veggies.Carrot(carrotHp, carrotStartIdleTime, carrotIdleRange, carrotBulletCount, carrotBulletDelay, carrotBulletSpeed, carrotHomingInitDelay, carrotHomingSpeed, carrotHomingRotation, carrotHomingHP, carrotHomingDelay, carrotHomingDuration, carrotHomingBgSpeed, carrotHomingNumOfCarrots);

        list.Add(new LevelProperties.Veggies.State(10f, new LevelProperties.Veggies.Pattern[][]{
            new LevelProperties.Veggies.Pattern[0]}, LevelProperties.Veggies.States.Main,
            potato, onion, beet, peas, carrot));

        return new LevelProperties.Veggies(hp, goalTimes, list.ToArray());
    }

    public static LevelProperties.Slime SlimeGetMode(On.LevelProperties.Slime.orig_GetMode orig, Level.Mode mode)
    {
        int hp = 2000;
        Level.GoalTimes goalTimes = null;
        List<LevelProperties.Slime.State> list = new List<LevelProperties.Slime.State>();
        goalTimes = new Level.GoalTimes(120f, 120f, 120f);

        float sjGroundDelay = 0.2f;
        float sjHighJumpVerticalSpeed = 2500f;//2350f
        float sjHighJumpHorizontalSpeed = 1500f;//840f
        float sjHighJumpGravity = 6000f;//6500f
        float sjLowJumpVerticalSpeed = 2000f;
        float sjLowJumpHorizontalSpeed = 2000f;//780f
        float sjLowJumpGravity = 40000f;//6500f
        MinMax sjNumJumps = new MinMax(5f, 8f);
        string sjPatternString = "HJ,LJ,HJ,LJ,LJ,RJ,HJ,LJ,HJ,LJ,LJ,HJ,RJ,LJ,LJ";
        int sjBigSlimeInitialJumpPunchCount = 3;

        float spPreHold = 0.3f;//0.65f
        float spMainHold = 0.1f;//0.4f

        float bjGroundDelay = 0.35f;//0.23
        float bjHighJumpVerticalSpeed = 2500f;//2850f
        float bjHighJumpHorizontalSpeed = 1500f;//820f
        float bjHighJumpGravity = 6000f;//7500f
        float bjLowJumpVerticalSpeed = 2000f;//2250
        float bjLowJumpHorizontalSpeed = 2000f;//760f
        float bjLowJumpGravity = 40000f;//7500f
        MinMax bjNumJumps = new MinMax(5f, 8f);
        string bjPatternString = "HJ,HJ,LJ,HJ,RJ,LJ,LJ,HL,LJ,RJ,HJ,LJ,LJ,HJ,LJ,LJ";
        int bjBigSlimeInitialJumpPunchCount = 2;

        float bpPreHold = 0.3f;//0.65f
        float bpMainHold = 0.1f;//0.4f

        float tombMoveSpeed = 2137f;//950
        MinMax tombAttackDelay = new MinMax(1f, 2f);//new MinMax(2f, 4f);
        float tombAnticipationHold = 0.25f;//0.35
        string tombAttackOffsetString = "-130,70,0,130,-70,0";
        float tombTinyMeltDelay = 0.2f;
        float tombTinyRunTime = 3.1f;
        float tombTinyHealth = 3.9f;
        float tombTinyTimeUntilUnmelt = 3.4f;

        LevelProperties.Slime.Jump smallJump = new LevelProperties.Slime.Jump(sjGroundDelay, sjHighJumpVerticalSpeed, sjHighJumpHorizontalSpeed, sjHighJumpGravity, sjLowJumpVerticalSpeed, sjLowJumpHorizontalSpeed, sjLowJumpGravity, sjNumJumps, sjPatternString, sjBigSlimeInitialJumpPunchCount);
        LevelProperties.Slime.Jump bigJump = new LevelProperties.Slime.Jump(bjGroundDelay, bjHighJumpVerticalSpeed, bjHighJumpHorizontalSpeed, bjHighJumpGravity, bjLowJumpVerticalSpeed, bjLowJumpHorizontalSpeed, bjLowJumpGravity, bjNumJumps, bjPatternString, bjBigSlimeInitialJumpPunchCount);
        LevelProperties.Slime.Punch smallPunch = new LevelProperties.Slime.Punch(spPreHold, spMainHold);
        LevelProperties.Slime.Punch bigPunch = new LevelProperties.Slime.Punch(bpPreHold, bpMainHold);
        LevelProperties.Slime.Tombstone tombstone = new LevelProperties.Slime.Tombstone(tombMoveSpeed, tombAttackDelay, tombAnticipationHold, tombAttackOffsetString, tombTinyMeltDelay, tombTinyRunTime, tombTinyHealth, tombTinyTimeUntilUnmelt);

        list.Add(new LevelProperties.Slime.State(10f, new LevelProperties.Slime.Pattern[][]{
        new LevelProperties.Slime.Pattern[1]}, LevelProperties.Slime.States.Main,
        smallJump, smallPunch, tombstone));

        list.Add(new LevelProperties.Slime.State(0.76f, new LevelProperties.Slime.Pattern[][]{
        new LevelProperties.Slime.Pattern[0]}, LevelProperties.Slime.States.BigSlime,
        bigJump, bigPunch, tombstone));

        list.Add(new LevelProperties.Slime.State(0.31f, new LevelProperties.Slime.Pattern[][]{
        new LevelProperties.Slime.Pattern[0]}, LevelProperties.Slime.States.Tombstone,
        bigJump, bigPunch, tombstone));

        return new LevelProperties.Slime(hp, goalTimes, list.ToArray());
    }

    public Level.Timeline SlimeCreateTimeline(On.LevelProperties.Slime.orig_CreateTimeline orig, LevelProperties.Slime self, Level.Mode mode)
    {
        Level.Timeline timeline = new Level.Timeline();
        timeline.health = 2000f;
        timeline.events.Add(new Level.Timeline.Event("BigSlime", 0.76f));
        timeline.events.Add(new Level.Timeline.Event("Tombstone", 0.31f));
        return timeline;
    }

    public static LevelProperties.Frogs FrogsGetMode(On.LevelProperties.Frogs.orig_GetMode orig, Level.Mode mode)
    {
        /*new LevelProperties.Frogs.ShortRage(1f, 900f, 0.5f, 5, new string[]
			{
			"RRPRP",
			"RPRPR",
			"PRRRP"
			}, 2f)*/
        //new LevelProperties.Frogs.ShortRoll(1.5f, 1.5f, 0.8f, 0.1f)
        /*clapstring{
				"S:1, D:1.5, S:1, D:1.8, S:1",
				"S:1, D:1.7, S:1",
				"S:1, D:1.6, S:1, D:1.7, S:1",
				"S:1, D:2, S:1",
				"S:1, D:1.4, S:1, D:1.9, S:1",
				"S:1, D:2, S:1"
			}*/

        LevelProperties.Frogs.Pattern pRage = LevelProperties.Frogs.Pattern.ShortRage;
        LevelProperties.Frogs.Pattern pClap = LevelProperties.Frogs.Pattern.ShortClap;

        int hp = 1900;

        float fanPower = -410f;
        int fanAccelerationTime = 1;//2
        MinMax fanDuration = new MinMax(2.5f, 3.5f);
        int fanHesitate = 0;

        string[] fireflies = new string[]
        {
            "D:1, S:2, D:2.5, S:2, D:1.5, S:1",
            "D:0.5, S:1, D:2, S:1, D:1.5, S:2",
            "D:1, S:2, D:2.2, S:2",
            "D:1, S:2, D:2, S:1, D:1, S:1",
            "D:0.5, S:1, D:2.5, S:2, D:2.1, S:2",
            "D:1.4, S:2, D:2.4, S:2",
            "D:1, S:1, D:1.8, S:2, D:1.5, S:1"
            };
        float fliesSpeed = 1000f;
        float fliesFollowTime = 0.4f;
        float fliesFollowDelay = 1f;
        float fliesFollowDistance = 180;
        int fliesHp = 4;
        float fliesHesitate = 2f;
        float fliesInvincibleDuration = 0.15f;

        float rageAnticipation = 1f;
        float rageSpeed = 900;
        float rageDelay = 0.18f;
        int rageCount = 100;
        string[] rage = new string[] { "RPR" };
        float rageHesitate = 2f;

        float rollDelay = 0f;
        float rollTime = 0.5f;
        float rollReturn = 0.2f;
        float rollHesitate = 0.1f;

        string[] clap = new string[] { "S:1, D:0.5, S:1, D:0.5, S:1, D:1, S:1, D:1.5, S:1, D:0.5, S:1, D:1.5, S:1, D:0.5, S:1" };
        float[] clapAngles = new float[] { 73f, 68f, 70f, 75f, 76f, 69f, 74f, 70f, 67f, 77f, 72f, 67f };
        float clapSpeed = 1030f;
        float clapDelay = 0.8f;
        float clapHesitate = 1f;

        float MorthArmDownDelay = 3f;
        float slotSelection = 0.7f;
        MinMax coinSpeed = new MinMax(800f, 1000f);
        MinMax coinDelay = new MinMax(0.7f, 0.5f);
        float coinTime = 5f;
        MinMax snakeSpeed = new MinMax(700f, 1150f);
        MinMax snakeDelay = new MinMax(0.7f, 0.3f);
        float snakeTime = 6f;
        float snakeDuration = 10f;
        MinMax bisonSpeed = new MinMax(600f, 920f);
        MinMax bisonDelay = new MinMax(1f, 0.65f);
        float bisonTime = 6f;
        float bisonSmallX = 600f;
        float bisonBigX = 300f;
        int bisonDuration = 10;
        float tigerSpeed = 488f;
        MinMax tigerDelay = new MinMax(1.2f, 0.7f);
        float tigerTime = 6f;
        float tigerDuration = 10f;

        float demonHeight = 8f;
        float demonParryHeight = 15f;
        //MinMax demonSpeed = new MinMax(530f, 690f);
        MinMax demonSpeed = new MinMax(500f, 1000f);
        //MinMax demonDelay = new MinMax(1.2f, 0.7f);
        MinMax demonDelay = new MinMax(0.8f, 0.4f);
        float demonTime = 13f;
        string[] demonString = new string[]
        {
            "S,S,T,S,B,S,S,S,O,S,S,T",
            "S,T,B,S,S,O,S,B,T,S,S,B",
            "S,S,O,S,T,B,S,S,B,T,S,O",
            "S,S,B,S,B,S,S,T,S,O,S,S,B",
            "S,S,T,B,S,S,B,S,O,S,S,B",
            "S,S,T,S,B,O,S,S,B,S,S,T",
            "S,S,T,S,T,S,S,B,O,S,B,S",
            "S,S,O,S,T,S,S,B,B,S,S,O"
        };

        LevelProperties.Frogs.TallFan tallFan = new LevelProperties.Frogs.TallFan(fanPower, fanAccelerationTime, fanDuration, fanHesitate);
        LevelProperties.Frogs.TallFireflies tallFireflies = new LevelProperties.Frogs.TallFireflies(fireflies, fliesSpeed, fliesFollowTime, fliesFollowDelay, fliesFollowDistance, fliesHp, fliesHesitate, fliesInvincibleDuration);
        LevelProperties.Frogs.ShortRage shortRage = new LevelProperties.Frogs.ShortRage(rageAnticipation, rageSpeed, rageDelay, rageCount, rage, rageHesitate);
        LevelProperties.Frogs.ShortRoll shortRoll = new LevelProperties.Frogs.ShortRoll(rollDelay, rollTime, rollReturn, rollHesitate);
        LevelProperties.Frogs.ShortClap shortClap = new LevelProperties.Frogs.ShortClap(clap, clapAngles, clapSpeed, clapDelay, clapHesitate);
        LevelProperties.Frogs.Morph morph = new LevelProperties.Frogs.Morph(MorthArmDownDelay, slotSelection, coinSpeed, coinDelay, coinTime, snakeSpeed, snakeDelay, snakeTime, snakeDuration, bisonSpeed, bisonDelay, bisonTime, bisonSmallX, bisonBigX, bisonDuration, tigerSpeed, tigerDelay, tigerTime, tigerDuration);
        LevelProperties.Frogs.Demon demon = new LevelProperties.Frogs.Demon(demonHeight, demonParryHeight, demonSpeed, demonDelay, demonTime, demonString);

        Level.GoalTimes goalTimes = null;
        List<LevelProperties.Frogs.State> list = new List<LevelProperties.Frogs.State>();
        goalTimes = new Level.GoalTimes(120f, 120f, 120f);

        //First
        list.Add(new LevelProperties.Frogs.State(10f, new LevelProperties.Frogs.Pattern[][]{new LevelProperties.Frogs.Pattern[]
        {LevelProperties.Frogs.Pattern.RagePlusFireflies}}, LevelProperties.Frogs.States.Main,
        tallFan, tallFireflies, shortRage, shortRoll, shortClap, morph, demon));

        //Second
        list.Add(new LevelProperties.Frogs.State(0.74f, new LevelProperties.Frogs.Pattern[][]{
        new LevelProperties.Frogs.Pattern[]{
        pRage, pClap, pRage, pClap, pRage, pClap, pRage, pClap, pClap}}, LevelProperties.Frogs.States.Roll,
        tallFan, tallFireflies, shortRage, shortRoll, shortClap, morph, demon));

        //Morphed
        list.Add(new LevelProperties.Frogs.State(0.35f, new LevelProperties.Frogs.Pattern[][]{new LevelProperties.Frogs.Pattern[0]
        }, LevelProperties.Frogs.States.Morph,
        tallFan, tallFireflies, shortRage, shortRoll, shortClap, morph, demon));

        return new LevelProperties.Frogs(hp, goalTimes, list.ToArray());
    }

    public static LevelProperties.Flower FlowerGetMode(On.LevelProperties.Flower.orig_GetMode orig, Level.Mode mode)
    {
        int hp = 2000;//1500
        Level.GoalTimes goalTimes = null;
        List<LevelProperties.Flower.State> list = new List<LevelProperties.Flower.State>();
        goalTimes = new Level.GoalTimes(120f, 120f, 120f);

        float gattlingTime = 3f;
        float initialSeedDelay = 0.5f;
        float hesitateAfterAttack = 0.5f;//1
        float fallingSeedDelay = 0.6f;//0.7
        string[] seedStrings = new string[]
        {/*
				"A50,A350,C600,A100,A700",
				"A250,A150,A500,C650",
				"A700,A450,A550,A200,C50",
				"A100,A250,A150,C600",
				"A650,A500,A700,C150,A50",
				"C300,A450,A250,A400,A550",
				"A700,A500,C300,A100",
				"A450,A650,C150,A550,A350"*/
            "B0,B450,B700,B500,C200",
            "B100,B50,C450,A300,B750",
            "B600,A500,C650,A350,A0",
            "C50,B650,B500,B700,A250",
            "A600,C350,A100,B50,A250",
            "C350,A650,A0,B100,A500",
            "A150,B350,B50,C700,A550",
            "A0,C550,A150,A750,B250"
        };

        LevelProperties.Flower.Laser laser = new LevelProperties.Flower.Laser(1f, 1.5f, "T,B,B,B,T,B,B,T,B", 1f);
        LevelProperties.Flower.PodHands podHands = new LevelProperties.Flower.PodHands(1f, 1f, 2.5f, "3,2,3,2,2,2,3,3,2,3,2,2,2,2,3,3,3,2", "B,S,S,B,B,S,B,S,S,S,B,B,S,B,S");
        LevelProperties.Flower.Boomerang boomerang = new LevelProperties.Flower.Boomerang(820, 0.6f, 1.5f, 0.4f);
        LevelProperties.Flower.Bullets bullets = new LevelProperties.Flower.Bullets(0.3f, new MinMax(100f, 700f), 1.8f, 0, 4);
        LevelProperties.Flower.PuffUp puffUp = new LevelProperties.Flower.PuffUp(400, 1f, 5);
        LevelProperties.Flower.GattlingGun gattlingGun = new LevelProperties.Flower.GattlingGun(gattlingTime, initialSeedDelay, hesitateAfterAttack, fallingSeedDelay, seedStrings);
        LevelProperties.Flower.EnemyPlants enemyPlants = new LevelProperties.Flower.EnemyPlants(8, 10, 50, 5, 0.9f, 250, 4, 4, new MinMax(4f, 7f), 400, 1, 25);
        //			new LevelProperties.Flower.EnemyPlants(8, 10, 350, 2, 0.9f, 140, 4, 4, new MinMax(4f, 7f), 400, 1, 25)
        LevelProperties.Flower.VineHands vineHands = new LevelProperties.Flower.VineHands(0.6f, 0.2f, new MinMax(3.3f, 5.7f), "1,2,3,2,1,3");
        LevelProperties.Flower.PollenSpit pollenSpit = new LevelProperties.Flower.PollenSpit("1,2,D0.5,1,1", 1.5f, "R,R,P,R,R,R,P", 2.3f, 330, 50f);

        list.Add(new LevelProperties.Flower.State(10f, new LevelProperties.Flower.Pattern[][]{new LevelProperties.Flower.Pattern[]
        {LevelProperties.Flower.Pattern.GattlingGun}}, LevelProperties.Flower.States.Main,
        laser, podHands, boomerang, bullets, puffUp, gattlingGun, enemyPlants, vineHands, pollenSpit));

        list.Add(new LevelProperties.Flower.State(0.73f, new LevelProperties.Flower.Pattern[][]{new LevelProperties.Flower.Pattern[]
        {LevelProperties.Flower.Pattern.PodHands}}, LevelProperties.Flower.States.Generic,
        laser, podHands, boomerang, bullets, puffUp, gattlingGun, enemyPlants, vineHands, pollenSpit));

        list.Add(new LevelProperties.Flower.State(0.46f, new LevelProperties.Flower.Pattern[][]{new LevelProperties.Flower.Pattern[0]
        }, LevelProperties.Flower.States.PhaseTwo,
        laser, podHands, boomerang, bullets, puffUp, gattlingGun, enemyPlants, vineHands, pollenSpit));

        return new LevelProperties.Flower(hp, goalTimes, list.ToArray());
    }

    public Level.Timeline FlowerCreateTimeline(On.LevelProperties.Flower.orig_CreateTimeline orig, LevelProperties.Flower self, Level.Mode mode)
    {
        Level.Timeline timeline = new Level.Timeline();
        timeline.health = 2000f;
        //timeline.events.Add(new Level.Timeline.Event("Generic", 0.73f));
        timeline.events.Add(new Level.Timeline.Event("PhaseTwo", 0.46f));
        return timeline;
    }

    public static LevelProperties.FlyingBlimp BlimpGetMode(On.LevelProperties.FlyingBlimp.orig_GetMode orig, Level.Mode mode)
    {
        int hp = 3000;
        Level.GoalTimes goalTimes = null;
        List<LevelProperties.FlyingBlimp.State> list = new List<LevelProperties.FlyingBlimp.State>();
        goalTimes = new Level.GoalTimes(120f, 120f, 120f);

        float moveSpeed = 5.1f;
        MinMax moveInitialAttackDelay = new MinMax(1.3f, 2.8f);

        string[] dashSummonString = new string[] { "D0.5,1" };
        float dashHold = 0.1f;//1
        float dashSpeed = 3500f;//2700
        float dashReentryDelay = 0.1f;
        float dashSummonSpeed = 3500f;//780
        float dashHesitate = 0.1f;//3

        int enemyHp = 2137;//10
        float enemySpeed = 805f;
        float enemyShotDelay = 0.4f;
        MinMax enemyStopDistance = new MinMax(10f, 80f);
        float enemyStringDelay = 3f;//3.6
        float enemyStringDelaySagittarius = 1f;//3.6
        float enemyStringDelayGemini = 2f;//3.6
        string[] enemySpawnString = new string[] { "600,200,500,300,100,550,150,450,250,50" };
        string[] enemyTypeString = new string[] { "A,B,A" };
        MinMax enemyBSpreadAngle = new MinMax(15f, 75f);//MinMax(0f, 90f);
        int enemyBBulletsNumber = 7;//5
        float enemyASpeed = 575f;
        float enemyBSpeed = 525f;
        MinMax enemyPinkOccurance = new MinMax(1f, 1f);

        float tornadoMoveSpeed = 60f;//600
        float tornadoHomingSpeed = 0.2f;//1.05f
        float tornadoLoopDuration = 1f;
        float tornadoHesitateAfterAttack = 2.6f;

        float shootSpeedMin = 450f;
        float shootSpeedMax = 1800f;
        float shootAccelerationTime = 1450f;
        MinMax shootHesitateAfterAttack = new MinMax(1.2f, 1.5f);

        float morphCrazyAHold = 0.1f;//0.4
        float morphCrazyBHold = 0.1f;//2.3

        MinMax starsSpeedX = new MinMax(350f, 800f);
        float starsSpeedY = 5f;
        float starsSineY = 5f;
        float starsDelay = 0.4f;//0.9
        string[] starsTypeString = new string[]{
            "A,B,C,A,C,A,A,B,C,C,A,B,C,B,A,A,P",
            "B,C,A,A,C,B,A,B,B,C,A,B,C,C,B,A,P"};
        string[] starsPositionString = new string[]{
            "100,300,400,500,600,200,350,550,50,600",
            "300,600,400,50,200,500,50,550,450,150"};

        int sagArrowHp = 10;
        int sagMovementSpeed = 3;
        MinMax sagAttackDelayRange = new MinMax(1.0f, 1.5f);//MinMax(3f, 5.1f);
        float sagArrowInitialSpeed = 755f;
        float sagArrowWarning = 0.5f;
        MinMax sagHomingSpreadAngle = new MinMax(0f, 90f);
        float sagHomingDelay = 1.45f;
        float sagHomingSpeed = 495f;
        float sagHomingRotation = 2.5f;
        MinMax sagHomingDurationRange = new MinMax(5f, 8f);

        MinMax taurusAttackDelay = new MinMax(1.0f, 2.0f);//MinMax(1.7f, 3.9f);
        float taurusMovementSpeed = 6f;//4

        MinMax geminiSpawnerDelay = new MinMax(0.5f, 1.0f);//MinMax(1.5f, 3f);
        float geminiSpawnerSpeed = 1f;
        float geminiBulletDelay = 5f;
        float geminiRotationSpeed = 0.25f;//0.3
        float geminiBulletSpeed = 300f;

        float ufoHp = 9999f;
        float ufoSpeed = 325f;
        float ufoAProximity = 140f;
        float ufoBProximity = 340f;
        float ufoBeamDuration = 0.2f;//0.65
        float ufoDelay = 1.2f;//1.6
        string[] ufoString = new string[]{
            "A,B,A,A,B,B,A",
            "B,A,B,B,A,A,B",
            "A,A,B,A,A,B,B",
            "B,B,A,B,A,A,A",
            "A,B,A,B,A,A,B",
            "B,A,A,B,A,A,B"};
        float ufoWarningBeamDuration = 0.3f;
        float ufoInitialDelay = 0.5f;
        float ufoMoonAttackAnticipation = 1.5f;
        float ufoMoonAttackDuration = 2137f;//8
        float ufoMoonWaitForNextAttack = 2f;
        bool ufoInvincible = false;

        float ufoBirdSpeed = 325f;
        float ufoBirdBeamDuration = 0.65f;
        float ufoBirdDelay = 2.5f;

        LevelProperties.FlyingBlimp.Move move = new LevelProperties.FlyingBlimp.Move(moveSpeed, moveInitialAttackDelay);
        LevelProperties.FlyingBlimp.Enemy enemy = new LevelProperties.FlyingBlimp.Enemy(true, enemyHp, enemySpeed, enemyShotDelay, enemyStopDistance, enemyStringDelay, enemySpawnString, enemyTypeString, enemyBSpreadAngle, enemyBBulletsNumber, enemyASpeed, enemyBSpeed, enemyPinkOccurance);
        LevelProperties.FlyingBlimp.Enemy enemySagittarius = new LevelProperties.FlyingBlimp.Enemy(true, enemyHp, enemySpeed, enemyShotDelay, enemyStopDistance, enemyStringDelaySagittarius, enemySpawnString, enemyTypeString, enemyBSpreadAngle, enemyBBulletsNumber, enemyASpeed, enemyBSpeed, enemyPinkOccurance);
        LevelProperties.FlyingBlimp.Enemy enemyGemini = new LevelProperties.FlyingBlimp.Enemy(true, enemyHp, enemySpeed, enemyShotDelay, enemyStopDistance, enemyStringDelayGemini, enemySpawnString, enemyTypeString, enemyBSpreadAngle, enemyBBulletsNumber, enemyASpeed, enemyBSpeed, enemyPinkOccurance);
        LevelProperties.FlyingBlimp.Tornado tornado = new LevelProperties.FlyingBlimp.Tornado(tornadoMoveSpeed, tornadoHomingSpeed, tornadoLoopDuration, tornadoHesitateAfterAttack);
        LevelProperties.FlyingBlimp.Shoot shoot = new LevelProperties.FlyingBlimp.Shoot(shootSpeedMin, shootSpeedMax, shootAccelerationTime, shootHesitateAfterAttack);
        LevelProperties.FlyingBlimp.Morph morph = new LevelProperties.FlyingBlimp.Morph(morphCrazyAHold, morphCrazyBHold);
        LevelProperties.FlyingBlimp.Stars stars = new LevelProperties.FlyingBlimp.Stars(starsSpeedX, starsSpeedY, starsSineY, starsDelay, starsTypeString, starsPositionString);
        LevelProperties.FlyingBlimp.Sagittarius sag = new LevelProperties.FlyingBlimp.Sagittarius(sagArrowHp, sagMovementSpeed, sagAttackDelayRange, sagArrowInitialSpeed, sagArrowWarning, sagHomingSpreadAngle, sagHomingDelay, sagHomingSpeed, sagHomingRotation, sagHomingDurationRange);
        LevelProperties.FlyingBlimp.Taurus taurus = new LevelProperties.FlyingBlimp.Taurus(taurusAttackDelay, taurusMovementSpeed);
        LevelProperties.FlyingBlimp.Gemini gemini = new LevelProperties.FlyingBlimp.Gemini(geminiSpawnerDelay, geminiSpawnerSpeed, geminiBulletDelay, geminiRotationSpeed, geminiBulletSpeed);
        LevelProperties.FlyingBlimp.UFO ufo = new LevelProperties.FlyingBlimp.UFO(ufoHp, ufoSpeed, ufoAProximity, ufoBProximity, ufoBeamDuration, ufoDelay, ufoString, ufoWarningBeamDuration, ufoInitialDelay, ufoMoonAttackAnticipation, ufoMoonAttackDuration, ufoMoonWaitForNextAttack, ufoInvincible);
        LevelProperties.FlyingBlimp.UFO ufoBird = new LevelProperties.FlyingBlimp.UFO(ufoHp, ufoBirdSpeed, ufoAProximity, ufoBProximity, ufoBirdBeamDuration, ufoBirdDelay, ufoString, ufoWarningBeamDuration, ufoInitialDelay, ufoMoonAttackAnticipation, ufoMoonAttackDuration, ufoMoonWaitForNextAttack, ufoInvincible);
        LevelProperties.FlyingBlimp.Gear gear = new LevelProperties.FlyingBlimp.Gear(2, -255f, 1100f);
        LevelProperties.FlyingBlimp.DashSummon dash = new LevelProperties.FlyingBlimp.DashSummon(dashSummonString, dashHold, dashSpeed, dashReentryDelay, dashSummonSpeed, dashHesitate);

        LevelProperties.FlyingBlimp.Pattern[][] mainPattern = new LevelProperties.FlyingBlimp.Pattern[][]{
        new LevelProperties.FlyingBlimp.Pattern[]{LevelProperties.FlyingBlimp.Pattern.Shoot, LevelProperties.FlyingBlimp.Pattern.Shoot, LevelProperties.FlyingBlimp.Pattern.Shoot}};
        LevelProperties.FlyingBlimp.Pattern[][] genericPattern = new LevelProperties.FlyingBlimp.Pattern[][]{
        new LevelProperties.FlyingBlimp.Pattern[]{LevelProperties.FlyingBlimp.Pattern.Shoot,
        LevelProperties.FlyingBlimp.Pattern.Tornado, LevelProperties.FlyingBlimp.Pattern.Shoot,
        LevelProperties.FlyingBlimp.Pattern.Shoot, LevelProperties.FlyingBlimp.Pattern.Tornado}};
        LevelProperties.FlyingBlimp.Pattern[][] otherPattern = new LevelProperties.FlyingBlimp.Pattern[][] { new LevelProperties.FlyingBlimp.Pattern[0] };

        //Main
        /*list.Add(new LevelProperties.FlyingBlimp.State(10f, mainPattern, LevelProperties.FlyingBlimp.States.Main,
			move, dash, enemy, tornado, shoot, morph, stars, sag, taurus, gemini, ufo, gear));*/

        //Generic
        list.Add(new LevelProperties.FlyingBlimp.State(10f, genericPattern, LevelProperties.FlyingBlimp.States.Generic,
        move, dash, enemy, tornado, shoot, morph, stars, sag, taurus, gemini, ufoBird, gear));

        //Sag
        list.Add(new LevelProperties.FlyingBlimp.State(0.9f, otherPattern, LevelProperties.FlyingBlimp.States.Sagittarius,
           move, dash, enemySagittarius, tornado, shoot, morph, stars, sag, taurus, gemini, ufo, gear));

        //Generic
        list.Add(new LevelProperties.FlyingBlimp.State(0.78f, genericPattern, LevelProperties.FlyingBlimp.States.Generic,
        move, dash, enemy, tornado, shoot, morph, stars, sag, taurus, gemini, ufo, gear));

        //Taurus
        list.Add(new LevelProperties.FlyingBlimp.State(0.68f, otherPattern, LevelProperties.FlyingBlimp.States.Taurus,
        move, dash, enemy, tornado, shoot, morph, stars, sag, taurus, gemini, ufo, gear));

        //Generic
        list.Add(new LevelProperties.FlyingBlimp.State(0.56f, genericPattern, LevelProperties.FlyingBlimp.States.Generic,
        move, dash, enemy, tornado, shoot, morph, stars, sag, taurus, gemini, ufo, gear));

        //Gemini
        list.Add(new LevelProperties.FlyingBlimp.State(0.46f, otherPattern, LevelProperties.FlyingBlimp.States.Gemini,
        move, dash, enemyGemini, tornado, shoot, morph, stars, sag, taurus, gemini, ufo, gear));

        //Generic
        list.Add(new LevelProperties.FlyingBlimp.State(0.34f, genericPattern, LevelProperties.FlyingBlimp.States.Generic,
        move, dash, enemy, tornado, shoot, morph, stars, sag, taurus, gemini, ufo, gear));

        //Moon
        list.Add(new LevelProperties.FlyingBlimp.State(0.24f, otherPattern, LevelProperties.FlyingBlimp.States.Moon,
           move, dash, enemy, tornado, shoot, morph, stars, sag, taurus, gemini, ufo, gear));

        return new LevelProperties.FlyingBlimp(hp, goalTimes, list.ToArray());
    }

    public Level.Timeline BlimpCreateTimeline(On.LevelProperties.FlyingBlimp.orig_CreateTimeline orig, LevelProperties.FlyingBlimp self, Level.Mode mode)
    {
        Level.Timeline timeline = new Level.Timeline();
        timeline.health = 3000f;
        /*timeline.events.Add(new Level.Timeline.Event("Gemini", 0.95f));
        timeline.events.Add(new Level.Timeline.Event("Generic", 0.8f));
        timeline.events.Add(new Level.Timeline.Event("Sagittarius", 0.67f));
        timeline.events.Add(new Level.Timeline.Event("Generic", 0.51f));
        timeline.events.Add(new Level.Timeline.Event("Moon", 0.39f));*/
        timeline.events.Add(new Level.Timeline.Event("Sagittarius", 0.9f));//new start
        timeline.events.Add(new Level.Timeline.Event("Generic", 0.78f));
        timeline.events.Add(new Level.Timeline.Event("Taurus", 0.68f));
        timeline.events.Add(new Level.Timeline.Event("Generic", 0.56f));
        timeline.events.Add(new Level.Timeline.Event("Gemini", 0.46f));
        timeline.events.Add(new Level.Timeline.Event("Generic", 0.36f));
        timeline.events.Add(new Level.Timeline.Event("Moon", 0.24f));//new end
        return timeline;
    }

    public static LevelProperties.Baroness BaronessGetMode(On.LevelProperties.Baroness.orig_GetMode orig, Level.Mode mode)
    {
        int hp = 530;
        Level.GoalTimes goalTimes = null;
        List<LevelProperties.Baroness.State> list = new List<LevelProperties.Baroness.State>();
        goalTimes = new Level.GoalTimes(120f, 120f, 120f);

        int baronessHp = 0;
        float minibossStart = 1f;//3
        string[] baronessTimeString = new string[] { "5,7,9,7,8,6,7" };
        MinMax baronessAttackDelay = new MinMax(2f, 3.2f);
        MinMax baronessAttackCount = new MinMax(1f, 1f);
        int baronessProjHp = 3;
        float baronessProjRotation = 100f;
        int baronessProjSpeed = 300;
        float baronessFinalProjSpeed = 620f;
        float baronessFinalProjDuration = 0.61f;
        float baronessFinalProjRedirectDelay = 0.45f;
        float baronessFinalProjRedirectCount = 6f;
        MinMax baronessFinalProjDelay = new MinMax(3.5f, 6f);
        float baronessFinalProjInitialDelay = 1.5f;
        string baronessFinalProjHeadString = "H,H,D:2.4,H,H,D:2.7,H,H,D:3";

        int minibossCount = 5;
        string[] minibossString = new string[] { "1,2,3,4,5" };

        int jellyHp = 2137; //7
        float jellySpeed = 315f;
        float jellyHeight = 60f;
        float jellyJumpSpeed = 1000f; //550f
        MinMax jellyJumpHeight = new MinMax(100f, 101f); //new MinMax(-101f, -100f)
        float jellyAfterJumpDuration = 100f; //1f
        string[] jellyType = new string[]{
            "R,R,P,R,P,P,R,R,P",
            "R,P,R,P,R,R,P",
            "P,P,R,R,P,R,R,P"};
        MinMax jellySpawnDelay = new MinMax(5f, 6.8f);
        float jellyStartingPoint = 1f;
        float jellyDelayChangePercent = 5f;

        int gumHp = 500; //320
        float gumSpeed = 12f; //1.85
        float gumDeathSpeed = 5000f;//500
        MinMax gumOff = new MinMax(2.5f, 3f); //new MinMax(2f, 3.5f)
        MinMax gumOn = new MinMax(1.5f, 2f); //new MinMax(4.3f, 5.7f)
        float gumGravity = 950f;
        MinMax gumVX = new MinMax(-260f, 600f); //MinMax(-260f, 450f)
        float gumFireRate = 0.1f; //0.16f
        MinMax gumVY = new MinMax(600f, 850f);
        MinMax gumOffsetX = new MinMax(0f, 200f); //new MinMax(330f, 600f)

        int waffleHp = 450;//305
        float waffleSpeed = 1f; //1.8
        float waffleAnticipation = 1f;
        MinMax waffleDelay = new MinMax(0.5f, 1.5f);//new MinMax(2f, 5f);
        float waffleExplodeSpeed = 5f; //3
        float waffleReturn = 0.1f; //1
        float waffleExplodeDistance = 2000f; //450
        float waffleReturnSpeed = 4f;
        float waffleSpeedX = 130f;
        float wafflePivotPointMoveAround = 200f;//380

        int candyHp = 250;
        float candySpeed = 535f;
        string[] candyMoveString = new string[]{
            "Y,N,Y,Y,N,N,Y,Y,Y,N,Y,N,N",
            "Y,Y,Y,N,Y,N,N,Y,N,N,Y,Y,Y"};
        float candyCenter = 150f;
        float candyDeathSpeed = 600f;
        float candyDeathAcceleration = 2f;
        MinMax candySpawnDelay = new MinMax(0.8f, 1.5f);//new MinMax(1.3f, 2f);
        int candyMiniHp = 1000; //10
        float candyMiniSpeed = 40f; //80
        bool candySpawn = true;

        int cupcakeHp = 200;//235
        string[] cupcakeXSpeed = new string[]{
            "1000, 1200, 1500, 1300,1100,1050,1400",
            "950,1150,1400,1300,1000,1000,1500"};
        float cupcakeHold = 0.5f; //0.85
        float cupcakeSplashOriginal = 200f;
        float cupcakeSplashNext = 100f; //75
        bool cupcakeProj = true;

        int jawMinis = 5; //2
        float jawMiniSpace = 400f; //230f
        float jawHomingSpeed = 5f;
        int jawHp = 400; //220
        float jawSpeed = 150f; //265
        float jawHome = 1.8f;

        float pepperSpeed = 690f; //340f
        MinMax pepperDelay = new MinMax(1.5f, 2.5f); //new MinMax(3.5f, 6.7f)



        LevelProperties.Baroness.BaronessVonBonbon baroness = new LevelProperties.Baroness.BaronessVonBonbon(baronessHp, minibossStart, baronessTimeString, baronessAttackDelay, baronessAttackCount, baronessProjHp, baronessProjRotation, baronessProjSpeed, baronessFinalProjSpeed, baronessFinalProjDuration, baronessFinalProjRedirectDelay, baronessFinalProjRedirectCount, baronessFinalProjDelay, baronessFinalProjInitialDelay, baronessFinalProjHeadString);
        LevelProperties.Baroness.Open open = new LevelProperties.Baroness.Open(minibossCount, minibossString);
        LevelProperties.Baroness.Jellybeans jellybeans = new LevelProperties.Baroness.Jellybeans(jellyHp, jellySpeed, jellyHeight, jellyJumpSpeed, jellyJumpHeight, jellyAfterJumpDuration, jellyType, jellySpawnDelay, jellyStartingPoint, jellyDelayChangePercent);
        LevelProperties.Baroness.Gumball gum = new LevelProperties.Baroness.Gumball(gumHp, gumSpeed, gumDeathSpeed, gumOff, gumOn, gumGravity, gumVX, gumFireRate, gumVY, gumOffsetX);
        LevelProperties.Baroness.Waffle waffle = new LevelProperties.Baroness.Waffle(waffleHp, waffleSpeed, waffleAnticipation, waffleDelay, waffleExplodeSpeed, waffleReturn, waffleExplodeDistance, waffleReturnSpeed, waffleSpeedX, wafflePivotPointMoveAround);
        LevelProperties.Baroness.CandyCorn candy = new LevelProperties.Baroness.CandyCorn(candyHp, candySpeed, candyMoveString, candyCenter, candyDeathSpeed, candyDeathAcceleration, candySpawnDelay, candyMiniHp, candyMiniSpeed, candySpawn);
        LevelProperties.Baroness.Cupcake cupcake = new LevelProperties.Baroness.Cupcake(cupcakeHp, cupcakeXSpeed, cupcakeHold, cupcakeSplashOriginal, cupcakeSplashNext, cupcakeProj);
        LevelProperties.Baroness.Jawbreaker jaw = new LevelProperties.Baroness.Jawbreaker(jawMinis, jawMiniSpace, jawHomingSpeed, jawHp, jawSpeed, jawHome);
        LevelProperties.Baroness.Peppermint pepper = new LevelProperties.Baroness.Peppermint(pepperSpeed, pepperDelay);
        LevelProperties.Baroness.Platform platform = new LevelProperties.Baroness.Platform(0f, 0f, -15f); //(180f, 400f, -15f)));

        list.Add(new LevelProperties.Baroness.State(10f, new LevelProperties.Baroness.Pattern[][] { new LevelProperties.Baroness.Pattern[1] },
        LevelProperties.Baroness.States.Main,
        baroness, open, jellybeans, gum, waffle, candy, cupcake, jaw, pepper, platform));

        return new LevelProperties.Baroness(hp, goalTimes, list.ToArray());
    }

    public static LevelProperties.Clown ClownGetMode(On.LevelProperties.Clown.orig_GetMode orig, Level.Mode mode)
    {
        int hp = 1900;//1850
        Level.GoalTimes goalTimes = null;
        List<LevelProperties.Clown.State> list = new List<LevelProperties.Clown.State>();
        goalTimes = new Level.GoalTimes(120f, 120f, 120f);

        float bumperSpeed = 525f;
        float dashSpeed = 1400f;
        string[] attackDelayString = new string[] { "0.5" };
        //			string[] attackDelayString = new string[]{"3.3,3.6,2.2,3.5,3,1.5,3.5,2,3.8,2,1.5"};
        float movementDuration = 0.2f;//0.6
        string[] movementStrings = { "F,F,B,B,F,B,F,B,B,F,B,F,F,B,F,B" };
        float movementDashWarning = 0.3f;//0.8
        float movementDelay = 0.1f;//0.3

        MinMax duckHeightRange = new MinMax(120f, 160f);
        string[] duckYStartPercent = new string[] { "0,10,20,30,40" };
        float duckXSpeed = 210f;
        float duckYSpeed = 420f;
        string[] duckTypeString = new string[] { "B,B,B,B,B,B,B,B" };
        //			string[] duckTypeString = new string[]{"R,B,R,R,B,R,P,B","R,R,B,P,R,B,R,B"};
        float duckDelay = 2.6f;
        float duckSpinTime = 12f;
        float duckBombSpeed = 350f;

        bool heliumCoaster = true;
        float dogHp = 2137f;//7
        float dogSpeed = 200f;//300
        string[] dogDelayString = new string[] { "1.2" };
        //			string[] dogDelayString = new string[]{"2.5,2.5,2.8,2.5,2.5,2.5,2.8", "2.5,2.5,2.6,2.7,2.5,2.5,2.8"};
        string[] dogTypeString = new string[]
            {"P"};
        /*string[] dogTypeString = new string[]
				{"R,R,R,R,P,R,R,R,R,R,P,R,R,R,P",
				"R,R,R,R,P,R,R,R,R,P,R,R,R,R,R,P",
				"R,R,R,R,R,R,P,R,R,R,R,P,R,R,R,R,R,P"};*/
        string[] dogSpawnOrder = new string[]
            {"1-6"};
        /*string[] dogSpawnOrder = new string[]
				{"1-2-3,3-4,4-5-6,1-3,3-4,4-6",
				"2-3,4-5,3-4,1-2,5-6,2-3,4-5,3-4,1-2,5-6,1-3-4-6",
				"1-2,5-6,2-3,4-5,3-4,1-2,5-6,2-3,4-5,3-4,1-3-4-6",
				"3-6,4-1,2-5,3-6,4-1,2-5,1-3-4-6",
				"4-1,3-6,5-2,4-1,3-6,5-2,1-3-4-6",
				"1-6,2-5,3-4,1-6,2-5,3-4,1-3-4-6",
				"3-4,2-5,1-6,3-4,2-5,1-6,1-3-4-6"};*/
        float heliumMoveSpeed = 200f;
        float heliumAcceleration = 15f;
        bool dogDieOnGround = false;//true

        bool horseCoaster = true;
        float horseSpeed = 400f;
        string[] horseString = new string[] { "W,D,D,W,D,W,W,D" };
        float horseXOffset = 200f;
        int waveBulletCount = 3;
        float waveBulletSpeed = 320f;
        float waveBulletAmount = 50f;//30
        float waveBulletWaveSpeed = 5f;//4
        float waveBulletDelay = 0.3f;
        float waveAttackDelay = 3f;
        float waveAttackRepeat = 2f;
        string[] wavePosString = new string[] { "105,115,110", "100,110,105" };
        string[] wavePinkString = new string[] { "R,P,R" };
        //            string[] wavePinkString = new string[]{"P,R,R","R,R,R","R,R,P","R,R,R"};
        float waveHesitate = 0f;//1
        float dropBulletInitialSpeed = 2000f;//1600
        float dropBulletDelay = 0.5f;//0.9
        MinMax dropBulletOneDelay = new MinMax(0.1f, 0.2f);
        MinMax dropBulletTwoDelay = new MinMax(0.1f, 0.2f);
        float dropBulletSpeedDown = 2500f;//1000
        float dropAttackDelay = 1.5f;//2.5
        string[] dropHorsePos = new string[] { "300" };//75
        string[] dropBulletPosition = new string[]
            {"400-800-900-1000-1100-1200-1300",
            "400-500-600-700-800-900-1300",
            "400-500-600-700-800-1200-1300",
            "400-500-900-1000-1100-1200-1300",
            "400-500-600-700-1100-1200-1300",
            "400-800-900-1000-1100-1200-1300",
            "400-500-600-700-800-1200-1300",
            "400-500-600-700-800-900-1300",
            "400-800-900-1000-1100-1200-1300",
            "400-500-600-1000-1100-1200-1300"};
        float dropHesitate = 0f;//1
        float dropAttackRepeat = 1f;
        float dropReturnDelay = 0f;//0.5

        MinMax coasterInitialDelayHelium = new MinMax(10f,12f);//MinMax(2f, 3f);
        MinMax coasterInitialDelay = new MinMax(1f, 2f);
        float noseParrySuperGain = 5f;//1
        float coasterSpeed = 2250f;//450
        float coasterSpeedHorse = 2500f;//450
        float coasterBackSpeedMultiplayer = 1.25f;
        MinMax mainLoopDelay = new MinMax(0.0f, .0f);//MinMax(0.8f, 1.6f)
        MinMax mainLoopDelaySwing = new MinMax(4f, 6f);
        string[] coasterTypeStringHelium = new string[] { "F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F" };
        string[] coasterTypeStringHorse = new string[] { "F" };
        string[] coasterTypeStringSwing = new string[] { "F,F,F" };
        //			string[] coasterTypeString = new string[]{"E,F,E,E,F,E,E,F,E"};
        float coasterBackToFrontDelay = 0f;//1

        float swingSpeed = 10000f;//360;
        float swingSpacing = float.MaxValue;//200
        float swingDropWarningDuration = 0.1f;//0.5
        float swingFullDropDuration = 0.1f;//0.7
        bool swingDropOn = true;//false
        float swingBulletSpeed = 500f;
        MinMax swingAttackDelayRange = new MinMax(0.0f, 0.0f);//MinMax(1.5f, 3.5f);
        float swingHp = 2137f;//13
        float swingMovementSpeed = 8000f;//800
        string[] swingPositionString = new string[]
            {"0,200,1000,1200",
            "100-500-1100",
            "1200,800,400,0",
            "0,150,1050,1200",
            "50,500,700,1150",
            "1100-700-100",
            "1150-775-375-50",
            "0,400,800,1200",
            "0,150,1050,1200"};
        float swingSpawnDelay = 0.1f;//0.8
        float swingInitialAttackDelay = 0f;

        LevelProperties.Clown.BumperCar bumperCar = new LevelProperties.Clown.BumperCar(bumperSpeed, dashSpeed, attackDelayString, movementDuration, movementStrings, movementDashWarning, movementDelay);
        LevelProperties.Clown.Duck duck = new LevelProperties.Clown.Duck(duckHeightRange, duckYStartPercent, duckXSpeed, duckYSpeed, duckTypeString, duckDelay, duckSpinTime, duckBombSpeed);
        LevelProperties.Clown.HeliumClown heliumClown = new LevelProperties.Clown.HeliumClown(heliumCoaster, dogHp, dogSpeed, dogDelayString, dogTypeString, dogSpawnOrder, heliumMoveSpeed, heliumAcceleration, dogDieOnGround);
        LevelProperties.Clown.Horse horse = new LevelProperties.Clown.Horse(horseCoaster, horseSpeed, horseString, horseXOffset, waveBulletCount, waveBulletSpeed, waveBulletAmount, waveBulletWaveSpeed, waveBulletDelay, waveAttackDelay, waveAttackRepeat, wavePosString, wavePinkString, waveHesitate, dropBulletInitialSpeed, dropBulletDelay, dropBulletOneDelay, dropBulletTwoDelay, dropBulletSpeedDown, dropAttackDelay, dropHorsePos, dropBulletPosition, dropHesitate, dropAttackRepeat, dropReturnDelay);

        LevelProperties.Clown.Coaster coasterHelium = new LevelProperties.Clown.Coaster(coasterInitialDelayHelium, noseParrySuperGain, coasterSpeed, coasterBackSpeedMultiplayer, mainLoopDelay, coasterTypeStringHelium, coasterBackToFrontDelay);
        LevelProperties.Clown.Coaster coasterHorse = new LevelProperties.Clown.Coaster(coasterInitialDelay, noseParrySuperGain, coasterSpeedHorse, coasterBackSpeedMultiplayer, mainLoopDelay, coasterTypeStringHorse, coasterBackToFrontDelay);
        LevelProperties.Clown.Coaster coasterSwing = new LevelProperties.Clown.Coaster(coasterInitialDelay, noseParrySuperGain, coasterSpeed, coasterBackSpeedMultiplayer, mainLoopDelaySwing, coasterTypeStringSwing, coasterBackToFrontDelay);

        LevelProperties.Clown.Swing swing = new LevelProperties.Clown.Swing(swingSpeed, swingSpacing, swingDropWarningDuration, swingFullDropDuration, swingDropOn, swingBulletSpeed, swingAttackDelayRange, swingHp, swingMovementSpeed, swingPositionString, swingSpawnDelay, swingInitialAttackDelay);

        list.Add(new LevelProperties.Clown.State(10f, new LevelProperties.Clown.Pattern[][]{
        new LevelProperties.Clown.Pattern[1]}, LevelProperties.Clown.States.Main,
        bumperCar, duck, heliumClown, horse, coasterHelium, swing));

        //0.83
        list.Add(new LevelProperties.Clown.State(0.82f, new LevelProperties.Clown.Pattern[][]{
        new LevelProperties.Clown.Pattern[0]}, LevelProperties.Clown.States.HeliumTank,
        bumperCar, duck, heliumClown, horse, coasterHelium, swing));

        //0.63
        list.Add(new LevelProperties.Clown.State(0.55f, new LevelProperties.Clown.Pattern[][]{
        new LevelProperties.Clown.Pattern[0]}, LevelProperties.Clown.States.CarouselHorse,
        bumperCar, duck, heliumClown, horse, coasterHorse, swing));

        //0.45
        list.Add(new LevelProperties.Clown.State(0.37f, new LevelProperties.Clown.Pattern[][]{
        new LevelProperties.Clown.Pattern[0]}, LevelProperties.Clown.States.Swing,
        bumperCar, duck, heliumClown, horse, coasterSwing, swing));

        return new LevelProperties.Clown(hp, goalTimes, list.ToArray());
    }

    public Level.Timeline ClownCreateTimeline(On.LevelProperties.Clown.orig_CreateTimeline orig, LevelProperties.Clown self, Level.Mode mode)
    {
        Level.Timeline timeline = new Level.Timeline();
        /*timeline.health = 1850f;
        timeline.events.Add(new Level.Timeline.Event("HeliumTank", 0.83f));
        timeline.events.Add(new Level.Timeline.Event("CarouselHorse", 0.63f));
        timeline.events.Add(new Level.Timeline.Event("Swing", 0.45f));*/
        //1 - 314
        //2 - 370
        //3 - 333
        //4 - 832
        //new:
        //1 - 350
        //2 - 500
        //3 - 350
        //4 - 700
        //all = 1900 = 18% + 27% + 18% + 37%
        timeline.health = 1900f;
        timeline.events.Add(new Level.Timeline.Event("HeliumTank", 0.82f));
        timeline.events.Add(new Level.Timeline.Event("CarouselHorse", 0.55f));
        timeline.events.Add(new Level.Timeline.Event("Swing", 0.37f));
        return timeline;
    }

    public static LevelProperties.Dragon DragonGetMode(On.LevelProperties.Dragon.orig_GetMode orig, Level.Mode mode)
    {
        int hp = 1900;
        Level.GoalTimes goalTimes = null;
        List<LevelProperties.Dragon.State> list = new List<LevelProperties.Dragon.State>();
        goalTimes = new Level.GoalTimes(120f, 120f, 120f);
        List<LevelProperties.Dragon.State> list2 = list;
        float healthTrigger = 10f;
        LevelProperties.Dragon.Pattern[][] array = new LevelProperties.Dragon.Pattern[1][];
        int num = 0;
        LevelProperties.Dragon.Pattern[] array2 = new LevelProperties.Dragon.Pattern[2];
        array2[0] = LevelProperties.Dragon.Pattern.Peashot;
        array[num] = array2;
        /*
			string[] meteorString = new string[]{
				"U,D",
				"D,U",
				"D,B",
				"U,D",
				"D,U"};*/
        string[] meteorString = new string[] { "B,B" };
        float meteorSpeedX = 750f;//350
        float meteorTimeY = 0.8f;//0.9
        float meteorDelay = 0.8f;//1.2
        float meteorHesitate = 0f;//2

        float tailWarning = 1.0f;//1.3
        float tailIn = 0.1f;//0.25
        float tailOut = 0.1f;//1
        float tailHold = 0.05f;//0.5
        MinMax tailDelay = new MinMax(0f, 0.5f);//3,4.5

        /*string[] peashotString = new string[]{
				"0.4,P,0.7,P",
				"0.7,P,0.4,P",
				"0.4,P,0.7,P,0.6,P"};*/
        string[] peashotString = new string[]{
            "P"};
        float peashotDelay = 0.05f;//0.18
        float peashotSpeed = 1000f;//865
        string peashotColor = "OBBP";//OBP
        float peashotHesitate = 0f;//2

        string fireAndSmokeString = "F,S,F,F,S,F,S,F,F,F,S,F,S,F,S,F,F,S";

        float marchersSpeed = 360f;
        float marchersSpawn = 0.3f;//0.46
        MinMax marchersJumpDelay = new MinMax(0.6f, 0.9f);//MinMax(0.9f, 2f)
        float marchersCrouch = 0.5f;
        float marchersGravity = 1000f;//2200
        MinMax marchersJumpSpeed = new MinMax(300f, 1200f);//MinMax(200f, 1700f)
        MinMax marchersJumpAngle = new MinMax(45f, 70f);//MinMax(45f, 70f);
        MinMax marchersJumpX = new MinMax(150f, 800f);

        float potionSpeed = 460f;
        float potionSpitSpeed = 700f;
        float potionHp = 5f;
        string[] potionType = new string[] { "X" };
        string[] potionPosition = new string[]{
            "T:A,T:C,B:A,B:C,T:A,T:C",
            "B:A,T:A,T:C,B:A,T:A,T:C",
            "B:A,B:C,T:A,T:C,B:A,B:C",
            "T:C,B:C,B:A,T:C,B:C,B:A"};
        float potionScale = 1f;//0.7
        float potionExplosionScale = 0.6f;
        string[] potionAttackCount = new string[] { "12,6,8,10,6,10,12,8" };
        float potionRepeatDelay = 0.3f;//0f
        float potionAttackMainDelay = 0.5f;//1.5
        float potionPlayerAimCount = 3f;

        float potionScale2 = 2f;
        float potionExplosionScale2 = 1.5f;

        string[] blowtorchDelay = new string[] { "11,14,8,13,11,8,15,10,8,12,14" };
        int blowtorchWarningOne = 12;
        int blowtorchWarningTwo = 20;
        float blowtorchFireDuration = 1.5f;
        float blowtorchRepeatDelay = 1f;//2
        float blowtorchFireSize = 0.8f;
        /*
			string[] cloudsPositions = new string[]{
				"445,D0.6,290,D0.6,565,D0.3,190,D0.5,330,D0.6",
				"180,485,D0.8,560,310,D0.8,240,D0.3,570,D0.6,350,D0.7",
				"505,D0.3,218,480,D0.7,305,D0.8",
				"205,D0.2,580,D0.7,415,D0.4,235,560,D0.8,505,D0.6,345,D0.2,150,D0.5,565,D0.4,375,D0.7",
				"560,D0.2,285,D0.8,425,D0.6,170,500,D0.9,250,530,D0.7,380,D0.8",
				"495,235,D0.7,402,D0.6",
				"246,D0.3,550,D0.6,394,D0.8,525,295,D0.6,394,D0.6,515,D0.1,265,D0.5,370,D0.7"};*/
        string[] cloudsPositions = new string[] { "350" };
        float cloudsSpeed = 100f;//225
        bool cloudsRight = true;//false
        float cloudsDelay = 0.0f;//0.36

        LevelProperties.Dragon.Meteor meteor = new LevelProperties.Dragon.Meteor(meteorString, meteorSpeedX, meteorTimeY, meteorDelay, meteorHesitate);
        LevelProperties.Dragon.Tail tail = new LevelProperties.Dragon.Tail(true, tailWarning, tailIn, tailOut, tailHold, tailDelay);
        LevelProperties.Dragon.Peashot peashot = new LevelProperties.Dragon.Peashot(peashotString, peashotDelay, peashotSpeed, peashotColor, peashotHesitate);
        LevelProperties.Dragon.FireAndSmoke fireAndSmoke = new LevelProperties.Dragon.FireAndSmoke(fireAndSmokeString);
        LevelProperties.Dragon.FireMarchers fireMarchers = new LevelProperties.Dragon.FireMarchers(marchersSpeed, marchersSpawn, marchersJumpDelay, marchersCrouch, marchersGravity, marchersJumpSpeed, marchersJumpAngle, marchersJumpX);
        LevelProperties.Dragon.Potions potions = new LevelProperties.Dragon.Potions(potionSpeed, potionSpitSpeed, potionHp, potionType, potionPosition, potionScale, potionExplosionScale, potionAttackCount, potionRepeatDelay, potionAttackMainDelay, potionPlayerAimCount);
        LevelProperties.Dragon.Potions potionsForBlimp = new LevelProperties.Dragon.Potions(potionSpeed, potionSpitSpeed, 2137f, potionType, potionPosition, potionScale2, potionExplosionScale2, potionAttackCount, potionRepeatDelay, potionAttackMainDelay, potionPlayerAimCount);
        LevelProperties.Dragon.Blowtorch blowtorch = new LevelProperties.Dragon.Blowtorch(blowtorchDelay, blowtorchWarningOne, blowtorchWarningTwo, blowtorchFireDuration, blowtorchRepeatDelay, blowtorchFireSize);
        LevelProperties.Dragon.Clouds clouds = new LevelProperties.Dragon.Clouds(cloudsPositions, cloudsSpeed, cloudsRight, cloudsDelay);

        list2.Add(new LevelProperties.Dragon.State(healthTrigger, array, LevelProperties.Dragon.States.Main, meteor, tail, peashot, fireAndSmoke, fireMarchers, potionsForBlimp, blowtorch, clouds));

        //0.9
        list.Add(new LevelProperties.Dragon.State(1.0f, new LevelProperties.Dragon.Pattern[][]{new LevelProperties.Dragon.Pattern[]
        {LevelProperties.Dragon.Pattern.Meteor,LevelProperties.Dragon.Pattern.Peashot}}, LevelProperties.Dragon.States.Generic,
        meteor, tail, peashot, fireAndSmoke, fireMarchers, potions, blowtorch, clouds));

        list.Add(new LevelProperties.Dragon.State(0.64f, new LevelProperties.Dragon.Pattern[][]{new LevelProperties.Dragon.Pattern[0]
        }, LevelProperties.Dragon.States.FireMarchers,
        meteor, tail, peashot, fireAndSmoke, fireMarchers, potions, blowtorch, clouds));

        list.Add(new LevelProperties.Dragon.State(0.35f, new LevelProperties.Dragon.Pattern[][]{new LevelProperties.Dragon.Pattern[0]
        }, LevelProperties.Dragon.States.ThreeHeads,
        meteor, tail, peashot, fireAndSmoke, fireMarchers, potions, blowtorch, clouds));

        return new LevelProperties.Dragon(hp, goalTimes, list.ToArray());
    }

    public Level.Timeline DragonCreateTimeline(On.LevelProperties.Dragon.orig_CreateTimeline orig, LevelProperties.Dragon self, Level.Mode mode)
    {
        Level.Timeline timeline = new Level.Timeline();
        timeline.health = 1900f;
        //timeline.events.Add(new Level.Timeline.Event("Generic", 0.9f));
        timeline.events.Add(new Level.Timeline.Event("FireMarchers", 0.64f));
        timeline.events.Add(new Level.Timeline.Event("ThreeHeads", 0.35f));
        return timeline;
    }

    public static LevelProperties.FlyingBird BirdGetMode(On.LevelProperties.FlyingBird.orig_GetMode orig, Level.Mode mode)
    {
        int hp = 750;
        Level.GoalTimes goalTimes = null;
        List<LevelProperties.FlyingBird.State> list = new List<LevelProperties.FlyingBird.State>();
        goalTimes = new Level.GoalTimes(120f, 120f, 120f);

        LevelProperties.FlyingBird.Pattern egg = LevelProperties.FlyingBird.Pattern.Eggs;
        LevelProperties.FlyingBird.Pattern shoot = LevelProperties.FlyingBird.Pattern.Lasers;

        float floatTime = 2.4f;
        float floatTop = 360f;
        float floatBottom = -120f;
        MinMax floatInitialAttackDelay = new MinMax(0.5f, 2.2f);

        string[] featherPattern = new string[] { "P:15", "P:20", "P:25", "P:20" };
        int featherCount = 10;
        float featherSpeed = 850f;
        float featherOffset = 22f;
        float featherDelay = 0.23f;
        float featherHesitate = 1.4f;

        string[] eggPattern = new string[] { "P:1" };
        //string[] eggPattern = new string[]{"P:3", "P:1", "P:2", "P:2", "P:1"};
        float eggSpeed = 500f;
        float eggDelay = 1.2f;
        float eggHesitate = 0.1f;//1

        int enemyCount = 4;
        float enemyDelay = 0.7f;
        int enemyHp = 8;
        float enemySpeed = 350f;
        float enemyFloatRange = 100f;
        float enemyFloatTime = 2f;
        float enemyProjHeight = 600f;
        float enemyProjFallTime = 1f;
        float enemyProjDelay = 10f;
        float enemyGroupDelay = 2f;//4.1
        float enemyInitialGroupDelay = 2f;
        bool enemyAim = true;

        float shootSpeed = 1050f;
        float shootHesitate = 0.1f;//0.7

        int turretHealth = 1;
        float turretIn = 2f;
        float turretBulletSpeed = 100f;
        float turretBulletDelay = 3f;
        float turretRespawnTime = 10f;
        float turretFloatRange = 100f;
        float turretFloatTime = 1f;

        float smallTimeX = 3.2f;
        float smallTimeY = 1.4f;
        float smallminX = -245f;
        int smallEggCount = 6;
        MinMax smallEggRange = new MinMax(100f, 545f);
        float smallEggRotation = 110f;
        float smallEggTime = 5f;
        float smallShotDelay = 0.1f;//6.5
        float smallShotSpeed = 500f;//700
        float smallLeaveTime = 2f;

        float bigBirdXTime = 5f;//3.7

        float nurseBulletSpeed = 550f;
        float nursePillSpeed = 450f;
        float nursePillMaxHeight = 130f;
        float nursePillExplodeDelay = 0.8f;
        string nursePinkString = "R,R,R,R,P,R,R,R,R,R,P";
        float nurseAttackRepeatDelay = 0.8f;//2
        string nurseAttackCount = "6,2,4,2";
        float nurseAttackMainDelay = 0.8f;//4

        float garMaxHeight = 100f;//210
        float garSpeedX = 500f;//1020
        float garSpeedY = 80f;//160
        float garSpeedXIncrease = 100f;//150
        string garCount = "5,6";
        float garShotDelay = 0.2f;
        float garShotSize = 1f;
        MinMax garHesitate = new MinMax(0.8f, 1.5f);//new MinMax(1.5f, 3.3f);
        string[] garPattern = new string[]{
            "P,F,B,A,B,F",
            "B,F,A,P,A,B",
            "A,B,F,P,A,F",
            "A,P,F,B,F,A"};

        float heartSpeed = 425f;
        int heartShotCount = 3;//2
        MinMax heartHesitate = new MinMax(0.8f, 1.5f);//MinMax heartHesitate = new MinMax(1.8f, 3.4f);
        MinMax heartSpread = new MinMax(0f, 60f);
        float heartProjSpeed = 500f;
        float heartHeight = 500f;
        string[] heartShotString = new string[]{
            "0.6,0.4,0.5",
            "0.6,0.6,0.5",
            "0.4,0.4,0.3",
            "0.5,0.4,0.3",
            "0.5,0.4,0.6",
            "0.6,0.5,0.5",
            "0.5,0.5,0.4",
            "0.6,0.4,0.5",
            "0.4,0.5,0.4",
            "0.4,0.5,0.6",
            "0.3,0.4,0.4",
            "0.6,0.5,0.5",
            "0.4,0.5,0.5"};
        /*string[] heartShotString = new string[]{
				"0.6,0.8",
				"0.8,0.8",
				"0.5,0.5",
				"0.7,0.5",
				"0.6,0.9",
				"0.9,0.7",
				"0.7,0.7",
				"0.6,0.9",
				"0.8,0.5",
				"0.7,0.8",
				"0.5,0.6",
				"0.9,0.7",
				"0.5,0.9"};*/
        string[] heartProjCount = new string[] { "3,3,3" };//"3,3,3"



        LevelProperties.FlyingBird.Floating floating = new LevelProperties.FlyingBird.Floating(floatTime, floatTop, floatBottom, floatInitialAttackDelay);
        LevelProperties.FlyingBird.Feathers feathers = new LevelProperties.FlyingBird.Feathers(featherPattern, featherCount, featherSpeed, featherOffset, featherDelay, featherDelay, featherHesitate);
        LevelProperties.FlyingBird.Eggs eggs = new LevelProperties.FlyingBird.Eggs(eggPattern, eggSpeed, eggDelay, eggHesitate);
        LevelProperties.FlyingBird.Enemies enemies = new LevelProperties.FlyingBird.Enemies(true, enemyCount, enemyDelay, enemyHp, enemySpeed, enemyFloatRange, enemyFloatTime, enemyProjHeight, enemyProjFallTime, enemyProjDelay, enemyGroupDelay, enemyInitialGroupDelay, enemyAim);
        LevelProperties.FlyingBird.Enemies enemiesOff = new LevelProperties.FlyingBird.Enemies(false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, false);
        LevelProperties.FlyingBird.Lasers shootAttack = new LevelProperties.FlyingBird.Lasers(shootSpeed, shootHesitate);
        LevelProperties.FlyingBird.Turrets turrets = new LevelProperties.FlyingBird.Turrets(true, turretHealth, turretIn, turretBulletSpeed, turretBulletDelay, turretRespawnTime, turretFloatRange, turretFloatTime);
        LevelProperties.FlyingBird.Turrets turretsOff = new LevelProperties.FlyingBird.Turrets(false, 0, 0, 0, 0, 0, 0, 0);
        LevelProperties.FlyingBird.SmallBird small = new LevelProperties.FlyingBird.SmallBird(smallTimeX, smallTimeY, smallminX, smallEggCount, smallEggRange, smallEggRotation, smallEggTime, smallShotDelay, smallShotSpeed, smallLeaveTime);
        LevelProperties.FlyingBird.BigBird big = new LevelProperties.FlyingBird.BigBird(bigBirdXTime);
        LevelProperties.FlyingBird.Nurses nurses = new LevelProperties.FlyingBird.Nurses(nurseBulletSpeed, nursePillSpeed, nursePillMaxHeight, nursePillExplodeDelay, nursePinkString, nurseAttackRepeatDelay, nurseAttackCount, nurseAttackMainDelay);
        LevelProperties.FlyingBird.Garbage garbage = new LevelProperties.FlyingBird.Garbage(garMaxHeight, garSpeedX, garSpeedY, garSpeedXIncrease, garCount, garShotDelay, garShotSize, garHesitate, garPattern);
        LevelProperties.FlyingBird.Heart heart = new LevelProperties.FlyingBird.Heart(heartSpeed, heartShotCount, heartHesitate, heartSpread, heartProjSpeed, heartHeight, heartShotString, heartProjCount);

        //Main
        list.Add(new LevelProperties.FlyingBird.State(10f, new LevelProperties.FlyingBird.Pattern[][]{
        new LevelProperties.FlyingBird.Pattern[]{egg, shoot, egg, shoot, egg, shoot, shoot, egg, shoot, egg, shoot, egg, shoot, shoot, egg, shoot, shoot, egg, shoot}}, LevelProperties.FlyingBird.States.Main, floating, feathers, eggs, enemies, shootAttack, turretsOff, small, big, nurses, garbage, heart));
        /*list.Add(new LevelProperties.FlyingBird.State(10f, new LevelProperties.FlyingBird.Pattern[][]{
			new LevelProperties.FlyingBird.Pattern[]{egg, egg, egg, shoot, egg, egg, shoot, egg, egg, egg, shoot, egg, egg, shoot, egg, egg, egg, egg, shoot}}, LevelProperties.FlyingBird.States.Main, floating, feathers, eggs, enemies, shootAttack, turretsOff, small, big, nurses, garbage, heart));*/

        //Phase 2
        list.Add(new LevelProperties.FlyingBird.State(0.75f, new LevelProperties.FlyingBird.Pattern[][] { new LevelProperties.FlyingBird.Pattern[1] }, LevelProperties.FlyingBird.States.Whistle, floating, feathers, eggs, enemies, shootAttack, turretsOff, small, big, nurses, garbage, heart));

        //Kid
        list.Add(new LevelProperties.FlyingBird.State(0.5f, new LevelProperties.FlyingBird.Pattern[][] { new LevelProperties.FlyingBird.Pattern[0] }, LevelProperties.FlyingBird.States.HouseDeath, floating, feathers, eggs, enemiesOff, shootAttack, turretsOff, small, big, nurses, garbage, heart));

        //Final Phase
        //list.Add(new LevelProperties.FlyingBird.State(0.31f, new LevelProperties.FlyingBird.Pattern[][]{new LevelProperties.FlyingBird.Pattern[]{LevelProperties.FlyingBird.Pattern.Heart, LevelProperties.FlyingBird.Pattern.Garbage}
        list.Add(new LevelProperties.FlyingBird.State(0.31f, new LevelProperties.FlyingBird.Pattern[][]{new LevelProperties.FlyingBird.Pattern[]{LevelProperties.FlyingBird.Pattern.Garbage}
        }, LevelProperties.FlyingBird.States.BirdRevival, floating, feathers, eggs, enemiesOff, shootAttack, turretsOff, small, big, nurses, garbage, heart));

        return new LevelProperties.FlyingBird(hp, goalTimes, list.ToArray());
    }

    public Level.Timeline BirdCreateTimeline(On.LevelProperties.FlyingBird.orig_CreateTimeline orig, LevelProperties.FlyingBird self, Level.Mode mode)
    {
        Level.Timeline timeline = new Level.Timeline();
        timeline.health = 750f;
        timeline.events.Add(new Level.Timeline.Event("Whistle", 0.75f));
        timeline.events.Add(new Level.Timeline.Event("HouseDeath", 0.5f));
        timeline.events.Add(new Level.Timeline.Event("BirdRevival", 0.31f));
        return timeline;
    }

    public static LevelProperties.FlyingGenie GenieGetMode(On.LevelProperties.FlyingGenie.orig_GetMode orig, Level.Mode mode)
    {
        int hp = 3000;
        Level.GoalTimes goalTimes = null;
        List<LevelProperties.FlyingGenie.State> list = new List<LevelProperties.FlyingGenie.State>();
        goalTimes = new Level.GoalTimes(120f, 120f, 120f);

        float gemSpeed = 350f;
        float gemWarning = 1f;
        string[] gemPattern = new string[] { "2.5" };
        //string[] gemPattern = new string[]{"2.7,3,2.5,3.5,2.6,4,3.3,2.5,3.1,4.1,2.7,3.6,2.8"};
        int gemRings = 1;
        string gemPink = "P";//"R"

        float swordSpeed = 400f;//600f
        float swordAppearDelay = 1f;
        float swordSpawnDelay = 2f;//0.9
        float swordAttackDelay = 2.4f;
        string[] swordPositionPattern = new string[]{
            "650-50,650-650,350-50,350-650,50-50,50-650,50-300,50-500",
            "650-650,650-50,350-650-350-50,50-650,50-50,50-500,50-300"};
        float swordRepeatDelay = 2.5f;
        float swordHesitate = 0f;
        string swordPink = "P,R,R,R,R,P,R,R,R,R,R,R";

        float pyramidSpeed = 0.9f;
        float pyramidWarning = 0.5f;//1
        float pyramidDuration = 0.7f;//1.05
        string[] pyramidDelay = new string[]{
            "3,3.4,2.5,4,2.8,2,3.4",
            "3.2,2.6,3.5,2.3,3,2.7,2"};
        string[] pyramidPattern = new string[]{
            "2,3,1,2,1,3,1,2,3",
            "1,2,3,2,1,3,2,3,1,3"};
        float pyramidLoopSize = 320f;

        float gemSmallSpeed = 600f;
        float gemBigSpeed = 400f;
        MinMax gemSmallDelay = new MinMax(0.22f, 0.5f);//new MinMax(0.11f, 0.25f);
        MinMax gemBigDelay = new MinMax(3f, 5f);//new MinMax(1.5f, 2.5f);
        string[] gemSmallAimOffset = new string[]{
"0,200,-150,25,300,45,400,0,100,-350,-150,25,200,-200,0,400,-50,-400,0,50,-100,15,300,-300,0,-100,0,-400,25,-100,-250,0,50,-400,150,0,350",
            "50,150,-100,0,300,0,-300,-50,15,100,400,0,-200,250,0,-400,175,0,-45,0,300,-75,15,50,-200,30,350",
            "0,200,-100,400,-100,0,45,100,-400,0,250,-175,45,-60,0,225,-100,15,450,165,-40,0,-450,80,150,-225,-68"};
        string[] gemBigAimOffset = new string[]{
            "0,100,25,125,-40,0,50,-50,150",
            "50,0,-50,200,150,-100,0,-150"};
        float gemSmallAttackDuration = 8f;
        float gemBigAttackDuration = 7f;
        float gemHesitate = 0f;
        float gemRepeatDelay = 1f;
        string gemPink2 = "P,R,R,R,R,R,R,R,R,R,R,R,R";

        MinMax gemSmallDelay2 = new MinMax(0.11f, 0.25f);
        MinMax gemBigDelay2 = new MinMax(1.5f, 2.5f);
        float gemRepeatDelay2 = 0f;//2.5f

        float sphinxSpeed = 400f;
        float sphinxSplitSpeed = 300f;
        string[] sphinxCount = new string[] { "2" };
        float sphinxSplitDelay = 1f;
        float sphinxMiniSpawnDelay = 0.6f;
        float sphinxMainDelay = 3.3f;
        string[] sphinxAimX = new string[]{
            "0,255,145,160,360,200",
            "400,50,300,188,340,110"};
        string[] sphinxAimY = new string[]{
            "100,600,350,477",
            "75,550,255,435",
            "400,300,500,125",
            "325,525,115,425"};
        float sphinxSpawnNum = 2f;//6f
        float sphinxMiniHP = 20f;
        MinMax sphinxMiniHomingDuration = new MinMax(2.5f, 3.4f);
        float sphinxHesitate = 0f;
        bool sphinxDieOnCollisionPlayer = false;
        float sphinxRepeatDelay = 4.3f;
        float sphinxMiniInitialSpawnDelay = 0.1f;
        float sphinxHomingSpeed = 200f;
        float sphinxHomingRotation = 2.4f;
        string sphinxScarabPinkString = "P,R,R,R,R,R,P,R,R,R,R,R,R,R";

        float coffinHeartMovement = 200f;
        MinMax coffinHeartShotDelayRange = new MinMax(0.2f, 0.4f);//new MinMax(0.6f, 1.3f);
        float coffinAttackDuration = 8.3f;
        float coffinHeartShotXSpeed = 300f;
        float coffinHeartShotYSpeed = 3f;//5.5
        float coffinHeartLoopYSize = 30f;
        float coffinHesitate = 0f;//1
        float mummyDelay = 0.8f;
        float mummyHP = 16f;
        string[] mummyAppear = new string[]{
            "200,0,100,200,-100,170,-170,200,50,-140,0,200,50,-140,200,-200,-50",
            "0,-200,200,100,170,50,200,-140,0,200,-140,50,170,0,200,-100,-200,50,200,-170,100,200,0,-140" };
        /*    "250,0,100,250,-100,200,-200,250,50,-150,0,250,50,-150,250,-250,-50",
            "0,-250,250,100,200,50,250,-150,0,250,-150,50,200,0,250,-100,-250,50,250,-200,100,250,0,-150"*/
        string[] mummyDirection = new string[]{
            "0,3,0,5,3,-4,0,-4,1,0,0,-5,2,-2,0,-4,3,-3",
            "1,3,-3,0,2,0,-5,4,-1,2,-2,-5,0,1,2,-1,3,-4,2,-3,0,1,-1"};
        string[] mummyType = new string[]{
            "A,B,C,A,C,B,C,A,B,C,B",
            "A,B,C,B,A,B,C,A,B,C,A,C",
            "A,B,A,B,C,A,B,C,B,A,C"};
        float mummyASpeed = 430f;
        float mummyBSpeed = 630f;
        float mummyCSpeed = 530f;
        bool mummyASinWave = true;
        bool mummyCSlowdown = false;

        float obeliskMovementSpeed = 200f;//285
        int obeliskCount = 5;//8
        float obeliskAppearDelay = 5f;//2.45
        string[] obeliskPos = new string[]
        {
            "1-3,1-4,2-5,1-5,1-3-5,2-5,1-4,3-5,2-5",
            "3-5,1-3,2-5,1-3-5,2-4,1-3,1-4,2-5,3-5",
            "2-4,1-5,2-4,1-4,1-3,1-3-5,1-4,3-5,2-5"
        };
        /*string[] obeliskPos = new string[]
        {
            "1,4,2,5,1-4,5,1,3,2-5",
            "3,1,2-5,4,2,3,1-4,2,5",
            "4,1,2,1-4,3,5,1,3,2-5"
        }; */
        float obeliskHP = 20f;
        float obeliskShootDelay = 3f;
        float obeliskShootSpeed = 300f;
        string[] obeliskShotDirection = new string[0];
        string[] obeliskPink = new string[0];
        float bouncerSpeed = 485f;
        string[] bouncerPink = new string[]
        {
            "R,P,R,R,P"
        };
        string[] bouncerAngle = new string[]
        {
            "45,300,40,290,50,310",
            "295,40,310,50,305,45"
        };
        bool bounceShotOn = true;
        bool normalShotOn = false;
        float obeliskHesitate = 5f;

        float scanDuration = 1f;
        float miniDuration = 0.6f;
        float miniMovementSpeed = 2.5f;
        float miniInitialDelay = 2f;
        float miniBulletSpeed = 565f;
        float miniShootDelay = 0.7f;
        string[] miniPink = new string[] { "R,P,R,P" };
        float miniHP = 605f;
        float miniTransitionDamage = -870f;

        float mainIntroHesitate = 1f;

        MinMax skullDelayRange = new MinMax(1.5f, 3f);//new MinMax(2.5f, 5.4f);
        float skullSpeed = 575f;
        int skullCount = 2137;//2

        float bulletShotSpeed = 635f;
        string[] bulletShotCount = new string[]{
            "5,4,4",
            "3,4,6",
            "4,3,6",
            "4,5,4",
            "3,6,4"};
        float bulletShotDelay = 0.7f;//0.55
        float spawnerSpeed = 325f;
        float spawnerRotateSpeed = 44f;
        int spawnerCount = 4;
        float spawnerShotDelay = 0.65f;
        float spawnerDistance = 335f;
        MinMax spawnerMoveCountRange = new MinMax(4f, 6f);
        float spawnerHesitate = 1.75f;
        int spawnerShotCount = 4;
        float spawnerMoveDelay = 0.65f;
        float childSpeed = 225f;
        MinMax idkWhatHesitate = new MinMax(2.75f, 2.75f);
        string marionettePink = "R,R,R,P";
        float marionetteMoveSpeed = 145f;
        float marionetteReturnSpeed = 100f;

        LevelProperties.FlyingGenie.Pyramids pyramids = new LevelProperties.FlyingGenie.Pyramids(pyramidSpeed, pyramidWarning, pyramidDuration, pyramidDelay, pyramidPattern, pyramidLoopSize);
        LevelProperties.FlyingGenie.GemStone gem = new LevelProperties.FlyingGenie.GemStone(gemSpeed, gemWarning, gemPattern, gemRings, gemPink);
        LevelProperties.FlyingGenie.Swords swords = new LevelProperties.FlyingGenie.Swords(swordSpeed, swordAppearDelay, swordSpawnDelay, swordAttackDelay, swordPositionPattern, swordRepeatDelay, swordHesitate, swordPink);
        LevelProperties.FlyingGenie.Gems gems = new LevelProperties.FlyingGenie.Gems(gemSmallSpeed, gemBigSpeed, gemSmallDelay, gemBigDelay, gemSmallAimOffset, gemBigAimOffset, gemSmallAttackDuration, gemBigAttackDuration, gemHesitate, gemRepeatDelay, gemPink2);
        LevelProperties.FlyingGenie.Gems gems2 = new LevelProperties.FlyingGenie.Gems(gemSmallSpeed, gemBigSpeed, gemSmallDelay2, gemBigDelay2, gemSmallAimOffset, gemBigAimOffset, gemSmallAttackDuration, gemBigAttackDuration, gemHesitate, gemRepeatDelay2, gemPink2);
        LevelProperties.FlyingGenie.Sphinx sphinx = new LevelProperties.FlyingGenie.Sphinx(sphinxSpeed, sphinxSplitSpeed, sphinxCount, sphinxSplitDelay, sphinxMiniSpawnDelay, sphinxMainDelay, sphinxAimX, sphinxAimY, sphinxSpawnNum, sphinxMiniHP, sphinxMiniHomingDuration, sphinxHesitate, sphinxDieOnCollisionPlayer, sphinxRepeatDelay, sphinxMiniInitialSpawnDelay, sphinxHomingSpeed, sphinxHomingRotation, sphinxScarabPinkString);
        LevelProperties.FlyingGenie.Coffin coffin = new LevelProperties.FlyingGenie.Coffin(coffinHeartMovement, coffinHeartShotDelayRange, coffinAttackDuration, coffinHeartShotXSpeed, coffinHeartShotYSpeed, coffinHeartLoopYSize, coffinHesitate, mummyDelay, mummyHP, mummyAppear, mummyDirection, mummyType, mummyASpeed, mummyBSpeed, mummyCSpeed, mummyASinWave, mummyCSlowdown);
        LevelProperties.FlyingGenie.Obelisk obelisk = new LevelProperties.FlyingGenie.Obelisk(obeliskMovementSpeed, obeliskCount, obeliskAppearDelay, obeliskPos, obeliskHP, obeliskShootDelay, obeliskShootSpeed, obeliskShotDirection, obeliskPink, bouncerSpeed, bouncerPink, bouncerAngle, bounceShotOn, normalShotOn, obeliskHesitate);
        LevelProperties.FlyingGenie.Scan scan = new LevelProperties.FlyingGenie.Scan(scanDuration, miniDuration, miniMovementSpeed, miniInitialDelay, miniBulletSpeed, miniShootDelay, miniPink, miniHP, miniTransitionDamage);
        LevelProperties.FlyingGenie.Bomb bomb = new LevelProperties.FlyingGenie.Bomb(0f, 0f, 0f, 0f, 0f, new string[0], 0f);
        LevelProperties.FlyingGenie.Main main = new LevelProperties.FlyingGenie.Main(mainIntroHesitate);
        LevelProperties.FlyingGenie.Skull skull = new LevelProperties.FlyingGenie.Skull(skullDelayRange, skullSpeed, skullCount);
        LevelProperties.FlyingGenie.Bullets bullets = new LevelProperties.FlyingGenie.Bullets(bulletShotSpeed, bulletShotCount, bulletShotDelay, spawnerSpeed, spawnerRotateSpeed, spawnerCount, spawnerShotDelay, spawnerDistance, spawnerMoveCountRange, spawnerHesitate, spawnerShotCount, spawnerMoveDelay, childSpeed, idkWhatHesitate, marionettePink, marionetteMoveSpeed, marionetteReturnSpeed);

        //Main
        list.Add(new LevelProperties.FlyingGenie.State(10f, new LevelProperties.FlyingGenie.Pattern[][]{
            new LevelProperties.FlyingGenie.Pattern[1]}, LevelProperties.FlyingGenie.States.Main, pyramids, gem, swords, gems, sphinx, coffin, obelisk, scan, bomb, main, skull, bullets));

        //Columns
        list.Add(new LevelProperties.FlyingGenie.State(0.85f, new LevelProperties.FlyingGenie.Pattern[][]{
            new LevelProperties.FlyingGenie.Pattern[0]}, LevelProperties.FlyingGenie.States.Disappear, pyramids, gem, swords, gems2, sphinx, coffin, obelisk, scan, bomb, main, skull, bullets));

        //Marionette 0.6
        list.Add(new LevelProperties.FlyingGenie.State(0.5f, new LevelProperties.FlyingGenie.Pattern[][]{
            new LevelProperties.FlyingGenie.Pattern[0]}, LevelProperties.FlyingGenie.States.Marionette, pyramids, gem, swords, gems, sphinx, coffin, obelisk, scan, bomb, main, skull, bullets));

        //Giant
        list.Add(new LevelProperties.FlyingGenie.State(0.25f, new LevelProperties.FlyingGenie.Pattern[][]{
            new LevelProperties.FlyingGenie.Pattern[0]}, LevelProperties.FlyingGenie.States.Giant, pyramids, gem, swords, gems, sphinx, coffin, obelisk, scan, bomb, main, skull, bullets));

        return new LevelProperties.FlyingGenie(hp, goalTimes, list.ToArray());
    }

    public Level.Timeline GenieCreateTimeline(On.LevelProperties.FlyingGenie.orig_CreateTimeline orig, LevelProperties.FlyingGenie self, Level.Mode mode)
    {
        Level.Timeline timeline = new Level.Timeline();
        timeline.health = 3000f;
        timeline.events.Add(new Level.Timeline.Event("Disappear", 0.85f));
        timeline.events.Add(new Level.Timeline.Event("Marionette", 0.5f));//0.6
        timeline.events.Add(new Level.Timeline.Event("Giant", 0.25f));
        return timeline;
    }

    public static LevelProperties.Bee BeeGetMode(On.LevelProperties.Bee.orig_GetMode orig, Level.Mode mode)
    {
        int hp = 1400;
        Level.GoalTimes goalTimes = null;
        List<LevelProperties.Bee.State> list = new List<LevelProperties.Bee.State>();
        goalTimes = new Level.GoalTimes(120f, 120f, 120f);

        bool platformMovement = true;
        float platformSpeed = 250f;//135
        float platformSpeed2 = 400f;
        int platformsMissing = 60;//1
        int platformsMissing2 = -1;

        bool gruntActive = true;
        int gruntHealth = 8;
        string[] gruntEntrancePoint = new string[] { "0,1,2,3,2,1" };
        float gruntSpeed = 320f;
        float gruntDelay = 3.5f;

        string[] blackHolePattern = new string[] {
            "2,0",
            "0,2"};
        float blackHoleChargeTime = 4f;
        float blackHoleAttackTime = 1f;
        float blackHoleSpeed = 200f;
        float blackHoleHealth = 1000000f;
        float blackHoleHesitate = 3f;
        float blackHoleChildDelay = 0.8f;
        int blackHoleChildSpeed = 550;
        float blackHoleChildHealth = 1f;
        bool blackHoleDamageable = true;

        int triangleCount = 3;//1
        float triangleChargeTime = 1f;//2
        float triangleAttackTime = 1.5f;
        float triangleIntroTime = 1f;
        float triangleSpeed = 180;
        float triangleRotationSpeed = 60f;
        float triangleHealth = 100000f;
        float triangleHesitate = 2.5f;
        float triangleChildSpeed = 550f;
        float triangleChildDelay = 0.5f;//1
        float triangleChildHealth = 1f;
        int triangleChildCount = 5;//3
        bool triangleDamageable = false;

        int followerCount = 1;
        float followerChargeTime = 2f;
        float followerAttackTime = 1.5f;
        float followerIntroTime = 1f;
        float followerHomingSpeed = 200f;//330
        float followerHomingRotation = 2.5f;
        float followerHomingTime = 2137f;//2.5
        float followerHealth = 10000f;
        float followerHesitate = 2.5f;
        float followerChildDelay = 0f;
        float followerChildHealth = 0f;
        bool followerDamageable = false;
        bool followerParryable = true;

        int chainCount = 4;
        float chainDelay = 3f;//1.4
        float chainTimeX = 0.55f;
        float chainTimeY = 0.05f;
        float chainSpeed = 2137f;//700
        float chainHesitate = 1f;
        bool chainForever = false;

        float securitySpeed = 325f;
        MinMax securityAttackDelay = new MinMax(1.4f, 2.7f);
        float securityIdleTime = 0f; //1
        float securityWarningTime = 2f;
        float securityChildSpeed = 450f;
        int securityChildCount = 6;//8

        float generalScreenScrollSpeed = 180f;
        float generalMovementSpeed = 120f;
        float generalMovementOffset = 500f;

        float wingMovementSpeed = 690f;//480
        string[] wingAttackCount = new string[] { "1,2,1,1,2,1,1,2,1,1,1,2" };
        float wingAttackDuration = 0.1f;
        float wingMaxDistance = 225f;
        float wingWarningDuration = 0.5f;//0.9
        float wingWarningMovementSpeed = 700f;//370
        float wingWarningMaxDistance = 400f;
        MinMax wingHesitateRange = new MinMax(0.0f, 0.1f);//MinMax(1.5f, 3.5f);

        float turbineBulletSpeed = 550f;
        float turbineBulletCircleTime = 1.6f;
        string[] turbineAttackDirectionString = new string[]
        {
            "B"
        };
        /*{
				"L,R,D:0.1,B",
				"R,L,R",
				"R,L,D:0.1,B",
				"L,R,L"
			};*/
        float turbineRepeatDealy = 0f;//3.1
        MinMax turbineHesitateRange = new MinMax(0.0f, 0.1f);//new MinMax(1f, 2f);

        LevelProperties.Bee.Movement platforms = new LevelProperties.Bee.Movement(platformMovement, platformSpeed, platformsMissing);
        LevelProperties.Bee.Movement platforms2 = new LevelProperties.Bee.Movement(platformMovement, platformSpeed2, platformsMissing2);
        LevelProperties.Bee.Grunts grunt = new LevelProperties.Bee.Grunts(gruntActive, gruntHealth, gruntEntrancePoint, gruntSpeed, gruntDelay);
        LevelProperties.Bee.Grunts gruntOff = new LevelProperties.Bee.Grunts(false, gruntHealth, gruntEntrancePoint, gruntSpeed, gruntDelay);
        LevelProperties.Bee.BlackHole blackHole = new LevelProperties.Bee.BlackHole(blackHolePattern, blackHoleChargeTime, blackHoleAttackTime, blackHoleSpeed, blackHoleHealth, blackHoleHesitate, blackHoleChildDelay, blackHoleChildSpeed, blackHoleChildHealth, blackHoleDamageable);
        LevelProperties.Bee.Triangle triangle = new LevelProperties.Bee.Triangle(triangleCount, triangleChargeTime, triangleAttackTime, triangleIntroTime, triangleSpeed, triangleRotationSpeed, triangleHealth, triangleHesitate, triangleChildSpeed, triangleChildDelay, triangleChildHealth, triangleChildCount, triangleDamageable);
        LevelProperties.Bee.Follower follower = new LevelProperties.Bee.Follower(followerCount, followerChargeTime, followerAttackTime, followerIntroTime, followerHomingSpeed, followerHomingRotation, followerHomingTime, followerHealth, followerHesitate, followerChildDelay, followerChildHealth, followerDamageable, followerParryable);
        LevelProperties.Bee.Chain chain = new LevelProperties.Bee.Chain(chainCount, chainDelay, chainTimeX, chainTimeY, chainSpeed, chainHesitate, chainForever);
        LevelProperties.Bee.SecurityGuard security = new LevelProperties.Bee.SecurityGuard(securitySpeed, securityAttackDelay, securityIdleTime, securityWarningTime, securityChildSpeed, securityChildCount);
        LevelProperties.Bee.General general = new LevelProperties.Bee.General(generalScreenScrollSpeed, generalMovementSpeed, generalMovementOffset);
        LevelProperties.Bee.WingSwipe wingSwipe = new LevelProperties.Bee.WingSwipe(wingMovementSpeed, wingAttackCount, wingAttackDuration, wingMaxDistance, wingWarningDuration, wingWarningMovementSpeed, wingWarningMaxDistance, wingHesitateRange);
        LevelProperties.Bee.TurbineBlasters turbine = new LevelProperties.Bee.TurbineBlasters(turbineBulletSpeed, turbineBulletCircleTime, turbineAttackDirectionString, turbineRepeatDealy, turbineHesitateRange);

        // Secority Guard
        list.Add(new LevelProperties.Bee.State(10f, new LevelProperties.Bee.Pattern[][]{new LevelProperties.Bee.Pattern[]{
                LevelProperties.Bee.Pattern.SecurityGuard}}, LevelProperties.Bee.States.Main,
                platforms, grunt, blackHole, triangle, follower, chain, security, general, wingSwipe, turbine));


        // Phase 2
        /*list.Add(new LevelProperties.Bee.State(0.76f, new LevelProperties.Bee.Pattern[][]{new LevelProperties.Bee.Pattern[]
				{LevelProperties.Bee.Pattern.Follower, LevelProperties.Bee.Pattern.Chain, LevelProperties.Bee.Pattern.Triangle,
				LevelProperties.Bee.Pattern.Chain}}, LevelProperties.Bee.States.Generic,
				platforms, grunt, blackHole, triangle, follower, chain, security, general, wingSwipe, turbine));*/
        list.Add(new LevelProperties.Bee.State(0.76f, new LevelProperties.Bee.Pattern[][]{new LevelProperties.Bee.Pattern[]
            {LevelProperties.Bee.Pattern.Triangle, LevelProperties.Bee.Pattern.Chain}}, LevelProperties.Bee.States.Generic,
            platforms, gruntOff, blackHole, triangle, follower, chain, security, general, wingSwipe, turbine));

        //Phase 3
        list.Add(new LevelProperties.Bee.State(0.41f, new LevelProperties.Bee.Pattern[][]{new LevelProperties.Bee.Pattern[]
            {LevelProperties.Bee.Pattern.Wing, LevelProperties.Bee.Pattern.Turbine}}, LevelProperties.Bee.States.Airplane,
            platforms2, gruntOff, blackHole, triangle, follower, chain, security, general, wingSwipe, turbine));

        return new LevelProperties.Bee(hp, goalTimes, list.ToArray());
    }

    public static LevelProperties.Robot RobotGetMode(On.LevelProperties.Robot.orig_GetMode orig, Level.Mode mode)
    {
        int hp = 1800;
        //1 - 0,1 = 180
        //2 - 0,15 = 270
        //3 - 0
        //4 - 0,75 = 1350
        //20 * 36 = 720
        Level.GoalTimes goalTimes = null;
        List<LevelProperties.Robot.State> list = new List<LevelProperties.Robot.State>();
        goalTimes = new Level.GoalTimes(120f, 120f, 120f);

        int laserHp = 180;
        float laserWarning = 1.5f;
        int laserDuration = 2;//1
        MinMax laserDelay = new MinMax(0f, 0f);//MinMax(2f, 5f);
        MinMax laserAimAngleParameter = new MinMax(90f, 170f);
        float laserDelayMinus = 0.8f;

        int minibotHatchHp = 180;
        int minibotHp = 40;//16
        int minibotBulletSpeed = 200;
        int minibotSpeed = 430;
        MinMax minibotPinkBulletCount = new MinMax(0f, 2f);
        int minibotCount = 1;//4
        float minibotDelay = 1f;
        MinMax minibotInitialSpawDelay = new MinMax(1f, 2f);
        MinMax minibotWaveDelay = new MinMax(3f, 5.5f);
        float minibotShootDelay = 1f;//100f
        float minibotShotDelayMinus = 0.5f;

        float cannonAttackDelay = 1.8f;
        string[] cannonSpreadVariableGroups = new string[]{
            "S360,N6,120-240",
            "S360,N5,134-226",
            "S370,N7,120-240",
            "S370,N6,125-245",
            "S380,N5,139-259",
            "S380,N7,125-245"};
        string[] cannonShoot = new string[]{
            "S4,S5,S6",
            "S5,S4,S5",
            "S2,S1,S2",
            "S1,S2,S3",
            "S4,S5,S4",
            "S4,S5,S6"};
        MinMax cannonDelay = new MinMax(2.5f, 4.5f);

        int orbChestHp = 180;
        int orbHp = 9999;
        int orbMovementSpeed = 150;
        float orbInitialLaserDelay = 1.1f;
        MinMax orbInitialSpawnDelay = new MinMax(2f, 3.5f);
        float orbSpawnDelay = 4.5f;
        float orbSpawnDelayMinus = 1f;
        bool orbShieldIsActive = true;
        float orbInitalOpenDelay = 0.5f;

        MinMax armsDelay = new MinMax(2f, 4f);
        string armsPattern = "M";// "M,T"

        float magnetStartDelay = 1f;
        float magnetStayDelay = 3f;
        float magnetForce = 1500f;//-1200

        float twistyWarningMoveAmount = 210f;
        float twistyWarningDuration = 1f;
        float twistyMoveSpeed = 380f;
        float twistyStayDuration = 0.5f;
        string twistyPosition = "350,355,345";
        float twistyBulletSpeed = 200f;
        bool twistyShootTwicePerCycle = false;

        int bombHP = 16;
        int bombInitialMovementSpeed = 290;
        float bombDelay = 6f;
        MinMax bombInitialMovementDuration = new MinMax(0f, 0f);//MinMax(1f, 2f);
        int bombBossDamage = 25;
        int bombHomingSpeed = 1000;//375
        float bombRotationSpeed = 2.5f;
        int bombLifeTime = 2000;

        int heartHp = 300;
        int heartDamageChangePercentage = 35;

        int heliheadMovementSpeed = 900;
        float heliheadOffScreenDelay = 0.5f;
        float heliheadAttackDelay = 0.1f;
        string heliheadOnScreenHeight = "250,300,350,400,450,400,350,300";

        float blueRobotRotationSpeed = 2137f;//865
        int blueRobotVerticalMovementSpeed = 250;
        MinMax blueBulletSpeed = new MinMax(200f, 500f);//new MinMax(150f, 525f);
        int blueBulletSpeedAcceleration = 50;//30
        float blueBulletSpawnDelay = 0.08f;//0.3
        int blueBulletSineWaveStrength = 0;
        float blueBulletWaveSpeedMultiplier = 0.1f;
        float blueBulletLifeTime = 7f;
        int blueNumberOfSpawnPoints = 5;
        bool blueGemWaveRotation = false;
        MinMax blueGemRotationRange = new MinMax(0f, 270f);
        string bluePinkString = "R,R,R,R,R,R,R,R,R,R,R,R,P";

        float redRobotRotationSpeed = 2137f;//865
        int redRobotVerticalMovementSpeed = 250;
        MinMax redBulletSpeed = new MinMax(30f, 50f);//new MinMax(150f, 525f);
        int redBulletSpeedAcceleration = 2;//30
        float redBulletSpawnDelay = 0.4f;//0.3
        int redBulletSineWaveStrength = 0;
        float redBulletWaveSpeedMultiplier = 0.1f;
        float redBulletLifeTime = 25f;//7f
        int redNumberOfSpawnPoints = 5;
        bool redGemWaveRotation = false;
        MinMax redGemRotationRange = new MinMax(0f, 270f);
        string redPinkString = "R,R,R,R,R,R,R,R,R,R,R,R,P";

        float inventorIdleSpeedMultiplier = 0.5f;
        float InventorInitialAttackDelay = 1f;
        MinMax inventorAttackDuration = new MinMax(3f, 5f);//new MinMax(8f, 12f);
        MinMax inventorAttackDelay = new MinMax(1f, 2f);
        string inventorGemColourString = "R,B";
        int blockadeHorizontalSpawnOffset = 1000;//0
        int blockadeHorizontalSpeed = 0;//250
        int blockadeVerticalSpeed = 0;//110
        float blockadeGroupDelay = 0f;//4.5
        float blockadeIndividualDelay = 2137f;//3.5
        int blockadeSegmentLength = 1;//8
        int blockadeGroupSize = 1;//3

        LevelProperties.Robot.Hose laser = new LevelProperties.Robot.Hose(laserHp, laserWarning, laserDuration, laserDelay, laserAimAngleParameter, laserDelayMinus);
        LevelProperties.Robot.ShotBot minibot = new LevelProperties.Robot.ShotBot(minibotHatchHp, minibotHp, minibotBulletSpeed, minibotSpeed, minibotPinkBulletCount, minibotCount, minibotDelay, minibotInitialSpawDelay, minibotWaveDelay, minibotShootDelay, minibotShotDelayMinus);
        LevelProperties.Robot.Cannon cannon = new LevelProperties.Robot.Cannon(cannonAttackDelay, cannonSpreadVariableGroups, cannonShoot, cannonDelay);
        LevelProperties.Robot.Orb orb = new LevelProperties.Robot.Orb(orbChestHp, orbHp, orbMovementSpeed, orbInitialLaserDelay, orbInitialSpawnDelay, orbSpawnDelay, orbSpawnDelayMinus, orbShieldIsActive, orbInitalOpenDelay);
        LevelProperties.Robot.Arms arms = new LevelProperties.Robot.Arms(armsDelay, armsPattern);
        LevelProperties.Robot.MagnetArms magnet = new LevelProperties.Robot.MagnetArms(magnetStartDelay, magnetStayDelay, magnetForce);
        LevelProperties.Robot.TwistyArms twisty = new LevelProperties.Robot.TwistyArms(twistyWarningMoveAmount, twistyWarningDuration, twistyMoveSpeed, twistyStayDuration, twistyPosition, twistyBulletSpeed, twistyShootTwicePerCycle);
        LevelProperties.Robot.BombBot bomb = new LevelProperties.Robot.BombBot(bombHP, bombInitialMovementSpeed, bombDelay, bombInitialMovementDuration, bombBossDamage, bombHomingSpeed, bombRotationSpeed, bombLifeTime);
        LevelProperties.Robot.Heart heart = new LevelProperties.Robot.Heart(heartHp, heartDamageChangePercentage);
        LevelProperties.Robot.HeliHead helihead = new LevelProperties.Robot.HeliHead(heliheadMovementSpeed, heliheadOffScreenDelay, heliheadAttackDelay, heliheadOnScreenHeight);
        LevelProperties.Robot.BlueGem blue = new LevelProperties.Robot.BlueGem(blueRobotRotationSpeed, blueRobotVerticalMovementSpeed, blueBulletSpeed, blueBulletSpeedAcceleration, blueBulletSpawnDelay, blueBulletSineWaveStrength, blueBulletWaveSpeedMultiplier, blueBulletLifeTime, blueNumberOfSpawnPoints, blueGemWaveRotation, blueGemRotationRange, bluePinkString);
        LevelProperties.Robot.RedGem red = new LevelProperties.Robot.RedGem(redRobotRotationSpeed, redRobotVerticalMovementSpeed, redBulletSpeed, redBulletSpeedAcceleration, redBulletSpawnDelay, redBulletSineWaveStrength, redBulletWaveSpeedMultiplier, redBulletLifeTime, redNumberOfSpawnPoints, redGemWaveRotation, redGemRotationRange, redPinkString);
        LevelProperties.Robot.Inventor inventor = new LevelProperties.Robot.Inventor(inventorIdleSpeedMultiplier, InventorInitialAttackDelay, inventorAttackDuration, inventorAttackDelay, inventorGemColourString, blockadeHorizontalSpawnOffset, blockadeHorizontalSpeed, blockadeVerticalSpeed, blockadeGroupDelay, blockadeIndividualDelay, blockadeSegmentLength, blockadeGroupSize);

        list.Add(new LevelProperties.Robot.State(10f, new LevelProperties.Robot.Pattern[][]{
            new LevelProperties.Robot.Pattern[1] }, LevelProperties.Robot.States.Main,
            laser, minibot, cannon, orb, arms, magnet, twisty, bomb, heart, helihead, blue, red, inventor));

        list.Add(new LevelProperties.Robot.State(0.9f, new LevelProperties.Robot.Pattern[][]{
            new LevelProperties.Robot.Pattern[0]}, LevelProperties.Robot.States.HeliHead,
            laser, minibot, cannon, orb, arms, magnet, twisty, bomb, heart, helihead, blue, red, inventor));

        //0.8
        list.Add(new LevelProperties.Robot.State(0.75f, new LevelProperties.Robot.Pattern[][]{
            new LevelProperties.Robot.Pattern[0]}, LevelProperties.Robot.States.Inventor,
            laser, minibot, cannon, orb, arms, magnet, twisty, bomb, heart, helihead, blue, red, inventor));

        return new LevelProperties.Robot(hp, goalTimes, list.ToArray());
    }

    public Level.Timeline RobotCreateTimeline(On.LevelProperties.Robot.orig_CreateTimeline orig, LevelProperties.Robot self, Level.Mode mode)
    {
        Level.Timeline timeline = new Level.Timeline();
        timeline.health = 3060f;//1800+180*3+720
        timeline.events.Add(new Level.Timeline.Event("HeliHead", 0.76f));
        timeline.events.Add(new Level.Timeline.Event("Invaders", 0.67f));
        timeline.events.Add(new Level.Timeline.Event("Inventor", 0.44f));
        //1 - 180*4/3060=0,24
        //2 - 270/3060=0,9
        //3 - 720/3060=0,23
        //4 - 1350/3060=0,44
        return timeline;
    }

    public static LevelProperties.Mouse MouseGetMode(On.LevelProperties.Mouse.orig_GetMode orig, Level.Mode mode)
    {
        int hp = 2600;//2100
        Level.GoalTimes goalTimes = null;
        List<LevelProperties.Mouse.State> list = new List<LevelProperties.Mouse.State>();
        goalTimes = new Level.GoalTimes(120f, 120f, 120f);
        LevelProperties.Mouse.Pattern pCherryBomb = LevelProperties.Mouse.Pattern.CherryBomb;
        LevelProperties.Mouse.Pattern pCatapult = LevelProperties.Mouse.Pattern.Catapult;
        LevelProperties.Mouse.Pattern pDash = LevelProperties.Mouse.Pattern.Dash;
        LevelProperties.Mouse.Pattern pRight = LevelProperties.Mouse.Pattern.RightClaw;
        LevelProperties.Mouse.Pattern pLeft = LevelProperties.Mouse.Pattern.LeftClaw;
        LevelProperties.Mouse.Pattern pGhost = LevelProperties.Mouse.Pattern.GhostMouse;

        float canSpeed = 300f;
        MinMax canMaxXPositionRange = new MinMax(100f, 200f);//new MinMax(250f, 450f);
        float canStopTime = 0f;
        float canInitialHesitate = 1f;

        float canDashTime = 0.95f;
        float canDashHesitate = 0.1f;//0.5
        MinMax[] canDashSpringVelocityX = new MinMax[]{
            new MinMax(-3000f, -5000f),//new MinMax(-280f, -330f)
				new MinMax(-400f, -440f)};
        MinMax[] canDashSpringVelocityY = new MinMax[]{
            new MinMax(0f, 200f),//new MinMax(600f, 650f)
				new MinMax(750f, 790f)};
        float canDashSpringGravity = 2137f;//900

        string[] bombPatterns = new string[]{
            "D:0.4, P:3,D:1.3,P:3",
            "D:0.4,P:4,D:1,P:2",
            "D:0.4,P:2,D:0.7,P:2,D:0.4,P:2",
            "D:0.4,P:3,D:1.5,P:3",
            "D:0.4,P:2,D:1,P:4",
            "D:0.4,P:2,D:0.4,P:2,D0.7,P:2"};
        float bombDelay = 1f;//0.7
        MinMax bombXVelocity = new MinMax(-50f, -600f);//new MinMax(-200f, -450f);
        MinMax bombYVelocity = new MinMax(430f, 700f);
        float bombGravity = 1150f;
        int bombChildSpeed = 1000;
        float bombHesitate = 0.7f;

        string[] catapultPatterns = new string[]{
            "BNGCG",
            "CBGGN",
            "NGGCB",
            "CGBGN",
            "NBGGC"};
        float catapultTimeIn = 0.1f;//0.7
        float catapultTimeOut = 0.1f;//0.7
        float catapultPumpDelay = 0.8f;//1
        float catapultRepeatDelay = 0.1f;//1
        int catapultProjectileSpeed = 1000;//770
        float catapultAngleOffset = 40f;//15
        float catapultSpreadAngle = 65f;
        int catapultCount = 3;//2
        int catapultHesitate = 0;//1

        MinMax rcCount = new MinMax(2f, 4f);
        float rcRepeatDelay = 1f;
        float rcSpeed = 640f;
        float rcRotationSpeed = 4f;
        float rcTimeBeforeHoming = 0.8f;
        float rcHesitate = 1f;

        string[] sawPatternString = new string[]{
            "1,6,3,5,2,4",
            "3,1,4,6,2,5",
            "1,2,5,3,6,4",
            "2,5,3,6,1,4",
            "6,2,5,1,4,3",
            "6,1,3,5,2,4"};
        float sawEntrySpeed = 75f;//50
        float sawDelayBeforeAttack = 1f;//1.7
        float sawDelayBeforeNextSaw = 0.7f;//1.8
        float sawSpeed = 500f;//300
        MinMax sawFullAttackTime = new MinMax(2f, 5f);//new MinMax(7f, 11f);

        string[] flameAttackString = new string[] { "F" };
        float flameDelayBeforeShot = 0.3f;//1
        float flameDelayAfterShot = 0.3f;//1.5
        float flameShotSpeed = 400f;
        float flameChargeTime = 0.5f;//1.3
        float flameLoopTime = 0.75f;

        float brokenSpeed = 100f;
        MinMax brokenMaxXPositionRange = new MinMax(20f, 50f);
        float brokenStopTime = 0.25f;

        float clawAttackDelay = 0.65f;
        float clawMoveSpeed = 1700f;//1500
        float clawHoldGroundTime = 0.083f;
        float clawLeaveSpeed = 5000f;//500
        string[] clawFallingObjectStrings = new string[]{
            "50,250,450,650,850,1050,1250",
            "1200,1000,800,600,400,200,0"};
        float clawObjectStartingFallSpeed = 30f;
        float clawObjectGravity = 800f;
        float clawObjectSpawnDelay = 0.4f;//0.6
        float clawHesitateAfterAttack = 0.1f;//1

        bool ghostFourMice = true;
        float ghostHp = 30f;
        float ghostJailDuration = 2f;
        MinMax ghostAttackDelayRange = new MinMax(0.1f, 0.5f);//new MinMax(2f, 3.5f);
        float ghostAttackAnticipation = 0.5f;
        float ghostBallSpeed = 720f;
        float ghostSplitSpeed = 1000f;
        MinMax ghostPinkBallRange = new MinMax(2f, 2f);
        float ghostHesitateAfterAttack = 0.1f;//1

        LevelProperties.Mouse.CanMove canMove = new LevelProperties.Mouse.CanMove(canSpeed, canMaxXPositionRange, canStopTime, canInitialHesitate);
        LevelProperties.Mouse.CanDash canDash = new LevelProperties.Mouse.CanDash(canDashTime, canDashHesitate, canDashSpringVelocityX, canDashSpringVelocityY, canDashSpringGravity);
        LevelProperties.Mouse.CanCherryBomb bomb = new LevelProperties.Mouse.CanCherryBomb(bombPatterns, bombDelay, bombXVelocity, bombYVelocity, bombGravity, bombChildSpeed, bombHesitate);
        LevelProperties.Mouse.CanCatapult catapult = new LevelProperties.Mouse.CanCatapult(catapultPatterns, catapultTimeIn, catapultTimeOut, catapultPumpDelay, catapultRepeatDelay, catapultProjectileSpeed, catapultAngleOffset, catapultSpreadAngle, catapultCount, catapultHesitate);
        LevelProperties.Mouse.CanRomanCandle romanCandle = new LevelProperties.Mouse.CanRomanCandle(rcCount, rcRepeatDelay, rcSpeed, rcRotationSpeed, rcTimeBeforeHoming, rcHesitate);
        LevelProperties.Mouse.BrokenCanSawBlades sawBlades = new LevelProperties.Mouse.BrokenCanSawBlades(sawPatternString, sawEntrySpeed, sawDelayBeforeAttack, sawDelayBeforeNextSaw, sawSpeed, sawFullAttackTime);
        LevelProperties.Mouse.BrokenCanFlame flame = new LevelProperties.Mouse.BrokenCanFlame(flameAttackString, flameDelayBeforeShot, flameDelayAfterShot, flameShotSpeed, flameChargeTime, flameLoopTime);
        LevelProperties.Mouse.BrokenCanMove brokenMove = new LevelProperties.Mouse.BrokenCanMove(brokenSpeed, brokenMaxXPositionRange, brokenStopTime);
        LevelProperties.Mouse.Claw claw = new LevelProperties.Mouse.Claw(clawAttackDelay, clawMoveSpeed, clawHoldGroundTime, clawLeaveSpeed, clawFallingObjectStrings, clawObjectStartingFallSpeed, clawObjectGravity, clawObjectSpawnDelay, clawHesitateAfterAttack);
        LevelProperties.Mouse.GhostMouse ghost = new LevelProperties.Mouse.GhostMouse(ghostFourMice, ghostHp, ghostJailDuration, ghostAttackDelayRange, ghostAttackAnticipation, ghostBallSpeed, ghostSplitSpeed, ghostPinkBallRange, ghostHesitateAfterAttack);

        list.Add(new LevelProperties.Mouse.State(10f, new LevelProperties.Mouse.Pattern[][]{
            new LevelProperties.Mouse.Pattern[]{pCherryBomb, pCatapult, pDash, pCherryBomb, pDash, pCatapult, pDash, pCatapult, pCherryBomb, pDash}}, LevelProperties.Mouse.States.Main,
            canMove, canDash, bomb, catapult, romanCandle, sawBlades, flame, brokenMove, claw, ghost));

        //0.73
        list.Add(new LevelProperties.Mouse.State(0.77f, new LevelProperties.Mouse.Pattern[][]{
            new LevelProperties.Mouse.Pattern[]{LevelProperties.Mouse.Pattern.Flame}}, LevelProperties.Mouse.States.BrokenCan,
            canMove, canDash, bomb, catapult, romanCandle, sawBlades, flame, brokenMove, claw, ghost));

        //0.37
        list.Add(new LevelProperties.Mouse.State(0.46f, new LevelProperties.Mouse.Pattern[][]{
            new LevelProperties.Mouse.Pattern[]{ pGhost, pRight, pGhost, pLeft, pGhost, pLeft, pGhost, pRight, pGhost, pRight, pGhost, pLeft, pGhost}
            }, LevelProperties.Mouse.States.Cat,
            canMove, canDash, bomb, catapult, romanCandle, sawBlades, flame, brokenMove, claw, ghost));//new

        /*list.Add(new LevelProperties.Mouse.State(0.37f, new LevelProperties.Mouse.Pattern[][]{
				new LevelProperties.Mouse.Pattern[]{pRight, pLeft, pGhost, pLeft, pRight, pGhost, pRight, pLeft, pGhost}
				}, LevelProperties.Mouse.States.Cat,
				canMove, canDash, bomb, catapult, romanCandle, sawBlades, flame, brokenMove, claw, ghost));*/

        return new LevelProperties.Mouse(hp, goalTimes, list.ToArray());
    }

    public Level.Timeline MouseCreateTimeline(On.LevelProperties.Mouse.orig_CreateTimeline orig, LevelProperties.Mouse self, Level.Mode mode)
    {
        Level.Timeline timeline = new Level.Timeline();
        /*timeline.health = 2100f;
			timeline.events.Add(new Level.Timeline.Event("BrokenCan", 0.73f));
			timeline.events.Add(new Level.Timeline.Event("Cat", 0.37f));
			}*/
        timeline.health = 2600f;//new start
        timeline.events.Add(new Level.Timeline.Event("BrokenCan", 0.77f));
        timeline.events.Add(new Level.Timeline.Event("Cat", 0.46f));//new end
        return timeline;
    }

    public static LevelProperties.Pirate PirateGetMode(On.LevelProperties.Pirate.orig_GetMode orig, Level.Mode mode)
    {
        int hp = 1400;
        Level.GoalTimes goalTimes = null;
        List<LevelProperties.Pirate.State> list = new List<LevelProperties.Pirate.State>();
        goalTimes = new Level.GoalTimes(120f, 120f, 120f);

        //LevelProperties.Pirate.Pattern pPea = LevelProperties.Pirate.Pattern.Peashot;
        //LevelProperties.Pirate.Pattern pSquid = LevelProperties.Pirate.Pattern.Squid;
        LevelProperties.Pirate.Pattern pShark = LevelProperties.Pirate.Pattern.Shark;
        //LevelProperties.Pirate.Pattern pDog = LevelProperties.Pirate.Pattern.DogFish;

        float squidstartDelay = 1f;
        int squidEndDelay = 1;
        MinMax squidHp = new MinMax(2137f, 2137f);//new MinMax(45f, 71f)
        float squidMaxTime = 2137f;//5.5
        MinMax squidXPos = new MinMax(-150f, -60f);
        float squidOpacityAdd = 0.4f;
        float squidOpacityAddTime = 0.4f;
        float squidDarkHoldTime = 2f;//4
        float squidDarkFadeTime = 3f;//5
        float squidBlobDelay = 1f;//0.12
        float squidBlobGravity = 200f;//1000
        MinMax squidBlobVelX = new MinMax(-100f, 150f);//new MinMax(-260f, 330f);
        MinMax squidBlobVelY = new MinMax(110f, 170f);//new MinMax(550f, 850f);

        float sharkStartDelay = 1f;
        float sharkEndDelay = 1f;
        float sharkFinTime = 1.8f;
        float sharkExitSpeed = 290f;
        float sharkShotExitSpeed = 340f;
        float sharkAttackDelay = 1f;
        float sharkX = -200f;//150

        float dogStartDelay = 1f;
        float dogEndDelay = 1f;
        float dogStartSpeed = 200f;//800
        float dogEndSpeed = 200f;//600
        float dogSpeedFalloffTime = 1.5f;
        int dogHp = 3;
        int dogCount = 4;
        MinMax dogNextFishDelay = new MinMax(1.5f, 1.5f);//new MinMax(0.9f, 1.3f);
        float dogDeathSpeed = 400f;

        float peaStartDelay = 2f;
        int peaEndDelay = 3;
        string[] peaPatterns = new string[]{
            "D:0.5,P:2,D:1,P3",
            "D:0.5,P:2,D:0.5,P:3",
            "D:1,P:3,D:1,P:2",
            "D:0.5,P:3,D:1.5,P:2",
            "D:1,P:4"};
        int peaDamage = 1;
        float peaSpeed = 650f;
        float peaShotDelay = 0.55f;
        string peaShotType = "P,P,R,P,R";

        float barrelDamage = 1f;
        float barrelMoveTime = 2.7f;
        float barrelFallTime = 0.9f;
        float barrelRiseTime = 1f;
        float barrelSafeTime = 2.3f;
        float barrelGroundHold = 0.8f;

        bool cannonFiring = true;
        float cannonDamage = 1f;
        float cannonSpeed = 850f;
        MinMax cannonDelayRange = new MinMax(3f, 4f);

        float boatPirateFallDelay = 3f;
        float boatPirateFallTime = 0.7f;
        float boatWinceDuration = 1f;//2
        float boatAttackDelay = 0.1f;//2
        float boatBulletSpeed = 100f;//355
        float boatBulletRotationSpeed = 230f;//460
        float boatBulletDelay = 0.1f;//0.6
        int boatBulletCount = 1;//2
        float boatBulletPostWait = 0.1f;//1.5
        float boatBeamDelay = 0.5f;//1
        float boatBeamDuration = 0.5f;//2.4
        float boatBeamPostWait = 0.1f;//1

        LevelProperties.Pirate.Squid squid = new LevelProperties.Pirate.Squid(squidstartDelay, squidEndDelay, squidHp, squidMaxTime, squidXPos, squidOpacityAdd, squidOpacityAddTime, squidDarkHoldTime, squidDarkFadeTime, squidBlobDelay, squidBlobGravity, squidBlobVelX, squidBlobVelY);
        LevelProperties.Pirate.Shark shark = new LevelProperties.Pirate.Shark(sharkStartDelay, sharkEndDelay, sharkFinTime, sharkExitSpeed, sharkShotExitSpeed, sharkAttackDelay, sharkX);
        LevelProperties.Pirate.DogFish dog = new LevelProperties.Pirate.DogFish(dogStartDelay, dogEndDelay, dogStartSpeed, dogEndSpeed, dogSpeedFalloffTime, dogHp, dogCount, dogNextFishDelay, dogDeathSpeed);
        LevelProperties.Pirate.Peashot pea = new LevelProperties.Pirate.Peashot(peaStartDelay, peaEndDelay, peaPatterns, peaDamage, peaSpeed, peaShotDelay, peaShotType);
        LevelProperties.Pirate.Barrel barrel = new LevelProperties.Pirate.Barrel(barrelDamage, barrelMoveTime, barrelFallTime, barrelRiseTime, barrelSafeTime, barrelGroundHold);
        LevelProperties.Pirate.Cannon cannon = new LevelProperties.Pirate.Cannon(cannonFiring, cannonDamage, cannonSpeed, cannonDelayRange);
        LevelProperties.Pirate.Boat boat = new LevelProperties.Pirate.Boat(boatPirateFallDelay, boatPirateFallTime, boatWinceDuration, boatAttackDelay, boatBulletSpeed, boatBulletRotationSpeed, boatBulletDelay, boatBulletCount, boatBulletPostWait, boatBeamDelay, boatBeamDuration, boatBeamPostWait);

        //Peashoot only
        /*list.Add(new LevelProperties.Pirate.State(10f, new LevelProperties.Pirate.Pattern[][]{
				new LevelProperties.Pirate.Pattern[]{pPea}},
				LevelProperties.Pirate.States.Main,
				squid, shark, dog, pea, barrel, cannon, boat));
			*/
        //Generic 0.92
        list.Add(new LevelProperties.Pirate.State(10f, new LevelProperties.Pirate.Pattern[][]{
            new LevelProperties.Pirate.Pattern[]{pShark},
            new LevelProperties.Pirate.Pattern[]{pShark}
            }, LevelProperties.Pirate.States.Generic,
            squid, shark, dog, pea, barrel, cannon, boat));

        //Boat activated 0.77
        list.Add(new LevelProperties.Pirate.State(0.99f, new LevelProperties.Pirate.Pattern[][]{
            new LevelProperties.Pirate.Pattern[]{pShark},
            new LevelProperties.Pirate.Pattern[]{pShark}
            }, LevelProperties.Pirate.States.Generic,
            squid, shark, dog, pea, barrel, cannon, boat));
        /*   new LevelProperties.Pirate.Pattern[]{pPea, pSquid, pPea, pShark, pPea, pDog},
            new LevelProperties.Pirate.Pattern[]{pShark, pPea, pSquid, pPea, pDog, pPea}*/

        //Final
        list.Add(new LevelProperties.Pirate.State(0.32f, new LevelProperties.Pirate.Pattern[][]{
            new LevelProperties.Pirate.Pattern[]{LevelProperties.Pirate.Pattern.Boat}}, LevelProperties.Pirate.States.Boat,
            squid, shark, dog, pea, barrel, cannon, boat));

        return new LevelProperties.Pirate(hp, goalTimes, list.ToArray());
    }

    public Level.Timeline PirateCreateTimeline(On.LevelProperties.Pirate.orig_CreateTimeline orig, LevelProperties.Pirate self, Level.Mode mode)
    {
        Level.Timeline timeline = new Level.Timeline();
        timeline.health = 1400f;
        timeline.events.Add(new Level.Timeline.Event("Boat", 0.32f));
        return timeline;
    }

    public static LevelProperties.SallyStagePlay SallyGetMode(On.LevelProperties.SallyStagePlay.orig_GetMode orig, Level.Mode mode)
    {
        int hp = 1700;
        Level.GoalTimes goalTimes = null;
        List<LevelProperties.SallyStagePlay.State> list = new List<LevelProperties.SallyStagePlay.State>();
        goalTimes = new Level.GoalTimes(120f, 120f, 120f);
        LevelProperties.SallyStagePlay.Pattern pJump = LevelProperties.SallyStagePlay.Pattern.Jump;
        LevelProperties.SallyStagePlay.Pattern pKiss = LevelProperties.SallyStagePlay.Pattern.Kiss;
        LevelProperties.SallyStagePlay.Pattern pTeleport = LevelProperties.SallyStagePlay.Pattern.Teleport;

        string jumpAttackString = "1,1,2,1,2,1,1,1,2";
        string jumpAttackCountString = "1,3,2,3,1,2,3,2,2,3";
        MinMax jumpHesitate = new MinMax(0f, 0.1f);//new MinMax(0f, 0.5f);
        float jumpDelay = 0.1f;//0.3

        float diveSpeed = 700f;
        MinMax diveAngleRange = new MinMax(15f, 60f);//new MinMax(25f, 35f);
        MinMax diveAttackHeight = new MinMax(350f, 430f);

        float rollVerticalMovement = 100f;
        MinMax rollHorizontalMovement = new MinMax(50f, 100f);
        MinMax rollHeight = new MinMax(200f, 300f);
        string rollAttackTypeString = "B";
        float rollRollDuration = 0.3f;
        MinMax rollShotDelayRange = new MinMax(0.8f, 1f);//new MinMax(0f, 0.3f);

        float shurikenInitialMovementSpeed = 850f;
        float shurikenArcOneGravity = 20f;
        float shurikenArcOneVerticalVelocity = 625f;
        float shurikenArcOneHorizontalVelocity = 150f;
        float shurikenArcTwoGravity = 20f;
        float shurikenArcTwoVerticalVelocity = 625f;
        float shurikenArcTwoHorizontalVelocity = 125f;
        int shurikenNumberOfChildSpawns = 2;

        float projectileSpeed = 750f;
        float projectileGroundDuration = 5f;//7f
        float projectileGroundSize = 2f;

        float umbrellaInitialAttackDelay = 0.5f;
        float umbrellaObjectSpeed = 600f;//430
        float umbrellaObjectDropSpeed = 400f;//265
        int umbrellaObjectCount = 3;//2
        float umbrellaObjectDelay = 1f;
        float umbrellaHesitate = 2f;
        float umbrellaHomingMaxSpeed = 745f;
        float umbrellaHomingAcceleration = 1300f;
        float umbrellaHomingBounceRatio = 0.5f;
        float umbrellaHomingUntilSwitchPlayer = 10f;

        float kissHeartSpeed = 500f;//170
        string kissHeartType = "P,P";
        float kissSineWaveSpeed = 10f;//5.6
        float kissSineWaveStrength = 250f;//175
        float kissHesitate = 0.1f;//0.5

        string teleportAppearOffsetString = "0,100,-100,0,200,-200,50,-200,0,150";
        MinMax teleportFallingSpeed = new MinMax(100f, 100f);//new MinMax(300f, 650f);
        float teleportAcceleration = 6.5f;
        float teleportHesitate = 0.1f;
        float teleportOffScreenDelay = 0.2f;
        float teleportSawAttackDuration = 1f;

        float babyBottleSpeed = 410f;
        float babyAttackDelay = 1f;
        int babyHP = 15;
        MinMax babyReappearDelayRange = new MinMax(1.3f, 2.1f);
        string[] babyAppearPosition = new string[]{
            "1,5,4,6,2,9,3,7,8,1,7,4,6,2,9,3,5,8",
            "3,9,7,1,4,2,5,8,6,3,2,7,1,4,9,5,8,6",
            "5,6,9,3,1,7,4,2,8,5,6,9,3,8,7,4,2,1",
            "1,3,5,7,8,2,4,6,9,1,5,7,8,3,2,4,6,9"};
        float babyHesitate = 0.1f;

        float nunRulerSpeed = 410f;
        float nunAttackDelay = 0.7f;
        int nunHP = 15;
        MinMax nunReappearDelayRange = new MinMax(2.5f, 3.5f);
        string[] nunAppearPosition = new string[]{
            "1,5,4,2,9,3,8,1,7,4,6,2,3,5",
            "3,7,1,4,2,5,3,2,1,4,9,5,8,6",
            "5,6,9,3,1,7,4,2,8,5,3,4,2,1",
            "1,3,5,7,8,2,4,6,9,1,5,7,8,3,2,4,6,9"};
        float nunHesitate = 0.3f;
        string[] nunPinkString = new string[] { "P,R,R,P,R" };

        float husbandHP = 400f;
        MinMax husbandShotDelayRange = new MinMax(3f, 4.5f);
        float husbandShotSpeed = 400f;
        float husbandShotScale = 1f;

        string[] phaseTwoAttackString = new string[]{
            "L,L,M,L,T,L,M,T,L,L,M,T",
            "L,M,T,L,M,L,T,L,M,T,L,L,M,L,T"};
        MinMax phaseTwoAttackDelayRange = new MinMax(0.5f, 1.2f);
        float finalMovementSpeed = 3.5f;
        float finalCupidDropMaxY = 400f;
        float finalCupidMoveSpeed = 80f;

        float lightningSpeed = 800f;//650
        string lightningAngleString = "140,190,165,150,130,180,145,165,140,175,155,160";
        MinMax lightningDirectAimRange = new MinMax(4f, 6f);
        string lightningShotCount = "3,3,4,4,3,4,4,3,4,4,4,3,4,3,3,4";//"3,2,4,2,3,4,2,3,4,3,3,4,4,3";
        MinMax lightningDelayRange = new MinMax(0.5f, 0.9f);
        string lightningSpawnString = "1100,540,320,1170,655,827,344,916,626,1050,1200,360,710,1100,440,844,570,1150,377,687,954,515,905";

        float meteorSpeed = 455f;
        int meteorHP = 35;
        float meteorHookSpeed = 100f;
        float meteorHookMaxHeight = 200f;
        float meteorHookRevealExitDelay = 1f;
        float meteorHookParryExitDelay = 2f;
        float meteorSize = 1f;
        string meteorSpawnString = "1000,750,950,800,1100,850,750,900,1200,950,750,900,1150";

        float tidalSpeed = 450f;
        float tidalSize = 1.2f;
        float tidalHesitate = 0.1f;

        MinMax roseFallSpeed = new MinMax(300f, 550f);
        float roseFallAcceleration = 2f;
        float roseGroundDuration = 0f;
        string[] roseSpawnString = new string[] {
            "1239,499,999,1103,1180,172,387,1094,220,89,1086,1192,551,253",
            "887,200,97,744,734,23,1013,703,801,666,719,489,1052,621",
            "1226,121,141,829,715,423,1214,263,581,1007,213,649,813,1254",
            "118,236,558,63,365,1032,938,1075,1219,390,631,999,821,861",
            "480,1044,772,586,689,722,498,422,621,706,816,702,1263,60",
            "1008,159,552,458,934,1002,71,934,868,527,998,493,109,468",
            "223,876,884,785,73,311,742,696,745,896,1007,808,35,67",
            "592,835,294,139,248,984,885,1191,584,712,472,269,733,1219",
            "1165,1137,48,1035,693,160,1091,948,178,480,1255,756,769,785",
            "817,248,306,450,1212,291,886,1176,206,651,257,1249,837,509"};
        //{ "100,500,200,1100,800,600,400,1000,700,100,900,300,600,1000" };
        MinMax roseSpawnDelayRange = new MinMax(0.2f, 0.4f);// new MinMax(2.5f, 3.5f);
        MinMax rosePlayerAimRange = new MinMax(4f, 7f);

        LevelProperties.SallyStagePlay.Jump jump = new LevelProperties.SallyStagePlay.Jump(jumpAttackString, jumpAttackCountString, jumpHesitate, jumpDelay);
        LevelProperties.SallyStagePlay.DiveKick dive = new LevelProperties.SallyStagePlay.DiveKick(diveSpeed, diveAngleRange, diveAttackHeight);
        LevelProperties.SallyStagePlay.JumpRoll roll = new LevelProperties.SallyStagePlay.JumpRoll(rollVerticalMovement, rollHorizontalMovement, rollHeight, rollAttackTypeString, rollRollDuration, rollShotDelayRange);
        LevelProperties.SallyStagePlay.Shuriken shuriken = new LevelProperties.SallyStagePlay.Shuriken(shurikenInitialMovementSpeed, shurikenArcOneGravity, shurikenArcOneVerticalVelocity, shurikenArcOneHorizontalVelocity, shurikenArcTwoGravity, shurikenArcTwoVerticalVelocity, shurikenArcTwoHorizontalVelocity, shurikenNumberOfChildSpawns);
        LevelProperties.SallyStagePlay.Projectile projectile = new LevelProperties.SallyStagePlay.Projectile(projectileSpeed, projectileGroundDuration, projectileGroundSize);
        LevelProperties.SallyStagePlay.Umbrella umbrella = new LevelProperties.SallyStagePlay.Umbrella(umbrellaInitialAttackDelay, umbrellaObjectSpeed, umbrellaObjectDropSpeed, umbrellaObjectCount, umbrellaObjectDelay, umbrellaHesitate, umbrellaHomingMaxSpeed, umbrellaHomingAcceleration, umbrellaHomingBounceRatio, umbrellaHomingUntilSwitchPlayer);
        LevelProperties.SallyStagePlay.Kiss kiss = new LevelProperties.SallyStagePlay.Kiss(kissHeartSpeed, kissHeartType, kissSineWaveSpeed, kissSineWaveStrength, kissHesitate);
        LevelProperties.SallyStagePlay.Teleport teleport = new LevelProperties.SallyStagePlay.Teleport(teleportAppearOffsetString, teleportFallingSpeed, teleportAcceleration, teleportHesitate, teleportOffScreenDelay, teleportSawAttackDuration);
        LevelProperties.SallyStagePlay.Baby baby = new LevelProperties.SallyStagePlay.Baby(babyBottleSpeed, babyAttackDelay, babyHP, babyReappearDelayRange, babyAppearPosition, babyHesitate);
        LevelProperties.SallyStagePlay.Nun nun = new LevelProperties.SallyStagePlay.Nun(nunRulerSpeed, nunAttackDelay, nunHP, nunReappearDelayRange, nunAppearPosition, nunHesitate, nunPinkString);
        LevelProperties.SallyStagePlay.Husband husband = new LevelProperties.SallyStagePlay.Husband(husbandHP, husbandShotDelayRange, husbandShotSpeed, husbandShotScale);
        LevelProperties.SallyStagePlay.General general = new LevelProperties.SallyStagePlay.General(phaseTwoAttackString, phaseTwoAttackDelayRange, finalMovementSpeed, finalCupidDropMaxY, finalCupidMoveSpeed);
        LevelProperties.SallyStagePlay.Lightning lightning = new LevelProperties.SallyStagePlay.Lightning(lightningSpeed, lightningAngleString, lightningDirectAimRange, lightningShotCount, lightningDelayRange, lightningSpawnString);
        LevelProperties.SallyStagePlay.Meteor meteor = new LevelProperties.SallyStagePlay.Meteor(meteorSpeed, meteorHP, meteorHookSpeed, meteorHookMaxHeight, meteorHookRevealExitDelay, meteorHookParryExitDelay, meteorSize, meteorSpawnString);
        LevelProperties.SallyStagePlay.Tidal tidal = new LevelProperties.SallyStagePlay.Tidal(tidalSpeed, tidalSize, tidalHesitate);
        LevelProperties.SallyStagePlay.Roses rose = new LevelProperties.SallyStagePlay.Roses(roseFallSpeed, roseFallAcceleration, roseGroundDuration, roseSpawnString, roseSpawnDelayRange, rosePlayerAimRange);

        //First phase
        list.Add(new LevelProperties.SallyStagePlay.State(10f, new LevelProperties.SallyStagePlay.Pattern[][]{
        new LevelProperties.SallyStagePlay.Pattern[]{pJump, pKiss, pTeleport, pJump, pKiss, pJump, pTeleport}
        }, LevelProperties.SallyStagePlay.States.Main,
        jump, dive, roll, shuriken, projectile, umbrella, kiss, teleport, baby, nun, husband, general, lightning, meteor, tidal, rose));

        //House
        list.Add(new LevelProperties.SallyStagePlay.State(0.72f, new LevelProperties.SallyStagePlay.Pattern[][]{
        new LevelProperties.SallyStagePlay.Pattern[]{LevelProperties.SallyStagePlay.Pattern.Umbrella}},
        LevelProperties.SallyStagePlay.States.House,
        jump, dive, roll, shuriken, projectile, umbrella, kiss, teleport, baby, nun, husband, general, lightning, meteor, tidal, rose));

        //Angel
        list.Add(new LevelProperties.SallyStagePlay.State(0.43f, new LevelProperties.SallyStagePlay.Pattern[][]{
        new LevelProperties.SallyStagePlay.Pattern[0]}, LevelProperties.SallyStagePlay.States.Angel,
        jump, dive, roll, shuriken, projectile, umbrella, kiss, teleport, baby, nun, husband, general, lightning, meteor, tidal, rose));

        //Final
        list.Add(new LevelProperties.SallyStagePlay.State(0.14f, new LevelProperties.SallyStagePlay.Pattern[][]{
        new LevelProperties.SallyStagePlay.Pattern[0]}, LevelProperties.SallyStagePlay.States.Final,
        jump, dive, roll, shuriken, projectile, umbrella, kiss, teleport, baby, nun, husband, general, lightning, meteor, tidal, rose));

        return new LevelProperties.SallyStagePlay(hp, goalTimes, list.ToArray());
    }

    public static LevelProperties.FlyingMermaid MermaidGetMode(On.LevelProperties.FlyingMermaid.orig_GetMode orig, Level.Mode mode)
    {
        int hp = 2500;//3000
        Level.GoalTimes goalTimes = null;
        List<LevelProperties.FlyingMermaid.State> list = new List<LevelProperties.FlyingMermaid.State>();
        goalTimes = new Level.GoalTimes(120f, 120f, 120f);

        LevelProperties.FlyingMermaid.Pattern pFish = LevelProperties.FlyingMermaid.Pattern.Fish;
        LevelProperties.FlyingMermaid.Pattern pYell = LevelProperties.FlyingMermaid.Pattern.Yell;
        LevelProperties.FlyingMermaid.Pattern pSummon = LevelProperties.FlyingMermaid.Pattern.Summon;
        LevelProperties.FlyingMermaid.Pattern pZap = LevelProperties.FlyingMermaid.Pattern.Zap;
        LevelProperties.FlyingMermaid.Pattern pEel = LevelProperties.FlyingMermaid.Pattern.Eel;
        LevelProperties.FlyingMermaid.Pattern pBubbleHeadBlast = LevelProperties.FlyingMermaid.Pattern.BubbleHeadBlast;
        LevelProperties.FlyingMermaid.Pattern pBubble = LevelProperties.FlyingMermaid.Pattern.Bubble;


        string[] yellPatternString = new string[]
        {
            "Y1,D0.5,Y1,D0.5,Y1"
        };
        float yellAnticipateInitialHold = 0.5f;
        float yellMouthHold = 0.5f;
        MinMax yellSpreadAngle = new MinMax(120f, 400f);//new MinMax(140f, 240f);
        int yellNumBullets = 3;
        float yellBulletSpeed = 1600f;
        float yellAnticipateHold = 0.25f;
        float yellHesitateAfterAttack = 0f;

        float summonHoldBeforeCreature = 0.25f;
        float summonHoldAfterCreature = 1f;
        float summonHesitateAfterAttack = 0f;

        float horseHp = 30f;//100
        float horseMaxSpeed = 800f;
        float horseAcceleration = 1300f;
        float horseBounceRatio = 1f;
        float horseWaterForce = -200f;//480
        float horseHomingDuration = 2137f;//6.2

        float pufferHp = 10f;
        float pufferFloatSpeed = 320f;
        float pufferDelay = 3f;//1.1
        float pufferSpawnDuration = 6f;//8
        string[] pufferSpawnString = new string[] { "0-100-200-300-400-500-600-700-800-900" };
            /*new string[]{
            "50-300-550-800,175-425-675,275-525,175-425-675",
            "50-200-350,450-600-750,150-300-450,550-700-850",
            "50-850-450,150-300,50-850-450,550-700"};*/
        MinMax pufferPinkPufferSpawnRange = new MinMax(7f, 10f);

        float turtleHp = 50f;//180
        MinMax turtleAppearPosition = new MinMax(900f, 1050f);
        float turtleSpeed = 200f;
        float turtleBulletSpeed = 400f;//900
        MinMax turtleTimeUntilShoot = new MinMax(1f, 1.8f);
        MinMax turtleBulletTimeToExplode = new MinMax(0.8f, 1.2f);//new MinMax(0.4f, 0.65f);
        float turtleSpreadshotBulletSpeed = 100f;//445
        string[] turtleExplodeSpreadshotString = new string[]{
            "350-35-80-125-180-225-260-305,D0.6,350-35-80-125-180-225-260-305,D0.6,350-35-80-125-180-225-260-305",
            "350-35-80-125-180-225-260-305,D0.7,350-35-80-125-180-225-260-305,D0.6,350-35-80-125-180-225-260-305",
            "350-35-80-125-180-225-260-305,D0.5,350-35-80-125-180-225-260-305,D0.5,350-35-80-125-180-225-260-305"};
        float turtleSpiralRate = 0f;

        float fishDelayBeforeFirstAttack = 0.5f;//1f
        float fishDelayBeforeFly = 0.5f;//1.9f
        float fishFlyingSpeed = 1150f;
        float fishFlyingUpSpeed = 1500f;//300
        float fishFlyingGravity = 3200f;
        float fishHesitateAfterAttack = 0f;

        float spreadshotFishAttackDelay = 0.1f;
        string[] spreadshotFishVariableGroups = new string[]{
            "S300,N6,100-220",
            "S350,N5,112-208",
            "S300,N6,90-210",
            "S350,N5,102-198",
            "S300,N6,80-200",
            "S350,N5,92-188"};
        /*"S600,N6,100-220",
            "S700,N5,112-208",
            "S600,N6,90-210",
            "S700,N5,102-198",
            "S600,N6,80-200",
            "S700,N5,92-188"};*/
        string[] spreadshotFishShootString = new string[]{
            "S1,S2,S1,S2",
            "S3,S4,S3,S4",
            "S5,S6,S5,S6"};
        string spreadshotFishPinkString = "R,R,R,R,R,R,P,R,R,R,R,R,R,R,P";

        float spinnerFishBulletSpeed = 320f;
        float spinnerFishTimeBeforeTails = 0.3f;
        float spinnerFishRotationSpeed = 215f;
        float spinnerFishAttackDelay = 0.2f;//0.9
        string[] spinnerFishShootString = new string[]{
            /*"S,S,D1.5,S",
            "S,D1.4,S,S",
            "S,S,D1.3,S",
            "S,D1.6,S,S",*/
        "S,S,S,S,S"};//new

        float homerFishInitSpeed = 1000f;
        float homerFishTimeBeforeHoming = 0.78f;
        float homerFishBulletSpeed = 350f;//475
        float homerFishRotationSpeed = 10f;//3.3f
        float homerFishTimeBeforeDeath = 3.5f;//2.6
        float homerFishAttackDelay = 1.5f;
        string[] homerFishShootString = new string[] { "S,S" };

        float eelHp = 100f;//45
        MinMax eelAttackAmount = new MinMax(2137f, 2137f);//new MinMax(0f, 1f);
        MinMax eelIdleTime = new MinMax(0f, 3f);//new MinMax(1.2f, 2.7f);
        MinMax eelAppearDelay = new MinMax(0f, 10f);//new MinMax(3f, 7f);
        MinMax eelSpreadAngle = new MinMax(105f, 225f);
        float eelNumBullets = 8f;//5
        float eelBulletSpeed = 200f;//540
        float eelHesitateAfterAttack = 0.1f;
        string eelBulletPinkString = "R,P,R,R,R,R,R,R,R,R,P,R,P";//"R,R,R,R,R,R,R,R,R,R,R,R,P";

        float zapAttackTime = 0.1f;//0.5
        MinMax zapHesitateAfterAttack = new MinMax(1f, 2f);//new MinMax(3.5f, 7f);
        float zapStoneTime = 1f;//2.3

        float bubblesMovementSpeed = 700f;//285
        float bubblesWaveSpeed = 4f;//3
        float bubblesWaveAmount = 8f;//5f
        float bubblesHp = 2137f;//1
        MinMax bubblesAttackDelayRange = new MinMax(0.5f, 1f);//new MinMax(1f, 2f);

        float headBlastMovementSpeed = 745f;
        MinMax headBlastAttackDelayRange = new MinMax(2137f, 2137f);//new MinMax(3f, 4.1f);

        float coralMoveSpeed = 1000f;//415
        string[] coraYellowDotPosString = new string[]{
            "0,60,120",
            "-60,-120",
            "0,60",
            "60,120",
            "0,-60,-120",
            "0,-60",
            "-60,-120"};
        MinMax coraYellowSpawnDelayRange = new MinMax(2137f, 2137f);// new MinMax(2.5f, 3.8f);
        MinMax coraBubbleEyewaveSpawnDelayRange = new MinMax(3f, 4.1f);

        LevelProperties.FlyingMermaid.Yell yell = new LevelProperties.FlyingMermaid.Yell(yellPatternString, yellAnticipateInitialHold, yellMouthHold, yellSpreadAngle, yellNumBullets, yellBulletSpeed, yellAnticipateHold, yellHesitateAfterAttack);
        LevelProperties.FlyingMermaid.Summon summon = new LevelProperties.FlyingMermaid.Summon(summonHoldBeforeCreature, summonHoldAfterCreature, summonHesitateAfterAttack);
        LevelProperties.FlyingMermaid.Seahorse horse = new LevelProperties.FlyingMermaid.Seahorse(horseHp, horseMaxSpeed, horseAcceleration, horseBounceRatio, horseWaterForce, horseHomingDuration);
        LevelProperties.FlyingMermaid.Pufferfish puffer = new LevelProperties.FlyingMermaid.Pufferfish(pufferHp, pufferFloatSpeed, pufferDelay, pufferSpawnDuration, pufferSpawnString, pufferPinkPufferSpawnRange);
        LevelProperties.FlyingMermaid.Turtle turtle = new LevelProperties.FlyingMermaid.Turtle(turtleHp, turtleAppearPosition, turtleSpeed, turtleBulletSpeed, turtleTimeUntilShoot, turtleBulletTimeToExplode, turtleSpreadshotBulletSpeed, turtleExplodeSpreadshotString, turtleSpiralRate);
        LevelProperties.FlyingMermaid.Fish fish = new LevelProperties.FlyingMermaid.Fish(fishDelayBeforeFirstAttack, fishDelayBeforeFly, fishFlyingSpeed, fishFlyingUpSpeed, fishFlyingGravity, fishHesitateAfterAttack);
        LevelProperties.FlyingMermaid.SpreadshotFish spreadshotFish = new LevelProperties.FlyingMermaid.SpreadshotFish(spreadshotFishAttackDelay, spreadshotFishVariableGroups, spreadshotFishShootString, spreadshotFishPinkString);
        LevelProperties.FlyingMermaid.SpinnerFish spinnerFish = new LevelProperties.FlyingMermaid.SpinnerFish(spinnerFishBulletSpeed, spinnerFishTimeBeforeTails, spinnerFishRotationSpeed, spinnerFishAttackDelay, spinnerFishShootString);
        LevelProperties.FlyingMermaid.HomerFish homerFish = new LevelProperties.FlyingMermaid.HomerFish(homerFishInitSpeed, homerFishTimeBeforeHoming, homerFishBulletSpeed, homerFishRotationSpeed, homerFishTimeBeforeDeath, homerFishAttackDelay, homerFishShootString);
        LevelProperties.FlyingMermaid.Eel eel = new LevelProperties.FlyingMermaid.Eel(eelHp, eelAttackAmount, eelIdleTime, eelAppearDelay, eelSpreadAngle, eelNumBullets, eelBulletSpeed, eelHesitateAfterAttack, eelBulletPinkString);
        LevelProperties.FlyingMermaid.Zap zap = new LevelProperties.FlyingMermaid.Zap(zapAttackTime, zapHesitateAfterAttack, zapStoneTime);
        LevelProperties.FlyingMermaid.Bubbles bubbles = new LevelProperties.FlyingMermaid.Bubbles(bubblesMovementSpeed, bubblesWaveSpeed, bubblesWaveAmount, bubblesHp, bubblesAttackDelayRange);
        LevelProperties.FlyingMermaid.HeadBlast headBlast = new LevelProperties.FlyingMermaid.HeadBlast(headBlastMovementSpeed, headBlastAttackDelayRange);
        LevelProperties.FlyingMermaid.Coral coral = new LevelProperties.FlyingMermaid.Coral(coralMoveSpeed, coraYellowDotPosString, coraYellowSpawnDelayRange, coraBubbleEyewaveSpawnDelayRange);

        //First
        list.Add(new LevelProperties.FlyingMermaid.State(10f, new LevelProperties.FlyingMermaid.Pattern[][]{
        new LevelProperties.FlyingMermaid.Pattern[]{pFish, pSummon, pYell, pSummon}}, LevelProperties.FlyingMermaid.States.Main,
        yell, summon, horse, puffer, turtle, fish, spreadshotFish, spinnerFish, homerFish, eel, zap, bubbles, headBlast, coral));

        //Second 0.6
        list.Add(new LevelProperties.FlyingMermaid.State(0.7f, new LevelProperties.FlyingMermaid.Pattern[][]{
        new LevelProperties.FlyingMermaid.Pattern[]{pEel, pZap}}, LevelProperties.FlyingMermaid.States.Merdusa,
        yell, summon, horse, puffer, turtle, fish, spreadshotFish, spinnerFish, homerFish, eel, zap, bubbles, headBlast, coral));

        //Last 0.3
        list.Add(new LevelProperties.FlyingMermaid.State(0.35f, new LevelProperties.FlyingMermaid.Pattern[][]{
        new LevelProperties.FlyingMermaid.Pattern[]{pBubbleHeadBlast, pBubble, pBubbleHeadBlast, pBubble, pBubble,
        pBubbleHeadBlast, pBubble, pBubble, pBubbleHeadBlast, pBubble, pBubbleHeadBlast, pBubble, pBubbleHeadBlast}},
        LevelProperties.FlyingMermaid.States.Head,
        yell, summon, horse, puffer, turtle, fish, spreadshotFish, spinnerFish, homerFish, eel, zap, bubbles, headBlast, coral));

        return new LevelProperties.FlyingMermaid(hp, goalTimes, list.ToArray());
    }

    public Level.Timeline MermaidCreateTimeline(On.LevelProperties.FlyingMermaid.orig_CreateTimeline orig, LevelProperties.FlyingMermaid self, Level.Mode mode)
    {
        Level.Timeline timeline = new Level.Timeline();
        timeline.health = 2500f;//3000
        timeline.events.Add(new Level.Timeline.Event("Merdusa", 0.7f));//0.6
        timeline.events.Add(new Level.Timeline.Event("Head", 0.35f));//0.3
        return timeline;
    }

    public static LevelProperties.Train TrainGetMode(On.LevelProperties.Train.orig_GetMode orig, Level.Mode mode)
    {
        int hp = 500;
        Level.GoalTimes goalTimes = null;
        List<LevelProperties.Train.State> list = new List<LevelProperties.Train.State>();
        goalTimes = new Level.GoalTimes(120f, 120f, 120f);

        int specterHealth = 425;
        int specterAttackLoops = 2;//3
        MinMax specterHeightMax = new MinMax(150f, 400f);//new MinMax(100f, 300f);
        MinMax specterTimeX = new MinMax(2.5f, 4.5f);//new MinMax(3.4f, 4.3f);
        MinMax specterTimeY = new MinMax(0.5f, 1.8f);//new MinMax(0.5f, 0.7f);
        float specterHesitate = 1.4f;
        float specterEyeHealth = 2137f;//3.5

        float skeletonHealth = 325f;
        MinMax skeletonAttackDelay = new MinMax(0.5f, 1f);//MinMax(2f, 2.5f);
        float skeletonAppearDelay = 1f;//2f
        float skeletonSlapHoldTime = 0.5f;//1.5f

        float ghoulHealth = 200f;
        float ghoulInitDelay = 0f;//1.5f
        float ghoulMainDelay = 0.5f;
        float ghoulWarningTime = 0.5f;//1f
        float ghoulMoveTime = 7f;//1.8
        float ghoulMoveDistance = 800f;//600f
        float ghoulCannonDelay = 2137f;//2.7f
        float ghoulGhostDelay = 1f;
        float ghoulGhostSpeed = 275f;
        float ghoulGhostAimSpeed = 1.35f;
        float ghoulGhostHealth = 5f;
        float ghoulSkullSpeed = 900f;

        float engineHealth = 200f;
        float engineForwardTime = 2.9f;
        float engineBackTime = 3.6f;
        MinMax engineDoorTime = new MinMax(4.5f, 6f);
        float engineTailDelay = 1f;
        float engineMaxDist = -300f;
        float engineMinDist = 425f;
        float engineFireDelay = 0.2f;//0.35f
        int engineFireGravity = 800;
        MinMax engineFireVelocityX = new MinMax(-325f, 325f);
        MinMax engineFireVelocityY = new MinMax(400f, 650f);
        float engineProjectileDelay = 3.8f;
        float engineProjectileUpSpeed = 650f;
        float engineProjectileXSpeed = 850f;
        float engineProjectileGravity = 1000f;

        string pumpkinBossPhaseOn = "1,2";//"1,2,4"
        float pumpkinHealth = 4f;
        float pumpkinSpeed = 250f;
        float pumpkinFallTime = 1.5f;
        float pumpkinDelay = 3.5f;//4.5

        LevelProperties.Train.BlindSpecter specter = new LevelProperties.Train.BlindSpecter(specterHealth, specterAttackLoops, specterHeightMax, specterTimeX, specterTimeY, specterHesitate, specterEyeHealth);
        LevelProperties.Train.Skeleton skeleton = new LevelProperties.Train.Skeleton(skeletonHealth, skeletonAttackDelay, skeletonAppearDelay, skeletonSlapHoldTime);
        LevelProperties.Train.LollipopGhouls ghoul = new LevelProperties.Train.LollipopGhouls(ghoulHealth, ghoulInitDelay, ghoulMainDelay, ghoulWarningTime, ghoulMoveTime, ghoulMoveDistance, ghoulCannonDelay, ghoulGhostDelay, ghoulGhostSpeed, ghoulGhostAimSpeed, ghoulGhostHealth, ghoulSkullSpeed);
        LevelProperties.Train.Engine engine = new LevelProperties.Train.Engine(engineHealth, engineForwardTime, engineBackTime, engineDoorTime, engineTailDelay, engineMaxDist, engineMinDist, engineFireDelay, engineFireGravity, engineFireVelocityX, engineFireVelocityY, engineProjectileDelay, engineProjectileUpSpeed, engineProjectileXSpeed, engineProjectileGravity);
        LevelProperties.Train.Pumpkins pumpkin = new LevelProperties.Train.Pumpkins(pumpkinBossPhaseOn, pumpkinHealth, pumpkinSpeed, pumpkinFallTime, pumpkinDelay);

        list.Add(new LevelProperties.Train.State(10f, new LevelProperties.Train.Pattern[][]{
        new LevelProperties.Train.Pattern[1]}, LevelProperties.Train.States.Main,
        specter, skeleton, ghoul, engine, pumpkin));

        return new LevelProperties.Train(hp, goalTimes, list.ToArray());
    }

    public static LevelProperties.DicePalaceMain KingDiceGetMode(On.LevelProperties.DicePalaceMain.orig_GetMode orig, Level.Mode mode)
    {
        int hp = 400;//750
        Level.GoalTimes goalTimes = null;
        List<LevelProperties.DicePalaceMain.State> list = new List<LevelProperties.DicePalaceMain.State>();
        goalTimes = new Level.GoalTimes(60f, 60f, 60f);

        float diceMovementSpeed = 1.5f;
        float diceStartPositionOneX = -300f;
        float diceStartPositionOneY = 1000f;
        float diceStartPositionTwoX = 300f;
        float diceStartPositionTwoY = 50f;
        float diceRollFrameCount = 8f;
        float dicePauseWhenRolled = 1f;
        float diceRevealDelay = 2f;

        float cardSpeed = 410f;
        string[] cardString = new string[]
        {
                "R,P,R,R,R,P,R,P,R,R,R,R,P,R,R,P",
                "R,R,P,R,R,R,R,P,R,P,R,R,R,P,R,R",
                "R,R,R,P,R,R,R,P,R,R,R,P,R,P,R,R",
                "R,R,P,R,R,P,R,P,R,R,R,R,P,R,R,P",
                "R,P,R,R,P,R,R,R,P,R,R,P,R,R,R,R",
                "R,R,R,P,R,P,R,R,R,P,R,R,R,P,R,R",
                "R,P,R,R,R,R,P,R,R,R,P,R,R,P,R,P",
                "R,R,P,R,P,R,R,R,P,R,R,R,R,P,R,R",
                "R,P,R,P,R,R,R,R,P,R,R,P,R,R,R,P",
                "R,R,R,P,R,P,R,R,P,R,P,R,R,P,R,R",
                "R,R,P,R,R,P,R,R,P,R,R,R,R,P,R,R",
                "R,R,P,R,P,R,R,R,P,R,R,R,R,P,R,P",
                "R,R,P,R,R,P,R,P,R,R,R,P,R,R,R,R"
        };
        string[] cardSideOrder = new string[]
        {
                "L,R,L,R,L,R,L,R,L,R,L,R"//"L,R,L,R,L,L,R,L,R,L,R,R"
        };
        float cardHesitate = 3f;
        float cardScale = 0.5f;
        float cardDelay = 0.3f;

        LevelProperties.DicePalaceMain.Dice dice = new LevelProperties.DicePalaceMain.Dice(diceMovementSpeed, diceStartPositionOneX, diceStartPositionOneY, diceStartPositionTwoX, diceStartPositionTwoY, diceRollFrameCount, dicePauseWhenRolled, diceRevealDelay);
        LevelProperties.DicePalaceMain.Cards cards = new LevelProperties.DicePalaceMain.Cards(cardSpeed, cardString, cardSideOrder, cardHesitate, cardScale, cardDelay);

        list.Add(new LevelProperties.DicePalaceMain.State(10f, new LevelProperties.DicePalaceMain.Pattern[][]
        {new LevelProperties.DicePalaceMain.Pattern[1]}, LevelProperties.DicePalaceMain.States.Main,
        dice, cards));

        return new LevelProperties.DicePalaceMain(hp, goalTimes, list.ToArray());
    }

    public Level.Timeline KingDiceCreateTimeline(On.LevelProperties.DicePalaceMain.orig_CreateTimeline orig, LevelProperties.DicePalaceMain self, Level.Mode mode)
    {
        Level.Timeline timeline = new Level.Timeline();
        timeline.health = 400f;//750
        return timeline;
    }

    public static LevelProperties.DicePalaceRabbit RabbitGetMode(On.LevelProperties.DicePalaceRabbit.orig_GetMode orig, Level.Mode mode)
    {
        int hp = 400;//850
        Level.GoalTimes goalTimes = null;
        List<LevelProperties.DicePalaceRabbit.State> list = new List<LevelProperties.DicePalaceRabbit.State>();
        goalTimes = new Level.GoalTimes(60f, 60f, 60f);

        LevelProperties.DicePalaceRabbit.Pattern pWand = LevelProperties.DicePalaceRabbit.Pattern.MagicWand;
        LevelProperties.DicePalaceRabbit.Pattern pParry = LevelProperties.DicePalaceRabbit.Pattern.MagicParry;

        float wandSpinningSpeed = 500f;
        float wandBulletSpeed = 500f;//245
        MinMax wandAttackDelayRange = new MinMax(1f, 1f);//MinMax(1.3f, 2.4f);
        float wandCircleDiameter = 1000f;//530
        float wandHesitate = 0.4f;
        string wandSafeZoneString = "1,4,7,8,9,6,3,2,4,6,7,2,9,3,8,1,8,7,3,4,2,8,6,1,9,4,3,8,2,1,9,6,8";
        float wandBulletSize = 2f;
        float wandInitialAttackDelay = 1f;//2f

        MinMax parryAttackDelayRange = new MinMax(1f, 1.5f);//MinMax(1.5f, 2f);
        float parrySpeed = 700f;//465f
        float parryHesitate = 0f;
        string parryPinkString = "1,8,7,2,9,3,6,1,8,2,9,4,7,2,7,3,9,1,4,1,8,2,6,3,1,7,2,9,4,9,1,7,1,8,3,7,9,4,9,2,8,3,9,1,6,2,8,3,1,7,9,1,6";
        float parryInitialAttackDelay = 0.1f;//1
        string parryMagicPositions = "25-135-245-355-465-575-685-795-905";
        float parryYOffset = 100f;

        string platformOnePosition = "-200,-50";
        string platformTwoPosition = "1000,1000";

        LevelProperties.DicePalaceRabbit.MagicWand wand = new LevelProperties.DicePalaceRabbit.MagicWand(wandSpinningSpeed, wandBulletSpeed, wandAttackDelayRange, wandCircleDiameter, wandHesitate, wandSafeZoneString, wandBulletSize, wandInitialAttackDelay);
        LevelProperties.DicePalaceRabbit.MagicParry parry = new LevelProperties.DicePalaceRabbit.MagicParry(parryAttackDelayRange, parrySpeed, parryHesitate, parryPinkString, parryInitialAttackDelay, parryMagicPositions, parryYOffset);
        LevelProperties.DicePalaceRabbit.General general = new LevelProperties.DicePalaceRabbit.General(platformOnePosition, platformTwoPosition);

        list.Add(new LevelProperties.DicePalaceRabbit.State(10f, new LevelProperties.DicePalaceRabbit.Pattern[][]{
        new LevelProperties.DicePalaceRabbit.Pattern[]{pWand, pParry, pWand, pParry, pWand, pParry, pWand, pParry, pWand, pParry, pWand, pParry, pWand, pParry, pWand, pParry, pWand, pParry}
        }, LevelProperties.DicePalaceRabbit.States.Main,
        wand, parry, general));
        /*list.Add(new LevelProperties.DicePalaceRabbit.State(10f, new LevelProperties.DicePalaceRabbit.Pattern[][]{
        new LevelProperties.DicePalaceRabbit.Pattern[]{pWand, pParry, pWand, pWand, pParry, pWand, pWand, pParry,
        pWand, pParry, pWand, pParry, pWand, pWand, pParry, pWand, pWand, pWand, pParry, pWand, pWand, pParry}
        }, LevelProperties.DicePalaceRabbit.States.Main,
        wand, parry, general));*/

        return new LevelProperties.DicePalaceRabbit(hp, goalTimes, list.ToArray());
    }

    public Level.Timeline RabbitCreateTimeline(On.LevelProperties.DicePalaceRabbit.orig_CreateTimeline orig, LevelProperties.DicePalaceRabbit self, Level.Mode mode)
    {
        Level.Timeline timeline = new Level.Timeline();
        timeline.health = 400f;//850
        return timeline;
    }

    public static LevelProperties.DicePalaceRoulette RouletteGetMode(On.LevelProperties.DicePalaceRoulette.orig_GetMode orig, Level.Mode mode)
    {
        int hp = 300;//800
        Level.GoalTimes goalTimes = null;
        List<LevelProperties.DicePalaceRoulette.State> list = new List<LevelProperties.DicePalaceRoulette.State>();
        goalTimes = new Level.GoalTimes(60f, 60f, 60f);

        float platformHeightRow = 0f;
        float platformWidth = 30f;
        float platformCount = 4f;
        float platformOpenDuration = 0.1f;//1.8

        float twirlMovementSpeed = 2137f;//950f
        float twirlMoveDelayRange = 0.0001f;
        float twirlHesitate = 0.8f;
        string[] twirlAmount = new string[]
        {
                "4,5,3,5,4,4,5,3,4,5,4,3"//"5,6,3,6,4,5,6,3,4,7,4,3"
        };
        float twirlScale = 1f;

        float marbleSpeed = 750f;//1200
        string[] marblePositionStrings = new string[]
        {
                //"25-1050,100-1000,150-950,200-900,250-850,300,350-750,400-700,450-650,500-600,550",
                "25-1050,100-1000,150-950,200-900,250-850,300-800,350-750,700,450-650,500-600,550",
                "25-1050,100-1000,150-950,200-900,250-850,300-800,750,400-700,450-650,500-600,550",
                "25-1050,100-1000,150-950,200-900,250-850,300-800,350-750,400,450-650,500-600,550",
                //"25-1050,100-1000,150-950,200-900,250-850,300-800,350-750,400-700,450-650,600,550",
                //"25-1050,100-1000,150-950,200-900,250-850,800,350-750,400-700,450-650,500-600,550",
                //"25-1050,100-1000,150-950,200-900,250-850,300-800,350-750,400-700,450,500-600,550",
                //"25-1050,100-1000,150-950,200-900,250-850,300-800,350-750,400-700,650,500-600,550",
                //"25-1050,100-1000,150-950,200-900,250-850,300-800,350-750,400-700,450,500-600,550",
                //"25-1050,100-1000,150-950,200-900,250-850,300,350-750,400-700,450-650,500-600,550",
                "25-1050,100-1000,150-950,200-900,250-850,300-800,750,400-700,450-650,500-600,550",
                "25-1050,100-1000,150-950,200-900,250-850,300-800,750,400-700,450-650,500-600,550",
                "25-1050,100-1000,150-950,200-900,250-850,300-800,350,400-700,450-650,500-600,550",
                "25-1050,100-1000,150-950,200-900,250-850,300-800,350-750,700,450-650,500-600,550",
                "25-1050,100-1000,150-950,200-900,250-850,300-800,350-750,400-700,650,500-600,550",
                "25-1050,100-1000,150-950,200-900,250-850,300-800,350,400-700,450-650,500-600,550"
        };
        float marbleDelay = 0.30f;//0.24
        float marbleInitalDelay = 1f;
        float marbleHesitate = 2.5f;//1.6f

        LevelProperties.DicePalaceRoulette.Platform platform = new LevelProperties.DicePalaceRoulette.Platform(platformHeightRow, platformWidth, platformCount, platformOpenDuration);
        LevelProperties.DicePalaceRoulette.Twirl twirl = new LevelProperties.DicePalaceRoulette.Twirl(twirlMovementSpeed, twirlMoveDelayRange, twirlHesitate, twirlAmount, twirlScale);
        LevelProperties.DicePalaceRoulette.MarbleDrop marble = new LevelProperties.DicePalaceRoulette.MarbleDrop(marbleSpeed, marblePositionStrings, marbleDelay, marbleInitalDelay, marbleHesitate);

        list.Add(new LevelProperties.DicePalaceRoulette.State(10f, new LevelProperties.DicePalaceRoulette.Pattern[][]
        {
                new LevelProperties.DicePalaceRoulette.Pattern[]
                {
                    LevelProperties.DicePalaceRoulette.Pattern.Twirl,
                    LevelProperties.DicePalaceRoulette.Pattern.Marble
                }
        }, LevelProperties.DicePalaceRoulette.States.Main, platform, twirl, marble));
        return new LevelProperties.DicePalaceRoulette(hp, goalTimes, list.ToArray());
    }

    public Level.Timeline RouletteCreateTimeline(On.LevelProperties.DicePalaceRoulette.orig_CreateTimeline orig, LevelProperties.DicePalaceRoulette self, Level.Mode mode)
    {
        Level.Timeline timeline = new Level.Timeline();
        timeline.health = 300f;//800
        return timeline;
    }

    public static LevelProperties.DicePalaceDomino DominoGetMode(On.LevelProperties.DicePalaceDomino.orig_GetMode orig, Level.Mode mode)
    {
        int hp = 150;//750
        Level.GoalTimes goalTimes = null;
        List<LevelProperties.DicePalaceDomino.State> list = new List<LevelProperties.DicePalaceDomino.State>();
        goalTimes = new Level.GoalTimes(60f, 60f, 60f);

        //LevelProperties.DicePalaceDomino.Pattern pBouncyBall = LevelProperties.DicePalaceDomino.Pattern.BouncyBall;
        LevelProperties.DicePalaceDomino.Pattern pBoomerang = LevelProperties.DicePalaceDomino.Pattern.Boomerang;

        int dominoHP = 150;//500
        float dominoSwingSpeed = 1.3f;
        float dominoSwingDistance = 80f;
        float dominoSwingPosY = 440f;
        float dominoFloorSpeed = 360f;
        string dominoFloorColourString = "R,G,B,Y,R,R,G,B,Y,R,G,B,B,Y,R,G,G,B,Y,R,G,B,Y,Y";
        float dominoFloorTileScale = 1f;
        float dominoSpikesWarningDuration = 2f;
        string dominoMainString = "B,B,B,S,S,B";

        float ballBulletSpeed = 650f;
        string ballAngleString = "71,76,72,70,75,76,73,75,72,74,69";
        string ballUpDownString = "D,U,U,U,D,D,U,D,U,D,D,U,U";
        MinMax ballAttackDelayRange = new MinMax(0.1f, 0.5f);
        string ballProjectileTypeString = "R,R,R,P,R,R,P";
        float ballInitialAttackDelay = 1f;

        float boomerangSpeed = 100f;//400f
        MinMax boomerangAttackDelayRange = new MinMax(0.1f, 0.5f);
        string boomerangTypeString = "R";
        float boomerangInitialAttackDelay = 2f;//1f
        float boomerangHealth = 10f;//20

        LevelProperties.DicePalaceDomino.Domino domino = new LevelProperties.DicePalaceDomino.Domino(dominoHP, dominoSwingSpeed, dominoSwingDistance, dominoSwingPosY, dominoFloorSpeed, dominoFloorColourString, dominoFloorTileScale, dominoSpikesWarningDuration, dominoMainString);
        LevelProperties.DicePalaceDomino.BouncyBall ball = new LevelProperties.DicePalaceDomino.BouncyBall(ballBulletSpeed, ballAngleString, ballUpDownString, ballAttackDelayRange, ballProjectileTypeString, ballInitialAttackDelay);
        LevelProperties.DicePalaceDomino.Boomerang boomerang = new LevelProperties.DicePalaceDomino.Boomerang(boomerangSpeed, ballAttackDelayRange, boomerangTypeString, boomerangInitialAttackDelay, boomerangHealth);

        /*list.Add(new LevelProperties.DicePalaceDomino.State(10f, new LevelProperties.DicePalaceDomino.Pattern[][]
        {new LevelProperties.DicePalaceDomino.Pattern[]{pBouncyBall, pBouncyBall, pBoomerang, pBouncyBall, pBouncyBall, pBouncyBall, pBoomerang}}, LevelProperties.DicePalaceDomino.States.Main,
        domino, ball, boomerang));*/
        list.Add(new LevelProperties.DicePalaceDomino.State(10f, new LevelProperties.DicePalaceDomino.Pattern[][]
        {new LevelProperties.DicePalaceDomino.Pattern[]{pBoomerang}}, LevelProperties.DicePalaceDomino.States.Main,
        domino, ball, boomerang));

        return new LevelProperties.DicePalaceDomino(hp, goalTimes, list.ToArray());
    }

    public Level.Timeline DominoCreateTimeline(On.LevelProperties.DicePalaceDomino.orig_CreateTimeline orig, LevelProperties.DicePalaceDomino self, Level.Mode mode)
    {
        Level.Timeline timeline = new Level.Timeline();
        timeline.health = 150f;//750
        return timeline;
    }

    public static LevelProperties.DicePalaceCigar CigarGetMode(On.LevelProperties.DicePalaceCigar.orig_GetMode orig, Level.Mode mode)
    {
        int hp = 400;//850
        Level.GoalTimes goalTimes = null;
        List<LevelProperties.DicePalaceCigar.State> list = new List<LevelProperties.DicePalaceCigar.State>();
        goalTimes = new Level.GoalTimes(60f, 60f, 60f);

        float cigarWarningDelay = 2f;//0.55
        float cigarPlatformWidthMultiplier = 1f;
        float cigarPlatformHeight = 150f;

        float smokeHorizontalSpeed = 120f;//185
        float smokeCircleSpeed = 2.5f;//3.6
        string smokeRotationDirectionString = "2,2,1,2,2,1,1,1";
        float smokeAttackDelay = 1.4f;
        string smokeAttackCount = "1,0,2,1,2,0,2,2,1";
        float smokeSpiralSmokeCircleSize = 100f;//205
        float smokeHesitateBeforeAttackDelay = 2.5f;//0.5

        float ghostVerticalSpeed = 220f;
        float ghostHorizontalSpeed = 2f;
        string ghostAttackDelayString = "3";
        //"1.8,2.1,1.7,1.9,2.3,1.7,1.7,2.4,1.5,1.9,2.2";
        float ghostHorizontalSpacing = 90f;
        string ghostSpawnPositionString = "0,50,-50,0,25,-25";

        LevelProperties.DicePalaceCigar.Cigar cigar = new LevelProperties.DicePalaceCigar.Cigar(cigarWarningDelay, cigarPlatformWidthMultiplier, cigarPlatformHeight);
        LevelProperties.DicePalaceCigar.SpiralSmoke smoke = new LevelProperties.DicePalaceCigar.SpiralSmoke(smokeHorizontalSpeed, smokeCircleSpeed, smokeRotationDirectionString, smokeAttackDelay, smokeAttackCount, smokeSpiralSmokeCircleSize, smokeHesitateBeforeAttackDelay);
        LevelProperties.DicePalaceCigar.CigaretteGhost ghost = new LevelProperties.DicePalaceCigar.CigaretteGhost(ghostVerticalSpeed, ghostHorizontalSpeed, ghostAttackDelayString, ghostHorizontalSpacing, ghostSpawnPositionString);

        list.Add(new LevelProperties.DicePalaceCigar.State(10f, new LevelProperties.DicePalaceCigar.Pattern[][]
        {
                new LevelProperties.DicePalaceCigar.Pattern[1]
        }, LevelProperties.DicePalaceCigar.States.Main, cigar, smoke, ghost));

        return new LevelProperties.DicePalaceCigar(hp, goalTimes, list.ToArray());
    }

    public Level.Timeline CigarCreateTimeline(On.LevelProperties.DicePalaceCigar.orig_CreateTimeline orig, LevelProperties.DicePalaceCigar self, Level.Mode mode)
    {
        Level.Timeline timeline = new Level.Timeline();
        timeline.health = 400f;//850
        return timeline;
    }

    public static LevelProperties.DicePalaceBooze BoozeGetMode(On.LevelProperties.DicePalaceBooze.orig_GetMode orig, Level.Mode mode)
    {
        int hp = 450;//500
        Level.GoalTimes goalTimes = null;
        List<LevelProperties.DicePalaceBooze.State> list = new List<LevelProperties.DicePalaceBooze.State>();
        goalTimes = new Level.GoalTimes(60f, 60f, 60f);

        float decanterHP = 130f;//265
        float decanterBeamDropSpeed = 730f;
        string decanterAttackDelayString = "2.9,3.5,4.2,3.4,2.8,4.2,3,3.4";
        MinMax decanterBeamAppearDelayRange = new MinMax(0.8f, 1.3f);

        float tumblerHP = 130f;//265
        string tumblerBeamDelayString = "4.6,5,4.4,5.1,4.1,5.5,5.6";
        float tumblerBeamDuration = 0.35f;//0.5
        float tumblerBeamWarningDuration = 0.1f;//0.75

        float martiniHP = 130f;//265
        int martiniOliveHP = 15;//8
        float martiniOliveSpawnDelay = 2f;//3.6
        string martiniMoveString = "2137";//"3,5,4,3,3,4";
        float martiniOliveSpeed = 530f;
        float martiniOliveStopDuration = 0.1f;//1
        string[] martiniOlivePositionStringY = new string[]
        {
                "400,650,375,525",
                "425,500,625",
                "600,475,550,450"
        };
        string[] martiniOlivePositionStringX = new string[]
        {
                "0,500,700,300,725,150,500",
                "550,200,650,50",
                "100,400,600,250,450"
        };
        float martiniBulletSpeed = 50f;//350
        string martiniPinkString = "1,2";
        float martiniOliveHesitateAfterShooting = 0.1f;//1

        float delaySubstractAmount = 0.3f;

        LevelProperties.DicePalaceBooze.Decanter decanter = new LevelProperties.DicePalaceBooze.Decanter(decanterHP, decanterBeamDropSpeed, decanterAttackDelayString, decanterBeamAppearDelayRange);
        LevelProperties.DicePalaceBooze.Tumbler tumbler = new LevelProperties.DicePalaceBooze.Tumbler(tumblerHP, tumblerBeamDelayString, tumblerBeamDuration, tumblerBeamWarningDuration);
        LevelProperties.DicePalaceBooze.Martini martini = new LevelProperties.DicePalaceBooze.Martini(martiniHP, martiniOliveHP, martiniOliveSpawnDelay, martiniMoveString, martiniOliveSpeed, martiniOliveStopDuration, martiniOlivePositionStringY, martiniOlivePositionStringX, martiniBulletSpeed, martiniPinkString, martiniOliveHesitateAfterShooting);
        LevelProperties.DicePalaceBooze.Main main = new LevelProperties.DicePalaceBooze.Main(delaySubstractAmount);

        list.Add(new LevelProperties.DicePalaceBooze.State(10f, new LevelProperties.DicePalaceBooze.Pattern[][]{
                new LevelProperties.DicePalaceBooze.Pattern[1]}, LevelProperties.DicePalaceBooze.States.Main,
            decanter, tumbler, martini, main));

        return new LevelProperties.DicePalaceBooze(hp, goalTimes, list.ToArray());
    }

    public Level.Timeline BoozeCreateTimeline(On.LevelProperties.DicePalaceBooze.orig_CreateTimeline orig, LevelProperties.DicePalaceBooze self, Level.Mode mode)
    {
        Level.Timeline timeline = new Level.Timeline();
        timeline.health = 390f;//500
        return timeline;
    }

    public static LevelProperties.DicePalaceChips ChipsGetMode(On.LevelProperties.DicePalaceChips.orig_GetMode orig, Level.Mode mode)
    {
        int hp = 200;//575
        Level.GoalTimes goalTimes = null;
        List<LevelProperties.DicePalaceChips.State> list = new List<LevelProperties.DicePalaceChips.State>();
        goalTimes = new Level.GoalTimes(60f, 60f, 60f);

        float chipInitialAttackDelay = 0f;//3
        float chipSpeedMultiplier = 5f;//0.8f
        string[] chipAttackString = new string[]
        {
                "1-2-8,3-4-5,6-7",
                "1-2,3-8,4-5-6-7",
                "1-2-3-4,5-6-7-8",
                "2-4-6-1,3-8,5-7",
                "1-7-8,2-3,4-5-6",
                "2-3-8,1-5-6,4-7",
                "3-4-5-6,1-2-7-8",
                "4-5-6-7,1-2-3-8",
                "5-6-7-8,1-2-3-4",
                "2-3-8-1,4-5-6-7",
                "3-4-5,1-2-6-7-8",
                "1-8,3-4-5,2-6-7",
                "4-5-6,1-2-3-7-8",
                "5-6-7-8,1-2-3-4",
                "2-3-4,1-5-6-7-8",
                "3-4-5-6,1-8,2-7",
                "1-2-3-8,4-5-6-7"
        };
        float chipAttackDelay = 0.4f;//0.8
        MinMax chipAttackCycleDelay = new MinMax(0f, 0f);//new MinMax(1.5f, 2.5f);
        float chipSpacing = 1.2f;

        LevelProperties.DicePalaceChips.Chips chips = new LevelProperties.DicePalaceChips.Chips(chipInitialAttackDelay, chipSpeedMultiplier, chipAttackString, chipAttackDelay, chipAttackCycleDelay, chipSpacing);

        list.Add(new LevelProperties.DicePalaceChips.State(10f, new LevelProperties.DicePalaceChips.Pattern[][] { new LevelProperties.DicePalaceChips.Pattern[1] }, LevelProperties.DicePalaceChips.States.Main,
        chips));

        return new LevelProperties.DicePalaceChips(hp, goalTimes, list.ToArray());
    }

    public Level.Timeline ChipsCreateTimeline(On.LevelProperties.DicePalaceChips.orig_CreateTimeline orig, LevelProperties.DicePalaceChips self, Level.Mode mode)
    {
        Level.Timeline timeline = new Level.Timeline();
        timeline.health = 200f;//575
        return timeline;
    }

    public static LevelProperties.DicePalaceEightBall BallGetMode(On.LevelProperties.DicePalaceEightBall.orig_GetMode orig, Level.Mode mode)
    {
        int hp = 350;//750
        Level.GoalTimes goalTimes = null;
        List<LevelProperties.DicePalaceEightBall.State> list = new List<LevelProperties.DicePalaceEightBall.State>();
        goalTimes = new Level.GoalTimes(60f, 60f, 60f);

        float shootSpeed = 550f;
        string[] shootString = new string[] { "R,R" };
        float shootDelay = 1.5f;//3.3f
        int idleLoopAmount = 2;
        float attackDuration = 2f;

        string[] sideString = new string[] { "L,R" };
        float spawnDelay = 1.5f;//2.35
        float oneGroundDelay = 0.1f;
        float oneJumpVerticalSpeed = 2000f;
        float oneJumpHorizontalSpeed = 600f;
        float oneJumpGravity = 7000f;
        float twoGroundDelay = 0.7f;
        float twoJumpVerticalSpeed = 2300f;
        float twoJumpHorizontalSpeed = 800f;
        float twoJumpGravity = 7000f;
        float threeGroundDelay = 0.5f;
        float threeJumpVerticalSpeed = 1900f;
        float threeJumpHorizontalSpeed = 900f;
        float threeJumpGravity = 7000f;
        float fourGroundDelay = 0.4f;
        float fourJumpVerticalSpeed = 2200f;
        float fourJumpHorizontalSpeed = 400f;
        float fourJumpGravity = 7000f;
        float fiveGroundDelay = 0.6f;
        float fiveJumpVerticalSpeed = 2100f;
        float fiveJumpHorizontalSpeed = 800f;
        float fiveJumpGravity = 7000f;

        LevelProperties.DicePalaceEightBall.General general = new LevelProperties.DicePalaceEightBall.General(shootSpeed, shootString, shootDelay, idleLoopAmount, attackDuration);
        LevelProperties.DicePalaceEightBall.PoolBalls balls = new LevelProperties.DicePalaceEightBall.PoolBalls(sideString, spawnDelay, oneGroundDelay, oneJumpVerticalSpeed, oneJumpHorizontalSpeed, oneJumpGravity, twoGroundDelay, twoJumpVerticalSpeed, twoJumpHorizontalSpeed, twoJumpGravity, threeGroundDelay, threeJumpVerticalSpeed, threeJumpHorizontalSpeed, threeJumpGravity, fourGroundDelay, fourJumpVerticalSpeed, fourJumpHorizontalSpeed, fourJumpGravity, fiveGroundDelay, fiveJumpVerticalSpeed, fiveJumpHorizontalSpeed, fiveJumpGravity);

        list.Add(new LevelProperties.DicePalaceEightBall.State(10f, new LevelProperties.DicePalaceEightBall.Pattern[][]
        {new LevelProperties.DicePalaceEightBall.Pattern[1]}, LevelProperties.DicePalaceEightBall.States.Main,
        general, balls));

        return new LevelProperties.DicePalaceEightBall(hp, goalTimes, list.ToArray());
    }

    public Level.Timeline BallCreateTimeline(On.LevelProperties.DicePalaceEightBall.orig_CreateTimeline orig, LevelProperties.DicePalaceEightBall self, Level.Mode mode)
    {
        Level.Timeline timeline = new Level.Timeline();
        timeline.health = 300f;//750
        return timeline;
    }

    public static LevelProperties.DicePalaceFlyingMemory MonkeyGetMode(On.LevelProperties.DicePalaceFlyingMemory.orig_GetMode orig, Level.Mode mode)
    {
        int hp = 2137;//1000
        Level.GoalTimes goalTimes = null;
        List<LevelProperties.DicePalaceFlyingMemory.State> list = new List<LevelProperties.DicePalaceFlyingMemory.State>();
        goalTimes = new Level.GoalTimes(60f, 60f, 60f);

        float botsSpeed = 1f;
        float botsScale = 1f;
        float botsHP = 1f;
        float bulletWarningDuration = 1f;
        float bulletSpeed = 1f;
        string[] botsSpawnOrder = new string[] { "U:2,D:4,L:1,R:3,U:4,L:3" };
        float botsSpawnDelay = 99999f;
        string[] botsMovementString = new string[] { "2,4,1,3" };
        string[] botsDirectionString = new string[] { "N,N,N,P,N,N,N,N,N,P" };
        float bulletDelay = 9999f;
        bool botsOn = false;

        string[] patternOrder = new string[]
        {
                "2B,3A,3B,2A,2A,2B,1A,1A,1B,3B,3A,1B",
                "1B,3A,3A,3B,2A,2B,1B,1A,3B,1A,2B,2A",
                "2A,1A,1B,3B,1B,3A,2A,2B,1A,3B,2B,3A",
                "3A,2A,1A,2B,1A,3B,1B,2A,2B,1B,3B,3A",
                "1A,2B,3B,3A,3B,1A,1B,2A,2A,2B,3A,1B",
                "2A,3B,1A,1A,2B,3A,3B,1B,1B,3A,2A,2B",
                "1B,1A,3B,1B,3A,2A,2B,1A,2B,2A,3A,3B",
                "3A,2A,3A,1A,1A,2B,3B,2A,2B,3B,1B,1B",
                "3B,2B,1B,1A,2B,3A,1B,2A,1A,2A,3A,3B",
                "2B,2A,1A,1A,3B,3A,2B,1B,1B,3B,2A,3A",
                "3B,1B,2B,1B,2A,3A,1A,3B,2B,2A,1A,3A",
                "1A,3B,2B,3B,1B,3A,1B,2A,1A,2A,3A,2B"
        };
        float initialRevealTime = 2.1f;

        string[] monkeyAngleString = new string[]
        {
                "220,45,125,325,37,140,305,55,135,235,40,140"
        };
        string[] monkeyBounceCount = new string[]
        {
                "2,4"
        };
        float monkeyBounceSpeed = 600f;
        float monkeyPunishSpeed = 900f;
        float monkeyPunishTime = 6f;
        float monkeyDirectionChangeDelay = 1.3f;
        float monkeyAttackAnti = 0.5f;//1f
        MinMax monkeyShotDelayRange = new MinMax(0.5f, 1f);//new MinMax(1.4f, 2.8f);
        string[] monkeyShotType = new string[]
        {
                "2"
        };
        float monkeyIncrementSpeedBy = 5f;
        string[] monkeyAngleAdditionString = new string[]
        {
                "5,-3,8,-6,7,-4,8,-6,1,-5,-5"
        };
        float monkeyRegularSpeed = 500f;
        float monkeySpreadSpeed = 50f;//500
        MinMax monkeySpreadAngle = new MinMax(0f, 300f);
        int monkeySpreadBullets = 6;
        float monkeySpiralSpeed = 500f;
        float monkeySpiralMovementRate = 1f;
        float monkeyMusicDeathTimer = 2137f;//0.4

        LevelProperties.DicePalaceFlyingMemory.Bots bots = new LevelProperties.DicePalaceFlyingMemory.Bots(botsSpeed, botsScale, botsHP, bulletWarningDuration, bulletSpeed, botsSpawnOrder, botsSpawnDelay, botsMovementString, botsDirectionString, bulletDelay, botsOn);
        LevelProperties.DicePalaceFlyingMemory.FlippyCard card = new LevelProperties.DicePalaceFlyingMemory.FlippyCard(patternOrder, initialRevealTime);
        LevelProperties.DicePalaceFlyingMemory.StuffedToy monkey = new LevelProperties.DicePalaceFlyingMemory.StuffedToy(monkeyAngleString, monkeyBounceCount, monkeyBounceSpeed, monkeyPunishSpeed, monkeyPunishTime, monkeyDirectionChangeDelay, monkeyAttackAnti, monkeyShotDelayRange, monkeyShotType, monkeyIncrementSpeedBy, monkeyAngleAdditionString, monkeyRegularSpeed, monkeySpreadSpeed, monkeySpreadAngle, monkeySpreadBullets, monkeySpiralSpeed, monkeySpiralMovementRate, monkeyMusicDeathTimer);

        list.Add(new LevelProperties.DicePalaceFlyingMemory.State(10f, new LevelProperties.DicePalaceFlyingMemory.Pattern[][] { new LevelProperties.DicePalaceFlyingMemory.Pattern[1] }, LevelProperties.DicePalaceFlyingMemory.States.Main,
            bots, card, monkey));

        return new LevelProperties.DicePalaceFlyingMemory(hp, goalTimes, list.ToArray());
    }

    public static LevelProperties.DicePalaceFlyingHorse HorseGetMode(On.LevelProperties.DicePalaceFlyingHorse.orig_GetMode orig, Level.Mode mode)
    {
        int hp = 400;//1200
        Level.GoalTimes goalTimes = null;
        List<LevelProperties.DicePalaceFlyingHorse.State> list = new List<LevelProperties.DicePalaceFlyingHorse.State>();
        goalTimes = new Level.GoalTimes(60f, 60f, 60f);

        float giftInitialSpeed = 350f;
        float giftExplosionSpeed = 100f;//200
        float giftExplosionTime = 0f;//1
        MinMax giftPlayerAimRange = new MinMax(2f, 5f);
        string[] giftPositionStringY = new string[]
        {
                "100,360,245,160,360,200,100,345,150,275,360,100,255,180"
        };
        string[] giftPositionStringX = new string[]
        {
                "100,600,350,477,100,285,600,100,550,300,100,480,600"
        };
        string giftSpreadCount = "0,15,30,45,60,75,90,105,120,135,150,165,180,195,210,225,240,255,270,285,300,315,330,345";
        //"0,45,90,135,180,225,270,315"
        float giftDelay = 1.2f;//1.5

        float miniHP = 9999f;
        string[] miniDelayString = new string[]
        {
                "1,1.3,1.4,1,1.5,1.2,1.2,1.3,1.7,1.4,1.3,1.6",
                "1,1.2,1.5,1,,1.4,1.6,1,1.1,1.3,1.4,1.6,1.2,1"
        };
        MinMax miniSpeedRange = new MinMax(270f, 450f);
        string[] miniTypeString = new string[]
        {
                "1,2,1,3,1,2,1,1,3,1,2,3,1,2,3,1,2,2,3,2,2,1,1,3,1,1,2,1,3",
                "1,2,3,1,2,2,1,3,1,2,3,1,2,1,1,3,1,2,3,1,2,2,1,3,1,2,2,3"
        };
        float miniTwoBulletSpeed = 400f;
        float miniThreeJockeySpeed = 600f;
        MinMax miniTwoShotDelayRange = new MinMax(999f, 999f);
        string[] miniThreeProxString = new string[]
        {
                "0,100,50,0,120,0,-50,0,80,-20,0,50"
        };
        string[] miniTwoPinkString = new string[]
        {
                "1,1"
        };

        LevelProperties.DicePalaceFlyingHorse.GiftBombs gift = new LevelProperties.DicePalaceFlyingHorse.GiftBombs(giftInitialSpeed, giftExplosionSpeed, giftExplosionTime, giftPlayerAimRange, giftPositionStringY, giftPositionStringX, giftSpreadCount, giftDelay);
        LevelProperties.DicePalaceFlyingHorse.MiniHorses mini = new LevelProperties.DicePalaceFlyingHorse.MiniHorses(miniHP, miniDelayString, miniSpeedRange, miniTypeString, miniTwoBulletSpeed, miniThreeJockeySpeed, miniTwoShotDelayRange, miniThreeProxString, miniTwoPinkString);

        list.Add(new LevelProperties.DicePalaceFlyingHorse.State(10f, new LevelProperties.DicePalaceFlyingHorse.Pattern[][]
        {
                new LevelProperties.DicePalaceFlyingHorse.Pattern[1]
        }, LevelProperties.DicePalaceFlyingHorse.States.Main, gift, mini));

        return new LevelProperties.DicePalaceFlyingHorse(hp, goalTimes, list.ToArray());
    }

    public Level.Timeline HorseCreateTimeline(On.LevelProperties.DicePalaceFlyingHorse.orig_CreateTimeline orig, LevelProperties.DicePalaceFlyingHorse self, Level.Mode mode)
    {
        Level.Timeline timeline = new Level.Timeline();
        timeline.health = 400f;//1200
        return timeline;
    }

    public static LevelProperties.Devil DevilGetMode(On.LevelProperties.Devil.orig_GetMode orig, Level.Mode mode)
    {
        int hp = 2300;//2100
        Level.GoalTimes goalTimes = null;
        List<LevelProperties.Devil.State> list = new List<LevelProperties.Devil.State>();
        goalTimes = new Level.GoalTimes(180f, 180f, 180f);

        LevelProperties.Devil.Pattern pHead = LevelProperties.Devil.Pattern.Head;
        LevelProperties.Devil.Pattern pClap = LevelProperties.Devil.Pattern.Clap;
        //LevelProperties.Devil.Pattern pPitchfork = LevelProperties.Devil.Pattern.Pitchfork;
        LevelProperties.Devil.Pattern pBombEye = LevelProperties.Devil.Pattern.BombEye;
        //LevelProperties.Devil.Pattern pSkullEye = LevelProperties.Devil.Pattern.SkullEye;

        MinMax wallXRange = new MinMax(-350f, 350f);
        MinMax wallSpeed = new MinMax(315f, 345f);
        MinMax wallHesitateAfterAttack = new MinMax(1.8f, 2.2f);

        MinMax splitProjNumProjectiles = new MinMax(3f, 7f);
        MinMax splitProjDelayBetweenProjectiles = new MinMax(0.4f, 0.5f);
        float splitProjectileSpeed = 606f;
        MinMax splitProjHesitateAfterAttack = new MinMax(0.5f, 1.5f);

        float demonHp = 15f;//5
        float demonSpeed = 575f;
        float demonDelay = 3.5f;

        MinMax clapDelay = new MinMax(0.1f, 0.5f);
        float clapWarning = 0.5f;
        float clapSpeed = 0.2f;
        float clapHesitate = 1.5f;

        float spiderDownSpeed = 700f;//950
        float spiderUpSpeed = 1500f;//1150
        string spiderPositionOffset = "-150, 50, -50, 300, -200, 50, 150, -300, 0, 100, -50, 200, 50, 0, 100, -150, 50, -250, 200, 0";
        MinMax spiderNumAttacks = new MinMax(3f, 6f);
        MinMax spiderEntranceDelay = new MinMax(0.3f, 0.7f);
        float spiderHesitate = 1.5f;

        float dragonSpeed = 1f;
        float dragonSinHeight = 300f;
        float dragonSinSpeed = 6f;
        string dragonPositionOffset = "0, 150, 50, 200, 0, 100, 200, 50";
        float dragonReturnSpeed = 1600f;
        float dragonReturnDelay = 0.5f;
        float dragonHesitate = 1.5f;

        string[] forkPatternString = new string[]
        {
                /*"4, 5, 6",
                "6, 5, 4, 5",
                "4, 6, 5"*/
                "4, 6"
        };
        float forkSpawnCenterY = 50f;
        float forkSpawnRadius = 300f;
        float forkDormantDuration = 1f;

        string forkBouncerAngleOffset = "55, 30, 35, 60, 40, 70, 20, 35, 50, 100, 200, 35, 60, 35, 55, 100, 70, 20, 30, 50, 40, 200";
        MinMax forkBouncerInitialAttackDelay = new MinMax(1f, 1.5f);
        float forkBouncerSpeed = 750f;//885
        int forkBouncerNumBounces = 6;
        float forkBouncerHesitate = 1.5f;

        string forkSpinnerAngleOffset = "0, 10, -10, 0, 5, -5, -20, 20, 0, 30, -30";
        float forkSpinnerRotationSpeed = 115f;
        float forkSpinnerMaxSpeed = 275f;
        float forkSpinnerAcceleration = 415f;
        float forkSpinnerAttackDuration = 2137f;//5.5
        float forkSpinnerHesitate = 1.5f;

        string forkRingAngleOffset = "0, 10, -10, 25, 5, 30, -25, 15, 35, 20, -20";
        MinMax forkRingInitialAttackDelay = new MinMax(1f, 1.5f);
        float forkRingAttackDelay = 0.55f;
        float forkRingSpeed = 635f;
        float forkRingGroundDuration = 0f;
        float forkRingHesitate = 1.5f;

        float platformExitSpeed = 185f;
        float platformRiseSpeed = 125f;
        string platformRiseString = "1,3,2,5,4,1,5,4,2,3,1,3,5,4,2";
        float platformMaxHeight = 150f;
        float platformHoldDelay = 1f;
        MinMax platformRiseDelayRange = new MinMax(1.5f, 2.8f);
        float platformSize = 200f;
        bool platformRiseDuringTearPhase = false;

        float fireballInitialDelay = 2f;
        float fireballFallSpeed = 500f;//240
        float fireballFallAcceleration = 0f;//550
        float fireballSpawnDelay = 1f;//2.8
        float fireballSpawnDelayTears = 2f;
        float fireballSize = 220f;

        float bombXSinHeight = 200f;
        float bombYSinHeight = 100f;
        float bombXSinSpeed = 2f;
        float bombYSinSpeed = 5f;
        float bombExplodeDelay = 5f;//1.6
        MinMax bombHesitate = new MinMax(0f, 1f);//new MinMax(6f, 7.5f);

        float axeInitialMoveDuration = 0f;//0.75
        float axeInitialMoveSpeed = 335f;
        float axeSwirlMoveOutwardSpeed = 180f;
        float axeSwirlRotationSpeed = 20f;//216
        MinMax axeHesitate = new MinMax(4.8f, 6f);

        float handsHP = 2137f;//50
        MinMax handsYRange = new MinMax(-250f, 50f);//new MinMax(-180f, 100f);
        float handsSpeed = 175f;
        MinMax handsShotDelay = new MinMax(1f, 2f);//new MinMax(2.5f, 3.8f);
        float handsBulletSpeed = 435f;
        MinMax handsInitialSpawnDelay = new MinMax(1f, 2f);
        MinMax handsSpawnDelayRange = new MinMax(6f, 7.3f);
        string handsPinkString = "P";//"R,R,P"

        string swooperPositions = "0,300,600,200,800,1000,500,1100,100,900,400,700,1200";
        MinMax swooperSpawnCount = new MinMax(5f, 8f);//new MinMax(3f, 4f);
        int swooperMaxCount = 15;//7
        float swooperHp = 25f;//3.5
        MinMax swooperAttackDelay = new MinMax(0f, 0f);//new MinMax(1.8f, 2.9f);
        MinMax swooperLaunchAngle = new MinMax(45f, 90f);
        MinMax swooperLaunchSpeed = new MinMax(1200f, 800f);//new MinMax(1200f, 800f);
        float swooperGravity = 30f;//1000
        MinMax swooperInitialSpawnDelay = new MinMax(2f, 3f);
        MinMax swooperSpawnDelay = new MinMax(0f, 0f);//new MinMax(4.5f, 6f);
        MinMax swooperYIdlePos = new MinMax(320f, 350f);//new MinMax(220f, 300f);

        float tearsSpeed = 280f;//485
        float tearsDelay = 7f;//0.4f

        float firewallSpeed = 500f;//300

        LevelProperties.Devil.SplitDevilWall wall = new LevelProperties.Devil.SplitDevilWall(wallXRange, wallSpeed, wallHesitateAfterAttack);
        LevelProperties.Devil.SplitDevilProjectiles splitProj = new LevelProperties.Devil.SplitDevilProjectiles(splitProjNumProjectiles, splitProjDelayBetweenProjectiles, splitProjectileSpeed, splitProjHesitateAfterAttack);
        LevelProperties.Devil.Demons demon = new LevelProperties.Devil.Demons(demonHp, demonSpeed, demonDelay);
        LevelProperties.Devil.Clap clap = new LevelProperties.Devil.Clap(clapDelay, clapWarning, clapSpeed, clapHesitate);
        LevelProperties.Devil.Spider spider = new LevelProperties.Devil.Spider(spiderDownSpeed, spiderUpSpeed, spiderPositionOffset, spiderNumAttacks, spiderEntranceDelay, spiderHesitate);
        LevelProperties.Devil.Dragon dragon = new LevelProperties.Devil.Dragon(dragonSpeed, dragonSinHeight, dragonSinSpeed, dragonPositionOffset, dragonReturnSpeed, dragonReturnDelay, dragonHesitate);
        LevelProperties.Devil.Pitchfork fork = new LevelProperties.Devil.Pitchfork(forkPatternString, forkSpawnCenterY, forkSpawnRadius, forkDormantDuration);
        LevelProperties.Devil.PitchforkTwoFlameWheel forkWheel = new LevelProperties.Devil.PitchforkTwoFlameWheel(string.Empty, 0f, 0f, new MinMax(0f, 1f), new MinMax(0f, 1f), 0f);
        LevelProperties.Devil.PitchforkThreeFlameJumper forkJumper = new LevelProperties.Devil.PitchforkThreeFlameJumper(string.Empty, new MinMax(0f, 1f), new MinMax(0f, 1f), 0f, new MinMax(0f, 1f), 0f, 0, 0f);
        LevelProperties.Devil.PitchforkFourFlameBouncer forkBouncer = new LevelProperties.Devil.PitchforkFourFlameBouncer(forkBouncerAngleOffset, forkBouncerInitialAttackDelay, forkBouncerSpeed, forkBouncerNumBounces, forkBouncerHesitate);
        LevelProperties.Devil.PitchforkFiveFlameSpinner forkSpinner = new LevelProperties.Devil.PitchforkFiveFlameSpinner(forkSpinnerAngleOffset, forkSpinnerRotationSpeed, forkSpinnerMaxSpeed, forkSpinnerAcceleration, forkSpinnerAttackDuration, forkSpinnerHesitate);
        LevelProperties.Devil.PitchforkSixFlameRing forkRing = new LevelProperties.Devil.PitchforkSixFlameRing(forkRingAngleOffset, forkRingInitialAttackDelay, forkRingAttackDelay, forkRingSpeed, forkRingGroundDuration, forkRingHesitate);
        LevelProperties.Devil.GiantHeadPlatforms platform = new LevelProperties.Devil.GiantHeadPlatforms(platformExitSpeed, platformRiseSpeed, platformRiseString, platformMaxHeight, platformHoldDelay, platformRiseDelayRange, platformSize, platformRiseDuringTearPhase);
        LevelProperties.Devil.Fireballs fireball = new LevelProperties.Devil.Fireballs(fireballInitialDelay, fireballFallSpeed, fireballFallAcceleration, fireballSpawnDelay, fireballSize);
        LevelProperties.Devil.Fireballs fireballOff = new LevelProperties.Devil.Fireballs(2137f, 0f, 0f, 2137f, 0f);
        LevelProperties.Devil.Fireballs fireballTears = new LevelProperties.Devil.Fireballs(fireballInitialDelay, fireballFallSpeed, fireballFallAcceleration, fireballSpawnDelayTears, fireballSize);
        LevelProperties.Devil.BombEye bomb = new LevelProperties.Devil.BombEye(bombXSinHeight, bombYSinHeight, bombXSinSpeed, bombYSinSpeed, bombExplodeDelay, bombHesitate);
        LevelProperties.Devil.SkullEye axe = new LevelProperties.Devil.SkullEye(axeInitialMoveDuration, axeInitialMoveSpeed, axeSwirlMoveOutwardSpeed, axeSwirlRotationSpeed, axeHesitate);
        LevelProperties.Devil.Hands hands = new LevelProperties.Devil.Hands(handsHP, handsYRange, handsSpeed, handsShotDelay, handsBulletSpeed, handsInitialSpawnDelay, handsSpawnDelayRange, handsPinkString);
        LevelProperties.Devil.Swoopers swooper = new LevelProperties.Devil.Swoopers(swooperPositions, swooperSpawnCount, swooperMaxCount, swooperHp, swooperAttackDelay, swooperLaunchAngle, swooperLaunchSpeed, swooperGravity, handsInitialSpawnDelay, swooperSpawnDelay, swooperYIdlePos);
        LevelProperties.Devil.Tears tears = new LevelProperties.Devil.Tears(tearsSpeed, tearsDelay);
        LevelProperties.Devil.Firewall firewall = new LevelProperties.Devil.Firewall(firewallSpeed);

        list.Add(new LevelProperties.Devil.State(10f, new LevelProperties.Devil.Pattern[][]{new
            LevelProperties.Devil.Pattern[]{pHead, pClap, pHead, pHead, pClap, pHead, pClap, pHead, pHead}},
        LevelProperties.Devil.States.Main, wall, splitProj, demon, clap, spider, dragon, fork, forkWheel,
        forkJumper, forkBouncer, forkSpinner, forkRing, platform, fireball, bomb, axe, hands, swooper,
        tears, firewall));
        /*list.Add(new LevelProperties.Devil.State(10f, new LevelProperties.Devil.Pattern[][]{new
            LevelProperties.Devil.Pattern[]{pHead, pClap, pPitchfork, pClap, pHead, pClap, pPitchfork}},
        LevelProperties.Devil.States.Main, wall, splitProj, demon, clap, spider, dragon, fork, forkWheel,
        forkJumper, forkBouncer, forkSpinner, forkRing, platform, fireball, bomb, axe, hands, swooper,
        tears, firewall));*/

        //0.65
        list.Add(new LevelProperties.Devil.State(0.72f, new LevelProperties.Devil.Pattern[][]{
            new LevelProperties.Devil.Pattern[]{pBombEye}}, LevelProperties.Devil.States.GiantHead, wall, splitProj, demon, clap, spider, dragon, fork, forkWheel, forkJumper, forkBouncer, forkSpinner, forkRing, platform, fireball, bomb, axe, hands, swooper, tears, firewall));
        /*list.Add(new LevelProperties.Devil.State(0.65f, new LevelProperties.Devil.Pattern[][]{
            new LevelProperties.Devil.Pattern[]{pBombEye, pSkullEye, pBombEye, pBombEye, pSkullEye, pBombEye,
            pSkullEye, pSkullEye, pBombEye, pSkullEye, pBombEye, pBombEye, pSkullEye, pBombEye, pSkullEye,
            pBombEye, pSkullEye, pSkullEye}}, LevelProperties.Devil.States.GiantHead, wall, splitProj, demon,
        clap, spider, dragon, fork, forkWheel, forkJumper, forkBouncer, forkSpinner, forkRing, platform,
        fireball, bomb, axe, hands, swooper, tears, firewall));*/

        //0.35
        list.Add(new LevelProperties.Devil.State(0.44f, new LevelProperties.Devil.Pattern[][]{
            new LevelProperties.Devil.Pattern[0]}, LevelProperties.Devil.States.Hands, wall, splitProj,
        demon, clap, spider, dragon, fork, forkWheel, forkJumper, forkBouncer, forkSpinner, forkRing,
        platform, fireballOff, bomb, axe, hands, swooper, tears, firewall));

        //0.1
        list.Add(new LevelProperties.Devil.State(0.22f, new LevelProperties.Devil.Pattern[][]{
            new LevelProperties.Devil.Pattern[0]}, LevelProperties.Devil.States.Tears, wall, splitProj,
        demon, clap, spider, dragon, fork, forkWheel, forkJumper, forkBouncer, forkSpinner, forkRing,
        platform, fireballTears, bomb, axe, hands, swooper, tears, firewall));

        return new LevelProperties.Devil(hp, goalTimes, list.ToArray());
    }

    public Level.Timeline DevilCreateTimeline(On.LevelProperties.Devil.orig_CreateTimeline orig, LevelProperties.Devil self, Level.Mode mode)
    {
        Level.Timeline timeline = new Level.Timeline();
        timeline.health = 2300f;//2100
        timeline.events.Add(new Level.Timeline.Event("GiantHead", 0.72f));
        timeline.events.Add(new Level.Timeline.Event("Hands", 0.44f));
        timeline.events.Add(new Level.Timeline.Event("Tears", 0.22f));
        /*timeline.events.Add(new Level.Timeline.Event("GiantHead", 0.65f));
        timeline.events.Add(new Level.Timeline.Event("Hands", 0.35f));
        timeline.events.Add(new Level.Timeline.Event("Tears", 0.1f));*/
        //1 - 2100 * 0.35 = 735
        //2 - 2100 * 0.30 = 630
        //3 - 2100 * 0.25 = 525
        //4 - 2100 * 0.1 = 210
        //new: 650 + 650 + 500 + 500 = 2300
        //1 - 650 / 2300 = 0.28
        //2 - 650 / 2300 = 0.28
        //3 - 500 / 2300 = 0.22
        //4 - 500 / 2300 = 0.22
        return timeline;
    }

    public static LevelProperties.FlyingCowboy CowGetMode(On.LevelProperties.FlyingCowboy.orig_GetMode orig, Level.Mode mode)
    {
        int hp = 3200;
        Level.GoalTimes goalTimes = null;
        List<LevelProperties.FlyingCowboy.State> list = new List<LevelProperties.FlyingCowboy.State>();
        goalTimes = new Level.GoalTimes(120f, 120f, 120f);

        string[] cartAttackString = new string[]{
            "S,B,M,S,M,S,M,B,S,M,S,M,B,S,M",
            "B,M,S,M,B,M,S,M,S,B,M"};
        float cartMoveSpeed = 550f;
        float cartPopinTime = 0.25f;

        float snakeBreakLinePosition = 65f;
        string[] snakeOffsetString = new string[] { "15,10,0,-10,0,15,0,-10,5,-15,5" };
        string[] snakeWidthString = new string[]{
            "200,150,100,150",
            "130,190,120,160",
            "120,180,130,170",
            "160,190,140,110"};
        float snakeAttackDelay = 1.1f;
        float snakeSpeed = 1005f;
        string[] snakeShotsPerAttack = new string[]{
            "1,3,2,2,3,1,2,3,2",
            "3,1,2,2,1,3,2,1,2,2"};
        float snakeAttackRecovery = 0.6f;
        float snakeTimeToApex = 0.55f;
        float snakeApexHeight = 250f;

        float beamWarningTime = 0f;
        float beamDuration = 0f;
        float beamAttackRecovery = 0f;

        float UFOHealth = 2137;
        float UFOIntroSpeed = 500;
        float UFOtopSpeed = 200;
        float UFOtopVerticalPosition = 300f;
        string[] UFOtopShootString = new string[] { "2.1, 3.7, 1.69" };
        float UFOPathLength = 1280f;
        float UFOtopRespawnDelay = 2f;
        int UFObulletCount = 3;
        float UFOspreadAngle = 55f;
        float UFObulletSpeed = 100f;
        string UFObulletParryString = "R,R,R,R,R,R,P";

        float backshotSpeed = 555f;
        float backshotHealth = 6f;
        float backshotBulletSpeed = 495f;
        string[] backshotHighSpawnPosition = new string[]{
            "200,100,150",
            "200,150,100"};
        string[] backshotLowSpawnPosition = new string[]{
            "-200,-100,-150",
            "-200,-150,-100"};
        string[] backshotSpawnDelay = new string[]{
            "0.5,1.2,1.7,0.8,1.4,1.9",
            "0.9,1.5,0.8,1.9,0.6,1.6"};
        string backshotBulletParryString = "N,N,P,N,N,N,P";
        string[] backshotAnticipationStartDistance = new string[] { "200,1000" };

        MinMax debrisWarningDelayRange = new MinMax(1.7f, 1.8f);
        MinMax debrisOneSpeedStartEnd = new MinMax(365f, 680f);
        MinMax debrisTwoSpeedStartEnd = new MinMax(405f, 780f);
        MinMax debrisThreeSpeedStartEnd = new MinMax(475f, 880f);
        float debrisSpeedUpDistance = 700f;
        string[] debrisSideSpawn = new string[]{
            "3,1,2,5,4,2,3",
            "2,3,4,5,2,3,1",
            "0,2,4,3,1,4,3",
            "4,1,2,3,4,2,0",
            "3,2,5,3,4,1,2",
            "2,4,1,0,2,1,3",
            "0,3,2,4,5,2,4"};
        string[] debrisTopSpawn = new string[]{
            "0,5,4,3,5,2,4",
            "1,3,0,4,5,2",
            "3,0,5,4,1,3,5",
            "5,2,3,0,4,1",
            "4,1,2,5,3,1,4",
            "0,5,3,4,2,3",
            "2,4,0,3,5,4,1"};
        string[] debrisBottomSpawn = new string[]{
            "5,3,2,4,0,3,1",
            "2,0,5,1,0,5",
            "0,2,5,1,4,0,2",
            "4,2,3,1,5,2",
            "5,3,0,4,1,2,5",
            "3,1,5,0,4,3",
            "2,0,5,4,1,3,4"};
        string debrisTypeString = "1,2,3,2,1,3,2,2,3,1,2,3,2,1,3,2,3,2,1,2,3,3";
        float debrisDelay = 0.51f;
        float debrisHesitate = 0.3f;
        string[] debrisCurveShotString = new string[] { "B:0,T:1,T:3,B:2,B:3,B:3,T:2,T:0" };
        float debrisCurveApexTime = 1.2f;
        float debrisVacuumWindStrength = -605f;
        float debrisVacuumTimeToFullStrength = 2.1f;
        string debrisParryString = "N,N,N";
        string[] debrisTransitionSideSpawn = new string[]{
            "5,3,2,0",
            "4,3,1,5"};
        string[] debrisTransitionTopSpawn = new string[]{
            "3,2,1,5",
            "0,4,5,1"};
        string[] debrisTransitionBottomSpawn = new string[]{
            "0,2,1,5",
            "5,0,3,1"};
        string[] debrisTransitionCurveShotString = new string[] { "B:0,T:1,T:3,B:2,B:3,B:3,T:2,T:0" };

        string[] sausageStringA = new string[]{
            "3,2,3,2,4",
            "2,3,4,2,2"};
        string[] sausageStringB = new string[]{
            "4,2,2,3,3,1",
            "2,4,3,2,1,3"};
        string[] sausageGapDistA = new string[]{
            "1,2,1,2,3,1,1,3",
            "2,2,1,3,1,2,1"};
        string[] sausageGapDistB = new string[]{
            "3,2,1,2,1,2",
            "1,1,3,1,2"};
        float sausageTrainSpeed = 460f;
        float sausageSweepSpeed = 6f;
        float sausageMaxAngle = 45f;
        bool sausageShootBullets = true;
        float sausageShotDelay = 0.215f;
        string[] sausageBulletCount = new string[] { "5,6" };
        float sausageBulletSpeed = 365f;
        float sausageBulletSpreadAngle = 115f;
        string sausageBulletParryString = "N,N,N,N,N,P,N,N,N,N,N,N,P";
        float sausageBeanCanTriggerTime = 1f;
        MinMax sausageBeanCanSpeed = new MinMax(400f, 550f);
        string[] sausageBeanCanPostionUpper = new string[] { "300:D,200:U" };
        string[] sausageBeanCanPositionLower = new string[] { "-225:U,-175:D" };
        string sausageBeanCanExtendTimer = "0,0.4,0.8";
        float sausageWobbleRadiusX = 75f;
        float sausageWobbleRadiusY = 40f;
        float sausageWobbleDurationX = 2.3f;
        float sausageWobbleDurationY = 1.5f;

        float srMirrorTime = 0.5f;
        string[] srTimeTillSwitch = new string[]
        {
            "2.3,5,4.5"
        };
        MinMax srBeansSpeed = new MinMax(550f, 750f);
        string[] srBeansPositionString = new string[]{
"-250:U,200:D,0:U,-110:U,50:D,-180:U,210:D,0:U,285:D,-50:U,100:U,0:D,200:U,-50:D,190:D,-100:U,-250:U,100:D,-180:U,-120:D",
"285:D,110:D,200:U,-150:D,-50:U,150:D,-250:U,-110:U,170:U,80:D,-10:D,-200:U,80:D,210:D,40:D,-180:U,-60:U,100:D",
"110:D,240:D,-110:U,0:D,100:U,-100:U,20:D,160:U,-60:D,285:D,90:U,-120:D,210:D,40:U,285:D,150:U,-260:U,-80:D"
        };
        MinMax srBeansSpawnDelay = new MinMax(0.9f, 1.2f);
        string[] srGroupBeansDelayString = new string[]
        {
            "1.5,2"
        };
        string srBeansExtendTimer = "0.5,0.7,0.1,0.8,0.6,0.4,0.3,0.6,1,0.1";
        bool srShootBullets = true;
        float srBulletDelay = 2.8f;
        float srBulletSpeed = 265f;
        float srBulletRotationSpeed = 305f;
        float srBulletRotationRadius = 135f;
        float srBulletTopMaxUpAngle = 0f;
        float srBulletTopMaxDownAngle = 45f;
        bool srBulletTopRotateClockwise = false;
        float srBulletBottomMaxUpAngle = 45f;
        float srBulletBottomMaxDownAngle = 0f;
        bool srBulletBottomRotateClockwise = true;
        string srBulletParry = "N,N,N,P";

        bool ricOn = true;
        string ricRainDelayString = "1,1,0.6,1,0.8,0.6,1,0.6,1,0.6,0.7,1,0.7,0.8,1,0.6";
        string ricRainSpeedString = "550,500,505,515";
        string ricRainTypeString = "R,R,R,R";
        string ricRainSpawnString = "400,100,600,-150,300,100,500,0,200,-100,450,50,250,550,300,50,-150,600,400,-100,350,0,550,-50,450,0,600,100,350,-150,250,550,300,50";
        float ricRainDuration = 5.1f;
        float ricRainRecoveryTime = 0.3f;
        int ricSplitBulletCount = 0;
        float ricSplitSpreadAngle = 0f;
        float ricSplitBulletSpeed = 525f;
        string ricSplitParryString = "N,N,N,N,N,N,P";
        MinMax ricCoinCountRange = new MinMax(1f, 3f);
        MinMax ricCoinHeightRange = new MinMax(50f, 150f);
        float ricCoinGravity = 1200f;
        MinMax ricCoinSpeedXRange = new MinMax(-375f, 250f);

        MinMax birdSpawnDelayRange = new MinMax(3.5f, 5.5f);
        float birdSpeed = 495f;
        float birdBulletArcHeight = 70f;
        float birdBulletGravity = 855f;
        string[] birdBulletLandingPosition = new string[]
        {
            "-400,-250,-350,-450,-200,-150,-450,-250",
            "-450,-300,-500,-250,-350,-450,-150,-250",
            "-400,-150,-300,-500,-200,-450,-350,-250",
            "-500,-300,-450,-200,-350,-500,-150,-250"
        };
        float birdShrapnelSpeed = 535f;
        int birdShrapnelCount = 7;
        float birdShrapnelSpreadAngle = 125f;
        float birdShrapnelSecondStageDelay = 0.65f;
        float birdSafetyZoneMaxDuration = 1.5f;

        LevelProperties.FlyingCowboy.Cart cart = new LevelProperties.FlyingCowboy.Cart(cartAttackString, cartMoveSpeed, cartPopinTime);
        LevelProperties.FlyingCowboy.SnakeAttack snake = new LevelProperties.FlyingCowboy.SnakeAttack(snakeBreakLinePosition, snakeOffsetString, snakeWidthString, snakeAttackDelay, snakeSpeed, snakeShotsPerAttack, snakeAttackRecovery, snakeTimeToApex, snakeApexHeight);
        LevelProperties.FlyingCowboy.BeamAttack beam = new LevelProperties.FlyingCowboy.BeamAttack(beamWarningTime, beamDuration, beamAttackRecovery);
        LevelProperties.FlyingCowboy.UFOEnemy ufo = new LevelProperties.FlyingCowboy.UFOEnemy(UFOHealth, UFOIntroSpeed, UFOtopSpeed, UFOtopVerticalPosition, UFOtopShootString, UFOPathLength, UFOtopRespawnDelay, UFObulletCount, UFOspreadAngle, UFObulletSpeed, UFObulletParryString);
        LevelProperties.FlyingCowboy.BackshotEnemy backshot = new LevelProperties.FlyingCowboy.BackshotEnemy(backshotSpeed, backshotHealth, backshotBulletSpeed, backshotHighSpawnPosition, backshotLowSpawnPosition, backshotSpawnDelay, backshotBulletParryString, backshotAnticipationStartDistance);
        LevelProperties.FlyingCowboy.Debris debris = new LevelProperties.FlyingCowboy.Debris(debrisWarningDelayRange, debrisOneSpeedStartEnd, debrisTwoSpeedStartEnd, debrisThreeSpeedStartEnd, debrisSpeedUpDistance, debrisSideSpawn, debrisTopSpawn, debrisBottomSpawn, debrisTypeString, debrisDelay, debrisHesitate, debrisCurveShotString, debrisCurveApexTime, debrisVacuumWindStrength, debrisVacuumTimeToFullStrength, debrisParryString, debrisTransitionSideSpawn, debrisTopSpawn, debrisTransitionBottomSpawn, debrisTransitionCurveShotString);
        LevelProperties.FlyingCowboy.Can can = new LevelProperties.FlyingCowboy.Can(sausageStringA, sausageStringB, sausageGapDistA, sausageGapDistB, sausageTrainSpeed, sausageSweepSpeed, sausageMaxAngle, sausageShootBullets, sausageShotDelay, sausageBulletCount, sausageBulletSpeed, sausageBulletSpreadAngle, sausageBulletParryString, sausageBeanCanTriggerTime, sausageBeanCanSpeed, sausageBeanCanPostionUpper, sausageBeanCanPositionLower, sausageBeanCanExtendTimer, sausageWobbleRadiusX, sausageWobbleRadiusY, sausageWobbleDurationX, sausageWobbleDurationY);
        LevelProperties.FlyingCowboy.SausageRun sausageRun = new LevelProperties.FlyingCowboy.SausageRun(srMirrorTime, srTimeTillSwitch, srBeansSpeed, srBeansPositionString, srBeansSpawnDelay, srGroupBeansDelayString, srBeansExtendTimer, srShootBullets, srBulletDelay, srBulletSpeed, srBulletRotationSpeed, srBulletRotationRadius, srBulletTopMaxUpAngle, srBulletTopMaxDownAngle, srBulletTopRotateClockwise, srBulletBottomMaxUpAngle, srBulletBottomMaxDownAngle, srBulletBottomRotateClockwise, srBulletParry);
        LevelProperties.FlyingCowboy.Ricochet ricochet = new LevelProperties.FlyingCowboy.Ricochet(ricOn, ricRainDelayString, ricRainSpeedString, ricRainTypeString, ricRainSpawnString, ricRainDuration, ricRainRecoveryTime, ricSplitBulletCount, ricSplitSpreadAngle, ricSplitBulletSpeed, ricSplitParryString, ricCoinCountRange, ricCoinHeightRange, ricCoinGravity, ricCoinSpeedXRange);
        LevelProperties.FlyingCowboy.Bird bird = new LevelProperties.FlyingCowboy.Bird(birdSpawnDelayRange, birdSpeed, birdBulletArcHeight, birdBulletGravity, birdBulletLandingPosition, birdShrapnelSpeed, birdShrapnelCount, birdShrapnelSpreadAngle, birdShrapnelSecondStageDelay, birdSafetyZoneMaxDuration);

        // First Phase
        list.Add(new LevelProperties.FlyingCowboy.State(10f, new LevelProperties.FlyingCowboy.Pattern[][]{
            new LevelProperties.FlyingCowboy.Pattern[1]}, LevelProperties.FlyingCowboy.States.Main,
            cart, snake, beam, ufo, backshot, debris, can, sausageRun, ricochet, bird));

        // Vacuum
        list.Add(new LevelProperties.FlyingCowboy.State(0.82f, new LevelProperties.FlyingCowboy.Pattern[][]{
            new LevelProperties.FlyingCowboy.Pattern[]{LevelProperties.FlyingCowboy.Pattern.Ricochet,
            LevelProperties.FlyingCowboy.Pattern.Vacuum}}, LevelProperties.FlyingCowboy.States.Vacuum,
            cart, snake, beam, ufo, backshot, debris, can, sausageRun, ricochet, bird));

        // Sausage run
        list.Add(new LevelProperties.FlyingCowboy.State(0.52f, new LevelProperties.FlyingCowboy.Pattern[][]{
            new LevelProperties.FlyingCowboy.Pattern[0]}, LevelProperties.FlyingCowboy.States.Meatball,
            cart, snake, beam, ufo, backshot, debris, can, sausageRun, ricochet, bird));

        // Sausage can
        list.Add(new LevelProperties.FlyingCowboy.State(0.27f, new LevelProperties.FlyingCowboy.Pattern[][]{
            new LevelProperties.FlyingCowboy.Pattern[0]}, LevelProperties.FlyingCowboy.States.Sausage,
            cart, snake, beam, ufo, backshot, debris, can, sausageRun, ricochet, bird));

        return new LevelProperties.FlyingCowboy(hp, goalTimes, list.ToArray());
    }

    public static LevelProperties.Graveyard GraveyardGetMode(On.LevelProperties.Graveyard.orig_GetMode orig, Level.Mode mode)
    {
        int hp = 800;
        Level.GoalTimes goalTimes = null;
        List<LevelProperties.Graveyard.State> list = new List<LevelProperties.Graveyard.State>();
        goalTimes = new Level.GoalTimes(120f, 120f, 120f);

        MinMax beamSpeed = new MinMax(550f, 675f);
        MinMax beamHesitateAfterAttacknew = new MinMax(0.8f, 1.3f);
        float beamYPos = 150f;
        string beamAttacksBeforeBeamString = "2,1,2,1,1";
        float beamWarning = 0.25f;

        string[] projNumProjectiles = new string[]{
            "6,3,5,6,3,2",
            "4,6,5,2,7"
        };
        MinMax projDelayBetweenProjectiles = new MinMax(0.75f, 1.1f);
        float projProjectileSpeed = 500f;//565
        MinMax projHesitateAfterAttack = new MinMax(.15f, 2.1f);
        string projAngleOffsetString = "0,-12,2,-2,5,0,0,-5,8,0,0,-8,2,3,-2,1,10,12,-3,7";
        string projPinkString = "N,N,P,N,N,N,P";

        LevelProperties.Graveyard.SplitDevilBeam beam = new(beamSpeed, beamHesitateAfterAttacknew, beamYPos, beamAttacksBeforeBeamString, beamWarning);
        LevelProperties.Graveyard.SplitDevilProjectiles proj = new(projNumProjectiles, projDelayBetweenProjectiles, projProjectileSpeed, projHesitateAfterAttack, projAngleOffsetString, projPinkString);

        list.Add(new LevelProperties.Graveyard.State(10f, new LevelProperties.Graveyard.Pattern[][]{
                new LevelProperties.Graveyard.Pattern[0]}, LevelProperties.Graveyard.States.Main, 
                beam, proj));
               
        return new LevelProperties.Graveyard(hp, goalTimes, list.ToArray());
    }
}
