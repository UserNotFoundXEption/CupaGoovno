using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;
using static Rewired.ComponentControls.Effects.RotateAroundAxis;

namespace CupaGoovno;

public class MoaiLevelSpike : CustomLevelSummon
{
    public static MoaiLevelSpike Create(float x)
    {
        MoaiLevelSpike spike = Create<MoaiLevelSpike>(prefab);

        spike.canTakeDamage = false;
        spike.hp = p.hp;
        spike.transform.position = new Vector2(x, p.hiddenY);
        spike.StartCoroutine(spike.spike_cr());

        spike.GetComponent<SpriteRenderer>().sortingLayerName = "Background";

        return spike;
    }

    private IEnumerator spike_cr()
    {
        float t = 0f;
        float maxTime = p.enterTime;
        while (t < maxTime)
        {
            t += CupheadTime.delta;
            float y = Mathf.Lerp(p.hiddenY, p.y.max, t / maxTime);
            transform.SetPosition(null, y);
            yield return null;
        }
        
        canTakeDamage = true;
        yield return CupheadTime.WaitForSeconds(this, p.time);
        canTakeDamage = false;

        t = 0f;
        maxTime = p.exitTime;
        float currentY = transform.position.y;
        while (t < maxTime)
        {
            t += CupheadTime.delta;
            float y = Mathf.Lerp(currentY, p.hiddenY, t / maxTime);
            transform.SetPosition(null, y);
            yield return null;
        }

        if (gameObject != null)
        {
            Destroy(gameObject);
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (!canTakeDamage)
        {
            return;
        }

        float y = transform.position.y;
        float targetY = Mathf.Lerp(p.y.min, p.y.max, hp / p.hp);
        if (targetY < y || hp <= 0f)
        {
            transform.AddPosition(0f, -100f * CupheadTime.FixedDelta);
        }
    }

    public override void Die()
    {
        StopAllCoroutines();
        Destroy(gameObject, 2f);
    }

    public static GameObject prefab;

    private bool canTakeDamage;

    private static MoaiProperties.Spikes p = new();
}
