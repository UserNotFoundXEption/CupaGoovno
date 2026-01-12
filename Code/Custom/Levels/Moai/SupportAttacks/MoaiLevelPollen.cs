using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Bindings;

namespace CupaGoovno;

public class MoaiLevelPollen : CustomProjectile
{
    public static new MoaiLevelPollen Create(Vector2 pos, float parameter)
    {
        MoaiLevelPollen pollen = Create<MoaiLevelPollen>(prefab);

        pollen.transform.position = pos;
        pollen.vel = new(p.spawnVelocity, 0f);
        pollen.drag = p.linearDrag;
        pollen.gravity = p.gravity;
        pollen.spinSpeed = Rand.Bool() ? p.spinSpeed : -p.spinSpeed;
        pollen.bonusX = p.bonusX.GetFloatAt(parameter);

        float scale = p.scale;
        pollen.transform.SetScale(scale, scale, 1f);

        return pollen;
    }

    public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
    {
        base.OnCollisionPlayer(hit, phase);
        Destroy(gameObject);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        transform.AddPosition(vel.x * CupheadTime.FixedDelta, vel.y * CupheadTime.FixedDelta);
        if (transform.position.y <= -450f)
        {
            Destroy(gameObject);
        }

        transform.AddEulerAngles(0f, 0f, spinSpeed * CupheadTime.delta);

        if(bonusX > 0f)
        {
            bonusX -= vel.x * CupheadTime.FixedDelta;
            vel.y -= gravity * CupheadTime.FixedDelta;
        }
        else
        {
            float dragPercent = 1 - drag * CupheadTime.FixedDelta;
            vel.x = vel.x * dragPercent;
            vel.y = vel.y * dragPercent - gravity * CupheadTime.FixedDelta;
        }
    }

    public static GameObject prefab;

    private float drag;
    private Vector2 vel;
    private float gravity;
    private float spinSpeed;
    private float bonusX;

    private static MoaiProperties.Pollen p = new();
}
