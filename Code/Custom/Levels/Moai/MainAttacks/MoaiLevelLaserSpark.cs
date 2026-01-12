using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class MoaiLevelLaserSpark : CustomProjectile
{
    public static MoaiLevelLaserSpark Create(float angle, Vector2 pos)
    {
        MoaiLevelLaserSpark spark = Create<MoaiLevelLaserSpark>(prefab);

        spark.transform.position = pos;
        float vel = p.spawnVelocity.RandomFloat();
        spark.vel = new Vector2(vel * Mathf.Cos(angle * Mathf.Deg2Rad), vel * Mathf.Sin(angle * Mathf.Deg2Rad));
        spark.drag = p.linearDrag;
        spark.gravity = p.gravity;
        spark.spinSpeed = Rand.Bool() ? p.spinSpeed : -p.spinSpeed;
        float scale = p.scale;
        spark.transform.SetScale(scale, scale, 1f);

        return spark;
    }

    public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
    {
        base.OnCollisionPlayer(hit, phase);
        Destroy(gameObject);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        transform.AddEulerAngles(0f, 0f, spinSpeed * CupheadTime.delta);

        transform.AddPosition(vel.x * CupheadTime.FixedDelta, vel.y * CupheadTime.FixedDelta, 0f);
        if (transform.position.y <= -450f)
        {
            Destroy(gameObject);
        }

        float dragPercent = 1 - drag * CupheadTime.FixedDelta;
        vel.x = vel.x * dragPercent;
        vel.y = vel.y * dragPercent - gravity * CupheadTime.FixedDelta;
    }

    public static GameObject prefab;
    public static Sprite redSprite;

    private float drag;
    private Vector2 vel;
    private float gravity;
    private float spinSpeed;

    private static MoaiProperties.LaserSpark p = new();
}
