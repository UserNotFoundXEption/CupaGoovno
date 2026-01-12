using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public static class MoaiAttackSpikes
{
    public static IEnumerator attack_cr(float parameter)
    {
        float spikeXDelta = p.spikeXDelta.GetFloatAt(parameter);
        float[] spikesX = [
            p.middleSpikeX - spikeXDelta,
            p.middleSpikeX,
            p.middleSpikeX + spikeXDelta];

        foreach(float x in spikesX)
        {
            GameObject warning = MoaiLevelWarning.Create(
                new Vector2(x, -150),
                90f,
                new Vector2(0.15f, 0.1f));
            warning.GetComponent<SpriteRenderer>().sortingOrder = 8;
        }

        yield return CupheadTime.WaitForSeconds(MoaiLevel.moai, p.warningTime);
        MoaiLevelWarning.DestroyAll();

        foreach(float x in spikesX)
        {
            MoaiLevelSpike.Create(x);
        }
    }

    private static MoaiProperties.Spikes p = new();
}
