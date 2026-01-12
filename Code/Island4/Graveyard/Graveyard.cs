using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class Graveyard
{
    public void Init()
    {
        new GraveyardDevil().Init();
        On.GraveyardLevel.OnLevelStart += OnLevelStart;
    }

    public void OnLevelStart(On.GraveyardLevel.orig_OnLevelStart orig, GraveyardLevel self)
    {
        currentSideIndex = 0;
        speed = self.properties.CurrentState.splitDevilProjectiles.projectileSpeed;
        self.StartCoroutine(timer_cr(self));
        self.StartCoroutine(checkForPlayerTurned_cr(self));
    }

    public IEnumerator timer_cr(GraveyardLevel self)
    {
		yield return CupheadTime.WaitForSeconds(self, 1f);

        timer = 0f;
        for(; ; )
        {
            while (timer < maxIdleTime)
            {
                timer += CupheadTime.delta;
                yield return null;
            }
            FireProjectiles(self);
        }
    }

    private IEnumerator checkForPlayerTurned_cr(GraveyardLevel self)
    {
        playerTurnedRight = true;
        for(; ; )
        {
            bool newTurnedRight = Other.Player().transform.localScale.x > 0f;
            if(playerTurnedRight != newTurnedRight)
            {
                FireProjectiles(self);
                playerTurnedRight = newTurnedRight;
            }
            yield return null;
        }
    }

    private void FireProjectiles(GraveyardLevel self)
    {
        timer = 0f;
        Array sidesArray = Enum.GetValues(typeof(Side));
        Side currentSide = (Side)sidesArray.GetValue(currentSideIndex);

        float rotation;
        Vector2 spawnStart, spawnEnd;
        bool horizontal;
        switch (currentSide)
        {
            case Side.Left:
                rotation = 0f;
                horizontal = false;
                spawnStart = new(-800f, -400f);
                spawnEnd = new(-800f, 400f);
                break;
            case Side.Up:
                rotation = 270f;
                horizontal = true;
                spawnStart = new(-800f, 400f);
                spawnEnd = new(800f, 400f);
                break;
            case Side.Right:
                rotation = 180f;
                horizontal = false;
                spawnStart = new(800f, -400f);
                spawnEnd = new(800f, 400f);
                break;
            default:
                rotation = 90f;
                horizontal = true;
                spawnStart = new(-800f, -400f);
                spawnEnd = new(800f, -400f);
                break;
        }

        int loopStart = horizontal ? (int)spawnStart.x : (int)spawnStart.y;
        int loopEnd = horizontal ? (int)spawnEnd.x : (int)spawnEnd.y;
        int count = (loopEnd - loopStart) / spaceBetweenProjectiles;
        bool startWithDevil0 = Rand.Bool();
        for(int i = 0; i < count; i++)
        {
            Vector2 pos = new(
                horizontal ? 
                    spawnStart.x + i * spaceBetweenProjectiles :
                    spawnStart.x,
                horizontal ?
                    spawnStart.y :
                    spawnStart.y + i * spaceBetweenProjectiles);

            GraveyardLevelSplitDevil devil = startWithDevil0 ^ i > count / 2 ?
                self.splitDevil[0] :
                self.splitDevil[1];

            self.splitDevil[0].projectilePrefab.Create(pos, rotation, speed, devil);
        }

        currentSideIndex++;
        if(currentSideIndex >= sidesArray.Length)
        {
            currentSideIndex = 0;
        }
    }

    public static IEnumerator warning_cr(GraveyardLevel self)//new
    {
        if (!showedWarning)
        {
            yield return CupheadTime.WaitForSeconds(self, 1f);
            string tText = "No Spinjitsu allowed!";
            string mText = "Each turn will spawn a new wave of projectiles.";
            self.StartCoroutine(Other.notification_cr(self, tText, mText, 5f, 125f, false));
            showedWarning = true;
        }
    }

    private static bool showedWarning = false;

    private float timer;
    private bool playerTurnedRight;
    private int currentSideIndex;
    private float speed;

    private const int spaceBetweenProjectiles = 75;
    private const float maxIdleTime = 2f;

    private enum Side
    {
        Left,
        Up,
        Right,
        //Down
    }
}
