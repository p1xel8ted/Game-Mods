using MassOfTheLamb.Core;

namespace MassOfTheLamb.Patches;

[Harmony]
[SuppressMessage("ReSharper", "SuggestBaseTypeForParameter")]
public static class MassFarmPatches
{
    private static GameManager GI => GameManager.GetInstance();

    #region Mass Water

    [HarmonyPrefix]
    [HarmonyPatch(typeof(FarmPlot), nameof(FarmPlot.OnInteract), typeof(StateMachine))]
    public static void FarmPlot_OnInteract_MassWater(ref FarmPlot __instance)
    {
        if (Plugin.MassWater.Value && __instance.StructureBrain.CanWater())
        {
            GI.StartCoroutine(WaterAllPlants());
        }
    }

    private static IEnumerator WaterAllPlants()
    {
        yield return new WaitForEndOfFrame();
        var waterablePlots = FarmPlot.FarmPlots.Where(p => p != null && p.Structure != null && p.StructureBrain != null && p.StructureBrain.CanWater()).ToList();
        if (waterablePlots.Count == 0 || !MassActionCosts.TryDeductCosts(waterablePlots.Count)) yield break;

        Plugin.WriteLog($"[MassWater] Watering {waterablePlots.Count} plots");
        foreach (var plot in waterablePlots)
        {
            plot.StructureInfo.Watered = true;
            plot.StructureInfo.WateredCount = 0;
            plot.WateringTime = 0.95f;
            plot.UpdateWatered();
            if (MonoSingleton<UIManager>.Instance)
            {
                plot.UpdateCropImage();
            }
            yield return new WaitForSeconds(0.05f);
        }
    }

    #endregion

    #region Mass Fertilize

    [HarmonyPrefix]
    [HarmonyPatch(typeof(FarmPlot), nameof(FarmPlot.AddFertiliser), typeof(InventoryItem.ITEM_TYPE))]
    public static void FarmPlot_AddFertiliser(ref FarmPlot __instance, InventoryItem.ITEM_TYPE chosenItem)
    {
        if (!Plugin.MassFertilize.Value) return;
        GI.StartCoroutine(FertilizeAllPlots(chosenItem));
    }

    private static IEnumerator FertilizeAllPlots(InventoryItem.ITEM_TYPE chosenItem)
    {
        yield return new WaitForEndOfFrame();
        var fertilizablePlots = FarmPlot.FarmPlots.Where(p => p != null && p.Structure != null && p.StructureBrain != null && p.StructureBrain.CanFertilize()).ToList();
        if (fertilizablePlots.Count == 0 || !MassActionCosts.TryDeductCosts(fertilizablePlots.Count)) yield break;
        var itemsNeeded = fertilizablePlots.Count;
        var itemsAvailable = Inventory.GetItemQuantity((int)chosenItem);

        if (itemsNeeded > itemsAvailable)
        {
            Plugin.WriteLog($"[MassFertilize] Need {itemsNeeded} fertilizer but only have {itemsAvailable}");
            fertilizablePlots = fertilizablePlots.Take(itemsAvailable).ToList();
        }

        Plugin.WriteLog($"[MassFertilize] Fertilizing {fertilizablePlots.Count} plots with {chosenItem}");
        foreach (var plot in fertilizablePlots)
        {
            plot.StructureBrain.AddFertilizer(chosenItem);
            ResourceCustomTarget.Create(plot.gameObject, PlayerFarming.Instance.transform.position, chosenItem, plot.AddFertilizer);
            Inventory.ChangeItemQuantity((int)chosenItem, -1);
            yield return new WaitForSeconds(0.05f);
        }
    }

    #endregion

    #region Mass Plant Seeds

    [HarmonyPostfix]
    [HarmonyPatch(typeof(FarmPlot), nameof(FarmPlot.OnInteract), typeof(StateMachine))]
    public static void FarmPlot_OnInteract_MassPlant(FarmPlot __instance)
    {
        if (!Plugin.MassPlantSeeds.Value) return;
        if (__instance.Structure == null || __instance.StructureBrain == null) return;
        if (!__instance.StructureBrain.CanPlantSeed()) return;
        GI.StartCoroutine(HookSeedSelection(__instance));
    }

    private static IEnumerator HookSeedSelection(FarmPlot triggered)
    {
        yield return new WaitForEndOfFrame();

        if (UIItemSelectorOverlayController.SelectorOverlays.Count == 0)
        {
            yield break;
        }

        var selector = UIItemSelectorOverlayController.SelectorOverlays[UIItemSelectorOverlayController.SelectorOverlays.Count - 1];
        if (selector == null)
        {
            yield break;
        }

        var originalCallback = selector.OnItemChosen;

        selector.OnItemChosen = chosenItem =>
        {
            originalCallback?.Invoke(chosenItem);

            var emptyPlots = FarmPlot.FarmPlots
                .Count(p => p != null && p != triggered &&
                            p.Structure != null && p.StructureBrain != null &&
                            p.StructureBrain.CanPlantSeed() &&
                            p.ToDeposit.Count == 0);
            if (emptyPlots > 0 && MassActionCosts.TryDeductCosts(emptyPlots))
            {
                GI.StartCoroutine(PlantSeedInAllEmptyPlots(triggered, chosenItem));
            }
        };
    }

    private static IEnumerator PlantSeedInAllEmptyPlots(FarmPlot triggered, InventoryItem.ITEM_TYPE seedType)
    {
        foreach (var plot in FarmPlot.FarmPlots.ToList())
        {
            if (plot == null || plot == triggered) continue;
            if (plot.Structure == null || plot.StructureBrain == null) continue;
            if (!plot.StructureBrain.CanPlantSeed()) continue;
            if (plot.ToDeposit.Count > 0) continue;
            if (Inventory.GetItemQuantity(seedType) <= 0) break;

            Inventory.ChangeItemQuantity((int)seedType, -1);
            var p = plot;
            ResourceCustomTarget.Create(p.gameObject, PlayerFarming.Instance.transform.position, seedType, delegate
            {
                p.StructureBrain.PlantSeed(seedType);
                ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.PlantCrops);
                p.UpdateCropImage();
                p.HasChanged = true;
                p.checkWaterIndicator();
            });
            yield return new WaitForSeconds(0.05f);
        }
    }

    #endregion
}
