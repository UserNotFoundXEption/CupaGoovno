using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CupaGoovno;

public class RobotBodyPart
{
    public void Init()
    {
        On.RobotLevelRobotBodyPart.primaryAttack_cr += primaryAttack_cr;
    }

    protected virtual IEnumerator primaryAttack_cr(On.RobotLevelRobotBodyPart.orig_primaryAttack_cr orig, RobotLevelRobotBodyPart self)
    {
        while (self.current == RobotLevelRobotBodyPart.state.primary)
        {
            //yield return CupheadTime.WaitForSeconds(self, self.primaryAttackDelay);
            self.isAttacking = true;
            self.OnPrimaryAttack();
            while (self.isAttacking)
            {
                yield return null;
            }
        }
        yield return null;
        yield break;
    }
}
