namespace CupaGoovno;

public class PlaneWeaponManager
{
    public void Init()
    {
        On.PlanePlayerWeaponManager.Start += Start;
        On.PlanePlayerWeaponManager.CheckEx += CheckEx;
        On.PlanePlayerWeaponManager.StartEx += StartEx;
        On.PlanePlayerWeaponManager.HandleWeaponSwitch += HandleWeaponSwitch;
    }

    private void Start(On.PlanePlayerWeaponManager.orig_Start orig, PlanePlayerWeaponManager self)
    {
        orig(self);
        YoMamaFat.planeWeaponManager = self;
    }

    private void CheckEx(On.PlanePlayerWeaponManager.orig_CheckEx orig, PlanePlayerWeaponManager self)
    {
        if (!invaderMode)
        {
            orig(self);
        }
    }

    public void StartEx(On.PlanePlayerWeaponManager.orig_StartEx orig, PlanePlayerWeaponManager self)
    {
        orig(self);
        if(self.player.stats.Loadout.charm == CustomCharms.milk)
        {
            CharmMilk.Fire();
        }
    }

    private void HandleWeaponSwitch(On.PlanePlayerWeaponManager.orig_HandleWeaponSwitch orig, PlanePlayerWeaponManager self)
    {
        if (!invaderMode)
        {
            orig(self);
            if(self.player.stats.Loadout.charm == CustomCharms.balance)
            {
                StatsManager.UpdateDamageCounter(self.player.id);
            }
        }
    }

    public static void InvaderModeOn(PlanePlayerWeaponManager self)//new
    {
        if (self.player.stats.isChalice)
        {
            self.SwitchToWeapon(Weapon.plane_chalice_weapon_3way);
        }
        else
        {
            self.SwitchToWeapon(Weapon.plane_weapon_peashot);
        }
        invaderMode = true;
    }

    public static void InvaderModeOff()//new
    {
        invaderMode = false;
    }

    private static bool invaderMode = false;
}
