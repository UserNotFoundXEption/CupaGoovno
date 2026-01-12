using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CupaGoovno;

public class MoaiQLearning : MoaiLearnerDiscrete
{
    public MoaiQLearning() : base()
    {
    }

    public override void Update(MoaiAttacks attack, bool heart)
    {
        float reward = GetReward(heart);
        float rewardNormalized = Normalize(reward);

        int mainIndex = (int)attack.main;
        int supportIndex = (int)attack.support;

        UpdateTable(rewardNormalized, mainIndex, supportIndex);

        string message = $"Reward = {reward}, Normalized reward = {rewardNormalized}, Main = {attack.main} ({mainIndex}), Support = {attack.support} ({supportIndex}), Updated table:";
        MoaiUtils.CsvLogTable(table, message, false, true);
    }

    private void UpdateTable(float reward, int mainIndex, int supportIndex)
    {
        for(int i = 0;i < table.Length; i++)
        {
            for (int j = 0; j < table[i].Length; j++)
            {
                if (i != mainIndex && j != supportIndex)
                {
                    continue;
                }
                if (i == mainIndex && j == supportIndex)
                {
                    UpdateCell(i, j, p.learningRateMain, reward);
                    continue;
                }
                UpdateCell(i, j, p.learningRate, reward);
            }
        }
    }

    private void UpdateCell(int i, int j, float learningRate, float reward)
    {
        float originalPart = table[i][j] * (1 - learningRate);
        float rewardPart = reward * learningRate;
        float discountedPart = p.discountFactor * MoaiUtils.GetMaxFromTable(table) * learningRate;

        table[i][j] = originalPart + rewardPart + discountedPart;
    }


    private float Normalize(float reward)
    {
        return (float)Math.Tanh(reward / p.normalizationConstant) * p.normalizationScaler;
    }

    private static MoaiLearningProperties.QLearning p = new();
}
