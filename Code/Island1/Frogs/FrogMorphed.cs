using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CupaGoovno;

public class FrogMorphed
{
    public void Init()
    {
        On.FrogsLevelMorphed.Enable += Enable;
        On.FrogsLevelMorphed.oni_cr += oni_cr;
    }

    public void Enable(On.FrogsLevelMorphed.orig_Enable orig, FrogsLevelMorphed self, bool demonTriggered)
    {
        self.demonTriggered = true;//demonTriggered
        self.gameObject.SetActive(true);
        self.dustEffect.gameObject.SetActive(true);
        self.properties.OnBossDeath += self.OnBossDeath;
        self.GetComponent<LevelBossDeathExploder>().enabled = true;
        self.StartCoroutine(self.loop_cr());
    }

    private IEnumerator oni_cr(On.FrogsLevelMorphed.orig_oni_cr orig, FrogsLevelMorphed self)
    {
        LevelProperties.Frogs.Demon p = self.properties.CurrentState.demon;
        float bulletSpeed = 0f;
        float bulletDelay = 1000f;
        float delay = 0f;
        float val = 0f;
        float time = p.demonMaxTime;
        float t = 0f;
        string patterns = "TBO";//new
        bool nowSnake = false;//new
        for (; ; )
        {
            FrogsLevelBisonBullet.Direction dir = (FrogsLevelBisonBullet.Direction)UnityEngine.Random.Range(0, 2);
            string[] demonPattern = p.demonString[self.mainIndex].Split(new char[]
            {
            ','
            });
            if (bulletDelay >= delay)
            {
                demonPattern = p.demonString[self.mainIndex].Split(new char[]
                {
                ','
                });
                bulletSpeed = p.demonSpeed.GetFloatAt(val);
                //char c = demonPattern[self.index][0];
                char c = nowSnake ? 'S' : patterns[UnityEngine.Random.Range(0, 3)];//new
                nowSnake = !nowSnake;//new
                if (c != 'S')
                {
                    if (c != 'T')
                    {
                        if (c != 'B')
                        {
                            if (c == 'O')
                            {
                                self.ShootOni(bulletSpeed);
                            }
                        }
                        else
                        {
                            yield return CupheadTime.WaitForSeconds(self, 0.2f);//new
                            dir = (FrogsLevelBisonBullet.Direction)UnityEngine.Random.Range(0, 2);
                            self.ShootBison(bulletSpeed, dir, self.properties.CurrentState.morph.bisonBigX, self.properties.CurrentState.morph.bisonSmallX);
                            if (dir == FrogsLevelBisonBullet.Direction.Up)
                            {
                                dir = FrogsLevelBisonBullet.Direction.Down;
                            }
                            else
                            {
                                dir = FrogsLevelBisonBullet.Direction.Up;
                            }
                        }
                    }
                    else
                    {
                        self.ShootTiger(bulletSpeed);
                    }
                }
                else
                {
                    self.ShootSnake(bulletSpeed);
                }
                if (self.index < demonPattern.Length - 1)
                {
                    self.index++;
                }
                else
                {
                    self.mainIndex = (self.mainIndex + 1) % p.demonString.Length;
                    self.index = 0;
                }
                bulletDelay = 0f;
            }
            delay = p.demonDelay.GetFloatAt(val);
            bulletDelay += CupheadTime.Delta;
            if (val < 1f)
            {
                val = t / time;
                t += CupheadTime.Delta;
            }
            else
            {
                val = 1f;
            }
            yield return null;
        }
    }
}
