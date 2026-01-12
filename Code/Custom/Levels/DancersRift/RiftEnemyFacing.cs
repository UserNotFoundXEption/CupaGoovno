using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static CupaGoovno.RiftManager;
using UnityEngine;

namespace CupaGoovno;

public class RiftEnemyFacing : RiftEnemyAbstract
{
    public override void Init(EnemySpawnDelay delay)
    {
        base.Init(delay);
        facingDirection = delay.arguments[0] == 'l' ? FacingDirection.Left : FacingDirection.Right;
        UpdateScale();
    }

    protected void UpdateScale()
    {
        float scaleAbs = Mathf.Abs(transform.localScale.x);
        if (facingDirection == FacingDirection.Right)
        {
            transform.SetScale(-scaleAbs, transform.localScale.y);
        }
        else
        {
            transform.SetScale(scaleAbs, transform.localScale.y);
        }
    }

    protected override float GetNewScale(int newDistance)
    {
        float newScale = p.enemyScale[newDistance];
        if (facingDirection == FacingDirection.Right)
        {
            newScale *= -1;
        }
        return newScale;
    }

    protected Column GetNewColumn()
    {
        int columnId = (int)column;
        int newColumnId = columnId;
        if (facingDirection == FacingDirection.Left)
        {
            newColumnId--;
        }
        else
        {
            newColumnId++;
        }
        if (newColumnId < 0 || newColumnId >= Enum.GetValues(typeof(Column)).Length)
        {
            Plugin.Log($"Enemy wanted to go to null column.\nfacingDirection: {facingDirection}, columnId: {columnId}, newColumnId: {newColumnId}");
            return column;
        }
        return (Column)Enum.GetValues(typeof(Column)).GetValue(newColumnId);
    }

    protected FacingDirection facingDirection;

    protected enum FacingDirection
    {
        Left,
        Right
    }
}
