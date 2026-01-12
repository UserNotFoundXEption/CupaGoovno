using UnityEngine;
using static CupaGoovno.MoaiProperties;

namespace CupaGoovno;

public class MoaiLevelBaseball : CustomLevelSummon
{
    public static MoaiLevelBaseball Create(float parameter)
    {
        MoaiLevelBaseball baseball = Create<MoaiLevelBaseball>(prefab);

        baseball.transform.position = new Vector2(-800f, p.y.min);
        baseball.transform.SetScale(p.scale, p.scale);
        baseball.t = 0f;
        baseball.maxHp = p.hp / p.hpMultiplier.GetFloatAt(parameter);
        baseball.hp = baseball.maxHp;
        
        float speedMultiplier = p.speedMultiplier.GetFloatAt(parameter);
        baseball.speed = p.speed * speedMultiplier;
        baseball.swingTime = p.swingTime / speedMultiplier;

        baseball.bat = baseball.transform.Find("BaseballBatPivot");
        baseball.bat.gameObject.layer = LayerMask.NameToLayer("Projectile");
        baseball.bat.gameObject.tag = "EnemyProjectile";

        Transform bat = MoaiLevelBaseballBat.Create().transform;
        bat.transform.SetParent(baseball.bat, false);
        bat.transform.SetLocalPosition(0f, 150f);

        

        return baseball;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        transform.AddPosition(speed * CupheadTime.FixedDelta);
        transform.SetEulerAngles(null, null, 180f);

        float y = transform.position.y;
        float targetY = Mathf.Lerp(p.y.max, p.y.min, hp / maxHp);
        if(targetY > y || hp <= 0f)
        {
            transform.AddPosition(0f, Mathf.Abs(speed) * CupheadTime.FixedDelta);
        }

        float x = transform.position.x;
        bool tooFarRight = x > p.x.max && speed > 0f;
        bool tooFarLeft = x < p.x.min && speed < 0f;
        if(tooFarRight || tooFarLeft)
        {
            if(t < p.time)
            {
                speed = -speed;
            }
            else if(x < Level.Current.Left - 200f || x > Level.Current.Right + 200f)
            {
                Destroy(gameObject);
            }
        }

        t += CupheadTime.FixedDelta;
        float angle = Mathf.Sin(t * Mathf.PI / swingTime) * p.swingAngle + 180f;
        bat.SetEulerAngles(null, null, angle);
    }

    public override void Die()
    {
        Destroy(gameObject, 2f);
    }

    public static GameObject prefab;

    private float speed;
    private float swingTime;
    private float t;
    private float maxHp;
    private Transform bat;
    private static MoaiProperties.Baseball p = new();
}
