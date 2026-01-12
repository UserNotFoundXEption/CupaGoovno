using System;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class MoaiLevelPusher : CustomLevelSummon
{
    public static new MoaiLevelPusher Create(Vector2 pos, float parameter)
    {
        MoaiLevelPusher pusher = Create<MoaiLevelPusher>(prefab);

        pusher.transform.position = pos;
        pusher.velocity = 0f;
        pusher.damageMultiplier = 1f;
        pusher.pushes = 0;
        pusher.acceleration = p.startingAcceleration * p.startingAccelerationMultiplier.GetFloatAt(parameter);
        pusher.accelerationIncrease = p.accelerationIncrease / p.accelerationIncreaseMultiplier.GetFloatAt(parameter);

        float scale = p.scale;
        pusher.transform.SetScale(scale, scale);

        ParrySwitch parry = pusher.gameObject.AddComponent<ParrySwitch>();
        parry.enabled = true;
        parry.OnActivate += pusher.OnParry;

        pusher.tag = "Enemy";
        pusher.hitFlash = pusher.gameObject.GetComponent<HitFlash>();

        pusher.StartCoroutine(pusher.damageImmunity_cr());

        return pusher;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (pushes >= p.pushes)
        {
            if(transform.position.x < Level.Current.Left - 200f)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            if(transform.position.x < Level.Current.Left - 200f && velocity < 0f)
            {
                velocity = 0f;
            }
            if(transform.position.x > Level.Current.Right + 200f)
            {
                transform.SetPosition(Level.Current.Left - 200f);
                velocity = 0f;
                pushes++;
                if(pushes >= p.pushes)
                {
                    Destroy(gameObject);
                }
            }
            velocity += acceleration * CupheadTime.FixedDelta;
        }
        transform.AddPosition(velocity * CupheadTime.FixedDelta, 0f, 0f);
    }

    private IEnumerator damageImmunity_cr()
    {
        yield return CupheadTime.WaitForSeconds(this, p.timeToDamageImmunity);

        float t = 0f;
        while(t < p.damageImmunityTransitionTime)
        {
            t += CupheadTime.Delta;
            damageMultiplier = Mathf.Lerp(1f, 0f, t / p.damageImmunityTransitionTime);
            GetComponent<SpriteRenderer>().color = new Color(1f, damageMultiplier, damageMultiplier);
            hitFlash.damageColor = new Color(1f, damageMultiplier * 0.75f, damageMultiplier * 0.75f);
            yield return null;
        }
    }

    public void OnParry()
    {
        if(velocity > 0f)
        {
            Other.Player().stats.SuperChangedFromParry(0.2f);
            velocity = p.velocityAfterParry;
            acceleration += accelerationIncrease;
            pushes++;
        }
    }

    public override void OnDamageTaken(DamageDealer.DamageInfo info)
    {
        velocity -= damageMultiplier * info.damage * p.velocityDamageMultiplier;
    }

    public static GameObject prefab;

    private float velocity;
    private float acceleration;
    private float accelerationIncrease;
    private float damageMultiplier;
    private int pushes;
    private HitFlash hitFlash;

    private static MoaiProperties.Pusher p = new();
}
