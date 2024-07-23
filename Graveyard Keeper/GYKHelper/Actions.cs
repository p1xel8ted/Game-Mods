namespace GYKHelper;

[HarmonyPatch]
[HarmonyPriority(1)]
public static class Actions
{
    private readonly static string[] SafeGerryTags =
    [
        "tavern_skull",
        "talking_skull",
        "crafting_skull",
        "crafting_skull_1",
        "crafting_skull_2",
        "crafting_skull_3",
        "crafting_skull_4",
        "crafting_skull_5",
        "crafting_skull_6",
        "crafting_skull_7",
        "crafting_skull_8",
        "crafting_skull_9",
        "crafting_skull_10",
        "talking_skull_in_tavern"
    ];

    //public static Action GameStatusInGame;
    public static Action PlayerSpawnedIn;
    public static Action ReturnToMenu;
    public static Action GameStatusUndefined;
    public static Action GameStartedPlaying;

    
    public static Action EndOfDayPrefix;
    public static Action EndOfDayPostfix;
    
    
    public static Action GameBalanceLoad;
    public static Action<WorldGameObject> WorldGameObjectInteract;

    public static Action<WorldGameObject, WorldGameObject> WorldGameObjectInteractPrefix;
    
    [HarmonyPrefix]
    [HarmonyPriority(1)]
    [HarmonyPatch(typeof(EnvironmentEngine), nameof(EnvironmentEngine.OnEndOfDay))]
    public static void EnvironmentEngine_OnEndOfDay_Prefix()
    {
        if (EndOfDayPrefix?.GetInvocationList().Length > 0)
        {
            var delegates = EndOfDayPrefix.GetInvocationList();
            Plugin.Log.LogInfo($"End of day beginning. Invoking EndOfDayPrefix Action for {delegates.Length} attached mods.");
            foreach (var del in delegates)
            {
                Plugin.Log.LogInfo($"Type: {del.Method.DeclaringType}, Method: {del.Method.Name}");
            }

            EndOfDayPrefix.Invoke();
        }
        else
        {
            Plugin.Log.LogInfo("End of day beginning. No mods attached to EndOfDayPrefix Action.");
        }
    }
    
    [HarmonyPostfix]
    [HarmonyPriority(1)]
    [HarmonyPatch(typeof(EnvironmentEngine), nameof(EnvironmentEngine.OnEndOfDay))]
    public static void EnvironmentEngine_OnEndOfDay_Postfix()
    {
        if (EndOfDayPostfix?.GetInvocationList().Length > 0)
        {
            var delegates = EndOfDayPostfix.GetInvocationList();
            Plugin.Log.LogInfo($"End of day finished. Invoking EndOfDayPostfix Action for {delegates.Length} attached mods.");
            foreach (var del in delegates)
            {
                Plugin.Log.LogInfo($"Type: {del.Method.DeclaringType}, Method: {del.Method.Name}");
            }

            EndOfDayPostfix.Invoke();
        }
        else
        {
            Plugin.Log.LogInfo("End of day finished. No mods attached to EndOfDayPostfix Action.");
        }
    }
    
    [HarmonyPrefix]
    [HarmonyPriority(1)]
    [HarmonyPatch(typeof(InGameMenuGUI), nameof(InGameMenuGUI.ReturnToMainMenu))]
    public static void InGameMenuGUI_ReturnToMainMenu()
    {
        MainGame.game_started = false;
        Tools.SetAllInteractionsFalse();
        if (ReturnToMenu?.GetInvocationList().Length > 0)
        {
            var delegates = ReturnToMenu.GetInvocationList();
            Plugin.Log.LogInfo($"Player returning to main menu. Invoking ReturnToMenu Action for {delegates.Length} attached mods.");
            foreach (var del in delegates)
            {
                Plugin.Log.LogInfo($"Type: {del.Method.DeclaringType}, Method: {del.Method.Name}");
            }
    
            ReturnToMenu.Invoke();
        }
        else
        {
            Plugin.Log.LogInfo("Player returning to main menu. No mods attached to ReturnToMenu Action.");
        }
    }


    [HarmonyPostfix]
    [HarmonyPriority(1)]
    [HarmonyPatch(typeof(PlayerComponent), nameof(PlayerComponent.SpawnPlayer), typeof(bool), typeof(Item))]
    public static void PlayerComponent_SpawnPlayer(bool is_local_player)
    {
        if (!is_local_player) return;
        Plugin.Log.LogInfo("Player spawned in. Invoking PlayerSpawnedIn Action for attached mods.");
        PlayerSpawnedIn?.Invoke();
    }

