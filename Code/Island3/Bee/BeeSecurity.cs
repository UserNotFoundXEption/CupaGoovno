using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class BeeSecurity
{
    public void Init()
    {
        On.BeeLevelSecurityGuard.Attack += Attack;
        On.BeeLevelSecurityGuard.attack_cr += attack_cr;
    }

    private void Attack(On.BeeLevelSecurityGuard.orig_Attack orig, BeeLevelSecurityGuard self)
    {
        Vector3 randomMove = new Vector3(UnityEngine.Random.Range(-100f, 100f), UnityEngine.Random.Range(-300f, 300f));//new start
        Vector3 newPosition = self.bombRoot.position + randomMove;
        while (newPosition.x > 600f)
        {
            newPosition -= new Vector3(UnityEngine.Random.Range(0f, 100f), 0f);
        }
        while (newPosition.x < -600f)
        {
            newPosition += new Vector3(UnityEngine.Random.Range(0f, 100f), 0f);
        }
        self.bombPrefab.Create(newPosition, -(int)self.transform.localScale.x, self.p.idleTime, self.p.warningTime, self.p.childSpeed, self.p.childCount);//new end
        //self.bombPrefab.Create(self.bombRoot.position, -(int)self.transform.localScale.x, self.p.idleTime, self.p.warningTime, self.p.childSpeed, self.p.childCount);
    }

    private IEnumerator attack_cr(On.BeeLevelSecurityGuard.orig_attack_cr orig, BeeLevelSecurityGuard self)
    {
        self.state = BeeLevelSecurityGuard.State.Attack;
        //self.animator.SetTrigger("OnAttack");
        //yield return CupheadTime.WaitForSeconds(self, 0.5f);
        self.animator.Play("Attack", -1, 0.5f);//new
        //yield return self.animator.WaitForAnimationToEnd(self, "Attack", false, true);
        yield break;
    }
}
