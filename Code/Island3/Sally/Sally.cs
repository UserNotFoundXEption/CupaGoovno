using System.Collections;
using UnityEngine;

namespace CupaGoovno;

public class Sally
{
    public void Init()
    {
        new SallyBackgroundHandler().Init();
        new SallyWindowProjectile().Init();
        new SallyUmbrellaProjectile().Init();
        new SallyLightning().Init();
        new SallyAngel().Init();
        new SallyProjectile().Init();
        On.SallyStagePlayLevel.Start += Start;
        On.SallyStagePlayLevel.nextPattern_cr += nextPattern_cr;
        On.SallyStagePlayLevel.jump_cr += jump_cr;
        On.SallyStagePlayLevelSally.Awake += Awake;
        On.SallyStagePlayLevelSally.fall_cr += fall_cr;
        On.SallyStagePlayLevelSally.jump_cr += jump_cr;
        On.SallyStagePlayLevelSally.diveKick_cr += diveKick_cr;
        On.SallyStagePlayLevelSally.jumpRoll_cr += jumpRoll_cr;
        On.SallyStagePlayLevelSally.SpawnHeart += SpawnHeart;
        On.SallyStagePlayLevelSally.delay_cr += delay_cr;
        On.SallyStagePlayLevelSally.slide_cr += slide_cr;
        On.SallyStagePlayLevelSally.phase_2_cr += phase_2_cr;
        On.SallyStagePlayLevelSally.SpawnProjectile += SpawnProjectile;
        On.SallyStagePlayLevelSally.OnDestroy += OnDestroy;
    }

    protected void Start(On.SallyStagePlayLevel.orig_Start orig, SallyStagePlayLevel self)
    {
        orig(self);
        YoMamaFat.sallyParent = self;
        waitForHorizontalAttack = false;
    }

    private IEnumerator nextPattern_cr(On.SallyStagePlayLevel.orig_nextPattern_cr orig, SallyStagePlayLevel self)
    {
        switch (self.properties.CurrentState.NextPattern)
        {
            case LevelProperties.SallyStagePlay.Pattern.Jump:
                yield return self.StartCoroutine(self.jump_cr());//new
                //self.StartCoroutine(self.jump_cr());
                break;
            case LevelProperties.SallyStagePlay.Pattern.Umbrella:
                self.StartCoroutine(self.umbrella_cr());
                break;
            case LevelProperties.SallyStagePlay.Pattern.Kiss:
                self.StartCoroutine(self.kiss_cr());
                break;
            case LevelProperties.SallyStagePlay.Pattern.Teleport:
                self.StartCoroutine(self.teleport_cr());
                break;
            default:
                yield return CupheadTime.WaitForSeconds(self, 1f);
                break;
        }
        yield break;
    }

    private IEnumerator jump_cr(On.SallyStagePlayLevel.orig_jump_cr orig, SallyStagePlayLevel self)
    {
        while (self.sally.state != SallyStagePlayLevelSally.State.Idle)
        {
            yield return null;
        }
        self.sally.OnJumpAttack();
        while (self.sally.state != SallyStagePlayLevelSally.State.Idle || waitForHorizontalAttack)//new
        //while (self.sally.state != SallyStagePlayLevelSally.State.Idle)
        {
            yield return null;
        }
        yield break;
    }

    protected void Awake(On.SallyStagePlayLevelSally.orig_Awake orig, SallyStagePlayLevelSally self)
    {
        orig(self);
        SallyStagePlayLevelWindow window = UnityEngine.Object.Instantiate<SallyStagePlayLevelWindow>(self.house.windowPrefab);
        YoMamaFat.sallyBottle = window.bottle;
        YoMamaFat.sallyRuler = window.ruler;
    }

    private IEnumerator fall_cr(On.SallyStagePlayLevelSally.orig_fall_cr orig, SallyStagePlayLevelSally self)
    {
        if (!waitForHorizontalAttack)
        {
            self.transform.SetPosition(0f, null, null);
        }
        yield return orig(self);
    }

    private IEnumerator jump_cr(On.SallyStagePlayLevelSally.orig_jump_cr orig, SallyStagePlayLevelSally self)
    {
        while (waitForHorizontalAttack)//new start
        {
            yield return null;
        }//new end
        yield return orig(self);
    }

    private IEnumerator diveKick_cr(On.SallyStagePlayLevelSally.orig_diveKick_cr orig, SallyStagePlayLevelSally self)
    {
        AbstractPlayerController player = PlayerManager.GetNext();//new start
        bool left = player.transform.position.x < 0f;
        float x = left ? 700f : -700f;
        Vector3 spawnPosition = new Vector3(x, -100f);
        float rotation = left ? 0f : 180f;
        SallyWindowProjectile.Create(YoMamaFat.sallyRuler, spawnPosition, rotation, 300f, player).transform.SetScale(2f, 2f, null);//new end
        yield return orig(self);
    }

