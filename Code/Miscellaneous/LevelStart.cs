using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CupaGoovno;

public class LevelStart
{
    public void Init()
    {
        On.Level.Start += Start;
    }

    protected void Start(On.Level.orig_Start orig, Level self)
    {
        orig(self);
        if(self is RobotLevel robotSelf)
        {
            RobotStart(robotSelf);
        }
    }

    private void RobotStart(RobotLevel self)
    {
        self.properties.OnBossDamaged -= self.timeline.DealDamage;
        self.robot.LevelInit(self.properties);
    }
}
