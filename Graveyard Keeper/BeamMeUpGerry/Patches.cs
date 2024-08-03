namespace BeamMeUpGerry;

[Harmony]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class Patches
{
    private static int OneTimeCraftCount;
    private static int KnownZoneCount;
    private static int KnownNpcCount;

    [HarmonyPrefix]
    [HarmonyPatch(typeof(GameSave), nameof(GameSave.OnEnteredWorldZone))]
    [HarmonyPatch(typeof(BuildModeLogics), nameof(BuildModeLogics.DoPlace))]
    [HarmonyPatch(typeof(GameSave), nameof(GameSave.OnMetNPC))]
    public static void BuildModeLogics_DoPlace_Prefix()
    {
        OneTimeCraftCount = MainGame.me.save.completed_one_time_crafts.Count(a => a.Contains("blockage"));
        KnownZoneCount = MainGame.me.save.known_world_zones.Count;
        KnownNpcCount = MainGame.me.save.known_npcs.npcs.Count;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameSave), nameof(GameSave.OnEnteredWorldZone))]
    [HarmonyPatch(typeof(BuildModeLogics), nameof(BuildModeLogics.DoPlace))]
    [HarmonyPatch(typeof(GameSave), nameof(GameSave.OnMetNPC))]
    public static void BuildModeLogics_DoPlace_Postfix()
    {
        var newCraftCount = MainGame.me.save.completed_one_time_crafts.Count(a => a.Contains("blockage"));
        var newZoneCount = MainGame.me.save.known_world_zones.Count;
        var newNpcCount = MainGame.me.save.known_npcs.npcs.Count;

        if (newCraftCount > OneTimeCraftCount || newZoneCount > KnownZoneCount || newNpcCount > KnownNpcCount)
        {
            Plugin.InitConfiguration();
        }
    }

    [HarmonyPostfix]
    [HarmonyWrapSafe]
    [HarmonyPatch(typeof(MultiAnswerGUI), nameof(MultiAnswerGUI.Update))]
    public static void MultiAnswerGUI_Update(MultiAnswerGUI __instance)
    {
        try
        {
            if (!MultiAnswerGUI.talker_wgo.is_player || !Helpers.MakingChoice) return;

            Plugin.CachedPlayer ??= ReInput.players.GetPlayer(0);

            var find = __instance._answers.Find(a => a._answer_id.Contains("cancel"));
            if (find && (Input.GetKeyUp(KeyCode.Escape) || LazyInput.gamepad_active && (Plugin.CachedPlayer.GetButtonUp("B") || Plugin.CachedPlayer.GetButtonUp("Back"))))
            {
                find.OnChosen();
            }
        }
        catch (Exception)
        {
            // ignored
        }
    }

    private static Bounds NorthCoalBounds = new(new Vector3(-834f, 6125f, 0), new Vector3(750f, 750f, 0f));

    [HarmonyPostfix]
    [HarmonyPatch(typeof(PlayerComponent), nameof(PlayerComponent.Update))]
    public static void PlayerComponent_Update()
    {
        CheckNorthCoal();
    }

    private static void CheckNorthCoal()
    {
        if (MainGame.me.save.known_world_zones.Contains(Constants.NorthCoal)) return;
        var player = MainGame.me.player;
        if (player && NorthCoalBounds.Contains(player.transform.position))
        {
            MainGame.me.save.known_world_zones.Add(Constants.NorthCoal);
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Item), nameof(Item.GetGrayedCooldownPercent))]
    public static void Item_GetGrayedCooldownPercent(ref Item __instance, ref int __result)
    {
        if (__instance is not {id: Constants.Hearthstone}) return;

        __result = 0;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameSave), nameof(GameSave.OnFinishedCraft))]
    public static void GameSave_OnFinishedCraft(CraftDefinition craft)
    {
        if (craft.id.Contains("blockage"))
        {
            Plugin.Log.LogInfo("Blockage Crafted (blockage to Quarry etc) - Refreshing List");
            Plugin.InitConfiguration();
        }
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameSave), nameof(GameSave.OnEnteredWorldZone))]
    public static void GameSave_OnEnteredWorldZone(WorldZone z)
    {
        if (Array.IndexOf(Helpers.BlockageLocations, z.id) != -1)
        {
            Helpers.Log("Entered Blockage Location (meaning the blockage has been removed) - Refreshing List");
            Plugin.InitConfiguration();
        }
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

        if (Plugin.DebugEnabled.Value)
        {
            for (var index = 0; index < LocationLists.Locations.Count; index++)
            {
                var loc = LocationLists.Locations[index];
                Plugin.Log.LogInfo($"Page: {index + 1}");
                foreach (var locItem in loc)
                {
                    Plugin.Log.LogInfo($"LocationItem: {locItem.id}");
                }
            }
        }

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