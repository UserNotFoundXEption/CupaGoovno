using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class AbstractWeapon
{
    public void Init()
    {
        On.AbstractLevelWeapon.fireBasic += fireBasic;
        On.AbstractLevelWeapon.StartCoroutines += StartCoroutines;
        On.AbstractLevelWeapon.chargeFireWeapon_cr += chargeFireWeapon_cr;
        On.AbstractLevelWeapon.fireWeapon_cr += fireWeapon_cr;
    }

    public AbstractProjectile fireBasic(On.AbstractLevelWeapon.orig_fireBasic orig, AbstractLevelWeapon self)
    {
        AbstractProjectile proj = orig(self);
        if (SuperBerserker.active)
        {
            if (SuperBerserker.fired)
            {
                SuperBerserker.fired = false;
            }
            else
            {
                Weapon primaryWeapon = self.player.stats.Loadout.primaryWeapon;
                Weapon secondaryWeapon = self.player.stats.Loadout.secondaryWeapon;
                Weapon currentWeapon = self.player.weaponManager.currentWeapon;
                Weapon weapon;
                if (currentWeapon == primaryWeapon)
                {
                    weapon = secondaryWeapon;
                }
                else
                {
                    weapon = primaryWeapon;
                }
                if (weapon != Weapon.None)
                {
                    AbstractLevelWeapon weapon2 = self.weaponManager.weaponPrefabs.GetWeapon(weapon);
                    SuperBerserker.fired = true;
                    weapon2.BeginBasic();
                    weapon2.fireBasic();
                }
            }

        }
        return proj;
    }

    public void StartCoroutines(On.AbstractLevelWeapon.orig_StartCoroutines orig, AbstractLevelWeapon self)
    {
        orig(self);
        self.StartCoroutine(fixCustomProjectiles_cr());
    }

    private IEnumerator fixCustomProjectiles_cr()
    {
        for (; ; )
        {
            while (CupheadTime.GlobalSpeed <= 0f)
            {
                yield return null;
            }

            FixCustomProjectiles<BasicProjectile>(CustomWeapons.projectilesBasic);
            FixCustomProjectiles<WeaponUpshotProjectile>(CustomWeapons.projectilesUpshot);
            yield return null;
        }
    }

    private void FixCustomProjectiles<T>(List<T> list) where T : AbstractProjectile
    {
        List<int> projectilesToRemove = [];

        for (int i = 0; i < list.Count; i++)
        {
            var proj = list[i];
            if (proj == null)
            {
                projectilesToRemove.Add(i);
            }
            else
            {
                T component = proj.GetComponent<T>();
                if (component == null)
                {
                    Plugin.Log("FixCustomProjectiles projectile is null");
                }
                else
                {
                    if (!component.enabled)
                    {
                        component.enabled = true;
                    }
                }
            }
        }

        int removedCount = 0;
        foreach (int index in projectilesToRemove)
        {
            list.RemoveAt(index - removedCount);
            removedCount++;
        }
    }

    public virtual IEnumerator chargeFireWeapon_cr(On.AbstractLevelWeapon.orig_chargeFireWeapon_cr orig, AbstractLevelWeapon self, AbstractLevelWeapon.Mode mode)
    {
        WaitForFixedUpdate waitInstruction = new();
        for (; ; )
        {
            yield return waitInstruction;
            if (mode == AbstractLevelWeapon.Mode.Basic && self.firing.Get(mode) && self.weaponManager.IsShooting)
            {
                self.alreadyHeld = true;
            }
            else if (mode == AbstractLevelWeapon.Mode.Basic && self.alreadyHeld)
            {
                self.alreadyReleased = true;
            }
            if (mode == AbstractLevelWeapon.Mode.Basic && self.t < self.rapidFireRate)
            {
                if (self.weaponManager.CurrentWeapon == self)
                {
                    self.t += CupheadTime.FixedDelta / SuperSandevistan.multiplier;//new
                    //self.t += CupheadTime.FixedDelta;
                    self.charging = false;
                }
            }
            else if (self.firing.Get(mode) && self.weaponManager.IsShooting && !self.player.motor.Dashing && !self.player.motor.IsHit && !self.player.motor.IsUsingSuperOrEx && !self.alreadyReleased)
            {
                if (!self.charging)
                {
                    self.StartCharging();
                }
                self.charging = true;
            }
            else if ((self.charging || self.alreadyReleased) && !self.player.motor.Dashing)//new
            //else if (self.charging || self.alreadyReleased)
            {
                self.charging = false;
                self.alreadyReleased = false;
                self.alreadyHeld = false;
                self.weaponManager.TriggerWeaponFire();
                self.getFiringMethod(mode)();
                if (!self.weaponManager.IsShooting)
                {
                    self.firing.Set(mode, false);
                }
                self.t = 0f;
            }
            else if (!self.charging)
            {
                self.StopCharging();
            }
        }
    }

    public IEnumerator fireWeapon_cr(On.AbstractLevelWeapon.orig_fireWeapon_cr orig, AbstractLevelWeapon self, AbstractLevelWeapon.Mode mode)
    {
        WaitForFixedUpdate waitInstruction = new();
        for (; ; )
        {
            yield return waitInstruction;
            if (!self.player.motor.Dashing)
            {
                if (mode == AbstractLevelWeapon.Mode.Basic && self.t < self.rapidFireRate)
                {
                    if (self.weaponManager.CurrentWeapon == self)
                    {
                        //self.t += CupheadTime.FixedDelta;
                        self.t += CupheadTime.FixedDelta / SuperSandevistan.multiplier;//new
                    }
                }
                else if (self.firing.Get(mode) && self.weaponManager.IsShooting)
                {
                    self.weaponManager.TriggerWeaponFire();
                    self.getFiringMethod(mode)();
                    if (mode == AbstractLevelWeapon.Mode.Ex || !self.rapidFire)
                    {
                        self.firing.Set(mode, false);
                        self.weaponManager.IsShooting = false;
                    }
                    self.t = 0f;
                }
            }
        }
    }
}
