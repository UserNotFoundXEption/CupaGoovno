using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CupaGoovno;

public class LevelWeaponManager
{
    public void Init()
    {
        On.LevelPlayerWeaponManager.HandleWeaponFiring += HandleWeaponFiring;
        On.LevelPlayerWeaponManager.EndEx += EndEx;
        On.LevelPlayerWeaponManager.HandleWeaponSwitch += HandleWeaponSwitch;
        On.LevelPlayerWeaponManager.OnDash += OnDash;
    }

    private void HandleWeaponFiring(On.LevelPlayerWeaponManager.orig_HandleWeaponFiring orig, LevelPlayerWeaponManager self)
    {
        if (self.player.motor.Dashing || self.player.motor.IsHit)
        {
            return;
        }
        if (self.player.input.actions.GetButtonDown(4) || self.player.motor.HasBufferedInput(LevelPlayerMotor.BufferedInput.Super) || (self.player.stats.Loadout.charm == Charm.charm_EX && self.player.input.actions.GetButton(3) && !self.ex.firing))
        {
            self.player.motor.ClearBufferedInput();
            Super super = PlayerData.Data.Loadouts.GetPlayerLoadout(self.player.id).super;
            if (self.player.stats.SuperMeter >= self.player.stats.SuperMeterMax && super != Super.None && !self.player.stats.ChaliceShieldOn && self.allowSuper && self.player.stats.Loadout.charm != Charm.charm_EX)
            {
                //self.StartSuper();
                if (!CustomSupers.IsCustomSuper(super, self))//new start
                {
                    self.StartSuper();
                }
                else
                {
                    Level.ScoringData.superMeterUsed += 5;
                }//new end
                return;
            }
            //if (self.player.stats.CanUseEx && self.ex.Able)
            Charm charm = PlayerManager.GetPlayer(self.player.id).stats.Loadout.charm;//new start
            bool isPractiseMode = charm == Charm.charm_smoke_dash;
            if ((self.player.stats.CanUseEx || isPractiseMode)
                && self.ex.Able
                && !SuperSandevistan.Active)//new end
            {
                self.StartEx();
                if (charm == CustomCharms.milk)//new start
                {
                    CharmMilk.Fire();
                }//new end
                return;
            }
        }
        if (self.ex.firing || self.player.stats.Loadout.charm == Charm.charm_EX)
        {
            return;
        }
        if (self.basic.firing != self.player.input.actions.GetButton(3))
        {
            if (self.player.input.actions.GetButton(3))
            {
                if (PlayerData.Data.Loadouts.GetPlayerLoadout(self.player.id).charm == Charm.charm_curse && self.player.stats.CurseCharmLevel > -1)
                {
                    int[] availableWeaponIDs = WeaponProperties.CharmCurse.availableWeaponIDs;
                    int num;
                    for (num = (int)self.currentWeapon; num == (int)self.currentWeapon; num = availableWeaponIDs[UnityEngine.Random.Range(0, availableWeaponIDs.Length)])
                    {
                    }
                    self.SwitchWeapon((Weapon)num);
                }
                else
                {
                    self.StartBasic();
                }
            }
            else
            {
                self.EndBasic();
            }
        }
        self.basic.firing = self.player.input.actions.GetButton(3);
    }

    private void EndEx(On.LevelPlayerWeaponManager.orig_EndEx orig, LevelPlayerWeaponManager self)
    {
        orig(self);
        if (self.player.stats.Loadout.charm == Charm.charm_parry_plus && !self.player.motor.Grounded)
        {
            self.player.motor.ForceParry();
        }
    }

    public void HandleWeaponSwitch(On.LevelPlayerWeaponManager.orig_HandleWeaponSwitch orig, LevelPlayerWeaponManager self)
    {
        orig(self);
        if (self.player.stats.Loadout.charm == CustomCharms.balance)
        {
            StatsManager.UpdateDamageCounter(self.player.id);
        }
    }

    public void OnDash(On.LevelPlayerWeaponManager.orig_OnDash orig, LevelPlayerWeaponManager self)
    {
        Weapon weapon = self.currentWeapon;
        if (weapon != Weapon.level_weapon_charge && weapon != CustomWeapons.mangetsunami)
        {
            orig(self);
        }
    }
}
