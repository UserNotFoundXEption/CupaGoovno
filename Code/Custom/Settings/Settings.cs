using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CupaGoovno;

public static class Settings
{
    public static bool hideNotifications;
    public static bool hideDamageMultiplier;
    public static bool betterPlatforms;
}

public class SettingsModel
{
    public bool hideNotifications;
    public bool hideDamageMultiplier;
    public bool betterPlatforms;

    public SettingsModel()
    {
        hideNotifications = false;
        hideDamageMultiplier = false;
        betterPlatforms = true;
    }
}
