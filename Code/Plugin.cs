using BepInEx;
using Blender.Patching;
using Blender.Utility;
using UnityEngine.U2D;
using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

namespace CupaGoovno;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    public static Plugin instance;

    private void Awake()
    {
        LocalizationPatcher.RegisterLocalization(new Identifier("CupaGoovno", "localization.json"));
        instance = this;
        SettingsManager.LoadSettings();
        CustomAchievements.LoadAchievements();

        new HarmonyPatching().Init();

        new CustomCharms().Init();
        new CustomWeapons().Init();
        new CustomSupers().Init();
        new CustomAchievements().Init();
        new CustomNpcs().Init();
        new CustomSprites().Init();

        new Moai().Init();
        new Rift().Init();

        new Veggies().Init();
        new Slime().Init();
        new Frogs().Init();
        new Flower().Init();
        new Blimp().Init();

        new Baroness().Init();
        new Clown().Init();
        new Dragon().Init();
        new Bird().Init();
        new Genie().Init();

        new Bee().Init();
        new Robot().Init();
        new Mouse().Init();
        new Pirate().Init();
        new Sally().Init();
        new Mermaid().Init();
        new Train().Init();

        new KingDice().Init();
        new Rabbit().Init();
        new Roulette().Init();
        new Domino().Init();
        new Cigar().Init();
        new Booze().Init();
        new Chips().Init();
        new Ball().Init();
        new Monkey().Init();
        new Horse().Init();
        new Devil().Init();

        new Cow().Init();
        new Graveyard().Init();

        new Awake().Init();
        new LevelStart().Init();
        new Properties().Init();
        new FlowerPlatform().Init();
        new DifficultySelectUI().Init();
        new ParryOrb().Init();
        new Other().Init();
        new StatsManager().Init();
        new PlayerAnimationController().Init();
        new PlaneAnimationController().Init();
        new EquipmentReworks().Init();
        new Dragonfly().Init();
        new PlaneWeaponManager().Init();
        new Camera().Init();
        new HitFlash2().Init();
        new PauseMenu().Init();
        new FlowerEffect().Init();
        new ParryStuff().Init();
        new Localization2().Init();
        new Motor().Init();
        new LevelWeaponManager().Init();
        new AbstractWeapon().Init();
        RunNGunFollower.Init();
    }

    public static void Log(object data)
    {
        if (data is string s)
            instance.Logger.LogInfo(s);
        else if (data is IDictionary dict)
        {
            var entries = new List<string>();
            foreach (var key in dict.Keys)
            {
                entries.Add($"{key}={dict[key]}");
            }
            instance.Logger.LogInfo($"Dictionary: {string.Join(", ", entries.ToArray())}");
        }
        else
            instance.Logger.LogInfo(data?.ToString() ?? "NULL");
    }
}
