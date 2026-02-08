namespace BeamMeUpGerry;

[Harmony]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class Patches
{

    private static int OneTimeCraftCount;
    private static int KnownZoneCount;
    private static int KnownNpcCount;

    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameSave), nameof(GameSave.GlobalEventsCheck))]
    public static void GameSave_GlobalEventsCheck()
    {
        UpdateZoneUpdaters();
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(WorldGameObject), nameof(WorldGameObject.Interact))]
    public static void WorldGameObject_Interact(WorldGameObject __instance)
    {
        Helpers.IsInDungeon = __instance.obj_id.ToLowerInvariant().Contains("dungeon_enter");
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(GameSave), nameof(GameSave.OnEnteredWorldZone))]
    [HarmonyPatch(typeof(BuildModeLogics), nameof(BuildModeLogics.DoPlace))]
    [HarmonyPatch(typeof(GameSave), nameof(GameSave.OnMetNPC))]
    public static void BuildModeLogics_DoPlace_Prefix()
    {
        OneTimeCraftCount = MainGame.me.save.completed_one_time_crafts.Count(a => a.Contains("blockage") || a.Contains("fix_bridge"));
        KnownZoneCount = MainGame.me.save.known_world_zones.Count;
        KnownNpcCount = MainGame.me.save.known_npcs.npcs.Count;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameSave), nameof(GameSave.OnEnteredWorldZone))]
    [HarmonyPatch(typeof(BuildModeLogics), nameof(BuildModeLogics.DoPlace))]
    [HarmonyPatch(typeof(GameSave), nameof(GameSave.OnMetNPC))]
    public static void BuildModeLogics_DoPlace_Postfix()
    {
        var newCraftCount = MainGame.me.save.completed_one_time_crafts.Count(a => a.Contains("blockage") || a.Contains("fix_bridge"));
        var newZoneCount = MainGame.me.save.known_world_zones.Count;
        var newNpcCount = MainGame.me.save.known_npcs.npcs.Count;

        if (newCraftCount > OneTimeCraftCount || newZoneCount > KnownZoneCount || newNpcCount > KnownNpcCount)
        {
            Plugin.InitConfiguration();
        }
    }

    private static void UpdateZoneUpdaters()
    {
        if (!MainGame.me || MainGame.me.save == null) return;

        MainGame.me.save.completed_one_time_crafts = MainGame.me.save.completed_one_time_crafts.Distinct().ToList();

        var steepCoals = WorldMap._objs.Where(a => a.obj_id == "steep_coal").ToList();
        foreach (var steepCoal in steepCoals)
        {
            if (steepCoal.GetMyWorldZoneId() != "flat_under_waterflow") continue;

            var kzu = steepCoal.gameObject.TryAddComponent<KnownZoneUpdater>();
            kzu.Initialize(Constants.NorthCoalZone);
        }

        var zombieTrees = WorldMap._objs.Where(a => a.obj_id == "tree_big_sawmill").ToList();
        foreach (var zombieTree in zombieTrees)
        {
            if (zombieTree.GetMyWorldZoneId() != "flat_under_waterflow_2") continue;

            var kzu = zombieTree.gameObject.TryAddComponent<KnownZoneUpdater>();
            kzu.Initialize(Constants.ZombieSawmill);
        }

        MainGame.me.save.known_world_zones.TryAdd(Constants.SandMoundZone);
        MainGame.me.save.known_world_zones.TryAdd(Constants.ClayPitZone);

        MainGame.me.save.known_world_zones = MainGame.me.save.known_world_zones.Distinct().ToList();
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


    [HarmonyPostfix]
    [HarmonyPatch(typeof(Item), nameof(Item.GetGrayedCooldownPercent))]
    public static void Item_GetGrayedCooldownPercent(Item __instance, ref int __result)
    {
        if (__instance is not {id: Constants.Hearthstone}) return;

        __result = 0;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Item), nameof(Item.UseItem))]
    public static bool Item_UseItem(Item __instance, ref GameRes __result)
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

    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameSettings), nameof(GameSettings.Init))]
    [HarmonyPatch(typeof(GameSettings), nameof(GameSettings.ApplyLanguageChange))]
    public static void GameSettings_Init()
    {
        Plugin.UpdateLists();
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(SpeechBubbleGUI), nameof(SpeechBubbleGUI.SpeechText), typeof(string))]
    public static void SpeechBubbleGUI_SpeechText(string s, ref string __result)
    {
        var term = Language.GetTerm(s);

        if (term is Language.Terms.None) return;

        __result = Language.GetTranslation(term);
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(MultiAnswerGUI), nameof(MultiAnswerGUI.ShowAnswers), typeof(List<AnswerVisualData>), typeof(bool))]
    public static void MultiAnswerGUI_ShowAnswers(MultiAnswerGUI __instance)
    {
        var playerRequest = MultiAnswerGUI.talker_wgo == MainGame.me.player;

        if (playerRequest && Plugin.IncreaseMenuAnimationSpeed.Value)
        {
            __instance.anim_delay /= 3f;
            __instance.anim_time /= 3f;
        }
    }
}
