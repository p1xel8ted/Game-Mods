namespace BeamMeUpGerry;

[Harmony]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class Patches
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(Item), nameof(Item.GetGrayedCooldownPercent))]
    public static void Item_GetGrayedCooldownPercent(ref Item __instance, ref int __result)
    {
        if (__instance is not {id: Constants.Hearthstone}) return;

        __result = 0;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Item), nameof(Item.UseItem))]
    public static bool Item_UseItem(ref Item __instance, ref GameRes __result)
    {
        if (!Plugin.EnableListExpansion.Value) return true;
        if (__instance is not {id: Constants.Hearthstone}) return true;

        if (__instance.definition.cooldown.has_expression)
        {
            if (__instance.GetGrayedCooldownPercent() != 0)
            {
                Debug.LogError("Can't use item '" + __instance.id + "' because of cooldown");
                __result = new GameRes();
            }

            MainGame.me.player.SetParam("_cooldown_" + __instance.id, __instance.RecalculateTotalCooldown());
        }

        MainGame.me.save.quests.CheckKeyQuests("use_item_" + __instance.id);
        Stats.DesignEvent("Item:Use:" + __instance.id);
        GUIElements.me.hud.toolbar.Redraw();
        if (!string.IsNullOrEmpty(__instance.definition.on_use_snd))
        {
            Sounds.PlaySound(__instance.definition.on_use_snd);
        }

        MainGame.me.player.AddToParams(__instance.definition.params_on_use);
        var gameRes = new GameRes(__instance.definition.params_on_use);
        __result = gameRes;


        Menus.ShowMultiAnswer(LocationLists.Locations[0]);


        return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(MultiAnswerGUI), nameof(MultiAnswerGUI.ShowAnswers), typeof(List<AnswerVisualData>), typeof(bool))]
    public static void MultiAnswerGUI_ShowAnswers(MultiAnswerGUI __instance, ref List<AnswerVisualData> answers)
    {
        // foreach (var saveKnownWorldZone in MainGame.me.save.known_world_zones)
        // {
        //     Plugin.Log.LogWarning($"Zone: {saveKnownWorldZone}");
        // }
        
     
        var playerRequest = MultiAnswerGUI.talker_wgo == MainGame.me.player;
        if (playerRequest && Plugin.IncreaseMenuAnimationSpeed.Value)
        {
            __instance.anim_delay /= 3f;
            __instance.anim_time /= 3f;
        }

        // var pageTwo = answers.Count == LocationLists.AnswersPageTwo.Count &&
        //               answers.All(l1Item => LocationLists.AnswersPageTwo.Any(l2Item => l1Item.id == l2Item.id));
        //
        // var pageThree = answers.Count == LocationLists.AnswersPageThree.Count &&
        //                 answers.All(l1Item => LocationLists.AnswersPageThree.Any(l2Item => l1Item.id == l2Item.id));

        // if (pageTwo || pageThree)
        // {
        //     answers = Helpers.ValidateAnswerList(answers);
        // }
    }
}