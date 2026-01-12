using Rewired;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static PlanePlayerWeaponManager.States;
using static Rewired.ComponentControls.Effects.RotateAroundAxis;

namespace CupaGoovno;

public class StatsManager
{
    public void Init()
    {
        On.PlayerStatsManager.CalculateHealthMax += CalculateHealthMax;
        On.PlayerStatsManager.SuperChangedFromParry += SuperChangedFromParry;
        On.PlayerStatsManager.HealerCharm += HealerCharm;
        On.PlayerStatsManager.OnParry += OnParry;
        On.PlayerStatsManager.LevelInit += LevelInit;
        On.PlayerStatsManager.OnDestroy += OnDestroy;
        On.PlayerStatsManager.hit_cr += hit_cr;
        On.PlayerStatsManager.emptySuper_cr += emptySuper_cr;
        On.PlayerStatsManager.OnDealDamage += OnDealDamage;
    }

    private void CalculateHealthMax(On.PlayerStatsManager.orig_CalculateHealthMax orig, PlayerStatsManager self)
    {
        self.HealthMax = 3;
        if (!Level.IsChessBoss)
        {
            switch (self.Loadout.charm)
            {
                case Charm.charm_smoke_dash:
                    self.HealthMax = 2137;
                    break;
                case Charm.charm_health_up_1:
                    self.HealthMax = 1;
                    break;
                case Charm.charm_chalice:
                    if(SceneManager.GetActiveScene().name != "scene_level_dancers_rift")
                    {
                        self.HealthMax = 4;
                    }
                    break;
            }

            Charm casualMode = CustomCharms.casualMode;
            if (self.Loadout.charm == casualMode)
            {
                self.HealthMax = 30;
            }

            self.HealthMax += self.HealerHP;

            if (Level.IsInBossesHub)
            {
                PlayersStatsBossesHub playerStats = Level.GetPlayerStats(self.basePlayer.id);
                if (playerStats != null)
                {
                    self.HealthMax += playerStats.BonusHP;
                }
            }
        }
    }

    public void SuperChangedFromParry(On.PlayerStatsManager.orig_SuperChangedFromParry orig, PlayerStatsManager self, float multiplier)
    {
        bool damagedWithCursedHeart = self.Loadout.charm == Charm.charm_health_up_2 && multiplier == 5f;

        if (self.CanGainSuperMeter || (damagedWithCursedHeart && !Level.IsChessBoss))
        {
            if (self.CanGainSuperMeter || damagedWithCursedHeart)
            //if (this.CanGainSuperMeter)
            {
                self.SuperMeter += 10f * multiplier;
                self.OnSuperChanged(true);
            }
        }

        if (damagedWithCursedHeart && !Level.IsChessBoss)
        {
            CursedHeartAttack(self);
        }

        if (!damagedWithCursedHeart && self.Loadout.charm == CustomCharms.milk)
        {
            CharmMilk.charged = true;
        }

        if (!damagedWithCursedHeart)
        {
            parried = true;
        }
    }

    private void CursedHeartAttack(PlayerStatsManager self)//new
    {
        AbstractPlayerController player = PlayerManager.GetPlayer(self.basePlayer.id);
        LevelPlayerWeaponManager levelWeaponManager = player.GetComponent<LevelPlayerWeaponManager>();
        if (levelWeaponManager != null)
        {
            if (player.stats.Loadout.super != Super.None)
            {
                if (!CustomSupers.IsCustomSuper(player.stats.Loadout.super, levelWeaponManager))//new start
                {
                    levelWeaponManager.StartSuper();
                }//new end
            }
        }
        else
        {
            PlanePlayerWeaponManager planeWeaponManager = player.GetComponent<PlanePlayerWeaponManager>();
            if (planeWeaponManager != null)
            {
                planeWeaponManager.StartSuper();
            }
        }
    }

