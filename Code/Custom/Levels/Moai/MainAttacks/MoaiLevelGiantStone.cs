using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class MoaiLevelGiantStone : CustomLevelSummon
{
    public static new MoaiLevelGiantStone Create(Vector2 pos, float parameter)
    {
        MoaiLevelGiantStone stone = Create<MoaiLevelGiantStone>(prefab);

        stone.transform.position = pos;
        stone.speed = p.stoneSpeed;
        stone.maxHp = p.hp / p.healthMultiplier.GetFloatAt(parameter);
        stone.hp = stone.maxHp;
        stone.baseY = pos.y;

        stone.baseScale = p.stoneScale * p.scaleMultiplier.GetFloatAt(parameter);
        stone.transform.SetScale(stone.baseScale, stone.baseScale);
        stone.OnScaleChanged();

        return stone;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        transform.AddPosition(speed * CupheadTime.FixedDelta);
        if (transform.position.x > 800f)
        {
            Destroy(gameObject);
        }

        transform.AddEulerAngles(0f, 0f, -spinSpeed * CupheadTime.FixedDelta);
    }

    public override void OnDamageTaken(DamageDealer.DamageInfo info)
    {
        base.OnDamageTaken(info);

        float scale = baseScale * hp / maxHp;
        if(scale < 0.25f)
        {
            scale = 0.25f;
        }
        transform.SetScale(scale, scale);
        OnScaleChanged();
    }

    public override void Die()
    {
    }

    private void OnScaleChanged()
    {
        CircleCollider2D circleCollider = prefab.GetComponent<CircleCollider2D>();
        if (circleCollider != null)
        {
            radius = circleCollider.radius * transform.localScale.x;
            spinSpeed = 100f * speed / radius;

            float y = baseY + radius;
            transform.SetPosition(null, y);
        }
        else
        {
            throw new Exception("MoaiLevelGiantStone CircleCollider2D not found in prefab");
        }
    }

    public static GameObject prefab;

    private float speed;
    private float spinSpeed;
    private float radius;
    private float baseY;
    private float baseScale;
    private float maxHp;

    private static MoaiProperties.GiantStone p = new();
}
