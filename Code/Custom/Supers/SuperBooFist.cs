using Blender.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public static class SuperBooFist
{
    public static void Init()
    {
        AssetHelper.AddPersistentPath(AssetHelper.LoaderType.Single, SuperBooFistProjectile.prefabPath);
    }

    public static void StartSuper(LevelPlayerWeaponManager self)
    {
        AbstractPlayerController player = PlayerManager.GetPlayer(self.player.id);
        SuperBooFistProjectile.Create(player.center.x);
        player.stats.SuperMeter = 0f;
        player.stats.OnSuperChanged();
    }
}
