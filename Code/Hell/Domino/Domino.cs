using System;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class Domino
{
    public void Init()
    {
        new DominoBoomerang().Init();
        On.DicePalaceDominoLevel.Start += Start;
        On.DicePalaceDominoLevelDomino.boomerang_cr += boomerang_cr;
    }

    protected void Start(On.DicePalaceDominoLevel.orig_Start orig, DicePalaceDominoLevel self)
    {
        orig(self);
        self.StartCoroutine(cigarSpit_cr(self));
    }

    private IEnumerator boomerang_cr(On.DicePalaceDominoLevelDomino.orig_boomerang_cr orig, DicePalaceDominoLevelDomino self)
    {
        self.state = DicePalaceDominoLevelDomino.State.Boomerang;
        yield return CupheadTime.WaitForSeconds(self, self.properties.CurrentState.boomerang.initialAttackDelay);
        //self.animator.SetTrigger("OnBird");
        //yield return self.animator.WaitForAnimationToEnd(self, "Bird_Attack", false, true);
        //yield return CupheadTime.WaitForSeconds(self, self.sadAttackDelay);
		LevelProperties.DicePalaceDomino.Boomerang p = self.properties.CurrentState.boomerang;//new start
        float x = UnityEngine.Random.Range(-300f, 100f);
        float y = 400f;
        Vector2 pos = new Vector2(x, y);
        DicePalaceDominoLevelBoomerang proj = self.boomerangPrefab.Create(pos, p.boomerangSpeed, p.health);//new end
        self.state = DicePalaceDominoLevelDomino.State.Idle;
        yield break;
    }

    private IEnumerator cigarSpit_cr(DicePalaceDominoLevel self)
    {
        Vector2 spawnPos = new Vector2(700f, -100f);
        for(; ; )
        {
            AbstractProjectile abstractProjectile = YoMamaFat.cigarSpit.Create(spawnPos, 0f);
            abstractProjectile.GetComponent<DicePalaceCigarLevelCigarSpit>().InitProjectile(YoMamaFat.cigarProperties, true, true);
            yield return CupheadTime.WaitForSeconds(self, 4f);
        }
    }
}
