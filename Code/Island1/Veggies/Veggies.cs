using UnityEngine;
using System.Collections;

namespace CupaGoovno;

public class Veggies
{
    public void Init()
    {
        new Potato().Init();
        new Carrot().Init();
        new Onion().Init();
        new OnionTearProjectile().Init();
        On.VeggiesLevel.OnDestroy += OnDestroy;
        On.VeggiesLevel.veggiesPattern_cr += veggiesPattern_cr;
        On.VeggiesLevel.carrot_cr += carrot_cr;
    }

    protected void OnDestroy(On.VeggiesLevel.orig_OnDestroy orig, VeggiesLevel self)
    {
        orig(self);
        SetPlatformX(-2137f);//new
    }

    private IEnumerator veggiesPattern_cr(On.VeggiesLevel.orig_veggiesPattern_cr orig, VeggiesLevel self)
    {
        yield return CupheadTime.WaitForSeconds(self, 1f);
        yield return self.StartCoroutine(self.potato_cr());
        if (self.mode != Level.Mode.Easy)
        {
            SetPlatformX(-450f);//new
            yield return self.StartCoroutine(self.onion_cr());
        }
        SetPlatformX(-2137f);//new
        yield return self.StartCoroutine(self.carrot_cr());
        yield return self.StartCoroutine(self.win_cr());
        yield break;
    }

    private IEnumerator carrot_cr(On.VeggiesLevel.orig_carrot_cr orig, VeggiesLevel self)
    {
        self.currentBoss = VeggiesLevel.CurrentBoss.Carrot;
        VeggiesLevelCarrot carrotMain = self.prefabs.carrot.InstantiatePrefab<VeggiesLevelCarrot>();
        Carrot.SetMainCarrot(null, carrotMain);//new
        carrotMain.LevelInit(self.properties);
        carrotMain.OnDamageTakenEvent += self.timeline.DealDamage;

        VeggiesLevelCarrot carrotRight = self.prefabs.carrot.InstantiatePrefab<VeggiesLevelCarrot>();//new start
        Carrot.SetMainCarrot(carrotMain, carrotRight);
        carrotRight.transform.position = new Vector3(carrotMain.transform.position.x + 400, carrotMain.transform.position.y);
        carrotRight.LevelInit(self.properties);

        VeggiesLevelCarrot carrotLeft = self.prefabs.carrot.InstantiatePrefab<VeggiesLevelCarrot>();
        Carrot.SetMainCarrot(carrotMain, carrotLeft);//new end
        carrotLeft.transform.position = new Vector3(carrotMain.transform.position.x - 400, carrotMain.transform.position.y);
        carrotLeft.LevelInit(self.properties);

        while (carrotMain.state != VeggiesLevelCarrot.State.Complete)
        {
            yield return null;
        }
        yield break;
    }

    public static void SetPlatformX(float x)//new
    {
        if (YoMamaFat.flowerPlatform != null)
        {
            YoMamaFat.flowerPlatform.transform.position = new Vector3(x, -100f, 0f);
            YoMamaFat.flowerPlatform.transform.SetScale(2f, 1f, 1f);
        }
    }
}
