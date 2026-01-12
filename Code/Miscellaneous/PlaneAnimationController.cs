namespace CupaGoovno;

public class PlaneAnimationController
{
    public void Init()
    {
        On.PlanePlayerAnimationController.HandleShrunk += HandleShrunk;
        On.PlanePlayerAnimationController.OnParryStart += OnParryStart;
    }

    private void HandleShrunk(On.PlanePlayerAnimationController.orig_HandleShrunk orig, PlanePlayerAnimationController self)
    {
        if (self.ShrinkState == PlanePlayerAnimationController.ShrinkStates.Cooldown)
        {
            if (self.shrinkCooldownTimeLeft <= 0f)
            {
                self.ShrinkState = PlanePlayerAnimationController.ShrinkStates.Ready;
            }
            self.shrinkCooldownTimeLeft -= CupheadTime.FixedDelta;
        }
        if (self.player.Parrying || self.player.WeaponBusy || self.player.stats.StoneTime > 0f || self.ShrinkState == PlanePlayerAnimationController.ShrinkStates.Cooldown)
        {
            return;
        }
        if (self.ShrinkState == PlanePlayerAnimationController.ShrinkStates.Ready && (self.player.input.actions.GetButtonDown(7) || self.player.input.actions.GetButtonDown(6)))
        {
            self.animator.SetLayerWeight(1, 1f);
            self.animator.Play("Shrink_In", 0);
            self.Shrinking = true;
            self.ShrinkState = PlanePlayerAnimationController.ShrinkStates.Shrunk;
            /* if (self.OnShrinkEvent != null)
             {
                 self.OnShrinkEvent();
             }
         if (self.player.stats.Loadout.charm == Charm.charm_smoke_dash || self.player.stats.CurseSmokeDash)
         {
             self.smokeDashEffect.Create(self.player.center);
         }*/
            AudioManager.Play("player_plane_shrink");
        }
        if (self.ShrinkState == PlanePlayerAnimationController.ShrinkStates.Shrunk && !self.player.input.actions.GetButton(7) && !self.player.input.actions.GetButton(6))
        {
            self.Shrinking = false;
            self.animator.SetLayerWeight(1, 0f);
            self.animator.Play("Shrink_Out", 0);
            self.ShrinkState = PlanePlayerAnimationController.ShrinkStates.Cooldown;
            self.shrinkCooldownTimeLeft = 0.23300001f;
            AudioManager.Play("player_plane_expand");
        }
    }

    private void OnParryStart(On.PlanePlayerAnimationController.orig_OnParryStart orig, PlanePlayerAnimationController self)
    {
        if (self.isStoned)
        {
            self.Breakout();
        }
        self.animator.SetBool("ParrySuccess", false);
        self.animator.SetBool("ParryPlusCharm", self.player.stats.Loadout.charm == Charm.charm_parry_plus);
        bool whetstone = self.player.stats.Loadout.charm == Charm.charm_parry_attack;//new start
        bool relic = self.player.stats.CurseWhetsone;
        bool casualMode = self.player.stats.Loadout.charm == CustomCharms.casualMode;
        if (whetstone || relic || casualMode)//new end
        //if (self.player.stats.Loadout.charm == Charm.charm_parry_attack || self.player.stats.CurseWhetsone)
        {
            self.animator.Play("ParryAttack");
        }
        else
        {
            self.animator.Play("Parry");
        }
    }
}
