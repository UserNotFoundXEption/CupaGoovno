using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace CupaGoovno;

public class PauseMenu
{
    public void Init()
    {
        On.LevelPauseGUI.Init_bool_OptionsGUI_AchievementsGUI_RestartTowerConfirmGUI += Init;
        On.LevelPauseGUI.OnPause += OnPause;
        On.LevelPauseGUI.Select += Select;
        On.OptionsGUI.ShowMainOptionMenu += ShowMainOptionMenu;
        On.OptionsGUI.Update += Update;


    }

    public void Init(On.LevelPauseGUI.orig_Init_bool_OptionsGUI_AchievementsGUI_RestartTowerConfirmGUI orig, LevelPauseGUI self, bool checkIfDead, OptionsGUI options, AchievementsGUI achievements, RestartTowerConfirmGUI restartTowerConfirm)
    {
        //self.Init(checkIfDead, options, achievements);
        self.input = new CupheadInput.AnyPlayerInput(checkIfDead);//new from base
        self.options = options;
        self.achievements = achievements;
        self.restartTowerConfirm = restartTowerConfirm;

        AddCupaGoovnoOption(self);//new

        //if (PlatformHelper.IsConsole && self.menuItems.Length > 7)
        if (PlatformHelper.IsConsole && self.menuItems.Length > 8)//new
        {
            //self.menuItems[7].gameObject.SetActive(false);
            self.menuItems[8].gameObject.SetActive(false);//new
        }
        if (Level.Current != null && Level.Current.CurrentLevel == Levels.Airplane)
        {
            self.menuItems[2].gameObject.SetActive(true);
            self.updateRotateControlsToggleVisualValue();
        }
        else if (!PlatformHelper.ShowAchievements && self.menuItems.Length > 2)
        {
            self.menuItems[2].gameObject.SetActive(false);
        }
        if (Level.IsTowerOfPower)
        {
            self.ReplaceRestartWRestartTowerOfPower();
        }
        options.Init(checkIfDead);
        if (achievements != null)
        {
            achievements.Init(checkIfDead);
        }
        if (restartTowerConfirm != null)
        {
            restartTowerConfirm.Init(checkIfDead);
        }
    }

    public void OnPause(On.LevelPauseGUI.orig_OnPause orig, LevelPauseGUI self)
    {
        orig(self);
        self.menuItems[4].gameObject.SetActive(true);
        self.menuItems[5].gameObject.SetActive(PlayerManager.Multiplayer);
    }

    private void Select(On.LevelPauseGUI.orig_Select orig, LevelPauseGUI self)
    {
        switch (self.selection)
        {
            case 0:
                self.Unpause();
                break;
            case 1:
                self.Restart();
                break;
            case 2:
                self.Achievements();
                break;
            case 3:
                cgOptions = false;//new
                self.Options();
                break;
            case 4:
                cgOptions = true;//new
                self.Options();//new
                break;
            case 5:
                self.Player2Leave();
                break;
            case 6:
                self.Exit();
                break;
            case 7:
                self.ExitToTitle();
                break;
            case 8:
                self.ExitToDesktop();
                break;
        }
    }

    public void ShowMainOptionMenu(On.OptionsGUI.orig_ShowMainOptionMenu orig, OptionsGUI self)
    {
        if (!cgOptions)
        {
            foreach(OptionsGUI.Button button in self.currentItems)
            {
                button.localizationHelper.ApplyTranslation();
            }
            orig(self);
        }
        else
        {
            self.mainObject.SetActive(true);
            self.visualObject.SetActive(false);
            self.audioObject.SetActive(false);
            self.languageObject.SetActive(false);
            self.bigCard.SetActive(false);
            self.bigNoise.SetActive(false);

            RenameOptions(self);

            self.optionMenuOpen = true;
            self.canvasGroup.alpha = 1f;
            self.FrameDelayedCallback(new Action(self.Interactable), 1);
        }
    }

