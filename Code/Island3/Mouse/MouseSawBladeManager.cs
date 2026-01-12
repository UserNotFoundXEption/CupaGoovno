using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CupaGoovno;

public class MouseSawBladeManager
{
    public void Init()
    {
        On.MouseLevelSawBladeManager.Leave += Leave;
    }

    public void Leave(On.MouseLevelSawBladeManager.orig_Leave orig, MouseLevelSawBladeManager self)
    {/*
        self.StopAllCoroutines();
        self.leftSawBlades.Leave();
        self.rightSawBlades.Leave();*/
    }
}
