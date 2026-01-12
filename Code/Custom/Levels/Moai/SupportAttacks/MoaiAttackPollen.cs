using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public static class MoaiAttackPollen
{
    public static IEnumerator attack_cr(float parameter)
    {
        /*yield return MoaiLevelWarning.warning_cr(
            new Vector2(-550f, 250f),
            0f,
            new Vector2(0.3f, 0.5f),
            p.warningTime,
            MoaiLevel.moai,
            new UnityEngine.Color(1f, 1f, 0f));*/

        audio = MoaiLevel.moai.gameObject.AddComponent<AudioSource>();
        audio.clip = warning;
        audio.volume = Other.GetSfxVolumeMultiplier();
        audio.Play();

        yield return CupheadTime.WaitForSeconds(MoaiLevel.moai, p.warningTime);

        audio.clip = launch;
        audio.Play();

        List<Vector2> positions = GetPos();
        foreach(Vector2 pos in positions)
        {
            MoaiLevelPollen.Create(pos, parameter);
        }
    }

    private static List<Vector2> GetPos()
    {
        List<Vector2> positions = [];

        float width = p.spawnX.max - p.spawnX.min;
        float height = p.spawnY.max - p.spawnY.min;

        float deltaX = p.spacing;
        float deltaY = p.spacing * Mathf.Sqrt(3f) / 2f;

        int rows = (int)(height / deltaY);
        int cols = (int)(width / deltaX);

        for (int row = 0; row <= rows; row++)
        {
            float y = row * deltaY;
            bool offset = row % 2 == 1;

            for (int col = 0; col <= cols; col++)
            {
                float x = col * deltaX + (offset ? deltaX / 2 : 0);
                if (x <= width && y <= height)
                {
                    positions.Add(new(
                        x + p.spawnX.min + UnityEngine.Random.Range(-p.randomScatter, p.randomScatter),
                        y + p.spawnY.min + UnityEngine.Random.Range(-p.randomScatter, p.randomScatter)));
                }
            }
        }

        return positions;
    }

    public static AudioClip warning;
    public static AudioClip launch;

    private static MoaiProperties.Pollen p = new();
    private static AudioSource audio;
}
