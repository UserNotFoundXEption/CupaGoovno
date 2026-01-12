using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace CupaGoovno;

public class EquipmentReworks
{
    public void Init()
    {
        CrackshotBasic();
        CrackshotEx();
        HomingBasic();
        HomingEx();
        WideshotBasic();
        ChargeBasic();
        PlaneBombEx();
        ParryAttack();
        Curse();
        SuperGhostCuphead();
        SuperInvincibilityCuphead();
        SuperBeamCuphead();
        PlaneSuperBomb();
        CharmCurse();
        On.WeaponWideShot.angle_cr += angle_cr;
        On.WeaponHoming.fireEx += fireEx;
        On.WeaponHomingProjectile.FindTarget += FindTarget;
        On.PlaneWeaponBombExProjectile.FindTarget += FindTarget;
        On.WeaponCrackshotExProjectile.Start += Start;
        On.WeaponCrackshotExProjectile.LaunchAtTarget += LaunchAtTarget;
        On.PlayerSuperGhost.StartSuper += StartSuper;
        On.PlayerSuperInvincible.StartSuper += StartSuper;
        On.WeaponCharge.FixedUpdate += FixedUpdate;
        On.PlaneSuperBomb.super_cr += super_cr;
        On.PlaneSuperChalice.super_cr += super_cr;
        On.PlayerSuperBeam.Fire += Fire;
        On.PlayerSuperChaliceVerticalBeam.Fire += Fire;
        On.PlayerSuperGhost.super_cr += super_cr;
        On.DamageDealer.DealDamage += DealDamage;
    }

    private void CrackshotBasic()
    {
        Type type = typeof(WeaponProperties.LevelWeaponCrackshot.Basic);
        AccessTools.Field(type, "initialDamage").SetValue(null, 5f);//10.56f
        AccessTools.Field(type, "crackedDamage").SetValue(null, 2f);//6.7f
    }

    private void CrackshotEx()
    {
        Type type = typeof(WeaponProperties.LevelWeaponCrackshot.Ex);
        AccessTools.Field(type, "bulletDamage").SetValue(null, 1.5f);//3.5f
        AccessTools.Field(type, "shotNumber").SetValue(null, 2137);//5
    }

    private void HomingBasic()
    {
        Type type = typeof(WeaponProperties.LevelWeaponHoming.Basic);
        AccessTools.Field(type, "damage").SetValue(null, 2f);//2.85f
        AccessTools.Field(type, "rotationSpeed").SetValue(null, new MinMax(0f, 2137f));//MinMax(0f, 500f)
    }

    private void HomingEx()
    {
        Type type = typeof(WeaponProperties.LevelWeaponHoming.Ex);
        AccessTools.Field(type, "damage").SetValue(null, 4f);//7f
        AccessTools.Field(type, "spread").SetValue(null, 36f);//90f
        AccessTools.Field(type, "bulletCount").SetValue(null, 10);//4
        AccessTools.Field(type, "swirlDistance").SetValue(null, 250);//100
    }

    private void WideshotBasic()
    {
        Type type = typeof(WeaponProperties.LevelWeaponWideShot.Basic);
        AccessTools.Field(type, "angleRange").SetValue(null, new MinMax(135f, 5f));////MinMax(50f, 8f)
        AccessTools.Field(type, "closingAngleSpeed").SetValue(null, 1f);//1.1f
        AccessTools.Field(type, "openingAngleSpeed").SetValue(null, 20f);//1.8f
    }

    private void ChargeBasic()
    {
        Type type = typeof(WeaponProperties.LevelWeaponCharge.Basic);
        AccessTools.Field(type, "damageStateThree").SetValue(null, 40f);//46f
    }

    private void PlaneBombEx()
    {
        Type type = typeof(WeaponProperties.PlaneWeaponBomb.Ex);
        AccessTools.Field(type, "damage").SetValue(null, 5f);//6f
        AccessTools.Field(type, "rotationSpeed").SetValue(null, new MinMax(250f, 500f));//MinMax(0f, 250f)
        AccessTools.Field(type, "maxHomingTime").SetValue(null, 10f);//2.5f
    }

    private void ParryAttack()
    {
        Type type = typeof(WeaponProperties.CharmParryAttack);
        AccessTools.Field(type, "damage").SetValue(null, 25f);//16f
    }

    private void Curse()
    {
        Type type = typeof(WeaponProperties.CharmCurse);
        AccessTools.Field(type, "levelThreshold").SetValue(null, new int[]
        {
            0, 0, 0, 0, 0
            //0, 4, 8, 12, 16
        });
    }

