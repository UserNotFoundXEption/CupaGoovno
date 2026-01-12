namespace CupaGoovno;

public class PlayerAnimationController
{
    public void Init()
    {
        On.LevelPlayerAnimationController.Update += Update;
        On.LevelPlayerAnimationController.OnDashStart += OnDashStart;
        On.LevelPlayerAnimationController.OnDashEnd += OnDashEnd;
        On.LevelPlayerAnimationController.OnParryStart += OnParryStart;
        On.LevelPlayerAnimationController.OnParrySuccess += OnParrySuccess;
    }

    private void Update(On.LevelPlayerAnimationController.orig_Update orig, LevelPlayerAnimationController self)
    {
        if (self.player.IsDead || !self.player.levelStarted)
        {
            return;
        }
        if (self.curseCharmLevel > -1 && !self.showCurseFX && !Level.IsChessBoss)
        {
            self.InitializeCurseFX();
            self.showCurseFX = true;
        }
        if (self.player.stats.isChalice && self.chaliceActivated)
        {
            self.ChaliceAimSpriteHandling();
            self.ChaliceJumpHandling();
            self.ChaliceJumpShootHandling();
            if (!self.player.motor.Dashing)
            {
                self.animator.SetLayerWeight(3, 1f);
                if (self.chaliceInvincibleSparklesCoroutine != null)
                {
                    self.StopCoroutine(self.chaliceInvincibleSparklesCoroutine);
                    self.chaliceInvincibleSparklesCoroutine = null;
                }
            }
        }
        if (self.curseCharmLevel > -1)
        {
            self.HandleCurseFX();
        }
        if (!self.hitAnimation && self.player.motor.LookDirection.x != 0 && (int)self.lastTrueLookDir.x != (int)self.player.motor.TrueLookDirection.x)
        {
            self.SetBool(LevelPlayerAnimationController.Booleans.Turning, true);
        }
        else
        {
            self.SetBool(LevelPlayerAnimationController.Booleans.Turning, false);
        }
        self.lastTrueLookDir = self.player.motor.TrueLookDirection;
        self.SetBool(LevelPlayerAnimationController.Booleans.Grounded, self.player.motor.Grounded);
        self.SetBool(LevelPlayerAnimationController.Booleans.Locked, self.player.motor.Locked);
        if (self.player.motor.Locked)
        {
            self.SetInt(LevelPlayerAnimationController.Integers.MoveX, 0);
        }
        else
        {
            self.SetInt(LevelPlayerAnimationController.Integers.MoveX, self.player.motor.LookDirection.x);
        }
        if (self.player.motor.Ducking || self.player.motor.IsUsingSuperOrEx)
        {
            self.SetInt(LevelPlayerAnimationController.Integers.MoveY, 0);
            self.SetBool(LevelPlayerAnimationController.Booleans.ChaliceOffIdle, true);
        }
        else
        {
            self.SetInt(LevelPlayerAnimationController.Integers.MoveY, self.player.motor.MoveDirection.y);
            self.SetBool(LevelPlayerAnimationController.Booleans.ChaliceOffIdle, false);
        }
        self.SetInt(LevelPlayerAnimationController.Integers.LookX, self.player.motor.LookDirection.x);
        self.SetInt(LevelPlayerAnimationController.Integers.LookY, self.player.motor.LookDirection.y);
        self.SetBool(LevelPlayerAnimationController.Booleans.Shooting, self.player.weaponManager.IsShooting);
        float num = (!self.player.weaponManager.IsShooting && self.timeSinceStoppedShooting >= 0.0833f) ? 0f : 1f;
        if (!self.player.stats.isChalice)
        {
            self.animator.SetLayerWeight(1, num);
            self.animator.SetLayerWeight(2, (self.player.motor.LookDirection.y <= 0) ? 0f : num);
        }
        else
        {
            if (!self.player.motor.Grounded && self.animator.GetBool(LevelPlayerAnimationController.Booleans.ChaliceAirEX))
            {
                num = 0f;
            }
            if (!self.ExitingChaliceSuper())
            {
                self.animator.SetLayerWeight(4, 1f - num);
            }
            else
            {
                self.animator.SetLayerWeight(4, 0f);
            }
            self.animator.SetLayerWeight(5, num);
            self.animator.SetLayerWeight(6, (self.player.motor.LookDirection.y <= 0) ? 0f : num);
            if (self.player.motor.ChaliceDuckDashed && !self.player.motor.Grounded)
            {
                self.chaliceFellFromDuckDash = true;
            }
            if (self.player.motor.Grounded)
            {
                self.chaliceFellFromDuckDash = false;
            }
        }
        if (self.shooting)
        {
            self.timeSinceStoppedShooting = 0f;
        }
        else
        {
            self.timeSinceStoppedShooting += CupheadTime.Delta;
        }
        bool flag = false;
        if (self.fired && ((self.player.motor.Grounded && (self.player.motor.LookDirection.x == 0 || self.player.motor.Locked || self.player.motor.LookDirection.y < 0)) || (self.player.stats.isChalice && !self.player.motor.ChaliceDoubleJumped)))
        {
            self.SetTrigger(LevelPlayerAnimationController.Triggers.OnFire);
            flag = true;
        }
        self.fired = false;
        self.shooting = self.player.weaponManager.IsShooting;
        if (!self.shooting && !flag)
        {
            self.ResetTrigger(LevelPlayerAnimationController.Triggers.OnFire);
        }
        if (self.player.motor.Dashing && self.GetBool(LevelPlayerAnimationController.Booleans.Dashing) != self.player.motor.Dashing)
        {
            if (self.player.stats.isChalice)
            {
                self.animator.SetLayerWeight(3, 0f);
            }
            if (self.player.stats.isChalice && self.player.motor.Ducking)
            {
                self.ChaliceDuckDashHandling();
            }
            else
            {
                self.Play("Dash.Air");/*
				if (self.player.stats.Loadout.charm != Charm.charm_smoke_dash || !self.player.stats.CurseSmokeDash || Level.IsChessBoss || (self.player.stats.isChalice && !self.player.motor.Ducking))
				{
					self.dashEffect.Create(self.transform.position, self.transform.localScale);
				}*/
                if (self.player.stats.isChalice)
                {
                    self.chaliceDashEffectActive = self.chaliceDashEffect.Create(self.transform.position, self.transform.localScale);
                    self.chaliceDashEffectActive.transform.parent = self.transform;
                }
            }
        }
        self.SetBool(LevelPlayerAnimationController.Booleans.Dashing, self.player.motor.Dashing);
        if (!self.player.motor.Dashing)
        {
            if (self.player.motor.LookDirection.x != 0 && !self.ExitingChaliceSuper())
            {
                self.transform.SetScale(new float?(self.player.motor.LookDirection.x), null, null);
            }
        }
        else
        {
            self.transform.SetScale(new float?((float)self.player.motor.DashDirection), null, null);
        }
    }

