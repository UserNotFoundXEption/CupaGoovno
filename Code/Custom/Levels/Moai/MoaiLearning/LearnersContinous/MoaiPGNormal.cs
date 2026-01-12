using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class MoaiPGNormal : MoaiPolicyGradient
{
    public MoaiPGNormal(IMoaiLearnerDiscrete learner, float mean, float stddev)
    {
        this.learner = learner;
        this.random = new();
        this.mean = mean;
        this.stddev = stddev;
        this.pNormal = new();
    }

    protected override float Sample()
    {
        double randomUniform1 = 1 - random.NextDouble();
        double randomUniform2 = 1 - random.NextDouble();
        double randomNormal = Math.Sqrt(-2.0 * Math.Log(randomUniform1)) * Math.Sin(2.0 * Math.PI * randomUniform2);
        return mean + stddev * (float)randomNormal;
    }

    public override float GetNext()
    {
        float actionValue;
        do
        {
            actionValue = Sample();
        } while (actionValue < 0f || actionValue > 1f);
        return actionValue;
    }

    public override void Update(float reward, float actionValue)
    {
        float advantage = (float)Math.Tanh(reward / pNormal.normalizationConstant);

        float dMean = (actionValue - mean) / (stddev * stddev);
        float dStddev = ((actionValue - mean) * (actionValue - mean) - stddev * stddev) / (stddev * stddev * stddev);

        mean += pNormal.learningRate * advantage * dMean;
        stddev += pNormal.learningRate * advantage * dStddev;

        mean = Mathf.Clamp(mean, 0.1f, 0.9f);
        stddev = Mathf.Clamp(stddev, 0.2f, 1f);

        Plugin.Log($"actionValue: {actionValue}, reward: {reward}, advantage: {advantage}, mean: {mean}, stddev: {stddev}");
    }

    protected float mean;
    protected float stddev;
    protected IMoaiLearnerDiscrete learner;
    protected bool actionIsMain;
    protected int actionId;
    protected MoaiLearningProperties.PolicyGradient.Normal pNormal;
}
