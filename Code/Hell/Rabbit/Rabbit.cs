using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class Rabbit
{
    public void Init()
    {
        new RabbitMagic().Init();
        On.DicePalaceRabbitLevelRabbit.magicwand_cr += magicwand_cr;
        On.DicePalaceRabbitLevelRabbit.orbs_cr += orbs_cr;
        On.DicePalaceRabbitLevelRabbit.collapse_cr += collapse_cr;
        On.DicePalaceRabbitLevelRabbit.magicparry_cr += magicparry_cr;
    }

    private IEnumerator magicwand_cr(On.DicePalaceRabbitLevelRabbit.orig_magicwand_cr orig, DicePalaceRabbitLevelRabbit self)
    {
        int startingHp = PlayerManager.GetFirst().stats.Health;
        targetLocked = false;//new
        self.attacking = true;
        self.state = DicePalaceRabbitLevelRabbit.State.MagicWand;
        yield return CupheadTime.WaitForSeconds(self, self.properties.CurrentState.magicWand.initialAttackDelay);
        AbstractPlayerController player = PlayerManager.GetNext();
        self.animator.SetTrigger("OnAttack");
        string[] safeZoneStrings = self.properties.CurrentState.magicWand.safeZoneString.Split(new char[]{','});
        for (int i = 0; i < 3; i++)//new start
        {
           int safeZone = UnityEngine.Random.Range(0, 4) * 2 + 1;
           self.StartCoroutine(self.orbs_cr(player.id, safeZone));//new end
           /*self.StartCoroutine(self.orbs_cr(player.id, Parser.IntParse(safeZoneStrings[self.playerOneCircleIndex])));
            self.playerOneCircleIndex++;
            if (self.playerOneCircleIndex >= safeZoneStrings.Length)
            {
                self.playerOneCircleIndex = 0;
            }*/
            yield return CupheadTime.WaitForSeconds(self, self.properties.CurrentState.magicWand.attackDelayRange.RandomFloat());
        }//new
        self.attacking = false;
        self.animator.SetTrigger("OnAttackEnd");
        yield return CupheadTime.WaitForSeconds(self, self.properties.CurrentState.magicWand.hesitate);
        self.state = DicePalaceRabbitLevelRabbit.State.Idle;
        yield break;
    }

    private IEnumerator orbs_cr(On.DicePalaceRabbitLevelRabbit.orig_orbs_cr orig, DicePalaceRabbitLevelRabbit self, PlayerId target, int safeZone)
    {
        GameObject centerPoint = new GameObject();
        AbstractPlayerController player = PlayerManager.GetPlayer(target);
        centerPoint.transform.position = player.center;
        if (!targetLocked)//new start
        {
            center = player.center;
        }
        else
        {
            centerPoint.transform.position = center;
        }//new end
        self.currentCenterPoint = centerPoint;
        Vector3 dir = Vector3.up;
        float dist = self.properties.CurrentState.magicWand.circleDiameter / 2f;
        //safeZone = self.GetSafeZone(safeZone);
        Transform[] orbs = new Transform[7];
        int orbsIndex = 0;
        float initialRotation = (float)UnityEngine.Random.Range(0, 350);
        for (int i = 0; i < 8; i++)
        {
            if (i != safeZone)
            {
                DicePalaceRabbitLevelOrb dicePalaceRabbitLevelOrb = self.orbPrefab.Create(center + dir * dist, 0f, Vector2.one) as DicePalaceRabbitLevelOrb;//new
                //DicePalaceRabbitLevelOrb dicePalaceRabbitLevelOrb = self.orbPrefab.Create(player.center + dir * dist, 0f, Vector2.one) as DicePalaceRabbitLevelOrb;
                dicePalaceRabbitLevelOrb.transform.parent = centerPoint.transform;
                dicePalaceRabbitLevelOrb.transform.Rotate(Vector3.forward, -initialRotation);
                dicePalaceRabbitLevelOrb.SetAsGold(i % 2 == 1);
                Color color = dicePalaceRabbitLevelOrb.GetComponent<SpriteRenderer>().color;
                color.a = 0.2f;
                dicePalaceRabbitLevelOrb.GetComponent<SpriteRenderer>().color = color;
                orbs[orbsIndex] = dicePalaceRabbitLevelOrb.transform;
                orbsIndex++;
            }
            dir = Quaternion.AngleAxis(-45f, Vector3.forward) * dir; 
        }
        centerPoint.transform.Rotate(Vector3.forward, initialRotation);
        float time = 0f;//new start
        float timeMax = 1f;
        while (time < timeMax)//new ned
        //while (self.attacking)
        {
            //if (player != null && !player.IsDead)
            time += CupheadTime.FixedDelta;
            if (player != null && !player.IsDead && !targetLocked)//new
            {
                center = player.center;//new
                //centerPoint.transform.position = player.center;
            }
            centerPoint.transform.position = center;//new
            centerPoint.transform.Rotate(Vector3.forward * CupheadTime.FixedDelta, -self.properties.CurrentState.magicWand.spinningSpeed * CupheadTime.FixedDelta);
            for (int j = 0; j < orbs.Length; j++)
            {
                SpriteRenderer component = orbs[j].GetComponent<SpriteRenderer>();
                Color color2 = component.color;
                color2.a += CupheadTime.FixedDelta / 2f;
                component.color = color2;
                if (color2.a >= 1f)
                {
                    orbs[j].GetComponent<Collider2D>().enabled = true;
                }
                orbs[j].Rotate(Vector3.forward * CupheadTime.FixedDelta, self.properties.CurrentState.magicWand.spinningSpeed * CupheadTime.FixedDelta);
            }
            yield return new WaitForFixedUpdate();
        }
        for (int k = 0; k < orbs.Length; k++)
        {
            orbs[k].GetComponent<Collider2D>().enabled = true;
        }
        /*while (Vector3.Angle(Vector3.up, centerPoint.transform.up) > 5f)
        {
            if (player != null && !player.IsDead)
            {
                centerPoint.transform.position = player.center;
            }
            centerPoint.transform.Rotate(Vector3.forward * CupheadTime.FixedDelta, -self.properties.CurrentState.magicWand.spinningSpeed * CupheadTime.FixedDelta);
            for (int l = 0; l < orbs.Length; l++)
            {
                orbs[l].Rotate(Vector3.forward * CupheadTime.FixedDelta, self.properties.CurrentState.magicWand.spinningSpeed * CupheadTime.FixedDelta);
            }
            yield return new WaitForFixedUpdate();
        }*/
        centerPoint.transform.up = Vector3.up;//new
        targetLocked = true;//new
        centerPoint.transform.up = Vector3.up;
        self.StartCoroutine(self.collapse_cr(centerPoint));
        yield break;
    }

    public IEnumerator collapse_cr(On.DicePalaceRabbitLevelRabbit.orig_collapse_cr orig, DicePalaceRabbitLevelRabbit self, System.Object centerPoint2)
    {
        float dist = self.properties.CurrentState.magicWand.circleDiameter / 2f;
        //float explodeDist = self.properties.CurrentState.magicWand.circleDiameter * 0.1f;
        float explodeDist = 5f;//new
        GameObject centerPoint = (GameObject)centerPoint2;
        while (dist >= explodeDist)
        {
            for (int i = 0; i < 7; i++)
            {
                Vector3 vector = (centerPoint.transform.GetChild(i).position - centerPoint.transform.position).normalized * dist;
                centerPoint.transform.GetChild(i).position = centerPoint.transform.position + vector;
            }

            dist -= self.properties.CurrentState.magicWand.bulletSpeed * (float)CupheadTime.Delta;
            yield return null;
        }

        AudioManager.Play("projectile_explo");
        self.explosionPrefab.Create(centerPoint.transform.position);
        self.currentCenterPoint = null;
        GameObject.Destroy(centerPoint);
    }

    private IEnumerator magicparry_cr(On.DicePalaceRabbitLevelRabbit.orig_magicparry_cr orig, DicePalaceRabbitLevelRabbit self)
    {
        self.attacking = true;
        self.state = DicePalaceRabbitLevelRabbit.State.MagicParry;
        yield return CupheadTime.WaitForSeconds(self, self.properties.CurrentState.magicParry.initialAttackDelay);
        self.animator.SetTrigger("OnAttack");
        /*string[] positionsSplits = self.properties.CurrentState.magicParry.magicPositions.Split(new char[]
        {
            '-'
        });
        DicePalaceRabbitLevelMagic[] magicOrbs = new DicePalaceRabbitLevelMagic[positionsSplits.Length];
        string[] parryPattern = self.properties.CurrentState.magicParry.pinkString.Split(new char[]
        {
            ','
        });
        string[] parryIndexes = parryPattern[self.parryCurrentIndex].Split(new char[]
        {
            '-'
        });
        float yOffset = self.properties.CurrentState.magicParry.yOffset;
        float posY = (!self.isMagicParryTop) ? (-360f + yOffset) : (360f - yOffset);*/
        int suit = 0;
        //for (int i = 0; i < magicOrbs.Length; i++)
        bool left = Rand.Bool();//new start
        float x = left ? -600f : 450f;
        DicePalaceRabbitLevelMagic[] magicOrbs = new DicePalaceRabbitLevelMagic[7];
        int parryableIndex = UnityEngine.Random.Range(1, 4);
        for (int i = 0; i < 7; i++)//new end
        {
            /*float num = 0f;
            Parser.FloatTryParse(positionsSplits[i], out num);
            num += -640f;
            magicOrbs[i] = (DicePalaceRabbitLevelMagic)self.magicPrefab.Create(new Vector3(num, posY));*/
            float y = i * 130f - 270f;//new start
            magicOrbs[i] = (DicePalaceRabbitLevelMagic)self.magicPrefab.Create(new Vector3(x, y));
            magicOrbs[i].transform.SetScale(1.5f, 1.5f, null);//new end
            magicOrbs[i].IsOffset(i % 2 == 1);
            magicOrbs[i].AppearTime = self.properties.CurrentState.magicParry.attackDelayRange;
            /*bool flag = false;
            for (int j = 0; j < parryIndexes.Length; j++)
            {
                int num2 = 0;
                if (Parser.IntTryParse(parryIndexes[j], out num2) && num2 - 1 == i)
                {
                    magicOrbs[i].SetParryable(true);
                    flag = true;
                }
            }
            if (!flag)
            {
                magicOrbs[i].SetSuit(suit);
                suit = (suit + 1) % 3;
            }*/
            if(i == parryableIndex)//new start
            {
                magicOrbs[i].SetParryable(true);
            }
            else
            {
                magicOrbs[i].SetSuit(suit);
                suit = (suit + 1) % 3;
            }//new end
        }
        yield return CupheadTime.WaitForSeconds(self, self.properties.CurrentState.magicParry.attackDelayRange);
        for (int k = 0; k < magicOrbs.Length; k++)
        {
            magicOrbs[k].ActivateOrb();
            magicOrbs[k].Move(x, left, self.properties.CurrentState.magicParry.speed);//new
            //magicOrbs[k].Move(posY, self.isMagicParryTop, self.properties.CurrentState.magicParry.speed);
        }
        self.animator.SetTrigger("OnAttackEnd");
        yield return CupheadTime.WaitForSeconds(self, self.properties.CurrentState.magicParry.hesitate);
        self.attacking = false;
        //self.isMagicParryTop = !self.isMagicParryTop;
        //self.parryCurrentIndex = (self.parryCurrentIndex + 1) % parryPattern.Length;
        self.state = DicePalaceRabbitLevelRabbit.State.Idle;
        yield break;
    }

    private bool targetLocked;
    Vector3 center;
}
