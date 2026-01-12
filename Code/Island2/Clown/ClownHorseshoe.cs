using System.Collections;
using UnityEngine;

namespace CupaGoovno;

public class ClownHorseshoe
{
    public void Init()
    {
        On.ClownLevelHorseshoe.move_to_drop_point_cr += move_to_drop_point_cr;
    }

    private IEnumerator move_to_drop_point_cr(On.ClownLevelHorseshoe.orig_move_to_drop_point_cr orig, ClownLevelHorseshoe self)
    {
        Vector3 pos = self.transform.position;
        if (self.onRight)
        {
            float leavePos = -740f;
            while (self.transform.position.x > leavePos)
            {
                self.transform.AddPosition(-self.velocityX * CupheadTime.Delta, 0f, 0f);
                yield return null;
            }
            pos.x = -740f;
        }
        else
        {
            float leavePos = 740f;
            while (self.transform.position.x < leavePos)
            {
                self.transform.AddPosition(self.velocityX * CupheadTime.Delta, 0f, 0f);
                yield return null;
            }
            pos.x = 740f;
        }

        LevelProperties.Clown.States state = YoMamaFat.clownProperties.CurrentState.stateName;//new start
        bool main = state == LevelProperties.Clown.States.Main;
        bool helium = state == LevelProperties.Clown.States.HeliumTank;
        if (main || helium)
        {
            GameObject.Destroy(self.gameObject);
        }//new end

        pos.y = 260f;
        self.transform.position = pos;
        float dropPos = self.onRight ? (640f - self.velocityY) : (-640f + self.velocityY);
        self.animator.SetTrigger("onTop");
        yield return CupheadTime.WaitForSeconds(self, self.properties.DropBulletDelay);
        while (self.transform.position.x != dropPos)
        {
            pos.x = Mathf.MoveTowards(self.transform.position.x, dropPos, self.velocityX * CupheadTime.Delta);
            self.transform.position = pos;
            yield return null;
        }
        self.isSparkling = false;
        yield return CupheadTime.WaitForSeconds(self, self.durationBeforeDrop);
        self.isSparkling = true;
        self.animator.SetTrigger("down");
        AudioManager.Play("clown_horseshoe_drop");
        self.emitAudioFromObject.Add("clown_horseshoe_drop");
        while (self.transform.position.y > (float)Level.Current.Ground)
        {
            pos.y -= self.properties.DropBulletSpeedDown * CupheadTime.Delta;
            self.transform.position = pos;
            yield return null;
        }
        AudioManager.Play("clown_horseshoe_land");
        self.emitAudioFromObject.Add("clown_horseshoe_land");
        self.animator.SetTrigger("dead");
        self.deathPoof.Create(self.transform.position);
        yield return null;
        yield break;
    }
}
