using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CupaGoovno;

public class GenieGoop
{
    public void Init()
    {
        On.FlyingGenieLevelGoop.shoot_cr += shoot_cr;
        On.FlyingGenieLevelGoop.ShootProjectiles += ShootProjectiles;
    }

    private IEnumerator shoot_cr(On.FlyingGenieLevelGoop.orig_shoot_cr orig, FlyingGenieLevelGoop self)
    {
        yield return self.animator.WaitForAnimationToEnd(self, "Intro", false, true);
        for (; ; )
        {
            if (self.moving)
            {
                yield return CupheadTime.WaitForSeconds(self, self.properties.CurrentState.coffin.heartShotDelayRange.RandomFloat());
                //self.animator.SetTrigger("OnAttack");
                self.animator.Play("Attack", -1, 0.2f);//new
                yield return self.animator.WaitForAnimationToEnd(self, "Attack", false, true);
            }
            yield return null;
        }
    }

    private void ShootProjectiles(On.FlyingGenieLevelGoop.orig_ShootProjectiles orig, FlyingGenieLevelGoop self)
    {
        AudioManager.Play("genie_sarcophagus_eye_plop");
        self.emitAudioFromObject.Add("genie_sarcophagus_eye_plop");
        FlyingGenieLevelHelixProjectile proj1 = self.projectile.Create(self.topRoot.position, self.properties.CurrentState.coffin, true);//new start
        FlyingGenieLevelHelixProjectile proj2 = self.projectile.Create(self.bottomRoot.position, self.properties.CurrentState.coffin, false);
        float rotationSpeed = UnityEngine.Random.Range(1f, 2f);
        int rotationDirection = UnityEngine.Random.Range(0, 2) * 2 - 1; //0*2-1=-1 ; 1*2-1=1
        GenieHelixProjectile.SetRotationProperties(proj1, rotationSpeed, rotationDirection);
        GenieHelixProjectile.SetRotationProperties(proj2, rotationSpeed, rotationDirection);//new end
        /*self.projectile.Create(self.topRoot.position, self.properties.CurrentState.coffin, true);
        self.projectile.Create(self.bottomRoot.position, self.properties.CurrentState.coffin, false);*/
    }
}
