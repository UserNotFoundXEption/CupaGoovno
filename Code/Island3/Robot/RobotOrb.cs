using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class RobotOrb
{
    public void Init()
    {
        On.RobotLevelOrb.move_cr += move_cr;
    }

    private IEnumerator move_cr(On.RobotLevelOrb.orig_move_cr orig, RobotLevelOrb self)
    {
        float clockwise = Rand.Bool() ? 1f : -1f;//new
        for (; ; )
        {
            self.transform.AddEulerAngles(0, 0, clockwise * CupheadTime.Delta * 80f);//new
            if (self.transform.position.x < self.offsetAfterSpawn.x && self.transform.position.y < self.offsetAfterSpawn.y)
            {
                self.transform.position += Vector3.up * (float)self.speed * CupheadTime.Delta * 0.5f;
            }
            self.transform.position += Vector3.left * (float)self.speed * CupheadTime.Delta;
            if (self.transform.position.x < (float)Level.Current.Left - self.GetComponents<BoxCollider2D>()[0].size.x / 2f)
            {
                AudioManager.Stop("robot_orb_spark_loop");
                UnityEngine.Object.Destroy(self.gameObject);
            }
            yield return null;
        }
    }
}
