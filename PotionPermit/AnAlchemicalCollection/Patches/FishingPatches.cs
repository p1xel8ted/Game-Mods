using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace AnAlchemicalCollection;

[Harmony]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class FishingPatches
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(FishingController), nameof(FishingController.DoStart))]
    public static void FishingController_DoStart()
    {
        if (!Plugin.ModifyCamera.Value) return;
        Plugin.L("Player is fishing, restoring zoom to default",true);
        Plugin.UpdateCameraZoom(true);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(FishingController), nameof(FishingController.QuitFishing))]
    public static void FishingController_QuitFishing(ref FishingController __instance)
    {
        if (!Plugin.ModifyCamera.Value) return;
        __instance.StartCoroutine(ResetCameraZoom());
    }

    private static IEnumerator ResetCameraZoom()
    {
        yield return new WaitForSeconds(1.0f);
        Plugin.UpdateCameraZoom();
        Plugin.L("Player is done fishing, restoring zoom to modified value",true);
    }
}