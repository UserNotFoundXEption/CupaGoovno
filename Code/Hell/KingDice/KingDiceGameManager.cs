using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class KingDiceGameManager
{
    public void Init()
    {
        On.DicePalaceMainLevelGameManager.check_for_rolled_cr += check_for_rolled_cr;
        On.DicePalaceMainLevelGameManager.LevelInit += LevelInit;
    }

    public void LevelInit(On.DicePalaceMainLevelGameManager.orig_LevelInit orig, DicePalaceMainLevelGameManager self, LevelProperties.DicePalaceMain properties)
    {
        orig(self, properties);
        if (DicePalaceMainLevelGameInfo.PLAYER_SPACES_MOVED == 0)
        {
            moveMarkerBack = false;
        }
    }

    private IEnumerator check_for_rolled_cr(On.DicePalaceMainLevelGameManager.orig_check_for_rolled_cr orig, DicePalaceMainLevelGameManager self)
    {
        yield return CupheadTime.WaitForSeconds(self, self.properties.CurrentState.dice.revealDelay);
        DicePalaceMainLevelGameInfo.IS_FIRST_ENTRY = false;
        self.kingDiceAni.SetTrigger("OnReveal");
        yield return self.kingDiceAni.WaitForAnimationToEnd(self.kingDice, "Dice_Reveal", false, true);
        LevelProperties.DicePalaceMain.Dice p = self.properties.CurrentState.dice;
        int spacesToMove = 0;
        bool playingGame = true;
        while (playingGame)
        {
            while (self.dice.waitingToRoll)
            {
                yield return null;
            }
            if (KingDice.cardsCoroutine != null)//new start
            {
                KingDice.king.StopCoroutine(KingDice.cardsCoroutine);
            }//new end
            spacesToMove = 0;
            DicePalaceMainLevelDice.Roll roll = self.dice.roll;
            if (roll != DicePalaceMainLevelDice.Roll.One)
            {
                if (roll != DicePalaceMainLevelDice.Roll.Two)
                {
                    if (roll == DicePalaceMainLevelDice.Roll.Three)
                    {
                        spacesToMove = 3;
                    }
                }
                else
                {
                    spacesToMove = 2;
                }
            }
            else
            {
                spacesToMove = 1;
            }
            if (moveMarkerBack)//new start
            {
                spacesToMove *= -1;
            }
            moveMarkerBack = !moveMarkerBack;//new end
            int spaces = (DicePalaceMainLevelGameInfo.PLAYER_SPACES_MOVED + spacesToMove <= 1) ? 0 : (DicePalaceMainLevelGameInfo.PLAYER_SPACES_MOVED + spacesToMove - 1);
            if (spaces < self.maxSpaces && self.allBoardSpaces[spaces] != DicePalaceMainLevelGameManager.BoardSpaces.FreeSpace && self.allBoardSpaces[spaces] != DicePalaceMainLevelGameManager.BoardSpaces.StartOver && spaces + 1 < self.boardSpacesObj.Length && !self.boardSpacesObj[spaces + 1].HasHeart)
            {
                AudioManager.Stop("vox_curious");
                AudioManager.Play("vox_laugh");
                self.emitAudioFromObject.Add("vox_laugh");
            }
            yield return self.StartCoroutine(self.MoveMarker(spacesToMove, false));
            DicePalaceMainLevelGameInfo.PLAYER_SPACES_MOVED += spacesToMove;
            if(DicePalaceMainLevelGameInfo.PLAYER_SPACES_MOVED < 0)//new start
            {
                DicePalaceMainLevelGameInfo.PLAYER_SPACES_MOVED = 0;
            }//new end
            if (DicePalaceMainLevelGameInfo.PLAYER_SPACES_MOVED > self.maxSpaces)
            {
                DicePalaceMainLevelGameInfo.PLAYER_SPACES_MOVED = self.maxSpaces;
                playingGame = false;
                self.kingDiceAni.SetBool("IsSafe", true);
                break;
            }
            DicePalaceMainLevelGameManager.BoardSpaces space = self.allBoardSpaces[spaces];
            self.kingDiceAni.SetTrigger("OnEager");
            if (playingGame)
            {
                if (space == DicePalaceMainLevelGameManager.BoardSpaces.FreeSpace || DicePalaceMainLevelGameInfo.PLAYER_SPACES_MOVED == self.previousSpace)
                {
                    AudioManager.Play("vox_curious");
                    self.emitAudioFromObject.Add("vox_curious");
                    self.kingDiceAni.SetBool("IsSafe", true);
                }
                else if (space == DicePalaceMainLevelGameManager.BoardSpaces.StartOver)
                {
                    AudioManager.Play("vox_startover");
                    self.emitAudioFromObject.Add("vox_startover");
                    AudioManager.Stop("vox_curious");
                    DicePalaceMainLevelGameInfo.SAFE_INDEXES.Add(spaces);
                    self.boardSpacesObj[spaces + 1].Clear = true;
                    self.CheckSafeSpaces();
                    yield return self.StartCoroutine(self.MoveMarker(-self.maxSpaces, true));
                    self.kingDiceAni.SetBool("IsSafe", true);
                    DicePalaceMainLevelGameInfo.PLAYER_SPACES_MOVED = 0;
                    yield return CupheadTime.WaitForSeconds(self, p.pauseWhenRolled);
                }
                else
                {
                    KingDice.king.StopCoroutine(KingDice.cardsCoroutine);//new start
                    GameObject[] cards = GameObject.FindObjectsOfType<GameObject>();
                    foreach(GameObject gameObject in cards)
                    {
                        if (gameObject.name == "DicePalaceMain_Regular_Card(Clone)" || gameObject.name == "DicePalaceMain_Pink_Card(Clone)")
                        {
                            GameObject.Destroy(gameObject);
                        }
                    }//new end
                    self.previousSpace = DicePalaceMainLevelGameInfo.PLAYER_SPACES_MOVED;
                    self.kingDiceAni.SetBool("IsSafe", false);
                    DicePalaceMainLevelGameInfo.SAFE_INDEXES.Add(spaces);
                    for (int i = 0; i < DicePalaceMainLevelGameInfo.HEART_INDEXES.Length; i++)
                    {
                        if (DicePalaceMainLevelGameInfo.HEART_INDEXES[i] == spaces)
                        {
                            if (DicePalaceMainLevelGameInfo.PLAYER_ONE_STATS == null)
                            {
                                DicePalaceMainLevelGameInfo.PLAYER_ONE_STATS = new PlayersStatsBossesHub();
                            }
                            PlayerStatsManager playerStats = PlayerManager.GetPlayer(PlayerId.PlayerOne).stats;
                            if (playerStats.Health > 0 && playerStats.Loadout.charm != Charm.charm_health_up_1)//new
                            //if (playerStats.Health > 0)
                            {
                                DicePalaceMainLevelGameInfo.PLAYER_ONE_STATS.BonusHP++;
                                playerStats.SetHealth(playerStats.Health + 1);
                            }
                            if (PlayerManager.Multiplayer)
                            {
                                if (DicePalaceMainLevelGameInfo.PLAYER_TWO_STATS == null)
                                {
                                    DicePalaceMainLevelGameInfo.PLAYER_TWO_STATS = new PlayersStatsBossesHub();
                                }
                                PlayerStatsManager stats = PlayerManager.GetPlayer(PlayerId.PlayerTwo).stats;
                                if (stats.Health > 0)
                                {
                                    DicePalaceMainLevelGameInfo.PLAYER_TWO_STATS.BonusHP++;
                                    stats.SetHealth(stats.Health + 1);
                                }
                            }
                            self.boardSpacesObj[DicePalaceMainLevelGameInfo.HEART_INDEXES[i] + 1].HasHeart = false;
                            self.heart.transform.position = self.boardSpacesObj[DicePalaceMainLevelGameInfo.HEART_INDEXES[i] + 1].HeartSpacePosition;
                            self.heart.SetActive(true);
                            AudioManager.Play("pop_up");
                            self.emitAudioFromObject.Add("pop_up");
                            yield return CupheadTime.WaitForSeconds(self, 1.5f);
                            self.heart.SetActive(false);
                            DicePalaceMainLevelGameInfo.HEART_INDEXES[i] = -1;
                            break;
                        }
                    }
                    yield return self.StartCoroutine(self.start_mini_boss_cr(self.SelectLevel(space)));
                }
                self.dice.waitingToRoll = true;
                yield return null;
            }
            yield return null;
        }
        self.EndBoardGame(self.dice);
        yield break;
    }

    bool moveMarkerBack;
}
