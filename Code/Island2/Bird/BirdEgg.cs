using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class BirdEgg
{
    public void Init()
    {
        On.FlyingBirdLevelBirdEgg.Update += Update;
        On.FlyingBirdLevelBirdEgg.Explode += Explode;
    }

    protected void Update(On.FlyingBirdLevelBirdEgg.orig_Update orig, FlyingBirdLevelBirdEgg self)
    {
        if (!shrapnelDic.ContainsKey(self))//new
        {//new
            orig(self);
        }//new start
        else
        {
            self.transform.position += self.transform.right * self.speed * CupheadTime.Delta;
            if (self.state == FlyingBirdLevelBirdEgg.State.Idle && (self.transform.position.y > 360f || self.transform.position.y < -360f))
            {
                self.Explode();
                self.Die();
            }
        }//new end
    }

    private void Explode(On.FlyingBirdLevelBirdEgg.orig_Explode orig, FlyingBirdLevelBirdEgg self)
    {
        AudioManager.Play("level_flying_bird_egg_explode");
        self.emitAudioFromObject.Add("level_flying_bird_egg_explode");
        AudioManager.Play("level_flying_bird_egg_break");
        self.emitAudioFromObject.Add("level_flying_bird_egg_break");
        if (self.state != FlyingBirdLevelBirdEgg.State.Idle)
        {
            return;
        }
        self.state = FlyingBirdLevelBirdEgg.State.Exploded;
        self.effectPrefab.Create(self.transform.position);
        if (self.maxProjectiles == 0)
        {
            return;
        }
        Vector3 position = self.transform.position;
        position.x += 42f;
        if (shrapnelDic.ContainsKey(self))//new start
        {
            float angle1 = shrapnelDic[self] == 1 ? -60f : 60f;
            float angle2 = shrapnelDic[self] == 1 ? -120f : 120f;
            self.childPrefab.Create(position, angle1, Vector2.one, self.speed);
            self.childPrefab.Create(position, angle2, Vector2.one, self.speed);
        }
        else
        {//new end
            if (self.maxProjectiles == 2)
            {
                self.childPrefab.Create(position, 90f, Vector2.one, -self.speed);
                self.childPrefab.Create(position, -90f, Vector2.one, -self.speed);
            }
            else
            {
                /*
                for (int i = 0; i < self.maxProjectiles; i++)
                {
                    float rotation;
                    switch (i)
                    {
                    default:
                        rotation = 0f;
                        break;
                    case 1:
                        rotation = -45f;
                        break;
                    case 2:
                        rotation = 45f;
                        break;
                    case 3:
                        rotation = 90f;
                        break;
                    case 4:
                        rotation = -90f;
                        break;
                    }
                    self.childPrefab.Create(position, rotation, Vector2.one, -self.speed);
                }*/
                self.childPrefab.Create(position, 90f, Vector2.one, -self.speed);
                FlyingBirdLevelBirdEgg egg1 = eggPrefabDic[self].Create(self.speed, position) as FlyingBirdLevelBirdEgg;
                egg1.transform.SetEulerAngles(0f, 0f, 45f);
                egg1.transform.SetScale(0.5f, 0.5f, null);
                shrapnelDic[egg1] = 1;
                SpriteRenderer renderer1 = egg1.GetComponent<SpriteRenderer>();
                if(renderer1 != null)
                {
                    renderer1.color = Color.red;
                }
                self.childPrefab.Create(position, 0f, Vector2.one, -self.speed);
                FlyingBirdLevelBirdEgg egg2 = eggPrefabDic[self].Create(self.speed, position) as FlyingBirdLevelBirdEgg;
                egg2.transform.SetEulerAngles(0f, 0f, -45f);
                egg2.transform.SetScale(0.5f, 0.5f, null);
                SpriteRenderer renderer2 = egg2.GetComponent<SpriteRenderer>();
                if (renderer2 != null)
                {
                    renderer2.color = Color.red;
                }
                shrapnelDic[egg2] = 2;
                self.childPrefab.Create(position, -90f, Vector2.one, -self.speed);
            }
        }//new
    }

    public static Dictionary<FlyingBirdLevelBirdEgg, int> shrapnelDic = new Dictionary<FlyingBirdLevelBirdEgg, int>();
    public static Dictionary<FlyingBirdLevelBirdEgg, FlyingBirdLevelBirdEgg> eggPrefabDic = new Dictionary<FlyingBirdLevelBirdEgg, FlyingBirdLevelBirdEgg>();
}
