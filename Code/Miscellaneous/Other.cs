using Blender.Utility;
using On;
using RektTransform;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Policy;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CupaGoovno;

public class Other
{
    public void Init()
    {
        On.PlayerDamageReceiver.TakeDamage += TakeDamage;
        On.PlayerDamageReceiver.Update += Update;
        On.SceneLoader.loop_cr += loop_cr;
        On.SceneLoader.out_cr += out_cr;
        On.AbstractEquipUI.OnPause += OnPause;
        On.LevelGameOverGUI.In += In;
        On.Level.zHack_OnWin += zHack_OnWin;
    }

    public void TakeDamage(On.PlayerDamageReceiver.orig_TakeDamage orig, PlayerDamageReceiver self, DamageDealer.DamageInfo info)
    {
        if (self.player.stats.SuperInvincible)
        {
            return;
        }
        if (info.damage > 0f)
        {
            self.HandleChaliceShmupSuper(info);
            if (!self.enabled)
            {
                return;
            }
            if (info.damageSource == DamageDealer.DamageSource.Pit)
            {
                if (self.player.damageReceiver.state != PlayerDamageReceiver.State.Vulnerable)
                {
                    return;
                }
            }
            else if (self.timer < 2f)//new
            {//new
                if (!self.player.CanTakeDamage)
                {
                    return;
                }
                if (self.timer > 0f)
                {
                    return;
                }
            }//new
            float num = 1f;
            self.Invulnerable(2f * num);
            self.TakeDamageBruteForce(info);
            SuperBerserker.achievementTimer = 0f;//new
            if (self.player.stats.ChaliceShieldOn)
            {
                self.player.stats.SetChaliceShield(false);
            }
        }
        else if (info.stoneTime > 0f)
        {
            self.TakeDamageBruteForce(info);
        }
    }

    public void Update(On.PlayerDamageReceiver.orig_Update orig, PlayerDamageReceiver self)
    {
        if (self.state != PlayerDamageReceiver.State.Invulnerable)
        {
            return;
        }
        if (self.timer > 0f)
        {
            self.timer -= CupheadTime.Delta / SuperSandevistan.multiplier;//new
            //self.timer -= CupheadTime.Delta;
            if (self.timer <= 0f)
            {
                self.Vulnerable();
            }
        }
    }

    public static float GetDamageMultiplier(bool ignoreBalancedKnife = false, PlayerId playerId = PlayerId.PlayerOne)
    {
        //float num = PlayerManager.DamageMultiplier;
        float multiplier = PlayerManager.Count > 1 && Level.Current is not MoaiLevel ? 0.5f : 1f;
        try
        {
            AbstractPlayerController player = PlayerManager.GetPlayer(playerId);
            Charm charm = player.stats.Loadout.charm;
            if (charm == Charm.charm_smoke_dash)
            {
                multiplier *= 5f;
            }
            else if (charm == Charm.charm_chalice)
            {
                multiplier *= 0.75f;
            }
            else if (charm == Charm.charm_healer)
            {
                multiplier *= 1f - (YoMamaFat.healerParryCounter * 0.03f);
            }
            else if (charm == Charm.charm_health_up_1)
            {
                multiplier *= 1f + (YoMamaFat.oneHeartDamageBoost * 0.01f);
            }
            else if (charm == CustomCharms.balance)
            {
                if (ignoreBalancedKnife)
                {
                    multiplier *= 1f + new CustomCharmProperties.Balance().passiveBoost;
                }
                else
                {
                    multiplier *= CharmBalance.GetDamageMultiplier(player);
                }
            }
            if (SuperBerserker.active)
            {
                multiplier *= CustomSuperProperties.Berserker.damageMultiplier;
            }
        }
        catch{}
        return multiplier;
    }

    public static MeshRenderer SetTransparentMaterial(GameObject stuff, UnityEngine.Color color)
    {
        MeshRenderer meshRenderer = stuff.GetComponent<MeshRenderer>();
        meshRenderer.material = new(Shader.FindObjectOfType<Material>());
        meshRenderer.material.color = color;
        meshRenderer.material.SetFloat("_Mode", 3);
        meshRenderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        meshRenderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        meshRenderer.material.SetInt("_ZWrite", 0);
        meshRenderer.material.DisableKeyword("_ALPHATEST_ON");
        meshRenderer.material.EnableKeyword("_ALPHABLEND_ON");
        meshRenderer.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        meshRenderer.material.renderQueue = 3000;
        return meshRenderer;
    }

