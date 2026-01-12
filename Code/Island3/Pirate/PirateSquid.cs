using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class PirateSquid
{
    public void Init()
    {
        On.PirateLevelSquid.OnEnterAnimationComplete += OnEnterAnimationComplete;
        On.PirateLevelSquid.Update += Update;
    }

    private void OnEnterAnimationComplete(On.PirateLevelSquid.orig_OnEnterAnimationComplete orig, PirateLevelSquid self)
    {
        //base.GetComponent<Collider2D>().enabled = true;
        self.state = PirateLevelSquid.State.Attack;
        self.attackTime = 0f;
        self.StartCoroutine(self.attack_cr());
    }

    private void Update(On.PirateLevelSquid.orig_Update orig, PirateLevelSquid self)
    {
        orig(self);
        if (self.state == PirateLevelSquid.State.Attack && self.properties.CurrentState.stateName == LevelProperties.Pirate.States.Boat)
        {
            self.Exit();
        }
    }
}
