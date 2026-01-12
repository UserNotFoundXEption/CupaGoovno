using Blender.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class Rift
{
    public void Init()
    {
        riftScene = SceneRegistries.Levels.Register("scene_level_dancers_rift",
    new LevelInfo(typeof(RiftLevel), bundle, levelName)
        .SetActualType(Level.Type.Battle)
        .SetPlayerMode(PlayerMode.Level)
        .SetDefaultGoalTimes(new Level.GoalTimes(1F, 1.25F, 1.5F))
        .SetSetupAction((level) => {
        }));
    }

    public static void Load(MonoBehaviour self)
    {
        //if (alreadyLoaded)
        //{
        //    string tText = "Can't load Rift twice";
        //    string mText = "Due to BlenderAPI bug you need to use death screen to change items. Relaunch your game to fight again.";
        //    self.StartCoroutine(Other.notification_cr(self, tText, mText, 15f, 150f, true));
        //}
        //else
        //{
            //alreadyLoaded = true;
            Levels level = SceneRegistries.GetLevel(riftScene);
            SceneLoader.LoadLevel(level, SceneLoader.Transition.Fade);
        //}
    }

    private static string levelName = "RiftLevel";
    private static string bundle = "CupaGoovno:dancersrift";
    private static Scenes riftScene;
    //private static bool alreadyLoaded;
}
