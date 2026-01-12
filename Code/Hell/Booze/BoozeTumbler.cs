using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CupaGoovno;

public class BoozeTumbler
{
    public void Init()
    {
        On.DicePalaceBoozeLevelTumbler.Awake += Awake;
        On.DicePalaceBoozeLevelTumbler.attack_cr += attack_cr;
        On.DicePalaceBoozeLevelTumbler.OnDamageTaken += OnDamageTaken;
    }

    protected void Awake(On.DicePalaceBoozeLevelTumbler.orig_Awake orig, DicePalaceBoozeLevelTumbler self)
    {
        orig(self);
        tumbler = self;
    }

    private IEnumerator attack_cr(On.DicePalaceBoozeLevelTumbler.orig_attack_cr orig, DicePalaceBoozeLevelTumbler self)
    {
        yield return null;
    }

    private void OnDamageTaken(On.DicePalaceBoozeLevelTumbler.orig_OnDamageTaken orig, DicePalaceBoozeLevelTumbler self, DamageDealer.DamageInfo info)
    {
        bool isSuper = info.damageSource == DamageDealer.DamageSource.Super;
        Super super = PlayerManager.GetFirst().stats.Loadout.super;
        bool superOneEquiped = super == Super.level_super_beam;
        if (isSuper && superOneEquiped)
        {
            self.health += info.damage * 8 / 9;
            BoozeDecanter.decanter.health -= info.damage / 9;
            BoozeMartini.martini.health -= info.damage / 9;
        }
        else
        {
            self.health += info.damage * 2 / 3;
            BoozeDecanter.decanter.health -= info.damage / 3;
            BoozeMartini.martini.health -= info.damage / 3;
        }
        orig(self, info);
        if (self.health <= 0)
        {
            if (!BoozeMartini.martini.isDead)
            {
                BoozeMartini.martini.StartDying();
            }
            if (!BoozeDecanter.decanter.isDead)
            {
                BoozeDecanter.decanter.StartDying();
            }
        }
    }

    public static void AttackOnce()
    {
        attacking = true;
        tumbler.StartCoroutine(attackOnce_cr());
    }

    public static IEnumerator attackOnce_cr()
    {
        /*for (; ; )
        {
            yield return CupheadTime.WaitForSeconds(tumbler, Parser.FloatParse(tumbler.properties.CurrentState.tumbler.beamDelayString.Split(new char[]
            {
                ','
            })[tumbler.attackDelayIndex]) - DicePalaceBoozeLevelBosstumbler.ATTACK_DELAY);*/
        tumbler.animator.SetTrigger("OnAttack");
        yield return tumbler.animator.WaitForAnimationToEnd(tumbler, "Attack_Start", false, true);
        yield return CupheadTime.WaitForSeconds(tumbler, tumbler.properties.CurrentState.tumbler.beamWarningDuration);
        tumbler.animator.SetTrigger("Continue");
        yield return tumbler.animator.WaitForAnimationToStart(tumbler, "Attack", false);
        AudioManager.Play("booze_tumbler_attack");
        tumbler.emitAudioFromObject.Add("booze_tumbler_attack");
        yield return tumbler.animator.WaitForAnimationToEnd(tumbler, "Attack", false, true);
        attacking = false;//new
        /*tumbler.attackDelayIndex = (tumbler.attackDelayIndex + 1) % tumbler.properties.CurrentState.tumbler.beamDelayString.Split(new char[]
        {
            ','
        }).Length;
        yield return null;
        }*/
        yield break;
    }

    public static DicePalaceBoozeLevelTumbler tumbler;
    public static bool attacking;
}
