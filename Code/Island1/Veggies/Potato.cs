using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class Potato
{
    public void Init()
    {
        On.VeggiesLevelPotato.Shoot += Shoot;
        On.VeggiesLevelPotato.potato_cr += potato_cr;
        On.VeggiesLevelPotato.Start += Start;
    }

    private void Start(On.VeggiesLevelPotato.orig_Start orig, VeggiesLevelPotato self)
    {
        self.SfxGround();
        self.transform.position = new Vector3(self.transform.position.x + 100f, self.transform.position.y);//new
    }

    private void Shoot(On.VeggiesLevelPotato.orig_Shoot orig, VeggiesLevelPotato self)
    {
        if (!self.projectileParryFlag)
        {
            AudioManager.Play("levels_veggies_potato_spit");
        }
        else
        {
            AudioManager.Play("level_veggies_potato_spit_worm");
        }
        self.didShoot = true;
        self.projectilePrefab.Create(self.gunRoot.position, self.gunRoot.eulerAngles.z, self.properties.bulletSpeed).SetParryable(false);
        self.spitEffect.Create(self.gunRoot.position);
        Vector2 vector = new Vector2(self.gunRoot.position.x, self.gunRoot.position.y + 150f);//new start
        self.projectilePrefab.Create(vector, self.gunRoot.eulerAngles.z, self.properties.bulletSpeed).SetParryable(false);
        self.spitEffect.Create(vector);
        Vector2 vector2 = new Vector2(self.gunRoot.position.x, self.gunRoot.position.y + 300f);
        BasicProjectile topProj = self.projectilePrefab.Create(vector2, self.gunRoot.eulerAngles.z, self.properties.bulletSpeed);
        if (self.projectileParryFlag)
        {
            topProj.SetParryable(true);
            topProj._countParryTowardsScore = true;
        }
        self.spitEffect.Create(vector2);//new end
    }

    private IEnumerator potato_cr(On.VeggiesLevelPotato.orig_potato_cr orig, VeggiesLevelPotato self)
    {
        for (; ; )
        {
            int groups = 1;//0
            int shots = 0;
            while (groups < self.properties.seriesCount)
            {
                float delay = self.properties.bulletDelay.GetFloatAt(1f - (float)groups / ((float)self.properties.seriesCount - 1f));
                int num;
                while ((float)shots < (float)self.properties.bulletCount)
                {
                    num = shots;
                    shots = num + 1;
                    self.animator.SetTrigger("Shoot");
                    self.didShoot = false;
                    //self.projectileParryFlag = (shots == self.properties.bulletCount);
                    self.projectileParryFlag = (shots % 2 == 1);//new
                    while (!self.didShoot)
                    {
                        yield return null;
                    }
                    yield return CupheadTime.WaitForSeconds(self, delay);
                }
                num = groups;
                groups = num + 1;
                shots = 0;
                if (groups != self.properties.seriesCount)
                {
                    yield return CupheadTime.WaitForSeconds(self, self.properties.seriesDelay);
                    //yield return CupheadTime.WaitForSeconds(self, 0.6f);
                }
            }
            yield return CupheadTime.WaitForSeconds(self, self.properties.idleTime);
        }
    }
}
