using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class MouseCan
{
    public void Init()
    {
        On.MouseLevelCanMouse.dash_cr += dash_cr;
        On.MouseLevelCanMouse.FireCatapult += FireCatapult;
        On.MouseLevelCanMouse.catapult_cr += catapult_cr;
    }

    private IEnumerator dash_cr(On.MouseLevelCanMouse.orig_dash_cr orig, MouseLevelCanMouse self)
    {
        LevelProperties.Mouse.CanDash dashProperties = self.properties.CurrentState.canDash;
        /*for (int i = 0; i < self.springs.Length; i++)
        {
            Vector2 velocity = new Vector2(dashProperties.springVelocityX[i].RandomFloat(), dashProperties.springVelocityY[i].RandomFloat());
            if (self.direction == MouseLevelCanMouse.Direction.Right)
            {
                velocity.x *= -1f;
            }
            self.springs[i].LaunchSpring(new Vector2(self.transform.position.x, self.transform.position.y + 200f), velocity, dashProperties.springGravity);
            AudioManager.Play("level_mouse_can_springboard_shoot");
            self.emitAudioFromObject.Add("level_mouse_can_springboard_shoot");
            self.StartCoroutine(self.timedAudioMouseSnarky_cr());
        }*/
        if (self.moving)
        {
            yield return self.StartCoroutine(self.moveBack_cr());
        }
        Vector2 start = self.transform.position;
        Vector2 end = new Vector2(-450f * self.transform.localScale.x, self.transform.position.y);
        self.animator.Play("Dash", 1);
        AudioManager.PlayLoop("level_mouse_can_dash_loop");
        self.dash = false;
        while (!self.dash)
        {
            yield return null;
        }
        FireSpring(self);//new
        yield return self.StartCoroutine(self.tween_cr(self.transform, start, end, EaseUtils.EaseType.easeInSine, dashProperties.time));
        self.animator.SetTrigger("CanContinue");
        AudioManager.Stop("level_mouse_can_dash_loop");
        AudioManager.Play("level_mouse_can_dash_stop");
        self.emitAudioFromObject.Add("level_mouse_can_dash_stop");
        yield return self.animator.WaitForAnimationToEnd(self, "Dash_End", 1, false, true);
        yield return CupheadTime.WaitForSeconds(self, dashProperties.hesitate);
        self.state = MouseLevelCanMouse.State.Idle;
        yield break;
    }

    private void FireCatapult(On.MouseLevelCanMouse.orig_FireCatapult orig, MouseLevelCanMouse self)
    {
        LevelProperties.Mouse.CanCatapult canCatapult = self.properties.CurrentState.canCatapult;
        char[] array = canCatapult.patterns.GetRandom<string>().ToLower().ToCharArray();
        float num = (float)((self.direction != MouseLevelCanMouse.Direction.Right) ? 165 : -45);
        if (array.Length <= 1)
        {
            self.catapultProjectilePrefab.CreateFromPrefab(self.catapultRoot.position, num + canCatapult.angleOffset, (float)canCatapult.projectileSpeed, array[0]);
            return;
        }
        canFreeUp = array[2] == 'g';//new
        for (int i = 0; i < array.Length; i++)
        {
            float rotation = num + canCatapult.spreadAngle / (float)(array.Length - 1) * (float)i;
            self.catapultProjectilePrefab.CreateFromPrefab(self.catapultRoot.position, rotation, (float)canCatapult.projectileSpeed, array[i]);
        }
    }

    private IEnumerator catapult_cr(On.MouseLevelCanMouse.orig_catapult_cr orig, MouseLevelCanMouse self)
    {
        LevelProperties.Mouse.CanCatapult properties = self.properties.CurrentState.canCatapult;
        self.animator.ResetTrigger("Continue");
        self.animator.ResetTrigger("Shoot");
        self.animator.Play("Catapult_Idle", 0);
        yield return self.StartCoroutine(self.tweenCatapultY_cr(-280f, 0f, properties.timeIn, EaseUtils.EaseType.easeOutSine));
        yield return CupheadTime.WaitForSeconds(self, properties.pumpDelay);
        for (int i = 0; i < properties.count; i++)
        {
            self.animator.SetTrigger("Continue");
            self.SoundMouseCatapultGlug();
            yield return self.animator.WaitForAnimationToEnd(self, "Catapult_Pump", 0, false, true);
            yield return CupheadTime.WaitForSeconds(self, properties.pumpDelay);
            self.animator.SetTrigger("Shoot");
            yield return self.animator.WaitForAnimationToEnd(self, "Catapult_Shoot", 0, false, true);
            FireCatapultBonus(self);//new
            yield return CupheadTime.WaitForSeconds(self, properties.repeatDelay);
        }
        yield return self.StartCoroutine(self.tweenCatapultY_cr(0f, -280f, properties.timeOut, EaseUtils.EaseType.easeOutSine));
        self.animator.Play("Idle_Down", 0);
        yield return CupheadTime.WaitForSeconds(self, (float)properties.hesitate);
        self.state = MouseLevelCanMouse.State.Idle;
        yield break;
    }

    private void FireSpring(MouseLevelCanMouse self)//new
    {
        LevelProperties.Mouse.CanDash dashProperties = self.properties.CurrentState.canDash;
        Vector2 velocity = new Vector2(dashProperties.springVelocityX[0].RandomFloat(), dashProperties.springVelocityY[0].RandomFloat());
        if (self.direction == MouseLevelCanMouse.Direction.Right)
        {
            velocity.x *= -1f;
        }
        self.springs[0].LaunchSpring(new Vector2(self.transform.position.x, self.transform.position.y + 200f), velocity, dashProperties.springGravity);
        AudioManager.Play("level_mouse_can_springboard_shoot");
        self.emitAudioFromObject.Add("level_mouse_can_springboard_shoot");
        self.StartCoroutine(self.timedAudioMouseSnarky_cr());
    }

    private void FireCatapultBonus(MouseLevelCanMouse self)//new
    {
        LevelProperties.Mouse.CanCatapult canCatapult = self.properties.CurrentState.canCatapult;
        int openSpace;
        if (canFreeUp)
        {
            openSpace = UnityEngine.Random.Range(0, 4);
        }
        else
        {
            openSpace = UnityEngine.Random.Range(0, 3);
        }
        for (int i = 0; i < 6; i++)
        {
            if (i != openSpace)
            {
                Vector3 position = new Vector3(-1000f * self.transform.localScale.x, 150f * i - 230f);
                float rotation = 90f - 90f * self.transform.localScale.x;
                self.catapultProjectilePrefab.CreateFromPrefab(position, rotation, (float)canCatapult.projectileSpeed, 'n').transform.SetScale(2f, 2f, 2f);
            }
        }
    }

    private bool canFreeUp = false;
}
