using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CupaGoovno;

public class DevilOribitngProjectile
{
    public void Init()
    {
        On.DevilLevelPitchforkOrbitingProjectile.Update += Update;
    }

    public void Update(On.DevilLevelPitchforkOrbitingProjectile.orig_Update orig, DevilLevelPitchforkOrbitingProjectile self)
    {
        AbstractProj.Update(self);
        
        LevelProperties.Devil.States state = Devil.devil.properties.CurrentState.stateName;//new start
        bool isHands = state == LevelProperties.Devil.States.Hands;
        bool isTears = state == LevelProperties.Devil.States.Tears;
        if (self.parent == null && !isHands && !isTears)//new end
        //if (self.parent == null)
        {
            self.Die();
        }
    }
}
