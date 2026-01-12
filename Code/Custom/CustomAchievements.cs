using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace CupaGoovno;

public class CustomAchievements
{
    public void Init()
    {
        On.LocalAchievementsManager.IsAchievementUnlocked += IsAchievementUnlocked;
        On.AchievementsGUI.refreshIcons += refreshIcons;
        On.AchievementsGUI.updateSelection += updateSelection;
        On.AchievementsGUI.HideAchievements += HideAchievements;
    }

    public static void LoadAchievements()
    {
        try
        {
            if (File.Exists(achievementsPath))
            {
                string jsonEncrypted = File.ReadAllText(achievementsPath);
                string json = Decrypt(jsonEncrypted);
                var dict = Json.Deserialize(json) as Dictionary<string, object>;
                if (dict != null)
                {
                    int settingsParsed = 0;
                    foreach(var ach in achievements)
                    {
                        if (dict.TryGetValue(ach.name, out var unlocked))
                        {
                            ach.unlocked = Convert.ToBoolean(unlocked);
                            settingsParsed++;
                        }
                    }
                    if (settingsParsed == achievements.Count)
                    {
                        return;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Plugin.Log($"Error while loading CupaGoovno achievements: {ex.Message}");
            Plugin.Log($"Saving default achievements.");
        }

        SetAllToFalse();
        //Mock();
        SaveAchievements();
    }

    private static void SetAllToFalse()
    {
        foreach(var ach in achievements)
        {
            ach.unlocked = false;
        }
    }

    private static void Mock()
    {
        achievements =
        [
            new("builtDifferent", Achievements.BuiltDifferent, true),
            new("butcher", Achievements.Butcher, true),
            new("dancersRift", Achievements.DancersRift, true),
            new("relic", Achievements.Relic, true),
            new("bird", Achievements.Bird, true),
            new("invincible", Achievements.Invincible, true),
            new("dice", Achievements.Dice)
        ];
    }

    public static void Unlock(Achievements toUnlock)
    {
        bool isValidLevel = Level.world1BossLevels.Contains(Level.Current.CurrentLevel) ||
                           Level.world2BossLevels.Contains(Level.Current.CurrentLevel) ||
                           Level.world3BossLevels.Contains(Level.Current.CurrentLevel) ||
                           Level.world4BossLevels.Contains(Level.Current.CurrentLevel) ||
                           toUnlock == Achievements.DancersRift;
        
        Charm charm = Other.Player().stats.Loadout.charm;
        bool hasBannedCharm = charm == Charm.charm_smoke_dash ||
                            charm == CustomCharms.casualMode;

        if (!isValidLevel || hasBannedCharm || Awake.showedCompatibilityWarning)
        {
            Plugin.Log($"Couldn't unlock achievement {toUnlock}.\n" +
                $"isValidLevel: {isValidLevel}\n" +
                $"hasBannedCharm: {hasBannedCharm}\n" +
                $"showedCompatibilityWarning: {Awake.showedCompatibilityWarning}");
            return;
        }

        Achievement unlocked;
        foreach(var ach in achievements)
        {
            if (ach.ach == toUnlock && !ach.unlocked)
            {
                unlocked = ach;
                unlocked.unlocked = true;
                SaveAchievements();
                CustomWeapons.GiftWeapons();
                CustomCharms.GiftCharms();
                CustomSupers.GiftSupers();
                break;
            }
        }

    }

    public static void SaveAchievements()
    {
        try
        {
            var dict = new Dictionary<string, object>();
            foreach(var ach in achievements)
            {
                dict.Add(ach.name, ach.unlocked);
            }
            string json = Json.Serialize(dict);
            string jsonEncrypted = Encrypt(json);
            File.WriteAllText(achievementsPath, jsonEncrypted);
        }
        catch (Exception ex)
        {
            Plugin.Log($"Error while saving CupaGoovno achievements: {ex.Message}");
        }
    }

    private static string Encrypt(string plainText)
    {
        Aes aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv;

        using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream();
        using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
        using var sw = new StreamWriter(cs);

        sw.Write(plainText);
        sw.Close();

        string base64Encrypted = Convert.ToBase64String(ms.ToArray());
        return marker + Environment.NewLine + base64Encrypted;
    }

    public static string Decrypt(string input)
    {
        string[] lines = input.Replace("\r", "").Split('\n');
        if (lines.Length < 2 || lines[0] != marker)
            throw new InvalidOperationException("Marker not found or data malformed.");

        string encryptedBase64 = lines[1];
        byte[] cipherBytes = Convert.FromBase64String(encryptedBase64);

        using Aes aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv;

        using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream(cipherBytes);
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var sr = new StreamReader(cs);

        return sr.ReadToEnd();
    }

    public bool IsAchievementUnlocked(On.LocalAchievementsManager.orig_IsAchievementUnlocked orig, LocalAchievementsManager.Achievement achievement)
    {
        return false;
    }

    public void refreshIcons(On.AchievementsGUI.orig_refreshIcons orig, AchievementsGUI self)
    {
        ShowAchievementsWarning();
        self.topArrow.enabled = false;
        self.bottomArrow.enabled = false;
        SpriteAtlas cachedAsset = AssetLoader<UnityEngine.Object>.GetCachedAsset(CustomSprites.spriteAtlas) as SpriteAtlas;

        int counter = 0;
        foreach (AchievementsGUI.IconRow iconRow in self.iconRows)
        {
            foreach (AchievementIcon achievementIcon in iconRow.achievementIcons)
            {
                if (counter < achievements.Count)
                {
                    string iconName = achievements[counter].name;
                    iconName = achievements[counter].unlocked ?
                        "ach_" + iconName + "_unlocked" :
                        "ach_" + iconName + "_locked";
                    achievementIcon.SetIcon(cachedAsset.GetSprite(iconName));
                    counter++;
                }
                else
                {
                    achievementIcon.gameObject.SetActive(false);
                }
            }
        }
    }

    public void updateSelection(On.AchievementsGUI.orig_updateSelection orig, AchievementsGUI self)
    {
        AchievementIcon achievementIcon = self.iconRows[self.cursorIndex.y].achievementIcons[self.cursorIndex.x];
        self.cursor.position = achievementIcon.transform.position;
        int num = self.achievementIndex.y * self.currentGridSize.x + self.achievementIndex.x;
        Achievement ach = num < achievements.Count ?
            achievements[num] : 
            new("none", Achievements.None);

        string title = "ach_" + ach.name + "_title";
        string description = "ach_" + ach.name + "_description";

        self.titleLocalization.ApplyTranslation(Localization.Find(title), null);
        self.descriptionLocalization.ApplyTranslation(Localization.Find(description), null);

        string iconName = ach.unlocked ?
            "ach_" + ach.name + "_unlocked" :
            "ach_" + ach.name + "_locked";
        SpriteAtlas cachedAsset = AssetLoader<UnityEngine.Object>.GetCachedAsset(CustomSprites.spriteAtlas) as SpriteAtlas;
        Sprite sprite = cachedAsset.GetSprite(iconName);
        self.largeIcon.sprite = sprite;

        self.titleText.color = ach.unlocked ? AchievementsGUI.UnlockedTextColor : AchievementsGUI.LockedTextColor;
        self.descriptionText.color = ach.unlocked ? AchievementsGUI.UnlockedTextColor : AchievementsGUI.LockedTextColor;

        self.unearnedBackground.enabled = !ach.unlocked;
        self.noise.sprite = self.getSprite(ach.unlocked ? "cheev_card_noise_earned" : "cheev_card_noise_unearned", self.defaultAtlas);

        AudioManager.Play("level_menu_move");
    }

    public void HideAchievements(On.AchievementsGUI.orig_HideAchievements orig, AchievementsGUI self)
    {
        HideAchievementsWarning();
        orig(self);
    }


    private void ShowAchievementsWarning()
    {
        GameObject canvasObject = new GameObject("AchievementsWarning");
        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObject.AddComponent<CanvasScaler>();
        canvasObject.AddComponent<GraphicRaycaster>();

        GameObject message = new GameObject("Message");
        message.transform.SetParent(canvas.transform, false);

        Text messageText = message.AddComponent<Text>();
        messageText.text = "Achievements can only be unlocked in non-DLC boss fights. Custom levels and King Dice's mini-bosses don't count.\nYou can't use Practise Mode, Casual Mode and other mods.";
        messageText.fontSize = 24;
        messageText.color = UnityEngine.Color.white;
        messageText.font = Resources.GetBuiltinResource<UnityEngine.Font>("Arial.ttf");
        messageText.fontStyle = UnityEngine.FontStyle.Italic;
        messageText.alignment = TextAnchor.UpperCenter;

        RectTransform messageRect = messageText.GetComponent<RectTransform>();
        messageRect.sizeDelta = new Vector2(1600, 100);
        messageRect.anchorMin = new Vector2(0.5f, 0f);
        messageRect.anchorMax = new Vector2(0.5f, 0f);
        messageRect.anchoredPosition = new Vector2(0, 20f);
    }

    private void HideAchievementsWarning()
    {
        GameObject canvasObject = GameObject.Find("AchievementsWarning");
        if (canvasObject != null)
        {
            GameObject.Destroy(canvasObject);
        }
    }

    public static List<Achievement> achievements =
        [
            new("builtDifferent", Achievements.BuiltDifferent),
            new("butcher", Achievements.Butcher),
            new("dancersRift", Achievements.DancersRift),
            new("relic", Achievements.Relic),
            new("bird", Achievements.Bird),
            new("invincible", Achievements.Invincible),
            new("dice", Achievements.Dice)
        ];

    private const string achievementsPath = "CupaGoovnoAchievements";
    private static readonly byte[] key = Encoding.UTF8.GetBytes("IDontReallyCareToHideThisKey1234");
    private static readonly byte[] iv = Encoding.UTF8.GetBytes("GoodJobCheater<3");
    private static readonly string marker = "Not so fast, cheater. ;D";

    public class Achievement
    {
        public string name;
        public bool unlocked;
        public Achievements ach;

        public Achievement(string name, Achievements ach, bool unlocked = false)
        {
            this.name = name;
            this.unlocked = unlocked;
            this.ach = ach;
        }
    }

    public enum Achievements
    {
        BuiltDifferent,
        Butcher,
        DancersRift,
        Relic,
        Bird,
        Invincible,
        Dice,
        None
    }
}
