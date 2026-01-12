using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class BaronessPlatform
{
    public void Init()
    {
        On.BaronessLevelPlatform.move_cr += move_cr;
    }

    private IEnumerator move_cr(On.BaronessLevelPlatform.orig_move_cr orig, BaronessLevelPlatform self)
    {
        bool movingLeft = true;
        controlledByGumball = false;//new
        for (; ; )
        {
            if (controlledByGumball)//new start
            {
                Vector3 pos = self.transform.position;
                float gumballX = BaronessGumball.gumball.transform.position.x;
                float targetX = movingLeft ? gumballX - 200f : gumballX + 200f;
                pos.x = Mathf.MoveTowards(pos.x, targetX, self.speed * CupheadTime.Delta * 3);
                if(pos.x == targetX)
                {
                    movingLeft = !movingLeft;
                }
                self.transform.position = pos;
            }
            else
            {
                if (self.castle.state == BaronessLevelCastle.State.Chase)
                {
                    Other.MakePlayersFatherless();
                    self.transform.position = new Vector3(-1000f, 0f, 0f);
                    yield break;
                }
                else
                {//new end
                    Vector3 pos = self.transform.position;
                    if (movingLeft)
                    {
                        /*if (self.castle.state == BaronessLevelCastle.State.Chase)
                        {
                            self.animator.Play("Fast");
                        }*/
                        pos.x = Mathf.MoveTowards(self.transform.position.x, -640f + self.properties.LeftBoundaryOffset, self.speed * CupheadTime.Delta * 3);//speed x3
                        movingLeft = (self.transform.position.x != -640f + self.properties.LeftBoundaryOffset);
                    }
                    else
                    {
                        /*if (self.castle.state == BaronessLevelCastle.State.Chase)
                        {
                            self.animator.Play("Slow");
                        }*/
                        pos.x = Mathf.MoveTowards(self.transform.position.x, (float)Level.Current.Right - self.properties.RightBoundaryOffset, self.speed * CupheadTime.Delta * 3);//speed x3
                        movingLeft = (self.transform.position.x == (float)Level.Current.Right - self.properties.RightBoundaryOffset);
                    }
                    self.transform.position = pos;
                }//new
            }//new
            yield return null;
        }
    }

    public static bool controlledByGumball;
}
