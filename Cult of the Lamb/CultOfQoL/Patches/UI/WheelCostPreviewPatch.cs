using CultOfQoL.Core;

namespace CultOfQoL.Patches.UI;

[Harmony]
public static class WheelCostPreviewPatch
{
    /// <summary>
    /// Appends mass action cost preview text to the command wheel description.
    /// <see cref="UIRadialMenuBase.DoWheelLoop"/> calls <see cref="UIFollowerWheelInteractionItem.GetDescription"/>
    /// every frame for the highlighted item, so the cost text updates naturally as the player hovers.
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(UIFollowerWheelInteractionItem), nameof(UIFollowerWheelInteractionItem.GetDescription))]
    public static void GetDescription_CostPreview(UIFollowerWheelInteractionItem __instance, ref string __result)
    {
        var costText = MassActionCosts.GetCostPreviewText(__instance._command);
        if (costText != null)
        {
            __result += "\n" + costText;
        }
    }
}