    private void OnDashStart(On.LevelPlayerAnimationController.orig_OnDashStart orig, LevelPlayerAnimationController self)
    {
        self.hitAnimation = false;
        /*if ((self.player.stats.Loadout.charm == Charm.charm_smoke_dash || self.player.stats.CurseSmokeDash) && !Level.IsChessBoss)
        {
            self.spriteRenderer.enabled = false;
            self.smokeDashEffect.Create(self.player.center);
        }*/
    }

    private void OnDashEnd(On.LevelPlayerAnimationController.orig_OnDashEnd orig, LevelPlayerAnimationController self)
    {/*
		if ((self.player.stats.Loadout.charm == Charm.charm_smoke_dash || self.player.stats.CurseSmokeDash) && !Level.IsChessBoss)
		{
			self.spriteRenderer.enabled = true;
			self.smokeDashEffect.Create(self.player.center);
		}*/
        if (!self.player.motor.Grounded && self.player.stats.isChalice)
        {
            self.animator.Play((!self.player.motor.ChaliceDoubleJumped) ? self.ChaliceJumpDescend : self.ChaliceJumpBall, 3, 0f);
        }
    }

    private void OnParryStart(On.LevelPlayerAnimationController.orig_OnParryStart orig, LevelPlayerAnimationController self)
    {
        if (self.super)
        {
            return;
        }
        if (self.player.stats.Loadout.charm == Charm.charm_parry_plus && !Level.IsChessBoss)
        {
            self.SetBool(LevelPlayerAnimationController.Booleans.HasParryCharm, true);
        }
        bool whetstone = self.player.stats.Loadout.charm == Charm.charm_parry_attack;//new start
        bool relic = self.player.stats.CurseWhetsone;
        bool casualMode = self.player.stats.Loadout.charm == CustomCharms.casualMode;
        bool attackParryUsed = self.GetComponent<IParryAttack>().AttackParryUsed;
        if ((whetstone || relic || casualMode) && !Level.IsChessBoss && !attackParryUsed)//new end
        //if ((self.player.stats.Loadout.charm == Charm.charm_parry_attack || self.player.stats.CurseWhetsone) && !self.GetComponent<IParryAttack>().AttackParryUsed && !Level.IsChessBoss)
        {
            self.SetBool(LevelPlayerAnimationController.Booleans.HasParryAttack, true);
        }
        else if (self.player.stats.Loadout.charm == Charm.charm_curse)
        {
            self.SetBool(LevelPlayerAnimationController.Booleans.HasParryAttack, false);
        }
        self.SetTrigger(LevelPlayerAnimationController.Triggers.OnParry);
    }

    public void OnParrySuccess(On.LevelPlayerAnimationController.orig_OnParrySuccess orig, LevelPlayerAnimationController self)
    {
        if (self.player.stats.Loadout.charm == Charm.charm_parry_plus && !Level.IsChessBoss)
        {
            self.SetBool(LevelPlayerAnimationController.Booleans.HasParryCharm, false);
        }
        bool whetstone = self.player.stats.Loadout.charm == Charm.charm_parry_plus;//new start
        bool relic = self.player.stats.CurseWhetsone;
        bool casualMode = self.player.stats.Loadout.charm == CustomCharms.casualMode;
        if ((whetstone || relic || casualMode) && !Level.IsChessBoss)//new end
        //if ((self.player.stats.Loadout.charm == Charm.charm_parry_attack || self.player.stats.CurseWhetsone) && !Level.IsChessBoss)
        {
            self.SetBool(LevelPlayerAnimationController.Booleans.HasParryAttack, false);
        }
        self.SetAlpha(1f);
        if (self.player.stats.isChalice)
        {
            if (self.chaliceDashEffectActive != null)
            {
                UnityEngine.Object.Destroy(self.chaliceDashEffectActive.gameObject);
            }
            self.animator.Play("Jump_Launch", 3, 0f);
        }
    }
}
