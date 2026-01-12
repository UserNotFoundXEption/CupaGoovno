using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace CupaGoovno;

public class Dragonfly
{
    public void Init()
    {
        On.TreePlatformingLevelDragonfly.Start += Start;
        On.TreePlatformingLevelDragonfly.fall_cr += fall_cr;
    }

    protected void Start(On.TreePlatformingLevelDragonfly.orig_Start orig, TreePlatformingLevelDragonfly self)
    {
        self.StartWithCondition(AbstractPlatformingLevelEnemy.StartCondition.Instant);
        Level.Current.OnLevelStartEvent += self.OnLevelStart;
        self._aim = new GameObject("Aim").transform;
        self._aim.SetParent(self._projectileRoot);
        self._aim.ResetLocalTransforms();
        self.FrameDelayedCallback(new Action(self.StartLockCheck), 1);
        if (YoMamaFat.loadDragofly)//new start
        {
            YoMamaFat.loadDragofly = false;
            YoMamaFat.dragonfly = self;
            self.transform.parent = null;
            GameObject.DontDestroyOnLoad(self.gameObject);
            Awake.Load(Levels.FlyingBird, true);
        }
        enter = false;//new end
        self.LockDistance = 1550f;
        self.startPos = self.transform.position;
        self.aimIndex = UnityEngine.Random.Range(0, self.Properties.dragonFlyAimString.Split(',').Length);
        self.delayIndex = UnityEngine.Random.Range(0, self.Properties.dragonFlyAtkDelayString.Split(',').Length);
        self.LockDistance -= self.Properties.dragonFlyLockDistOffset;
        self.mosquitos = new List<TreePlatformingLevelMosquito>(self.platforms.GetComponentsInChildren<TreePlatformingLevelMosquito>());
        self.currentMosquitos = self.randomizeList(self.mosquitos);
        self.StartCoroutine(enter_cr(self));
    }

    private IEnumerator enter_cr(TreePlatformingLevelDragonfly self)
    {
        self.transform.position = new Vector3(self.startPos.x + 800f, self.startPos.y);
        while (!self.bigEnemyCameraLock && !enter)
        //while (!self.bigEnemyCameraLock)
        {
            yield return null;
        }
        if (enter)//new start
        {
            self.transform.SetScale(0.5f, 0.5f, null);
            self.startPos = new Vector3(500, 0);
            self.transform.position = new Vector3(self.startPos.x + 800f, self.startPos.y);
        }//new end
        float t = 0f;
        float time = self.Properties.dragonFlyInitRiseTime;
        while (t < time)
        {
            float val = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, t / time);
            self.transform.position = Vector2.Lerp(self.transform.position, self.startPos, val);
            t += CupheadTime.Delta;
            yield return null;
        }
        self.transform.position = self.startPos;
        self.GetComponent<Collider2D>().enabled = true;
        if (enter)//new start
        {
            self.StartCoroutine(square_movement_cr(self));
        }
        else
        {
            self.StartCoroutine(self.sine_cr());
        }//new end
         //self.StartCoroutine(self.sine_cr());
        yield break;
    }

    private IEnumerator fall_cr(On.TreePlatformingLevelDragonfly.orig_fall_cr orig, TreePlatformingLevelDragonfly self)
    {
        float velocity = 0f;
        float gravity = 1500f;
        yield return CupheadTime.WaitForSeconds(self, 1.5f);
        self.explosion.StopExplosions();
        while (self.transform.position.y > -CupheadLevelCamera.Current.Height - 1000f)//200
        {
            self.transform.AddPosition(0f, velocity * CupheadTime.Delta, 0f);
            velocity -= gravity * CupheadTime.Delta;
            yield return null;
        }
        UnityEngine.Object.Destroy(self.gameObject);//new
        yield return null;
        yield break;
    }

    private IEnumerator square_movement_cr(TreePlatformingLevelDragonfly self)//new
    {
        float speedY = 300f;
        float speedX = 300f;
        for (; ; )
        {
            while (self.transform.position.y > -150f)
            {
                self.transform.AddPosition(0f, -CupheadTime.Delta * speedY, 0f);
                yield return null;
            }
            while (self.transform.position.x > -550f)
            {
                self.transform.AddPosition(-CupheadTime.Delta * speedX, 0f, 0f);
                yield return null;
            }
            while (self.transform.position.y < 250f)
            {
                self.transform.AddPosition(0f, CupheadTime.Delta * speedY, 0f);
                yield return null;
            }
            while (self.transform.position.x < 550f)
            {
                self.transform.AddPosition(CupheadTime.Delta * speedX, 0f, 0f);
                yield return null;
            }
        }
    }

    public static bool enter;
}
