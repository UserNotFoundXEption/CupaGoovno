using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.SceneManagement;

namespace CupaGoovno;

public class ParryOrb
{
    public void Init()
    {
        On.TutorialLevelParryNext.SetNextParry += SetNextParry;
    }

    private void SetNextParry(On.TutorialLevelParryNext.orig_SetNextParry orig, TutorialLevelParryNext self)
    {
        Levels level = Level.Current.CurrentLevel;
        if (level != Levels.Slime && level != Levels.SallyStagePlay && level != Levels.Devil)//new start
        {
            if (self.nextSphere != null)
            {//new end
                self.nextSphere.parrySwitch.enabled = true;
                self.nextSphere.spriteRenderer.sprite = self.nextSphere.parrySprite;
                self.nextSphere.spriteRenderer.sharedMaterial = self.parryMaterial;
            }//new
            self.parrySwitch.enabled = false;
            self.spriteRenderer.sprite = self.normalSprite;
            self.spriteRenderer.sharedMaterial = self.normalMaterial;
            if (self.lastPlayerController != null)
            {
                self.lastPlayerController.stats.OnParry(1f, true);
                self.lastPlayerController = null;
            }
        }//new
    }
}
