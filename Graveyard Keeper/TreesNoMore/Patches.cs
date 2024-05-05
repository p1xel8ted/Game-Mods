using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace TreesNoMore;

[HarmonyPatch]
public static class Patches
{
    internal static void DestroyTress()
    {
        var sw = new System.Diagnostics.Stopwatch();
        sw.Start();

        // Create a new list to hold the trees that you want to destroy.
        List<WorldGameObject> treesToDestroy = new List<WorldGameObject>();

        foreach (var tree in WorldMap.objs.Where(o => o.name.Contains("tree") && !o.name.Contains("bees") && !o.name.Contains("apple")))
        {
            var treeExists = Plugin.Trees.Any(x => Vector3.Distance(x.location, tree.pos3) <= Plugin.TreeSearchDistance.Value);
            if (treeExists)
            {
                Plugin.Log.LogWarning($"Found existing tree at {tree.pos3} that should be removed.");
                // Add the tree to the treesToDestroy list instead of destroying it immediately.
                treesToDestroy.Add(tree);
            }
        }

        // Now you can destroy the trees without modifying the collection you're iterating over.
        foreach (var tree in treesToDestroy)
        {
            WorldMap.objs.Remove(tree); // removing the reference from WorldMap.objs
            UnityEngine.Object.DestroyImmediate(tree);
        }

        sw.Stop();
        Plugin.Log.LogWarning($"Search N Destroyed {Plugin.Trees.Count} trees in {sw.ElapsedMilliseconds}ms");
        WorldMap.RescanWGOsList();
    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(WorldGameObject), nameof(WorldGameObject.SmartInstantiate))]
    public static void WorldGameObject_SmartInstantiate(ref WorldGameObject __instance, ref WorldObjectPart prefab)
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
        Plugin.Log.LogWarning($"Stump spawn at {instancePos}");

        var tree = new Tree(instance.obj_id, instancePos);
        Plugin.Trees.Add(tree);
        Plugin.SaveTrees();

        if (Plugin.InstantStumpRemoval.Value)
        {
            prefab = null;
        }

        Plugin.Log.LogWarning($"Tree at {instancePos} added to list");
    }

    private static bool IsValidTree(string prefabName)
    {
        return prefabName.Contains("tree") && !prefabName.Contains("bees") && !prefabName.Contains("apple");
    }

    private static void HandleTree(WorldGameObject instance, Vector3 instancePos, ref WorldObjectPart prefab)
    {
        var treeExists = Plugin.Trees.Any(tree => Vector3.Distance(tree.location, instancePos) <= Plugin.TreeSearchDistance.Value);

        if (!treeExists && MainGame.game_started)
        {
            Plugin.Log.LogWarning($"Tree at {instancePos} added to list");
            var tree = new Tree(instance.obj_id, instancePos);
            Plugin.Trees.Add(tree);
            Plugin.SaveTrees();
            prefab = null;
        }
    }


    [HarmonyFinalizer]
    [HarmonyPatch(typeof(WorldGameObject), nameof(WorldGameObject.SmartInstantiate))]
    public static Exception WorldGameObject_SmartInstantiate_Finalizer()
    {
        return null;
    }
}