    private void SuperGhostCuphead()
    {
        Type type = typeof(WeaponProperties.LevelSuperGhost);
        AccessTools.Field(type, "initialSpeed").SetValue(null, 2137f);//700f
        AccessTools.Field(type, "maxSpeed").SetValue(null, 50f);//1250f
        AccessTools.Field(type, "initialSpeedTime").SetValue(null, 0.5f);//1.8f
        AccessTools.Field(type, "accelerationTime").SetValue(null, 2f);//1f
        AccessTools.Field(type, "damage").SetValue(null, 69f);//5.1f
        AccessTools.Field(type, "damageRate").SetValue(null, 2.2f);//0.22f
    }

    private void SuperInvincibilityCuphead()
    {
        Type type = typeof(WeaponProperties.LevelSuperInvincibility);
        AccessTools.Field(type, "durationInvincible").SetValue(null, 3f);//4.85f
        AccessTools.Field(type, "durationFX").SetValue(null, 2.7f);//4.55f
    }

    private void SuperBeamCuphead()
    {
        Type type = typeof(WeaponProperties.LevelSuperBeam);
        AccessTools.Field(type, "time").SetValue(null, 0.1f);//1.25f
        AccessTools.Field(type, "damage").SetValue(null, 75f);//14.5f ; 87f
    }

    private void PlaneSuperBomb()
    {
        Type type = typeof(WeaponProperties.PlaneSuperBomb);
        AccessTools.Field(type, "damage").SetValue(null, 152f);//38
        AccessTools.Field(type, "damageRate").SetValue(null, 2137f);//0.25
    }

    private void CharmCurse()
    {
        Type type = typeof(WeaponProperties.CharmCurse);
        int[] availableWeaponIDs = new int[]
        {
            1456773641,
            1456773649,
            1460621839,
            1466518900,
            1466416941,
            1467024095,
            1487081743,
            1568276855,
            1614768724,
            1614768814,//striker
            1614768815,//skytickler
            1614768816,//peeshooter
            1614768817//mangetsunami
        };
        AccessTools.Field(type, "availableWeaponIDs").SetValue(null, availableWeaponIDs);
    }

    private IEnumerator angle_cr(On.WeaponWideShot.orig_angle_cr orig, WeaponWideShot self)
    {
        float openTimeMax = WeaponProperties.LevelWeaponWideShot.Basic.openingAngleSpeed;
        float closeTimeMax = WeaponProperties.LevelWeaponWideShot.Basic.closingAngleSpeed;
        //float t = 0f;
        float val = 0f;
        bool playerLocked = false;
        for (; ; )
        {
            if (playerLocked)
            {
                if (val < 1f)
                {
                    //val = t / closeTimeMax;
                    //t += CupheadTime.Delta;
                    val += CupheadTime.Delta / closeTimeMax;//new
                }
                else
                {
                    val = 1f;
                    //t = 1f;
                }
            }
            else if (val > 0f)
            {
                //val = t / openTimeMax;
                //t -= CupheadTime.Delta;
                val -= CupheadTime.Delta / openTimeMax;//new
            }
            else
            {
                val = 0f;
                //t = 0f;
            }
            playerLocked = self.player.input.actions.GetButton(6);
            MinMax angleRange = WeaponProperties.LevelWeaponWideShot.Basic.angleRange;
            self.maxAngle = angleRange.GetFloatAt(val);
            yield return null;
        }
    }

    protected AbstractProjectile fireEx(On.WeaponHoming.orig_fireEx orig, WeaponHoming self)
    {
        while(self.swirlingProjectiles.Count > 0)
        {
            if (self.swirlingProjectiles[0] != null)
            {
                GameObject.Destroy(self.swirlingProjectiles[0].gameObject);
            }
            self.swirlingProjectiles.RemoveAt(0);
        }
        return orig(self);
    }

    public void FindTarget(On.WeaponHomingProjectile.orig_FindTarget orig, WeaponHomingProjectile self)
    {
        self.target = StatsManager.homingWeaponCursorCollider;
    }

    public void FindTarget(On.PlaneWeaponBombExProjectile.orig_FindTarget orig, PlaneWeaponBombExProjectile self)
    {
        self.target = StatsManager.homingWeaponCursorCollider;
    }

    public void Start(On.WeaponCrackshotExProjectile.orig_Start orig, WeaponCrackshotExProjectile self)
    {
        orig(self);
        self.CollisionDeath.Enemies = false;
    }

    public void LaunchAtTarget(On.WeaponCrackshotExProjectile.orig_LaunchAtTarget orig, WeaponCrackshotExProjectile self)
    {
        self.Die();
    }

