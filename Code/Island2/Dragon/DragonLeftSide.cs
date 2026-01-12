using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CupaGoovno;

public class DragonLeftSide
{
    public void Init()
    {
        On.DragonLevelLeftSideDragon.spawnFireMarchers_cr += spawnFireMarchers_cr;
        On.DragonLevelLeftSideDragon.potion_cr += potion_cr;
    }

    private IEnumerator spawnFireMarchers_cr(On.DragonLevelLeftSideDragon.orig_spawnFireMarchers_cr orig, DragonLevelLeftSideDragon self)
    {
        self.fireMarcherLeaderPrefab.Create(self.fireMarcherRoot, self.properties.CurrentState.fireMarchers);
        for (; ; )
        {
            yield return CupheadTime.WaitForSeconds(self, self.properties.CurrentState.fireMarchers.spawnDelay);
            //self.lastFireMarcher = self.fireMarcherPrefabs.RandomChoice<DragonLevelFireMarcher>().Create(self.fireMarcherRoot, self.properties.CurrentState.fireMarchers);
            self.lastFireMarcher = self.fireMarcherPrefabs[1].Create(self.fireMarcherRoot, self.properties.CurrentState.fireMarchers);//new
            yield return null;
        }
    }

    private IEnumerator potion_cr(On.DragonLevelLeftSideDragon.orig_potion_cr orig, DragonLevelLeftSideDragon self)
    {
        LevelProperties.Dragon.Potions p = self.properties.CurrentState.potions;
        string[] attackCountString = p.attackCount[self.attackCountMainIndex].Split(',');
        string[] shotPositionString = p.shotPositionString[self.shotPositionMainIndex].Split(',');
        int attackCount = 0;
        bool aTop = true;//new start
        DragonLevelLeftSideDragon.HeadPicked top = DragonLevelLeftSideDragon.HeadPicked.ATop;
        DragonLevelLeftSideDragon.HeadPicked bottom = DragonLevelLeftSideDragon.HeadPicked.CBottom;
        yield return CupheadTime.WaitForSeconds(self, 3f);//new end
        for (; ; )
        {
            attackCountString = p.attackCount[self.attackCountMainIndex].Split(',');
            Parser.IntTryParse(attackCountString[self.attackCountIndex], out attackCount);
            for (int i = 0; i < attackCount; i++)
            {
                /*shotPositionString = p.shotPositionString[self.shotPositionMainIndex].Split(',');
                string[] pickedDragon = shotPositionString[self.shotPositionIndex].Split(':');
                foreach (string picked in pickedDragon)
                {*/
                    while (self.torch)
                    {
                        yield return null;
                    }
                    /*if (shotPositionString[self.shotPositionIndex][0] == 'T')
                    {
                        self.animationString = "High_Attack";
                    }
                    else if (shotPositionString[self.shotPositionIndex][0] == 'B')
                    {
                        self.animationString = "Low_Attack";
                    }
                    if (picked == "A")
                    {
                        self.layer = 5;
                    }
                    else if (picked == "C")
                    {
                        self.layer = 6;
                    }
                }
                if (self.layer == 5 && self.animationString == "High_Attack")
                {
                    self.headPicked = DragonLevelLeftSideDragon.HeadPicked.CTop;
                }
                else if (self.layer == 6 && self.animationString == "High_Attack")
                {
                    self.headPicked = DragonLevelLeftSideDragon.HeadPicked.ATop;
                }
                else if (self.layer == 5 && self.animationString == "Low_Attack")
                {
                    self.headPicked = DragonLevelLeftSideDragon.HeadPicked.CBottom;
                }
                else if (self.layer == 6 && self.animationString == "Low_Attack")
                {
                    self.headPicked = DragonLevelLeftSideDragon.HeadPicked.ABottom;
                }
                yield return self.animator.WaitForAnimationToEnd(self, self.animationString, self.layer, false, true);
                if (self.shotPositionIndex < shotPositionString.Length - 1)
                {
                    self.shotPositionIndex++;
                }
                else
                {
                    self.shotPositionMainIndex = (self.shotPositionMainIndex + 1) % p.shotPositionString.Length;
                    self.shotPositionIndex = 0;
                }*/
                DragonLevelLeftSideDragon.HeadPicked picked = aTop ? top : bottom;//new start
                self.headPicked = picked;
                self.animationString = aTop ? "High_Attack" : "Low_Attack";
                self.layer = aTop ? 6 : 5;
                self.PotionAttack(picked);
                yield return self.animator.WaitForAnimationToEnd(self, self.animationString, self.layer, false, true);
                aTop = !aTop;//new end
                yield return CupheadTime.WaitForSeconds(self, p.repeatDelay);
            }
            if (self.attackCountIndex < attackCountString.Length - 1)
            {
                self.attackCountIndex++;
            }
            else
            {
                self.attackCountMainIndex = (self.attackCountMainIndex + 1) % p.attackCount.Length;
                self.attackCountIndex = 0;
            }
            yield return CupheadTime.WaitForSeconds(self, p.attackMainDelay);
            yield return null;
        }
    }
}
