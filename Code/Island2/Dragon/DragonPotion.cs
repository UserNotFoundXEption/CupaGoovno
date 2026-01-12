using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CupaGoovno;

public class DragonPotion
{
    public void Init()
    {
        On.DragonLevelPotion.Awake += Awake;
        On.DragonLevelPotion.OnDamageTaken += OnDamageTaken;
        On.DragonLevelPotion.handle_die_cr += handle_die_cr;
    }

    protected void Awake(On.DragonLevelPotion.orig_Awake orig, DragonLevelPotion self)
    {
        orig(self);
        blimpScene = SceneManager.GetActiveScene().name == "scene_level_flying_blimp";
        blimpIsGemini = Blimp.blimpLady && Blimp.blimpLady.properties.CurrentState.stateName == LevelProperties.FlyingBlimp.States.Gemini;
        if (blimpScene)
        {
            self.StartCoroutine(autodestruction_cr(self));
        }
    }

    private void OnDamageTaken(On.DragonLevelPotion.orig_OnDamageTaken orig, DragonLevelPotion self, DamageDealer.DamageInfo info)
    {
        if (info.damageSource != DamageDealer.DamageSource.SmallPlane)
        {
            orig(self, info);
        }
    }

    private IEnumerator handle_die_cr(On.DragonLevelPotion.orig_handle_die_cr orig, DragonLevelPotion self)
    {
        DragonLevelPotion.PotionType potionType = self.type;
        if (potionType != DragonLevelPotion.PotionType.Horizontal)
        {
            if (potionType != DragonLevelPotion.PotionType.Vertical)
            {
                if (potionType == DragonLevelPotion.PotionType.Both)
                {
                    SpawnProjectile(self, Vector3.right);
                    SpawnProjectile(self, -Vector3.right);
                    SpawnProjectile(self, Vector3.up);
                    SpawnProjectile(self, - Vector3.up);
                }
            }
            else
            {
                SpawnProjectile(self, Vector3.up);
                SpawnProjectile(self, - Vector3.up);
            }
        }
        else
        {
            SpawnProjectile(self, Vector3.right);
            SpawnProjectile(self, - Vector3.right);
        }
        self.animator.SetTrigger("Explode");
        self.GetComponent<Collider2D>().enabled = false;
        yield return CupheadTime.WaitForSeconds(self, 0.5f);
        UnityEngine.Object.Destroy(self.gameObject);
        yield return null;
        yield break;
    }

    private void SpawnProjectile(DragonLevelPotion self, Vector3 direction)
    {
        float rotation = MathUtils.DirectionToAngle(direction);
        //self.bulletPrefab.Create(self.transform.position, rotation, self.properties.spitBulletSpeed).transform.SetScale(new float?(self.properties.explosionBulletScale), new float?(self.properties.explosionBulletScale), new float?(self.properties.explosionBulletScale));
        BasicProjectile proj = self.bulletPrefab.Create(self.transform.position, rotation, self.properties.spitBulletSpeed);//new start
        if (!blimpScene || !blimpIsGemini)
        {
            float scale = self.properties.explosionBulletScale;
            proj.transform.SetScale(new float?(scale), new float?(scale), new float?(scale));
        }//new end
    }

    private IEnumerator autodestruction_cr(DragonLevelPotion self)//new
    {
        yield return CupheadTime.WaitForSeconds(self, 1.2f);
        self.StartCoroutine(self.handle_die_cr());
        yield break;
    }

    private bool blimpScene;
    private bool blimpIsGemini;
}
