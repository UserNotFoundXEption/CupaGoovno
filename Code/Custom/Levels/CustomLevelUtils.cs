using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public static class CustomLevelUtils
{
    public static IEnumerator endMusic_cr(AudioSource theme, float multiplier = 1f)
    {
        float t = 0f;
        while (t < 3f)
        {
            t += Time.deltaTime;
            theme.volume = Mathf.Lerp(Other.GetMusicVolumeMultiplier() * multiplier, 0f, t / 3f);
            yield return null;
        }
    }
}
