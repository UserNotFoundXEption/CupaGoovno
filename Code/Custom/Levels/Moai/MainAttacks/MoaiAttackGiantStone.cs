using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public static class MoaiAttackGiantStone
{
    public static IEnumerator attack_cr(float parameter)
    {
        MoaiLevel.moai.StartCoroutine(screenShake_cr());

        AudioSource audio = Other.PlayCustomSfx(loop, 1f);
        audio.loop = true;
        audio.volume = 0f;
        float maxVolume = Other.GetSfxVolumeMultiplier();

        if (MoaiLevel.ultra)
        {
            Other.PlayCustomSfx(ultrakillCrush, 1f);
        }

        float t = 0f;
        while(t < p.initialDelay)
        {
            t += CupheadTime.Delta;
            audio.volume = Mathf.Lerp(0f, maxVolume, t / p.initialDelay);
            yield return null;
        }
        audio.volume = maxVolume;

        yield return stone_cr(parameter);
        yield return CupheadTime.WaitForSeconds(MoaiLevel.moai, p.repeatDelay);
        yield return stone_cr(parameter);

        yield return CupheadTime.WaitForSeconds(MoaiLevel.moai, p.hesitate);
        t = 0f;
        while (t < p.initialDelay)
        {
            t += CupheadTime.Delta;
            audio.volume = Mathf.Lerp(maxVolume, 0f, t / p.initialDelay);
            yield return null;
        }

        MoaiLevel.moai.state = MoaiLevelMoai.States.Idle;
    }

    private static IEnumerator stone_cr(float parameter)
    {
        MoaiLevelGiantStone.Create(new Vector2(-1000f, -235f), parameter);
        yield return CupheadTime.WaitForSeconds(MoaiLevel.moai, p.timeToBirdOne);
        MoaiLevelBird.Create(new Vector2(-1000f, 0f));
        yield return CupheadTime.WaitForSeconds(MoaiLevel.moai, p.timeToBirdTwo);
        MoaiLevelBird.Create(new Vector2(-1000f, 0f));
    }

    private static IEnumerator screenShake_cr()
    {
        CupheadLevelCamera.Current.StartShake(6f);
        yield return CupheadTime.WaitForSeconds(MoaiLevel.moai, 1f);
        CupheadLevelCamera.Current.EndShake(1f);
    }

    public static AudioClip loop;
    public static AudioClip ultrakillCrush;

    private static MoaiProperties.GiantStone p = new();
}
