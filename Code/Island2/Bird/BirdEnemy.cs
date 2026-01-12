using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class BirdEnemy
{
    public void Init()
    {
        On.FlyingBirdLevelEnemy.OnDamageTaken += OnDamageTaken;
        On.FlyingBirdLevelEnemy.Init += Init;
    }

    private void OnDamageTaken(On.FlyingBirdLevelEnemy.orig_OnDamageTaken orig, FlyingBirdLevelEnemy self, DamageDealer.DamageInfo info)
    {
        if (info.damageSource == DamageDealer.DamageSource.Super || info.damageSource == DamageDealer.DamageSource.Ex)
        {
            orig(self, info);
        }
    }

    private void Init(On.FlyingBirdLevelEnemy.orig_Init orig, FlyingBirdLevelEnemy self)
    {
        self.startPos = self.transform.position;
        self.health = (float)self.properties.health;
        self.StartCoroutine(self.x_cr());
        self.StartCoroutine(y_sin_cr(self));//new
        self.StartCoroutine(self.shoot_cr());
    }

    private IEnumerator y_sin_cr(FlyingBirdLevelEnemy self)//new
    {
        float center = self.transform.position.y;
        float t = 0f;
        for (; ; )
        {
            t += CupheadTime.Delta;
            float sinTmp = t / self.properties.floatTime * 6.283f;
            float sin = Mathf.Sin(sinTmp) * self.properties.floatRange;
            self.transform.position = new Vector3(self.transform.position.x, center + sin);
            yield return null;
        }
    }
}
