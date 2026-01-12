using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class BeePlatforms
{
    public void Init()
    {
        On.BeeLevelPlatforms.Init += Init;
        On.BeeLevelPlatforms.Randomize += Randomize;
    }

    public void Init(On.BeeLevelPlatforms.orig_Init orig, BeeLevelPlatforms self)
    {
        orig(self);
        pattern = "BRBLBRBRBLBRBRBLBRBRBRBLBRBLBLBLBRBRBRBRBRBLBLBRBR";
        patternId = 0;
        startingPlatforms = 0;
    }

    public void Randomize(On.BeeLevelPlatforms.orig_Randomize orig, BeeLevelPlatforms self, int missingCount)
    {
        foreach (Transform transform in self.rows)
        {
            List<Transform> list = new List<Transform>(transform.GetChildTransforms());
            foreach (Transform transform2 in list)
            {
                transform2.gameObject.SetActive(true);
            }
            for (int j = 0; j < list.Count; j++)//new start
            {
                if (startingPlatforms > 20 && missingCount != -1)
                {
                    list[j].gameObject.SetActive(false);
                }
                startingPlatforms++;
            }
            if (pattern[patternId] == 'L')
            {
                int num = UnityEngine.Random.Range(0, list.Count / 2 + list.Count % 2);
                list[num].gameObject.SetActive(true);
            }
            if (pattern[patternId] == 'R')
            {
                int num = UnityEngine.Random.Range(list.Count / 2, list.Count);
                list[num].gameObject.SetActive(true);
            }
            if (pattern[patternId] == 'B')
            {
                int num = UnityEngine.Random.Range(0, list.Count / 2);
                list[num].gameObject.SetActive(true);
                num = UnityEngine.Random.Range(list.Count / 2, list.Count);
                list[num].gameObject.SetActive(true);
            }
            patternId++;
            if (patternId == 50)
            {
                patternId = 0;
            }//new end

            /*for (int j = 0; j < missingCount; j++)
            {
                if (list.Count <= 1)
                {
                    break;
                }
                int num = UnityEngine.Random.Range(0, list.Count);
                if (num == 0 && BeeLevelPlatforms.lastPlatform == 0)
                {
                    break;
                }
                if (num == 3 && BeeLevelPlatforms.lastPlatform == 2)
                {
                    break;
                }
                list[num].gameObject.SetActive(false);
                BeeLevelPlatforms.lastPlatform = num;
                list.RemoveAt(num);
            }*/
        }
    }

    private static string pattern;
    private static int patternId;
    private static int startingPlatforms;
}
