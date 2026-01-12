using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class SallyBackgroundHandler
{
    public void Init()
    {
        On.SallyStagePlayLevelBackgroundHandler.Start += Start;
        On.SallyStagePlayLevelBackgroundHandler.HaltCoroutines += HaltCoroutines;
        On.SallyStagePlayLevelBackgroundHandler.GetProperties += GetProperties;
        On.SallyStagePlayLevelBackgroundHandler.cupid_check_falling += cupid_check_falling;
        On.SallyStagePlayLevelBackgroundHandler.check_bools_cr += check_bools_cr;
        On.SallyStagePlayLevelBackgroundHandler.RollUpCupids += RollUpCupids;
        On.SallyStagePlayLevelBackgroundHandler.roll_up_cupids_cr += roll_up_cupids_cr;
        On.SallyStagePlayLevelBackgroundHandler.swing_cr += swing_cr;
    }

    private void Start(On.SallyStagePlayLevelBackgroundHandler.orig_Start orig, SallyStagePlayLevelBackgroundHandler self)
    {
        Level.Current.OnLevelStartEvent += self.StartPriestLoop;
        Level.Current.OnLevelStartEvent += self.StartHusbandLoop;
        self.curtainStartPos = self.curtainSprite.position;
        self.curtainShadowStartPos = self.curtainShadow.position;
        self.chandelierStartPosX = self.chandelier.transform.position.x;
        SallyStagePlayLevelBackgroundHandler.HUSBAND_GONE = false;
        /*foreach (SallyStagePlayLevelBackgroundHandler.Cupid cupid in self.cupids)
        {
            cupid.startPosition = cupid.cupidTransform.position;
        }*/
        self.applauseHandler.SlideApplause(true);
    }

    private void HaltCoroutines(On.SallyStagePlayLevelBackgroundHandler.orig_HaltCoroutines orig, SallyStagePlayLevelBackgroundHandler self)
    {
        foreach (Coroutine routine in self.phaseDependentCoroutines)
        {
            if (routine != null)//new
            {//new
                self.StopCoroutine(routine);
            }//new
        }
        self.phaseDependentCoroutines.Clear();
    }

    private IEnumerator swing_cr(On.SallyStagePlayLevelBackgroundHandler.orig_swing_cr orig, SallyStagePlayLevelBackgroundHandler self, object swing)
    {
        yield break;
    }


    public void GetProperties(On.SallyStagePlayLevelBackgroundHandler.orig_GetProperties orig, SallyStagePlayLevelBackgroundHandler self, LevelProperties.SallyStagePlay properties, SallyStagePlayLevel parent)
    {
        self.properties = properties;
        self.parent = parent;
        foreach (SpriteRenderer flicker in self.flickeringLights)
        {
            self.StartCoroutine(self.flicker_cr(flicker));
        }
        self.phaseDependentCoroutines = new List<Coroutine>();
        /*self.phaseDependentCoroutines.Add(self.StartCoroutine(self.check_bools_cr()));
        for (int j = 0; j < self.cupids.Length; j++)
        {
            self.phaseDependentCoroutines.Add(self.StartCoroutine(self.cupid_check_falling(self.cupids[j])));
        }*/
        foreach (Transform swing in self.churchSwingies)
        {
            self.phaseDependentCoroutines.Add(self.StartCoroutine(self.swing_cr(swing)));
        }
        AbstractPlayerController next = PlayerManager.GetNext();
        LevelPlayerController levelPlayerController = (LevelPlayerController)next;
        levelPlayerController.motor.OnHitEvent += self.PlayYay;
        parent.OnPhase2 += self.OnPhase2;
        parent.OnPhase3 += self.OnPhase3;
        parent.OnPhase4 += self.OnPhase4;
    }

    private IEnumerator cupid_check_falling(On.SallyStagePlayLevelBackgroundHandler.orig_cupid_check_falling orig, SallyStagePlayLevelBackgroundHandler self, SallyStagePlayLevelBackgroundHandler.Cupid cupid)
    {
        yield break;
    }

    private IEnumerator check_bools_cr(On.SallyStagePlayLevelBackgroundHandler.orig_check_bools_cr orig, SallyStagePlayLevelBackgroundHandler self)
    {
        yield break;
    }

    public void RollUpCupids(On.SallyStagePlayLevelBackgroundHandler.orig_RollUpCupids orig, SallyStagePlayLevelBackgroundHandler self)
    {

    }

    private IEnumerator roll_up_cupids_cr(On.SallyStagePlayLevelBackgroundHandler.orig_roll_up_cupids_cr orig, SallyStagePlayLevelBackgroundHandler self, SallyStagePlayLevelBackgroundHandler.Cupid cupid)
    {
        yield break;
    }

}
