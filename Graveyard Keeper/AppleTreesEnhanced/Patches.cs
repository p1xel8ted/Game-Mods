namespace AppleTreesEnhanced;

[Harmony]
public static class Patches
{
    private static readonly HashSet<string> SellableItemIds = [..Helpers.SellThesePlease];

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Vendor), nameof(Vendor.CanBuyItem), typeof(ItemDefinition), typeof(bool))]
    [HarmonyPatch(typeof(Vendor), nameof(Vendor.CanTradeItem), typeof(ItemDefinition))]
    public static void Vendor_TradeBeeItem(ref Vendor __instance, ref ItemDefinition item_def, ref bool __result)
    {
        if(__instance == null || item_def == null) return;
        if (!Plugin.BeeKeeperBuyback.Value || !__instance.id.Contains(Helpers.Constants.OutputItems.Bee)) return;
        if (SellableItemIds.Contains(item_def.id))
        {
            __result = true;
        }
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(WorldGameObject), nameof(WorldGameObject.ReplaceWithObject))]
    public static void WorldGameObject_ReplaceWithObject(ref WorldGameObject __instance, ref string new_obj_id)
    {
        if (string.Equals(new_obj_id, Helpers.Constants.HarvestReady.BeeHouse) && Helpers.IsPlayerBeeHive(__instance))
        {
            Helpers.ProcessGardenBeeHive(__instance);
        }
        else if (string.Equals(new_obj_id, Helpers.Constants.HarvestReady.GardenAppleTree))
        {
            Helpers.ProcessGardenAppleTree(__instance);
        }
        else if (string.Equals(new_obj_id, Helpers.Constants.HarvestReady.GardenBerryBush))
        {
            Helpers.ProcessGardenBerryBush(__instance);
        }
        else if (string.Equals(new_obj_id, Helpers.Constants.HarvestReady.WorldBerryBush1))
        {
            Helpers.ProcessBerryBush1(__instance);
        }
        else if (string.Equals(new_obj_id, Helpers.Constants.HarvestReady.WorldBerryBush2))
        {
            Helpers.ProcessBerryBush2(__instance);
        }
        else if (string.Equals(new_obj_id, Helpers.Constants.HarvestReady.WorldBerryBush3))
        {
            Helpers.ProcessBerryBush3(__instance);
        }
    }
}