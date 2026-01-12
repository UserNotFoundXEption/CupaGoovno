using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class FlowerMiniFlowerSpawn
{
    public void Init()
    {
        On.FlowerLevelMiniFlowerSpawn.Update += Update;
    }

    protected void Update(On.FlowerLevelMiniFlowerSpawn.orig_Update orig, FlowerLevelMiniFlowerSpawn self)
    {
        orig(self);
        if (Flower.phase2)
        {
            self.Die();
            GameObject.Destroy(self.gameObject);
        }
    }
}
