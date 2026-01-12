using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CupaGoovno;

public static class CustomWeaponProperties
{
    public static class Striker
    {
        public static class Basic
        {
            public static float damage = 2f;
        }
        public static class Ex
        {
            public static float damage = 15f;//8.334f
            public static float maxDamage = 50f;//25f
        }
    }

    public static class Skytickler
    {
        public static class Basic
        {
            public static float[] xSpeed = [-210f, 273f, 31f];
        }
    }

    public static class Mangetsunami
    {
        public static class Basic
        {
            public static float fireRate = 0.25f;
            public static float chargeTime = 1f;
            public static float damageUncharged = 5f;
            public static float damageCharged = 15f;
            public static float speed = 1000f;
            public static int maxBounces = 2;
            public static float bounceMultiplier = 1.5f;
            public static float unchargedScale = 0.6f;
            public static float chargedScale = 1f;
            public static float spreadAngle = 15f;
        }

        public static class Ex
        {
            public static float startingDamage = 25f;
            public static float startingSpeed = 200f;
            public static float damageAfterDash = 50f;
            public static float speedAfterDash = 1000f;
        }
    }

    public static class Peeshooter
    {
        public static class Basic
        {
            public static float damage = 2f;
            public static float strayBulletChance = 0.1f;
            public static MinMax strayBulletAngleDiff = new(15f, 60f);
        }

        public static class Ex
        {
            public static float destroyLifetime = 30f;
            public static float scale = 1.4f;
        }
    }
}
