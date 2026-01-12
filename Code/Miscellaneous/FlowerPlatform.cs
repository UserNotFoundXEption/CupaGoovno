using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

namespace CupaGoovno;

public class FlowerPlatform
{
    public void Init()
    {
        On.FlowerLevelPlatform.Start += Start;
        On.FlowerLevelPlatform.StartUp += StartUp;
        On.FlowerLevelPlatform.StartDown += StartDown;
        On.FlowerLevelPlatform.AddChild += AddChild;
        On.FlowerLevelPlatform.OnPlayerExit += OnPlayerExit;
        On.FlowerLevelPlatform.goTo_cr += goTo_cr;
    }

    private void Start(On.FlowerLevelPlatform.orig_Start orig, FlowerLevelPlatform self)
    {
        self.YPositionDown = self.YPositionUp - 30f;
        self.YFall = self.YPositionUp - 35f;
        if (self.shadow != null)
        {
            self.shadow.parent = null;
            Vector3 position = self.shadow.position;
            position.y = (float)Level.Current.Ground;
            self.shadow.position = position;
        }
        self.startPos = self.transform.position;
        self.startPos.y = self.YPositionUp;
        self.endPos = self.transform.position;
        self.endPos.y = self.YPositionDown;
        if (SceneManager.GetActiveScene().name != "scene_level_veggies")//new
        {//new
            if (self.state == FlowerLevelPlatform.State.Down)
            {
                self.transform.SetPosition(null, new float?(self.YPositionUp), null);
                self.StartDown();
            }
            else
            {
                self.transform.SetPosition(null, new float?(self.YPositionDown), null);
                self.StartUp();
            }
        }//new
    }

    public void StartDown(On.FlowerLevelPlatform.orig_StartDown orig, FlowerLevelPlatform self)
    {
        if (SceneManager.GetActiveScene().name != "scene_level_veggies")//new
        {//new
            orig(self);
        }//new
    }

    public void StartUp(On.FlowerLevelPlatform.orig_StartUp orig, FlowerLevelPlatform self)
    {
        if (SceneManager.GetActiveScene().name != "scene_level_veggies")//new
        {//new
            orig(self);
        }//new
    }

    public void OnPlayerExit(On.FlowerLevelPlatform.orig_OnPlayerExit orig, FlowerLevelPlatform self, object player)
    {
        if (player != null && SceneManager.GetActiveScene().name != "scene_level_veggies")//new
        {//new
            orig(self, player);
        }//new
    }

    public void AddChild(On.FlowerLevelPlatform.orig_AddChild orig, FlowerLevelPlatform self, object player)
    {
        if (player != null && SceneManager.GetActiveScene().name != "scene_level_veggies")//new
        {//new
            orig(self, player);
        }//new
    }

    private IEnumerator goTo_cr(On.FlowerLevelPlatform.orig_goTo_cr orig, FlowerLevelPlatform self, float start, float end, float time, EaseUtils.EaseType ease)
    {
        if(SceneManager.GetActiveScene().name != "scene_level_veggies")
        {
            yield return orig(self, start, end, time, ease);
        }
    }
}
