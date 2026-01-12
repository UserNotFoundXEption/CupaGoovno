using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class Ball
{
    public void Init()
    {
        On.DicePalaceEightBallLevelEightBall.shoot_bullet_cr += shoot_bullet_cr;
    }

    private IEnumerator shoot_bullet_cr(On.DicePalaceEightBallLevelEightBall.orig_shoot_bullet_cr orig, DicePalaceEightBallLevelEightBall self)
    {
        LevelProperties.DicePalaceEightBall.General p = self.properties.CurrentState.general;
        //string[] projectileType = p.shootString.GetRandom<string>().Split(new char[]{','});
        //int projectileIndex = UnityEngine.Random.Range(0, projectileType.Length);
        for (; ; )
        {
            yield return CupheadTime.WaitForSeconds(self, p.shootDelay);
            /*self.animator.SetTrigger("OnAttack");
            yield return self.animator.WaitForAnimationToStart(self, "Attack_Start", false);
            AudioManager.Play("dice_palace_eight_ball_attack_start");
            self.emitAudioFromObject.Add("dice_palace_eight_ball_attack_start");
            yield return self.animator.WaitForAnimationToEnd(self, "Attack_Start", false, true);
            Effect effect = UnityEngine.Object.Instantiate<Effect>(self.projectileEffect);
            effect.transform.position = self.root.transform.position;
            yield return effect.GetComponent<Animator>().WaitForAnimationToEnd(self, "Projectile", false, true);
            AbstractPlayerController player = PlayerManager.GetNext();
            Vector3 dir = player.transform.position - self.transform.position;
            AudioManager.Play("dice_palace_eight_ball_eight_attack_fire");
            self.emitAudioFromObject.Add("dice_palace_eight_ball_eight_attack_fire");
            if (projectileType[projectileIndex][0] == 'R')
            {
                self.attackEffect.Create(self.root.transform.position);
                self.projectile.Create(self.root.transform.position, MathUtils.DirectionToAngle(dir), self.properties.CurrentState.general.shootSpeed);
            }
            else if (projectileType[projectileIndex][0] == 'P')
            {
                self.attackEffect.Create(self.root.transform.position);
                self.pinkProjectile.Create(self.root.transform.position, MathUtils.DirectionToAngle(dir), self.properties.CurrentState.general.shootSpeed);
            }
            projectileIndex = (projectileIndex + 1) % projectileType.Length;
            yield return CupheadTime.WaitForSeconds(self, p.attackDuration);
            self.animator.SetTrigger("OnEnd");
            AudioManager.Play("dice_palace_eight_ball_attack_end");
            self.emitAudioFromObject.Add("dice_palace_eight_ball_attack_end");
            yield return null;*/
            Vector2 pos = new Vector2(700f, -200f);//new start
            float angle = 180f;
            float speed = self.properties.CurrentState.general.shootSpeed;
            BasicProjectile projectile = self.projectile.Create(pos, angle, speed);
            projectile.transform.SetScale(2f, 2f);//new end
        }
    }
}