    private void HealerCharm(On.PlayerStatsManager.orig_HealerCharm orig, PlayerStatsManager self)
    {
        /*int num = self.HealerHPReceived + 1;
        if (self.Loadout.charm == Charm.charm_curse)
        {
            num = CharmCurse.GetHealerInterval(self.CurseCharmLevel, self.HealerHPReceived);
        }*/
        self.HealerHPCounter++;
        int healInterval = 3; //new start
        if (self.Loadout.charm == Charm.charm_curse)
        {
            healInterval = 5;
        }
        else
        {
            YoMamaFat.healerParryCounter++;
            UpdateDamageCounter(self.basePlayer.id);
        }
        if (self.HealerHPCounter >= healInterval)//new end
        //if (self.HealerHPCounter >= num)
        {
            self.HealerHP++;
            self.HealerHPReceived++;
            self.SetHealth(self.Health + 1);
            self.OnHealthChanged();
            self.HealerHPCounter = 0;
            LevelPlayerController levelPlayerController = self.basePlayer as LevelPlayerController;
            PlanePlayerController planePlayerController = self.basePlayer as PlanePlayerController;
            if (levelPlayerController != null)
            {
                levelPlayerController.animationController.OnHealerCharm();
            }
            else if (planePlayerController != null)
            {
                planePlayerController.animationController.OnHealerCharm();
            }
        }
    }

    public void OnParry(On.PlayerStatsManager.orig_OnParry orig, PlayerStatsManager self, float multiplier = 1f, bool countParryTowardsScore = true)
    {
        if(parryCooldown)//new start
        {
            return;
        }
        else if (self.basePlayer.stats.Loadout.charm == Charm.charm_parry_plus)
        {
            parryCooldown = true;
            self.StartCoroutine(parryCooldown_cr(self));
        }//new end

        if ((self.Loadout.charm == Charm.charm_healer || (self.Loadout.charm == Charm.charm_curse && self.CurseCharmLevel >= 0)) && !Level.IsChessBoss)
        {
            //if (self.HealerHPReceived < 3)
            //{
            self.HealerCharm();
            /*}
            else
            {
                self.SuperChangedFromParry(multiplier);
            }*/
        }
        else
        {
            self.SuperChangedFromParry(multiplier);
        }
        if (countParryTowardsScore && !Level.Current.Ending)
        {
            Level.ScoringData.numParries++;
        }
        OnlineManager.Instance.Interface.IncrementStat(self.basePlayer.id, "Parries", 1);
        if (Level.Current.CurrentLevel != Levels.Tutorial && Level.Current.CurrentLevel != Levels.ShmupTutorial && (Level.Current.playerMode == PlayerMode.Level || Level.Current.playerMode == PlayerMode.Arcade))
        {
            self.ParriesThisJump++;
            if (self.ParriesThisJump > PlayerData.Data.GetNumParriesInRow(self.basePlayer.id))
            {
                PlayerData.Data.SetNumParriesInRow(self.basePlayer.id, self.ParriesThisJump);
            }
            if (self.ParriesThisJump == 5)
            {
                OnlineManager.Instance.Interface.UnlockAchievement(self.basePlayer.id, "ParryChain");
            }
        }
        if (self.SuperMeter == self.SuperMeterMax)
        {
            AudioManager.Play("player_parry_power_up_full");
        }
        else
        {
            AudioManager.Play("player_parry_power_up");
        }
    }

    private IEnumerator parryCooldown_cr(PlayerStatsManager self)
    {
        yield return CupheadTime.WaitForSeconds(self, 0.1f);
        parryCooldown = false;
    }

    public void LevelInit(On.PlayerStatsManager.orig_LevelInit orig, PlayerStatsManager self)
    {
        orig(self);

        if(!Level.IsDicePalace ||
            (Level.IsDicePalaceMain && DicePalaceMainLevelGameInfo.PLAYER_SPACES_MOVED == 0))
        {
            YoMamaFat.healerParryCounter = 0;
            YoMamaFat.oneHeartDamageBoost = 0;
            CharmBalance.boostDifference = 0f;
        }

        CharmMilk.charged = false;
        SuperBerserker.active = false;
        SuperSandevistan.state = SuperSandevistan.State.Inactive;
        SuperSandevistan.multiplier = 1f;
        parryCooldown = false;
        parried = false;
        CreateDamageCounter(self.basePlayer.id);
        if (self.Loadout.charm == Charm.charm_curse)
        {
            Time.timeScale = 1.2f;
        }
        if(self.Loadout.charm == Charm.charm_health_up_1)
        {
            self.StartCoroutine(oneHeart_cr(self));
        }
        
        bool isPlane = self.basePlayer is PlanePlayerController;
        bool homingWeapon = self.Loadout.primaryWeapon == Weapon.level_weapon_homing ||
            self.Loadout.secondaryWeapon == Weapon.level_weapon_homing;
        if (isPlane || homingWeapon)
        {
            self.basePlayer.StartCoroutine(homingWeaponCursor_cr(self.basePlayer));
        }
    }

