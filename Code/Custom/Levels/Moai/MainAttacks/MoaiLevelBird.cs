using BepInEx;
using System;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;
using static CupaGoovno.MoaiProperties;

namespace CupaGoovno;

public class MoaiLevelBird : CustomProjectile
{
    public static new MoaiLevelBird Create(Vector2 pos)
    {
        MoaiLevelBird bird = Create<MoaiLevelBird>(prefab);

        bird.transform.position = pos;
        bird.speed = p.birdSpeed;

        float scale = p.birdScale;
        bird.transform.SetScale(scale, scale);

        bird.SetParryable(true);

        return bird;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        transform.AddPosition(speed * CupheadTime.FixedDelta);
        if (transform.position.x > 800f)
        {
            Destroy(gameObject);
        }
    }

    public override void OnParryDie()
    {
        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        StartCoroutine(die_cr());
    }

    private IEnumerator die_cr()
    {
        audio = gameObject.AddComponent<AudioSource>();
        audio.volume = 0.8f * Other.GetSfxVolumeMultiplier();
        audio.clip = death;
        audio.Play();
        yield return CupheadTime.WaitForSeconds(this, 3f);
        base.OnParryDie();
    }

    public static GameObject prefab;
    public static AudioClip death;

    private float speed;
    private AudioSource audio;

    private static MoaiProperties.GiantStone p = new();
}
