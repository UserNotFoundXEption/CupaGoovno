using Rewired;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static CupaGoovno.MoaiLearningProperties.PolicyGradient;

namespace CupaGoovno;

public abstract class MoaiLearnerDiscrete : IMoaiLearnerDiscrete
{
    public MoaiLearnerDiscrete()
    {
        Array arrayMain = Enum.GetValues(typeof(MoaiAttacks.Main));
        Array arraySupport = Enum.GetValues(typeof(MoaiAttacks.Support));
        int mainLength = arrayMain.Length;
        int supportLength = arraySupport.Length;
        table = new float[mainLength][];
        for (int i = 0; i < mainLength; i++)
        {
            table[i] = new float[supportLength];
            for (int j = 0; j < supportLength; j++)
            {
                table[i][j] = 0f;
            }
        }
    }

    public void SaveHealthInfo()
    {
        bossHp = MoaiLevel.moai.hp;
        playerHp = PlayerManager.GetFirst().stats.Health;
    }

    public float GetReward(bool heart)
    {
        float bossHpDelta = bossHp - MoaiLevel.moai.hp;
        float playerHpDelta = playerHp - PlayerManager.GetFirst().stats.Health;
        float reward = playerHpDelta * 2000f - bossHpDelta;
        if (heart)
        {
            reward += 1000f;
        }

        return reward;
    }

    public abstract void Update(MoaiAttacks attacks, bool heart);

    public float[][] table { get; set; }
    public float bossHp { get; set; }
    public float playerHp { get; set; }
}
