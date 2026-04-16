namespace TreesNoMore;

[Harmony]
public static class Patches
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameSave), nameof(GameSave.GlobalEventsCheck))]
    public static void GameSave_GlobalEventsCheck()
    {
        if (Plugin.DebugEnabled) Helpers.Log("[GlobalEventsCheck] postfix firing — show debug warning if needed, then destroy tracked trees");
        Helpers.ShowDebugWarningOnce();
        DestroyTrees();
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(InGameMenuGUI), nameof(InGameMenuGUI.ReturnToMainMenu))]
    public static void InGameMenuGUI_ReturnToMainMenu()
    {
        if (Plugin.DebugEnabled) Helpers.Log("[ReturnToMainMenu] saving tracked trees before exit to main menu");
        Plugin.SaveTrees();
    }

    internal static void DestroyTrees()
    {
        var sw = new System.Diagnostics.Stopwatch();
        sw.Start();

        if (!Plugin.LoadTrees())
        {
            if (Plugin.DebugEnabled) Helpers.Log("[DestroyTrees] LoadTrees returned false — nothing to destroy this load");
            return;
        }

        if (Plugin.DebugEnabled) Helpers.Log($"[DestroyTrees] starting; {Plugin.Trees.Count} tracked tree(s) on record, search distance {Plugin.TreeSearchDistance.Value}");

        // Create a new list to hold the trees that you want to destroy.
        List<WorldGameObject> treesToDestroy = [];
        var scannedCount = 0;

        foreach (var tree in WorldMap.objs.Where(o => o.name.Contains("tree") && !o.name.Contains("bees") && !o.name.Contains("apple")))
        {
            scannedCount++;
            var treeExists = Plugin.Trees.Any(x => Vector3.Distance(x.location, tree.pos3) <= Plugin.TreeSearchDistance.Value);
            if (treeExists)
            {
                if (Plugin.DebugEnabled) Helpers.Log($"[DestroyTrees] match — world tree {tree.obj_id} at {tree.pos3} is in tracked list, queued for removal");
                treesToDestroy.Add(tree);
            }
        }

        // Now you can destroy the trees without modifying the collection you're iterating over.
        foreach (var tree in treesToDestroy)
        {
            if (Plugin.DebugEnabled) Helpers.Log($"[DestroyTrees] removing world object {tree.obj_id} at {tree.pos3}");
            WorldMap.objs.Remove(tree); // removing the reference from WorldMap.objs
            UnityEngine.Object.DestroyImmediate(tree);
        }

        sw.Stop();
        if (Plugin.DebugEnabled) Helpers.Log($"[DestroyTrees] removed {treesToDestroy.Count} of {scannedCount} scanned world tree(s) (tracked total {Plugin.Trees.Count}) in {sw.ElapsedMilliseconds}ms");
        WorldMap.RescanWGOsList();
    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(WorldGameObject), nameof(WorldGameObject.SmartInstantiate))]
    public static void WorldGameObject_SmartInstantiate(WorldGameObject __instance, ref WorldObjectPart prefab)
    {
        if (prefab == null) return;

        var prefabName = prefab.name;
        var instancePos = __instance.pos3;

        if (prefabName.Contains("stump"))
        {
            HandleStump(__instance, instancePos, ref prefab);
        }
        else if (IsValidTree(prefabName))
        {
            HandleTree(__instance, instancePos, ref prefab);
        }
    }

    private static void HandleStump(WorldGameObject instance, Vector3 instancePos, ref WorldObjectPart prefab)
    {
        if (Plugin.DebugEnabled) Helpers.Log($"[HandleStump] entry — instance={instance.obj_id} at {instancePos}, InstantStumpRemoval={Plugin.InstantStumpRemoval.Value}");

        // SmartInstantiate can fire repeatedly for the same stump (zone enter/exit, save reload).
        // Without this dedup check, every re-fire appended a duplicate Tree entry, which SaveTrees
        // would then strip and log — producing pages of "Saved/Removed" spam plus a full JSON write
        // to disk per re-fire. Mirrors the same check already in HandleTree.
        var alreadyTracked = Plugin.Trees.Any(tree => Vector3.Distance(tree.location, instancePos) <= Plugin.TreeSearchDistance.Value);
        if (!alreadyTracked)
        {
            var tree = new Tree(instance.obj_id, instancePos);
            Plugin.Trees.Add(tree);
            Plugin.SaveTrees();

            if (Plugin.DebugEnabled) Helpers.Log($"[HandleStump] added new tracked stump for {instance.obj_id} at {instancePos}; total tracked now {Plugin.Trees.Count}");
        }
        else
        {
            if (Plugin.DebugEnabled) Helpers.Log($"[HandleStump] {instancePos} is already in tracked list (within search distance) — skip add");
        }

        if (Plugin.InstantStumpRemoval.Value)
        {
            if (Plugin.DebugEnabled) Helpers.Log($"[HandleStump] InstantStumpRemoval=true — nulling prefab so the stump never spawns");
            prefab = null;
        }
    }

    private static bool IsValidTree(string prefabName)
    {
        return prefabName.Contains("tree") && !prefabName.Contains("bees") && !prefabName.Contains("apple");
    }

    private static void HandleTree(WorldGameObject instance, Vector3 instancePos, ref WorldObjectPart prefab)
    {
        var treeExists = Plugin.Trees.Any(tree => Vector3.Distance(tree.location, instancePos) <= Plugin.TreeSearchDistance.Value);

        if (Plugin.DebugEnabled) Helpers.Log($"[HandleTree] entry — instance={instance.obj_id} at {instancePos}, treeExists={treeExists}, game_started={MainGame.game_started}");

        if (!treeExists && MainGame.game_started)
        {
            var tree = new Tree(instance.obj_id, instancePos);
            Plugin.Trees.Add(tree);
            Plugin.SaveTrees();
            if (Plugin.DebugEnabled) Helpers.Log($"[HandleTree] new tree felled — added to tracked list (total {Plugin.Trees.Count}) and nulling prefab so the world copy doesn't respawn this load");
            prefab = null;
        }
        else
        {
            if (Plugin.DebugEnabled) Helpers.Log($"[HandleTree] no action — already tracked or game not started");
        }
    }

}
