using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class BaronessPeppermint
{
    public void Init()
    {
        On.BaronessLevelPeppermint.Awake += Awake;
        On.BaronessLevelPeppermint.move_cr += move_cr;
    }

    protected void Awake(On.BaronessLevelPeppermint.orig_Awake orig, BaronessLevelPeppermint self)
    {
        orig(self);
        self.transform.SetScale(1.5f, 1.5f, null);
    }

    private IEnumerator move_cr(On.BaronessLevelPeppermint.orig_move_cr orig, BaronessLevelPeppermint self)
    {
        float offsetX = 220f;
        Vector3 pos = self.transform.position;
        for (; ; )
        {
            if (self.transform.position.x > -640f - offsetX)
            {
                pos.x = Mathf.MoveTowards(self.transform.position.x, -640f - offsetX, self.speed * CupheadTime.FixedDelta);
            }
            else
            {
                //this.Die();
                self.StartCoroutine(move_back_cr(self));//new
                yield break;//new
            }
            self.transform.position = pos;
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator move_back_cr(BaronessLevelPeppermint self)//new
    {
        self.transform.SetScale(-1.5f, -1.5f, null);
        float offsetX = 220f;
        Vector3 pos = self.transform.position;
        pos.y = 200f;
        for (; ; )
        {
            if (self.transform.position.x < 640f + offsetX)
            {
                pos.x = Mathf.MoveTowards(self.transform.position.x, 640f + offsetX, self.speed * CupheadTime.FixedDelta);
            }
            else
            {
                self.Die();
            }
            self.transform.position = pos;
            yield return new WaitForFixedUpdate();
        }
    }
}
