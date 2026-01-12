using Blender.Content;
using Blender.Utility;
using DialoguerCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class CustomNpcs
{
    public void Init()
    {
        RegisterNpcs();
    }

    private void RegisterNpcs()
    {
        RegisterCappuccinaNpc();
    }

    private void RegisterCappuccinaNpc()
    {
        List<AbstractDialoguePhase> phases =
        [
            new BranchedTextPhase("npc_cappuccina_1", ["npc_cappuccina_choice_1", "npc_cappuccina_choice_2", "npc_cappuccina_choice_3", "npc_cappuccina_choice_4", "npc_cappuccina_choice_5", "npc_cappuccina_choice_6"], null, false, null, null, null, null, 0, Rect.zero, [1, 2, 3, 4, 5, 6], 1, 0),
            new SendMessagePhase("Cappuccina", "Moai", [7]),
            new SendMessagePhase("Cappuccina", "MoaiRedLaser", [7]),
            new SendMessagePhase("Cappuccina", "ULTRAMOAI", [7]),
            new SendMessagePhase("Cappuccina", "MoaiPvpPlayerOne", [7]),
            new SendMessagePhase("Cappuccina", "MoaiPvpPlayerTwo", [7]),
            new SendMessagePhase("Cappuccina", "Rift", [7]),
            new EndPhase()
        ];

        string id = "npc_cappuccina_dialogue";
        DialoguerDialogues dialogue = SceneRegistries.Dialogues.Register(id,
            new DialoguerDialogue(id, 0,
            new DialoguerVariables([], [], []),
            phases));

        SceneRegistries.MapEntities.Add("CappuccinaNpc", new MapEntityInfo(path,
            ["scene_map_world_1"], new Vector3(5.8f, 5.8f, 0))
                .SetDialogue(dialogue)
                .SetSetupAction(CappuccinaNpc.AddBehaviour)
                .SetSpeechBubbleOffset(new Vector2(-0.3f, -1f)));
    }

    private const string path = "CupaGoovno:npcs";
}
