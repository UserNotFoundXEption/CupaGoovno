using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static Rewired.ComponentControls.Effects.RotateAroundAxis;

namespace CupaGoovno;

public class BlimpUFO
{
    public void Init()
    {
        On.FlyingBlimpLevelUFO.Awake += Awake;
        On.FlyingBlimpLevelUFO.move_cr += move_cr;
    }

    protected void Awake(On.FlyingBlimpLevelUFO.orig_Awake orig, FlyingBlimpLevelUFO self)
    {
        orig(self);
        SpriteRenderer spriteRenderer = self.GetComponent<SpriteRenderer>();//new start
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingLayerName = "UI";
            spriteRenderer.sortingOrder = int.MaxValue;
        }//new end
    }

    private IEnumerator move_cr(On.FlyingBlimpLevelUFO.orig_move_cr orig, FlyingBlimpLevelUFO self)
    {
        YieldInstruction wait = new WaitForFixedUpdate();
        float offset = 50f;
        if (self.speed > 0)//new
        {//new
            while (self.transform.position.x > -640f - offset)
            {
                self.player = PlayerManager.GetNext();
                float dist = self.player.transform.position.x - self.transform.position.x;
                Vector3 pos = self.transform.position;
                pos.x += -self.speed * CupheadTime.FixedDelta;
                self.transform.position = pos;
                self.proximity = ((!self.typeB) ? self.properties.UFOProximityA : self.properties.UFOProximityB);
                if (dist > -self.proximity && dist < self.proximity && !self.beamTriggered)
                {
                    self.beamTriggered = true;
                    self.StartCoroutine(self.ActivateBeam());
                }
                yield return wait;
            }
        }//new start
        else
        {
            while (self.transform.position.x < 640f + offset)
            {
                self.player = PlayerManager.GetNext();
                float dist = self.player.transform.position.x - self.transform.position.x;
                Vector3 pos = self.transform.position;
                pos.x += -self.speed * CupheadTime.FixedDelta;
                self.transform.position = pos;
                self.proximity = ((!self.typeB) ? self.properties.UFOProximityA : self.properties.UFOProximityB);
                if (dist > -self.proximity && dist < self.proximity && !self.beamTriggered)
                {
                    self.beamTriggered = true;
                    self.StartCoroutine(self.ActivateBeam());
                }
                yield return wait;
            }
        }//new end
        self.Die();
        yield break;
    }
}
