using System.Diagnostics.CodeAnalysis;
using HarmonyLib;

namespace AnAlchemicalCollection;

[HarmonyPatch]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class FastTravelPatches
{
    private const string McHome = "MC_HOME";
    public static bool DoFastTravel { get; set; }

    [HarmonyPatch(typeof(WorldMapUI), nameof(WorldMapUI.GoToDestination))]
    [HarmonyPrefix]
    public static void WorldMapUI_GoToDestination(ref WorldMapUI __instance)
    {
        if (!DoFastTravel) return;
        
        var marker = __instance.fastTravelPinList.Find(a => a.GetMarkerID == McHome);
        __instance.currentSelectedPin.Add(marker);
        __instance.currentSelectedPin[0] = marker;
        DoFastTravel = false;
    }
}