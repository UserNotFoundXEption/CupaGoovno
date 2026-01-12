using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class BoozeOlive
{
    public void Init()
    {
        On.DicePalaceBoozeLevelOlive.attack_cr += attack_cr;
        On.DicePalaceBoozeLevelOlive.shoot_cr += shoot_cr;
        On.DicePalaceBoozeLevelOlive.OnDeath += OnDeath;
    }

    private IEnumerator attack_cr(On.DicePalaceBoozeLevelOlive.orig_attack_cr orig, DicePalaceBoozeLevelOlive self)
    {
        moveDic[self] = true;//new
        projDic[self] = new List<BasicProjectile>();//new
        for (; ; )
        {
            //if (self.moveCount < self.moveCountMax)
            if (moveDic[self])//new
            {
                self.GetNextTarget();
                self.StartCoroutine(self.move_cr());
                self.moveCount++;
                while (self.moving)
                {
                    yield return null;
                }
                yield return CupheadTime.WaitForSeconds(self, self.properties.CurrentState.martini.oliveStopDuration);
                moveDic[self] = false;//new
            }
            else
            {
                AudioManager.Play("booze_olive_attack");
                self.emitAudioFromObject.Add("booze_olive_attack");
                self.animator.SetTrigger("OnAttack");
                yield return self.animator.WaitForAnimationToEnd(self, "Attack", false, true);
                yield return CupheadTime.WaitForSeconds(self, self.properties.CurrentState.martini.oliveHesitateAfterShooting);
                moveDic[self] = true;//new
            }
        }
    }

    private IEnumerator shoot_cr(On.DicePalaceBoozeLevelOlive.orig_shoot_cr orig, DicePalaceBoozeLevelOlive self)
    {
        self.moveCount = 0;
        Vector3 target = PlayerManager.GetPlayer(self.nextPlayerTarget).center - self.transform.position;
        BasicProjectile proj = self.pimentoPrefab.Create(self.transform.position, 0f, self.properties.CurrentState.martini.bulletSpeed);
        proj.animator.SetBool("Reverse", Rand.Bool());
        proj.transform.right = target;
        projDic[self].Add(proj);//new
        IEnumerator enumerator = proj.GetComponentInChildren<Transform>().GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                object obj = enumerator.Current;
                Transform transform = (Transform)obj;
                transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(0f));
            }
        }
        finally
        {
            IDisposable disposable;
            if ((disposable = (enumerator as IDisposable)) != null)
            {
                disposable.Dispose();
            }
        }
        self.shotCount++;
        if (self.shotCount > self.shotCountMax)
        {
            proj.SetParryable(true);
            self.shotCount = 0;
        }
        yield return null;
        yield break;
    }

    private void OnDeath(On.DicePalaceBoozeLevelOlive.orig_OnDeath orig, DicePalaceBoozeLevelOlive self)
    {
        if (projDic.ContainsKey(self))
        {
            while (projDic[self].Count > 0)
            {
                if(projDic[self][0] != null)
                {
                    GameObject.Destroy(projDic[self][0].gameObject);
                }
                projDic[self].RemoveAt(0);
            }
        }
        orig(self);
    }

    private Dictionary<DicePalaceBoozeLevelOlive, bool> moveDic = new Dictionary<DicePalaceBoozeLevelOlive, bool>();
    private Dictionary<DicePalaceBoozeLevelOlive, List<BasicProjectile>> projDic = new Dictionary<DicePalaceBoozeLevelOlive, List<BasicProjectile>>();
}
