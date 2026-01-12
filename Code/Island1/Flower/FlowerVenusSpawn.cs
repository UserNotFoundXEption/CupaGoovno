using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class FlowerVenusSpawn
{
    public void Init()
    {
        On.FlowerLevelVenusSpawn.Update += Update;
    }

    protected void Update(On.FlowerLevelVenusSpawn.orig_Update orig, FlowerLevelVenusSpawn self)
    {
        orig(self);
        if (Flower.phase2)
        {
            self.Die();
            GameObject.Destroy(self.gameObject);
        }
    }
}