    public static UnityEngine.UI.Image CreateScreenOverlay()
    {
        GameObject canvasGO = new GameObject("OverlayCanvas");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();


        GameObject overlay = new("ScreenOverlay");
        overlay.transform.SetParent(canvas.transform, false);

        RectTransform rect = overlay.AddComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        UnityEngine.UI.Image image = overlay.AddComponent<UnityEngine.UI.Image>();
        image.color = new(0f, 0f, 0f, 0f);

        return image;
    }

    public static IEnumerator notification_cr(MonoBehaviour self, string tText, string mText, float time, float height, bool forceNotification)
    {
        if(Settings.hideNotifications && !forceNotification)
        {
            yield break;
        }

        GameObject notification = GameObject.Find("Notification");
        while(notification != null)
        {
            yield return null;
        }

        GameObject canvasObject = new GameObject("Notification");
        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObject.AddComponent<CanvasScaler>();
        canvasObject.AddComponent<GraphicRaycaster>();

        GameObject bg = new GameObject("Background");
        bg.transform.SetParent(canvas.transform, false);
        UnityEngine.UI.Image bgImage = bg.AddComponent<UnityEngine.UI.Image>();
        bgImage.color = new UnityEngine.Color(0f, 0f, 0f, 0.5f);

        RectTransform bgRect = bg.GetComponent<RectTransform>();
        bgRect.sizeDelta = new Vector2(710f, height);
        bgRect.anchorMin = new Vector2(0f, 0.7f);
        bgRect.anchorMax = new Vector2(0f, 0.7f);
        bgRect.anchoredPosition = new Vector2(-100f, 100f - height / 2f);

        GameObject title = new GameObject("Title");
        title.transform.SetParent(canvas.transform, false);

        Text titleText = title.AddComponent<Text>();
        titleText.text = tText;
        titleText.fontSize = 36;
        titleText.color = UnityEngine.Color.yellow;
        titleText.font = Resources.GetBuiltinResource<UnityEngine.Font>("Arial.ttf");
        titleText.fontStyle = UnityEngine.FontStyle.Bold;
        titleText.alignment = TextAnchor.UpperLeft;

        RectTransform titleRect = titleText.GetComponent<RectTransform>();
        titleRect.sizeDelta = new Vector2(480, 100);
        titleRect.anchorMin = new Vector2(0f, 0.7f);
        titleRect.anchorMax = new Vector2(0f, 0.7f);
        titleRect.anchoredPosition = new Vector2(5, 40);

        GameObject message = new GameObject("Message");
        message.transform.SetParent(canvas.transform, false);

        Text messageText = message.AddComponent<Text>();
        messageText.text = mText;
        messageText.fontSize = 24;
        messageText.color = UnityEngine.Color.white;
        messageText.font = Resources.GetBuiltinResource<UnityEngine.Font>("Arial.ttf");
        messageText.fontStyle = UnityEngine.FontStyle.Normal;
        messageText.alignment = TextAnchor.UpperLeft;

        RectTransform messageRect = messageText.GetComponent<RectTransform>();
        messageRect.sizeDelta = new Vector2(480, height);
        messageRect.anchorMin = new Vector2(0f, 0.7f);
        messageRect.anchorMax = new Vector2(0f, 0.7f);
        messageRect.anchoredPosition = new Vector2(5, 45 - height / 2);

        Vector2 start = new Vector2(-300f, 0f);
        bgRect.anchoredPosition += start;
        titleRect.anchoredPosition += start;
        messageRect.anchoredPosition += start;
        float bgStart = bgRect.anchoredPosition.x;
        float titleStart = titleRect.anchoredPosition.x;
        float messageStart = messageRect.anchoredPosition.x;
        float bgEnd = bgStart + 550f;
        float titleEnd = titleStart + 550f;
        float messageEnd = messageStart + 550f;

        float t = 0f;
        float maxT = 0.5f;
        while(t < maxT)
        {
            t += Time.deltaTime;
            float sin = Mathf.Sin(t / maxT * Mathf.PI / 2f);
            float bgX = Mathf.Lerp(bgStart, bgEnd, sin);
            bgRect.anchoredPosition = new Vector2(bgX, bgRect.anchoredPosition.y);
            float titleX = Mathf.Lerp(titleStart, titleEnd, sin);
            titleRect.anchoredPosition = new Vector2(titleX, titleRect.anchoredPosition.y);
            float messageX = Mathf.Lerp(messageStart, messageEnd, sin);
            messageRect.anchoredPosition = new Vector2(titleX, messageRect.anchoredPosition.y);
            yield return null;
        }
        yield return new WaitForSeconds(time);
        t = 0f;
        maxT = 0.5f;
        while (t < maxT)
        {
            t += Time.deltaTime;
            float sin = Mathf.Sin(t / maxT * Mathf.PI / 2f);
            float bgX = Mathf.Lerp(bgEnd, bgStart, sin);
            bgRect.anchoredPosition = new Vector2(bgX, bgRect.anchoredPosition.y);
            float titleX = Mathf.Lerp(titleEnd, titleStart, sin);
            titleRect.anchoredPosition = new Vector2(titleX, titleRect.anchoredPosition.y);
            float messageX = Mathf.Lerp(messageEnd, messageStart, sin);
            messageRect.anchoredPosition = new Vector2(titleX, messageRect.anchoredPosition.y);
            yield return null;
        }
        GameObject.Destroy(canvasObject);
        yield break;
    }

