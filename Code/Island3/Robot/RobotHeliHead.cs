using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CupaGoovno;

public class RobotHeliHead
{
    public void Init()
    {
        On.RobotLevelHelihead.InitHeliHead += InitHeliHead;
        On.RobotLevelHelihead.inventorIntro_cr += inventorIntro_cr;
        On.RobotLevelHelihead.stateEasing_cr += stateEasing_cr;
        On.RobotLevelHelihead.verticalMovement_cr += verticalMovement_cr;
        On.RobotLevelHelihead.blockade_cr += blockade_cr;
        On.RobotLevelHelihead.ChangeState += ChangeState;
    }

    public void InitHeliHead(On.RobotLevelHelihead.orig_InitHeliHead orig, RobotLevelHelihead self, LevelProperties.Robot properties)
    {
        orig(self, properties);
        self.StartCoroutine(SpawnUFOs(self, 3, 3f));//new
    }

    private IEnumerator inventorIntro_cr(On.RobotLevelHelihead.orig_inventorIntro_cr orig, RobotLevelHelihead self)
    {
        Vector3 end = new Vector3(self.pivotPoint.transform.position.x, -760f);
        Vector3 start = self.transform.position;
        float pct = 0f;
        while (pct < 1f)
        {
            self.transform.position = Vector3.Lerp(start, end, pct);
            pct += CupheadTime.Delta;
            yield return null;
        }
        self.transform.position = end;
        self.StartCoroutine(space_invaders_cr(self));//new
        //self.StartCoroutine(self.stateEasing_cr());
        yield break;
    }

    private IEnumerator stateEasing_cr(On.RobotLevelHelihead.orig_stateEasing_cr orig, RobotLevelHelihead self)
    {
        self.transform.rotation = Quaternion.identity;
        Vector3 start = self.transform.position;
        //Vector3 end = new Vector3(self.pivotPoint.transform.position.x - 200f, self.pivotPoint.transform.position.y);
        Vector3 end = Vector3.left * 150f;//new
        float pct = 0f;
        while (pct < 1f)
        {
            self.transform.position = Vector3.Lerp(start, end, pct);
            pct += CupheadTime.Delta;
            yield return null;
        }
        AudioManager.Stop("robot_headspin");
        self.animator.Play("Inventor Intro");
        self.StartCoroutine(self.verticalMovement_cr());
        //self.speed *= 2f;
        yield return self.animator.WaitForAnimationToEnd(self, "End", true, true);
        yield return CupheadTime.WaitForSeconds(self, self.properties.CurrentState.inventor.initialAttackDelay);
        float normalizedTime = self.animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1f;
        float delay = 0f;
        if (self.animator.GetCurrentAnimatorStateInfo(0).length / normalizedTime < 1f)
        {
            delay -= normalizedTime;
        }
        else
        {
            delay += 1f - normalizedTime;
        }
        yield return CupheadTime.WaitForSeconds(self, delay);
        self.StartCoroutine(self.blockade_cr());
        if (self.properties.CurrentState.inventor.gemColourString.Split(new char[]
        {
        ','
        })[self.attackTypeIndex] == "R")
        {
            self.animator.Play("Red Gem Attack");
            yield return self.animator.WaitForAnimationToEnd(self, "Red Gem Attack", false, true);
            self.animator.Play("RedGemFXIntro", 2);
            self.gem.InitFinalStage(self, self.properties, false);
        }
        else
        {
            self.animator.Play("Blue Gem Attack");
            yield return self.animator.WaitForAnimationToEnd(self, "Blue Gem Attack", false, true);
            self.animator.Play("BlueGemFXIntro", 2);
            self.gem.InitFinalStage(self, self.properties, true);
        }
        //self.speed /= 2f;
        self.StartCoroutine(self.easeValues_cr(true));
        yield break;
    }

