using System;
using System.Collections.Generic;
using System.IO;

namespace CupaGoovno;

public static class SettingsManager
{
    private const string settingsPath = "CupaGoovnoSettings.json";

    public static void LoadSettings()
    {
        try
        {
            if (File.Exists(settingsPath))
            {
                string json = File.ReadAllText(settingsPath);
                var dict = Json.Deserialize(json) as Dictionary<string, object>;

                if (dict != null)
                {
                    int settingsParsed = 0;
                    if (dict.TryGetValue("hideNotifications", out var hideNotifs))
                    {
                        Settings.hideNotifications = Convert.ToBoolean(hideNotifs);
                        settingsParsed++;
                    }
                    if (dict.TryGetValue("hideDamageMultiplier", out var hideDmgMult))
                    {
                        Settings.hideDamageMultiplier = Convert.ToBoolean(hideDmgMult);
                        settingsParsed++;
                    }
                    if (dict.TryGetValue("betterPlatforms", out var betterPlatforms))
                    {
                        Settings.betterPlatforms = Convert.ToBoolean(betterPlatforms);
                        settingsParsed++;
                    }
                    if (settingsParsed == 3)
                    {
                        return;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Plugin.Log($"Error while loading CupaGoovno settings: {ex.Message}");
            Plugin.Log($"Saving default settings.");
        }

        SetDefaults();
        SaveSettings();
    }

    private static void SetDefaults()
    {
        SettingsModel defaultSettings = new();
        Settings.hideNotifications = defaultSettings.hideNotifications;
        Settings.hideDamageMultiplier = defaultSettings.hideDamageMultiplier;
        Settings.betterPlatforms = defaultSettings.betterPlatforms;
    }

    public static void SaveSettings()
    {
        try
        {
            var dict = new Dictionary<string, object>
            {
                { "hideNotifications", Settings.hideNotifications },
                { "hideDamageMultiplier", Settings.hideDamageMultiplier },
                { "betterPlatforms", Settings.betterPlatforms }
            };
            string json = Json.Serialize(dict);
            File.WriteAllText(settingsPath, json);
        }
        catch (Exception ex)
        {
            Plugin.Log($"Error while saving CupaGoovno settings: {ex.Message}");
        }
    }
}
