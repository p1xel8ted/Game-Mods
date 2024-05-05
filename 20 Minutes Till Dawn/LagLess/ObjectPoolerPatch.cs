using System.Collections.Generic;
using flanne;
using HarmonyLib;
using UnityEngine;

namespace LagLess;

[Harmony]
public static class ObjectPoolerPatch
{
    private static LLObjectPooler objectPoolReplacement;
    
    [HarmonyPrefix]
    [HarmonyPatch(typeof(ObjectPooler), nameof(ObjectPooler.Awake))]
    private static bool Awake(ref ObjectPooler __instance)
    {
        LLConstants.Logger.LogDebug("Pooler Awake");
        objectPoolReplacement = new LLObjectPooler(__instance.transform, __instance.itemsToPool);
        ObjectPooler.SharedInstance = __instance;
        return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(ObjectPooler), nameof(ObjectPooler.GetPooledObject))]
    private static bool GetPooledObject(ref string tag, ref GameObject __result)
    {
        __result = objectPoolReplacement.GetPooledObject(tag);
        return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(ObjectPooler), nameof(ObjectPooler.GetAllPooledObjects))]
    private static bool GetAllPooledObjects(ref string tag, ref List<GameObject> __result)
    {
        __result = objectPoolReplacement.GetAllPooledObjects(tag);
        return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(ObjectPooler), nameof(ObjectPooler.AddObject))]
    private static bool AddObject(ref string tag, ref GameObject GO, ref int amt, ref bool exp)
    {
        objectPoolReplacement.AddObject(tag, GO, amt, exp);
        return false;
    }
}