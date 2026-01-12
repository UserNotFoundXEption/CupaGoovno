using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class BoozeMartini
{
    public void Init()
    {
        On.DicePalaceBoozeLevelMartini.Awake += Awake;
        On.DicePalaceBoozeLevelMartini.OnDamageTaken += OnDamageTaken;
    }

    protected void Awake(On.DicePalaceBoozeLevelMartini.orig_Awake orig, DicePalaceBoozeLevelMartini self)
    {
        orig(self);
        martini = self;
    }

    private void OnDamageTaken(On.DicePalaceBoozeLevelMartini.orig_OnDamageTaken orig, DicePalaceBoozeLevelMartini self, DamageDealer.DamageInfo info)
    {
        bool isSuper = info.damageSource == DamageDealer.DamageSource.Super;
        Super super = PlayerManager.GetFirst().stats.Loadout.super;
        bool superOneEquiped = super == Super.level_super_beam;
        if (isSuper && superOneEquiped)
        {
            self.health += info.damage * 8 / 9;
            BoozeTumbler.tumbler.health -= info.damage / 9;
            BoozeDecanter.decanter.health -= info.damage / 9;
        }
        else
        {
            self.health += info.damage * 2 / 3;
            BoozeTumbler.tumbler.health -= info.damage / 3;
            BoozeDecanter.decanter.health -= info.damage / 3;
        }
        orig(self, info);
        if (self.health <= 0)
        {
            if (!BoozeTumbler.tumbler.isDead)
            {
                BoozeTumbler.tumbler.StartDying();
            }
            if (!BoozeDecanter.decanter.isDead)
            {
                BoozeDecanter.decanter.StartDying();
            }
        }
    }

    public static DicePalaceBoozeLevelMartini martini;
}
