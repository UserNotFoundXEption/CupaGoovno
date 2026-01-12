using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking.Match;

namespace CupaGoovno;

public static class MoaiAttackRockets
{
    public static IEnumerator attack_cr(float parameter)
    {
        if(MoaiLevel.ultra)
        {
            Other.PlayCustomSfx(ultrakillPrepare, 1f);
        }

        MoaiLevel.moai.StartCoroutine(sfxLoopStart_cr());
        yield return CupheadTime.WaitForSeconds(MoaiLevel.moai, p.initialDelay);

        float startY = -200f;
        int possibleYCount = 3;
        float[] possibleY = new float[possibleYCount];
        for(int i = 0; i < possibleYCount; i++)
        {
            possibleY[i] = i * p.yDifference;
        }
        possibleY.Shuffle();

        int j = 0;
        float delay = p.delay / p.delayMultplier.GetFloatAt(parameter);
        int count = (int)Math.Ceiling(p.attackTime/delay);
        for (int i = 0; i < count; i++)
        {
            j++;
            if (j == possibleYCount)
            {
                j = 0;
                possibleY.Shuffle();
            }
            float y = startY + possibleY[j];
            MoaiLevelRocket.Create(new Vector2(800f, y), parameter);
            yield return CupheadTime.WaitForSeconds(MoaiLevel.moai, delay);
        }

        yield return CupheadTime.WaitForSeconds(MoaiLevel.moai, p.hesitate);
        MoaiLevel.moai.state = MoaiLevelMoai.States.Idle;
        MoaiLevel.moai.StartCoroutine(sfxLoopEnd_cr());
    }

    private static IEnumerator sfxLoopStart_cr()
    {
        if(audio != null)
        {
            GameObject.Destroy(audio);
        }
        audio = Other.PlayCustomSfx(loop, 1f);
        audio.loop = true;

        float maxVolume = Other.GetSfxVolumeMultiplier();
        float t = 0f;
        while (t < 1f)
        {
            t += CupheadTime.delta;
            audio.volume = Mathf.Lerp(0f, maxVolume, t);
            yield return null;
        }
    }

    private static IEnumerator sfxLoopEnd_cr()
    {
        float maxVolume = Other.GetSfxVolumeMultiplier();
        float t = 0f;
        while (t < 1)
        {
            t += CupheadTime.delta;
            audio.volume = Mathf.Lerp(maxVolume, 0f, t);
            yield return null;
        }
    }

    public static AudioClip loop;
    public static AudioClip ultrakillPrepare;

    private static MoaiProperties.Rockets p = new();
    private static AudioSource audio;
}
