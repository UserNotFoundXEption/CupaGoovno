using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class SallyProjectile
{
    public void Init()
    {
        On.SallyStagePlayLevelProjectile.move_cr += move_cr;
    }

    public static void Init(SallyStagePlayLevelProjectile self, Vector2 pos, float rotation, LevelProperties.SallyStagePlay.Projectile properties)
    {
        self.transform.position = pos;
        self.properties = properties;
        self.speed = properties.projectileSpeed;
        self.transform.SetEulerAngles(null, null, new float?(rotation));
        self.StartCoroutine(self.move_cr());
        //AudioManager.Play("sally_fan_shoot");
        //self.emitAudioFromObject.Add("sally_fan_shoot");
    }

    private IEnumerator move_cr(On.SallyStagePlayLevelProjectile.orig_move_cr orig, SallyStagePlayLevelProjectile self)
    {
        //AudioManager.PlayLoop("sally_fan_shoot_loop");
        //self.emitAudioFromObject.Add("sally_fan_shoot_loop");
        while (self.transform.position.y > (float)Level.Current.Ground)
        {
            self.transform.position += self.transform.right * self.speed * CupheadTime.Delta;
            yield return null;
        }
        self.animator.SetTrigger("OnLand");
        //AudioManager.Play("sally_fan_stick");
        //self.emitAudioFromObject.Add("sally_fan_stick");
        //AudioManager.Stop("sally_fan_shoot_loop");
        yield return CupheadTime.WaitForSeconds(self, self.properties.groundDuration);
        self.animator.SetTrigger("OnDeath");
        yield break;
    }
}
