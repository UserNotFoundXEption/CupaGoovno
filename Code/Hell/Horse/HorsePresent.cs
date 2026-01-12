using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class HorsePresent
{
    public void Init()
    {
        On.DicePalaceFlyingHorseLevelPresent.Die += Die;
        On.DicePalaceFlyingHorseLevelPresent.SpawnBullet += SpawnBullet;
    }

    protected void Die(On.DicePalaceFlyingHorseLevelPresent.orig_Die orig, DicePalaceFlyingHorseLevelPresent self)
    {
        AudioManager.Play("projectile_explo");//new
        self.emitAudioFromObject.Add("projectile_explo");//new
        for (int i = 1; i < 9; i++)
        {
            float angle = 45f * i;
            float speed = self.properties.explosionSpeed * 5f;
            self.bullet.Create(self.transform.position, angle, speed);
        }
        BasicProjectile[] projectiles = GameObject.FindObjectsOfType<BasicProjectile>();
        foreach (BasicProjectile proj in projectiles)
        {
            if (proj.name == "FlyingHorse_Bullet(Clone)" && !horseshoes.Contains(proj))
            {
                horseshoes.Add(proj);
                Horse.horse.StartCoroutine(increaseSize_cr(proj));
            }
        }
        orig(self);
    }

    private void SpawnBullet(On.DicePalaceFlyingHorseLevelPresent.orig_SpawnBullet orig, DicePalaceFlyingHorseLevelPresent self, float angle, bool parryable)
    {
        //AudioManager.Play("projectile_explo");
        //self.emitAudioFromObject.Add("projectile_explo");
        BasicProjectile basicProjectile = self.bullet.Create(self.transform.position, angle, self.properties.explosionSpeed);
        basicProjectile.SetParryable(parryable);
    }

    private IEnumerator increaseSize_cr(BasicProjectile proj)//new
    {
        yield return CupheadTime.WaitForSeconds(Horse.horse, 3f);
        float time = 0f;
        float duration = 3f;
        while (time < duration)
        {
            time += CupheadTime.delta;
            float percent = time / duration;
            float scale = 1f + percent * 2;
            if(proj == null)
            {
                yield break;
            }
            proj.transform.SetScale(scale, scale);
            yield return null;
        }
    }

    public static List<BasicProjectile> horseshoes;
}
