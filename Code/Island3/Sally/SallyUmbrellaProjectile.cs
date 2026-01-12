using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CupaGoovno;

public class SallyUmbrellaProjectile
{
    public void Init()
    {
        On.SallyStagePlayLevelUmbrellaProjectile.InitProjectile += InitProjectile;
        On.SallyStagePlayLevelUmbrellaProjectile.Update += Update;
        //On.SallyStagePlayLevelUmbrellaProjectile.move_cr += move_cr;
        On.SallyStagePlayLevelUmbrellaProjectile.Die += Die;
    }

    public void InitProjectile(On.SallyStagePlayLevelUmbrellaProjectile.orig_InitProjectile orig, SallyStagePlayLevelUmbrellaProjectile self, LevelProperties.SallyStagePlay properties, int direction)
    {
        orig(self, properties, direction);
        self.transform.SetScale(self.transform.localScale.x * 1.5f, self.transform.localScale.y * 1.5f, null);//new start
        redDic[self] = false;
        blueDic[self] = false;
        int type = UnityEngine.Random.Range(0, 3);
        if (type == 0)
        {
            redDic[self] = true;
            SpriteRenderer renderer = self.GetComponent<SpriteRenderer>();
            if(renderer != null)
            {
                renderer.color = Color.red;
            }
        }
        if (type == 1)
        {
            blueDic[self] = true;
            SpriteRenderer renderer = self.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                renderer.color = Color.blue;
            }
        }//new end
    }

    protected void Update(On.SallyStagePlayLevelUmbrellaProjectile.orig_Update orig, SallyStagePlayLevelUmbrellaProjectile self)
    {
        orig(self);
        if (!waitDic.ContainsKey(self))
        {
            waitDic[self] = true;
            self.StartCoroutine(delay_cr(self));
        }
        if (!waitDic[self])
        {
            if (!alreadyShotDic.ContainsKey(self))
            {
                alreadyShotDic[self] = false;
            }
            if (!previousYDic.ContainsKey(self))
            {
                previousYDic[self] = self.transform.position.y;
            }
            if (redDic[self] && previousYDic[self] > self.transform.position.y && !alreadyShotDic[self] && self.transform.position.y < 0f)
            {
                alreadyShotDic[self] = true;
                YoMamaFat.sallyBottle.Create(self.transform.position, 0f, 2000f, YoMamaFat.sallyParent).transform.SetScale(2f, 2f, null);
                YoMamaFat.sallyBottle.Create(self.transform.position, 180f, 2000f, YoMamaFat.sallyParent).transform.SetScale(2f, 2f, null);
            }
            if (blueDic[self] && previousYDic[self] > self.transform.position.y && !alreadyShotDic[self] && self.transform.position.y < -200f)
            {
                alreadyShotDic[self] = true;
                YoMamaFat.sallyBottle.Create(self.transform.position, 0f, 2000f, YoMamaFat.sallyParent).transform.SetScale(2f, 2f, null);
                YoMamaFat.sallyBottle.Create(self.transform.position, 180f, 2000f, YoMamaFat.sallyParent).transform.SetScale(2f, 2f, null);
            }
        }
    }

    protected void Die(On.SallyStagePlayLevelUmbrellaProjectile.orig_Die orig, SallyStagePlayLevelUmbrellaProjectile self)
    {
        if (!redDic[self] && !blueDic[self] && PlayerManager.GetFirst() != null)//new start
        {
            YoMamaFat.sallyBottle.Create(self.transform.position, 90f, 2000f, YoMamaFat.sallyParent).transform.SetScale(2f, 2f, null);
        }//new end
        orig(self);
    }

    private IEnumerator delay_cr(SallyStagePlayLevelUmbrellaProjectile self)
    {
        yield return CupheadTime.WaitForSeconds(self, 3f);
        waitDic[self] = false;
    }

    public static Dictionary<SallyStagePlayLevelUmbrellaProjectile, bool> redDic = new Dictionary<SallyStagePlayLevelUmbrellaProjectile, bool>();
    public static Dictionary<SallyStagePlayLevelUmbrellaProjectile, bool> blueDic = new Dictionary<SallyStagePlayLevelUmbrellaProjectile, bool>();
    public static Dictionary<SallyStagePlayLevelUmbrellaProjectile, bool> alreadyShotDic = new Dictionary<SallyStagePlayLevelUmbrellaProjectile, bool>();
    public static Dictionary<SallyStagePlayLevelUmbrellaProjectile, bool> waitDic = new Dictionary<SallyStagePlayLevelUmbrellaProjectile, bool>();
    public static Dictionary<SallyStagePlayLevelUmbrellaProjectile, float> previousYDic = new Dictionary<SallyStagePlayLevelUmbrellaProjectile, float>();
}
