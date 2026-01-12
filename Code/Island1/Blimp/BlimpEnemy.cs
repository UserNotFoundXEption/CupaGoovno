using UnityEngine;

namespace CupaGoovno;

public class BlimpEnemy
{
    public void Init()
    {
        On.FlyingBlimpLevelEnemy.FireSpreadshot += FireSpreadshot;
        On.FlyingBlimpLevelEnemy.FireSingle += FireSingle;
    }

    private void FireSpreadshot(On.FlyingBlimpLevelEnemy.orig_FireSpreadshot orig, FlyingBlimpLevelEnemy self)
    {
        AbstractPlayerController next = PlayerManager.GetRandom();//getnext
        float x = next.transform.position.x - self.transform.position.x;
        float y = next.transform.position.y - self.transform.position.y;
        Effect effect = UnityEngine.Object.Instantiate<Effect>(self.bulletEffect);
        effect.transform.position = self.projectileRoot.transform.position;
        effect.GetComponent<Animator>().SetInteger("PickAni", UnityEngine.Random.Range(0, 3));
        float randomAngleDelta = UnityEngine.Random.Range(-30f, 30f);//new
        for (int i = 0; i < self.enemyProperties.numBullets; i++)
        {
            float num = self.enemyProperties.spreadAngle.GetFloatAt((float)i / ((float)self.enemyProperties.numBullets - 1f));
            //float num2 = self.enemyProperties.spreadAngle.max / 2f;
            //num -= num2;
            float num3 = Mathf.Atan2(y, x) * 57.29578f;
            self.animationPicker = UnityEngine.Random.Range(0, 3);
            if (next.transform.position.x > self.transform.position.x)
            {
                num3 = (float)((next.transform.position.y <= self.transform.position.y) ? -90 : 90);
            }
            int num4 = self.animationPicker + 1;//new start
            self.projectilePrefab.Create(self.projectileRoot.position, num3 + num + randomAngleDelta, self.enemyProperties.BSpeed).GetComponent<Animator>().Play("Bullet_" + num4);
            self.projectilePrefab.Create(self.projectileRoot.position, num3 - num + randomAngleDelta, self.enemyProperties.BSpeed).GetComponent<Animator>().Play("Bullet_" + num4);//new end
            /*
            int num4 = self.animationPicker;
            if (num4 != 0)
            {
                if (num4 != 1)
                {
                    self.projectilePrefab.Create(self.projectileRoot.position, num3 + num, self.enemyProperties.BSpeed).GetComponent<Animator>().Play("Bullet_3");
                }
                else
                {
                    self.projectilePrefab.Create(self.projectileRoot.position, num3 + num, self.enemyProperties.BSpeed).GetComponent<Animator>().Play("Bullet_2");
                }
            }
            else
            {
                self.projectilePrefab.Create(self.projectileRoot.position, num3 + num, self.enemyProperties.BSpeed).GetComponent<Animator>().Play("Bullet_1");
            }*/
        }
    }

    private void FireSingle(On.FlyingBlimpLevelEnemy.orig_FireSingle orig, FlyingBlimpLevelEnemy self)
    {
        AbstractPlayerController next = PlayerManager.GetRandom();//next
        float x = next.transform.position.x - self.transform.position.x;
        float y = next.transform.position.y - self.transform.position.y;
        //float num = -3f;
        float num2 = Mathf.Atan2(y, x) * 57.29578f;
        Effect effect = UnityEngine.Object.Instantiate<Effect>(self.bulletEffect);
        effect.transform.position = self.projectileRoot.transform.position;
        effect.GetComponent<Animator>().SetInteger("PickAni", UnityEngine.Random.Range(0, 3));
        if (next.transform.position.x > self.transform.position.x)
        {
            num2 = (float)((next.transform.position.y <= self.transform.position.y) ? -90 : 90);
        }
        if (!self.parryable)
        {
            self.animationPicker = UnityEngine.Random.Range(0, 3);
        }
        else
        {
            self.animationPicker = UnityEngine.Random.Range(0, 2);
        }
        int num3 = self.animationPicker + 1;//new start
        num2 += UnityEngine.Random.Range(-30f, 30f);
        DragonLevelPotion dragonLevelPotion = UnityEngine.Object.Instantiate<DragonLevelPotion>(YoMamaFat.dragonPotion);
        dragonLevelPotion.Init(self.transform.position, 1f, num2, YoMamaFat.dragonPotionProperties);
        if(Blimp.blimpLady.properties.CurrentState.stateName == LevelProperties.FlyingBlimp.States.Gemini)
        {
            dragonLevelPotion.transform.SetScale(1f, 1f, 1f);
        }//new end
        /*
        if (!self.parryable)
        {
            int num3 = self.animationPicker;
            if (num3 != 0)
            {
                if (num3 != 1)
                {
                    self.projectilePrefab.Create(self.projectileRoot.position, num2 + num, self.enemyProperties.ASpeed).GetComponent<Animator>().Play("Bullet_3");
                }
                else
                {
                    self.projectilePrefab.Create(self.projectileRoot.position, num2 + num, self.enemyProperties.ASpeed).GetComponent<Animator>().Play("Bullet_2");
                }
            }
            else
            {
                self.projectilePrefab.Create(self.projectileRoot.position, num2 + num, self.enemyProperties.ASpeed).GetComponent<Animator>().Play("Bullet_1");
            }
        }
        else
        {
            int num4 = self.animationPicker;
            if (num4 != 0)
            {
                self.parryablePrefab.Create(self.projectileRoot.position, num2 + num, self.enemyProperties.ASpeed).GetComponent<Animator>().Play("Bullet_2");
            }
            else
            {
                self.parryablePrefab.Create(self.projectileRoot.position, num2 + num, self.enemyProperties.ASpeed).GetComponent<Animator>().Play("Bullet_1");
            }
        }*/
    }
}