    public void StartSuper(On.PlayerSuperGhost.orig_StartSuper orig, PlayerSuperGhost self)
    {
        orig(self);
        BoxCollider2D[] colliders = self.GetComponentsInChildren<BoxCollider2D>();
        foreach(BoxCollider2D collider in colliders)
        {
            if (collider)
            {
                collider.size *= 1.5f;
            }
        }
    }

    public void StartSuper(On.PlayerSuperInvincible.orig_StartSuper orig, PlayerSuperInvincible self)
    {
        orig(self);
        Level.ScoringData.superMeterUsed -= 2;
    }

    public void FixedUpdate(On.WeaponCharge.orig_FixedUpdate orig, WeaponCharge self)
    {
        if (self.chargeEffect == null)
        {
            self.fullyCharged = false;
            self.damage = WeaponProperties.LevelWeaponCharge.Basic.baseDamage;
        }
        else
        {
            self.chargeEffect.transform.position = self.player.weaponManager.GetBulletPosition();
            self.timeCharged += CupheadTime.FixedDelta /  SuperSandevistan.multiplier;//new
            //self.timeCharged += CupheadTime.FixedDelta;
            if (self.timeCharged > WeaponProperties.LevelWeaponCharge.Basic.timeStateThree)
            {
                self.fullyCharged = true;
                if (self.AllowChargeSound)
                {
                    AudioManager.Play("player_weapon_charge_ready");
                    self.AllowChargeSound = false;
                }
                self.chargeEffect.animator.SetTrigger("IsFull");
                self.damage = WeaponProperties.LevelWeaponCharge.Basic.damageStateThree;
            }
            else
            {
                self.fullyCharged = false;
                self.damage = WeaponProperties.LevelWeaponCharge.Basic.baseDamage;
            }
        }
    }

    public IEnumerator super_cr(On.PlaneSuperBomb.orig_super_cr orig, PlaneSuperBomb self)
    {
        float t = 0f;
        self.damageDealer = new DamageDealer(WeaponProperties.PlaneSuperBomb.damage, WeaponProperties.PlaneSuperBomb.damageRate, DamageDealer.DamageSource.Super, false, true, true);
        self.damageDealer.DamageMultiplier *= Other.GetDamageMultiplier(true, self.player.id);//new
        //self.damageDealer.DamageMultiplier *= PlayerManager.DamageMultiplier;
        self.damageDealer.PlayerId = self.player.id;
        MeterScoreTracker tracker = new MeterScoreTracker(MeterScoreTracker.Type.Super);
        tracker.Add(self.damageDealer);
        while (t < WeaponProperties.PlaneSuperBomb.countdownTime && !self.earlyExplosion)
        {
            t += CupheadTime.Delta;
            yield return null;
        }
        self.Fire();
        if (self.player != null)
        {
            self.player.PauseAll();
            self.player.SetSpriteVisible(false);
            self.transform.position = self.player.transform.position;
        }
        else
        {
            UnityEngine.Object.Destroy(self.gameObject);
        }
        self.animator.SetTrigger("Explode");
        AudioManager.Stop("player_plane_bomb_ticktock_loop");
        AudioManager.Play("player_plane_bomb_explosion");
        yield break;
    }

    public IEnumerator super_cr(On.PlaneSuperChalice.orig_super_cr orig, PlaneSuperChalice self)
    {
        self.player.damageReceiver.Vulnerable();
        self.respawnPos = self.transform.position;
        self.state = PlanePlayerWeaponManager.States.Super.Countdown;
        self.damageDealer = new DamageDealer(WeaponProperties.PlaneSuperChaliceSuperBomb.damage, WeaponProperties.PlaneSuperChaliceSuperBomb.damageRate, DamageDealer.DamageSource.Super, false, true, true);
        self.damageDealer.DamageMultiplier *= Other.GetDamageMultiplier(true, self.player.id);//new
        //self.damageDealer.DamageMultiplier *= PlayerManager.DamageMultiplier;
        self.damageDealer.PlayerId = self.player.id;
        MeterScoreTracker tracker = new MeterScoreTracker(MeterScoreTracker.Type.Super);
        tracker.Add(self.damageDealer);
        self.curAngle = MathUtils.DirectionToAngle(Vector3.right);
        self.curSpeed = 0f;
        while (!self.exploded)
        {
            if (self.player != null)
            {
                self.player.transform.position = self.transform.position;
            }
            else
            {
                UnityEngine.Object.Destroy(self.gameObject);
            }
            yield return null;
        }
        self.respawnPos = self.transform.position;
        self.Fire();
        if (self.player != null)
        {
            self.player.PauseAll();
        }
        else
        {
            UnityEngine.Object.Destroy(self.gameObject);
        }
        self.animator.SetTrigger("Explode");
        AudioManager.Play("player_plane_bomb_explosion");
        AudioManager.Stop("player_plane_bomb_ticktock_loop");
        yield break;
    }

