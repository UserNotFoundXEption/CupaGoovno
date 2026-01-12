using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CupaGoovno;

public class Slime
{
    public void Init()
    {
        new SlimeTombstone().Init();
        On.SlimeLevelSlime.Awake += Awake;
        On.SlimeLevelSlime.Update += Update;
        On.SlimeLevelSlime.OnDestroy += OnDestroy;
        On.SlimeLevelSlime.jump_cr += jump_cr;
        On.SlimeLevelSlime.punch_cr += punch_cr;
    }

    protected void Awake(On.SlimeLevelSlime.orig_Awake orig, SlimeLevelSlime self)
    {
        orig(self);
        self.maxX += 100f;
    }

    protected void Update(On.SlimeLevelSlime.orig_Update orig, SlimeLevelSlime self)
    {
        orig(self);
        AbstractPlayerController player = PlayerManager.GetNext();
        Vector3 pos = player.transform.position;
        if (pos != null)
        {
            if (pos.x < -self.maxX)
            {
                player.transform.position = new Vector3(self.maxX - 1, pos.y);
            }
            if (pos.x > self.maxX)
            {
                player.transform.position = new Vector3(-self.maxX + 1, pos.y);
            }
        }
    }

    protected void OnDestroy(On.SlimeLevelSlime.orig_OnDestroy orig, SlimeLevelSlime self)
    {
        orig(self);
        if(YoMamaFat.parryOrbTransform != null)
        {
            YoMamaFat.parryOrbTransform.position = new Vector3(2137f, 2137f);//new
        }
    }

    private IEnumerator jump_cr(On.SlimeLevelSlime.orig_jump_cr orig, SlimeLevelSlime self)
    {
        LevelProperties.Slime.Jump p = self.CurrentPropertyState.jump;
        string[] pattern = p.patternString.Split(new char[]
        {
        ','
        });
        int i = UnityEngine.Random.Range(0, pattern.Length);
        int numJumpsLeft = 0;
        if (self.firstTime && self.isBig)
        {
            numJumpsLeft = p.bigSlimeInitialJumpPunchCount;
            self.firstTime = false;
        }
        else
        {
            numJumpsLeft = p.numJumps.RandomInt();
        }
        self.animator.SetTrigger("Jump");
        float delay = p.groundDelay;
        self.playerToPunch = PlayerManager.GetNext();
        for (; ; )
        {
            /*if (pattern[i][0] == 'D')
            {
                Parser.FloatTryParse(pattern[i].Substring(1), out delay);
            }
            else
            {*/
            yield return self.animator.WaitForAnimationToStart(self, "Jump_Squish_Loop", false);
            if (self.isBig)
            {
                self.BigJumpAudio();
            }
            else
            {
                self.SmallJumpAudio();
            }

            SlimeLevelSlime.Direction moveDir; //new start
            bool highJump;
            float speedXmultiplier = 1;
            float gravityMultiplier = 1;
            float hpPercent = self.properties.CurrentHealth / self.properties.TotalHealth * 100f;
            if (UnityEngine.Random.Range(0, 100) > hpPercent / 2 - 70)
            {
                float[] jumpData = GetJumpToPlayer(self);
                highJump = jumpData[0] == 1;
                moveDir = jumpData[1] == 1 ? SlimeLevelSlime.Direction.Left : SlimeLevelSlime.Direction.Right;
                speedXmultiplier = jumpData[2];
                gravityMultiplier = jumpData[3];
            }
            else
            {
                moveDir = UnityEngine.Random.Range(0, 2) == 0 ? SlimeLevelSlime.Direction.Right : SlimeLevelSlime.Direction.Left;
                highJump = UnityEngine.Random.Range(0, 2) == 0;
                speedXmultiplier = 1;
                gravityMultiplier = 1;
            }

            bool turnedLeft = moveDir == SlimeLevelSlime.Direction.Left;
            self.transform.SetScale(turnedLeft ? 1f : -1f);//new end

            yield return CupheadTime.WaitForSeconds(self, delay);
            self.animator.SetTrigger("Continue");
            yield return self.animator.WaitForAnimationToStart(self, "Up", false);
            bool goingUp = true;
            /*bool highJump; = pattern[i][0] == 'H';
			if (pattern[i][0] == 'R')
			{
				highJump = Rand.Bool();
			}*/



            self.velocityY = ((!highJump) ? p.lowJumpVerticalSpeed : p.highJumpVerticalSpeed);
            float speedX = (!highJump) ? p.lowJumpHorizontalSpeed : p.highJumpHorizontalSpeed;
            self.gravity = ((!highJump) ? p.lowJumpGravity : p.highJumpGravity);
            speedX *= speedXmultiplier;//new
            self.gravity *= gravityMultiplier;//new
            self.inAir = true;
            // SlimeLevelSlime.Direction moveDir = self.facingDirection;
            self.shadow.enabled = true;
            while (goingUp || self.transform.position.y > self.onGroundY)
            {
                self.velocityY -= self.gravity * CupheadTime.FixedDelta * self.hitPauseCoefficient();
                float velocityX = (moveDir != SlimeLevelSlime.Direction.Left) ? speedX : (-speedX);
                self.transform.AddPosition(velocityX * CupheadTime.FixedDelta * self.hitPauseCoefficient(), self.velocityY * CupheadTime.FixedDelta * self.hitPauseCoefficient(), 0f);
                if (self.velocityY < 0f && goingUp)
                {
                    goingUp = false;
                    self.animator.SetTrigger("Apex");
                }
                if (self.transform.position.x < -self.maxX)//new start
                {
                    self.transform.SetPosition(new float?(self.maxX - 1), null, null);
                }
                if (self.transform.position.x > self.maxX)
                {
                    self.transform.SetPosition(new float?(-self.maxX + 1), null, null);
                }//new end
                /*
                if ((moveDir == SlimeLevelSlime.Direction.Left && self.transform.position.x < -self.maxX) || (moveDir == SlimeLevelSlime.Direction.Right && self.transform.position.x > self.maxX))
                {
                //	if (moveDir == SlimeLevelSlime.Direction.Left)
                if(UnityEngine.Random.Range(0, 2) == 0)
                    {
                        self.transform.SetPosition(new float?(-self.maxX), null, null);
                        moveDir = SlimeLevelSlime.Direction.Right;
                    }
                    else
                    {
                        self.transform.SetPosition(new float?(self.maxX), null, null);
                        moveDir = SlimeLevelSlime.Direction.Left;
                    }
                    if (!goingUp)
                    {
                        speedX = 0f;
                    }
                    //turn byl tutaj
                }*/
                self.Turn();//new
                yield return new WaitForFixedUpdate();
            }
            self.transform.SetPosition(null, new float?(self.onGroundY), null);
            self.shadow.enabled = false;
            self.inAir = false;
            delay = p.groundDelay;
            float screenShakeCoefficient = (!highJump) ? 1f : 1.5f;
            screenShakeCoefficient *= ((!self.isBig) ? 1f : 2f);
            CupheadLevelCamera.Current.Shake(5f * screenShakeCoefficient, 0.2f * screenShakeCoefficient, false);
            self.dustPrefab.Create(self.transform.position);
            if (self.wantsToTransform && self.transform.position.x > -350f && self.transform.position.x < 350f)
            {
                break;
            }
            if (self.dieOnLand)
            {
                self.animator.SetTrigger("LandingDeath");
                self.state = SlimeLevelSlime.State.Dying;
                yield break;
            }
            self.animator.SetTrigger("Land");
            if (self.isBig && !self.firstPunch)
            {
                self.jumpsBeforeFirstPunch--;
                if (self.jumpsBeforeFirstPunch == 0)
                {
                    self.firstPunch = true;
                    yield return self.Punch();
                    yield break;
                }
            }
            else
            {
                numJumpsLeft--;
                if (numJumpsLeft <= 0 && self.inPunchPosition())
                {
                    yield return self.Punch();
                    yield break;
                }
            }
            //}
            i = (i + 1) % pattern.Length;
        }
        self.animator.SetTrigger("Transform");
        yield break;
    }

