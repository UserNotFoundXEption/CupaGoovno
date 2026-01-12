using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Rewired.ComponentControls.Effects.RotateAroundAxis;
using UnityEngine;

namespace CupaGoovno;

public class DevilDragon
{
    public void Init()
    {
        On.DevilLevelDragonHead.move_cr += move_cr;
    }

    public IEnumerator move_cr(On.DevilLevelDragonHead.orig_move_cr orig, DevilLevelDragonHead self, DevilLevelSittingDevil parent, bool isLeft)
    {
        YieldInstruction wait = new WaitForFixedUpdate();
        self.transform.position = ((!isLeft) ? self.rightRoot.position : self.leftRoot.position);
        float xDelta = isLeft ? 200f : -200f;//new
        self.transform.AddPosition(xDelta, -200f);//new
        Vector3 dir = ((!isLeft) ? Vector3.left : Vector3.right);
        yield return parent.animator.WaitForAnimationToEnd(self, "Morph_Start" + ((!isLeft) ? "_Right" : "_Left"));
        parent.animator.SetTrigger("OnDragonAttack");
        self.children.gameObject.SetActive(value: true);
        self.StartCoroutine(dragonFireballs_cr(self, isLeft));//new
        while (self.state == DevilLevelDragonHead.State.Moving)
        {
            self.transform.position += dir * (self.speed * CupheadTime.FixedDelta) * parent.animator.speed;
            yield return wait;
        }

        yield return parent.animator.WaitForAnimationToEnd(self, "Morph_Attack", 1);
        self.state = DevilLevelDragonHead.State.Idle;
        self.children.gameObject.SetActive(value: false);
    }

    private IEnumerator dragonFireballs_cr(DevilLevelDragonHead self, bool left)//new
    {
        float start = left ? -650f : 650f;
        float delta = left ? 200f : -200f;
        //yield return CupheadTime.WaitForSeconds(self, 1f);
        for (int i = 0; i < 8; i++)
        {
            yield return CupheadTime.WaitForSeconds(self, 0.5f);
            float x = start + delta * i;
            DevilLevelFireball proj = Devil.devil.giantHead.fireballPrefab.Create(x, -1000f, 0f, 0.6f);
            proj.transform.position = new Vector2(x, -300f);
            proj.transform.SetEulerAngles(0f, 0f, 180f);
        }
    }
}