    protected void OnDestroy(On.PlayerStatsManager.orig_OnDestroy orig, PlayerStatsManager self)
    {
        orig(self);
        Time.timeScale = 1f;
        //UnityEngine.GameObject.Destroy(damageMultiplierUICanvas);
        //UnityEngine.GameObject.Destroy(damageMultiplierUIText);
    }

    private IEnumerator hit_cr(On.PlayerStatsManager.orig_hit_cr orig, PlayerStatsManager self)
    {
        if(self.Loadout.charm == Charm.charm_health_up_2)
        {
            self.SuperChangedFromParry(5f);
        }
        if(Level.Current is MausoleumLevel mausoleumLevel)
        {
            mausoleumLevel.Failure();
        }
        yield return orig(self);
    }

    public static void UpdateDamageCounter(PlayerId playerId)//new
    {
        if (Settings.hideDamageMultiplier)
        {
            return;
        }

        float damageMultiplier = Mathf.Round(Other.GetDamageMultiplier(false, playerId) * 100);
        if (!damageMultiplierUITextDic.ContainsKey(playerId) 
            || damageMultiplierUITextDic[playerId] == null 
            || damageMultiplierUICanvas == null)
        {
            if(damageMultiplier != 100)
            {
                CreateDamageCounter(playerId);
            }
        }
        else
        {
            if(damageMultiplier == 100)
            {
                //GameObject.Destroy(damageMultiplierUICanvas);
                GameObject.Destroy(damageMultiplierUITextDic[playerId]);
            }
            else if (damageMultiplier >= 0)
            {
                damageMultiplierUITextDic[playerId].text = "Damage multipier: " + damageMultiplier + "%";
            }
            else
            {
                damageMultiplierUITextDic[playerId].text = "Now you're healing the boss lmao";
            }
        }
    }

    private static void CreateDamageCounter(PlayerId playerId)//new
    {
        if (Settings.hideDamageMultiplier)
        {
            return;
        }

        float damageMultiplier = Mathf.Round(Other.GetDamageMultiplier(false, playerId) * 100);
        if (damageMultiplier == 100)
        {
            return;
        }

        Canvas canvas;
        if (damageMultiplierUICanvas == null)
        {
            damageMultiplierUICanvas = new GameObject("SimpleCanvas");
            canvas = damageMultiplierUICanvas.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            damageMultiplierUICanvas.AddComponent<CanvasScaler>();
            damageMultiplierUICanvas.AddComponent<GraphicRaycaster>();
        }
        else
        {
            canvas = damageMultiplierUICanvas.GetComponent<Canvas>();
        }

        GameObject textObject = new("SimpleText");
        textObject.transform.SetParent(canvas.transform);

        damageMultiplierUITextDic[playerId] = textObject.AddComponent<UnityEngine.UI.Text>();
        damageMultiplierUITextDic[playerId].text = "Damage multipier: " + damageMultiplier + "%";
        damageMultiplierUITextDic[playerId].fontSize = 24;
        damageMultiplierUITextDic[playerId].color = new Color(1, 1, 1, 0.5f);
        damageMultiplierUITextDic[playerId].font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        damageMultiplierUITextDic[playerId].fontStyle = FontStyle.Bold;
        damageMultiplierUITextDic[playerId].alignment = TextAnchor.UpperLeft;

        RectTransform rectTransform = damageMultiplierUITextDic[playerId].GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(600, 200);
        rectTransform.anchorMin = new Vector2(0f, 1f);
        rectTransform.anchorMax = new Vector2(0f, 1f);
        rectTransform.pivot = new Vector2(0f, 1f);
        rectTransform.anchoredPosition = new Vector2(10, playerId == PlayerId.PlayerOne ? -10 : -35);

        Shadow textShadow = textObject.AddComponent<Shadow>();
        textShadow.effectColor = new Color(0, 0, 0, 0.2f);
        textShadow.effectDistance = new Vector2(2, -2);
    }

    private IEnumerator oneHeart_cr(PlayerStatsManager self)
    {
        for(; ; )
        {
            yield return CupheadTime.WaitForSeconds(self, 1f);
            if(!Level.IsDicePalaceMain)
            {
                YoMamaFat.oneHeartDamageBoost+=2;
                UpdateDamageCounter(self.basePlayer.id);
            }
        }
    }

