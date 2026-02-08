namespace GerryFixer;

[Harmony]
public static class Patches
{
    private static WorldGameObject SpawnWGOWithTag(string obj_id, Vector3 position, string custom_tag)
    {
        var wgo = WorldMap.SpawnWGO(MainGame.me.world_root, obj_id, position);
        wgo.custom_tag = custom_tag;
        wgo.RecalculateZoneBelonging();
        return wgo;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameSave), nameof(GameSave.GlobalEventsCheck))]
    public static void GameSave_GlobalEventsCheck()
    {
        FixGerry();
    }

    internal static void FixGerry()
    {
        Plugin.Log.LogInfo($"Running FixGerry as Player has spawned in.");
        if (!MainGame.game_started) return;

        if (Plugin.Debug.Value)
        {
            foreach (var obj in WorldMap._objs)
            {
                Plugin.Log.LogInfo($"WorldObjects: {obj.obj_id}, CustomTag: {obj.custom_tag}, Loc: {obj.pos3}");
            }

            foreach (var obj in WorldMap._npcs)
            {
                Plugin.Log.LogInfo($"WorldNpcs: {obj.obj_id}, CustomTag: {obj.custom_tag}, Loc: {obj.pos3}");
            }

            foreach (var obj in WorldMap._vendors)
            {
                Plugin.Log.LogInfo($"WorldVendors: {obj.id}");
            }

            foreach (var zone in MainGame.me.save.known_world_zones)
            {
                Plugin.Log.LogInfo($"KnownWorldZone: {zone}");
            }
        }

        if (Plugin.SpawnTavernMorgueGerry.Value && WorldMap.GetWorldGameObjectByCustomTag("crafting_skull_3", true) == null)
        {
            if (MainGame.me.save.known_world_zones.Contains("morgue"))
            {
                Plugin.Log.LogInfo($"Object: 'crafting_skull_3' not found. Player has 'morgue' in their known zones. Spawning a 'crafting_skull_3' Gerry.");
                SpawnWGOWithTag("crafting_skull_3", new Vector3(9648.0f, -10850.0f, -2268.4f), "crafting_skull_3");
            }
            else
            {
                Plugin.Log.LogInfo($"Player doesnt have the morgue in their known zones. Not spawning morgue gerry.");
            }
        }

        if (Plugin.SpawnTavernCellarGerry.Value && WorldMap.GetWorldGameObjectByCustomTag("tavern_skull", true) == null)
        {
            if (MainGame.me.save.known_world_zones.Any(a => a.StartsWith("players_tavern")))
            {
                Plugin.Log.LogInfo($"Object: 'tavern_skull' not found. Player has 'players_tavern' in their known zones. Spawning a 'tavern cellar' Gerry.");
                SpawnWGOWithTag("tavern_skull", new Vector3(16352.0f, -9086.0f, -1899.5f), "tavern_skull");
            }
            else
            {
                Plugin.Log.LogInfo($"Player doesnt appear to have the player tavern in their known zones. Not spawning tavern gerry.");
            }
        }
    }

    private static bool IsCutsceneSkullTag(string custom_tag)
    {
        if (!custom_tag.Contains("skull")) return false;
        if (custom_tag.StartsWith("skulls_")) return false;
        if (custom_tag == "tavern_skull" || custom_tag.StartsWith("crafting_skull")) return false;
        return true;
    }

    [HarmonyPriority(1)]
    [HarmonyPrefix]
    [HarmonyPatch(typeof(WorldMap), nameof(WorldMap.GetWorldGameObjectByCustomTag))]
    public static void WorldMap_GetWorldGameObjectByCustomTag_Prefix(string custom_tag, ref bool ignore_not_found_error)
    {
        if (!MainGame.game_started) return;
        if (!Plugin.AttemptToFixCutsceneGerrys.Value) return;
        if (!IsCutsceneSkullTag(custom_tag)) return;

        ignore_not_found_error = true;
    }

    [HarmonyPriority(1)]
    [HarmonyPostfix]
    [HarmonyPatch(typeof(WorldMap), nameof(WorldMap.GetWorldGameObjectByCustomTag))]
    public static void WorldMap_GetWorldGameObjectByCustomTag_Postfix(string custom_tag, ref WorldGameObject __result)
    {
        if (!MainGame.game_started) return;
        if (!Plugin.AttemptToFixCutsceneGerrys.Value) return;
        if (!IsCutsceneSkullTag(custom_tag)) return;
        if (__result != null) return;

        Plugin.Log.LogInfo($"Object: {custom_tag} not found. Trying to spawn one. This may or may not work.");
        var a = MainGame.me.player_pos;
        __result = SpawnWGOWithTag(custom_tag, new Vector3(a.x, a.y + 100f, a.z), custom_tag);
    }
}
