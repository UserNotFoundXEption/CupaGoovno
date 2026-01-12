using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class MermaidEel
{
    public void Init()
    {
        On.FlyingMermaidLevelEel.Awake += Awake;
    }

    protected void Awake(On.FlyingMermaidLevelEel.orig_Awake orig, FlyingMermaidLevelEel self)
    {
        orig(self);
        self.transform.SetScale(3f, 3f, null);
        bulletPrefab = self.projectilePrefab;
    }

    public IEnumerator main_cr(On.FlyingMermaidLevelEel.orig_main_cr orig, FlyingMermaidLevelEel self)
    {
        yield return CupheadTime.WaitForSeconds(self, self.properties.appearDelay.RandomFloat());
        self.state = FlyingMermaidLevelEel.State.Spawned;
        self.animator.SetTrigger("Spawn");
        AudioManager.Play("level_mermaid_eel_intro");
        float t = 0f;
        self.hp = self.properties.hp;
        Collider2D collider = self.GetComponent<Collider2D>();
        collider.enabled = true;
        while (t < self.riseTime - 0.25f)
        {
            t += CupheadTime.Delta;
            self.transform.SetLocalPosition(null, new float?(Mathf.Lerp(self.initialY - self.riseDistance, self.initialY, t / self.riseTime)), null);
            yield return null;
        }
        self.animator.SetTrigger("Continue");
        while (t < self.riseTime)
        {
            t += CupheadTime.Delta;
            self.transform.SetLocalPosition(null, new float?(Mathf.Lerp(self.initialY - self.riseDistance, self.initialY, t / self.riseTime)), null);
            yield return null;
        }
        self.transform.SetLocalPosition(null, new float?(self.initialY), null);
        yield return self.animator.WaitForAnimationToStart(self, "Idle", false);
        for (int numAttacks = self.properties.attackAmount.RandomInt(); numAttacks >= 0; numAttacks--)
        {
            //yield return CupheadTime.WaitForSeconds(self, self.properties.idleTime.RandomFloat());
            AudioManager.Play("level_mermaid_eel_attack_start");
            self.animator.SetTrigger("Attack");
            yield return self.animator.WaitForAnimationToEnd(self, "Attack_Start", false, true);
            self.FireProjectiles();
            yield return self.animator.WaitForAnimationToEnd(self, "Attack_End", false, true);
            AudioManager.Play("level_mermaid_eel_attack_end");
            yield return CupheadTime.WaitForSeconds(self, self.properties.idleTime.RandomFloat());//new
        }
        yield return CupheadTime.WaitForSeconds(self, self.properties.idleTime.RandomFloat());
        self.animator.SetTrigger("Leave");
        yield return self.animator.WaitForAnimationToEnd(self, "Leave_Start", false, true);
        AudioManager.Play("level_mermaid_eel_attack_leave");
        t = 0f;
        bool spawnedSplash = false;
        SpriteRenderer sprite = self.GetComponent<SpriteRenderer>();
        float waterY = (float)((!(sprite.sortingLayerName == "Foreground")) ? -270 : -380);
        while (t < self.leaveTime)
        {
            t += CupheadTime.Delta;
            self.transform.SetLocalPosition(null, new float?(Mathf.Lerp(self.initialY, self.initialY - self.riseDistance, t / self.leaveTime)), null);
            if (!spawnedSplash && self.transform.position.y < waterY - 80f)
            {
                FlyingMermaidLevelSplashManager.Instance.SpawnSplashMedium(self.gameObject, 35f, true, waterY + 80f);
                spawnedSplash = true;
            }
            yield return null;
        }
        self.Die(false, false);
        yield break;
    }

    public static BasicProjectile bulletPrefab;
}