    private IEnumerator jumpRoll_cr(On.SallyStagePlayLevelSally.orig_jumpRoll_cr orig, SallyStagePlayLevelSally self)
    {
        waitForHorizontalAttack = true;//new
        yield return self.animator.WaitForAnimationToEnd(self, "JumpRoll_Transition", true, true);
        if (!self.getOutOfJump)
        {
            self.StartCoroutine(self.rollAttack_cr());
            AudioManager.PlayLoop("sally_double_jump_roll_loop");
            self.emitAudioFromObject.Add("sally_double_jump_roll_loop");
        }
        Vector3 start = self.transform.position;
        Vector3 end = start + Vector3.up * self.properties.CurrentState.jumpRoll.RollJumpVerticalMovement;
        end += -self.transform.right * self.properties.CurrentState.jumpRoll.RollJumpHorizontalMovement.RandomFloat();
        if (end.x - self.bounds.x / 2f < (float)Level.Current.Left)
        {
            end.x = (float)Level.Current.Left + self.bounds.x / 2f;
        }
        else if (end.x + self.bounds.x / 2f > (float)Level.Current.Right)
        {
            end.x = (float)Level.Current.Right - self.bounds.x / 2f;
        }
        if (!self.getOutOfJump)//new start
        {
            end.x = Mathf.Sign(self.transform.position.x) * 500f;
        }//new end
        float pct = 0f;
        while (pct < self.properties.CurrentState.jumpRoll.JumpRollDuration)
        {
            self.transform.position = start + (end - start) * pct;
            pct += CupheadTime.Delta;
            yield return null;
        }
        yield return self.animator.WaitForAnimationToEnd(self, "JumpRoll_Roll", true, true);
        AudioManager.Stop("sally_double_jump_roll_loop");
        self.StartCoroutine(self.fall_cr());
        self.jumpRollAttackTypeIndex++;
        if (self.jumpRollAttackTypeIndex >= self.properties.CurrentState.jumpRoll.JumpAttackTypeString.Split(new char[]
        {
        ','
        }).Length)
        {
            self.jumpRollAttackTypeIndex = 0;
        }
        yield return horizontalAttack_cr(self);//new
        waitForHorizontalAttack = false;//new
        yield break;
    }

