using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class MoaiAttackPusher
{
    public static IEnumerator attack_cr(float parameter)
    {
        yield return CupheadTime.WaitForSeconds(MoaiLevel.moai, p.initialDelay);

        if (MoaiLevel.ultra)
        {
            Other.PlayCustomSfx(ultrakillJudgement, 1f);
        }

        yield return MoaiLevelWarning.warning_cr(
            new Vector2(-550f, 0f),
            0f,
            new Vector2(0.3f, 1f),
            1f,
            MoaiLevel.moai,
            new Color(1f, 0f, 1f));

        MoaiLevelPusher pusher = MoaiLevelPusher.Create(new Vector2(-800f, 0f), parameter);
        while(pusher != null && pusher.gameObject != null)
        {
            yield return null;
        }

        MoaiLevel.moai.state = MoaiLevelMoai.States.Idle;
    }

    public static AudioClip ultrakillJudgement;

    private static MoaiProperties.Pusher p = new();
}
