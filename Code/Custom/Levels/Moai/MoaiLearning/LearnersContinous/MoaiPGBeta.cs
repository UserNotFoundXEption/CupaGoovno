using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class MoaiPGBeta : MoaiPGNormal
{
    public MoaiPGBeta(IMoaiLearnerDiscrete learner, float mean, float stddev, float alpha, float beta)
    : base(learner, mean, stddev)
    {
        this.alpha = alpha;
        this.beta = beta;
    }

    protected override float Sample()
    {
        double x = SampleGamma(alpha);
        double y = SampleGamma(beta);
        return (float)(x / (x + y));
    }

    private double SampleGamma(float shape)
    {
        if (shape < 1)
        {
            double randomSample = random.NextDouble();
            return SampleGamma(shape + 1) * Math.Pow(randomSample, 1.0 / shape);
        }

        double d = shape - 1.0 / 3.0;
        double c = 1.0 / Math.Sqrt(9.0 * d);
        while (true)
        {
            double normalSample = base.Sample();
            double v = 1.0 + c * normalSample;
            if (v <= 0) continue;
            v = v * v * v;
            double randomSample = random.NextDouble();
            if (randomSample < 1.0 - 0.0331 * (normalSample * normalSample) * (normalSample * normalSample)) return d * v;
            if (Math.Log(randomSample) < 0.5 * normalSample * normalSample + d * (1.0 - v + Math.Log(v))) return d * v;
        }
    }

    public override void Update(float reward, float actionValue)
    {
        float x = Mathf.Clamp(actionValue, 0.05f, 0.95f);
        
        float advantage = (float)Math.Tanh(reward / pBeta.normalizationConstant);

        float oldApha = alpha;
        float oldBeta = beta;

        //float dAlpha = (Digamma(alpha + beta) - Digamma(alpha) + Mathf.Log(x));
        //float dBeta = (Digamma(alpha + beta) - Digamma(beta) + Mathf.Log(1.0f - x));
        float dAlpha = Mathf.Log(x, 10) + 0.5f;
        float dBeta = Mathf.Log(1 - x, 10) + 0.5f;

        alpha += pBeta.learningRate * advantage * dAlpha;
        beta += pBeta.learningRate * advantage * dBeta;

        alpha = Mathf.Max(0.01f, alpha);
        beta = Mathf.Max(0.01f, beta);

        //Plugin.Log($"Reward: {reward}, advantage: {advantage}, value: {actionValue}, alpha: {oldApha} -> {alpha}, beta: {oldBeta} -> {beta}");
    }

    public float alpha;
    public float beta;
    
    private MoaiLearningProperties.PolicyGradient.Beta pBeta = new();
}
