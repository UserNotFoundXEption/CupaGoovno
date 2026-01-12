using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class RobotHead
{
    public void Init()
    {
        On.RobotLevelRobotHead.warningLaser_cr += warningLaser_cr;
        On.RobotLevelRobotHead.attackLaser_cr += attackLaser_cr;
        On.RobotLevelRobotHead.cannonSpreadShot += cannonSpreadShot;
    }

    private IEnumerator warningLaser_cr(On.RobotLevelRobotHead.orig_warningLaser_cr orig, RobotLevelRobotHead self)
    {
        if (self.current == RobotLevelRobotBodyPart.state.primary)
        {
            //yield return CupheadTime.WaitForSeconds(self, self.properties.CurrentState.hose.warningDuration);
            if (self.current == RobotLevelRobotBodyPart.state.primary)
            {
                if (self.currentPlayer == null || self.currentPlayer.IsDead)
                {
                    self.currentPlayer = PlayerManager.GetNext();
                }
                Vector3 dir = (self.currentPlayer.center - self.transform.position).normalized;
                self.angle = Vector3.Angle(Vector3.up, dir);
                if (self.angle < 0f)
                {
                    self.angle *= -1f;
                }
                self.angle = Mathf.Clamp(self.angle, self.properties.CurrentState.hose.aimAngleParameter.min, self.properties.CurrentState.hose.aimAngleParameter.max);
                yield return null;
                self.laser = self.primary.GetComponent<RobotLevelHoseLaser>().Create(self.transform.position, self.angle - 90f, self);
                self.laser.animator.SetTrigger("OnWarning");
                AudioManager.Play("robot_raygun_charge");
                self.emitAudioFromObject.Add("robot_raygun_charge");
            }
            yield return CupheadTime.WaitForSeconds(self, self.properties.CurrentState.hose.warningDuration);
            if (self.current == RobotLevelRobotBodyPart.state.primary)
            {
                self.laser.animator.SetTrigger("OnAttack");
                AudioManager.Play("robot_raygun_shoot");
                self.emitAudioFromObject.Add("robot_raygun_shoot");
                yield return null;
            }
            else
            {
                AudioManager.Stop("robot_raygun_charge");
            }
            yield return null;
        }
        if (self.current == RobotLevelRobotBodyPart.state.primary)
        {
            self.StartCoroutine(self.attackLaser_cr());
        }
        yield break;
    }

    private IEnumerator attackLaser_cr(On.RobotLevelRobotHead.orig_attackLaser_cr orig, RobotLevelRobotHead self)
    {
        //yield return CupheadTime.WaitForSeconds(self, (float)self.properties.CurrentState.hose.beamDuration);
        yield return CupheadTime.WaitForSeconds(self, (float)self.properties.CurrentState.hose.beamDuration / 10);
        AudioManager.Stop("robot_raygun_charge");
        if (self.laser != null)
        {
            UnityEngine.Object.Destroy(self.laser.gameObject);
            self.isAttacking = false;
        }
        if ((float)UnityEngine.Random.Range(0, 100) <= 25f && !AudioManager.CheckIfPlaying("robot_vocals_laugh"))
        {
            AudioManager.Play("robot_vocals_laugh");
            self.emitAudioFromObject.Add("robot_vocals_laugh");
        }
        yield break;
    }

    private void cannonSpreadShot(On.RobotLevelRobotHead.orig_cannonSpreadShot orig, RobotLevelRobotHead self, string attackString)
    {
        int num = 0;
        Parser.IntTryParse(attackString.Substring(1), out num);
        num--;
        string[] array = self.properties.CurrentState.cannon.spreadVariableGroups[num].Split(new char[]
        {
        ','
        });
        float speed = 0f;
        int num2 = 0;
        MinMax minMax = new MinMax(0f, 0f);
        foreach (string text in array)
        {
            if (text[0] == 'S')
            {
                Parser.FloatTryParse(text.Substring(1), out speed);
            }
            else if (text[0] == 'N')
            {
                Parser.IntTryParse(text.Substring(1), out num2);
            }
            else
            {
                string[] array3 = text.Split(new char[]
                {
                '-'
                });
                Parser.FloatTryParse(array3[0], out minMax.min);
                Parser.FloatTryParse(array3[1], out minMax.max);
            }
        }
        AudioManager.Play("robot_head_shoot");
        self.emitAudioFromObject.Add("robot_head_shoot");
        /*for (int j = 0; j < num2; j++)
        {
            float floatAt = minMax.GetFloatAt((float)j / ((float)num2 - 1f));
            if (j % 2 == 0)
            {
                BasicProjectile component = self.secondary.GetComponent<BasicProjectile>();
                component.Create(self.transform.position, floatAt, speed);
            }
            else
            {
                self.nutProjectile.Create(self.transform.position, floatAt, speed);
            }
        }*/
        FireRandom(self, speed, num2, minMax);//new
    }

    private void FireRandom(RobotLevelRobotHead self, float speed, float count, MinMax angleRange)//new
    {
        switch (UnityEngine.Random.Range(0, 3))
        {
            case 0:
                FireShotgun(self, speed, count, angleRange);
                break;
            case 1:
                FireReverseShotgun(self, speed, count, angleRange);
                break;
            case 2:
                FireRegular(self, speed, count, angleRange);
                break;
        }
    }

    private void FireShotgun(RobotLevelRobotHead self, float speed, float count, MinMax angleRange)//new
    {
        float randomAngleChange = UnityEngine.Random.Range(-25f, 25f);
        float center = (angleRange.max + angleRange.min) / 2;
        float centerToMax = angleRange.max - center;
        float centerToMin = angleRange.min - center;
        MinMax newAngleRange = new MinMax(center - centerToMin / 4, center - centerToMax / 4);
        for (int j = 0; j < count; j++)
        {
            float angle = newAngleRange.GetFloatAt((float)j / ((float)count - 1f));
            FireProjectile(self, speed, angle + randomAngleChange, j);
        }
    }

    private void FireReverseShotgun(RobotLevelRobotHead self, float speed, float count, MinMax angleRange)//new
    {
        float randomAngleChange = UnityEngine.Random.Range(-25f, 25f);
        float center = (angleRange.max + angleRange.min) / 2;
        float centerToMax = angleRange.max - center;
        float centerToMin = angleRange.min - center;
        MinMax angleRange1 = new MinMax(angleRange.min, angleRange.min - centerToMin / 1.5f);
        MinMax angleRange2 = new MinMax(angleRange.max - centerToMax / 1.5f, angleRange.max);
        for (int j = 0; j < count; j++)
        {
            float angle = angleRange1.GetFloatAt((float)j / ((float)count - 1f));
            FireProjectile(self, speed, angle + randomAngleChange, j);
        }
        for (int j = 0; j < count; j++)
        {
            float angle = angleRange2.GetFloatAt((float)j / ((float)count - 1f));
            FireProjectile(self, speed, angle + randomAngleChange, j);
        }

    }

    private void FireRegular(RobotLevelRobotHead self, float speed, float count, MinMax angleRange)//new
    {
        for (int j = 0; j < count; j++)
        {
            float angle = angleRange.GetFloatAt((float)j / ((float)count - 1f));
            FireProjectile(self, speed, angle, j);
        }
    }

    private void FireProjectile(RobotLevelRobotHead self, float speed, float angle, int number)
    {
        if (number % 2 == 0)
        {
            BasicProjectile component = self.secondary.GetComponent<BasicProjectile>();
            component.Create(self.transform.position, angle, speed);
        }
        else
        {
            self.nutProjectile.Create(self.transform.position, angle, speed);
        }
    }
}
