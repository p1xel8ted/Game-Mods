using System.Collections.Generic;
using HarmonyLib;

namespace QoL.Patches;

[Harmony]
public static class ProjectileFix
{

    private readonly static List<AreaEffect> Landed = [];
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(AreaEffect), nameof(AreaEffect.Spawn))]
    private static void Postfix(AreaEffect __instance)
    {
        Landed.RemoveAll(x => !x);
        Landed.Add(__instance);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(AreaEffect), nameof(AreaEffect.ApplyDamageToPlayer))]
    private static bool Prefix(AreaEffect __instance)
    {
        return Landed.Contains(__instance);
    }

}