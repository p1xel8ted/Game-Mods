using DG.Tweening.Core;
using HarmonyLib;
using UnityEngine;

namespace QoL.Patches;

[Harmony]
public static class DebugPatches
{
    //patches to kill log spam
    
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Debug), nameof(Debug.LogWarning), typeof(Object))]
    public static bool Debug_LogWarning(ref Object message)
    {
        return !message.ToString().Contains("There was no season prefab for object");
    }
    
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Debugger), nameof(Debugger.LogWarning), typeof(Object))]
    public static bool Debugger_LogWarning(ref Object message)
    {
        return !message.ToString().Contains("DOTWEEN");
    }
}