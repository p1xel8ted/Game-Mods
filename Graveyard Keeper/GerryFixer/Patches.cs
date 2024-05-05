using System.Linq;
using GYKHelper;
using HarmonyLib;
using UnityEngine;

namespace GerryFixer;

[HarmonyPatch]
public static class Patches
{
    internal static void FixGerry()
    {
        Plugin.Log.LogInfo($"Running FixGerry as Player has spawned in.");
        if (!MainGame.game_started) return;
        
        if (Plugin.Debug.Value)
        {
            foreach (var obj in CrossModFields.WorldObjects)
            {
                Plugin.Log.LogInfo($"WorldObjects: {obj.obj_id}, CustomTag: {obj.custom_tag}, Loc: {obj.pos3}");
            }

            foreach (var obj in CrossModFields.WorldNpcs)
            {
                Plugin.Log.LogInfo($"WorldNpcs: {obj.obj_id}, CustomTag: {obj.custom_tag}, Loc: {obj.pos3}");
            }

            foreach (var obj in CrossModFields.WorldVendors)
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
                WorldMap.SpawnWGO(MainGame.me.world_root, "crafting_skull_3", new Vector3(9648.0f, -10850.0f, -2268.4f), "crafting_skull_3");
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
                WorldMap.SpawnWGO(MainGame.me.world_root, "tavern_skull", new Vector3(16352.0f, -9086.0f, -1899.5f), "tavern_skull");
            }
            else
            {
                Plugin.Log.LogInfo($"Player doesnt appear to have the player tavern in their known zones. Not spawning tavern gerry.");
            }
        }
    }

    [HarmonyPriority(1)]
    [HarmonyPostfix]
    [HarmonyPatch(typeof(WorldMap), nameof(WorldMap.GetWorldGameObjectByCustomTag))]
    public static void WorldMap_GetWorldGameObjectByCustomTag(string custom_tag, ref bool ignore_not_found_error, ref WorldGameObject __result)
    {
        if (!MainGame.game_started) return;
        ignore_not_found_error = true;


        if (!Plugin.AttemptToFixCutsceneGerrys.Value) return;
        if (custom_tag.StartsWith("skulls_")) return;
        if (custom_tag == "tavern_skull" || custom_tag.StartsWith("crafting_skull")) return;
        if (!custom_tag.Contains("skull")) return;
        if (__result != null) return;

        Plugin.Log.LogInfo($"Object: {custom_tag} not found. Trying to spawn one. This may or may not work.");
        var a = MainGame.me.player_pos;
        var newLoc = new Vector3(a.x, a.y + 100f, a.z);
        __result = WorldMap.SpawnWGO(MainGame.me.world_root, custom_tag, newLoc, custom_tag);
    }
}