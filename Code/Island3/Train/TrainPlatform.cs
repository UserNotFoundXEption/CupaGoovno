using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CupaGoovno;

public class TrainPlatform
{
    public void Init()
    {
        On.TrainLevelPlatform.Awake += Awake;
    }

    protected void Awake(On.TrainLevelPlatform.orig_Awake orig, TrainLevelPlatform self)
    {
        orig(self);
        Train.platform = self;
    }
}