    [HarmonyPostfix]
    [HarmonyPriority(1)]
    [HarmonyPatch(typeof(GameSave), nameof(GameSave.GlobalEventsCheck))]
    public static void GameSave_GlobalEventsCheck()
    {
        if (GameStartedPlaying?.GetInvocationList().Length > 0)
        {
            var delegates = GameStartedPlaying.GetInvocationList();
            Plugin.Log.LogInfo($"Final load task complete. Game starting. Invoking GameStartedPlaying Action for {delegates.Length} attached mods.");
            foreach (var del in delegates)
            {
                Plugin.Log.LogInfo($"Type: {del.Method.DeclaringType}, Method: {del.Method.Name}");
            }

            GameStartedPlaying.Invoke();
        }
        else
        {
            Plugin.Log.LogInfo("Final load task complete. Game starting. No mods attached to GameStartedPlaying Action.");
        }
    }

    [HarmonyPostfix]
    [HarmonyPriority(1)]
    [HarmonyPatch(typeof(GameBalance), nameof(GameBalance.LoadGameBalance))]
    private static void GameBalance_LoadGameBalance_Postfix()
    {
        if (GameBalanceLoad?.GetInvocationList().Length > 0)
        {
            var delegates = GameBalanceLoad.GetInvocationList();
            Plugin.Log.LogInfo($"Game balance loaded. Invoking GameBalanceLoad Action for {delegates.Length} attached mods.");
            foreach (var del in delegates)
            {
                Plugin.Log.LogInfo($"Type: {del.Method.DeclaringType}, Method: {del.Method.Name}");
            }

            GameBalanceLoad.Invoke();
        }
        else
        {
            Plugin.Log.LogInfo("Game balance loaded. No mods attached to GameStartedPlaying Action.");
        }
    }

    [HarmonyPostfix]
    [HarmonyPriority(1)]
    [HarmonyPatch(typeof(WorldGameObject), nameof(WorldGameObject.Interact))]
    private static void WorldGameObject_Interact_Postfix(ref WorldGameObject __instance)
    {
        if (WorldGameObjectInteract?.GetInvocationList().Length > 0)
        {
            var delegates = WorldGameObjectInteract.GetInvocationList();
            Plugin.Log.LogInfo($"WGO interacted with (postfix). Invoking WorldGameObjectInteract Action for {delegates.Length} attached mods.");
            foreach (var del in delegates)
            {
                Plugin.Log.LogInfo($"Type: {del.Method.DeclaringType}, Method: {del.Method.Name}");
            }

            WorldGameObjectInteract.Invoke(__instance);
        }
        else
        {
            Plugin.Log.LogInfo("WGO interacted with (postfix). No mods attached to WorldGameObjectInteract Action.");
        }
    }
    
    [HarmonyPrefix]
    [HarmonyPriority(1)]
    [HarmonyPatch(typeof(WorldGameObject), nameof(WorldGameObject.Interact))]
    private static void WorldGameObject_Interact_Prefix(ref WorldGameObject __instance, ref WorldGameObject other_obj)
    {
        if (WorldGameObjectInteractPrefix?.GetInvocationList().Length > 0)
        {
            var delegates = WorldGameObjectInteractPrefix.GetInvocationList();
            Plugin.Log.LogInfo($"WGO interacted with (prefix). Invoking WorldGameObjectInteractPrefix Action for {delegates.Length} attached mods.");
            foreach (var del in delegates)
            {
                Plugin.Log.LogInfo($"Type: {del.Method.DeclaringType}, Method: {del.Method.Name}");
            }

            WorldGameObjectInteractPrefix.Invoke(__instance, other_obj);
        }
        else
        {
            Plugin.Log.LogInfo("WGO interacted with (prefix). No mods attached to WorldGameObjectInteractPrefix Action.");
        }
    }

