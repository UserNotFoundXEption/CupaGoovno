using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class DragonFireMarcher
{
    public void Init()
    {
        On.DragonLevelFireMarcher.Awake += Awake;
        On.DragonLevelFireMarcher.CanJump += CanJump;
        On.DragonLevelFireMarcher.jump_cr += jump_cr;
    }

    protected void Awake(On.DragonLevelFireMarcher.orig_Awake orig, DragonLevelFireMarcher self)
    {
        orig(self);
        self.animator.Play("Idle", 0, UnityEngine.Random.Range(0f, 1f));
    }

    public bool CanJump(On.DragonLevelFireMarcher.orig_CanJump orig, DragonLevelFireMarcher self)
    {   /*
		if (!self.canJump || self.wantsToJump)
		{
			return false;
		}
		AnimatorStateInfo currentAnimatorStateInfo = self.animator.GetCurrentAnimatorStateInfo(0);
		float num = self.transform.localPosition.x + (1f - currentAnimatorStateInfo.normalizedTime % 1f) * currentAnimatorStateInfo.length * self.properties.moveSpeed;
		return num > self.properties.jumpX.min && num < self.properties.jumpX.max;*/
        return self.transform.position.x > -300f && self.transform.position.x < 600f && !self.wantsToJump;
    }

    private IEnumerator jump_cr(On.DragonLevelFireMarcher.orig_jump_cr orig, DragonLevelFireMarcher self)
    {
        self.animator.SetTrigger("StartJump");
        yield return self.animator.WaitForAnimationToStart(self, "Crouch_Start", false);
        AudioManager.Play("level_dragon_fire_marcher_b_couch_start");
        self.emitAudioFromObject.Add("level_dragon_fire_marcher_b_couch_start");
        self.slowing = true;
        yield return self.animator.WaitForAnimationToStart(self, "Crouch_Loop", false);
        Vector2 targetPos = self.targetPlayer.center;
        float targetX;//new start
        float playerX = self.targetPlayer.center.x;
        float currentX = self.transform.position.x;
        float minDistance = 300f;
        bool okTargetDistance = false;
        do
        {
            targetX = UnityEngine.Random.Range(-400f, 650f);
            okTargetDistance = Mathf.Abs(targetX - currentX) > minDistance--;
        } while (!okTargetDistance);//new end
        if (targetX < self.transform.position.x)
        {
            self.transform.SetScale(new float?(-1f), null, null);
        }
        /*
        float bestDistance = float.MaxValue;
        Vector2 bestLaunchVelocity = Vector2.zero;
        Vector2 relativeTargetPos = targetPos - self.transform.position;
        relativeTargetPos.x = Mathf.Abs(relativeTargetPos.x);
        for (float num = 0f; num < 1f; num += 0.01f)
        {
            float floatAt = self.properties.jumpAngle.GetFloatAt(num);
            float floatAt2 = self.properties.jumpSpeed.GetFloatAt(num);
            Vector2 vector = MathUtils.AngleToDirection(floatAt) * floatAt2;
            float num2 = relativeTargetPos.x / vector.x;
            float num3 = vector.y * num2 - 0.5f * self.properties.gravity * num2 * num2;
            float num4 = Mathf.Abs(relativeTargetPos.y - num3);
            if (num4 < bestDistance)
            {
                bestDistance = num4;
                bestLaunchVelocity = vector;
            }
        }*/

        yield return CupheadTime.WaitForSeconds(self, self.properties.crouchTime);
        self.animator.SetTrigger("Continue");
        yield return self.animator.WaitForAnimationToStart(self, "Jump_Start", false);
        AudioManager.Play("level_dragon_fire_marcher_b_jump_start");
        self.emitAudioFromObject.Add("level_dragon_fire_marcher_b_jump_start");/*
		Vector2 velocity = bestLaunchVelocity;
		velocity.x *= self.transform.localScale.x;
		float t = 0f;
		Vector2 initialPos = self.transform.localPosition;
		while (self.transform.position.y > -400f)
		{
			t += CupheadTime.FixedDelta;
			self.transform.SetLocalPosition(new float?(initialPos.x + t * velocity.x), new float?(initialPos.y + t * velocity.y - 0.5f * self.properties.gravity * t * t), null);
			yield return new WaitForFixedUpdate();
		}*/
        float velocity = (targetX - self.transform.position.x);//new start
        float t = 0f;
        float t2, deltaX, deltaY;
        float maxHeight = UnityEngine.Random.Range(300f, 650f);
        Vector2 initialPos = self.transform.localPosition;
        while (self.transform.position.y > -400f)
        {
            t += CupheadTime.FixedDelta;
            t2 = t * t;
            deltaX = t2 * velocity / 3;
            deltaY = Mathf.Sin(t2) * maxHeight;
            self.transform.SetLocalPosition(initialPos.x + deltaX, initialPos.y + deltaY, 0f);
            yield return new WaitForFixedUpdate();
        }//new end
        UnityEngine.Object.Destroy(self.gameObject);
        yield break;
    }
}
