using System;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;
using static CupaGoovno.RiftManager;

namespace CupaGoovno;

public class RiftEnemySlime : RiftEnemyFacing
{
    public override void Init(EnemySpawnDelay delay)
    {
        base.Init(delay);
        finalColumn = GetNewColumn();
    }

    public override void Move()
    {
        if (moving)
        {
            return;
        }
        moving = true;
        forceStop = false;

        enemies[column].Remove(this);
        column = GetNewColumn();

        int newDistance = BeatsFromPlayer - 1;
        int i = 0;
        for (; i < enemies[column].Count; i++)
        {
            if (enemies[column][i].BeatsFromPlayer > newDistance)
            {
                break;
            }
        }
        enemies[column].Insert(i, this);

        StartCoroutine(move_cr(newDistance));
        BeatsFromPlayer = newDistance;
        spriteRenderer.sortingOrder = 20 - BeatsFromPlayer;
    }

    protected override IEnumerator move_cr(int newDistance)
    {
        yield return base.move_cr(newDistance);
        facingDirection = facingDirection == FacingDirection.Left ? FacingDirection.Right : FacingDirection.Left;
        UpdateScale();
    }

    public override ParryScore GetParryScore(Column column)
    {
        if (column != finalColumn)
        {
            return ParryScore.Miss;
        }

        float difference = RiftManager.timeTotal - timeOfArrival;
        Plugin.Log($"Difference: {difference}");
        difference = Mathf.Abs(difference);

        return DifferenceToParryScore(difference);
    }

    private Column finalColumn;
}
