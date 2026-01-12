using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class DevilDemon
{
    public static DevilLevelDemon Create(DevilLevelDemon self, Vector2 position, float direction, float speed, float hp, DevilLevelSittingDevil parent)
    {
        DevilLevelDemon demon = self.InstantiatePrefab<DevilLevelDemon>();
        demon.frontDirection = direction;
        demon.speed = speed;
        demon.hp = hp;
        demon.parent = parent;
        demon.transform.localScale = new Vector3(-direction, 1f, 1f);
        demon.animator.Play("RunOut");
        demon.transform.position = position;
        demon.StartCoroutine(spiderSpawnMovement_cr(demon));
        return demon;
    }

    private static IEnumerator spiderSpawnMovement_cr(DevilLevelDemon self)
    {
        self.moving = true;
        /*if (!self.hasJumped)
        {
            self.transform.position = self.RunRoot;
        }
        Vector3 backDirection = (self.PillarDestination - self.transform.position).normalized;
        while (CupheadLevelCamera.Current.ContainsPoint(self.transform.position, new Vector2(150f, 150f)))
        {
            self.transform.position += backDirection * self.speed * CupheadTime.Delta;
            float scaleDelta = 0.0999999642f * CupheadTime.Delta;
            self.transform.localScale -= new Vector3(self.frontDirection * scaleDelta, scaleDelta, scaleDelta);
            yield return null;
        }
        yield return CupheadTime.WaitForSeconds(self, self.frontWaitTime);
        self.transform.localScale = new Vector3(-self.frontDirection, 1f, 1f);
        self.transform.position = self.FrontSpawn;*/
        self.collider2d.enabled = true;
        self.sprite.sortingLayerName = "Enemies";
        self.sprite.sortingOrder = 0;
        self.sprite.color = Color.black;
        int stage = 0;//new
        for (; ; )
        {
            //self.transform.AddPosition(self.frontDirection * self.speed * CupheadTime.Delta, 0f, 0f);

            float distance = self.frontDirection * self.speed * CupheadTime.Delta;//new start
            float rotation = 90f * self.frontDirection;
            float x = self.transform.position.x;
            float y = self.transform.position.y;
            bool left = self.frontDirection < 0f;
            switch (stage)
            {
                case 0:
                    self.transform.AddPosition(distance, 0f, 0f);
                    if((x > 530f && !left) || (x < -530f && left))
                    {
                        self.transform.AddEulerAngles(0f, 0f, rotation);
                        self.transform.SetPosition(null, -300f);
                        stage = 1;
                    }
                    break;
                case 1:
                    self.transform.AddPosition(0f, Mathf.Abs(distance), 0f);
                    if(y > 200f)
                    {
                        self.transform.AddEulerAngles(0f, 0f, rotation);
                        self.transform.SetPosition(left ? -600f : 600f);
                        stage = 2;
                    }
                    break;
                case 2:
                    self.transform.AddPosition(-distance, 0f, 0f);
                    if ((x > 530f && left) || (x < -530f && !left))
                    {
                        self.transform.AddEulerAngles(0f, 0f, rotation);
                        self.transform.SetPosition(null, 300f);
                        stage = 3;
                    }
                    break;
                case 3:
                    self.transform.AddPosition(0f, -Mathf.Abs(distance), 0f);
                    if (y < -200f)
                    {
                        self.transform.AddEulerAngles(0f, 0f, rotation);
                        self.transform.SetPosition(left ? 600f : -600f);
                        stage = 0;
                    }
                    break;
            }//new end

            if (CupheadLevelCamera.Current.ContainsPoint(self.transform.position, new Vector2(150f, 150f)))
            {
                self.enteredScreen = true;
            }
            else if (self.enteredScreen)
            {
                UnityEngine.Object.Destroy(self.gameObject);
            }
            yield return null;
        }
    }
}
