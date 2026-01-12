using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;

namespace CupaGoovno;

public abstract class MoaiPolicyGradient : IMoaiLearnerContinous
{
    protected abstract float Sample();

    public virtual float GetNext()
    {
        return Sample();
    }

    public virtual void Update(float reward, float actionValue)
    {
    }

    protected Random random;
}
