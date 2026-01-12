using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class MouseCherryBomb
{
    public void Init()
    {
        On.MouseLevelCherryBombProjectile.FixedUpdate += FixedUpdate;
        On.MouseLevelCherryBombProjectile.SpawnChildren += SpawnChildren;
    }

    public static void FixedUpdate(On.MouseLevelCherryBombProjectile.orig_FixedUpdate orig, MouseLevelCherryBombProjectile self)
    {
        AbstractProj.FixedUpdate(self);
        if (!canDieDic.ContainsKey(self))
        {
            canDieDic[self] = false;
        }
        if (!bouncedDic.ContainsKey(self))
        {
            bouncedDic[self] = false;
        }
        if (self.state == MouseLevelCherryBombProjectile.State.Moving)
        {
            /*if (self.transform.position.y < (float)Level.Current.Ground + 60f)
            {
                self.state = MouseLevelCherryBombProjectile.State.Dead;
                selfSelf.animator.SetTrigger("OnExplode");
                return;

            }*/
            if (self.transform.position.x > 650)//new start
            {
                self.velocity.x = -Mathf.Abs(self.velocity.x);
            }
            if (self.transform.position.x < -650)
            {
                self.velocity.x = Mathf.Abs(self.velocity.x);
            }
            if (self.transform.position.y < (float)Level.Current.Ground + 60f && !bouncedDic[self])
            {
                bouncedDic[self] = true;
                self.StartCoroutine(invincibilityTimer_cr(self));
                self.velocity.y *= -0.7f;
                self.SpawnChildren();
            }
            if (self.transform.position.y < (float)Level.Current.Ground + 60f && canDieDic[self])
            {
                self.state = MouseLevelCherryBombProjectile.State.Dead;
                self.animator.SetTrigger("OnExplode");
                return;
            }//new end
            self.transform.AddPosition(self.velocity.x * CupheadTime.FixedDelta, self.velocity.y * CupheadTime.FixedDelta, 0f);
            self.velocity.y = self.velocity.y - self.gravity * CupheadTime.FixedDelta;
        }
    }

    private void SpawnChildren(On.MouseLevelCherryBombProjectile.orig_SpawnChildren orig, MouseLevelCherryBombProjectile self)
    {
        //self.cloud.Create(new Vector3(self.transform.position.x + 20f, self.transform.position.y + 200f), new Vector3(0.52f, 0.52f, 0.52f));
        BasicProjectile basicProjectile = self.childProjectile.Create(self.transform.position - new Vector3(0f, 40f, 0f), 0f, new Vector2(0.6f, 0.6f), -self.childSpeed);
        basicProjectile.GetComponent<Animator>().SetBool("isRight", false);
        BasicProjectile basicProjectile2 = self.childProjectile.Create(self.transform.position - new Vector3(0f, 40f, 0f), 0f, new Vector2(-0.6f, -0.6f), self.childSpeed);
        basicProjectile2.GetComponent<Animator>().SetBool("isRight", true);
    }

    private static IEnumerator invincibilityTimer_cr(MouseLevelCherryBombProjectile self)//new
    {
        yield return CupheadTime.WaitForSeconds(self, 0.5f);
        canDieDic[self] = true;
        yield break;
    }

    private static Dictionary<MouseLevelCherryBombProjectile, bool> canDieDic = new Dictionary<MouseLevelCherryBombProjectile, bool>();
    private static Dictionary<MouseLevelCherryBombProjectile, bool> bouncedDic = new Dictionary<MouseLevelCherryBombProjectile, bool>();
}
