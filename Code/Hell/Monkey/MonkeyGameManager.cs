using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CupaGoovno;

public class MonkeyGameManager
{
    public void Init()
    {
        On.DicePalaceFlyingMemoryLevelGameManager.open_timer_cr += open_timer_cr;
    }

    private IEnumerator open_timer_cr(On.DicePalaceFlyingMemoryLevelGameManager.orig_open_timer_cr orig, DicePalaceFlyingMemoryLevelGameManager self)
    {
        //float HPToLower = self.maxHP / (float)(self.GridDimX * self.GridDimY / 2);
        if (self.matchMade)
        {
            //self.StartCoroutine(self.slide_cr(self.hiddenPosition));
            self.matchCounter++;
            while (self.stuffedToy.currentlyColliding)
            {
                yield return null;
            }
            //self.stuffedToy.Open();
            if (self.matchCounter == self.GridDimX * self.GridDimY / 2)
            {
                self.properties.DealDamage(self.properties.CurrentHealth);//new
                yield break;
            }
            /*while (self.properties.CurrentHealth >= self.maxHP - HPToLower * (float)self.matchCounter)
            {
                yield return null;
            }*/
        }
        else
        {
            //self.stuffedToy.guessedWrong = true;
            self.StartCoroutine(self.stuffedToy.punishment_cr());//new
            yield return CupheadTime.WaitForSeconds(self, 1f);
        }
        for (int i = 0; i < self.GridDimY; i++)
        {
            for (int j = 0; j < self.GridDimX; j++)
            {
                if (!self.cards[j, i].permanentlyFlipped)
                {
                    self.cards[j, i].EnableCards();
                }
            }
        }
        //self.StartCoroutine(self.slide_cr(self.cardStopRoot.position));
        //self.stuffedToy.Closed();
        self.checkForFlipped = true;
        yield return null;
        yield break;
    }
}