    public static void WorldGameObject_Interact(WorldGameObject instance, WorldGameObject other_obj)
    {
        if (!MainGame.game_started || instance == null) return;

        Plugin.Log.LogInfo($"Object: {instance.obj_id}, CustomTag: {instance.custom_tag}, Location: {instance.pos3}, Zone:{instance.GetMyWorldZoneId()}");
        //Where's Ma Storage
        CrossModFields.PreviousWgoInteraction = CrossModFields.CurrentWgoInteraction;
        CrossModFields.CurrentWgoInteraction = instance;
        CrossModFields.IsVendor = instance.vendor != null;
        CrossModFields.IsCraft = other_obj.is_player && instance.obj_def.interaction_type != ObjectDefinition.InteractionType.Chest && instance.obj_def.has_craft;
        //Log($"IsCraft: {CrossModFields.IsCraft}",true);
        CrossModFields.IsChest = instance.obj_def.interaction_type == ObjectDefinition.InteractionType.Chest;
        CrossModFields.IsBarman = instance.obj_id.ToLowerInvariant().Contains("barman");
        CrossModFields.IsTavernCellarRack = instance.obj_id.ToLowerInvariant().Contains("tavern_cellar_rack");
        CrossModFields.IsRefugee = instance.obj_id.ToLowerInvariant().Contains("refugee");
        CrossModFields.IsWritersTable = instance.obj_id.ToLowerInvariant().Contains("writer");
        CrossModFields.IsSoulBox = instance.obj_id.ToLowerInvariant().Contains("soul_container");
        CrossModFields.IsChurchPulpit = instance.obj_id.ToLowerInvariant().Contains("pulpit");
        CrossModFields.IsMoneyLender = instance.obj_id.ToLowerInvariant().Contains("lender");

        if (instance.obj_def.inventory_size > 0)
        {
            if (instance.obj_id.Length <= 0)
            {
                instance.data.sub_name = "Unknown#" + instance.GetMyWorldZoneId();
            }
            else
            {
                instance.data.sub_name = instance.obj_id + "#" + instance.GetMyWorldZoneId();
            }
        }

        //Beam Me Up & Save Now
        CrossModFields.IsInDungeon = instance.obj_id.ToLowerInvariant().Contains("dungeon_enter");
        // Log($"[InDungeon]: {CrossModFields.IsInDungeon}");

        //I Build Where I Want
        if (instance.obj_def.interaction_type is not ObjectDefinition.InteractionType.None)
        {
            CrossModFields.CraftAnywhere = false;
        }

        //Beam Me Up Gerry
        CrossModFields.TalkingToNpc(instance.obj_def.IsNPC() && !instance.obj_id.Contains("zombie"));

        //Log($"[WorldGameObject.Interact]: Instance: {__instance.obj_id}, InstanceIsPlayer: {__instance.is_player},  Other: {other_obj.obj_id}, OtherIsPlayer: {other_obj.is_player}");
    }

    public static void CleanGerries()
    {
        if (!MainGame.game_started) return;

        if (!Tools.TutorialDone()) return;
        //get all gerry objects + a few extras
        var otherGerrys = Object.FindObjectsOfType<WorldGameObject>(true).Where(a => a.obj_id.ToLowerInvariant().Contains("skull")).ToList();

        //removes the extras - mainly the skull objects in the dungeon
        otherGerrys.RemoveAll(a => a.obj_id.ToLowerInvariant().StartsWith("skulls"));

        //log each gerry object found
        foreach (var g in otherGerrys.Where(g => g != null))
        {
            Plugin.Log.LogInfo($"AllGerries: Gerry: {g.obj_id}, CustomTag: {g.custom_tag}, POS: {g.pos3}, Location: {g.GetMyWorldZoneId()}");
        }

        //find gerrys that match any gerrys made by mods
        var gerrys = WorldMap.GetWorldGameObjectsByCustomTag(CrossModFields.ModGerryTag);

        //log each mod_gerry object found
        foreach (var g in gerrys.Where(g => g != null))
        {
            Plugin.Log.LogInfo($"AllModGerries: Gerry: {g.obj_id}, CustomTag: {g.custom_tag}, POS: {g.pos3}, Location: {g.GetMyWorldZoneId()}");
        }

        //remove each gerry, ensuring no gerrys on the safeTag list are removed
        foreach (var gerry in gerrys.Where(gerry => gerry != null))
        {
            if (SafeGerryTags.Contains(gerry.custom_tag)) continue;
            Plugin.Log.LogInfo($"Destroyed Gerry: {gerry.obj_id}, CustomTag: {gerry.custom_tag}, POS: {gerry.pos3}, Location: {gerry.GetMyWorldZoneId()}");
            gerry.DestroyMe();
        }

        //remove each mod gerry, ensuring no gerrys on the safeTag list are removed
        foreach (var gerry in otherGerrys.Where(gerry => gerry != null))
        {
            if (SafeGerryTags.Contains(gerry.custom_tag)) continue;
            Plugin.Log.LogInfo($"Destroyed Gerry: {gerry.obj_id}, CustomTag: {gerry.custom_tag}, POS: {gerry.pos3}, Location: {gerry.GetMyWorldZoneId()}");
            gerry.DestroyMe();
        }
    }
}