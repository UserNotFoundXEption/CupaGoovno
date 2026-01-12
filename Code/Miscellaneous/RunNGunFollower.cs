using Blender.Utility;
using Rewired;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class RunNGunFollower : CustomProjectile
{
    public static void Init()
    {
        AssetHelper.AddPersistentPath(AssetHelper.LoaderType.Single, prefabPath);
    }

    public static RunNGunFollower Create(bool runNGun)
    {
        if(prefab == null)
        {
            prefab = AssetLoader<UnityEngine.Object>.GetCachedAsset(prefabPath) as GameObject;
            if (prefab == null)
            {
                Plugin.Log("runNGunFollower prefab not found: " + prefabPath);
            }
        }

        RunNGunFollower follower = Create<RunNGunFollower>(prefab);
        follower.DestroyDistance = 0f;

        follower.sr = follower.GetComponent<SpriteRenderer>();
        follower.sr.enabled = false;

        follower.collider = follower.GetComponent<CapsuleCollider2D>();
        follower.collider.enabled = false;

        follower.playerPath = [];
        follower.timeElapsed = 0f;
        follower.runNGun = runNGun;

        follower.StartCoroutine(follower.follower_cr());

        return follower;
    }

    private IEnumerator follower_cr()
    {
        AbstractPlayerController player = Other.Player();
        while(player == null)
        {
            player = Other.Player();
            yield return null;
        }
        
        yield return CupheadTime.WaitForSeconds(this, 3f);

        UpdateDelay();
        while(timeElapsed < delay)
        {
            timeElapsed += CupheadTime.delta;
            SavePos(player);
            yield return null;
        }

        sr.enabled = true;
        collider.enabled = true;

        for(; ; )
        {
            timeElapsed += CupheadTime.delta;
            SavePos(player);
            UpdateDelay();
            PosAndTime newPos = new(
                transform.position,
                0f,
                transform.localScale.x);

            while (playerPath[0].time < timeElapsed - delay)
            {
                newPos = playerPath[0];
                playerPath.RemoveAt(0);
            }

            transform.position = newPos.pos;
            transform.SetScale(newPos.scale);
            yield return null;
        }
    }

    private void UpdateDelay()
    {
        if (runNGun)
        {
            delay = initialDelay - timeElapsed * delayDecreasePerSecond;
            if(Level.Current.CurrentLevel == Levels.Platforming_Level_3_2)
            {
                delay += 0.5f;
            }
        }
        else
        {
            delay = mausoleumDelay;
        }
    }

    private void SavePos(AbstractPlayerController player)
    {
        playerPath.Add(new(
            new Vector2(player.center.x, player.center.y),
            timeElapsed,
            player.transform.localScale.x));
    }

    public override float DestroyLifetime => 0f;

    private static GameObject prefab;
    
    private List<PosAndTime> playerPath;
    private float timeElapsed;
    private CapsuleCollider2D collider;
    private SpriteRenderer sr;
    private bool runNGun;
    private float delay;

    private const float delayDecreasePerSecond = 0.01f;
    private const float initialDelay = 1f;
    private const float mausoleumDelay = 1f;
    private const string prefabPath = "CupaGoovno:cupagoovno\\runNGunFollowerPrefab";


    private struct PosAndTime(Vector2 pos, float time, float scale)
    {
        public Vector2 pos = pos;
        public float time = time;
        public float scale = scale;
    }
}
