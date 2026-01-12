using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class DevilFireball
{
    public static DevilLevelFireball Create(DevilLevelFireball self, float xPos, float speed, float gravity, float xScale)
    {
        DevilLevelFireball devilLevelFireball = self.InstantiatePrefab<DevilLevelFireball>();
        devilLevelFireball.transform.position = new Vector2(xPos, 2137f);
        devilLevelFireball.yVelocity = -speed;
        devilLevelFireball.gravity = gravity;
        devilLevelFireball.transform.SetScale(new float?(xScale), null, null);
        return devilLevelFireball;
    }
}
