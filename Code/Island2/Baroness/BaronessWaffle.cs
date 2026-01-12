using Blender.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class BaronessWaffle
{
    public void Init()
    {
        On.BaronessLevelWaffle.Awake += Awake;
        On.BaronessLevelWaffle.CheckIfTurn += CheckIfTurn;
        On.BaronessLevelWaffle.Turn += Turn;
        On.BaronessLevelWaffle.attack_cr += attack_cr;
        Other.LoadCustomAsset(waffelSfxPath);
    }

    protected void Awake(On.BaronessLevelWaffle.orig_Awake orig, BaronessLevelWaffle self)
    {
        orig(self);
        diagonal = true;
        self.transform.SetScale(0.5f, 0.5f, null);
        Other.PlayCustomSfx(waffelSfxPath, 0.3f);
    }

    private void CheckIfTurn(On.BaronessLevelWaffle.orig_CheckIfTurn orig, BaronessLevelWaffle self)
    {
        self.StartCoroutine(self.turn_cr());
    }

    private void Turn(On.BaronessLevelWaffle.orig_Turn orig, BaronessLevelWaffle self)
    {
        self.transform.SetScale(new float?(-self.transform.localScale.x), null, null);
        //self.transform.SetScale(new float?(-self.transform.localScale.x), new float?(1f), new float?(1f));
    }

    private IEnumerator attack_cr(On.BaronessLevelWaffle.orig_attack_cr orig, BaronessLevelWaffle self)
    {
        if (!self.isDead)
        {
            self.animator.Play("Waffle_Tuck_Start");
            /*self.GetComponent<Collider2D>().enabled = false;
            float randomValue = (float)UnityEngine.Random.Range(0, 2);
            self.diagFirst = (randomValue == 0f);
            yield return CupheadTime.WaitForSeconds(self, self.properties.anticipation);
            self.animator.SetTrigger("Continue");
            self.StartCoroutine(self.waffle_pieces((!self.diagFirst) ? self.straightPieces : self.diagonalPieces, true));
            yield return CupheadTime.WaitForSeconds(self, self.properties.explodeTwoDuration);
            self.StartCoroutine(self.waffle_pieces((!self.diagFirst) ? self.diagonalPieces : self.straightPieces, false));*/
            yield return CupheadTime.WaitForSeconds(self, self.properties.anticipation); //new start
            self.animator.SetTrigger("Continue");
            self.StartCoroutine(self.waffle_pieces((diagonal) ? self.diagonalPieces : self.straightPieces, false));
            diagonal = !diagonal;//new end
        }
        yield break;
    }

    private bool diagonal;
    private string waffelSfxPath = "CupaGoovno:cupagoovno\\waffel_sound";
}
