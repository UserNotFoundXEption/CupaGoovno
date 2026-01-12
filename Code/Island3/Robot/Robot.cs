namespace CupaGoovno;

public class Robot
{
    public void Init()
    {
        new RobotHead().Init();
        new RobotBodyPart().Init();
        new RobotHeliHead().Init();
        new RobotShotBot().Init();
        new RobotOrb().Init();
        new RobotHatch().Init();
        On.RobotLevel.PartialInit += PartialInit;
        On.RobotLevel.OnDestroy += OnDestroy;
    }

    protected void PartialInit(On.RobotLevel.orig_PartialInit orig, RobotLevel self)
    {
        YoMamaFat.robotGemProjectile = self.heliHead.gem.bulletPrefab;//new
        orig(self);
    }

    protected void OnDestroy(On.RobotLevel.orig_OnDestroy orig, RobotLevel self)
    {
        orig(self);
        PlaneWeaponManager.InvaderModeOff();
    }
}
