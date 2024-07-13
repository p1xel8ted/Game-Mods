namespace AutoLootHeavies;

[HarmonyPatch]
[HarmonyPriority(1)]
public partial class Plugin
{

    [HarmonyPrefix]
    [HarmonyPriority(1)]
    [HarmonyPatch(typeof(BaseCharacterComponent), nameof(BaseCharacterComponent.DropOverheadItem))]
    public static bool BaseCharacterComponent_DropOverheadItem_Postfix(ref BaseCharacterComponent __instance)
    {
        if (!__instance.wgo.is_player || !OverheadItemIsHeavy(__instance.overhead_item))
            return true;
        var item = __instance.overhead_item;
        
        List<Item> insert = [item];
        var itemId = item.id;

        WriteLog($"Refreshing and re-sorting stockpile distances.");
        foreach (var pile in SortedStockpiles)
            pile.DistanceFromPlayer = Vector3.Distance(MainGame.me.player_pos, pile.Wgo.pos3);

        SortedStockpiles.Sort((x, y) => x.DistanceFromPlayer.CompareTo(y.DistanceFromPlayer));

        var itemDumped = false;
        foreach (var stockpile in SortedStockpiles)
        {
            WriteLog($"Trying to insert {itemId} into {stockpile.Wgo}, {stockpile.DistanceFromPlayer} units away.");
            var success = TryPutToInventoryAndNull(__instance, stockpile.Wgo, insert);
            if (!success) continue;
            itemDumped = true;
            WriteLog($"Successfully inserted {itemId} into {stockpile.Wgo}, {stockpile.DistanceFromPlayer} units away.");
            ShowLootAddedIcon(item);
            break;
        }

        if (__instance.overhead_item != null || !itemDumped)
        {
            if (TeleportToDumpSiteWhenAllStockPilesFull.Value)
            {
                TeleportItem(__instance, __instance.overhead_item);
                WriteLog($"Teleporting {itemId} to dump site.");
            }
            else
            {
                DropObjectAndNull(__instance, __instance.overhead_item);
                WriteLog($"Dropping object due to teleportation being disabled.");
            }
        }

        return false;
    }


    // [HarmonyPostfix]
    [HarmonyPrefix]
    [HarmonyPriority(1)]
    [HarmonyPatch(typeof(BaseCharacterComponent), nameof(BaseCharacterComponent.SetOverheadItem))]
    public static void BaseCharacterComponent_SetOverheadItem(ref BaseCharacterComponent __instance, ref Item item)
    {
        if (__instance.wgo.is_player && item != null && OverheadItemIsHeavy(item))
        {
            MainGame.me.StartCoroutine(RunFullUpdate());
            // UpdateStockpiles();
        }
    }

    private static bool StockpileIsValid(WorldGameObject wgo)
    {
        return wgo.obj_id.Contains(Constants.ItemObjectId.Timber) ||
               wgo.obj_id.Contains(Constants.ItemObjectId.Ore) ||
               wgo.obj_id.Contains(Constants.ItemObjectId.Stone);
    }

    
    [HarmonyPostfix]
    [HarmonyPriority(1)]
    [HarmonyPatch(typeof(WorldMap), nameof(WorldMap.OnAddNewWGO), typeof(WorldGameObject))]
    public static void WorldGameObject_OnAddNewWGOt(ref WorldGameObject wgo)
    {
        if (StockpileIsValid(wgo))
        {
            AddStockpile(wgo);
        }
    }
    
    [HarmonyPostfix]
    [HarmonyPriority(1)]
    [HarmonyPatch(typeof(WorldMap), nameof(WorldMap.OnDestroyWGO), typeof(WorldGameObject))]
    public static void WorldGameObject_OnDestroyWGO(ref WorldGameObject wgo)
    {
        if (StockpileIsValid(wgo))
        {
            RemoveStockpile(wgo);
        }
    }
}