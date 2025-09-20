﻿namespace CultOfQoL.Patches.Gameplay;

[HarmonyPatch]
[SuppressMessage("ReSharper", "SuggestBaseTypeForParameter")]
public static class InteractionPatches
{
    private static GameManager GI => GameManager.GetInstance();

    // Prevents follower interaction when menus are open
    [HarmonyPrefix]
    [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.OnInteract), typeof(StateMachine))]
    public static bool Interaction_Follower_OnInteract(ref interaction_FollowerInteraction __instance)
    {
        if (UIMenuBase.ActiveMenus.Count == 0 && !GameManager.InMenu) return true;
        
        Plugin.L($"Blocking follower interaction - {UIMenuBase.ActiveMenus.Count} menu(s) open or InMenu={GameManager.InMenu}");
        foreach (var menu in UIMenuBase.ActiveMenus)
        {
            Plugin.L($"  Active Menu: {menu}");
        }
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
        
        Plugin.L($"Watering {waterablePlots.Count} plots");
        foreach (var plot in waterablePlots)
        {
            plot.StructureInfo.Watered = true;
            plot.StructureInfo.WateredCount = 0;
            plot.WateringTime = 0.95f;
            plot.UpdateWatered();
            plot.UpdateCropImage();
            yield return new WaitForSeconds(0.10f);
        }
    }

    private static IEnumerator FertilizeAllPlots(InventoryItem.ITEM_TYPE chosenItem)
    {
        yield return new WaitForEndOfFrame();
        var fertilizablePlots = FarmPlot.FarmPlots.Where(p => p.StructureBrain?.CanFertilize() == true).ToList();
        var itemsNeeded = fertilizablePlots.Count;
        var itemsAvailable = Inventory.GetItemQuantity((int)chosenItem);
        
        if (itemsNeeded > itemsAvailable)
        {
            Plugin.L($"Warning: Need {itemsNeeded} fertilizer but only have {itemsAvailable}");
            fertilizablePlots = fertilizablePlots.Take(itemsAvailable).ToList();
        }
        
        Plugin.L($"Fertilizing {fertilizablePlots.Count} plots with {chosenItem}");
        foreach (var plot in fertilizablePlots)
        {
            plot.StructureBrain.AddFertilizer(chosenItem);
            ResourceCustomTarget.Create(plot.gameObject, PlayerFarming.Instance.transform.position, chosenItem, plot.AddFertilizer);
            Inventory.ChangeItemQuantity((int)chosenItem, -1);
            yield return new WaitForSeconds(0.10f);
        }
    }
}