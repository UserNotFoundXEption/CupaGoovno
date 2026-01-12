using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class Onion
{
    public void Init()
    {
        On.VeggiesLevelOnion.Start += Start;
        On.VeggiesLevelOnion.die_cr += die_cr;
    }

    private void Start(On.VeggiesLevelOnion.orig_Start orig, VeggiesLevelOnion self)
    {
        if (self.properties == null)
        {
            UnityEngine.Object.Destroy(self.gameObject);
            return;
        }
        self.noSecret = true;
        self.circleCollider = self.GetComponent<CircleCollider2D>();
        //this.state = VeggiesLevelOnion.State.Idle;
        //base.StartCoroutine(this.happyTimer_cr());
        self.state = VeggiesLevelOnion.State.Crying;
        self.animator.SetTrigger("SadStart");
        self.HappyLeave = true;
        self.SfxGround();
    }

    private IEnumerator die_cr(On.VeggiesLevelOnion.orig_die_cr orig, VeggiesLevelOnion self)
    {
        self.circleCollider.enabled = false;
        self.state = VeggiesLevelOnion.State.Idle;//new start
        self.animator.Play("Happy_Exit");
        self.HappyLeave = true;
        self.animator.SetTrigger("HappyExit");
        self.StartCoroutine(self.handle_dirt_cr());//new end
        /*
        AudioManager.Play("level_veggies_onion_die");
        base.animator.Play("Sad_Die");
        this.StartExplosions();
        yield return CupheadTime.WaitForSeconds(this, 2f);
        this.StopExplosions();
        yield return null;
        */
        yield return null;
        yield break;
    }
}
