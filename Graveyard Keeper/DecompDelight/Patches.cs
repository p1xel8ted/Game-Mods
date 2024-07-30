namespace DecompDelight;

[Harmony]
internal static class Patches
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(ItemDefinition), nameof(ItemDefinition.GetTooltipData))]
    private static void ItemDefinition_GetTooltipData(ref ItemDefinition __instance, ref Item item, ref bool full_detail, ref List<BubbleWidgetData> __result)
    {
        if (!Utils.SurveyCompleted(__instance, full_detail)) return;

        var itemId = __instance.GetNameWithoutQualitySuffix();

        var decomposeOutput = Utils.GetDecomposeOutput(itemId);


        if (decomposeOutput.IsNullOrWhiteSpace()) return;

        if (ElementMaps.ItemElementCache.TryGetValue(itemId, out var element))
        {
            Utils.AddToTooltip(__result, element);
            return;
        }

        element = Utils.GetElementForItem(decomposeOutput);

        ElementMaps.ItemElementCache[itemId] = element;

        if (element is ElementMaps.Element.None)
        {
            Plugin.LOG.LogError($"Decompose output for '{itemId}' is '{decomposeOutput}' -> Element: {element}");
        }

        Utils.AddToTooltip(__result, element);
    }
}