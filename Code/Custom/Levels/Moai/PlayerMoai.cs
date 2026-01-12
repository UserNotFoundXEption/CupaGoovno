using Rewired;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace CupaGoovno;

public class PlayerMoai
{
    public void Init()
    {
        On.AbstractPlayerController.LevelInit += LevelInit;
        On.AbstractPlayerController.OnDeath += OnDeath;
    }

    public virtual void LevelInit(On.AbstractPlayerController.orig_LevelInit orig, AbstractPlayerController self, PlayerId id)
    {
        orig(self, id);
        if(Level.Current is MoaiLevel && MoaiLevel.bossPlayerId > 0)
        {
            if(id == GetBossPlayerId())
            {
                bossPlayerController = self;
            }
            mainId = 2;
            supportId = 2;
        }
    }

    public static IEnumerator PlayerBoss_cr()
    {
        Player player = PlayerManager.GetPlayer(GetBossPlayerId()).input.actions;
        while (true)
        {
            stamina.fillAmount += staminaGeneration * CupheadTime.delta;
            sliderMain.value += bossPlayerController.input.GetAxis(PlayerInput.Axis.X) * CupheadTime.delta;
            sliderSupport.value += bossPlayerController.input.GetAxis(PlayerInput.Axis.Y) * CupheadTime.delta;

            if (player.GetButtonDown(3) && mainId > 0)
            {
                arrowMainTransform.AddPosition(-40f);
                mainId--;
            }
            if (player.GetButtonDown(4) && mainId < 4)
            {
                arrowMainTransform.AddPosition(40f);
                mainId++;
            }
            if(player.GetButtonDown(2) 
                && MoaiLevel.moai.state == MoaiLevelMoai.States.Idle
                && stamina.fillAmount > staminaCostMain)
            {
                MoaiLevel.moai.Attack(mainAttacks[mainId], sliderMain.value);
                stamina.fillAmount -= staminaCostMain;
            }

            if (player.GetButtonDown(5) && supportId > 0)
            {
                arrowSupportTransform.AddPosition(-40f);
                supportId--;
            }
            if (player.GetButtonDown(6) && supportId < 4)
            {
                arrowSupportTransform.AddPosition(40f);
                supportId++;
            }
            if (player.GetButtonDown(7) && stamina.fillAmount > staminaCostSupport)
            {
                MoaiLevel.moai.Attack(supportAttacks[supportId], sliderSupport.value);
                stamina.fillAmount -= staminaCostSupport;
            }

            yield return null;
        }
    }

    /*Jump - 2
    Shoot - 3
    Ex - 4
    Change Weapon - 5
    Lock - 6
    Dash - 7*/

    public void OnDeath(On.AbstractPlayerController.orig_OnDeath orig, AbstractPlayerController self, PlayerId playerId)
    {
        orig(self, playerId);
        if(Level.Current is MoaiLevel && playerId != GetBossPlayerId())
        {
            Level.Current._OnLose();
        }
    }

    private static PlayerId GetBossPlayerId()
    {
        if(MoaiLevel.bossPlayerId == 1)
        {
            return PlayerId.PlayerOne;
        }
        else
        {
            return PlayerId.PlayerTwo;
        }
    }

    public static AbstractPlayerController bossPlayerController;
    public static RectTransform arrowMainTransform;
    public static RectTransform arrowSupportTransform;
    public static Slider sliderMain;
    public static Slider sliderSupport;
    public static Image stamina;

    private static int mainId;
    private static List<MoaiAttacks.Main> mainAttacks = [
        MoaiAttacks.Main.Rockets,
        MoaiAttacks.Main.GiantStone,
        MoaiAttacks.Main.Shitlings,
        MoaiAttacks.Main.Pusher,
        MoaiAttacks.Main.Laser
        ];
    private static int supportId;
    private static List<MoaiAttacks.Support> supportAttacks = [
        MoaiAttacks.Support.Bouncers,
        MoaiAttacks.Support.Pollen,
        MoaiAttacks.Support.Spikes,
        MoaiAttacks.Support.Crackhead,
        MoaiAttacks.Support.Baseball
        ];

    private static float staminaCostMain = 0.7f;
    private static float staminaCostSupport = 0.5f;
    private static float staminaGeneration = 0.1f;
}