    private IEnumerator loop_cr(On.SceneLoader.orig_loop_cr orig, SceneLoader self)
    {
        SceneLoader.currentlyLoading = true;
        yield return self.StartCoroutine(self.in_cr());
        self.StartCoroutine(self.load_cr());
        yield return self.StartCoroutine(self.iconFadeIn_cr());
        while (!self.doneLoadingSceneAsync)
        {
            yield return null;
        }
        if (SceneLoader.SceneName != Scenes.scene_slot_select.ToString())
        {
            AudioManager.SnapshotReset(SceneLoader.SceneName, 0.15f);
        }
        AsyncOperation op = Resources.UnloadUnusedAssets();
        while (!op.isDone)
        {
            yield return null;
        }
        if (SceneLoader.SceneName.Contains("scene_level"))//new start
        {
            self.ResetBgmVolume();
            SceneLoader.properties.Reset();
            SceneLoader.currentlyLoading = false;
        }//new end
        yield return self.StartCoroutine(self.iconFadeOut_cr());
        yield return self.StartCoroutine(self.out_cr());
        if (!SceneLoader.SceneName.Contains("scene_level"))//new
        {//new
            SceneLoader.properties.Reset();
            SceneLoader.currentlyLoading = false;
        }//new
        yield break;
    }

    private IEnumerator out_cr(On.SceneLoader.orig_out_cr orig, SceneLoader self)
    {
        yield return orig(self);
        if (SceneLoader.properties.transitionEnd == SceneLoader.Transition.None)
        {
            self.SetFaderAlpha(1f);
            self.StartCoroutine(noTransitionDelay_cr(self));
        }
    }


    private void OnPause(On.AbstractEquipUI.orig_OnPause orig, AbstractEquipUI self)
    {
        orig(self);
        if (!showedEqNotification)
        {
            string tText = "New stuff!";
            string mText = "Go further down the weapon/super/charm list to find new items. (just open the list and press down twice)";
            self.StartCoroutine(notification_cr(self, tText, mText, 5f, 150f, false));
            showedEqNotification = true;
        }
    }

    public void In(On.LevelGameOverGUI.orig_In orig, LevelGameOverGUI self, bool secretTriggered)
    {
        if(Level.Current is SlimeLevel slimeLevel)
        {
            self.gameObject.SetActive(true);
            self.bossPortraitImage.sprite = Slime.GetSlimeSprite(slimeLevel);
            if (secretTriggered)
            {
                self.cardCanvasGroup.GetComponent<UnityEngine.UI.Image>().sprite = self.timelineSecret;
                self.timelineObj.SetActive(false);
            }
            if (self.bossQuoteLocalization == null)
            {
                self.bossQuoteText.text = "\"" + Level.Current.BossQuote + "\"";
            }
            else
            {
                self.bossQuoteLocalization.ApplyTranslation(Localization.Find(Level.Current.BossQuote), null);
                if (Localization.language == Localization.Languages.Korean)
                {
                    self.bossQuoteLocalization.textMeshProComponent.fontStyle = FontStyles.Bold;
                }
            }
            if (self.bossPortraitImage.sprite != null)
            {
                self.bossPortraitImage.rectTransform.SetSize(self.bossPortraitImage.sprite.rect.width, self.bossPortraitImage.sprite.rect.height);
            }
            self.StartCoroutine(self.in_cr());
        }
        else
        {
            orig(self, secretTriggered);
        }
    }

    private IEnumerator noTransitionDelay_cr(SceneLoader self)//new
    {
        yield return CupheadTime.WaitForSeconds(self, 1f);
        self.SetFaderAlpha(0f);
    }

