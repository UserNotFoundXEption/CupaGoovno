using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class Clown
{
    public void Init()
    {
        new ClownDucks().Init();
        new ClownDogBalloon().Init();
        new ClownSwing().Init();
        new ClownHorse().Init();
        new ClownCoaster().Init();
        new ClownHorseshoe().Init();
        On.ClownLevel.OnStateChanged += OnStateChanged;
    }

    protected void OnStateChanged(On.ClownLevel.orig_OnStateChanged orig, ClownLevel self)
    {
        orig(self);
        PlayerManager.GetFirst().transform.parent = null;
        if (self.properties.CurrentState.stateName == LevelProperties.Clown.States.CarouselHorse)
        {
            GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();
            foreach (GameObject obj in objects)
            {
                if (obj.name == "Clown_Dog_Balloon_Pink(Clone)" || obj.name == "Clown_Coaster_Main(Clone)")
                {
                    Other.MakePlayersFatherless();
                    GameObject.Destroy(obj.gameObject);
                }
            }
        }
    }
}
