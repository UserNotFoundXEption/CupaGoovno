using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class GraveyardDevil
{
    public void Init()
    {
        On.GraveyardLevelSplitDevil.OnCollisionPlayer += OnCollisionPlayer;
    }

    public void OnCollisionPlayer(On.GraveyardLevelSplitDevil.orig_OnCollisionPlayer orig, GraveyardLevelSplitDevil self, object hit, CollisionPhase phase)
    {
    }
}
