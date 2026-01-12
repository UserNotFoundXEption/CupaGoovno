using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CupaGoovno;

public class ClownSwing
{
    public void Init()
    {
        On.ClownLevelClownSwing.enemies_cr += enemies_cr;
    }

    private IEnumerator enemies_cr(On.ClownLevelClownSwing.orig_enemies_cr orig, ClownLevelClownSwing self)
    {
        LevelProperties.Clown.Swing p = self.properties.CurrentState.swing;
        string[] enemyPosString = p.positionString[self.eyeMainIndex].Split(new char[]
        {
        ','
        });
        self.state = ClownLevelClownSwing.State.Enemies;
        AudioManager.Play("clown_swing_face_attack_intro");
        self.emitAudioFromObject.Add("clown_swing_face_attack_intro");
        //yield return self.animator.WaitForAnimationToEnd(self, "Face_Attack_Intro", 1, false, true);
        for (int i = 0; i < enemyPosString.Length; i++)
        {
            string[] enemyPos = enemyPosString[i].Split(new char[]
            {
            '-'
            });
            foreach (string pos in enemyPos)
            {
                float targetX = 0f;
                Parser.FloatTryParse(pos, out targetX);
                self.enemy.Create(self.transform.position, targetX, p.HP, p, self);
                yield return CupheadTime.WaitForSeconds(self, p.spawnDelay);
            }
        }
        self.eyeMainIndex = (self.eyeMainIndex + 1) % p.positionString.Length;
        self.animator.SetBool("IsAttacking", false);
        AudioManager.Play("clown_swing_face_attack_outro");
        self.emitAudioFromObject.Add("clown_swing_face_attack_outro");
        yield return null;
        yield break;
    }
}