    private IEnumerator emptySuper_cr(On.PlayerStatsManager.orig_emptySuper_cr orig, PlayerStatsManager self)
    {
        LevelPlayerWeaponManager weaponManager = self.GetComponent<LevelPlayerWeaponManager>();//new start
        if(weaponManager != null)
        {
            weaponManager.DisableInput();
        }//new end
        //while (self.SuperMeter > 0f)
        while (self.SuperMeter > 20f)//new
        {
            //self.SuperMeter -= self.SuperMeterMax * CupheadTime.Delta / WeaponProperties.LevelSuperInvincibility.durationFX;
            self.SuperMeter -= 30f * CupheadTime.Delta / WeaponProperties.LevelSuperInvincibility.durationFX;//new
            //self.OnSuperChanged(true);
            if(self.SuperMeter > 20f)//new start
            {
                self.OnSuperChanged(true);
            }//new end
            yield return null;
        }
        //self.SuperMeter = 0f;
        self.SuperMeter = 20f;//new start
        if(weaponManager != null)
        {
            weaponManager.EnableInput();
        }//new end
        self.OnSuperChanged(true);
        yield break;
    }

    public void OnDealDamage(On.PlayerStatsManager.orig_OnDealDamage orig, PlayerStatsManager self, float damage, DamageDealer dealer)
    {
        damage *= dealer.DamageMultiplier;
        CharmBalance.TryUpdate(damage);
        if(self.basePlayer is LevelPlayerController levelPlayer)
        {
            if (SuperSandevistan.Active)
            {
                float maxSuper = levelPlayer.stats.SuperMeterMax;
                float maxDamage = new CustomSuperProperties.Sandevistan().maxDamage;
                levelPlayer.stats.SuperMeter -= maxSuper * damage / maxDamage;
            }

            Weapon main = levelPlayer.stats.Loadout.primaryWeapon;
            Weapon secondary = levelPlayer.stats.Loadout.secondaryWeapon;
            Weapon current = levelPlayer.weaponManager.currentWeapon;
            if ((main == CustomWeapons.peeshooter && main != current) || 
                (secondary == CustomWeapons.peeshooter && secondary != current))
            {
                Collider2D playerCollider = self.basePlayer.GetComponent<Collider2D>();
                ContactFilter2D filter = new ContactFilter2D();
                filter.SetLayerMask(Physics2D.DefaultRaycastLayers);
                filter.useLayerMask = true;

                Collider2D[] collisions = new Collider2D[100];
                int count = playerCollider.OverlapCollider(filter, collisions);

                for(int i = 0; i < count; i++)
                {
                    if (collisions[i].gameObject.name.Contains("Pee EX"))
                    {
                        damage *= 2f;
                        break;
                    }
                }
            }
            else if (dealer.origin.gameObject.name.Contains("Pee Basic"))
            {
                damage *= 5f;
            }
        }

        orig(self, damage, dealer);
    }

    private IEnumerator homingWeaponCursor_cr(MonoBehaviour self)
    {
        homingWeaponCursor = new GameObject("HomingWeaponCursor");
        homingWeaponCursor.transform.SetPosition(0f, 0f);
        homingWeaponCursorCollider = homingWeaponCursor.AddComponent<CircleCollider2D>();

        SpriteRenderer homingWeaponCursorSr = homingWeaponCursor.AddComponent<SpriteRenderer>();
        homingWeaponCursorSr.sprite = homingWeaponCursorSprite;
        homingWeaponCursorSr.sortingLayerName = "UI";
        homingWeaponCursorSr.enabled = false;

        PlayerInput input = Other.Player().input;
        float speed = 500f;
        for (; ; )
        {
            if (input.GetButton(CupheadButton.Lock))
            {
                float horizontal = Other.Player().input.GetAxis(PlayerInput.Axis.X);
                float vertical = Other.Player().input.GetAxis(PlayerInput.Axis.Y);
                if (horizontal != 0f || vertical != 0f)
                {
                    homingWeaponCursor.transform.AddPosition(
                        horizontal * CupheadTime.delta * speed,
                        vertical * CupheadTime.delta * speed);

                    if (!homingWeaponCursorSr.enabled)
                    {
                        homingWeaponCursorSr.enabled = true;
                    }
                }
            }
            
            yield return null;
        }
    }

    public static bool parried;
    public static GameObject homingWeaponCursor;
    public static Collider2D homingWeaponCursorCollider;
    public static Sprite homingWeaponCursorSprite;

    private static GameObject damageMultiplierUICanvas;
    private static Dictionary<PlayerId, UnityEngine.UI.Text> damageMultiplierUITextDic = [];

    private static bool parryCooldown = false;
}
