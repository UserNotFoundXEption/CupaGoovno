using System;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class Motor
{
    public void Init()
    {
        On.MapPlayerMotor.MoveWalking += MoveWalking;
        On.PlanePlayerMotor.FixedUpdate += FixedUpdate;
        On.LevelPlayerMotor.HandleInput += HandleInput;
        On.LevelPlayerMotor.Start += Start;
        On.LevelPlayerMotor.OnPitKnockUp += OnPitKnockUp;
        On.LevelPlayerMotor.Move += Move;
        On.LevelPlayerMotor.HandleDash += HandleDash;
        On.LevelPlayerMotor.ChaliceDashCooldownCheck += ChaliceDashCooldownCheck;
        On.LevelPlayerMotor.HandleFalling += HandleFalling;
        On.LevelPlayerMotor.HandlePitKnockUp += HandlePitKnockUp;
        On.LevelPlayerMotor.HandleHit += HandleHit;
        On.LevelPlayerMotor.HandleJumping += HandleJumping;
    }

    private void MoveWalking(On.MapPlayerMotor.orig_MoveWalking orig, MapPlayerMotor self)
    {
        self.velocity = Vector2.Lerp(self.velocity, new Vector2(self.axis.x * 5f, self.axis.y * 5f), CupheadTime.Delta * 100f);
        //self.velocity = Vector2.Lerp(self.velocity, new Vector2(self.axis.x * 2.5f, self.axis.y * 2.5f), CupheadTime.Delta * 100f);
        self.rigidbody2D.velocity = self.velocity;
    }

    private void FixedUpdate(On.PlanePlayerMotor.orig_FixedUpdate orig, PlanePlayerMotor self)
    {
        /*if (self.player.stats.StoneTime > 0f)
        {
            return;
        }*/
        self.HandleInput();
        if (!self.player.input.GetButton(CupheadButton.Lock))
        {
            self.Move();
        }
        self.HandleRaycasts();
        self.ClampPosition();
    }

    public void HandleInput(On.LevelPlayerMotor.orig_HandleInput orig, LevelPlayerMotor self)
    {
        self.dashManager.timeSinceGroundDash += CupheadTime.FixedDelta * (1 / SuperSandevistan.multiplier - 1);//new
        orig(self);
    }


    public void Start(On.LevelPlayerMotor.orig_Start orig, LevelPlayerMotor self)
    {
        orig(self);
        if (self.player.stats.Loadout.charm == CustomCharms.gravity)
        {
            self.StartCoroutine(gravityGlobe_cr(self));
        }
    }

    private IEnumerator gravityGlobe_cr(LevelPlayerMotor self)
    {
        GameObject obj = new GameObject("Ceiling");
        obj.tag = "Ceiling";

        int layerId = -1;
        while (layerId == -1)
        {
            layerId = LayerMask.NameToLayer("Bounds_Ceiling");
            yield return null;
        }
        obj.layer = layerId;

        BoxCollider2D collider = obj.AddComponent<BoxCollider2D>();
        collider.size = new Vector2(1500f, 10f);
        collider.isTrigger = true;
        collider.transform.SetPosition(0f, 360f);

        bool reversed = false;
        for (; ; )
        {
            if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.JoystickButton9))
            {
                reversed = !reversed;
                self.SetGravityReversed(reversed);
            }
            if (reversed && self.player.transform.position.y > 500f)
            {
                self.player.transform.SetPosition(null, 250f);
            }
            yield return null;
        }
    }

    public void OnPitKnockUp(On.LevelPlayerMotor.orig_OnPitKnockUp orig, LevelPlayerMotor self, float y, float velocityScale)
    {
        if (Level.Current.CurrentLevel == Levels.Bee || Level.Current.CurrentLevel == Levels.Dragon)
        {
            velocityScale *= 2;
        }
        orig(self, y, velocityScale);
    }

    public void Move(On.LevelPlayerMotor.orig_Move orig, LevelPlayerMotor self)
    {
        self.velocityManager.Calculate();
        Vector3 vector = self.velocityManager.Total;
        if (self.hitManager.state != LevelPlayerMotor.HitManager.State.Hit && self.superManager.state == LevelPlayerMotor.SuperManager.State.Ready)
        {
            if (!self.velocityManager.yAxisForce)
            {
                self.forceLaunchUp = false;
                if (self.Grounded)
                {
                    vector.x += self.velocityManager.GroundForce;
                }
                else
                {
                    vector.x += self.velocityManager.AirForce;
                }
            }
            else if (self.Grounded)
            {
                if (!self.forceLaunchUp)
                {
                    self.LeaveGround(false);
                    self.velocityManager.y = self.properties.jumpPower * 2f;
                    self.DisableGravity();
                    self.forceLaunchUp = true;
                }
            }
            else
            {
                vector.y += self.velocityManager.AirForce;
                self.FrameDelayedCallback(new Action(self.EnableGravity), 1);
            }
        }
        if (vector.x > 0f && !self.directionManager.right.able)
        {
            vector.x = 0f;
        }
        if (vector.x < 0f && !self.directionManager.left.able)
        {
            vector.x = 0f;
        }
        if (self.platformManager.OnPlatform)
        {
            if (!self.directionManager.right.able && self.MoveDirection.x > 0)
            {
                vector.x = 0f;
                self.transform.SetPosition(new float?(self.lastPosition.x), null, null);
            }
            if (!self.directionManager.left.able && self.MoveDirection.x < 0)
            {
                vector.x = 0f;
                self.transform.SetPosition(new float?(self.lastPosition.x), null, null);
            }
        }
        if (self.GravityReversed)
        {
            vector.y *= -1f;
        }
        self.transform.localPosition += vector * CupheadTime.FixedDelta / SuperSandevistan.multiplier;//new
        //self.transform.localPosition += vector * CupheadTime.FixedDelta;
        if (self.Grounded)
        {
            Vector2 vector2 = self.transform.position;
            vector2.y = self.directionManager.down.pos.y;
            self.transform.position = vector2;
            LevelPlatform levelPlatform = null;
            if (self.directionManager.down.gameObject != null)
            {
                levelPlatform = self.directionManager.down.gameObject.GetComponent<LevelPlatform>();
            }
            if (levelPlatform == null && self.transform.parent != null)
            {
                self.ClearParent();
            }
            else if (levelPlatform != null && (self.transform.parent == null || levelPlatform.gameObject != self.transform.parent.gameObject))
            {
                self.ClearParent();
                levelPlatform.AddChild(self.transform);
            }
        }
    }

    public bool HandleDash(On.LevelPlayerMotor.orig_HandleDash orig, LevelPlayerMotor self)
    {
        if (self.dashManager.state == LevelPlayerMotor.DashManager.State.Dashing)
        {
            float multiplier = 1f / SuperSandevistan.multiplier - 1f;
            self.dashManager.timer += CupheadTime.FixedDelta * multiplier;
        }
        return orig(self);
    }

    public void ChaliceDashCooldownCheck(On.LevelPlayerMotor.orig_ChaliceDashCooldownCheck orig, LevelPlayerMotor self)
    {
        if (self.dashManager.chaliceParryCoolDown)
        {
            self.dashManager.chaliceParryCoolDownTimer += CupheadTime.FixedDelta / SuperSandevistan.multiplier;//new
            //self.dashManager.chaliceParryCoolDownTimer += CupheadTime.FixedDelta;
            if (self.dashManager.chaliceParryCoolDownTimer >= self.properties.dashParryCooldownTime)
            {
                self.dashManager.chaliceParryCoolDown = false;
                self.dashManager.chaliceParryCoolDownTimer = 0f;
            }
        }
    }

    public void HandleFalling(On.LevelPlayerMotor.orig_HandleFalling orig, LevelPlayerMotor self)
    {
        if (self.Grounded || self.dashManager.IsDashing)
        {
            self.isFloating = false;
            self.jumpManager.floatTimer = 0f;
            return;
        }
        if (Level.Current.LevelTime < 0.2f)
        {
            return;
        }
        float num = self.properties.timeToMaxY * 60f;
        float num2 = self.properties.maxSpeedY / num * CupheadTime.FixedDelta / SuperSandevistan.multiplier;//new
        //float num2 = self.properties.maxSpeedY / num * CupheadTime.FixedDelta;
        self.velocityManager.y += num2;
        self.jumpManager.ableToLand = (self.velocityManager.y > 0f);
        if (self.player.stats.Loadout.charm == Charm.charm_float && self.jumpManager.ableToLand && self.player.input.actions.GetButton(2) && self.jumpManager.floatTimer < WeaponProperties.CharmFloat.maxTime)
        {
            self.isFloating = true;
            float num3 = Mathf.Clamp(self.jumpManager.floatTimer - WeaponProperties.CharmFloat.falloffStartTime, 0f, WeaponProperties.CharmFloat.maxTime - WeaponProperties.CharmFloat.falloffStartTime);
            num3 = Mathf.InverseLerp(0f, WeaponProperties.CharmFloat.maxTime - WeaponProperties.CharmFloat.falloffStartTime, num3);
            self.velocityManager.y = Mathf.Clamp(self.velocityManager.y, 0f, EaseUtils.EaseInSine(WeaponProperties.CharmFloat.minFallSpeed, WeaponProperties.CharmFloat.maxFallSpeed, num3));
            self.jumpManager.floatTimer += CupheadTime.FixedDelta;
        }
        else
        {
            self.isFloating = false;
        }
    }

    public void HandlePitKnockUp(On.LevelPlayerMotor.orig_HandlePitKnockUp orig, LevelPlayerMotor self)
    {
        if (self.hitManager.state != LevelPlayerMotor.HitManager.State.KnockedUp)
        {
            return;
        }
        if (self.hitManager.timer > self.properties.knockUpStunTime)
        {
            self.hitManager.state = LevelPlayerMotor.HitManager.State.Inactive;
            self.velocityManager.hit = 0f;
        }
        else
        {
            self.hitManager.timer += CupheadTime.FixedDelta / SuperSandevistan.multiplier;
        }
    }

    public void HandleHit(On.LevelPlayerMotor.orig_HandleHit orig, LevelPlayerMotor self)
    {
        if (self.hitManager.state != LevelPlayerMotor.HitManager.State.Hit)
        {
            return;
        }
        if (self.hitManager.timer > self.properties.hitStunTime)
        {
            self.hitManager.state = LevelPlayerMotor.HitManager.State.Inactive;
            self.velocityManager.hit = 0f;
        }
        else
        {
            float value = self.hitManager.timer / self.properties.hitStunTime;
            self.velocityManager.hit = EaseUtils.Ease(self.properties.hitKnockbackEase, self.properties.hitKnockbackPower, 0f, value) * (float)self.hitManager.direction;
            self.hitManager.timer += CupheadTime.FixedDelta / SuperSandevistan.multiplier;//new
            //self.hitManager.timer += CupheadTime.FixedDelta;
        }
    }

    public void HandleJumping(On.LevelPlayerMotor.orig_HandleJumping orig, LevelPlayerMotor self)
    {
        orig(self);
        float multiplier = 1f / SuperSandevistan.multiplier - 1f;
        if (self.allowJumping)
        {
            if(self.jumpManager.state == LevelPlayerMotor.JumpManager.State.Hold)
            {
                self.jumpManager.timer += CupheadTime.FixedDelta * multiplier;
            }
            self.jumpManager.timeSinceDownJump += multiplier;
        }
    }
}
