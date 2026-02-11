using MassOfTheLamb.Core;

namespace MassOfTheLamb.Patches;

[Harmony]
public static class MassFillPatches
{
    private static GameManager GI => GameManager.GetInstance();

    // Flags to prevent infinite recursion when mass filling
    private static bool _refineryMassFillInProgress;
    private static bool _cookingFireMassFillInProgress;
    private static bool _kitchenMassFillInProgress;
    private static bool _pubMassFillInProgress;

    #region Mass Fill Troughs

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Interaction_RanchTrough), nameof(Interaction_RanchTrough.OnInteract), typeof(StateMachine))]
    public static void Interaction_RanchTrough_OnInteract_Postfix(Interaction_RanchTrough __instance)
    {
        if (!Plugin.FillTroughToCapacity.Value && !Plugin.MassFillTroughs.Value) return;
        GI.StartCoroutine(HookTroughFoodSelection(__instance));
    }

    private static IEnumerator HookTroughFoodSelection(Interaction_RanchTrough triggered)
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
            FillAllTroughs(triggered, chosenItem);

            if (triggered.GetCompostAndAirCount() >= triggered.StructureBrain.Capacity)
            {
                selector.Hide();
            }
        };
    }

    private static void FillAllTroughs(Interaction_RanchTrough triggered, InventoryItem.ITEM_TYPE foodType)
    {
        // Fill triggered trough to capacity (single-structure QoL, always free)
        while (triggered.GetCompostAndAirCount() < triggered.StructureBrain.Capacity && Inventory.GetItemQuantity(foodType) > 0)
        {
            Inventory.ChangeItemQuantity((int)foodType, -1);
            triggered.Structure.DepositInventory(new InventoryItem(foodType, 1));
        }

        // Fill all other non-full troughs (mass action, costs apply)
        if (!Plugin.MassFillTroughs.Value) return;

        var fillable = Interaction_RanchTrough.Troughs
            .Where(t => t != null && t != triggered &&
                        !t.StructureBrain.ReservedByPlayer &&
                        t.GetCompostCount() < t.StructureBrain.Capacity)
            .ToList();
        if (fillable.Count == 0 || !MassActionCosts.TryDeductCosts(fillable.Count)) return;

        foreach (var trough in fillable)
        {
            while (trough.GetCompostCount() < trough.StructureBrain.Capacity && Inventory.GetItemQuantity(foodType) > 0)
            {
                Inventory.ChangeItemQuantity((int)foodType, -1);
                trough.Structure.DepositInventory(new InventoryItem(foodType, 1));
            }

            trough.UpdateCapacityIndicators();
        }
    }

    #endregion

    #region Mass Fill Toolsheds (Carpentry Stations)

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Interaction_Toolshed), nameof(Interaction_Toolshed.OnInteract), typeof(StateMachine))]
    public static void Interaction_Toolshed_OnInteract_Postfix(Interaction_Toolshed __instance)
    {
        if (!Plugin.FillToolshedToCapacity.Value && !Plugin.MassFillToolsheds.Value) return;
        GI.StartCoroutine(HookToolshedSelection(__instance));
    }

    private static IEnumerator HookToolshedSelection(Interaction_Toolshed triggered)
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
            FillAllToolsheds(triggered, chosenItem);

            if (triggered.GetCompostAndAirCount() >= triggered.StructureBrain.Capacity)
            {
                selector.Hide();
            }
        };
    }

    private static void FillAllToolsheds(Interaction_Toolshed triggered, InventoryItem.ITEM_TYPE itemType)
    {
        // Fill triggered toolshed to capacity (single-structure QoL, always free)
        while (triggered.GetCompostAndAirCount() < triggered.StructureBrain.Capacity && Inventory.GetItemQuantity(itemType) > 0)
        {
            Inventory.ChangeItemQuantity((int)itemType, -1);
            triggered.Structure.DepositInventory(new InventoryItem(itemType, 1));
        }

        // Fill all other non-full toolsheds (mass action, costs apply)
        if (!Plugin.MassFillToolsheds.Value) return;

        var fillable = Interaction_Toolshed.Toolsheds
            .Where(t => t != null && t != triggered &&
                        !t.StructureBrain.ReservedByPlayer &&
                        t.GetCompostCount() < t.StructureBrain.Capacity)
            .ToList();
        if (fillable.Count == 0 || !MassActionCosts.TryDeductCosts(fillable.Count)) return;

        foreach (var toolshed in fillable)
        {
            while (toolshed.GetCompostCount() < toolshed.StructureBrain.Capacity && Inventory.GetItemQuantity(itemType) > 0)
            {
                Inventory.ChangeItemQuantity((int)itemType, -1);
                toolshed.Structure.DepositInventory(new InventoryItem(itemType, 1));
            }

            toolshed.UpdateCapacityIndicators();
        }
    }

    #endregion

    #region Mass Fill Medic Stations

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Interaction_Medic), nameof(Interaction_Medic.OnInteract), typeof(StateMachine))]
    public static void Interaction_Medic_OnInteract_Postfix(Interaction_Medic __instance)
    {
        if (!Plugin.FillMedicToCapacity.Value && !Plugin.MassFillMedicStations.Value) return;
        GI.StartCoroutine(HookMedicSelection(__instance));
    }

    private static IEnumerator HookMedicSelection(Interaction_Medic triggered)
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
            FillAllMedicStations(triggered, chosenItem);

            if (triggered.GetCompostAndAirCount() >= triggered.StructureBrain.Capacity)
            {
                selector.Hide();
            }
        };
    }

    private static void FillAllMedicStations(Interaction_Medic triggered, InventoryItem.ITEM_TYPE itemType)
    {
        // Fill triggered medic station to capacity (single-structure QoL, always free)
        while (triggered.GetCompostAndAirCount() < triggered.StructureBrain.Capacity && Inventory.GetItemQuantity(itemType) > 0)
        {
            Inventory.ChangeItemQuantity((int)itemType, -1);
            triggered.Structure.DepositInventory(new InventoryItem(itemType, 1));
        }

        // Fill all other non-full medic stations (mass action, costs apply)
        if (!Plugin.MassFillMedicStations.Value) return;

        var fillable = Interaction_Medic.Medics
            .Where(m => m != null && m != triggered &&
                        !m.StructureBrain.ReservedByPlayer &&
                        m.GetCompostCount() < m.StructureBrain.Capacity)
            .ToList();
        if (fillable.Count == 0 || !MassActionCosts.TryDeductCosts(fillable.Count)) return;

        foreach (var medic in fillable)
        {
            while (medic.GetCompostCount() < medic.StructureBrain.Capacity && Inventory.GetItemQuantity(itemType) > 0)
            {
                Inventory.ChangeItemQuantity((int)itemType, -1);
                medic.Structure.DepositInventory(new InventoryItem(itemType, 1));
            }

            medic.UpdateCapacityIndicators();
        }
    }

    #endregion

    #region Mass Fill Seed Silos

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Interaction_SiloSeeder), nameof(Interaction_SiloSeeder.OnInteract), typeof(StateMachine))]
    public static void Interaction_SiloSeeder_OnInteract_Postfix(Interaction_SiloSeeder __instance)
    {
        if (!Plugin.FillSeedSiloToCapacity.Value && !Plugin.MassFillSeedSilos.Value) return;
        GI.StartCoroutine(HookSeedSiloSelection(__instance));
    }

    private static IEnumerator HookSeedSiloSelection(Interaction_SiloSeeder triggered)
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
            FillAllSeedSilos(triggered, chosenItem, selector);
        };
    }

    private static void FillAllSeedSilos(Interaction_SiloSeeder triggered, InventoryItem.ITEM_TYPE seedType, UIItemSelectorOverlayController selector)
    {
        // Fill triggered seed silo to capacity (single-structure QoL, always free)
        while (triggered.GetCompostAndAirCount() < triggered.StructureBrain.Capacity && Inventory.GetItemQuantity(seedType) > 0)
        {
            Inventory.ChangeItemQuantity((int)seedType, -1);
            triggered.Structure.DepositInventory(new InventoryItem(seedType, 1));
        }

        triggered.UpdateCapacityIndicators();

        if (triggered.GetCompostAndAirCount() >= triggered.StructureBrain.Capacity)
        {
            selector.Hide();
        }

        // Fill all other non-full seed silos (mass action, costs apply)
        if (!Plugin.MassFillSeedSilos.Value) return;

        var fillable = Interaction_SiloSeeder.SiloSeeders
            .Where(s => s != null && s != triggered &&
                        !s.StructureBrain.ReservedByPlayer &&
                        s.GetCompostCount() < s.StructureBrain.Capacity)
            .ToList();
        if (fillable.Count == 0 || !MassActionCosts.TryDeductCosts(fillable.Count)) return;

        foreach (var silo in fillable)
        {
            while (silo.GetCompostCount() < silo.StructureBrain.Capacity && Inventory.GetItemQuantity(seedType) > 0)
            {
                Inventory.ChangeItemQuantity((int)seedType, -1);
                silo.Structure.DepositInventory(new InventoryItem(seedType, 1));
            }

            silo.UpdateCapacityIndicators();
        }
    }

    #endregion

    #region Mass Fill Fertilizer Silos

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Interaction_SiloFertilizer), nameof(Interaction_SiloFertilizer.OnInteract), typeof(StateMachine))]
    public static void Interaction_SiloFertilizer_OnInteract_Postfix(Interaction_SiloFertilizer __instance)
    {
        if (!Plugin.FillFertilizerSiloToCapacity.Value && !Plugin.MassFillFertilizerSilos.Value) return;
        GI.StartCoroutine(HookFertilizerSiloSelection(__instance));
    }

    private static IEnumerator HookFertilizerSiloSelection(Interaction_SiloFertilizer triggered)
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
            FillAllFertilizerSilos(triggered, chosenItem, selector);
        };
    }

    private static void FillAllFertilizerSilos(Interaction_SiloFertilizer triggered, InventoryItem.ITEM_TYPE fertType, UIItemSelectorOverlayController selector)
    {
        // Fill triggered fertilizer silo to capacity (single-structure QoL, always free)
        while (triggered.GetCompostCount() < triggered.StructureBrain.Capacity && Inventory.GetItemQuantity(fertType) > 0)
        {
            Inventory.ChangeItemQuantity((int)fertType, -1);
            triggered.Structure.DepositInventory(new InventoryItem(fertType, 1));
        }

        triggered.UpdateCapacityIndicators();

        if (triggered.GetCompostCount() >= triggered.StructureBrain.Capacity)
        {
            selector.Hide();
        }

        // Fill all other non-full fertilizer silos (mass action, costs apply)
        if (!Plugin.MassFillFertilizerSilos.Value) return;

        var fillable = Interaction_SiloFertilizer.SiloFertilizers
            .Where(s => s != null && s != triggered &&
                        !s.StructureBrain.ReservedByPlayer &&
                        s.GetCompostCount() < s.StructureBrain.Capacity)
            .ToList();
        if (fillable.Count == 0 || !MassActionCosts.TryDeductCosts(fillable.Count)) return;

        foreach (var silo in fillable)
        {
            while (silo.GetCompostCount() < silo.StructureBrain.Capacity && Inventory.GetItemQuantity(fertType) > 0)
            {
                Inventory.ChangeItemQuantity((int)fertType, -1);
                silo.Structure.DepositInventory(new InventoryItem(fertType, 1));
            }

            silo.UpdateCapacityIndicators();
        }
    }

    #endregion

    #region Mass Fill Refinery

    [HarmonyPostfix]
    [HarmonyPatch(typeof(UIRefineryMenuController), nameof(UIRefineryMenuController.AddToQueue))]
    public static void UIRefineryMenuController_AddToQueue(UIRefineryMenuController __instance, RefineryItem item)
    {
        if (!Plugin.RefineryMassFill.Value) return;
        if (_refineryMassFillInProgress) return;

        _refineryMassFillInProgress = true;
        try
        {
            var maxItems = __instance.kMaxItems;
            var currentCount = __instance._structureInfo.QueuedResources.Count;

            // Keep adding same item until queue is full or can't afford
            while (currentCount < maxItems && item.CanAfford)
            {
                __instance.AddToQueue(item);
                currentCount = __instance._structureInfo.QueuedResources.Count;
            }

            Plugin.WriteLog($"[RefineryMassFill] Filled queue with {item.Type} - {currentCount}/{maxItems} slots used");
        }
        finally
        {
            _refineryMassFillInProgress = false;
        }
    }

    #endregion

    #region Mass Fill Kitchen

    [HarmonyPostfix]
    [HarmonyPatch(typeof(UIFollowerKitchenMenuController), nameof(UIFollowerKitchenMenuController.AddToQueue))]
    public static void UIFollowerKitchenMenuController_AddToQueue(UIFollowerKitchenMenuController __instance, InventoryItem.ITEM_TYPE meal)
    {
        if (!Plugin.KitchenMassFill.Value) return;
        if (_kitchenMassFillInProgress) return;

        _kitchenMassFillInProgress = true;
        try
        {
            var maxItems = UIFollowerKitchenMenuController.kMaxItems;
            var currentCount = __instance._structureInfo.QueuedMeals.Count;

            while (currentCount < maxItems && CookingData.CanMakeMeal(meal))
            {
                __instance.AddToQueue(meal);
                currentCount = __instance._structureInfo.QueuedMeals.Count;
            }

            Plugin.WriteLog($"[KitchenMassFill] Filled queue with {meal} - {currentCount}/{maxItems} slots used");
        }
        finally
        {
            _kitchenMassFillInProgress = false;
        }
    }

    #endregion

    #region Mass Fill Pub

    [HarmonyPostfix]
    [HarmonyPatch(typeof(UIPubMenuController), nameof(UIPubMenuController.AddToQueue))]
    public static void UIPubMenuController_AddToQueue(UIPubMenuController __instance, InventoryItem.ITEM_TYPE meal)
    {
        if (!Plugin.PubMassFill.Value) return;
        if (_pubMassFillInProgress) return;

        _pubMassFillInProgress = true;
        try
        {
            var maxItems = __instance._pub.Brain.MaxQueue;
            var currentCount = __instance._structureInfo.QueuedMeals.Count;

            while (currentCount < maxItems && CookingData.CanMakeMeal(meal))
            {
                __instance.AddToQueue(meal);
                currentCount = __instance._structureInfo.QueuedMeals.Count;
            }

            Plugin.WriteLog($"[PubMassFill] Filled queue with {meal} - {currentCount}/{maxItems} slots used");
        }
        finally
        {
            _pubMassFillInProgress = false;
        }
    }

    #endregion

    #region Mass Fill Cooking Fire

    [HarmonyPostfix]
    [HarmonyPatch(typeof(UICookingFireMenuController), nameof(UICookingFireMenuController.OnRecipeChosen))]
    public static void UICookingFireMenuController_OnRecipeChosen(UICookingFireMenuController __instance, InventoryItem.ITEM_TYPE recipe)
    {
        if (!Plugin.CookingFireMassFill.Value) return;
        if (_cookingFireMassFillInProgress) return;

        _cookingFireMassFillInProgress = true;
        try
        {
            var maxItems = __instance.RecipeLimit();
            var currentCount = __instance._kitchenData.QueuedMeals.Count;

            while (currentCount < maxItems && CookingData.CanMakeMeal(recipe))
            {
                __instance.OnRecipeChosen(recipe);
                currentCount = __instance._kitchenData.QueuedMeals.Count;
            }

            Plugin.WriteLog($"[CookingFireMassFill] Filled queue with {recipe} - {currentCount}/{maxItems} slots used");
        }
        finally
        {
            _cookingFireMassFillInProgress = false;
        }
    }

    #endregion
}
