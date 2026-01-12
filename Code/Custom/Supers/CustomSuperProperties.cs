using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CupaGoovno;

public class CustomSuperProperties
{
    public class Berserker
    {
        public float timeIn = 0.5f;
        public float timeOut = 1f;
        public float duration = 10f;
        public float timeSpeedMultiplier = 1.2f;
        public static float damageMultiplier = 0.75f;
    }

    public class Sandevistan
    {
        public float timeIn = 1f;
        public float timeOut = 2f;
        public float duration = 25f;
        public float durationRunNGun = 5f;
        public float timeSpeedMultiplier = 0.3f;
        public float maxDamage = 150f;
    }

    public class BooFist
    {
        public float destroyLifetime = 2137f;
        public float scale = 1.5f;
        public int maxBlockedProjectiles = 10;
        public float fallingSpeed = 2137f;
    }
}
