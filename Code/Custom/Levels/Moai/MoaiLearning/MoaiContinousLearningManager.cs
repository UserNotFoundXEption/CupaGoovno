using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CupaGoovno;

public class MoaiContinousLearningManager
{
    public MoaiContinousLearningManager(MoaiLearnersContinous learnerContinousEnum, IMoaiLearnerDiscrete learnerDiscrete)
    {
        mainLearners = [];
        supportLearners = [];
        for (int i = 0; i < 5; i++)
        {
            MoaiAttacks.Main main = (MoaiAttacks.Main)i;
            MoaiAttacks.Support support = (MoaiAttacks.Support)i;
            switch (learnerContinousEnum)
            {
                case MoaiLearnersContinous.PolicyGradientUniform:
                    mainLearners.Add(main, new MoaiPGUniform());
                    supportLearners.Add(support, new MoaiPGUniform());
                    break;
                case MoaiLearnersContinous.PolicyGradientNormal:
                    mainLearners.Add(main, new MoaiPGNormal(learnerDiscrete, pNormal.mean, pNormal.stddev));
                    supportLearners.Add(support, new MoaiPGNormal(learnerDiscrete, pNormal.mean, pNormal.stddev));
                    break;
                case MoaiLearnersContinous.PolicyGradientBeta:
                    mainLearners.Add(main, new MoaiPGBeta(learnerDiscrete, 0f, 1f, pBeta.alpha, pBeta.beta));
                    supportLearners.Add(support, new MoaiPGBeta(learnerDiscrete, 0f, 1f, pBeta.alpha, pBeta.beta));
                    break;
            }
        }
    }

    public float GetNext(MoaiAttacks.Main main)
    {
        return mainLearners[main].GetNext();
    }

    public float GetNext(MoaiAttacks.Support support)
    {
        return supportLearners[support].GetNext();
    }

    public void Update(MoaiAttacks.Main main, float reward, float actionValue)
    {
        mainLearners[main].Update(reward, actionValue);
    }

    public void Update(MoaiAttacks.Support support, float reward, float actionValue)
    {
        supportLearners[support].Update(reward, actionValue);
    }

    public static Dictionary<MoaiAttacks.Main, IMoaiLearnerContinous> mainLearners = [];
    public static Dictionary<MoaiAttacks.Support, IMoaiLearnerContinous> supportLearners = [];
 
    private MoaiLearningProperties.PolicyGradient.Normal pNormal = new();
    private MoaiLearningProperties.PolicyGradient.Beta pBeta = new();
}
