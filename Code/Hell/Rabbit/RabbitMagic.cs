using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class RabbitMagic
{
    public void Init()
    {
        On.DicePalaceRabbitLevelMagic.move_cr += move_cr;
    }

    private IEnumerator move_cr(On.DicePalaceRabbitLevelMagic.orig_move_cr orig, DicePalaceRabbitLevelMagic self, float startX, bool left, float speed)
    {
        self.StartMagicSFX = false;
        self.StartMagicLaserSFX = false;
        //Vector3 velocity = speed * ((!down) ? Vector3.up : Vector3.down);
        Vector3 velocity = speed * (left ? Vector3.right : Vector3.left);//new
        float progress = 0f;
        //while ((!left && startY + progress < 360f) || (left && startY + progress > -360f))
        while ((!left && startX + progress > -700f) || (left && startX + progress < 700f))
        {
            self.transform.position += velocity * CupheadTime.Delta;
            //progress += velocity.y * CupheadTime.Delta;
            progress += velocity.x * CupheadTime.Delta;//new
            self.circleCollider.enabled = true;//new
            yield return null;
        }
        UnityEngine.Object.Destroy(self.gameObject);
        yield break;
    }
}
