using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class RiftString : CustomProjectile
{
    public static RiftString Create(int id)
    {
        RiftString riftString = Create<RiftString>(prefab);

        Vector2 pos = new(p.x[id], p.y[id]);
        riftString.transform.position = pos;
        riftString.transform.SetEulerAngles(0f, 0f, p.rotation[id]);
        riftString.id = id;

        //float scale = p.scale;
        //pusher.transform.SetScale(scale, scale);

        riftString.parrySwitch = riftString.gameObject.AddComponent<ParrySwitch>();
        riftString.parrySwitch.enabled = true;
        riftString.parrySwitch.coolDown = 0.1f;
        riftString.parrySwitch.OnActivate += riftString.OnParry;

        riftString.tag = "Enemy";
        riftString.DamagesType.Player = false;

        return riftString;
    }

    /*public override void OnParry(AbstractPlayerController player)
    {
    }*/

    public void OnParry()
    {
        parrySwitch.StartParryCooldown();
        RiftLevel.manager.OnStringParry(id);
    }

    public static GameObject prefab;

    private static RiftProperties.String p = new();

    public int id;

    private ParrySwitch parrySwitch;

    public override float DestroyLifetime => 2137f;
}
