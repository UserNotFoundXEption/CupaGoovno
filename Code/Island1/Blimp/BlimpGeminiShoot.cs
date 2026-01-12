using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class BlimpGeminiShoot
{
    public void Init()
    {
        On.FlyingBlimpLevelGeminiShoot.rotate_cr += rotate_cr;
        On.FlyingBlimpLevelGeminiShoot.ShootBullet += ShootBullet;
    }

    private IEnumerator rotate_cr(On.FlyingBlimpLevelGeminiShoot.orig_rotate_cr orig, FlyingBlimpLevelGeminiShoot self)
    {
        YieldInstruction wait = new WaitForFixedUpdate();
        AudioManager.Play("level_flying_blimp_wheel_start");
        yield return CupheadTime.WaitForSeconds(self, 1f);
        self.animator.SetBool("Attack", true);
        self.smallFXSpawning = true;
        self.StartCoroutine(self.spawn_small_fx_cr());
        AudioManager.PlayLoop("level_flying_blimp_gemini_sphere_attack");
        float pct = 0f;
        float pct2 = 0f;//new
        float toDirectionChange = UnityEngine.Random.Range(0.4f, 0.8f);//new
        bool clockwise = true;
        float startRotation = (float)((!Rand.Bool()) ? -360 : 360);
        while (pct <= 2f && Blimp.blimpLady.properties.CurrentState.stateName == LevelProperties.FlyingBlimp.States.Gemini)//new
        //while (pct <= 1f)
        {
            float rotationDelta = CupheadTime.FixedDelta * self.properties.rotationSpeed;
            //self.transform.SetEulerAngles(null, null, new float?(startRotation * pct));
            self.transform.SetEulerAngles(null, null, new float?(startRotation * pct2));
            //pct += CupheadTime.FixedDelta * self.properties.rotationSpeed;
            pct += rotationDelta;//new start
            toDirectionChange -= rotationDelta;
            if(toDirectionChange < 0f)
            {
                clockwise = !clockwise;
                toDirectionChange = UnityEngine.Random.Range(0.4f, 0.8f);
            }
            if (clockwise)
            {
                pct2 += rotationDelta;
            }
            else
            {
                pct2 -= rotationDelta;
            }//new end
            self.ShootBullet();
            yield return wait;
        }
        self.transform.SetEulerAngles(null, null, new float?((float)((startRotation != 360f) ? 360 : -360)));
        self.smallFXSpawning = false;
        self.animator.SetBool("Attack", false);
        self.animator.SetTrigger("Leave");
        AudioManager.Stop("level_flying_blimp_gemini_sphere_attack");
        AudioManager.Play("level_flying_blimp_wheel_end");
        yield break;
    }

    private void ShootBullet(On.FlyingBlimpLevelGeminiShoot.orig_ShootBullet orig, FlyingBlimpLevelGeminiShoot self)
    {
        float x = self.projectileRoot.position.x - self.transform.position.x;
        float y = self.projectileRoot.position.y - self.transform.position.y;
        float rotation = Mathf.Atan2(y, x) * 57.29578f;
        if (self.delayTime < self.properties.bulletDelay)
        {
            self.delayTime += 1f;
        }
        else
        {
            self.projectilePrefab.Create(self.projectileRoot.position, rotation, self.properties.bulletSpeed);
            self.projectilePrefab.Create(self.projectileRoot.position, rotation + 180f, self.properties.bulletSpeed);//new
            self.delayTime = 0f;
        }
    }
}
