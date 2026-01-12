using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class DominoBoomerang
{
    public void Init()
    {
        On.DicePalaceDominoLevelBoomerang.move_cr += move_cr;
    }

    private IEnumerator move_cr(On.DicePalaceDominoLevelBoomerang.orig_move_cr orig, DicePalaceDominoLevelBoomerang self)
    {
        float dropPoint = (float)Level.Current.Ground + self.GetComponent<Collider2D>().bounds.size.y;
        /*float goToPos = -440f;
        while (self.transform.position.x > goToPos)
        {
            self.transform.position += Vector3.left * self.speed * CupheadTime.Delta;
            yield return null;
        }*/
        self.animator.SetTrigger("OnDrop");
        yield return self.animator.WaitForAnimationToStart(self, "Fly_Drop_Start", false);
        AudioManager.Play("dice_palace_domino_bird_dive");
        self.emitAudioFromObject.Add("dice_palace_domino_bird_dive");
        while (self.transform.position.y > dropPoint)
        {
            self.transform.position += Vector3.down * self.speed * CupheadTime.Delta;
            yield return null;
        }
        //self.animator.SetTrigger("OnStop");
        self.Die();//new
        yield return null;
        yield break;
    }
}
