using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class GenieHead
{
    public void Init()
    {
        On.FlyingGenieLevelGenieHead.Die += Die;
        On.FlyingGenieLevelGenieHead.OnDamageTaken += OnDamageTaken;
    }

    private void OnDamageTaken(On.FlyingGenieLevelGenieHead.orig_OnDamageTaken orig, FlyingGenieLevelGenieHead self, DamageDealer.DamageInfo info)
    {
        self.health -= info.damage;
        if (self.health < 0f)
        {
            self.Die();
        }
        //self.parent.DoDamage(info.damage);
    }

    protected void Die(On.FlyingGenieLevelGenieHead.orig_Die orig, FlyingGenieLevelGenieHead self)
    {
        AudioManager.Play("genie_pillar_destruction");
        self.emitAudioFromObject.Add("genie_pillar_destruction");
        //self.headExplode.Create(new Vector3(self.transform.position.x - 75f, self.transform.position.y));
        self.GetComponent<SpriteRenderer>().enabled = false;
        self.darkSprite.GetComponent<SpriteRenderer>().enabled = false;
        orig(self);
    }
}
