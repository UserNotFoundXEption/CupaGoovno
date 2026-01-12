using Blender.Utility;
using System;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.U2D;

namespace CupaGoovno;

public static class CharmMilk
{
    public static void Init()
    {
        AssetHelper.AddPersistentPath(AssetHelper.LoaderType.Single, CharmMilkProjectile.prefabPath);
    }

    public static void Fire()
    {
        if (charged)
        {
            Other.Player().StartCoroutine(fire_cr());
        }
    }

    private static IEnumerator fire_cr()
    {
        AbstractPlayerController player = Other.Player();
        if(player is LevelPlayerController levelPlayer)
        {
            yield return CupheadTime.WaitForSeconds(Other.Player(), p.normalSpawnDelay);

            LevelPlayerWeaponManager weaponManager = levelPlayer.GetComponent<LevelPlayerWeaponManager>();
            float angle = weaponManager.aim.transform.eulerAngles.z;
            CharmMilkProjectile.Create(weaponManager.ExPosition, angle);
        }
        if (player is PlanePlayerController planePlayer)
        {
            yield return CupheadTime.WaitForSeconds(Other.Player(), p.planeSpawnDelay);

            PlanePlayerWeaponManager weaponManager = planePlayer.GetComponent<PlanePlayerWeaponManager>();
            CharmMilkProjectile.Create(weaponManager.GetBulletPosition(), 0f);
        }

        charged = false;
    }

    public static bool charged;

    private static CustomCharmProperties.Milk p = new();
}