    private IEnumerator punch_cr(On.SlimeLevelSlime.orig_punch_cr orig, SlimeLevelSlime self)
    {
        SlimeLevelSlime.Direction direction = self.punchDirection;
        self.facingDirection = direction;
        /*SlimeLevelSlime.Direction direction2 = self.facingDirection;
        if (self.punchDirection != self.facingDirection)
        {
            self.facingDirection = ((self.facingDirection != SlimeLevelSlime.Direction.Left) ? SlimeLevelSlime.Direction.Left : SlimeLevelSlime.Direction.Right);
            self.transform.SetScale(new float?((float)((self.facingDirection != SlimeLevelSlime.Direction.Right) ? 1 : -1)), null, null);
            yield return CupheadTime.WaitForSeconds(self, 0.1f);
        }*/
        if (self.isBig)
        {
            YoMamaFat.parryOrb.parrySwitch.enabled = true;
            self.transform.AddPosition(0, -100f, 0);
            Vector3 pos = self.transform.position;
            float orbX = self.punchDirection == SlimeLevelSlime.Direction.Left ? -400 : 400f;
            orbX += pos.x + UnityEngine.Random.Range(-150f, 150f);
            float orbY = pos.y + 400f + UnityEngine.Random.Range(-50f, 50f);
            YoMamaFat.parryOrbTransform.position = new Vector3(orbX, orbY);
            yield return CupheadTime.WaitForSeconds(self, 0.5f);
        }
        self.transform.SetScale(self.punchDirection == SlimeLevelSlime.Direction.Left ? 1f : -1f, null, null);
        self.animator.Play("Punch_Pre_Hold");
        yield return CupheadTime.WaitForSeconds(self, self.CurrentPropertyState.punch.preHold);
        self.animator.SetTrigger("Continue");
        yield return self.animator.WaitForAnimationToStart(self, "Punch_Hold", false);
        if (self.isBig)
        {
            yield return CupheadTime.WaitForSeconds(self, 0.5f);
            self.transform.AddPosition(0, 100f, 0);
            YoMamaFat.parryOrbTransform.position = new Vector3(2137f, 2137f);
        }
        yield return CupheadTime.WaitForSeconds(self, self.CurrentPropertyState.punch.mainHold);
        self.animator.SetTrigger("Continue");
        yield return self.animator.WaitForAnimationToStart(self, "Punch_End", false);
        if (!self.isBig)
        {
            int num;
            for (int i = 0; i < 2; i = num + 1)
            {
                self.animator.Play("Punch_Pre_Hold");
                yield return CupheadTime.WaitForSeconds(self, 0.05f);
                self.animator.SetTrigger("Continue");
                yield return self.animator.WaitForAnimationToStart(self, "Punch_Hold", false);
                yield return CupheadTime.WaitForSeconds(self, self.CurrentPropertyState.punch.mainHold / 5f);
                self.animator.SetTrigger("Continue");
                yield return self.animator.WaitForAnimationToStart(self, "Punch_End", false);
                num = i;
            }
        }
        self.BigPunchPlaying = false;
        self.StartJump();
        if (self.isBig)
        {
            self.animator.SetBool("FirstPunch", false);
        }
        yield break;
    }

