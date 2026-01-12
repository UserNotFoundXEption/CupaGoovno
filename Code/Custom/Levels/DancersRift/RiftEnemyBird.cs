using System;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;
using static CupaGoovno.RiftManager;

namespace CupaGoovno;

public class RiftEnemyBird : RiftEnemyFacing
{
    public override void Init(EnemySpawnDelay delay)
    {
        base.Init(delay);
        hp = delay.arguments[1] - 48;
        UpdateSprite();
    }

    public override void Damage()
    {
        hp--;
        if(hp <= 0)
        {
            Die();
        }
        else
        {
            UpdateSprite();
            this.StopAllCoroutines();
            this.StartCoroutine(moveToNewColumn_cr());
            timeOfArrival += p.timePerBeat;
        }
    }

    private IEnumerator moveToNewColumn_cr()
    {
        enemies[column].Remove(this);
        column = GetNewColumn();
        enemies[column].Insert(0, this);
        BeatsFromPlayer = 1;
        moving = true;
        yield return base.move_cr(1, true);
    }

    private void UpdateSprite()
    {
        switch (hp)
        {
            case 1:
                spriteRenderer.sprite = sprite1hp;
                break;
            case 2:
                spriteRenderer.sprite = sprite2hp;
                break;
            case 3:
                spriteRenderer.sprite = sprite3hp;
                break;
        }
    }

    public static Sprite sprite3hp;
    public static Sprite sprite2hp;
    public static Sprite sprite1hp;

    private int hp;
}
