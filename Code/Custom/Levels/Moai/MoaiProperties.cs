using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CupaGoovno;

public class MoaiProperties
{
    public class Moai
    {
        public float hp = 4321f;
    }

    public class Pollen
    {
        public float linearDrag = 1f;
        public float spawnVelocity = 800f;
        public float gravity = 100f;
        public MinMax spawnX = new(-1350f, -1000f);
        public MinMax spawnY = new(300f, 900f);
        public float spacing = 300f;
        public float randomScatter = 75f;
        public float scale = 1f;
        public float spinSpeed = 20f;
        public float warningTime = 1f;

        public MinMax bonusX = new(0f, 500f);
    }

    public class Spikes
    {
        public float hp = 80f;
        public MinMax y = new(-220f, -150f);
        public float hiddenY = -300f;
        public float enterTime = 0.2f;
        public float time = 8f;
        public float exitTime = 1f;
        public float warningTime = 1f;

        public float middleSpikeX = -200f;
        public MinMax spikeXDelta = new(120f, 350f);
    }

    public class Laser
    {
        public float initialDelay = 1f;
        public float delayBetweenShots = 1f;
        public float fullAttackTime = 9f;
        public float warningTime = 0.7f;
        public float laserTime = 0.3f;
        public float hesitate = 0f;
        public MinMax rightAngleRange = new(-70f, -95f);

        public MinMax delayMultiplier = new(0.7f, 2f);// *
        public MinMax durationMultiplier = new(0.5f, 3f);// *
    }

    public class LaserSpark
    {
        public float linearDrag = 0f;
        public MinMax spawnVelocity = new(500f, 800f);
        public float gravity = 500f;

        public float scale = 1f;
        public float spinSpeed = 1000f;
    }

    public class GiantStone
    {
        public float hp = 300f;
        public float initialDelay = 2f;
        public float timeToBirdOne = 1f;
        public float timeToBirdTwo = 1f;
        public float repeatDelay = 2f;
        public float hesitate = 3f;

        public float birdSpeed = 950f;
        public float stoneSpeed = 300f;

        public float birdScale = 0.15f;
        public float stoneScale = 0.45f;

        public MinMax scaleMultiplier = new(0.8f, 1.4f);// *
        public MinMax healthMultiplier = new(0.666f, 2f);// /
    }

    public class Shitlings
    {
        public float initialDelay = 1f;
        public float shakeTime = 1f;
        public int count = 40;
        public float speed = -500f;
        public float delay = 0.2f;
        public float hesitate = 1f;
        public float scale = 0.3f;

        public MinMax scaleMultiplier = new(1f, 2f);// *
        public MinMax speedMultiplier = new(1f, 1.111f);// /
        public MinMax countMultiplier = new(1f, 3.333f);// /
    }

    public class Crackhead
    {
        public float hp = 75f;
        public float initialDelay = 0f;
        public float time = 8f;
        public float speed = 1000f;
        public float introSpeed = 800f;
        public float introScale = 0.2f;
        public float scale = 0.5f;

        public float wiggleAmount = 40f;
        public float wiggleRotation = 20f;
        public float wiggleSpeed = 20f;

        public MinMax speedMultiplier = new(0.3f, 1.5f);// *
        public MinMax hpMultiplier = new(0.833f, 2f);// /
    }

    public class Rockets
    {
        public float initialDelay = 1.5f;
        public float delay = 1f;
        public float attackTime = 8f;
        public float speed = -800f;
        public float hesitate = 1f;
        public float scale = 1f;
        public float yDifference = 110f;

        public MinMax speedMultiplier = new(0.5f, 1.5f);// *
        public MinMax delayMultplier = new(1f, 2.5f);// /
    }

    public class Bouncers
    {
        public float hp = 15f;
        public float initialDelay = 0f;
        public int count = 5;
        public float xSpeed = -300f;
        public float gravity = 1000f;
        public float delay = 1.8f;
        public float scale = 1.2f;
        public MinMax startX = new(800f, 1000f);
        public MinMax startY = new(300f, 400f);

        public MinMax gravityMultiplier = new(0.5f, 2f);// *
        public MinMax hpMultiplier = new(0.5f, 1.333f);// /
    }

    public class Pusher
    {
        public float initialDelay = 0f;
        public int pushes = 7;
        public float startingAcceleration = 500f;
        public float accelerationIncrease = 500f;
        public float velocityAfterParry = -2000f;
        public float velocityDamageMultiplier = 10f;
        public float hesitate = 2f;
        public float scale = 1f;
        public float timeToDamageImmunity = 10f;
        public float damageImmunityTransitionTime = 2f;

        public MinMax startingAccelerationMultiplier = new(0.3f, 2f);// *
        public MinMax accelerationIncreaseMultiplier = new(0.666f, 1.666f);// /
    }

    public class Baseball
    {
        public float hp = 50f;
        public float initialDelay = 0f;
        public float time = 9f;
        public float speed = 500f;
        public float swingTime = 1f;
        public float swingAngle = 40f;
        public MinMax x = new(-600f, 400f);
        public MinMax y = new(280f, 380f);
        public float scale = 0.7f;
        public float batScale = 0.8f;

        public MinMax speedMultiplier = new(0.5f, 2f);// *
        public MinMax hpMultiplier = new(0.5f, 2f);// /
    }

    public class Heart
    {
        public float initialDelay = 2f;
        public float speed = -300f;
        public float sinusTime = 2f;
        public float startX = 800f;
        public MinMax y = new(-200f, 300f);
        public MinMax delay = new(2f, 4f);
        public float scale = 0.6f;
    }
}
