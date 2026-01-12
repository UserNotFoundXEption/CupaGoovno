using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class TrainLollipopGhoulsManager
{
    public void Init()
    {
        On.TrainLevelLollipopGhoulsManager.ghouls_cr += ghouls_cr;
    }

    private IEnumerator ghouls_cr(On.TrainLevelLollipopGhoulsManager.orig_ghouls_cr orig, TrainLevelLollipopGhoulsManager self)
    {
        firstSoapHell = true;//new start
        ghoulLeft = self.ghoulLeft;
        ghoulRight = self.ghoulRight;//new end
        self.current = UnityEngine.Random.Range(0, 2);
        for (; ; )
        {
            TrainLevelLollipopGhoul ghoul = self.NextGhoul();
            yield return null;
            bool platformEndLeft = Train.platform.transform.position.x < -100f && ghoul == self.ghoulRight;
            bool platformEndRight = Train.platform.transform.position.x > 100f && ghoul == self.ghoulLeft;
            if (ghoul != null && !platformEndLeft && !platformEndRight)
            {
                if (ghoul == self.ghoulLeft)//new start
                {
                    self.StartCoroutine(soapHell_cr(parent, true));
                }
                else
                {
                    self.StartCoroutine(soapHell_cr(parent, false));
                }//new end
                ghoul.Attack();
                while (ghoul.state == TrainLevelLollipopGhoul.State.Attacking)
                {
                    yield return null;
                }
                yield return CupheadTime.WaitForSeconds(self, self.properties.CurrentState.lollipopGhouls.mainDelay);
            }
        }
    }

    private IEnumerator soapHell_cr(TrainLevel self, bool left)//new
    {
        if (firstSoapHell)
        {
            yield return CupheadTime.WaitForSeconds(self, 1.5f);
            firstSoapHell = false;
        }
        TrainLevelLollipopGhoul ghoulLeft = self.ghouls.ghoulLeft;
        TrainLevelLollipopGhoul ghoulRight = self.ghouls.ghoulRight;
        float[][] yPatterns = new float[][]
        {
            new float[]{ -150f, 50f, 100f, 0f, -50f },
            new float[]{ 50f, 100f, -150f, -50f, -150f },
            new float[]{ -150f, 0f, 100f, -150f, 50f }
        };
        int patternId = 2137;
        float[] pattern = yPatterns[0];
        for (int i = 0; i< 20; i++ )
        {
            bool leftDead = ghoulLeft.state == TrainLevelLollipopGhoul.State.Dead || ghoulLeft.transform == null;
            bool rightDead = ghoulRight.state == TrainLevelLollipopGhoul.State.Dead || ghoulRight.transform == null;
            bool platformEndLeft = Train.platform.transform.position.x < -100f && !left;
            bool platformEndRight = Train.platform.transform.position.x > 100f && left;
            if ((left && leftDead) || (!left && rightDead) || platformEndLeft || platformEndRight)
            {
                yield break;
            }
            if(patternId > 4)
            {
                pattern = yPatterns.RandomChoice<float[]>();
                patternId = 0;
            }
            yield return CupheadTime.WaitForSeconds(self, 0.35f);
            TrainLevelPumpkinProjectile soap = self.pumpkinPrefab.brickPrefab.Create() as TrainLevelPumpkinProjectile;
            soap.GetComponent<SpriteRenderer>().color = Color.yellow;
            TrainPumpkinProjectile.phase3YDic[soap] = pattern[patternId];
            TrainPumpkinProjectile.phase3LeftDic[soap] = left;
            patternId++;
        }
        yield break;
    }

    public static TrainLevel parent;
    public static TrainLevelLollipopGhoul ghoulLeft;
    public static TrainLevelLollipopGhoul ghoulRight;
    private bool firstSoapHell;
}
