using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CupaGoovno;

public class MouseCat
{
    public void Init()
    {
        On.MouseLevelCat.manageGhostMice_cr += manageGhostMice_cr;
        On.MouseLevelCat.spawnFallingObjects_cr += spawnFallingObjects_cr;
    }

    private IEnumerator manageGhostMice_cr(On.MouseLevelCat.orig_manageGhostMice_cr orig, MouseLevelCat self)
    {
        self.alreadyManagingGhostMice = true;
        MouseLevelGhostMouse[] ghostMice = (!self.properties.CurrentState.ghostMouse.fourMice) ? self.twoGhostMice : self.fourGhostMice;
        int shotsTillPinkAttack = self.properties.CurrentState.ghostMouse.pinkBallRange.RandomInt();
        bool anyMiceSpawned = true;
        while (anyMiceSpawned)
        {
            ghostMice.Shuffle<MouseLevelGhostMouse>();
            anyMiceSpawned = false;
            foreach (MouseLevelGhostMouse mouse in ghostMice)
            {
                if (mouse.state != MouseLevelGhostMouse.State.Unspawned && mouse.state != MouseLevelGhostMouse.State.Dying)
                {
                    anyMiceSpawned = true;
                }
                //if (mouse.state == MouseLevelGhostMouse.State.Idle)
                if (mouse.state == MouseLevelGhostMouse.State.Idle && mouse.transform.position.x < 600f && mouse.transform.position.x > -600f) //new
                {
                    shotsTillPinkAttack--;
                    bool pinkAttack = false;
                    if (shotsTillPinkAttack == 0)
                    {
                        pinkAttack = true;
                        shotsTillPinkAttack = self.properties.CurrentState.ghostMouse.pinkBallRange.RandomInt();
                    }
                    mouse.Attack(pinkAttack);
                    while (mouse.state == MouseLevelGhostMouse.State.Attack)
                    {
                        yield return null;
                    }
                    yield return CupheadTime.WaitForSeconds(self, self.properties.CurrentState.ghostMouse.attackDelayRange.RandomFloat());
                }
            }
            yield return null;
        }
        self.alreadyManagingGhostMice = false;
        yield break;
    }

    private IEnumerator spawnFallingObjects_cr(On.MouseLevelCat.orig_spawnFallingObjects_cr orig, MouseLevelCat self, bool left)
    {
        yield break;
    }
}
