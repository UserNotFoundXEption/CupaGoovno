using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CupaGoovno;

public class MoaiLearningProperties
{
    public class EpsilonGreedy
    {
        public float epsilon = 0.9f;
        public float learningRate = 0.1f;
    }

    public class QLearning
    {
        public float learningRate = 0.1f;
        public float learningRateMain = 0.3f;
        public float discountFactor = 0f;
        public float normalizationConstant = 650f;
        public float normalizationScaler = 650f;
    }

    public class Softmax
    {
        public float temperature = 80f;
    }

    public class PolicyGradient
    {
        public class Normal
        {
            public float normalizationConstant = 5000f;
            public float learningRate = 0.5f;
            public float mean = 0.5f;
            public float stddev = 0.2f;
        }

        public class Beta
        {
            public float normalizationConstant = 500f;
            public float learningRate = 3f;
            public float alpha = 1f;
            public float beta = 1f;
        }
    }
}
