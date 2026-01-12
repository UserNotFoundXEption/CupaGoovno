using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Bindings;

namespace CupaGoovno;

public class MoaiLevelBouncer : CustomLevelSummon
{
    public static new MoaiLevelBouncer Create(Vector2 pos, float parameter)
    {
        MoaiLevelBouncer bouncer = Create<MoaiLevelBouncer>(prefab);

        bouncer.transform.position = pos;
        bouncer.xSpeed = p.xSpeed;
        bouncer.gravity = p.gravity * p.gravityMultiplier.GetFloatAt(parameter);
        bouncer.maxHp = p.hp / p.hpMultiplier.GetFloatAt(parameter);
        bouncer.hp = bouncer.maxHp;

        float scale = p.scale;
        bouncer.transform.SetScale(scale, scale);

        bouncer.transform.SetEulerAngles(null, null, UnityEngine.Random.Range(0f, 360f));

        return bouncer;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        transform.AddPosition(xSpeed * CupheadTime.FixedDelta, ySpeed * CupheadTime.FixedDelta);
        if (transform.position.x < Level.Current.Left - 200f)
        {
            Destroy(gameObject);
        }

        ySpeed -= gravity * CupheadTime.FixedDelta;
        if(transform.position.y < Level.Current.Ground && ySpeed < 0f)
        {
            ySpeed = Mathf.Abs(ySpeed);
            if(audio == null)
            {
                audio = gameObject.AddComponent<AudioSource>();
                audio.volume = 0.8f * Other.GetSfxVolumeMultiplier();
            }
            audio.clip = bounces.RandomChoice();
            audio.Play();
        }

        transform.AddEulerAngles(0f, 0f, 100f * CupheadTime.FixedDelta);
    }

    public override void OnDamageTaken(DamageDealer.DamageInfo info)
    {
        base.OnDamageTaken(info);

        float scaleMultiplier = hp / maxHp * 0.5f + 0.5f;
        float scale = p.scale * scaleMultiplier;
        transform.SetScale(scale, scale);
    }

    public override void Die()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        StartCoroutine(die_cr());
    }

    private IEnumerator die_cr()
    {
        audio = gameObject.AddComponent<AudioSource>();
        audio.volume = Other.GetSfxVolumeMultiplier();
        audio.clip = death;
        audio.Play();
        yield return CupheadTime.WaitForSeconds(this, 3f);
        UnityEngine.GameObject.Destroy(gameObject);
    }

    public static GameObject prefab;
    public static List<AudioClip> bounces = [];
    public static AudioClip death;

    private float xSpeed;
    private float ySpeed;
    private float gravity;
    private float maxHp;
    private AudioSource audio;

    private static MoaiProperties.Bouncers p = new();
}
