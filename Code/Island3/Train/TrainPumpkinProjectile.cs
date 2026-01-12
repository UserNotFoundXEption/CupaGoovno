using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CupaGoovno;

public class TrainPumpkinProjectile
{
    public void Init()
    {
        On.TrainLevelPumpkinProjectile.Start += Start;
        On.TrainLevelPumpkinProjectile.OnCollision += OnCollision;
    }

    protected void Start(On.TrainLevelPumpkinProjectile.orig_Start orig, TrainLevelPumpkinProjectile self)
    {
        AbstractProj.Start(self);
        self.SetParryable(true);
        self.StartCoroutine(float_cr(self));
    }

    protected void OnCollision(On.TrainLevelPumpkinProjectile.orig_OnCollision orig, TrainLevelPumpkinProjectile self, object hit, CollisionPhase phase)
    {
        if(hit is GameObject hitObject)
        {
            if (hitObject.tag == "Player" || self.CanParry)
            {
                orig(self, hit, phase);
            }
        }
    }

    private IEnumerator float_cr(TrainLevelPumpkinProjectile self)
    {
        float top = self.transform.localPosition.y;
        float bottom = top - 20f;
        float time = 0.4f;
        for (; ; )
        {
            if (horizontalDic.ContainsKey(self) && horizontalDic[self])//new start
            {
                self.StartCoroutine(horizontalMovement_cr(self));
                yield break;
            }
            if(diagonalDic.ContainsKey(self) && diagonalDic[self])
            {
                self.StartCoroutine(diagonalMovement_cr(self));
                yield break;
            }
            if (phase3YDic.ContainsKey(self) && phase3LeftDic.ContainsKey(self))
            {
                self.StartCoroutine(phase3_cr(self));
                yield break;
            }//new end
            yield return self.TweenLocalPositionY(top, bottom, time, EaseUtils.EaseType.easeInOutSine);
            yield return self.TweenLocalPositionY(bottom, top, time, EaseUtils.EaseType.easeInOutSine);
        }
    }

    private IEnumerator horizontalMovement_cr(TrainLevelPumpkinProjectile self)//new
    {
        float startX = 700f;
        float endX = -800f;
        float y = -150f;
        float time = 5f;
        self.transform.SetPosition(startX, y, null);
        yield return self.TweenLocalPositionX(startX, endX, time, EaseUtils.EaseType.linear);
        self.Die();
        yield break;
    }

    private IEnumerator diagonalMovement_cr(TrainLevelPumpkinProjectile self)//new
    {
        Vector2 startPos = new Vector2(700f, 400f);
        Vector2 endPos = GameObject.Find("Valve_Right").transform.position + new Vector3(0f, 100f);
        Vector2 direction = endPos - startPos;
        float angle = MathUtils.DirectionToAngle(direction);
        self.transform.SetEulerAngles(null, null, angle + 180f);
        float time = 5f;
        self.transform.position = startPos;
        yield return self.TweenLocalPosition(startPos, endPos, time, EaseUtils.EaseType.linear);
        self.Die();
        yield break;
    }

    private IEnumerator phase3_cr(TrainLevelPumpkinProjectile self)//new
    {
        self.SetParryable(false);
        float startX = phase3LeftDic[self] ? 700f : -700f;
        float endX = -startX;
        float y = phase3YDic[self];
        Vector2 startPos = new Vector2(startX, y);
        Vector2 endPos = new Vector2(endX, y);
        float time = 2f;
        self.transform.position = startPos;
        yield return self.TweenLocalPosition(startPos, endPos, time, EaseUtils.EaseType.linear);
        self.Die();
        yield break;
    }

    public static Dictionary<TrainLevelPumpkinProjectile, bool> horizontalDic = new Dictionary<TrainLevelPumpkinProjectile, bool>();
    public static Dictionary<TrainLevelPumpkinProjectile, bool> diagonalDic = new Dictionary<TrainLevelPumpkinProjectile, bool>();
    public static Dictionary<TrainLevelPumpkinProjectile, float> phase3YDic = new Dictionary<TrainLevelPumpkinProjectile, float>();
    public static Dictionary<TrainLevelPumpkinProjectile, bool> phase3LeftDic = new Dictionary<TrainLevelPumpkinProjectile, bool>();
}
