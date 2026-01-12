using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class CappuccinaNpc : MonoBehaviour
{
    public static void AddBehaviour(GameObject gameObject)
    {
        if (gameObject.GetComponent<CappuccinaNpc>() == null)
        {
            gameObject.AddComponent<CappuccinaNpc>();
        }
    }

    public void Start()
    {
        AddDialoguerEvents();
    }

    public void OnDestroy()
    {
        RemoveDialoguerEvents();
    }

    public void AddDialoguerEvents()
    {
        Dialoguer.events.onMessageEvent += OnDialoguerMessageEvent;
    }

    public void RemoveDialoguerEvents()
    {
        Dialoguer.events.onMessageEvent -= OnDialoguerMessageEvent;
    }

    public void OnDialoguerMessageEvent(string message, string metadata)
    {
        if(message == "Cappuccina")
        {
            switch (metadata)
            {
                case "Moai":
                    MoaiLevel.redLaser = false;
                    MoaiLevel.ultra = false;
                    MoaiLevel.bossPlayerId = -1;
                    Moai.Load(this);
                    break;
                case "MoaiRedLaser":
                    MoaiLevel.redLaser = true;
                    MoaiLevel.ultra = false;
                    MoaiLevel.bossPlayerId = -1;
                    Moai.Load(this);
                    break;
                case "ULTRAMOAI":
                    MoaiLevel.redLaser = true;
                    MoaiLevel.ultra = true;
                    MoaiLevel.bossPlayerId = -1;
                    Moai.Load(this);
                    break;
                case "MoaiPvpPlayerOne":
                    MoaiLevel.redLaser = true;
                    MoaiLevel.ultra = false;
                    MoaiLevel.bossPlayerId = 1;
                    Moai.Load(this);
                    break;
                case "MoaiPvpPlayerTwo":
                    MoaiLevel.redLaser = true;
                    MoaiLevel.ultra = false;
                    MoaiLevel.bossPlayerId = 2;
                    Moai.Load(this);
                    break;
                case "Rift":
                    Rift.Load(this);
                    break;
            }
        }
    }
}
