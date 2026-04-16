namespace FasterCraftReloaded;

[Harmony]
[HarmonyBefore("p1xel8ted.gyk.queueeverything")]
public static class Patches
{
    private static readonly string[] Exclude =
    [
        "zombie", "refugee", "bee", "tree", "berry", "bush", "pump", "compost", "peat", "slime", "candelabrum", "incense", "garden", "planting"
    ];

    // De-spam trackers: log once per (wgo/craft/branch) change, not once per frame.
    // Reset during a session only when the state actually changes.
    private static string _lastBuildRemoveKey;
    private static string _lastPlayerDoActionKey;
    private static string _lastCraftDoActionKey;
    private static string _lastReallyUpdateKey;

    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameSave), nameof(GameSave.GlobalEventsCheck))]
    public static void GameSave_GlobalEventsCheck_DebugWarning()
    {
        Plugin.ShowDebugWarningOnce();
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(BuildModeLogics), nameof(BuildModeLogics.ProcessRemovingCraft))]
    public static void BuildModeLogics_ProcessRemovingCraft(WorldGameObject wgo, ref float delta_time)
    {
        if (!Plugin.IncreaseBuildAndDestroySpeed.Value) return;

        if (Plugin.DebugEnabled)
        {
            var key = wgo?.obj_id ?? "null";
            if (key != _lastBuildRemoveKey)
            {
                Helpers.Log($"[Destroy] Applying x{Plugin.BuildAndDestroySpeed.Value} to removal of {key}");
                _lastBuildRemoveKey = key;
            }
        }

        delta_time *= Plugin.BuildAndDestroySpeed.Value;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(WorldGameObject), nameof(WorldGameObject.DoAction))]
    public static void WorldGameObject_DoAction(WorldGameObject other_obj, ref float delta_time)
    {
        if (!Plugin.IncreaseBuildAndDestroySpeed.Value) return;
        if (other_obj == null || !other_obj.is_player) return;

        if (Plugin.DebugEnabled)
        {
            var key = other_obj.obj_id;
            if (key != _lastPlayerDoActionKey)
            {
                Helpers.Log($"[Build] Applying x{Plugin.BuildAndDestroySpeed.Value} to player action on {key}");
                _lastPlayerDoActionKey = key;
            }
        }

        delta_time *= Plugin.BuildAndDestroySpeed.Value;
    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(CraftComponent), nameof(CraftComponent.DoAction))]
    public static void CraftComponent_DoAction(CraftComponent __instance, ref float delta_time)
    {
        if (__instance.other_obj == null) return;
        if (!__instance.other_obj.is_player) return;

        var wgoId = __instance.wgo.obj_id;
        var craftId = __instance.current_craft?.id;

        if (craftId != null && craftId.Contains(":r:"))
        {
            if (Plugin.DebugEnabled)
            {
                var key = $"{wgoId}|{craftId}|repair-skip";
                if (key != _lastCraftDoActionKey)
                {
                    Helpers.Log($"[RepairSkip] {wgoId} craft={craftId} — multiplier not applied (protects repair energy cost)");
                    _lastCraftDoActionKey = key;
                }
            }
            return;
        }

        if (Exclude.Any(wgoId.ToLowerInvariant().Contains))
        {
            if (Plugin.DebugEnabled)
            {
                var key = $"{wgoId}|{craftId}|excluded";
                if (key != _lastCraftDoActionKey)
                {
                    Helpers.Log($"[Excluded] {wgoId} craft={craftId} — workbench matches Exclude list, no speed change");
                    _lastCraftDoActionKey = key;
                }
            }
            return;
        }

        if (Plugin.DebugEnabled)
        {
            var key = $"{wgoId}|{craftId}|craft-x{Plugin.CraftSpeedMultiplier.Value}";
            if (key != _lastCraftDoActionKey)
            {
                Helpers.Log($"[Craft] {wgoId} craft={craftId} — applying x{Plugin.CraftSpeedMultiplier.Value}");
                _lastCraftDoActionKey = key;
            }
        }

        delta_time *= Plugin.CraftSpeedMultiplier.Value;
    }


    [HarmonyPrefix]
    [HarmonyAfter("p1xel8ted.GraveyardKeeper.TheSeedEqualizer", "p1xel8ted.GraveyardKeeper.AppleTreesEnhanced")]
    [HarmonyPriority(3)]
    [HarmonyPatch(typeof(CraftComponent), nameof(CraftComponent.ReallyUpdateComponent))]
    public static void CraftComponent_ReallyUpdateComponent(CraftComponent __instance, ref float delta_time)
    {
        if (__instance?.current_craft == null) return;

        var wgoId = __instance.wgo.obj_id;
        var craftId = __instance.current_craft.id;

        if (craftId != null && craftId.Contains(":r:"))
        {
            LogReallyUpdate(wgoId, craftId, "repair-skip",
                $"[RepairSkip] {wgoId} craft={craftId} — multiplier not applied (protects repair energy cost)");
            return;
        }

        if (Plugin.ModifyCompostSpeed.Value && wgoId.Contains("compost_heap"))
        {
            delta_time *= Plugin.CompostSpeedMultiplier.Value;
            LogReallyUpdate(wgoId, craftId, $"compost-x{Plugin.CompostSpeedMultiplier.Value}",
                $"[Compost] {wgoId} craft={craftId} — applying x{Plugin.CompostSpeedMultiplier.Value}");
            return;
        }

        if (Plugin.ModifyZombieMinesSpeed.Value && (wgoId.StartsWith("zombie_mine_") || wgoId.StartsWith("mine_zombie_")))
        {
            delta_time *= Plugin.ZombieMinesSpeedMultiplier.Value;
            LogReallyUpdate(wgoId, craftId, $"zmine-x{Plugin.ZombieMinesSpeedMultiplier.Value}",
                $"[ZombieMine] {wgoId} craft={craftId} — applying x{Plugin.ZombieMinesSpeedMultiplier.Value}");
            return;
        }

        if (Plugin.ModifyZombieSawmillSpeed.Value && wgoId.StartsWith("zombie_sawmill_"))
        {
            delta_time *= Plugin.ZombieSawmillSpeedMultiplier.Value;
            LogReallyUpdate(wgoId, craftId, $"zsawmill-x{Plugin.ZombieSawmillSpeedMultiplier.Value}",
                $"[ZombieSawmill] {wgoId} craft={craftId} — applying x{Plugin.ZombieSawmillSpeedMultiplier.Value}");
            return;
        }

        if (Plugin.ModifyPlayerGardenSpeed.Value && wgoId.StartsWith("garden_"))
        {
            delta_time *= Plugin.PlayerGardenSpeedMultiplier.Value;
            LogReallyUpdate(wgoId, craftId, $"pgarden-x{Plugin.PlayerGardenSpeedMultiplier.Value}",
                $"[PlayerGarden] {wgoId} craft={craftId} — applying x{Plugin.PlayerGardenSpeedMultiplier.Value}");
            return;
        }

        if (Plugin.ModifyRefugeeGardenSpeed.Value && wgoId.StartsWith("refugee_camp_garden_"))
        {
            delta_time *= Plugin.RefugeeGardenSpeedMultiplier.Value;
            LogReallyUpdate(wgoId, craftId, $"rgarden-x{Plugin.RefugeeGardenSpeedMultiplier.Value}",
                $"[RefugeeGarden] {wgoId} craft={craftId} — applying x{Plugin.RefugeeGardenSpeedMultiplier.Value}");
            return;
        }

        if (Plugin.ModifyZombieGardenSpeed.Value && wgoId.Contains("zombie_garden_"))
        {
            delta_time *= Plugin.ZombieGardenSpeedMultiplier.Value;
            LogReallyUpdate(wgoId, craftId, $"zgarden-x{Plugin.ZombieGardenSpeedMultiplier.Value}",
                $"[ZombieGarden] {wgoId} craft={craftId} — applying x{Plugin.ZombieGardenSpeedMultiplier.Value}");
            return;
        }

        if (Plugin.ModifyZombieVineyardSpeed.Value && wgoId.Contains("zombie_vineyard_"))
        {
            delta_time *= Plugin.ZombieVineyardSpeedMultiplier.Value;
            LogReallyUpdate(wgoId, craftId, $"zvineyard-x{Plugin.ZombieVineyardSpeedMultiplier.Value}",
                $"[ZombieVineyard] {wgoId} craft={craftId} — applying x{Plugin.ZombieVineyardSpeedMultiplier.Value}");
            return;
        }

        if (Exclude.Any(wgoId.ToLowerInvariant().Contains))
        {
            LogReallyUpdate(wgoId, craftId, "excluded",
                $"[Excluded] {wgoId} craft={craftId} — workbench matches Exclude list, no speed change");
            return;
        }

        delta_time *= Plugin.CraftSpeedMultiplier.Value;
        LogReallyUpdate(wgoId, craftId, $"craft-x{Plugin.CraftSpeedMultiplier.Value}",
            $"[Craft] {wgoId} craft={craftId} — applying x{Plugin.CraftSpeedMultiplier.Value}");
    }

    // Shared de-spam helper for CraftComponent_ReallyUpdateComponent: skips the formatted log
    // call unless (wgo, craft, branch) has changed since the last tick through this patch.
    private static void LogReallyUpdate(string wgoId, string craftId, string branch, string message)
    {
        if (!Plugin.DebugEnabled) return;
        var key = $"{wgoId}|{craftId}|{branch}";
        if (key == _lastReallyUpdateKey) return;
        _lastReallyUpdateKey = key;
        Helpers.Log(message);
    }
}
