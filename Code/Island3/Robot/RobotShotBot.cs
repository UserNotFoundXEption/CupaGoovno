using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CupaGoovno;

public class RobotShotBot
{
    public void Init()
    {
        On.RobotLevelHatchShotbot.InitShotbot += InitShotbot;
        On.RobotLevelHatchShotbot.fire_cr += fire_cr;
    }

    public void InitShotbot(On.RobotLevelHatchShotbot.orig_InitShotbot orig, RobotLevelHatchShotbot self, int hp, int bulletSpeed, int pinkBulletCount, float shootDelay, int flightSpeed)
    {
        orig(self, hp, bulletSpeed, pinkBulletCount, shootDelay, flightSpeed);
        self.StartCoroutine(self.fire_cr());//new
    }

    private IEnumerator fire_cr(On.RobotLevelHatchShotbot.orig_fire_cr orig, RobotLevelHatchShotbot self)
    {
        for (; ; )
        {
            yield return CupheadTime.WaitForSeconds(self, self.shootDelay);
            Vector3 dir = (PlayerManager.GetRandom().center - self.transform.position).normalized;//new start
            float angle = Vector3.Angle(Vector3.up, dir) - 90f;
            Vector3 pos = self.transform.position;
            Quaternion rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
            MinMax speed = new MinMax(100f, 200f);
            float acceleration = 50f;
            YoMamaFat.robotGemProjectile.Spawn(pos, rotation).Init(speed, acceleration, 0, 0, 10f, false, false);//new end
            /*AudioManager.Play("robot_shotbot_shoot");
            self.emitAudioFromObject.Add("robot_shotbot_shoot");
            GameObject proj = UnityEngine.Object.Instantiate<GameObject>(self.projectile);
            proj.transform.position = self.transform.position;
            proj.transform.right = (PlayerManager.GetNext().center - self.transform.position).normalized;
            proj.GetComponent<BasicProjectile>().Speed = (float)self.bulletSpeed;
            if (self.shotsFired >= self.pinkBulletCount)
            {
                self.shotsFired = 0;
                proj.GetComponent<SpriteRenderer>().sprite = self.spriteSpecial;
                proj.GetComponent<BasicProjectile>().SetParryable(true);
            }
            else
            {
                self.shotsFired++;
            }*/
            yield return null;
        }
    }

    public static void InitSpaceInvader(RobotLevelHatchShotbot self, int hp, int bulletSpeed, int pinkBulletCount, float defaultShootDelay, int flightSpeed, MinMax yRange, int row, int invaderNumber)//new
    {
        self.health = hp;
        self.bulletSpeed = bulletSpeed;
        self.pinkBulletCount = pinkBulletCount;
        self.shootDelay = defaultShootDelay;
        self.flightSpeed = flightSpeed;
        self.damageDealer = DamageDealer.NewEnemy();
        yRangeDic[self] = yRange;
        self.transform.SetEulerAngles(null, null, 180f);
        self.GetComponent<DamageReceiver>().OnDamageTaken += self.OnDamageTaken;
        self.StartCoroutine(invaderMove_cr(self, row, invaderNumber));
    }

    private static IEnumerator invaderMove_cr(RobotLevelHatchShotbot self, int row, int invaderNumber)//new
    {
        float absSpeed = Mathf.Abs(self.flightSpeed);
        float currentX = self.transform.position.x;
        float minY = yRangeDic[self].min;
        float maxY = yRangeDic[self].max;
        int bounces = 0;
        for (; ; )
        {
            while ((self.transform.position.y > minY && self.flightSpeed < 0) || (self.transform.position.y < maxY && self.flightSpeed > 0))
            {
                self.transform.AddPosition(0f, self.flightSpeed * CupheadTime.Delta, 0f);
                yield return null;
            }
            self.flightSpeed = -self.flightSpeed;
            currentX -= 100f;
            bounces++;
            if (bounces == 8)
            {
                absSpeed /= 5;
                self.flightSpeed /= 5;
                BeginFire(self, row, invaderNumber);
            }
            while (self.transform.position.x > currentX && bounces <= 15)
            {
                self.transform.AddPosition(-absSpeed * CupheadTime.Delta, 0f, 0f);
                yield return null;
            }
        }
    }

