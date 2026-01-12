using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class ClownDogBalloon
{
    public void Init()
    {
        On.ClownLevelDogBalloon.Update += Update;
    }

    protected void Update(On.ClownLevelDogBalloon.orig_Update orig, ClownLevelDogBalloon self)
    {
        orig(self);
        if (self.transform.position.y > 350f || self.transform.position.y < -200)//new start
        {
            self.Die();
            UnityEngine.GameObject.Destroy(self.gameObject);
        }//new end
    }
}
