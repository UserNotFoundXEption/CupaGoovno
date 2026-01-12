using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CupaGoovno;

public class KingDiceGameInfo
{
    public void Init()
    {
        On.DicePalaceMainLevelGameInfo.ChooseHearts += ChooseHearts;
    }

    private static void ChooseHearts(On.DicePalaceMainLevelGameInfo.orig_ChooseHearts orig)
    {
        /*DicePalaceMainLevelGameInfo.HEART_INDEXES[0] = UnityEngine.Random.Range(0, 3);
        DicePalaceMainLevelGameInfo.HEART_INDEXES[1] = UnityEngine.Random.Range(4, 7);
        DicePalaceMainLevelGameInfo.HEART_INDEXES[2] = UnityEngine.Random.Range(8, 11);*/
        int[] noHeart = new int[3];
        int[] battleIndexes = [0, 1, 2, 4, 5, 6, 8, 9, 10];
        DicePalaceMainLevelGameInfo.HEART_INDEXES = new int[6];
        int h = 0;
        noHeart[0] = UnityEngine.Random.Range(0, 3);
        noHeart[1] = UnityEngine.Random.Range(4, 7);
        noHeart[2] = UnityEngine.Random.Range(8, 11);
        for (int i = 0; i < 11; i++)
        {
            if (battleIndexes.Contains(i) && !noHeart.Contains(i))
            {
                DicePalaceMainLevelGameInfo.HEART_INDEXES[h] = i;
                h++;
            }
        }
    }
}