    private void SpawnHeart(On.SallyStagePlayLevelSally.orig_SpawnHeart orig, SallyStagePlayLevelSally self)
    {
        AbstractProjectile abstractProjectile = self.heartPrefab.Create(self.spawnPoints[0].position);
        /*bool isParryable = self.properties.CurrentState.kiss.heartType.Split(new char[]
        {
            ','
        })[self.heartTypeIndex][0] != 'R';*/
        bool isParryable = Rand.Bool();//new start
        if (!isParryable)
        {
            SpriteRenderer spriteRenderer = abstractProjectile.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.color = Color.black;
            }
        }//new end
        int direction = ((int)self.transform.eulerAngles.y != 180) ? 1 : -1;
        abstractProjectile.GetComponent<SallyStagePlayLevelHeart>().InitHeart(self.properties, direction, isParryable);
        abstractProjectile.GetComponent<Transform>().SetScale(new float?((float)((self.transform.right.x <= 0f) ? -1 : 1)), null, null);
        self.heartTypeIndex++;
        if (self.heartTypeIndex >= self.properties.CurrentState.kiss.heartType.Split(new char[]
        {
        ','
        }).Length)
        {
            self.heartTypeIndex = 0;
        }
        self.StartCoroutine(self.endKiss_cr());
    }

    private IEnumerator delay_cr(On.SallyStagePlayLevelSally.orig_delay_cr orig, SallyStagePlayLevelSally self)
    {
        Vector3 pos;
        pos.y = (float)Level.Current.Ceiling + self.teleportOffset;
        pos.z = 0f;
        self.animator.SetTrigger("OnTeleport");
        yield return self.animator.WaitForAnimationToStart(self, "Teleport_Loop", false);
        yield return CupheadTime.WaitForSeconds(self, self.properties.CurrentState.teleport.offScreenDelay);
        self.target = PlayerManager.GetNext();
        pos.x = self.target.center.x + (float)Parser.IntParse(self.properties.CurrentState.teleport.appearOffsetString.Split(new char[]
        {
        ','
        })[self.teleportOffsetIndex]);
        if (Parser.IntParse(self.properties.CurrentState.teleport.appearOffsetString.Split(new char[]
        {
        ','
        })[self.teleportOffsetIndex]) <= 0)
        {
            if (pos.x - 75f < (float)Level.Current.Left)
            {
                pos.x = (float)(Level.Current.Left + 75);
            }
            self.transform.right *= -1f;
        }
        else
        {
            if (pos.x + 75f > (float)Level.Current.Right)
            {
                pos.x = (float)(Level.Current.Right - 75);
            }
            self.transform.right *= 1f;
        }
        self.transform.position = pos;
        yield return teleportAttack_cr(self);//new
        self.StartCoroutine(self.fall_cr());
        yield return self.animator.WaitForAnimationToStart(self, "Idle", false);
        self.teleportOffsetIndex++;
        if (self.teleportOffsetIndex >= self.properties.CurrentState.teleport.appearOffsetString.Split(new char[]
        {
        ','
        }).Length)
        {
            self.teleportOffsetIndex = 0;
        }
        yield return CupheadTime.WaitForSeconds(self, self.properties.CurrentState.teleport.hesitate);
        self.transform.position = self.ground;
        self.isTeleporting = false;
        yield return null;
        yield break;
    }

    private IEnumerator slide_cr(On.SallyStagePlayLevelSally.orig_slide_cr orig, SallyStagePlayLevelSally self)
    {
        float startPos = 0f;
        float endPos = 0f;
        //float appearPos = 300f;
        float appearPos = 100f;//new
        AbstractPlayerController player = PlayerManager.GetPlayer(PlayerId.PlayerOne);
        AbstractPlayerController player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
        if (player2 == null || player.IsDead || player2.IsDead)
        {
            if (self.target == null || self.target.IsDead)
            {
                self.target = PlayerManager.GetNext();
            }
            if (self.target.transform.position.x > 0f)
            {
                if (self.transform.right.x > 0f)
                {
                    self.transform.right *= -1f;
                }
                startPos = -840f;
                endPos = -640f + appearPos;
            }
            else
            {
                if (self.transform.right.x < 0f)
                {
                    self.transform.right *= -1f;
                }
                startPos = 840f;
                endPos = 640f - appearPos;
            }
        }
        else
        {
            float num = -640f - player.transform.position.x;
            float num2 = 640f - player.transform.position.x;
            float num3 = -640f - player2.transform.position.x;
            float num4 = 640f - player2.transform.position.x;
            if (player.transform.position.x < 0f)
            {
                if (player2.transform.position.x < 0f)
                {
                    if (self.transform.right.x < 0f)
                    {
                        self.transform.right *= -1f;
                    }
                    startPos = 840f;
                    endPos = 640f - appearPos;
                }
                else if (num < num4)
                {
                    if (self.transform.right.x < 0f)
                    {
                        self.transform.right *= -1f;
                    }
                    startPos = 840f;
                    endPos = 640f - appearPos;
                }
                else
                {
                    if (self.transform.right.x > 0f)
                    {
                        self.transform.right *= -1f;
                    }
                    startPos = -840f;
                    endPos = -640f + appearPos;
                }
            }
            else if (player2.transform.position.x > 0f)
            {
                if (self.transform.right.x > 0f)
                {
                    self.transform.right *= -1f;
                }
                startPos = -840f;
                endPos = -640f + appearPos;
            }
            else if (num2 < num3)
            {
                if (self.transform.right.x < 0f)
                {
                    self.transform.right *= -1f;
                }
                startPos = 840f;
                endPos = 640f - appearPos;
            }
            else
            {
                if (self.transform.right.x > 0f)
                {
                    self.transform.right *= -1f;
                }
                startPos = -840f;
                endPos = -640f + appearPos;
            }
        }
        self.transform.position = new Vector3(startPos, self.transform.position.y, self.transform.position.z);
        float t = 0f;
        float time = 0.75f;
        YieldInstruction wait = new WaitForFixedUpdate();
        float frameTime = 0f;
        while (t < time)
        {
            t += CupheadTime.FixedDelta;
            frameTime += CupheadTime.FixedDelta;
            if (frameTime > 0.0416666679f)
            {
                frameTime -= 0.0416666679f;
                float t2 = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, t / time);
                self.transform.SetPosition(new float?(Mathf.Lerp(startPos, endPos, t2)), null, null);
            }
            yield return wait;
        }
        yield return null;
        yield break;
    }

    private IEnumerator teleportAttack_cr(SallyStagePlayLevelSally self)//new
    {
        bool attackOne = Rand.Bool();
        int attackCount = UnityEngine.Random.Range(3, 7);
        for (int i = 0; i < attackCount; i++)
        {
            float startI = attackOne ? -2f : -2.5f;
            TeleportAttack(2500f, 50f, startI);
            attackOne = !attackOne;
            yield return CupheadTime.WaitForSeconds(self, 0.3f);
        }
        yield return CupheadTime.WaitForSeconds(self, 0.4f);
        yield break;
    }

    private IEnumerator phase_2_cr(On.SallyStagePlayLevelSally.orig_phase_2_cr orig, SallyStagePlayLevelSally self)
    {
        YoMamaFat.parryOrbTransform.position = new Vector3(2137f, 2137f);//new
        yield return self.animator.WaitForAnimationToEnd(self, "Teleport_GONE", false, true);
        self.StartCoroutine(self.slide_cr());
        yield return self.animator.WaitForAnimationToStart(self, "Idle", false);
        self.isInvincible = false;
        self.getOutOfJump = false;
        yield return CupheadTime.WaitForSeconds(self, 1f);
        self.state = SallyStagePlayLevelSally.State.Idle;
        self.house.StartAttacks();
        yield return null;
        yield break;
    }

    private void SpawnProjectile(On.SallyStagePlayLevelSally.orig_SpawnProjectile orig, SallyStagePlayLevelSally self)
    {
        /*
        Vector3 v = self.target.transform.position - self.centerPoint.transform.position;
        SallyStagePlayLevelProjectile sallyStagePlayLevelProjectile = UnityEngine.Object.Instantiate<SallyStagePlayLevelProjectile>(self.projectilePrefab);
        sallyStagePlayLevelProjectile.Init(self.centerPoint.transform.position, MathUtils.DirectionToAngle(v), self.properties.CurrentState.projectile);*/
        float diff = self.transform.position.x < 0 ? -640f : 0f;//new start
        for (float x = 0; x < 640; x += 40f)
        {
            Vector3 v = new Vector3(x + diff, -250f) - self.centerPoint.transform.position;
            SallyStagePlayLevelProjectile sallyStagePlayLevelProjectile = UnityEngine.Object.Instantiate<SallyStagePlayLevelProjectile>(self.projectilePrefab);
            SallyProjectile.Init(sallyStagePlayLevelProjectile, self.centerPoint.transform.position, MathUtils.DirectionToAngle(v), self.properties.CurrentState.projectile);
        }//new end
    }

    protected void OnDestroy(On.SallyStagePlayLevelSally.orig_OnDestroy orig, SallyStagePlayLevelSally self)
    {
        if(YoMamaFat.parryOrbTransform != null)
        {
            YoMamaFat.parryOrbTransform.position = new Vector3(2137f, 2137f);//new
        }
        orig(self);
    }

    private void TeleportAttack(float speed, float delta, float startI)//new
    {
        float x = 0;
        float y = 360f;
        for (float i = startI; i < 3f; i++)
        {
            float deltaX = i * delta;
            float deltaY = Mathf.Abs(i) * delta;
            float rotation = i * 25f - 90f;
            SallyWindowProjectile.CreateAndWait(YoMamaFat.sallyBottle, new Vector3(x, y), rotation, speed);
        }

        for(float i = 400f; i < 800f; i += 75f)
        {
            SallyWindowProjectile.CreateAndWait(YoMamaFat.sallyBottle, new Vector3(i, y), -90f, speed);
            SallyWindowProjectile.CreateAndWait(YoMamaFat.sallyBottle, new Vector3(-i, y), -90f, speed);
        }
    }

    private IEnumerator horizontalAttack_cr(SallyStagePlayLevelSally self)//new
    {
        bool left = self.transform.position.x < 0;
        float speed = -1000f;
        float x = left ? -750f : 750f;
        float rotation = left ? 180f : 0f;
        float orbX = left ? 500f : -500f;
        YoMamaFat.parryOrbTransform.position = new Vector3(orbX, 100f);
        YoMamaFat.parryOrb.parrySwitch.enabled = true;
        yield return CupheadTime.WaitForSeconds(self, 1f);
        int sumOfEmptySpaceIndexes = 0;
        for (int wave = 0; wave < 3; wave++)
        {
            int emptySpaceIndex = UnityEngine.Random.Range(-1, 4);
            sumOfEmptySpaceIndexes += emptySpaceIndex;
            if (sumOfEmptySpaceIndexes > 6)
            {
                emptySpaceIndex = UnityEngine.Random.Range(-1, 2);
            }
            float emptySpace = emptySpaceIndex * 80f + 40f;
            for (float y = -360f; y < 800f; y += 80f)
            {
                if (y != emptySpace)
                {
                    SallyStagePlayLevelWindowProjectile projectile = YoMamaFat.sallyBottle.Create(new Vector3(x, y), rotation, speed, YoMamaFat.sallyParent);
                    projectile.transform.SetScale(-4f, null, null);
                }
            }
            yield return CupheadTime.WaitForSeconds(self, 1.1f);
        }
        yield return CupheadTime.WaitForSeconds(self, 1f);
        YoMamaFat.parryOrbTransform.position = new Vector3(2137, 2137f);
        yield break;
    }

    private bool waitForHorizontalAttack = false;
}
