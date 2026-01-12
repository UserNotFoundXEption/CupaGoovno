using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class MermaidSkullBubble
{
    public static FlyingMermaidLevelSkullBubble CreateBubble(FlyingMermaidLevelSkullBubble self, Vector2 pos, float velocity, float sinVelocity, float sinSize, float rotation)
    {
        FlyingMermaidLevelSkullBubble projectile;
        if (MermaidHead.nextBubbleBig)
        {
            projectile = self.Create(pos + new Vector2(1500f, 0f), rotation, velocity * 2f, 0f, 0f) as FlyingMermaidLevelSkullBubble;
            projectile.transform.SetScale(10f, 10f, null);
        }
        else
        {
            projectile = self.Create(pos, rotation, velocity, sinVelocity, sinSize) as FlyingMermaidLevelSkullBubble;
            BasicProjectile eelProjectile = MermaidEel.bulletPrefab.Create(pos + new Vector2(50f, -10f), rotation, 0f);
            eelProjectile.transform.parent = projectile.transform;
            eelProjectile.SetParryable(true);
        }
        return projectile;
    }
}
