using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class MoaiLevelRocket : CustomProjectile
{
    public static new MoaiLevelRocket Create(Vector2 pos, float parameter)
    {
        MoaiLevelRocket rocket = Create<MoaiLevelRocket>(prefab);

        rocket.transform.position = pos;
        rocket.speed = p.speed * p.speedMultiplier.GetFloatAt(parameter);

        float scale = p.scale;
        rocket.transform.SetScale(scale, scale);

        rocket.transform.SetEulerAngles(null, null, 90f);

        return rocket;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        transform.AddPosition(speed * CupheadTime.FixedDelta, 0f);
        if (transform.position.x < Level.Current.Left - 200f)
        {
            Destroy(gameObject);
        }
    }

    public static GameObject prefab;

    private float speed;

    private static MoaiProperties.Rockets p = new();
}
