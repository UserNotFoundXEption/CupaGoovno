using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class Chips
{
    public void Init()
    {
        On.DicePalaceChipsLevelChips.chipAttack_cr += chipAttack_cr;
    }

    private IEnumerator chipAttack_cr(On.DicePalaceChipsLevelChips.orig_chipAttack_cr orig, DicePalaceChipsLevelChips self)
    {
        LevelProperties.DicePalaceChips.Chips p = self.properties.CurrentState.chips;
        yield return CupheadTime.WaitForSeconds(self, self.properties.CurrentState.chips.initialAttackDelay);
        int mainStringIndex = UnityEngine.Random.Range(0, p.chipAttackString.Length);
        int dir = -1;
        //int attackIndex = UnityEngine.Random.Range(0, self.maxAttacksPerCycle);
        int attackIndex = 0;//new
        int cycleCounter = 0;//new
        for (; ; )
        {
            if(cycleCounter == 0)//new
            {//new
                self.animator.SetBool("IsSpread", true);
                yield return self.animator.WaitForAnimationToStart(self, "Spread_Open", false);
            }//new
            cycleCounter++;//new
            string[] currentAttackChips = p.chipAttackString[mainStringIndex].Split(new char[]
            {
                ','
            });
            self.maxAttacksPerCycle = currentAttackChips.Length;
            for (int j = 0; j < self.chips.Length; j++)
            {
                float rotationSpeed = (!Rand.Bool()) ? -5f : 5f;
                self.chips[j].rotationSpeed = rotationSpeed;
            }
            self.StartCoroutine(warningBeams_cr(self, currentAttackChips));//new
            //self.animator.SetBool("IsSpread", true);
            //yield return self.animator.WaitForAnimationToStart(self, "Spread_Open", false);
            float startPos = self.chips[self.chips.Length - 1].chipTransform.position.y;
            float frameTime = 0f;
            float time = 0.5f;//1.5f
            float t = 0f;
            int counter = 0;
            while (t < time)
            {
                float val = EaseUtils.Ease(EaseUtils.EaseType.linear, 0f, 1f, t / time);
                frameTime += CupheadTime.Delta * self.animator.speed;
                t += CupheadTime.Delta * self.animator.speed;
                if (frameTime > 0.0416666679f)
                {
                    frameTime -= 0.0416666679f;
                    for (int k = self.chips.Length - 1; k >= 0; k--)
                    {
                        float num = (k != 0) ? (self.chips[k].chipTransform.GetComponent<Renderer>().bounds.size.y / 1.7f) : (self.chips[k].chipTransform.GetComponent<Renderer>().bounds.size.y / 5.5f);
                        float b = startPos + (float)counter * num;
                        Vector3 position = self.chips[k].chipTransform.position;
                        position.y = Mathf.Lerp(position.y, b, val);
                        self.chips[k].chipTransform.position = position;
                        counter = (counter + 1) % self.chips.Length;
                        float num2 = Mathf.Sin(t / 0.7f);
                        self.chips[k].chipTransform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, num2 * self.chips[k].rotationSpeed));
                    }
                }
                yield return null;
            }
            self.currentlyFloating = true;
            foreach (DicePalaceChipsLevelChips.ChipPieces chipPieces in self.chips)
            {
                self.StartCoroutine(self.rotate_chips_cr(chipPieces.chipTransform, chipPieces.rotationSpeed, 0.7f, t));
            }
            yield return null;
            for (int i = attackIndex; i < currentAttackChips.Length; i++)
            {
                string[] currentAttackChipsMultiple = currentAttackChips[i].Split(new char[]
                {
                    '-'
                });
                self.SFX_DicePalaceChipsShoot();
                foreach (string chip in currentAttackChipsMultiple)
                {
                    if (chip[0] == 'D')
                    {
                        yield return CupheadTime.WaitForSeconds(self, Parser.FloatParse(chip.Substring(1)));
                    }
                    else if (self.currentAttackCount < self.maxAttacksPerCycle - 1)
                    {
                        self.StartCoroutine(self.moveChip_cr(self.transform.GetChild(Parser.IntParse(chip) - 1).transform, dir, false));
                    }
                    else
                    {
                        self.StartCoroutine(self.moveChip_cr(self.transform.GetChild(Parser.IntParse(chip) - 1).transform, dir, true));
                    }
                }
                self.currentAttackCount++;
                if (self.currentAttackCount >= self.maxAttacksPerCycle)
                {
                    self.currentAttackCount = 0;
                    //attackIndex = UnityEngine.Random.Range(0, self.maxAttacksPerCycle);
                    attackIndex = 0;//new
                }
                else
                {
                    yield return CupheadTime.WaitForSeconds(self, self.properties.CurrentState.chips.chipAttackDelay);
                }
                attackIndex = 0;
            }
            while (self.chipInFlight)
            {
                yield return null;
            }
            //yield return CupheadTime.WaitForSeconds(self, 1f);
            time = 0.1f;//0.3
            t = 0f;
            counter = 0;
            if(cycleCounter == 5)//new
            {//new
                self.animator.SetBool("IsSpread", false);
                yield return self.animator.WaitForAnimationToStart(self, "Spread_Close", false);
                time = 3f;//new start
                cycleCounter = 0;
            }//new end
            //yield return CupheadTime.WaitForSeconds(self, 0.4f);
            self.currentlyFloating = false;
            while (t < time)
            {
                float val2 = EaseUtils.Ease(EaseUtils.EaseType.linear, 0f, 1f, t / time);
                for (int n = self.chips.Length - 1; n >= 0; n--)
                {
                    float num3 = 0f;
                    if (n != self.chips.Length - 1)
                    {
                        num3 = 10f;
                    }
                    float b2 = self.chips[n].startPosition.y - (float)counter * num3;
                    Vector3 position2 = self.chips[n].chipTransform.position;
                    position2.y = Mathf.Lerp(position2.y, b2, val2);
                    self.chips[n].chipTransform.position = position2;
                    counter = (counter + 1) % self.chips.Length;
                }
                t += CupheadTime.Delta;
                yield return null;
            }
            dir *= -1;
            yield return null;
            mainStringIndex = (mainStringIndex + 1) % p.chipAttackString.Length;
            float tt = 0f;
            while (tt < self.properties.CurrentState.chips.attackCycleDelay)
            {
                tt += CupheadTime.Delta * self.animator.speed;
                yield return null;
            }
        }
    }

    private IEnumerator warningBeams_cr(DicePalaceChipsLevelChips self, string[] currentAttackChips)
    {
        float[] yPos = {303f, 85f, 28f, -29f, -86f, -143f, -200f, -257f };
        float delay = 0.4f;
        List<GameObject> beams = [];
        for (int i = 0; i < currentAttackChips.Length; i++)
        {
            string[] currentAttackChipsMultiple = currentAttackChips[i].Split('-');
            List<float> chipIndexes = [];
            foreach (string chip in currentAttackChipsMultiple)
            {
                chipIndexes.Add(Parser.IntParse(chip) - 1);
            }
            for(int j = 1; j < 8; j++)
            {
                float chipY = yPos[j];
                GameObject warningBeam = GameObject.CreatePrimitive(PrimitiveType.Cube);
                warningBeam.transform.localScale = new Vector3(1500f, 57f, 1f);
                warningBeam.transform.position = new Vector3(0f, chipY, 0f);
                if (chipIndexes.Contains(j))
                {
                    Other.SetTransparentMaterial(warningBeam, new UnityEngine.Color(1f, 0f, 0f, 0.5f));
                }
                else
                {
                    Other.SetTransparentMaterial(warningBeam, new UnityEngine.Color(0f, 1f, 0f, 0.5f));
                }
                beams.Add(warningBeam);
            }
            yield return CupheadTime.WaitForSeconds(self, delay);
            while(beams.Count > 0)
            {
                GameObject.Destroy(beams[0]);
                beams.RemoveAt(0);
            }
        }
    }
}