    private IEnumerator verticalMovement_cr(On.RobotLevelHelihead.orig_verticalMovement_cr orig, RobotLevelHelihead self)
    {
        self.speed = 1f;
        float time = 0f;
        for (; ; )
        {
            time += CupheadTime.Delta; //new start
            Vector3 main = Vector3.left * 150f;
            Vector3 vertical = Vector3.up * Mathf.Sin(time * self.speed) * self.verticalMovementStrength;
            Vector3 horizontal = Vector3.right * Mathf.Sin(time * (0.5f * self.speed)) * 500f;
            self.transform.position = main + vertical + horizontal;//new end
            //time += CupheadTime.Delta * 2;
            //self.transform.position = self.pivotPoint.transform.position + Vector3.left * 200f + Vector3.up * Mathf.Sin(time * self.speed) * self.verticalMovementStrength + Vector3.right * Mathf.Sin(time * (2f * self.speed)) * self.horizontalMovementStrength;
            yield return null;
        }
    }

    private IEnumerator blockade_cr(On.RobotLevelHelihead.orig_blockade_cr orig, RobotLevelHelihead self)
    {
        yield break;
    }

    public void ChangeState(On.RobotLevelHelihead.orig_ChangeState orig, RobotLevelHelihead self)
    {
        self.current = RobotLevelHelihead.state.second;
        self.StopAllCoroutines();
        self.StartCoroutine(self.inventorIntro_cr());
        foreach (FlyingCowboyLevelUFO ufo in ufoList)//new start
        {
            ufo.Dead();
        }//new end
    }

    private IEnumerator SpawnUFOs(RobotLevelHelihead self, int count, float delay) //new
    {
        LevelProperties.FlyingCowboy.UFOEnemy properties = YoMamaFat.cowboyUfoProperties;
        FlyingCowboyLevelUFO ufoPrefab = YoMamaFat.cowboyUfo;
        Vector3 pos = new Vector3(0f, properties.topUFOVerticalPosition);
        ufoList = new List<FlyingCowboyLevelUFO>();
        for (int i = 0; i < count; i++)
        {
            FlyingCowboyLevelUFO ufo = ufoPrefab.Spawn<FlyingCowboyLevelUFO>();
            ufo.Init(pos, properties, properties.UFOHealth);
            ufoList.Add(ufo);
            yield return CupheadTime.WaitForSeconds(self, delay);
        }
        yield break;
    }

    private IEnumerator space_invaders_cr(RobotLevelHelihead self)//new
    {
        PlaneWeaponManager.InvaderModeOn(YoMamaFat.planeWeaponManager);
        List<GameObject> invadersList = new List<GameObject>();
        invadersLeft = 0;
        for (float x = 1200f; x <= 1700f; x += 100f)
        {
            for (float y = -250f; y <= 250f; y += 100f)
            {
                invadersLeft++;
                int speed = 300;//430
                MinMax yRange = new MinMax(y - 30f, y + 70f);
                GameObject shotbot = UnityEngine.Object.Instantiate<GameObject>(YoMamaFat.robotShotBot, new Vector3(x, y), Quaternion.identity);
                int row = (int)(x / 100f) - 14;
                RobotShotBot.InitSpaceInvader(shotbot.GetComponent<RobotLevelHatchShotbot>(), self.properties.CurrentState.shotBot.shotbotHealth / 2, self.properties.CurrentState.shotBot.bulletSpeed, self.properties.CurrentState.shotBot.pinkBulletCount, self.properties.CurrentState.shotBot.shotbotShootDelay * 5,
                    speed, yRange, row, invadersLeft);
                invadersList.Add(shotbot);
            }
        }
        int invadersAlive = 1;
        while (invadersAlive > 0)
        {
            invadersAlive = 0;
            foreach (GameObject shotbot in invadersList)
            {
                if (shotbot.gameObject != null)
                {
                    invadersAlive++;
                }
            }
            invadersLeft = invadersAlive;
            //float start = Level.Current.timeline.events[1].percentage;
            //float end = Level.Current.timeline.events[2].percentage;
            float start = 0.67f;
            float end = 0.44f;
            float current = end + (start - end) * invadersLeft / 36;
            Level.Current.timeline.health = 3060f * current;
            yield return null;
        }
        PlaneWeaponManager.InvaderModeOff();
        self.StartCoroutine(self.stateEasing_cr());
        yield break;
    }

    private static List<FlyingCowboyLevelUFO> ufoList = new List<FlyingCowboyLevelUFO>();
    public static int invadersLeft;
}
