using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class TrainLollipopGhoul
{
    public void Init()
    {
        On.TrainLevelLollipopGhoul.head_cr += head_cr;
        On.TrainLevelLollipopGhoul.OnDamageTaken += OnDamageTaken;
    }

    private IEnumerator head_cr(On.TrainLevelLollipopGhoul.orig_head_cr orig, TrainLevelLollipopGhoul self)
    {
        float t = 0f;
        float time = self.properties.CurrentState.lollipopGhouls.moveTime;
        EaseUtils.EaseType ease = EaseUtils.EaseType.easeInOutSine;
        //Vector3 start = Vector3.zero;
        Vector3 start = new Vector3(-200f, 0f, 0f);//new
        Vector3 end = new Vector3(self.properties.CurrentState.lollipopGhouls.moveDistance, 0f, 0f);
        self.head.localPosition = start;
        while (t < time)
        {
            float val = EaseUtils.Ease(ease, 0f, 1f, t / time);
            self.head.localPosition = Vector3.Lerp(start, end, val);
            t += CupheadTime.Delta;
            yield return null;
        }
        /*self.head.localPosition = end;
        t = 0f;
        while (t < time)
        {
            float val2 = EaseUtils.Ease(ease, 0f, 1f, t / time);
            self.head.localPosition = Vector3.Lerp(end, start, val2);
            t += CupheadTime.Delta;
            yield return null;
        }*/
        self.head.localPosition = start;
        yield break;
    }

    private void OnDamageTaken(On.TrainLevelLollipopGhoul.orig_OnDamageTaken orig, TrainLevelLollipopGhoul self, DamageDealer.DamageInfo info)
    {
        self.health += info.damage;

        float damage = info.damage;
        bool isSuper = info.damageSource == DamageDealer.DamageSource.Super;
        Super super = PlayerManager.GetFirst().stats.Loadout.super;
        bool superOneEquiped = super == Super.level_super_beam;
        if (isSuper && superOneEquiped)
        {
            damage /= 2f;
        }

        TrainLollipopGhoulsManager.ghoulLeft.health -= damage / 2;
        TrainLollipopGhoulsManager.ghoulRight.health -= damage / 2;
        orig(self, info);

        if(self.health <= 0)
        {
            if (self == TrainLollipopGhoulsManager.ghoulLeft)
            {
                TrainLollipopGhoulsManager.ghoulRight.Die();
            }
            if(self == TrainLollipopGhoulsManager.ghoulRight)
            {
                TrainLollipopGhoulsManager.ghoulLeft.Die();
            }
        }
    }
}
