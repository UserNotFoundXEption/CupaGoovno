using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CupaGoovno;

public class MoaiPGUniform : MoaiPolicyGradient
{
    public MoaiPGUniform()
    {
        this.random = new();
    }

    protected override float Sample()
    {
        return UnityEngine.Random.Range(0f, 1f);
    }
}
