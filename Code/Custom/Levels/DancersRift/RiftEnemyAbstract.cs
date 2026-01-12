using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using UnityEngine;
using static CupaGoovno.RiftManager;

namespace CupaGoovno;

public abstract class RiftEnemyAbstract : AbstractPausableComponent
{
    public virtual void Init(RiftManager.EnemySpawnDelay delay)
    {
        BeatsFromPlayer = p.columnLength;
        timeOfArrival = (delay.beatsToSpawn + p.columnLength) * p.timePerBeat;

        Other.FixShader(gameObject);
        moving = false;

        column = delay.column;
        Vector2 pos = p.enemyPositions[column][p.columnLength];
        transform.position = pos;

        float scale = p.enemyScale[p.columnLength];
        transform.SetScale(scale, scale);

        transform.AddPosition(0f, yOffset * scale);

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public virtual void Damage()
    {
        Die();
    }

    public virtual void Die()
    {
        RiftManager.enemies[column].Remove(this);
        Destroy(gameObject);
    }

    public virtual void Move()
    {
        if (moving)
        {
            return;
        }
        moving = true;
        forceStop = false;

        BeatsFromPlayer--;
        StartCoroutine(move_cr(BeatsFromPlayer));
        spriteRenderer.sortingOrder = 20 - BeatsFromPlayer;
    }

    protected virtual IEnumerator move_cr(int newDistance)
    {
        yield return move_cr(newDistance, false);
    }

    protected virtual IEnumerator move_cr(int newDistance, bool ignoreForceStop)
    {
        if(newDistance < 0)
        {
            yield break;
        }

        float startScale = transform.localScale.x;
        float newScale = GetNewScale(newDistance);

        Vector2 startPos = transform.position;

        int d = newDistance >= p.enemyPositions[column].Count ? p.enemyPositions[column].Count - 1 : newDistance;
        Vector2 newPos = p.enemyPositions[column][d];
        newPos.y += yOffset * Mathf.Abs(newScale);

        float t = 0f;
        while(t < p.enemyMoveTime && (ignoreForceStop || !forceStop))
        {
            t += CupheadTime.Delta;
            transform.position = Vector2.Lerp(startPos, newPos, t / p.enemyMoveTime);
            float scale = Mathf.Lerp(startScale, newScale, t / p.enemyMoveTime);
            transform.SetScale(scale, Mathf.Abs(scale));
            yield return null;
        }
        transform.position = newPos;
        transform.SetScale(newScale, Mathf.Abs(newScale));

        if(newDistance == 0)
        {
            StartCoroutine(damageCountdown_cr());
        }
        moving = false;
    }

    protected virtual float GetNewScale(int newDistance)
    {
        if(newDistance >= p.enemyScale.Count)
        {
            newDistance = p.enemyScale.Count - 1;
        }
        return p.enemyScale[newDistance];
    }

    private IEnumerator damageCountdown_cr()
    {
        yield return CupheadTime.WaitForSeconds(this, p.parryTimeMaxDiff[3]);

        AbstractPlayerController player = PlayerManager.GetFirst();
        if (player.stats.Loadout.charm != Charm.charm_smoke_dash)
        {
            player.damageReceiver.TakeDamage(
            new DamageDealer.DamageInfo(
                1f,
                DamageDealer.Direction.Neutral,
                Vector2.zero,
                DamageDealer.DamageSource.Enemy));
            RiftManager.KillAllEnemies();
        }
        else
        {
            Die();
        }

        RiftManager.instance.OnMiss();
    }

    public float GetDifference()
    {
        return RiftManager.timeTotal - timeOfArrival;
    }

    public virtual ParryScore GetParryScore(Column column)
    {
        if (column != this.column)
        {
            return ParryScore.Miss;
        }

        float difference = GetDifference();
        difference = Mathf.Abs(difference);

        return DifferenceToParryScore(difference);
    }

    protected ParryScore DifferenceToParryScore(float difference)
    {
        for (int i = 0; i < p.parryTimeMaxDiff.Length; i++)
        {
            if (difference <= p.parryTimeMaxDiff[i])
            {
                return (ParryScore)i;
            }
        }
        return ParryScore.Miss;
    }

    public void ForceStop()
    {
        forceStop = true;
        moving = false;
    }

    public int BeatsFromPlayer { get; set; }

    protected float timeOfArrival;
    protected static RiftProperties p = new();
    protected bool moving;
    protected bool forceStop = false;

    protected Column column;
    protected float yOffset = 60f;
    protected SpriteRenderer spriteRenderer;
}
