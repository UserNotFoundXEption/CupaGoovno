using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CupaGoovno;

public interface IMoaiLearnerContinous
{
    public float GetNext();
    public void Update(float reward, float actionValue);
}
