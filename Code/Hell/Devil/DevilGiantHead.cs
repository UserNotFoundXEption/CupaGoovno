using System;
using System.Collections;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class DevilGiantHead
{
    public void Init()
    {
        On.DevilLevelGiantHead.SpawnSpiral += SpawnSpiral;
        On.DevilLevelGiantHead.eye_cr += eye_cr;
        On.DevilLevelGiantHead.StartBombEye += StartBombEye;
        On.DevilLevelGiantHead.SpawnBomb += SpawnBomb;
        On.DevilLevelGiantHead.fireballs_cr += fireballs_cr;
        On.DevilLevelGiantHead.hands_cr += hands_cr;
        On.DevilLevelGiantHead.tears_cr += tears_cr;
    }

    private void SpawnSpiral(On.DevilLevelGiantHead.orig_SpawnSpiral orig, DevilLevelGiantHead self)
    {
        //self.skullPrefab.Create(self.spawnPos, self.properties.CurrentState.skullEye);
        DevilLevelSkull skull = self.skullPrefab.Create(Vector2.zero, self.properties.CurrentState.skullEye);
        DevilSkull.ready = false;
        cameraFollowCoroutine = self.StartCoroutine(cameraFollow_cr(self, skull.gameObject));
    }

    private IEnumerator eye_cr(On.DevilLevelGiantHead.orig_eye_cr orig, DevilLevelGiantHead self, float hesitateTime)
    {
        if (self.state == DevilLevelGiantHead.State.BombEye)
        {
            self.bombOnLeft = Rand.Bool();
            self.spawnPos = ((!self.bombOnLeft) ? self.rightEyeRoot.position : self.leftEyeRoot.position);
            self.animator.SetTrigger("OnBomb");
            self.animator.SetBool("BombLeft", self.bombOnLeft);
        }
        else
        {
            self.spawnPos = self.middleRoot.transform.position;
            self.animator.SetTrigger("OnSpiral");
        }
        yield return CupheadTime.WaitForSeconds(self, hesitateTime);
        //self.state = DevilLevelGiantHead.State.Idle;
        yield break;
    }

    public void StartBombEye(On.DevilLevelGiantHead.orig_StartBombEye orig, DevilLevelGiantHead self)
    {
        if(self.state == DevilLevelGiantHead.State.Idle)//new
        {//new
            self.state = DevilLevelGiantHead.State.BombEye;
            self.StartCoroutine(self.eye_cr(self.properties.CurrentState.bombEye.hesitate.RandomFloat()));
        }//new
    }

    private void SpawnBomb(On.DevilLevelGiantHead.orig_SpawnBomb orig, DevilLevelGiantHead self)
    {
        self.bombPrefab.Create(self.spawnPos, self.properties.CurrentState.bombEye, self.bombOnLeft);
        self.state = DevilLevelGiantHead.State.Idle;//new
    }

    private IEnumerator fireballs_cr(On.DevilLevelGiantHead.orig_fireballs_cr orig, DevilLevelGiantHead self)
    {
        bool fromRight = Rand.Bool();
        int index = (!fromRight) ? 0 : (self.raisablePlatforms.Length - 1);
        LevelProperties.Devil.Fireballs p = self.properties.CurrentState.fireballs;
        yield return CupheadTime.WaitForSeconds(self, p.initialDelay);
        for (; ; )
        {
            p = self.properties.CurrentState.fireballs;
            DevilLevelPlatform platform = self.raisablePlatforms[index];
            index = (((!fromRight) ? (index + 1) : (index - 1)) + self.raisablePlatforms.Length) % self.raisablePlatforms.Length;
            if (platform.state == DevilLevelPlatform.State.Dead)
            {
                yield return null;
            }
            else
            {
                //self.fireballPrefab.Create(platform.transform.position.x, p.fallSpeed, p.fallAcceleration, p.size / 200f);
                DevilFireball.Create(self.fireballPrefab, platform.transform.position.x, p.fallSpeed, p.fallAcceleration, p.size / 200f);//new
                yield return CupheadTime.WaitForSeconds(self, p.spawnDelay);
            }
        }
    }

    private IEnumerator hands_cr(On.DevilLevelGiantHead.orig_hands_cr orig, DevilLevelGiantHead self)
    {
        DevilTear.canParry = true;//new start
        if (cameraFollowCoroutine != null)
        {
            self.StopCoroutine(cameraFollowCoroutine);
        }
        Camera.UnFollow();
        GameObject.Destroy(GameObject.FindObjectOfType<DevilLevelSkull>().gameObject);
        GameObject[] objs = GameObject.FindObjectsOfType<GameObject>();
        foreach(GameObject obj in objs)
        {
            if(obj.name == "Devil_Fireball" || obj.name == "Devil_Bomb")
            {
                GameObject.Destroy(obj);
            }
        }//new end
        self.waitingForTransform = true;
        /*while (self.state != DevilLevelGiantHead.State.Idle)
        {
            yield return null;
        }*/
        bool platformsDown = false;
        while (!platformsDown)
        {
            platformsDown = true;
            foreach (DevilLevelPlatform devilLevelPlatform in self.raisablePlatforms)
            {
                if (devilLevelPlatform.state == DevilLevelPlatform.State.Raising)
                {
                    platformsDown = false;
                }
            }
            yield return null;
        }
        self.waitingForTransform = false;
        foreach (DevilLevelPlatform devilLevelPlatform2 in self.HandsPhaseExit)
        {
            devilLevelPlatform2.Lower(self.properties.CurrentState.giantHeadPlatforms.exitSpeed);
        }
        self.StartSwoopers();
        bool leftHandShoot = Rand.Bool();
        self.hands[0].StartPattern(self.properties.CurrentState.hands);
        self.hands[1].StartPattern(self.properties.CurrentState.hands);
        self.handsSpawnCr = self.StartCoroutine(self.spawn_hand_cr());
        self.transform.AddPosition(0f, 200f, 0f);//new
        for (; ; )
        {
            int handIndex = (!leftHandShoot) ? 1 : 0;
            if (self.hands[handIndex] != null)
            {
                self.hands[handIndex].animator.SetTrigger("OnAttack");
            }
            leftHandShoot = !leftHandShoot;
            yield return CupheadTime.WaitForSeconds(self, self.properties.CurrentState.hands.shotDelay.RandomFloat());
            yield return null;
        }
    }

    private IEnumerator tears_cr(On.DevilLevelGiantHead.orig_tears_cr orig, DevilLevelGiantHead self)
    {
        self.animator.SetTrigger("OnTransB");
        self.waitingForTransform = true;
        /*while (self.state != DevilLevelGiantHead.State.Idle)
        {
            yield return null;
        }*/
        bool platformsDown = false;
        while (!platformsDown)
        {
            platformsDown = true;
            foreach (DevilLevelPlatform devilLevelPlatform in self.raisablePlatforms)
            {
                if (devilLevelPlatform.state == DevilLevelPlatform.State.Raising)
                {
                    platformsDown = false;
                }
            }
            yield return null;
        }
        self.waitingForTransform = false;
        foreach (DevilLevelPlatform devilLevelPlatform2 in self.TearsPhaseExit)
        {
            devilLevelPlatform2.Lower(self.properties.CurrentState.giantHeadPlatforms.exitSpeed);
        }
        if (!self.properties.CurrentState.giantHeadPlatforms.riseDuringTearPhase)
        {
            self.StopCoroutine(self.platformCr);
        }
        self.StopCoroutine(self.handsCr);
        self.StopCoroutine(self.handsSpawnCr);
        self.StopCoroutine(self.swooperSpawnCr);
        self.StopCoroutine(self.swooperSwoopCr);
        while (self.swoopers.Count > 0)
        {
            self.swoopers[0].Die();
        }
        foreach (DevilLevelHand devilLevelHand in self.hands)
        {
            devilLevelHand.isDead = true;
            devilLevelHand.Die();
        }
        bool spawnLeft = true;
        self.StartCoroutine(self.fireballs_cr());//new
        self.transform.AddPosition(0f, -200f, 0f);//new
        yield return CupheadTime.WaitForSeconds(self, 2f);
        for (; ; )
        {
            //self.tearPrefab.CreateTear((!spawnLeft) ? self.rightTearRoot.transform.position : self.leftTearRoot.transform.position, self.properties.CurrentState.tears.speed);
            WallOfTears(self, spawnLeft);//new
            yield return CupheadTime.WaitForSeconds(self, self.properties.CurrentState.tears.delay);
            spawnLeft = !spawnLeft;
        }
    }

    private void WallOfTears(DevilLevelGiantHead self, bool spawnLeft)//new
    {
        float x = spawnLeft ? -700f : 700f;
        float speed = self.properties.CurrentState.tears.speed;
        if(!spawnLeft)
        {
            speed *= -1f;
        }
        float rotation = spawnLeft ? 90f : -90f;
        for(float y = -400f; y < 800f; y += 50f)
        {
            DevilLevelTear tear = self.tearPrefab.CreateTear(new Vector2(x, y), speed);
            tear.transform.SetEulerAngles(0f, 0f, rotation);
        }
    }

    private IEnumerator cameraFollow_cr(DevilLevelGiantHead self, GameObject obj)//new
    {
        while (!DevilSkull.ready)
        {
            yield return null;
        }
        Camera.Follow(obj);
        float x = 0;
        float y = 0;
        float offset = 1000f;
        do
        {
            x = obj.transform.position.x;
            y = obj.transform.position.y;
            yield return null;
        }
        while (x < offset && x > -offset && y < offset && y > -offset);
        Camera.UnFollow();
        yield break;
    }

    private Coroutine cameraFollowCoroutine;
}