    private static void BeginFire(RobotLevelHatchShotbot self, int row, int invaderNumber)
    {
        switch (row)
        {
            default:
                self.StartCoroutine(invaderFireRed_cr(self));
                break;
            case 2:
                self.StartCoroutine(invaderFirePink_cr(self, invaderNumber));
                SpriteRenderer renderer = self.GetComponent<SpriteRenderer>();
                if(renderer != null)
                {
                    renderer.color = Color.magenta;
                }
                break;
            case 3:
                self.StartCoroutine(invaderFireBlue_cr(self, invaderNumber));
                SpriteRenderer renderer2 = self.GetComponent<SpriteRenderer>();
                if (renderer2 != null)
                {
                    renderer2.color = Color.cyan;
                }
                break;
        }
    }

    private static IEnumerator invaderFireRed_cr(RobotLevelHatchShotbot self)//new
    {
        float initialDelayPercent = UnityEngine.Random.Range(0f, 1f);
        for (; ; )
        {
            for (int i = 0; i < 10; i++)
            {
                float delay = self.shootDelay / 20 * RobotHeliHead.invadersLeft / 36;
                yield return CupheadTime.WaitForSeconds(self, delay * initialDelayPercent);
            }
            initialDelayPercent = 1f;
            float angle = UnityEngine.Random.Range(-15f, 15f);
            Vector3 pos = self.transform.position;
            Quaternion rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
            MinMax speed = new MinMax(300f, 500f);
            float acceleration = 50f;
            YoMamaFat.robotGemProjectile.Spawn(pos, rotation).Init(speed, acceleration, 0, 0, 10f, false, false);
            yield return null;
        }
    }

    private static IEnumerator invaderFirePink_cr(RobotLevelHatchShotbot self, int invaderNumber)//new
    {
        float initialDelayPercent = invaderNumber / 6f - 4f;
        for (; ; )
        {
            for (int i = 0; i < 10; i++)
            {
                float delay = self.shootDelay / 8 * RobotHeliHead.invadersLeft / 36;
                yield return CupheadTime.WaitForSeconds(self, delay * initialDelayPercent);
            }
            initialDelayPercent = 1f;
            float angle = UnityEngine.Random.Range(-15f, 15f);
            Vector3 pos = self.transform.position;
            Quaternion rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
            MinMax speed = new MinMax(300f, 500f);
            float acceleration = 50f;
            AbstractProjectile proj = YoMamaFat.robotGemProjectile.Spawn(pos, rotation).Init(speed, acceleration, 0, 0, 10f, false, false);
            proj.SetParryable(true);
            SpriteRenderer renderer = proj.GetComponent<SpriteRenderer>();
            if(renderer != null)
            {
                renderer.color = Color.magenta;
            }
            proj.transform.SetScale(2f, 2f);
            yield return null;
        }
    }

    private static IEnumerator invaderFireBlue_cr(RobotLevelHatchShotbot self, int invaderNumber)//new
    {
        float initialDelayPercent = invaderNumber / 6f - 5f;
        for (; ; )
        {
            for (int i = 0; i < 10; i++)
            {
                float delay = self.shootDelay / 3 * RobotHeliHead.invadersLeft / 36;
                yield return CupheadTime.WaitForSeconds(self, delay * initialDelayPercent);
            }
            initialDelayPercent = 1f;
            Vector3 dir = (PlayerManager.GetRandom().center - self.transform.position).normalized;
            float angle = Vector3.Angle(Vector3.up, dir) - 90f;
            Vector3 pos = self.transform.position;
            Quaternion rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
            MinMax speed = new MinMax(500f, 500f);
            float acceleration = 0f;
            for(int i = 0; i < 7; i++)
            {
                AbstractProjectile proj = YoMamaFat.robotGemProjectile.Spawn(pos, rotation).Init(speed, acceleration, 0, 0, 10f, false, false);
                SpriteRenderer renderer = proj.GetComponent<SpriteRenderer>();
                if(renderer != null)
                {
                    renderer.color = Color.cyan;
                }
                yield return CupheadTime.WaitForSeconds(self, 0.1f);
            }
            yield return null;
        }
    }

    private static Dictionary<RobotLevelHatchShotbot, MinMax> yRangeDic = new Dictionary<RobotLevelHatchShotbot, MinMax>();
}
