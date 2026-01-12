using System.Collections;
using UnityEngine;

namespace CupaGoovno;

public class KingDice
{
    public void Init()
    {
        new KingDiceGameManager().Init();
        new KingDiceDice().Init(); 
        new KingDiceGameInfo().Init();
        new KingDiceCard().Init();
        On.DicePalaceMainLevelKingDice.RevealDice += RevealDice;
        On.DicePalaceMainLevelKingDice.StartKingDiceBattle += StartKingDiceBattle;
    }

    private void RevealDice(On.DicePalaceMainLevelKingDice.orig_RevealDice orig, DicePalaceMainLevelKingDice self)
    {
        orig(self);
        king = self;
        if (cardsCoroutine != null)
        {
            king.StopCoroutine(cardsCoroutine);
        }
        cardsCoroutine = king.StartCoroutine(cards_cr(king));
    }

    private static IEnumerator cards_cr(DicePalaceMainLevelKingDice self)
    {
        LevelProperties.DicePalaceMain.Cards p = self.properties.CurrentState.cards;
        int cardIndex = UnityEngine.Random.Range(0, p.cardString.Length);
        string[] sideString = p.cardSideOrder.GetRandom<string>().Split(new char[]
        {
            ','
        });
        int suitIndex = UnityEngine.Random.Range(0, 3);
        int sideIndex = UnityEngine.Random.Range(0, sideString.Length);
        bool onLeft = false;
        Vector3 rootPos = Vector3.zero;
        //self.damageReceiver.enabled = true;
        //self.GetComponent<Collider2D>().enabled = true;
        for (; ; )
        {
            yield return CupheadTime.WaitForSeconds(self, 1f);//new start
            string[] cardString = p.cardString[cardIndex].Split(new char[]
            {
                ','
            });
            /*if (sideString[sideIndex][0] == 'L')
            {
                onLeft = true;
                rootPos = self.leftRoot.transform.position;
            }
            else if (sideString[sideIndex][0] == 'R')
            {
                onLeft = false;
                rootPos = self.rightRoot.transform.position;
            }
            else
            {
                global::Debug.LogError("Invalid pattern string", null);
            }
            self.animator.SetBool("OnLeftAttack", onLeft);
            yield return self.animator.WaitForAnimationToEnd(self, (!onLeft) ? "Attack_Right" : "Attack_Left", false, true);
            AudioManager.PlayLoop("king_dice_march_loop");
            self.emitAudioFromObject.Add("king_dice_march_loop");
            self.StartCoroutine(self.kd_laugh_cr());*/
            if (left)//new start
            {
                onLeft = true;
                rootPos = self.leftRoot.transform.position + new Vector3(-300f, 0f);
            }
            else
            {
                onLeft = false;
                rootPos = self.rightRoot.transform.position + new Vector3(300f, 0f);
            }
            left = !left;//new end
            for (int i = 0; i < cardString.Length; i++)
            {
                if (cardString[i][0] == 'R')
                {
                    DicePalaceMainLevelCard dicePalaceMainLevelCard = self.cardRegular.Create(rootPos, p, onLeft);
                    dicePalaceMainLevelCard.transform.SetScale(new float?((float)((!onLeft) ? -1 : 1)), null, null);
                    dicePalaceMainLevelCard.GetComponent<SpriteRenderer>().sortingOrder = i;
                    suitIndex = (suitIndex + 1) % 3;
                }
                else if (cardString[i][0] == 'P')
                {
                    DicePalaceMainLevelCard dicePalaceMainLevelCard2 = self.cardPink.Create(rootPos, p, onLeft);
                    dicePalaceMainLevelCard2.transform.SetScale(new float?((float)((!onLeft) ? -1 : 1)), null, null);
                    dicePalaceMainLevelCard2.GetComponent<SpriteRenderer>().sortingOrder = i;
                }
                /*else
                {
                    global::Debug.LogError("Invalid pattern string", null);
                }*/
                yield return CupheadTime.WaitForSeconds(self, p.cardDelay);
            }
            //AudioManager.Stop("king_dice_march_loop");
            //self.animator.SetBool("IsAttacking", false);
            //yield return CupheadTime.WaitForSeconds(self, p.hesitate);
            //self.animator.SetBool("IsAttacking", true);
            //sideIndex = (sideIndex + 1) % sideString.Length;
            cardIndex = (cardIndex + 1) % p.cardString.Length;
            yield return null;
        }
    }

    public void StartKingDiceBattle(On.DicePalaceMainLevelKingDice.orig_StartKingDiceBattle orig, DicePalaceMainLevelKingDice self)
    {
        orig(self);
        self.StartCoroutine(cigarSpit_cr(self));//new
    }

    private IEnumerator cigarSpit_cr(DicePalaceMainLevelKingDice self)//new
    {
        for(; ; )
        {
            Vector2 pos = new Vector2(700f, -100f);
            AbstractProjectile abstractProjectile = YoMamaFat.cigarSpit.Create(pos, 0f);
            abstractProjectile.GetComponent<DicePalaceCigarLevelCigarSpit>().InitProjectile(YoMamaFat.cigarProperties, false, true);
            yield return CupheadTime.WaitForSeconds(self, 3f);
            yield return null;
        }
    }

    public static Coroutine cardsCoroutine;
    public static DicePalaceMainLevelKingDice king;
    private static bool left = false;
}
