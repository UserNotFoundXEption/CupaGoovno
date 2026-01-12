using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static CupaGoovno.RiftManager;

namespace CupaGoovno;

public class RiftEnemyCupcake : RiftEnemyAbstract
{
    public override void Init(RiftManager.EnemySpawnDelay delay)
    {
        base.Init(delay);
        BeatsFromPlayer += 2;
    }

    public override void Move()
    {
        if (moving)
        {
            return;
        }
        moving = true;
        forceStop = false;

        if (movingUp)
        {
            BeatsFromPlayer++;
            spriteRenderer.sprite = spriteMoving;
        }
        else
        {
            BeatsFromPlayer -= 3;
        }

        StartCoroutine(move_cr(BeatsFromPlayer));
        spriteRenderer.sortingOrder = 20 - BeatsFromPlayer;
    }

    protected override IEnumerator move_cr(int newDistance)
    {
        yield return base.move_cr(newDistance);
        if (!movingUp)
        {
            spriteRenderer.sprite = spriteLanding;
        }
        movingUp = !movingUp;
    }

    public static Sprite spriteMoving;
    public static Sprite spriteLanding;

    private bool movingUp = false;
}
