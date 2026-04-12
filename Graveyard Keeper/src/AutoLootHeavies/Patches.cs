namespace AutoLootHeavies;

[Harmony]
[HarmonyPriority(1)]
public static class Patches
{
    [HarmonyPrefix]
    [HarmonyPriority(1)]
    [HarmonyPatch(typeof(BaseCharacterComponent), nameof(BaseCharacterComponent.DropOverheadItem))]
    public static bool BaseCharacterComponent_DropOverheadItem_Postfix(BaseCharacterComponent __instance)
    {
        if (!__instance.wgo.is_player || !__instance.has_overhead || !Plugin.OverheadItemIsHeavy(__instance.overhead_item))
            return true;
        var item = __instance.overhead_item;

        List<Item> insert = [item];
        var itemId = item.id;

        Plugin.SortedStockpiles.RemoveAll(pile => pile.Wgo == null);

        if (Plugin.DebugEnabled) Plugin.WriteLog($"[DropOverhead] {itemId}: refresh + sort ({Plugin.SortedStockpiles.Count} stockpiles)");
        foreach (var pile in Plugin.SortedStockpiles)
        {
            pile.DistanceFromPlayer = Vector3.Distance(MainGame.me.player_pos, pile.Wgo.pos3);
        }

        Plugin.SortedStockpiles.Sort((x, y) => x.DistanceFromPlayer.CompareTo(y.DistanceFromPlayer));

        var itemDumped = false;
        foreach (var stockpile in Plugin.SortedStockpiles)
        {
            if (Plugin.DebugEnabled) Plugin.WriteLog($"[DropOverhead] try {itemId} → {stockpile.Wgo.obj_id} @ {stockpile.DistanceFromPlayer:F1}");
            var success = Plugin.TryPutToInventoryAndNull(__instance, stockpile.Wgo, insert);
            if (!success) continue;
            itemDumped = true;
            if (Plugin.DebugEnabled) Plugin.WriteLog($"[DropOverhead] inserted {itemId} → {stockpile.Wgo.obj_id} @ {stockpile.DistanceFromPlayer:F1}");
            Plugin.ShowLootAddedIcon(item);
            break;
        }

        if (__instance.overhead_item != null || !itemDumped)
        {
            if (Plugin.TeleportToDumpSiteWhenAllStockPilesFull.Value)
            {
                Plugin.TeleportItem(__instance, __instance.overhead_item);
                if (Plugin.DebugEnabled) Plugin.WriteLog($"[DropOverhead] fallback → teleport {itemId} to dump site");
            }
            else
            {
                Plugin.DropObjectAndNull(__instance, __instance.overhead_item);
                if (Plugin.DebugEnabled) Plugin.WriteLog($"[DropOverhead] fallback → drop {itemId} (teleport disabled)");
            }
        }

        return false;
    }


    [HarmonyPrefix]
    [HarmonyPriority(1)]
    [HarmonyPatch(typeof(BaseCharacterComponent), nameof(BaseCharacterComponent.SetOverheadItem))]
    public static void BaseCharacterComponent_SetOverheadItem(BaseCharacterComponent __instance, Item item)
    {
        if (__instance.wgo.is_player && item != null && Plugin.OverheadItemIsHeavy(item))
        {
            MainGame.me.StartCoroutine(Plugin.RunFullUpdate());
        }
    }

    [HarmonyPostfix]
    [HarmonyPriority(1)]
    [HarmonyPatch(typeof(WorldMap), nameof(WorldMap.OnAddNewWGO), typeof(WorldGameObject))]
    public static void WorldGameObject_OnAddNewWGOt(WorldGameObject wgo)
    {
        if (Plugin.StockpileIsValid(wgo))
        {
            Plugin.AddStockpile(wgo);
        }
    }

    [HarmonyPostfix]
    [HarmonyPriority(1)]
    [HarmonyPatch(typeof(WorldMap), nameof(WorldMap.OnDestroyWGO), typeof(WorldGameObject))]
    public static void WorldGameObject_OnDestroyWGO(WorldGameObject wgo)
    {
        if (Plugin.StockpileIsValid(wgo))
        {
            Plugin.RemoveStockpile(wgo);
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(WorldGameObject), nameof(WorldGameObject.Interact))]
    public static void WorldGameObject_Interact_Postfix(WorldGameObject __instance)
    {
        Plugin.WorldGameObjectInteract(__instance);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameSave), nameof(GameSave.GlobalEventsCheck))]
    public static void GameSave_GlobalEventsCheck_DebugWarning()
    {
        Plugin.ShowDebugWarningOnce();
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(MainGame), nameof(MainGame.Update))]
    public static void MainGame_Update()
    {
        if (!MainGame.game_started)
        {
            Plugin.InitialFullUpdate = false;
            return;
        }

        if (!Plugin.InitialFullUpdate)
        {
            Plugin.InitialFullUpdate = true;
            Plugin.SortedStockpiles.Clear();
            MainGame.me.StartCoroutine(Plugin.RunFullUpdate());
        }

        Plugin.CheckKeybinds();
    }
}
