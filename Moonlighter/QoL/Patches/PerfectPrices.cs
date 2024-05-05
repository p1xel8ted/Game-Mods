using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace QoL.Patches;

[HarmonyPatch(typeof(NotebookPanel))]
public static class PerfectPrices
{
    // Default colors for price indications stored statically to avoid repeated lookups
    private static Color defaultTooCheapColor = Color.clear;
    private static Color defaultCheapColor = Color.clear;
    private static Color defaultExpensiveColor = Color.clear;
    private static Color defaultTooExpensiveColor = Color.clear;

    [HarmonyPostfix]
    [HarmonyPatch(nameof(NotebookPanel.DetailItem))]
    private static void NotebookPanel_DetailItem(ref NotebookPanel __instance, ref ItemMaster item, ref bool unlocked)
    {
        // Early exit if item is null
        if (item is null) return;

        // Initialize default colors once
        InitializeDefaultColorsIfNeeded(__instance);

        // Retrieve and temporarily set item popularity to Neutral
        var originalPopularity = ItemPriceManager.Instance.GetPopularity(item);
        ItemPriceManager.Instance.SetPopularity(item, ItemPriceInfo.Popularity.Neutral);

        // Update text colors based on price valuations
        UpdateTextColorBasedOnPrice(__instance.textLastTooCheap, item, ItemPriceValoration.TooCheap, defaultTooCheapColor);
        UpdateTextColorBasedOnPrice(__instance.textLastCheap, item, ItemPriceValoration.Cheap, defaultCheapColor);
        UpdateTextColorBasedOnPrice(__instance.textLastExpensive, item, ItemPriceValoration.Expensive, defaultExpensiveColor);
        UpdateTextColorBasedOnPrice(__instance.textLastTooExpensive, item, ItemPriceValoration.TooExpensive, defaultTooExpensiveColor, incrementThreshold: true);

        // Restore original item popularity
        ItemPriceManager.Instance.SetPopularity(item, originalPopularity);
    }

    private static void InitializeDefaultColorsIfNeeded(NotebookPanel notebookPanel)
    {
        if (defaultTooCheapColor.a == 0f) defaultTooCheapColor = notebookPanel.textLastTooCheap.color;
        if (defaultCheapColor.a == 0f) defaultCheapColor = notebookPanel.textLastCheap.color;
        if (defaultExpensiveColor.a == 0f) defaultExpensiveColor = notebookPanel.textLastExpensive.color;
        if (defaultTooExpensiveColor.a == 0f) defaultTooExpensiveColor = notebookPanel.textLastTooExpensive.color;
    }

    private static void UpdateTextColorBasedOnPrice(Graphic textElement, ItemMaster item, ItemPriceValoration valuation, Color defaultColor, bool incrementThreshold = false)
    {
        var lastPrice = ItemPriceManager.Instance.GetLastPrice(item, valuation);
        var priceThreshold = valuation is ItemPriceValoration.TooCheap or ItemPriceValoration.Cheap ? 
            ItemPriceManager.Instance.GetMinCorrectPrice(item) : 
            ItemPriceManager.Instance.GetMaxCorrectPrice(item);

        // Optionally increment threshold for TooExpensive valuation
        if (incrementThreshold) priceThreshold++;

        textElement.color = lastPrice == priceThreshold ? new Color(0f, 0.8f, 0.2f) : defaultColor;
    }
}