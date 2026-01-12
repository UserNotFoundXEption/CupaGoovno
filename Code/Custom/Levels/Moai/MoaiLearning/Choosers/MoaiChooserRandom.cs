using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CupaGoovno;

public class MoaiChooserRandom : IMoaiChooser
{
    public MoaiAttacks GetNext()
    {
        Array values = Enum.GetValues(typeof(MoaiAttacks.Main));
        Random random = new();
        MoaiAttacks.Main randomMain = (MoaiAttacks.Main)values.GetValue(random.Next(values.Length));
        values = Enum.GetValues(typeof(MoaiAttacks.Support));
        MoaiAttacks.Support randomSupport = (MoaiAttacks.Support)values.GetValue(random.Next(values.Length));
        return new MoaiAttacks(randomMain, randomSupport);
    }
}
