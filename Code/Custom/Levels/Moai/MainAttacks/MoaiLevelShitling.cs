using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;
using static CupaGoovno.MoaiProperties;

namespace CupaGoovno;

public class MoaiLevelShitling : CustomProjectile
{
    public static new MoaiLevelShitling Create(Vector2 pos, float parameter)
    {
        MoaiLevelShitling shitling = Create<MoaiLevelShitling>(prefab);

        shitling.transform.position = pos;
        shitling.speed = p.speed / p.speedMultiplier.GetFloatAt(parameter);
        shitling.dying = false;

        float scale = p.scale * p.scaleMultiplier.GetFloatAt(parameter);
        shitling.transform.SetScale(scale, scale);

        return shitling;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        transform.AddPosition(0f, speed * CupheadTime.FixedDelta);
        if (transform.position.y < Level.Current.Ground && !dying)
        {
            dying = true;
            this.GetComponent<SpriteRenderer>().enabled = false;
            this.GetComponent<BoxCollider2D>().enabled = false;
            this.StartCoroutine(die_cr());
        }
    }

    private IEnumerator die_cr()
    {
        audio = gameObject.AddComponent<AudioSource>();
        audio.volume = 0.3f * Other.GetSfxVolumeMultiplier();
        audio.clip = deaths.RandomChoice();
        audio.Play();
        yield return CupheadTime.WaitForSeconds(this, 3f);
        UnityEngine.GameObject.Destroy(gameObject);
    }

    public static GameObject prefab;
    public static List<AudioClip> deaths = [];

    private float speed;
    private AudioSource audio;
    private bool dying;

    private static MoaiProperties.Shitlings p = new();
}
