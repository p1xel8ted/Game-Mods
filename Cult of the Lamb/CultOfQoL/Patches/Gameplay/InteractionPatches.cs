using CultOfQoL.Core;

namespace CultOfQoL.Patches.Gameplay;

[Harmony]
[SuppressMessage("ReSharper", "SuggestBaseTypeForParameter")]
public static class InteractionPatches
{
    private static GameManager GI => GameManager.GetInstance();

    /// <summary>
    /// Interactions that game code triggers programmatically during menus/rewards.
    /// These call OnInteract directly while menus are open (e.g., job rewards, rituals, doctrines).
    /// </summary>
    private static readonly HashSet<Type> ProgrammaticInteractionTypes =
    [
        typeof(LoreStone),
        typeof(Interaction_TempleAltar),
        typeof(Interaction_DoctrineStone),
        typeof(Interaction_SimpleConversation)
    ];

    /// <summary>
    /// Prevents ALL interactions when menus are open.
    /// Fixes bug where pressing A on a menu also triggers world interactions with nearby objects.
    /// </summary>
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Interaction), nameof(Interaction.OnInteract), typeof(StateMachine))]
    public static bool Interaction_OnInteract(ref Interaction __instance)
    {
        if (ProgrammaticInteractionTypes.Contains(__instance.GetType()))
        {
            return true;
        }

        if (UIMenuBase.ActiveMenus.Count == 0 && !GameManager.InMenu)
        {
            return true;
        }

        Plugin.WriteLog($"[Interaction] Blocking interaction with {__instance.GetType().Name} - {UIMenuBase.ActiveMenus.Count} menu(s) open or InMenu={GameManager.InMenu}");
        return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(FarmPlot), nameof(FarmPlot.OnInteract), typeof(StateMachine))]
    public static void FarmPlot_OnInteract(ref FarmPlot __instance)
    {
        if (Plugin.MassWater.Value && __instance.StructureBrain.CanWater())
        {
            GI.StartCoroutine(WaterAllPlants());
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(FarmPlot), nameof(FarmPlot.AddFertiliser), typeof(InventoryItem.ITEM_TYPE))]
    public static void FarmPlot_AddFertiliser(ref FarmPlot __instance, InventoryItem.ITEM_TYPE chosenItem)
    {
        if (!Plugin.MassFertilize.Value) return;
        GI.StartCoroutine(FertilizeAllPlots(chosenItem));
    }

    private static IEnumerator WaterAllPlants()
    {
        yield return new WaitForEndOfFrame();
        var waterablePlots = FarmPlot.FarmPlots.Where(p => p.StructureBrain?.CanWater() == true).ToList();
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

    private static IEnumerator FertilizeAllPlots(InventoryItem.ITEM_TYPE chosenItem)
    {
        yield return new WaitForEndOfFrame();
        var fertilizablePlots = FarmPlot.FarmPlots.Where(p => p.StructureBrain?.CanFertilize() == true).ToList();
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
}