using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class PirateBarrel
{
    public void Init()
    {
        On.PirateLevelBarrel.OnStateChanged += OnStateChanged;
    }

    private void OnStateChanged(On.PirateLevelBarrel.orig_OnStateChanged orig, PirateLevelBarrel self)
    {
        orig(self);
        if (self.properties.CurrentState.stateName == LevelProperties.Pirate.States.Boat)
        {
            GameObject.Destroy(self.gameObject);
        }
    }
}