    public void Fire(On.PlayerSuperBeam.orig_Fire orig, PlayerSuperBeam self)
    {
        Fire(self);
        AudioManager.Play("player_superbeam_firing_loop");
        AudioManager.Play("player_superbeam_milk_explosion");
        self.damageDealer = new DamageDealer(WeaponProperties.LevelSuperBeam.damage, WeaponProperties.LevelSuperBeam.damageRate, DamageDealer.DamageSource.Super, false, true, true);
        self.damageDealer.OnDealDamage += self.OnDealDamage;
        self.damageDealer.DamageMultiplier *= Other.GetDamageMultiplier(true, self.player.id);//new
        //self.damageDealer.DamageMultiplier *= PlayerManager.DamageMultiplier;
        self.damageDealer.PlayerId = self.player.id;
        MeterScoreTracker meterScoreTracker = new MeterScoreTracker(MeterScoreTracker.Type.Super);
        meterScoreTracker.Add(self.damageDealer);
    }

    public void Fire(On.PlayerSuperChaliceVerticalBeam.orig_Fire orig, PlayerSuperChaliceVerticalBeam self)
    {
        AudioManager.Play("player_super_chalice_superbeam");
        Fire(self);
        self.damageDealer = new DamageDealer(WeaponProperties.LevelSuperChaliceVertBeam.damage, WeaponProperties.LevelSuperChaliceVertBeam.damageRate, DamageDealer.DamageSource.Super, false, true, true);
        self.damageDealer.OnDealDamage += self.OnDealDamage;
        self.damageDealer.DamageMultiplier *= Other.GetDamageMultiplier(true, self.player.id);//new
        //self.damageDealer.DamageMultiplier *= PlayerManager.DamageMultiplier;
        self.damageDealer.PlayerId = self.player.id;
        MeterScoreTracker meterScoreTracker = new MeterScoreTracker(MeterScoreTracker.Type.Super);
        meterScoreTracker.Add(self.damageDealer);
    }

    public IEnumerator super_cr(On.PlayerSuperGhost.orig_super_cr orig, PlayerSuperGhost self)
    {
        yield return self.animator.WaitForAnimationToEnd(self, "Start", false, true);
        AudioManager.Play("player_super_beam");
        self.state = PlayerSuperGhost.State.Spinning;
        self.damageDealer = new DamageDealer(WeaponProperties.LevelSuperGhost.damage, WeaponProperties.LevelSuperGhost.damageRate, DamageDealer.DamageSource.Super, false, true, true);
        self.damageDealer.DamageMultiplier *= Other.GetDamageMultiplier(true, self.player.id);//new
        //self.damageDealer.DamageMultiplier *= PlayerManager.DamageMultiplier;
        self.damageDealer.PlayerId = self.player.id;
        MeterScoreTracker tracker = new MeterScoreTracker(MeterScoreTracker.Type.Super);
        tracker.Add(self.damageDealer);
        self.lookDir = self.player.motor.TrueLookDirection;
        yield return CupheadTime.WaitForSeconds(self, WeaponProperties.LevelSuperGhost.initialSpeedTime);
        self.animator.SetTrigger("Continue");
        float t = 0f;
        float duration = (!self.createHeart) ? WeaponProperties.LevelSuperGhost.noHeartMaxSpeedTime : WeaponProperties.LevelSuperGhost.maxSpeedTime;
        while (t < duration && !self.interrupted)
        {
            t += CupheadTime.Delta;
            yield return null;
        }
        self.state = PlayerSuperGhost.State.Dying;
        self.animator.SetTrigger("Death");
        yield break;
    }

    private void Fire(AbstractPlayerSuper self)
    {
        PauseManager.Unpause();
        AudioManager.HandleSnapshot(AudioManager.Snapshots.Super.ToString(), 0.2f);
        if (self.player == null)
        {
            self.Interrupt();
        }
        else
        {
            self.player.PauseAll();
        }
        AnimationHelper component = self.GetComponent<AnimationHelper>();
        component.IgnoreGlobal = false;
    }

    public float DealDamage(On.DamageDealer.orig_DealDamage orig, DamageDealer self, object hit2)
    {
        if (hit2 is GameObject hit)
        {
            float originalDamage = orig(self, hit);
            if (originalDamage != 0f && self.damageSource == DamageDealer.DamageSource.Ex)
            {
                CharmBalance.TryUpdate(self.damage * self.damageMultiplier);
            }
            return originalDamage;
        }
        return 0f;
    }
}
