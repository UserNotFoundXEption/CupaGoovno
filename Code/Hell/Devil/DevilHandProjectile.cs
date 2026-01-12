using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class DevilHandProjectile
{
    public void Init()
    {
        On.AbstractProjectile.OnParry += OnParry;
        On.DevilLevelHandProjectile.Start += Start;
    }

    public void OnParry(On.AbstractProjectile.orig_OnParry orig, AbstractProjectile self, AbstractPlayerController player)
    {
        if (self is DevilLevelHandProjectile)
        {
            self.GetComponent<Collider2D>().enabled = false;
            self.GetComponent<SpriteRenderer>().enabled = false;
        }
        else if(self is DevilLevelTear tearSelf)
        {
            if (DevilTear.canParry)
            {
                orig(self, player);
                Devil.devil.StartCoroutine(DevilTear.parryCooldown_cr());
            }
        }
        else
        {
            orig(self, player);
        }
    }

    protected void Start(On.DevilLevelHandProjectile.orig_Start orig, DevilLevelHandProjectile self)
    {
        orig(self);
        DevilLevelPitchforkOrbitingProjectile prefab = Devil.orbiter;
        float rotationSpeed = 100f;
        float radius = 150f;
        float initialRotation = UnityEngine.Random.Range(0f, 90f);

        DevilLevelPitchforkOrbitingProjectile proj = prefab.Create(self, initialRotation, rotationSpeed, radius, null, 0f);
        //proj.transform.SetScale(0.5f, 0.5f);

        proj = prefab.Create(self, initialRotation + 180f, rotationSpeed, radius, null, 0f);
        //proj.transform.SetScale(0.5f, 0.5f);
    }
}
