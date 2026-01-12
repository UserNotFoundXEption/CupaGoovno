using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public static class MoaiAttackLaser
{
    public static IEnumerator attack_cr(float parameter)
    {
        audioLeft = laserLeft.AddComponent<AudioSource>();
        audioRight = laserRight.AddComponent<AudioSource>();

        MoaiLevel.moai.StartCoroutine(eyesSfx_cr(0.2f));
        yield return laserEyes_cr(0f, 1f, 0.3f);
        yield return CupheadTime.WaitForSeconds(MoaiLevel.moai, p.initialDelay);

        float laserTime = p.laserTime * p.durationMultiplier.GetFloatAt(parameter);
        float delay = p.delayBetweenShots * p.delayMultiplier.GetFloatAt(parameter);
        int attackCount = (int)Math.Ceiling(p.fullAttackTime / delay);
        for (int i = 0; i < attackCount; i++)
        {
            if (i % 2 == 0)
            {
                MoaiLevel.moai.StartCoroutine(laserLeft_cr(laserTime));
            }
            else
            {
                MoaiLevel.moai.StartCoroutine(laserRight_cr(laserTime));
            }
            yield return CupheadTime.WaitForSeconds(MoaiLevel.moai, delay);
        }

        yield return laserEyes_cr(1f, 0f, 1f);
        yield return CupheadTime.WaitForSeconds(MoaiLevel.moai, p.hesitate);
        UnityEngine.GameObject.Destroy(audioLeft);
        UnityEngine.GameObject.Destroy(audioRight);
        MoaiLevel.moai.state = MoaiLevelMoai.States.Idle;
    }

    private static IEnumerator eyesSfx_cr(float delay)
    {
        yield return CupheadTime.WaitForSeconds(MoaiLevel.moai, delay);

        if (MoaiLevel.ultra)
        {
            Other.PlayCustomSfx(ultrakillThyEnd, 1f);
        }
        else
        {
            Other.PlayCustomSfx(eyes, 0.6f);
        }
    }

    private static IEnumerator laserEyes_cr(float start, float end, float time)
    {
        SpriteRenderer leftEye = eyeLeft.GetComponent<SpriteRenderer>();
        SpriteRenderer rightEye = eyeRight.GetComponent<SpriteRenderer>();
        float t = 0f;
        while (t < time)
        {
            t += CupheadTime.delta;
            float alpha = Mathf.Lerp(start, end, t / time);
            Color color = leftEye.color;
            color.a = Mathf.Clamp01(alpha);
            leftEye.color = color;
            rightEye.color = color;
            yield return null;
        }
    }

    private static IEnumerator laserLeft_cr(float laserTime)
    {
        Vector3 dir = (PlayerManager.GetRandom().center - eyeLeft.transform.position).normalized;
        float angle = Vector3.Angle(Vector3.up, dir) + 180f;
        laserPivotLeft.transform.SetEulerAngles(0f, 0f, angle);
        LaserSwitch(laserWarningLeft, true);
        audioLeft.clip = warning;
        audioLeft.volume = 0.6f * Other.GetSfxVolumeMultiplier();
        audioLeft.Play();

        yield return CupheadTime.WaitForSeconds(MoaiLevel.moai, p.warningTime);
        LaserSwitch(laserWarningLeft, false);
        LaserSwitch(laserLeft, true);
        LaserSparks(eyeLeft.transform.position, angle, -650f);
        audioLeft.clip = fire;
        audioLeft.volume = 0.3f * Other.GetSfxVolumeMultiplier();
        audioLeft.Play();

        yield return CupheadTime.WaitForSeconds(MoaiLevel.moai, laserTime);
        LaserSwitch(laserLeft, false);
    }

    private static IEnumerator laserRight_cr(float laserTime)
    {
        float angle = p.rightAngleRange.RandomFloat();
        laserPivotRight.transform.SetEulerAngles(0f, 0f, angle);
        LaserSwitch(laserWarningRight, true);
        audioRight.clip = warning;
        audioRight.volume = 0.6f * Other.GetSfxVolumeMultiplier();
        audioRight.Play();

        yield return CupheadTime.WaitForSeconds(MoaiLevel.moai, p.warningTime);
        LaserSwitch(laserWarningRight, false);
        LaserSwitch(laserRight, true);
        LaserSparks(eyeRight.transform.position, angle, -650f);
        audioRight.clip = fire;
        audioRight.volume = 0.3f * Other.GetSfxVolumeMultiplier();
        audioRight.Play();

        yield return CupheadTime.WaitForSeconds(MoaiLevel.moai, laserTime);
        LaserSwitch(laserRight, false);
    }

    private static void LaserSwitch(GameObject laser, bool enabled)
    {
        laser.GetComponent<SpriteRenderer>().enabled = enabled;
        BoxCollider2D collider = laser.GetComponent<BoxCollider2D>();
        if (collider != null)
        {
            collider.enabled = enabled;
        }
    }

    private static void LaserSparks(Vector2 startPos, float angle, float targetX)
    {
        angle += 90f;
        while (angle > 180f)
        {
            angle -= 360f;
        }
        while (angle < -180f)
        {
            angle += 360f;
        }

        float angleRad = angle * Mathf.Deg2Rad;
        float slope = Mathf.Tan(angleRad);
        float yIntercept = startPos.y - slope * startPos.x;
        float yIntersection = slope * targetX + yIntercept;

        Vector2 pos = new(targetX, yIntersection);
        float alpha = 90f - angle;
        float angleOne = 90f - alpha / 2;
        float beta = 90f + angle;
        float angleTwo = beta / 2f - 90f;

        if (yIntersection < -150f || yIntersection > 360f)
        {
            return;
        }

        MoaiLevelLaserSpark.Create(angleOne, pos);
        MoaiLevelLaserSpark.Create(angleTwo, pos);
    }

    public static GameObject eyeLeft;
    public static GameObject eyeRight;
    public static GameObject laserLeft;
    public static GameObject laserRight;
    public static GameObject laserWarningLeft;
    public static GameObject laserWarningRight;
    public static GameObject laserPivotLeft;
    public static GameObject laserPivotRight;
    public static AudioClip warning;
    public static AudioClip fire;
    public static AudioClip eyes;
    public static AudioClip ultrakillThyEnd;
    public static Sprite redLaserSprite;
    public static Sprite redLaserWarningSprite;
    public static Sprite redEyeSprite;

    private static MoaiProperties.Laser p = new();
    private static AudioSource audioLeft;
    private static AudioSource audioRight;
}
