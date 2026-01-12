using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static CupaGoovno.MoaiLearningProperties;

namespace CupaGoovno;

public abstract class MoaiChooser : IMoaiChooser
{
    public abstract MoaiAttacks GetNext();

    protected float GetValueForAttacks(int i, int j)
    {
        return learning.table[i][j];
    }

    protected IMoaiLearnerDiscrete learning;
}
