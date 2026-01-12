using System;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class Camera
{
    public void Init()
    {
        On.CupheadLevelCamera.UpdatePath += UpdatePath;
        On.CupheadLevelCamera.UpdatePlatforming += UpdatePlatforming;
        On.CupheadLevelCamera.UpdateModeRelative += UpdateModeRelative;
    }

    public void UpdatePath(On.CupheadLevelCamera.orig_UpdatePath orig, CupheadLevelCamera self)
    {
        Vector3 cameraPos = self._position;
        Vector2 playerCenter = PlayerManager.Center;
        if (self.stabilizeY)
        {
            AbstractPlayerController player1 = PlayerManager.GetPlayer(PlayerId.PlayerOne);
            AbstractPlayerController player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
            Vector2 player1Pos = (!(player1 == null)) ? player1.center : Vector2.zero;
            Vector2 player2Pos = (!(player2 == null)) ? player2.center : Vector2.zero;
            if (player1Pos.y > cameraPos.y + self.stabilizePaddingTop)
            {
                player1Pos.y -= self.stabilizePaddingTop;
            }
            else if (player1Pos.y < cameraPos.y - self.stabilizePaddingBottom)
            {
                player1Pos.y += self.stabilizePaddingBottom;
            }
            else
            {
                player1Pos.y = cameraPos.y;
            }
            if (player2Pos.y > cameraPos.y + self.stabilizePaddingTop)
            {
                player2Pos.y -= self.stabilizePaddingTop;
            }
            else if (player2Pos.y < cameraPos.y - self.stabilizePaddingBottom)
            {
                player2Pos.y += self.stabilizePaddingBottom;
            }
            else
            {
                player2Pos.y = cameraPos.y;
            }
            if (player1 != null && !player1.IsDead && player2 != null && !player2.IsDead)
            {
                playerCenter = (player1Pos + player2Pos) / 2f;
            }
            else if (player1 != null && !player1.IsDead)
            {
                playerCenter = player1Pos;
            }
            else if (player2 != null && !player2.IsDead)
            {
                playerCenter = player2Pos;
            }
        }
        if (self.cameraOffset)
        {
            float cameraOffset = (!self.leftOffset) ? -500f : 500f;
            self.targetPos = new Vector3(playerCenter.x + cameraOffset, playerCenter.y);
        }
        else
        {
            self.targetPos = playerCenter;
        }
        Vector3 actualTargetPos = self.path.GetClosestPoint(self._position, self.targetPos, self.moveX, self.moveY);
        float perfectSpeed = (actualTargetPos - cameraPos).magnitude / CupheadTime.Delta / SuperSandevistan.multiplier;//new
        //float perfectSpeed  = (actualTargetPos - cameraPos).magnitude / CupheadTime.Delta;
        float maxSpeed = Mathf.Max(self._speedLastFrame + 5000f * CupheadTime.Delta / SuperSandevistan.multiplier, 1000f);//new
        //float maxSpeed = Mathf.Max(self._speedLastFrame + 5000f * CupheadTime.Delta, 1000f);
        if (perfectSpeed > maxSpeed)
        {
            actualTargetPos = cameraPos + (actualTargetPos - cameraPos).normalized * maxSpeed * CupheadTime.Delta / SuperSandevistan.multiplier;//new
            //actualTargetPos = cameraPos + (actualTargetPos - cameraPos).normalized * maxSpeed * CupheadTime.Delta;
        }
        self._speedLastFrame = Mathf.Min(perfectSpeed, maxSpeed);
        if (self.pathMovesOnlyForward)
        {
            float closestNormalizedPoint = self.path.GetClosestNormalizedPoint(self._position, actualTargetPos, self.moveX, self.moveY);
            if (closestNormalizedPoint < self._minPathValue)
            {
                return;
            }
        }
        cameraPos.x = actualTargetPos.x;
        cameraPos.y = actualTargetPos.y;
        if (!self.cameraLocked)
        {
            if (!self.autoScrolling)
            {
                self._position = Vector3.Lerp(self._position, cameraPos, CupheadTime.Delta * 15f / SuperSandevistan.multiplier);//new
                //self._position = Vector3.Lerp(self._position, cameraPos, CupheadTime.Delta * 15f);
            }
            else
            {
                Vector3 vector5 = new(self.transform.position.x + 500f, self.transform.position.y);
                Vector3 vector6 = self.path.GetClosestPoint(self._position, vector5, self.moveX, self.moveY);
                float num4 = 200f * self.autoScrollSpeedMultiplier;
                self._position = Vector3.MoveTowards(self._position, vector6, CupheadTime.Delta * num4);
            }
        }
        if (self.pathMovesOnlyForward)
        {
            self._minPathValue = self.path.GetClosestNormalizedPoint(self._position, self._position, self.moveX, self.moveY);
        }
    }

    public void UpdatePlatforming(On.CupheadLevelCamera.orig_UpdatePlatforming orig, CupheadLevelCamera self)
    {
        Vector3 position = self._position;
        Vector3 vector = PlayerManager.Center;
        if (self.moveX && position.x < vector.x)
        {
            position.x = vector.x;
        }
        if (self.moveY)
        {
            position.y = vector.y;
        }
        position.x = Mathf.Clamp(position.x, self.Left, self.Right);
        position.y = Mathf.Clamp(position.y, self.Bottom, self.Top);
        self._position = Vector3.Lerp(self._position, position, CupheadTime.Delta * 5f / SuperSandevistan.multiplier);//new
        //self._position = Vector3.Lerp(self._position, position, CupheadTime.Delta * 5f);
    }


    private void UpdateModeRelative(On.CupheadLevelCamera.orig_UpdateModeRelative orig, CupheadLevelCamera self)
    {
        if (camera == null)
        {
            camera = self;
        }
        if (following)
        {
            self._position = followObj.transform.position;
            float angle = MathUtils.DirectionToAngle(self._position);
            self.transform.SetEulerAngles(0f, 0f, angle + 90f);
        }
        Vector2 v = self._position;
        Vector2 vector = new Vector2(0f, 0f);
        vector.x = MathUtils.GetPercentage((float)Level.Current.Left, (float)Level.Current.Right, PlayerManager.Center.x);
        vector.y = MathUtils.GetPercentage((float)Level.Current.Ground, (float)Level.Current.Ceiling, PlayerManager.Center.y);
        if (self.moveX)
        {
            v.x = Mathf.Lerp(self.Left, self.Right, vector.x);
        }
        if (self.moveY)
        {
            v.y = Mathf.Lerp(self.Bottom, self.Top, vector.y);
        }
        v.x = Mathf.Clamp(v.x, self.Left, self.Right);
        v.y = Mathf.Clamp(v.y, self.Bottom, self.Top);
        self._position = Vector3.Lerp(self._position, v, CupheadTime.Delta * 5f);
    }

    public static void Follow(GameObject obj)//new
    {
        following = true;
        followObj = obj;
        originalZoom = camera.zoom;
        zoomCoroutine = camera.StartCoroutine(actualZoom_cr(originalZoom * 0.6f, 2f));
    }

    public static void UnFollow()//new
    {
        following = false;
        camera.transform.ResetLocalTransforms();
        if(zoomCoroutine != null)
        {
            camera.StopCoroutine(zoomCoroutine);
        }
        camera.zoom = originalZoom;
    }

    private static IEnumerator actualZoom_cr(float newZoom, float time)//new
    {
        float t = 0;
        float originalZoom = camera.zoom;
        float zoomDelta = newZoom - originalZoom;
        while(t < time)
        {
            t += CupheadTime.Delta;
            float percent = t / time;
            camera.zoom = originalZoom + zoomDelta * percent;
            yield return null;
        }
        for(; ; )
        {
            camera.zoom = newZoom;
            yield return null;
        }
    }
    
    public static CupheadLevelCamera camera;

    private static bool following = false;
    private static GameObject followObj;
    private static float originalZoom;
    private static Coroutine zoomCoroutine;
}
