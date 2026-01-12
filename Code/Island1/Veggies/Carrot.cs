using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class Carrot
{
    public void Init()
    {
        On.VeggiesLevelCarrot.OnDamageTaken += OnDamageTaken;
        On.VeggiesLevelCarrot.ShootHoming += ShootHoming;
        On.VeggiesLevelCarrot.OnInAnimComplete += OnInAnimComplete;
        On.VeggiesLevelCarrot.rings_cr += rings_cr;
    }

    private void OnDamageTaken(On.VeggiesLevelCarrot.orig_OnDamageTaken orig, VeggiesLevelCarrot self, DamageDealer.DamageInfo info)
    {
        orig(self, info);
        VeggiesLevelCarrot mainCarrot = mainCarrotDic[self];
        if (self.dead || (mainCarrot != self && info.damageSource == DamageDealer.DamageSource.Super))
        {
            return;
        }

        if (mainCarrot != self)
        {
            self.hp += info.damage;
            mainCarrot.OnDamageTaken(info);

        }
    }

    public void ShootHoming(On.VeggiesLevelCarrot.orig_ShootHoming orig, VeggiesLevelCarrot self)
    {
        VeggiesLevelCarrot mainCarrot = mainCarrotDic[self];
        if (self == mainCarrot)//new start
        {
            VeggiesLevelCarrotHomingProjectile proj = self.homingPrefab.Create(PlayerManager.GetNext(), self, new Vector2(0f, 400f), 50f, 0, 2137f);
            proj.transform.SetScale(0.6f, 0.6f);
        }//new end
         //self.homingPrefab.Create(PlayerManager.GetNext(), self, self.GetHomingRoot(), self.carrot.homingSpeed, self.carrot.homingRotation, (float)self.carrot.homingHP);
    }

    private void OnInAnimComplete(On.VeggiesLevelCarrot.orig_OnInAnimComplete orig, VeggiesLevelCarrot self)
    {
        self.transform.GetComponent<Collider2D>().enabled = true;
        //base.StartCoroutine(this.rings_cr());
        carrotsList.Add(self);//new start
        VeggiesLevelCarrot mainCarrot = mainCarrotDic[self];
        isShootingDic[self] = false;
        if (self == mainCarrot)
        {
            self.StartCoroutine(carrotsSharedCooldown_cr(self));
        }//new end
    }

    private IEnumerator carrotsSharedCooldown_cr(VeggiesLevelCarrot self)//new
    {
        while(carrotsList.Count < 3)
        {
            yield return null;
        }
        for(; ; )
        {
            MinMax idleRange = self.properties.CurrentState.carrot.idleRange;
            yield return CupheadTime.WaitForSeconds(self, idleRange.RandomFloat());
            VeggiesLevelCarrot chosenCarrot = carrotsList.RandomChoice();
            while (isShootingDic[chosenCarrot])
            {
                chosenCarrot = carrotsList.RandomChoice();
                yield return null;
            }
            isShootingDic[chosenCarrot] = true;
            chosenCarrot.StartCoroutine(chosenCarrot.rings_cr());
        }
    }

    private IEnumerator rings_cr(On.VeggiesLevelCarrot.orig_rings_cr orig, VeggiesLevelCarrot self)
    {
        //for (; ; )
        //{
        self.StartCoroutine(self.carrot_cr());
        //yield return CupheadTime.WaitForSeconds(self, self.properties.CurrentState.carrot.idleRange.RandomFloat());
        int count = 0;
        self.animator.SetTrigger("AttackStart");
        yield return self.animator.WaitForAnimationToEnd(self, "Attack_Start", false, true);
        while (count < self.carrot.bulletCount)
        {
            yield return CupheadTime.WaitForSeconds(self, self.carrot.bulletDelay * 0.5f);
            self.ringEffectPrefab.Create(self.straightRoot.position);
            yield return CupheadTime.WaitForSeconds(self, self.carrot.bulletDelay * 0.5f);
            self.straightRoot.LookAt2D(PlayerManager.GetNext().center);
            for (int i = 0; i < 5; i++)
            {
                AudioManager.Play("level_veggies_carrot_beam");
                BasicProjectile rings = self.ringPrefab.Create(self.straightRoot.position, self.straightRoot.eulerAngles.z, self.carrot.bulletSpeed);
                rings.transform.SetScale(0.8f, 0.8f, 0.8f);//new
                yield return CupheadTime.WaitForSeconds(self, 0.08f);//0.1
            }
            count++;
        }
        self.animator.SetTrigger("AttackEnd");
        isShootingDic[self] = false;//new
        //}
    }

    public static void SetMainCarrot(VeggiesLevelCarrot mainCarrot, VeggiesLevelCarrot self)//new
    {
        if (mainCarrot != null)
        {
            Carrot.mainCarrotDic[self] = mainCarrot;
        }
        else
        {
            Carrot.mainCarrotDic[self] = self;
            self.StartCoroutine(wall_cr(self));
        }
    }

    private static IEnumerator wall_cr(VeggiesLevelCarrot self)//new
    {
        for (; ; )
        {
            yield return CupheadTime.WaitForSeconds(self, 1f);
            self.ShootHoming();
            yield return CupheadTime.WaitForSeconds(self, 1f);
            self.ShootHoming();
            yield return CupheadTime.WaitForSeconds(self, 3f);
        }
    }

    public static Dictionary<VeggiesLevelCarrot, VeggiesLevelCarrot> mainCarrotDic = new Dictionary<VeggiesLevelCarrot, VeggiesLevelCarrot>();
    Dictionary<VeggiesLevelCarrot, bool> isShootingDic = new Dictionary<VeggiesLevelCarrot, bool>();
    public static List<VeggiesLevelCarrot> carrotsList = new List<VeggiesLevelCarrot>();
}
