using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class Cigar
{
    public void Init()
    {
        new CigarGhost().Init();
        On.DicePalaceCigarLevelCigar.Awake += Awake;
        On.DicePalaceCigarLevelCigar.attack_cr += attack_cr;
        On.DicePalaceCigarLevelCigar.ghostAttack_cr += ghostAttack_cr;
    }

    protected void Awake(On.DicePalaceCigarLevelCigar.orig_Awake orig, DicePalaceCigarLevelCigar self)
    {
        orig(self);
        self.transform.SetScale(1.5f, 1.5f, null);
    }

    private IEnumerator attack_cr(On.DicePalaceCigarLevelCigar.orig_attack_cr orig, DicePalaceCigarLevelCigar self)
    {
        self.StartCoroutine(verticalSpit_cr(self));
        for (; ; )
        {
            self.GetComponent<BoxCollider2D>().enabled = true;
            /*self.maxCounter = Parser.IntParse(self.properties.CurrentState.spiralSmoke.attackCount.Split(new char[]
            {
                ','
            })[self.spitAttackCountIndex]);
            self.isFiring = true;
            while (self.isFiring)
            {
                if (self.counter > self.maxCounter)
                {
                    self.isFiring = false;
                    self.counter = 0;
                    break;
                }
                yield return CupheadTime.WaitForSeconds(self, self.properties.CurrentState.spiralSmoke.hesitateBeforeAttackDelay);
                self.counter++;
                self.animator.SetTrigger("IsAttacking");
                yield return self.animator.WaitForAnimationToEnd(self, "Attack", false, true);
                yield return null;
            }
            self.spitAttackCountIndex++;
            if (self.spitAttackCountIndex >= self.properties.CurrentState.spiralSmoke.attackCount.Split(new char[]
            {
                ','
            }).Length)
            {
                self.spitAttackCountIndex = 0;
            }
            self.spitAttackDirectionIndex++;
            if (self.spitAttackDirectionIndex >= self.properties.CurrentState.spiralSmoke.rotationDirectionString.Split(new char[]
            {
                ','
            }).Length)
            {
                self.spitAttackDirectionIndex = 0;
            }*/
            yield return CupheadTime.WaitForSeconds(self, self.properties.CurrentState.cigar.warningDelay);
            self.animator.SetTrigger("OnStateChange");
            yield return self.animator.WaitForAnimationToEnd(self, "Teleport_End", false, true);
            yield return null;
        }
    }

    private IEnumerator ghostAttack_cr(On.DicePalaceCigarLevelCigar.orig_ghostAttack_cr orig, DicePalaceCigarLevelCigar self)
    {
        cigar = self;//new
        string[] delays = self.properties.CurrentState.cigaretteGhost.attackDelayString.Split(new char[]{','});
        string[] spawns = self.properties.CurrentState.cigaretteGhost.spawnPositionString.Split(new char[]{','});
        for (; ; )
        {
            //float spawnPosx = UnityEngine.Random.Range(self.ghostSpawnPoint.transform.position.x - self.ghostOffset, self.ghostSpawnPoint.transform.position.x + self.ghostOffset);
            bool left = PlayerManager.GetFirst().transform.position.x > 0;//new start
            float spawnPosx = left ? -700f : 700f;
            float spawnPosy = UnityEngine.Random.Range(-100f, 0f);//new end
            //yield return CupheadTime.WaitForSeconds(self, Parser.FloatParse(delays[self.ghostAttackDelayIndex]));
            //AbstractProjectile proj = self.ghostPrefab.Create(new Vector2(spawnPosx, self.ghostSpawnPoint.transform.position.y));
            AbstractProjectile proj = self.ghostPrefab.Create(new Vector2(spawnPosx, spawnPosy));//new
            proj.GetComponent<DicePalaceCigarLevelCigaretteGhost>().InitGhost(self.properties);
            self.ghostAttackDelayIndex++;
            if (self.ghostAttackDelayIndex >= delays.Length)
            {
                self.ghostAttackDelayIndex = 0;
            }
            self.ghostSpawnPositionIndex++;
            if (self.ghostSpawnPositionIndex >= spawns.Length)
            {
                self.ghostSpawnPositionIndex = 0;
            }
            yield return CupheadTime.WaitForSeconds(self, Parser.FloatParse(delays[self.ghostAttackDelayIndex]));//new
        }
    }

    private IEnumerator verticalSpit_cr(DicePalaceCigarLevelCigar self)
    {
        bool clockwise = true;
        for(; ; )
        {
            float x = UnityEngine.Random.Range(-100f, 100f);
            float y = UnityEngine.Random.Range(400f, 500f);
            Vector2 pos = new Vector2(x, y);
            AbstractProjectile abstractProjectile = self.spitPrefab.Create(pos, 0f);
            DicePalaceCigarLevelCigarSpit spit = abstractProjectile.GetComponent<DicePalaceCigarLevelCigarSpit>();
            CigarSpit.InitProjectileVertical(spit, self.properties, clockwise, self.onRightSpawn);
            clockwise = !clockwise;
            yield return CupheadTime.WaitForSeconds(self, self.properties.CurrentState.spiralSmoke.hesitateBeforeAttackDelay);
        }
    }

    public static DicePalaceCigarLevelCigar cigar;
}
