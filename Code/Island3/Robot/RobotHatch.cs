namespace CupaGoovno;

public class RobotHatch
{
    public void Init()
    {
        On.RobotLevelRobotHatch.InitBodyPart += InitBodyPart;
    }

    public void InitBodyPart(On.RobotLevelRobotHatch.orig_InitBodyPart orig, RobotLevelRobotHatch self, RobotLevelRobot parent, LevelProperties.Robot properties, int primaryHP = 0, int secondaryHP = 1, float attackDelayMinus = 0f)
    {
        orig(self, parent, properties, primaryHP, secondaryHP, attackDelayMinus);
        YoMamaFat.robotShotBot = self.primary;//new
    }
}
