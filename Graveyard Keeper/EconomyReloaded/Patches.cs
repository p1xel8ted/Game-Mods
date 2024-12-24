namespace EconomyReloaded;

[Harmony]
public static class Patches
{
    private static bool GameBalanceAlreadyRun { get; set; }
    private static readonly Dictionary<string, bool> BackedUpIsStaticCost = new();
    private static readonly HashSet<string> StaticCostItemIds = [];

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Trading), nameof(Trading.GetSingleItemCostInTraderInventory), typeof(Item), typeof(int))]
    public static void Trading_GetSingleItemCostInTraderInventory(ref float __result, Item item)
    {
        if (!Plugin.Inflation.Value)
        {
            if (__result != 0.0)
            {
                __result = item.definition.base_price;
            }

            // Plugin.Log.LogWarning(
            //     $"GetSingleItemCostInTraderInventory: Inflation is off, returning base price: {item.definition.base_price}");
        }

        // Plugin.Log.LogWarning(
        //     $"GetSingleItemCostInTraderInventory: Inflation is on, returning normal price: {__result}");
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Trading), nameof(Trading.GetSingleItemCostInPlayerInventory), typeof(Item), typeof(int))]
    public static void Trading_GetSingleItemCostInPlayerInventory(ref float __result, Item item)
    {
        if (!Plugin.Deflation.Value)
        {
            if (__result != 0.0)
            {
                __result = item.definition.base_price;
            }

            // Plugin.Log.LogWarning(
            //     $"GetSingleItemCostInPlayerInventory: Deflation is off, returning base price: {item.definition.base_price}");
        }

        // Plugin.Log.LogWarning(
        //     $"GetSingleItemCostInPlayerInventory: Deflation is on, returning normal price: {__result}");
    }

    public static void RestoreIsStaticCost()
    {
        if (GameBalance.me == null) return;
        foreach (var itemDef in GameBalance.me.items_data.Where(itemDef => StaticCostItemIds.Contains(itemDef.id)))
        {
            itemDef.is_static_cost = BackedUpIsStaticCost[itemDef.id];
        }

        Plugin.Log.LogWarning($"Restored {BackedUpIsStaticCost.Count} items to their original is_static_cost value.");
    }

    private static void MakeIsStaticCost()
    {
        if (GameBalance.me == null) return;
        foreach (var itemDef in GameBalance.me.items_data.Where(itemDef => StaticCostItemIds.Contains(itemDef.id)))
        {
            itemDef.is_static_cost = true;
        }

        Plugin.Log.LogWarning($"Set {BackedUpIsStaticCost.Count} items to is_static_cost = true.");
    }

    public static void GameBalance_LoadGameBalance()
    {
        if (GameBalanceAlreadyRun) return;
        GameBalanceAlreadyRun = true;

        var itemsWithBasePrice = GameBalance.me.items_data.Where(itemDef => itemDef.base_price > 0).ToList();
        BackedUpIsStaticCost.Clear();
        StaticCostItemIds.Clear();

        foreach (var itemDef in itemsWithBasePrice)
        {
            BackedUpIsStaticCost[itemDef.id] = itemDef.is_static_cost;
            StaticCostItemIds.Add(itemDef.id);
            itemDef.is_static_cost = true;
        }
        
        MakeIsStaticCost();
    }
}