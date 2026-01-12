using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class ClownCoaster
{
    public void Init()
    {
        On.ClownLevelCoaster.move_coaster_front_cr += move_coaster_front_cr;
        On.ClownLevelCoaster.OnCollisionPlayer += OnCollisionPlayer;
    }

    private IEnumerator move_coaster_front_cr(On.ClownLevelCoaster.orig_move_coaster_front_cr orig, ClownLevelCoaster self)
    {
        bool lightsOff = true;
        AudioManager.PlayLoop("sfx_clown_coaster_ratchet_loop");
        self.emitAudioFromObject.Add("sfx_clown_coaster_ratchet_loop");
        yield return CupheadTime.WaitForSeconds(self, self.properties.coasterBackToFrontDelay);
        GameObject tail = self.gameObject.transform.Find("Clown_Coaster_Back(Clone)").gameObject;
        while (tail.transform.position.x > -700f)//new
        //while (self.transform.position.x > -640f - self.coasterSize * self.coasterLength)
        {
            self.transform.position += -self.transform.right * self.properties.coasterSpeed * CupheadTime.Delta;
            if (self.transform.position.x < 640f + 0.2f * self.coasterSize && lightsOff)
            {
                self.warningLights.StartWarningLights();
                lightsOff = false;
            }
            if (self.transform.position.x < -640f - self.coasterSize * self.coasterLength && !lightsOff)
            {
                self.warningLights.StopWarningLights();
                lightsOff = true;
            }
            yield return null;
        }
        self.inView = false;
        AudioManager.Stop("sfx_clown_coaster_ratchet_loop");
        self.Die();
        yield return null;
        yield break;
    }

    public void OnCollisionPlayer(On.ClownLevelCoaster.orig_OnCollisionPlayer orig, ClownLevelCoaster self, object hit2, CollisionPhase phase)
    {
        if(hit2 is GameObject hit)
        {
            LevelPlayerController player = hit.GetComponent<LevelPlayerController>();
            if (player != null)
            {
                orig(self, hit2, phase);
            }
        }
    }
}