    public static float VolumeToMultiplier(float volume)
    {
        if(volume == -80f)
        {
            return 0f;
        }
        else
        {
            return Mathf.Pow(10f, volume / 20f);
        }
    }

    public static float GetMusicVolumeMultiplier()
    {
        float masterVolume = Other.VolumeToMultiplier(AudioManager.masterVolume);
        float musicVolume = Other.VolumeToMultiplier(AudioManager.bgmOptionsVolume);
        return masterVolume * musicVolume;
    }

    public static float GetSfxVolumeMultiplier()
    {
        float masterVolume = Other.VolumeToMultiplier(AudioManager.masterVolume);
        float sfxVolume = Other.VolumeToMultiplier(AudioManager.sfxOptionsVolume);
        return masterVolume * sfxVolume;
    }

    public static void MakePlayersFatherless()
    {
        Dictionary<int, AbstractPlayerController>.ValueCollection players = PlayerManager.GetAllPlayers();
        foreach(AbstractPlayerController player in players)
        {
            if(player != null)
            {
                player.transform.parent = null;
            }
        }
    }

    public static void FixShader(GameObject obj)
    {
        SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
        Shader errorShader = Shader.Find("Hidden/InternalErrorShader");
        Shader defaultShader = Shader.Find("Sprites/Default");
        if (spriteRenderer != null && spriteRenderer.material.shader == errorShader)
        {
            spriteRenderer.material = new Material(defaultShader);
        }
    }

    public static AbstractPlayerController Player()
    {
        if (player == null)
        {
            player = PlayerManager.GetFirst();
        }
        return player;
    }

    public void zHack_OnWin(On.Level.orig_zHack_OnWin orig, Level self)
    {
        orig(self);
        AbstractPlayerController player = PlayerManager.GetFirst();
        if(player != null)
        {
            PlayerData.PlayerLoadouts.PlayerLoadout loadout = player.stats.Loadout;
            if(player is LevelPlayerController)
            {
                Weapon primary = loadout.primaryWeapon;
                Weapon secondary = loadout.secondaryWeapon;
                if ((primary == CustomWeapons.striker && secondary == Weapon.level_weapon_crackshot) ||
                    (primary == Weapon.level_weapon_crackshot && secondary == CustomWeapons.striker))
                {
                    CustomAchievements.Unlock(CustomAchievements.Achievements.BuiltDifferent);
                }

                if (loadout.super == Super.level_super_invincible && player.stats.SuperInvincible)
                {
                    CustomAchievements.Unlock(CustomAchievements.Achievements.Invincible);
                }

                if (Level.Current is DicePalaceMainLevel && player.stats.Health == 9)
                {
                    CustomAchievements.Unlock(CustomAchievements.Achievements.Dice);
                }
            }
           
            if(loadout.charm == Charm.charm_curse)
            {
                CustomAchievements.Unlock(CustomAchievements.Achievements.Relic);
            }
        }

        if(Level.Current is FlyingBirdLevel && !StatsManager.parried)
        {
            CustomAchievements.Unlock(CustomAchievements.Achievements.Bird);
        }
    }

    public static void LoadCustomAsset(string path)
    {
        AssetHelper.AddPersistentPath(AssetHelper.LoaderType.Single, path);
    }

    public static AudioSource PlayCustomSfx(AudioClip audioClip, float multiplier)
    {
        AudioSource audioSource = levelSelf.gameObject.AddComponent<AudioSource>();
        PlayClip(audioSource, audioClip, multiplier);
        return audioSource;
    }

    public static AudioSource PlayCustomSfx(string path, float multiplier)
    {
        AudioSource audioSource = levelSelf.gameObject.AddComponent<AudioSource>();
        AudioClip audioClip = AssetLoader<UnityEngine.Object>.GetCachedAsset(path) as AudioClip;
        PlayClip(audioSource, audioClip, multiplier);
        return audioSource;
    }

    private static void PlayClip(AudioSource audioSource, AudioClip audioClip, float multiplier)
    {
        if (audioSource != null && audioClip != null)
        {
            audioSource.clip = audioClip;
            audioSource.volume = multiplier * GetSfxVolumeMultiplier();
            audioSource.Play();
        }
        
        levelSelf.StartCoroutine(customSfx_cr(audioSource));
    }

    private static IEnumerator customSfx_cr(AudioSource audioSource)
    {
        yield return CupheadTime.WaitForSeconds(levelSelf, 30f);
        if(audioSource != null)
        {
            GameObject.Destroy(audioSource);
        }
    }

    private bool showedEqNotification = false;
    public static Level levelSelf;
    private static AbstractPlayerController player;
}
