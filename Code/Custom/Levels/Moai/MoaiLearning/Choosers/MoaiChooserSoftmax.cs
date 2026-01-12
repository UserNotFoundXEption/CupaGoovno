using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CupaGoovno;

public class MoaiChooserSoftmax : MoaiChooser
{
    public MoaiChooserSoftmax(IMoaiLearnerDiscrete learning)
    {
        base.learning = learning;
        randomChooser = new();
    }

    public override MoaiAttacks GetNext()
    {
        List<SoftmaxCandidate> candidates = GetCandidates();
        List<float> probabilities = GetCandidateProbabilities(candidates);

        float r = UnityEngine.Random.Range(0f, 1f);
        float cumulative = 0f;
        for (int i = 0; i < candidates.Count; i++)
        {
            cumulative += probabilities[i];
            if (r < cumulative)
            {
                LogProbabilities(candidates, probabilities, i);
                return new MoaiAttacks(candidates[i].main, candidates[i].support);
            }
        }

        return randomChooser.GetNext();
    }

    private List<SoftmaxCandidate> GetCandidates()
    {
        List<SoftmaxCandidate> candidates = [];

        Array mainAttacks = Enum.GetValues(typeof(MoaiAttacks.Main));
        Array supportAttacks = Enum.GetValues(typeof(MoaiAttacks.Support));

        for (int i = 0; i < learning.table.Length; i++)
        {
            for (int j = 0; j < learning.table[i].Length; j++)
            {
                float value = GetValueForAttacks(i, j);
                candidates.Add(new SoftmaxCandidate
                {
                    main = (MoaiAttacks.Main)mainAttacks.GetValue(i),
                    support = (MoaiAttacks.Support)supportAttacks.GetValue(j),
                    value = value
                });
            }
        }

        return candidates;
    }

    List<float> GetCandidateProbabilities(List<SoftmaxCandidate> candidates)
    {
        float maxQ = float.MinValue;
        foreach (var c in candidates)
        {
            if (c.value > maxQ)
            {
                maxQ = c.value;
            }
        }

        List<float> probabilities = [];
        float sumExp = 0f;
        foreach (var c in candidates)
        {
            float probability = (float)Math.Exp((c.value - maxQ) / p.temperature);
            probabilities.Add(probability);
            sumExp += probability;
        }

        for (int i = 0; i < probabilities.Count; i++)
        {
            probabilities[i] /= sumExp;
        }

        LogProbabilitiesTable(probabilities);
        return probabilities;
    }

    private void LogProbabilities(List<SoftmaxCandidate> candidates, List<float> probabilities, int candidateId)
    {
        MoaiUtils.CsvLogMessage("Softmax chooses action at temperature = " + p.temperature + " with chance of " + probabilities[candidateId], false, false);

        var selected = candidates[candidateId];
        float rowSum = 0f;
        float columnSum = 0f;

        for (int i = 0; i < candidates.Count; i++)
        {
            if (candidates[i].main == selected.main)
                rowSum += probabilities[i];
            if (candidates[i].support == selected.support)
                columnSum += probabilities[i];
        }

        MoaiUtils.CsvLogMessage($"Softmax selected: Main = {selected.main} ({candidateId/5}), Support = {selected.support} ({candidateId%5})", false, false);
        MoaiUtils.CsvLogMessage($"Chance for main: {rowSum:F4}", false, false);
        MoaiUtils.CsvLogMessage($"Chance for support: {columnSum:F4}", false, true);
    }

    private void LogProbabilitiesTable(List<float> probabilities)
    {
        float[][] table = new float[5][];
        for (int i = 0; i < 5; i++)
        {
            table[i] = new float[5];
            for (int j = 0; j < 5; j++)
            {
                table[i][j] = probabilities[i * 5 + j];
            }
        }

        MoaiUtils.CsvLogTable(table, "Softmax probabilities table:", false, true);
    }

    private static MoaiLearningProperties.Softmax p = new();

    private MoaiChooserRandom randomChooser;

    private class SoftmaxCandidate
    {
        public MoaiAttacks.Main main;
        public MoaiAttacks.Support support;
        public float value;
    }
}
