using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class Horse
{
    public void Init()
    {
        new HorsePresent().Init();
        On.DicePalaceFlyingHorseLevelHorse.spawn_present_cr += spawn_present_cr;
        On.DicePalaceFlyingHorseLevelHorse.mini_horses_cr += mini_horses_cr;
    }

    private IEnumerator spawn_present_cr(On.DicePalaceFlyingHorseLevelHorse.orig_spawn_present_cr orig, DicePalaceFlyingHorseLevelHorse self)
    {
        LevelProperties.DicePalaceFlyingHorse.GiftBombs p = self.properties.CurrentState.giftBombs;
        //float positionX = 0f;
        //float positionY = 0f;
        //Vector3 endPos = Vector3.zero;
        Vector3 endPos = new Vector3(-200f, 0f);//new
        AbstractPlayerController player = PlayerManager.GetNext();
        /*string[] giftPositionXPattern = p.giftPositionStringX[self.giftPosXMainIndex].Split(',');
        string[] giftPositionYPattern = p.giftPositionStringY[self.giftPosYMainIndex].Split(',');
        if (self.playerAimCounter >= self.playerAimMaxCounter)
        {
            endPos = player.transform.position;
            player = PlayerManager.GetNext();
            self.playerAimMaxCounter = p.playerAimRange.RandomInt();
            self.playerAimCounter = 0;
        }
        else
        {
            Parser.FloatTryParse(giftPositionXPattern[self.giftPosXIndex], out positionX);
            Parser.FloatTryParse(giftPositionYPattern[self.giftPosYIndex], out positionY);
            endPos.x = -640f + positionX;
            endPos.y = 360f - positionY;
            self.playerAimCounter++;
        }*/
        DicePalaceFlyingHorseLevelPresent present = UnityEngine.Object.Instantiate<DicePalaceFlyingHorseLevelPresent>(self.presentPrefab);
        present.Init(self.projectileRoot.position, endPos, self.properties.CurrentState.giftBombs);
        /*if (self.giftPosXIndex < giftPositionXPattern[self.giftPosXIndex].Length)
        {
            self.giftPosXIndex++;
        }
        else
        {
            self.giftPosXMainIndex = (self.giftPosXMainIndex + 1) % p.giftPositionStringX.Length;
            self.giftPosXIndex = 0;
        }
        if (self.giftPosYIndex < giftPositionYPattern[self.giftPosYIndex].Length)
        {
            self.giftPosYIndex++;
        }
        else
        {
            self.giftPosYMainIndex = (self.giftPosYMainIndex + 1) % p.giftPositionStringY.Length;
            self.giftPosYIndex = 0;
        }*/
        yield return null;
        yield break;
    }

    private IEnumerator mini_horses_cr(On.DicePalaceFlyingHorseLevelHorse.orig_mini_horses_cr orig, DicePalaceFlyingHorseLevelHorse self)
    {
        yield break;
    }

    public static DicePalaceFlyingHorseLevelHorse horse;
}
