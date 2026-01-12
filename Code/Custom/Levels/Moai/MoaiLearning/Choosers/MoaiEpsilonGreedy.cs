using System;

namespace CupaGoovno;

public class MoaiEpsilonGreedy : MoaiChooser
{
    public MoaiEpsilonGreedy(IMoaiLearnerDiscrete learning)
    {
        base.learning = learning;
        this.epsilon = p.epsilon;
        randomChooser = new();
    }

    public override MoaiAttacks GetNext()
    {
        MoaiAttacks result = randomChooser.GetNext();

        if (UnityEngine.Random.Range(0f, 1f) < epsilon)
        {
            MoaiUtils.CsvLogMessage("Epsilon chooses random attack with epsilon = " + epsilon, false, true);
        }
        else
        {
            MoaiUtils.CsvLogMessage("Epsilon chooses best attack with epsilon = " + epsilon, false, true);
            float highestValue = int.MinValue;
            Array mainAttacks = Enum.GetValues(typeof(MoaiAttacks.Main));
            Array supportAttacks = Enum.GetValues(typeof(MoaiAttacks.Support));
            MoaiAttacks.Main bestMain = (MoaiAttacks.Main)mainAttacks.GetValue(0);
            MoaiAttacks.Support bestSupport = (MoaiAttacks.Support)supportAttacks.GetValue(0);

            for (int i = 0; i < learning.table.Length; i++)
            {
                for (int j = 0; j < learning.table[i].Length; j++)
                {
                    float currentValue = GetValueForAttacks(i, j);

                    if (currentValue > highestValue)
                    {
                        highestValue = currentValue;
                        bestMain = (MoaiAttacks.Main)mainAttacks.GetValue(i);
                        bestSupport = (MoaiAttacks.Support)supportAttacks.GetValue(j);
                    }
                }
            }

            result = new MoaiAttacks(bestMain, bestSupport);
        }

        epsilon *= (1 - p.learningRate);
        return result;
    }

    private float epsilon;
    private MoaiChooserRandom randomChooser;

    private static MoaiLearningProperties.EpsilonGreedy p = new();
}
