using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class CigarGhost
{
    public void Init()
    {
        On.DicePalaceCigarLevelCigaretteGhost.move_cr += move_cr;
    }

    private IEnumerator move_cr(On.DicePalaceCigarLevelCigaretteGhost.orig_move_cr orig, DicePalaceCigarLevelCigaretteGhost self)
    {
        self.StartCoroutine(self.spawn_fx_cr());
        YieldInstruction wait = new WaitForFixedUpdate();
        //while (self.transform.position.y < 560f)
        float speed = self.properties.CurrentState.cigaretteGhost.verticalSpeed;//new start
        bool left = PlayerManager.GetFirst().transform.position.x > 0;
        self.transform.SetEulerAngles(null, null, left ? -90f : 90f);
        if (!left)
        {
            speed *= -1;
        }//new end
        while ((!left && self.transform.position.x > -1000f) || (left && self.transform.position.x < 1000f))//new
        {
            //self.transform.AddPosition(0f, self.properties.CurrentState.cigaretteGhost.verticalSpeed * CupheadTime.FixedDelta, 0f);
            self.transform.AddPosition(speed * CupheadTime.FixedDelta, 0f, 0f);//new
            yield return wait;
        }
        self.Die();
        GameObject.Destroy(self.gameObject);//new
        yield break;
    }
}
