using System;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Bindings;
using static CupaGoovno.MoaiProperties;

namespace CupaGoovno;

public class MoaiLevelCrackhead : CustomLevelSummon
{
    public static new MoaiLevelCrackhead Create(Vector2 pos, float parameter)
    {
        MoaiLevelCrackhead crackhead = Create<MoaiLevelCrackhead>(prefab);

        crackhead.transform.position = pos;
        crackhead.baseAngle = 0f;
        crackhead.maxHp = p.hp / p.hpMultiplier.GetFloatAt(parameter);
        crackhead.hp = crackhead.maxHp;
        crackhead.speed = p.speed * p.speedMultiplier.GetFloatAt(parameter);

        float scale = p.introScale;
        crackhead.transform.SetScale(scale, scale);

        crackhead.StartCoroutine(crackhead.intro_cr(parameter));

        return crackhead;
    }

    private IEnumerator intro_cr(float parameter)
    {
        yield return new WaitForSeconds(p.initialDelay);

        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        while(collider == null)
        {
            yield return null;
            collider = GetComponent<BoxCollider2D>();
        }
        collider.enabled = false;

        Vector2 originalPos = transform.position;
        center = transform.position;

        float x = 800f;
        float t = 0f;
        float introSpeed = p.introSpeed * (p.speedMultiplier.GetFloatAt(parameter) / 2 + 0.5f);
        while (center.x > -800f)
        {
            x -= CupheadTime.delta * introSpeed;
            t += CupheadTime.delta;

            //y = 3.64 * 10^-11 * x^4 - 2.64 * 10^-7 * x^3 + 5.1 * 10^-5 * x^2 + 0.17 * x - 50
            double a = 3.64 * Math.Pow(10, -11);
            double b = 2.64 * Math.Pow(10, -7);
            double c = 5.1 * Math.Pow(10, -5);
            double d = 0.17;
            double e = -50;
            double x2 = x * x;
            double x3 = x2 * x;
            double x4 = x3 * x;
            double y = a * x4 - b * x3 + c * x2 + d * x + e;

            center = new Vector2(x, (float)y);
            Wiggle(t, true);
            yield return null;
        }

        scale = p.scale;
        transform.SetScale(-scale, scale);

        transform.position = originalPos;
        center = originalPos;

        collider.enabled = true;
        GetComponent<SpriteRenderer>().sortingOrder = 11;
        StartCoroutine(move_cr());
    }

    private IEnumerator move_cr()
    {
        float t = 0f;
        for(; ; )
        {
            t += CupheadTime.delta;
            hp -= CupheadTime.delta * maxHp / p.time;

            while (baseAngle < 0f)
            {
                baseAngle += 360f;
            }

            float x = Mathf.Cos(Mathf.Deg2Rad * baseAngle);
            float y = Mathf.Sin(Mathf.Deg2Rad * baseAngle);
            float delta = CupheadTime.delta * speed;
            center += new Vector2(x * delta, y * delta);

            bool tooFarLeft = baseAngle == 180f && center.x < -570f;
            bool tooFarRight = baseAngle == 0f && center.x > 570f;
            bool tooFarDown = baseAngle == 270f && center.y < -150f;
            bool tooFarUp = baseAngle == 90f && center.y > 300f;
            if (tooFarLeft || tooFarRight || tooFarUp || tooFarDown)
            {
                if (hp > 0f)
                {
                    baseAngle += 90f;
                }
                else
                {
                    GameObject.Destroy(gameObject, 5f);
                }
            }

            Wiggle(t, false);
            yield return null;
        }
    }

    private void Wiggle(float t, bool intro)
    {
        float delta = Mathf.Abs(Mathf.Sin(t * p.wiggleSpeed)) * p.wiggleAmount;
        if (intro)
        {
            delta *= p.introScale;
        }
        else
        {
            delta *= scale;
        }

        float x = -Mathf.Sin(baseAngle);
        float y = Mathf.Cos(baseAngle);
        transform.position = center + new Vector2(delta * x, delta * y);

        float angle = Mathf.Sin(t * p.wiggleSpeed) * p.wiggleRotation;
        transform.SetEulerAngles(0f, 0f, angle + baseAngle);
    }

    public override void Die()
    {
    }

    public static GameObject prefab;

    private Vector2 center;
    private float baseAngle;
    private float scale;
    private float speed;
    private float maxHp;

    private static MoaiProperties.Crackhead p = new();
}
