using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CupaGoovno;

public class Dragon
{
    public void Init()
    {
        new DragonPotion().Init();
        new DragonPlatformManager().Init();
        new DragonCloudPlatform().Init();
        new DragonFireMarcher().Init();
        new DragonLeftSide().Init();
        new DragonMeteor().Init();
        On.DragonLevel.OnStateChanged += OnStateChanged;
        On.DragonLevelDragon.meteor_cr += meteor_cr;
        On.DragonLevelDragon.FireMeteor += FireMeteor;
        On.DragonLevelDragon.peashot_cr += peashot_cr;
    }

    protected void OnStateChanged(On.DragonLevel.orig_OnStateChanged orig, DragonLevel self)
    {
        orig(self);
        if (self.properties.CurrentState.stateName == LevelProperties.Dragon.States.FireMarchers)
        {
            self.StartCoroutine(DragonPlatformManager.ChangeCloudsStopX(self.manager, true));
        }
        else if (self.properties.CurrentState.stateName == LevelProperties.Dragon.States.ThreeHeads)
        {
            self.StartCoroutine(DragonPlatformManager.ChangeCloudsStopX(self.manager, true));
        }
    }

    private IEnumerator meteor_cr(On.DragonLevelDragon.orig_meteor_cr orig, DragonLevelDragon self)
    {
        self.currentMeteorProperties = self.properties.CurrentState.meteor;
        char[] meteorPattern = self.currentMeteorProperties.pattern.GetRandom<string>().ToCharArray();
        //self.animator.SetTrigger("OnMeteor");
        //self.animator.SetBool("Repeat", true);
        //yield return self.animator.WaitForAnimationToStart(self, "MeteorStart", false);
        AudioManager.Play("level_dragon_left_dragon_meteor_start");
        self.emitAudioFromObject.Add("level_dragon_left_dragon_meteor_start");
        for (int i = 0; i < meteorPattern.Length; i++)
        {
            self.animator.Play("MeteorStart");//new
            //char c = meteorPattern[i];
            char[] patterns = new char[] { 'B', 'F' };//new
            char c = patterns[UnityEngine.Random.Range(0, 2)];//new
            switch (c)
            {
                case 'B':
                    self.meteorState = DragonLevelMeteor.State.Both;
                    break;
                default:
                    if (c != 'U')
                    {
                    }
                    self.meteorState = DragonLevelMeteor.State.Up;
                    break;
                case 'D':
                    self.meteorState = DragonLevelMeteor.State.Down;
                    break;
                case 'F':
                    self.meteorState = DragonLevelMeteor.State.Forward;
                    break;
            }
            if (i >= meteorPattern.Length - 1)
            {
                //self.animator.SetBool("Repeat", false);
            }
            yield return self.animator.WaitForAnimationToStart(self, "Meteor_Anticipation_Loop", false);
            AudioManager.Play("level_dragon_left_dragon_meteor_anticipation_loop");
            self.emitAudioFromObject.Add("level_dragon_left_dragon_meteor_anticipation_loop");
            yield return CupheadTime.WaitForSeconds(self, self.currentMeteorProperties.shotDelay);
            self.animator.SetTrigger("OnMeteor");
            AudioManager.Stop("level_dragon_left_dragon_meteor_anticipation_loop");
            yield return self.animator.WaitForAnimationToStart(self, "Meteor_Attack", false);
            AudioManager.Play("level_dragon_left_dragon_meteor_attack");
            yield return self.animator.WaitForAnimationToEnd(self, "Meteor_Attack", false, true);
        }
        yield return self.animator.WaitForAnimationToEnd(self, "Meteor_Attack_End", false, true);
        yield return CupheadTime.WaitForSeconds(self, self.currentMeteorProperties.hesitate);
        self.state = DragonLevelDragon.State.Idle;
        yield break;
    }