    private float[] GetJumpToPlayer(SlimeLevelSlime self)//new
    {
        LevelProperties.Slime.Jump p = self.CurrentPropertyState.jump;
        float playerX = self.playerToPunch.transform.position.x + UnityEngine.Random.Range(-150f, 150f);
        float slimeX = self.transform.position.x;
        float toPlayerStraight = playerX - slimeX;
        float toPlayerWrap;
        if (toPlayerStraight < 0)
        {
            toPlayerWrap = toPlayerStraight + 2 * self.maxX;
        }
        else
        {
            toPlayerWrap = toPlayerStraight - 2 * self.maxX;
        }
        //Vy*2=t; t*Vx=s
        float lowJumpX = p.lowJumpVerticalSpeed * 2 / p.lowJumpGravity * p.lowJumpHorizontalSpeed;
        float highJumpX = p.highJumpVerticalSpeed * 2 / p.highJumpGravity * p.highJumpHorizontalSpeed;
        float longJump = UnityEngine.Random.Range(0, 2) == 0 ? 1 : 0;
        //short left, short right, long left, long right
        float[] jumpX = new float[] { -lowJumpX, lowJumpX, -highJumpX, highJumpX };
        float minDifference = float.MaxValue;
        int chosenJump = 0;
        float toPlayer = 1f;
        for (int i = 0; i < 2; i++)
        {
            int currentJump = i + (int)longJump * 2;
            float differenceStraight = Mathf.Abs(toPlayerStraight - jumpX[currentJump]);
            float differenceWrap = Mathf.Abs(toPlayerWrap - jumpX[currentJump]);
            if (differenceStraight < minDifference)
            {
                minDifference = differenceStraight;
                toPlayer = toPlayerStraight;
                chosenJump = currentJump;
            }
            if (differenceWrap < minDifference)
            {
                minDifference = differenceWrap;
                toPlayer = toPlayerWrap;
                chosenJump = currentJump;
            }
        }
        float jumpLeft = chosenJump % 2 == 0 ? 1 : 0;
        float speedXMultiplier = toPlayer / jumpX[chosenJump];
        float gravityMultiplier = 1f;
        if (speedXMultiplier > 1.5f)
        {
            speedXMultiplier = 1.5f;
        }
        if (speedXMultiplier < 1f)
        {
            gravityMultiplier = 1f / speedXMultiplier * Mathf.Sign(speedXMultiplier);
            speedXMultiplier = 1f;
        }
        return new float[] { longJump, jumpLeft, speedXMultiplier, gravityMultiplier };
    }

    public static Sprite GetSlimeSprite(SlimeLevel level)
    {
        Sprite sprite1 = sprites.Count >= 1 ? sprites[0] : null;
        Sprite sprite2 = sprites.Count >= 2 ? sprites[1] : null;
        Sprite sprite3 = sprites.Count >= 3 ? sprites[2] : null;

        switch (level.properties.CurrentState.stateName)
        {
            case LevelProperties.Slime.States.Main:
            case LevelProperties.Slime.States.Generic:
                return sprite1 != null ? sprite1 : level._bossPortraitMain;
            case LevelProperties.Slime.States.BigSlime:
                return sprite2 != null ? sprite2 : level._bossPortraitBigSlime;
            case LevelProperties.Slime.States.Tombstone:
                return sprite3 != null ? sprite3 : level._bossPortraitTombstone;
            default:
                Plugin.Log("Couldn't find portrait for state " + level.properties.CurrentState.stateName + ". Using Main.");
                return level._bossPortraitMain;
        }
    }

    public static List<Sprite> sprites = [];
}