    public void Update(On.OptionsGUI.orig_Update orig, OptionsGUI self)
    {
        if (!cgOptions)
        {
            orig(self);
        }
        else
        {
            if (!self.inputEnabled)
            {
                return;
            }
            if (self.GetButtonDown(CupheadButton.Pause) || self.GetButtonDown(CupheadButton.Cancel))
            {
                self.MenuSelectSound();
                self.HideMainOptionMenu();
                self.StartCoroutine(optionsCloseDelay_cr(self));
                return;
            }
            if (self.GetButtonDown(CupheadButton.Accept) || self.GetButtonDown(CupheadButton.MenuLeft) || self.GetButtonDown(CupheadButton.MenuRight))
            {
                CupaGoovnoSelect(self);
                return;
            }
            if (self._selectionTimer >= 0.15f)
            {
                if (self.GetButton(CupheadButton.MenuUp))
                {
                    self.MenuMoveSound();
                    self.verticalSelection--;
                    if(self.verticalSelection < 0)
                    {
                        self.verticalSelection = 4;
                    }
                }
                if (self.GetButton(CupheadButton.MenuDown))
                {
                    self.MenuMoveSound();
                    self.verticalSelection++;
                    if (self.verticalSelection > 4)
                    {
                        self.verticalSelection = 0;
                    }
                }
            }
            else
            {
                self._selectionTimer += Time.deltaTime;
            }
        }
    }

    private void AddCupaGoovnoOption(LevelPauseGUI self)//new
    {
        GameObject cgOptions = GameObject.Instantiate(self.menuItems[0].gameObject);
        cgOptions.name = "CUPAGOOVNO";
        GameObject.Destroy(cgOptions.GetComponent<LocalizationHelper>());

        Text cgText = cgOptions.GetComponent<Text>();
        cgText.text = "CUPAGOOVNO";

        Transform mapBackground = self.transform.Find("Background");
        if (mapBackground != null)
        {
            cgOptions.transform.SetParent(mapBackground.Find("Card").Find("Text"));
            self.menuItems =
            [
                self.menuItems[0],
                self.menuItems[1],
                self.menuItems[2],
                self.menuItems[3],
                cgText,
                self.menuItems[4],
                self.menuItems[5],
                self.menuItems[6],
                self.menuItems[7]
            ];
        }
        else
        {
            cgOptions.transform.SetParent(self.transform.Find("Card").Find("Text"));
            self.menuItems =
            [
                self.menuItems[0],
                self.menuItems[1],
                self.menuItems[2],
                self.menuItems[3],
                cgText,
                self.menuItems[4],
                self.menuItems[5]
            ];
        }

        cgOptions.transform.SetScale(1f, 1f);
        cgOptions.transform.SetSiblingIndex(4);
    }

    private void CupaGoovnoSelect(OptionsGUI self)
    {
        AudioManager.Play("level_menu_select");
        switch (self.verticalSelection)
        {
            case 0:
                Settings.hideNotifications = !Settings.hideNotifications;
                SettingsManager.SaveSettings();
                RenameOptions(self);
                break;
            case 1:
                Settings.hideDamageMultiplier = !Settings.hideDamageMultiplier;
                SettingsManager.SaveSettings();
                RenameOptions(self);
                break;
            case 2:
                Settings.betterPlatforms = !Settings.betterPlatforms;
                SettingsManager.SaveSettings();
                RenameOptions(self);
                break;
            case 3:
                Application.OpenURL("https://www.youtube.com/watch?v=xvFZjo5PgG0&ab_channel=Duran");
                break;
            case 4:
                self.optionMenuOpen = false;
                HideMenu(self);
                break;
        }
    }

    private void RenameOptions(OptionsGUI self)
    {
        bool on = Settings.hideNotifications;
        string text = "HIDE NOTIFICATIONS";
        string text2 = on ? text + ": ON" : text + ": OFF";
        self.currentItems[0].text.text = text2;

        on = Settings.hideDamageMultiplier;
        text = "HIDE DMG MULTIPLIER";
        text2 = on ? text + ": ON" : text + ": OFF";
        self.currentItems[1].text.text = text2;

        on = Settings.betterPlatforms;
        text = "NEW PLATFORM PARRY";
        text2 = on ? text + ": ON" : text + ": OFF";
        self.currentItems[2].text.text = text2;

        self.currentItems[3].text.text = "DON'T PRESS THIS";
        self.currentItems[4].text.text = "BACK";
    }

    private void HideMenu(OptionsGUI self)
    {
        self.HideMainOptionMenu();
        self.StartCoroutine(optionsCloseDelay_cr(self));
    }

    private IEnumerator optionsCloseDelay_cr(OptionsGUI self)
    {
        yield return new WaitForSeconds(0.1f);
        self.justClosed = false;
    }

    private bool cgOptions = false;
}
