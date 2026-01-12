using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class DevilSpinnerProjectile
{
    public void Init()
    {
        On.DevilLevelPitchforkSpinnerProjectile.main_cr += main_cr;
    }

    private IEnumerator main_cr(On.DevilLevelPitchforkSpinnerProjectile.orig_main_cr orig, DevilLevelPitchforkSpinnerProjectile self)
    {
        self.GetComponent<Collider2D>().enabled = false;
        self.GetComponent<GroundHomingMovement>().EnableHoming = false;
        yield return CupheadTime.WaitForSeconds(self, self.waitTime);
        self.waitTimeUp = true;
        self.animator.SetTrigger("Continue");
        self.animator.SetBool("StartAtHalf", Rand.Bool());
        GroundHomingMovement homingMovement = self.GetComponent<GroundHomingMovement>();
        homingMovement.maxSpeed = self.homingMaxSpeed;
        homingMovement.acceleration = self.homingAcceleration;
        homingMovement.bounceEnabled = false;
        homingMovement.destroyOffScreen = false;
        homingMovement.TrackingPlayer = PlayerManager.GetNext();
        homingMovement.EnableHoming = false;
        //self.GetComponent<Collider2D>().enabled = true;
        self.GetComponent<GroundHomingMovement>().EnableHoming = true;
        yield return CupheadTime.WaitForSeconds(self, self.homingDuration);
        self.GetComponent<GroundHomingMovement>().EnableHoming = false;
        yield break;
    }
}