    private void FireMeteor(On.DragonLevelDragon.orig_FireMeteor orig, DragonLevelDragon self)
    {
        float deltaY = UnityEngine.Random.Range(-200f, 150f);//new start
        Vector3 pos = self.mouthRoot.position + new Vector3(0f, deltaY);
        float timeY = self.currentMeteorProperties.timeY;
        float speedX = self.currentMeteorProperties.speedX;//new end
        AudioManager.Play("level_dragon_left_dragon_meteor_spit");
        self.emitAudioFromObject.Add("level_dragon_left_dragon_meteor_spit");
        if (self.meteorState == DragonLevelMeteor.State.Both)
        {
            self.meteorPrefab.Create(pos, new DragonLevelMeteor.Properties(timeY, speedX, DragonLevelMeteor.State.Up));//new
            self.meteorPrefab.Create(pos, new DragonLevelMeteor.Properties(timeY, speedX, DragonLevelMeteor.State.Down));//new
            /*self.meteorPrefab.Create(self.mouthRoot.position, new DragonLevelMeteor.Properties(self.currentMeteorProperties.timeY, self.currentMeteorProperties.speedX, DragonLevelMeteor.State.Up));
            self.meteorPrefab.Create(self.mouthRoot.position, new DragonLevelMeteor.Properties(self.currentMeteorProperties.timeY, self.currentMeteorProperties.speedX, DragonLevelMeteor.State.Down));*/
        }
        else
        {
            if (self.meteorState == DragonLevelMeteor.State.Forward)//new start
            {
                self.meteorPrefab.Create(pos + new Vector3(0f, 220f), new DragonLevelMeteor.Properties(timeY, speedX, self.meteorState));//new
                self.meteorPrefab.Create(pos - new Vector3(0f, 130f), new DragonLevelMeteor.Properties(timeY, speedX, self.meteorState));//new
                /*self.meteorPrefab.Create(self.mouthRoot.position + new Vector3(0f, 220f), new DragonLevelMeteor.Properties(self.currentMeteorProperties.timeY, self.currentMeteorProperties.speedX * 2, self.meteorState));
                self.meteorPrefab.Create(self.mouthRoot.position - new Vector3(0f, 130f), new DragonLevelMeteor.Properties(self.currentMeteorProperties.timeY, self.currentMeteorProperties.speedX * 2, self.meteorState));*/
            }
            else
            {//new end
                self.meteorPrefab.Create(self.mouthRoot.position, new DragonLevelMeteor.Properties(self.currentMeteorProperties.timeY, self.currentMeteorProperties.speedX, self.meteorState));
            }//new
        }
    }

    private IEnumerator peashot_cr(On.DragonLevelDragon.orig_peashot_cr orig, DragonLevelDragon self)
    {
        LevelProperties.Dragon.Peashot p = self.properties.CurrentState.peashot;
        string[] pattern = p.patternString.GetRandom<string>().Split(new char[]
        {
            ','
        });
        self.animator.SetBool("Peashot", true);
        yield return self.animator.WaitForAnimationToEnd(self, "Peashot_In", false, true);
        self.animator.Play("Peashot_Zinger");
        for (int i = 0; i < pattern.Length; i++)
        {
            if (pattern[i].ToLower() == "p")
            {
                self.peashotRoot.LookAt2D(PlayerManager.GetNext().center);
                for (int c = 0; c < p.colorString.Length; c++)
                {
                    int color = 0;
                    char c2 = p.colorString[c];
                    if (c2 != 'O')
                    {
                        if (c2 != 'P')
                        {
                            if (c2 == 'B')
                            {
                                color = 1;
                            }
                        }
                        else
                        {
                            color = 2;
                        }
                    }
                    else
                    {
                        color = 0;
                    }
                    AudioManager.Play("level_dragon_left_dragon_peashot_fire");
                    self.emitAudioFromObject.Add("level_dragon_left_dragon_peashot_fire");
                    (self.peashotPrefab.Create(self.peashotRoot.position, self.peashotRoot.eulerAngles.z, p.speed) as DragonLevelPeashot).color = color;
                    yield return CupheadTime.WaitForSeconds(self, p.shotDelay);
                }
            }
            else
            {
                float delay = 0f;
                Parser.FloatTryParse(pattern[i], out delay);
                yield return CupheadTime.WaitForSeconds(self, delay);
            }
        }
        self.animator.SetBool("Peashot", false);
        self.animator.Play("Peashot_Out");
        //yield return self.animator.WaitForAnimationToStart(self, "Peashot_Out", false);
        AudioManager.Play("level_dragon_left_dragon_peashot_out");
        self.emitAudioFromObject.Add("level_dragon_left_dragon_peashot_out");
        yield return CupheadTime.WaitForSeconds(self, p.hesitate);
        self.state = DragonLevelDragon.State.Idle;
        yield break;
    }
}
