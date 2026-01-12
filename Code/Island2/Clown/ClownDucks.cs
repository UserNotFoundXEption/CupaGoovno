using System.Collections;
using UnityEngine;

namespace CupaGoovno;

public class ClownDucks
{
    public void Init()
    {
        On.ClownLevelDucks.OnDamageTaken += OnDamageTaken;
        On.ClownLevelDucks.drop_bomb_cr += drop_bomb_cr;
    }

    private void OnDamageTaken(On.ClownLevelDucks.orig_OnDamageTaken orig, ClownLevelDucks self, DamageDealer.DamageInfo info)
    {
        //	base.StartCoroutine(this.spin_cr());
        if (self.isBombDuck && !self.bombDropped && self.transform.position.x < 600f && self.transform.position.x > -600f)
        {
            self.StartCoroutine(self.drop_bomb_cr());
        }
    }

    private IEnumerator drop_bomb_cr(On.ClownLevelDucks.orig_drop_bomb_cr orig, ClownLevelDucks self)
    {
        yield return orig(self);
        Vector2 pos = self.bomb.transform.position;
        for (int i = 0; i < 3; i++)
        {
            LevelProperties.Clown.Horse horse = YoMamaFat.clownProperties.CurrentState.horse;
            ClownLevelHorseshoe clownLevelHorseshoe = UnityEngine.Object.Instantiate<ClownLevelHorseshoe>(YoMamaFat.clownHorseshoe);
            clownLevelHorseshoe.Init(pos, 2000f, -2000f, true, 0f, horse, ClownLevelClownHorse.HorseType.Drop);
            ClownLevelHorseshoe clownLevelHorseshoe2 = UnityEngine.Object.Instantiate<ClownLevelHorseshoe>(YoMamaFat.clownHorseshoe);
            clownLevelHorseshoe2.Init(pos, 2000f, -2000f, false, 0f, horse, ClownLevelClownHorse.HorseType.Drop);
            yield return CupheadTime.WaitForSeconds(self, 0.12f);
        }
    }
}
