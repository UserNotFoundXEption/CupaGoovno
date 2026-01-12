using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CupaGoovno;

public class CustomCharmProperties
{
    public class Milk
    {
        public float speed = 1200f;
        public float bonusVelY = 300f;
        public float gravity = 500f;
        public float damage = 50f;
        public float scale = 0.8f;
        public float normalSpawnDelay = 0.28f;
        public float planeSpawnDelay = 0.54f;
    }

    public class Balance
    {
        public float percentPerDamage = 0.1f;
        public float passiveBoost = 0.2f;
    }
}
