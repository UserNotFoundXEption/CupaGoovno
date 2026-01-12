using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking.Match;

namespace CupaGoovno;

public static class MoaiAttackShitlings
{
    public static IEnumerator attack_cr(float parameter)
    {
        if (MoaiLevel.ultra)
        {
            Other.PlayCustomSfx(ultrakillDie, 1f);
        }
        else
        {
            Other.PlayCustomSfx(falling, 1.2f);
        }

        yield return CupheadTime.WaitForSeconds(MoaiLevel.moai, p.initialDelay);

        float startX = -620f;
        int possibleXCount = 10;
        List<float> possibleX = [];
        for (int i = 0; i < possibleXCount; i++)
        {
            possibleX.Add(i * 100f);
        }
        possibleX = ShuffleList(possibleX);

        int count = (int)(p.count / p.countMultiplier.GetFloatAt(parameter));
        float delay = p.delay * p.countMultiplier.GetFloatAt(parameter);
        int j = 0;
        for (int i = 0; i < count; i++)
        {
            j++;
            if (j == possibleXCount)
            {
                j = 0;
                possibleX = ShuffleList(possibleX);
            }
            float x = startX + possibleX[j];
            MoaiLevelShitling.Create(new Vector2(x, 450f), parameter);
            yield return CupheadTime.WaitForSeconds(MoaiLevel.moai, delay);
        }

        yield return CupheadTime.WaitForSeconds(MoaiLevel.moai, p.hesitate);
        MoaiLevel.moai.state = MoaiLevelMoai.States.Idle;
    }

    private static List<float> ShuffleList(List<float> possibleX)
    {
        List<float> shuffledList = [];
        while(shuffledList.Count < possibleX.Count)
        {
            possibleX.Shuffle();
            shuffledList.Add(possibleX[0]);

            for (int i = 1; i < possibleX.Count; i++)
            {
                float lastX1 = shuffledList[shuffledList.Count - 1];
                float lastX2 = int.MaxValue;
                if(shuffledList.Count > 1)
                {
                    lastX2 = shuffledList[shuffledList.Count - 2];
                }

                for (int j = 0; j < possibleX.Count; j++)
                {
                    float x = possibleX[j];
                    float diff1 = Mathf.Abs(x - lastX1);
                    float diff2 = Mathf.Abs(x - lastX2);
                    if (!shuffledList.Contains(x) && diff1 >= 200f && diff2 >= 200f)
                    {
                        shuffledList.Add(x);
                        break;
                    }
                }
            }

            if(shuffledList.Count < possibleX.Count)
            {
                shuffledList = [];
            }
        }

        return shuffledList;
    }

    public static AudioClip falling;
    public static AudioClip ultrakillDie;

    private static MoaiProperties.Shitlings p = new();
}
