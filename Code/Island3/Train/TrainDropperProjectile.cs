using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class TrainDropperProjectile
{
    public void Init()
    {
        On.TrainLevelEngineBossDropperProjectile.go_cr += go_cr;
    }

    private IEnumerator go_cr(On.TrainLevelEngineBossDropperProjectile.orig_go_cr orig, TrainLevelEngineBossDropperProjectile self)
    {
        AbstractPlayerController target = PlayerManager.GetNext();
        while (self.transform.position.y > target.center.y)
        {
            /*Vector3 vel = Vector3.zero;
            self.transform.AddPosition(0f, self.velocity.y * CupheadTime.Delta, 0f);
            self.velocity.y = self.velocity.y - self.gravity * CupheadTime.Delta;*/
            self.transform.AddPosition(0f, -self.velocity.y * CupheadTime.Delta, 0f);//new
            yield return null;
            if (target == null || target.IsDead)
            {
                target = PlayerManager.GetNext();
            }
        }
        int direction = (target.center.x <= self.transform.position.x) ? -1 : 1;
        self.transform.localScale = new Vector3((float)(-(float)direction), 1f, 1f);
        self.animator.SetTrigger("Horizontal");
        self.dustFX.Create(self.transform.position, new Vector3((float)direction, 1f, 1f)).Play();
        self.verticalCollider.enabled = false;
        self.horizontalCollider.enabled = true;
        for (; ; )
        {
            self.transform.AddPosition((float)direction * self.velocity.x * CupheadTime.Delta, 0f, 0f);
            yield return null;
        }
    }
}
