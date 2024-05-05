using flanne;
using HarmonyLib;

namespace LagLess;

[Harmony]
public class ModPowerUpFixPatch
{
   
    [HarmonyPostfix]
    [HarmonyPriority(0)]
    [HarmonyPatch(typeof(Powerup), nameof(Powerup.Apply))]
    private static void ApplyAndNotify(Powerup __instance)
    {
        LLLayers.SetAllPickerUppersLayers();
    }

}