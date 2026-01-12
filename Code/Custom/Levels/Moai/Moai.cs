using Blender.Content;
using Blender.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CupaGoovno;

public class Moai
{
    public void Init()
    {
        moaiScene = SceneRegistries.Levels.Register("scene_level_moai",
    new LevelInfo(typeof(MoaiLevel), bundle, levelName)
        .SetActualType(Level.Type.Battle)
        .SetPlayerMode(PlayerMode.Level)
        .SetDefaultGoalTimes(new Level.GoalTimes(1F, 1.25F, 1.5F))
        .SetSetupAction((level) => {
        }));

        new PlayerMoai().Init();
    }

    public static void Load(MonoBehaviour self)
    {
        Levels level = SceneRegistries.GetLevel(moaiScene);
        SceneLoader.LoadLevel(level, SceneLoader.Transition.Fade);
    }

    private static string levelName = "MoaiLevel";
    private static string bundle = "CupaGoovno:moai";
    private static Scenes moaiScene;
}
