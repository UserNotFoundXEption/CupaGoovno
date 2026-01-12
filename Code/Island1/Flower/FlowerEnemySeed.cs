using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class FlowerEnemySeed
{
    public void Init()
    {
        On.FlowerLevelEnemySeed.Update += Update;
    }

    protected void Update(On.FlowerLevelEnemySeed.orig_Update orig, FlowerLevelEnemySeed self)
    {
        orig(self);
        if (Flower.phase2)
        {
            self.Die();
            GameObject.Destroy(self.gameObject);
        }
    }
}
