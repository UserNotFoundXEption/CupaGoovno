using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CupaGoovno;

public class PirateDogFish
{
    public void Init()
    {
        On.PirateLevelDogFish.OnEnableCollider += OnEnableCollider;
    }

    private void OnEnableCollider(On.PirateLevelDogFish.orig_OnEnableCollider orig, PirateLevelDogFish self)
    {
        //self.normalHitBox.GetComponent<DamageReceiver>().enabled = true;
        self.normalHitBox.GetComponent<DamageReceiver>().enabled = false;//new
        self.secretHitBox.GetComponent<DamageReceiver>().enabled = false;
        self.gameObject.layer = 0;
    }
}
