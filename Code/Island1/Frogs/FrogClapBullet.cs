using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class FrogClapBullet
{
    public void Init()
    {
        On.FrogsLevelShortClapBullet.go_cr += go_cr;
    }

    private IEnumerator go_cr(On.FrogsLevelShortClapBullet.orig_go_cr orig, FrogsLevelShortClapBullet self)
    {
        YieldInstruction wait = new WaitForFixedUpdate();
        float upY = self.velocity.y;
        float downY = -self.velocity.y;
        float x = (self.frogDirection != FrogsLevelShort.Direction.Right) ? (-self.velocity.x) : self.velocity.x;
        float y = (self.direction != FrogsLevelShortClapBullet.Direction.Up) ? downY : upY;
        if (self.direction == FrogsLevelShortClapBullet.Direction.Up)
        {
            self.transform.LookAt2D(self.transform.position + new Vector3(x, upY));
        }
        else
        {
            self.transform.LookAt2D(self.transform.position + new Vector3(x, downY));
        }
        for (; ; )
        {
            if (self.direction == FrogsLevelShortClapBullet.Direction.Up)
            {
                if (self.transform.position.y >= 360f)
                {
                    AudioManager.Play("level_frogs_short_clap_bounce");
                    self.emitAudioFromObject.Add("level_frogs_short_clap_bounce");
                    self.direction = FrogsLevelShortClapBullet.Direction.Down;
                    self.bounceEffect.Create(self.transform.position, new Vector3(1f, -1f, 1f));
                    y = downY;
                    self.transform.LookAt2D(self.transform.position + new Vector3(x, y));
                }
            }
            else if (self.transform.position.y <= (float)Level.Current.Ground)
            {
                AudioManager.Play("level_frogs_short_clap_bounce");
                self.emitAudioFromObject.Add("level_frogs_short_clap_bounce");
                self.direction = FrogsLevelShortClapBullet.Direction.Up;
                self.bounceEffect.Create(self.transform.position);
                y = upY;
                self.transform.LookAt2D(self.transform.position + new Vector3(x, y));
            }
            /*if (self.transform.position.x > 640f + self.GetComponent<SpriteRenderer>().bounds.size.x / 2f)
            {
                break;
            }
            self.transform.AddPosition(x * CupheadTime.FixedDelta, y * CupheadTime.FixedDelta, 0f);*/
            if(self.transform.position.x > 640f)//new start
            {
                AudioManager.Play("level_frogs_short_clap_bounce");
                self.emitAudioFromObject.Add("level_frogs_short_clap_bounce");
                self.bounceEffect.Create(self.transform.position);
                x = (self.frogDirection != FrogsLevelShort.Direction.Right) ? self.velocity.x : -self.velocity.x;
                self.transform.LookAt2D(self.transform.position + new Vector3(x, y));
            }
            if (self.transform.position.x < -700f)
            {
                break;
            }
            self.transform.AddPosition(x * CupheadTime.FixedDelta, y * CupheadTime.FixedDelta, 0f);//new end
            yield return wait;
        }
        self.Die();
        yield break;
    }
}
