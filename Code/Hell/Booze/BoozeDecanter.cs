using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class BoozeDecanter
{
    public void Init()
    {
        On.DicePalaceBoozeLevelDecanter.Awake += Awake;
        On.DicePalaceBoozeLevelDecanter.attack_cr += attack_cr;
        On.DicePalaceBoozeLevelDecanter.OnDamageTaken += OnDamageTaken;
    }

    protected void Awake(On.DicePalaceBoozeLevelDecanter.orig_Awake orig, DicePalaceBoozeLevelDecanter self)
    {
        orig(self);
        decanter = self;
    }

    private IEnumerator attack_cr(On.DicePalaceBoozeLevelDecanter.orig_attack_cr orig, DicePalaceBoozeLevelDecanter self)
    {
        yield return null;
    }

    private void OnDamageTaken(On.DicePalaceBoozeLevelDecanter.orig_OnDamageTaken orig, DicePalaceBoozeLevelDecanter self, DamageDealer.DamageInfo info)
    {
        bool isSuper = info.damageSource == DamageDealer.DamageSource.Super;
        Super super = PlayerManager.GetFirst().stats.Loadout.super;
        bool superOneEquiped = super == Super.level_super_beam;
        if (isSuper && superOneEquiped)
        {
            self.health += info.damage * 8 / 9;
            BoozeTumbler.tumbler.health -= info.damage / 9;
            BoozeMartini.martini.health -= info.damage / 9;
        }
        else
        {
            self.health += info.damage * 2 / 3;
            BoozeTumbler.tumbler.health -= info.damage / 3;
            BoozeMartini.martini.health -= info.damage / 3;
        }
        orig(self, info);
        if (self.health <= 0)
        {
            if (!BoozeTumbler.tumbler.isDead)
            {
                BoozeTumbler.tumbler.StartDying();
            }
            if (!BoozeMartini.martini.isDead)
            {
                BoozeMartini.martini.StartDying();
            }
        }
    }

    public static void AttackOnce(float x1, float x2)
    {
        attacking = true;
        decanter.StartCoroutine(attackOnce_cr(x1, x2));
    }

    private static IEnumerator attackOnce_cr(float x1, float x2)
    {
        /*for (; ; )
        /{
        yield return CupheadTime.WaitForSeconds(decanter, Parser.FloatParse(decanter.properties.CurrentState.decanter.attackDelayString.Split(new char[]
        {
            ','
        })[decanter.attackDelayIndex]) - DicePalaceBoozeLevelBossBase.ATTACK_DELAY);*/
        decanter.animator.SetTrigger("OnAttack");
        yield return decanter.animator.WaitForAnimationToStart(decanter, "Attack", false);
        AudioManager.Play("booze_decanter_attack");
        decanter.emitAudioFromObject.Add("booze_decanter_attack");
        decanter.StartCoroutine(spray_cr(x1, x2));
        /*decanter.attackDelayIndex++;
        if (decanter.attackDelayIndex >= decanter.properties.CurrentState.decanter.attackDelayString.Split(new char[]
        {
            ','
        }).Length)
        {
            decanter.attackDelayIndex = 0;
        }*/
        if (decanter.nextPlayerTarget == PlayerId.PlayerOne)
        {
            if (PlayerManager.GetPlayer(PlayerId.PlayerTwo) != null)
            {
                decanter.nextPlayerTarget = PlayerId.PlayerTwo;
            }
        }
        else
        {
            decanter.nextPlayerTarget = PlayerId.PlayerOne;
        }
        while (decanter.attacking)
        {
            yield return null;
        }
        //}
        yield break;
    }

    private static IEnumerator spray_cr(float x1, float x2)
    {
        decanter.attacking = true;
        yield return CupheadTime.WaitForSeconds(decanter, decanter.properties.CurrentState.decanter.beamAppearDelayRange.RandomFloat());
        AudioManager.Play("booze_decanter_spray_down");
        decanter.emitAudioFromObject.Add("booze_decanter_spray_down");
        GameObject spray = UnityEngine.Object.Instantiate<GameObject>(decanter.sprayPrefab, decanter.dropPosition, Quaternion.identity);
        GameObject spray2 = UnityEngine.Object.Instantiate<GameObject>(decanter.sprayPrefab, decanter.dropPosition, Quaternion.identity);//new
        decanter.attacking = false;
        /*decanter.dropPosition.x = PlayerManager.GetPlayer(decanter.nextPlayerTarget).center.x;
        Vector3 pos = spray.transform.position;
        pos.x = decanter.dropPosition.x;
        spray.transform.position = pos;*/
        spray.transform.SetPosition(x1, null, null);//new
        spray2.transform.SetPosition(x2, null, null);//new
        yield return spray.GetComponent<Animator>().WaitForAnimationToEnd(decanter, "Spray", false, true);
        UnityEngine.Object.Destroy(spray);
        UnityEngine.Object.Destroy(spray2);//new
        attacking = false;//new
        yield break;
    }

    public static DicePalaceBoozeLevelDecanter decanter;
    public static bool attacking;
}
