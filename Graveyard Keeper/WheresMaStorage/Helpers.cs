namespace WheresMaStorage;

public static class Helpers
{
    internal static void RunWmsTasks()
    {
        Plugin.Log.LogInfo($"Running WMS Tasks as the Player has spawned in or a new chest/craft has been built.");
        if (!MainGame.game_started) return;

        if (Plugin.Debug.Value & !Fields.DebugMessageShown)
        {
            Tools.ShowAlertDialog("Where's Ma Storage!", "Please note you have debug mode turned on. It isn't recommended to leave this on unless you have a purpose for it.", string.Empty, true);
            Fields.DebugMessageShown = true;
        }


        MainGame.me.StartCoroutine(Invents.LoadWildernessInventories());
        MainGame.me.StartCoroutine(Invents.LoadInventories());


        if (Plugin.CollectDropsOnGameLoad.Value && !Fields.DropsCleaned)
        {
            CollectDrops();
        }
    }

    internal static void GameBalanceLoad()
    {
        if (Fields.GameBalanceAlreadyRun) return;
        Fields.GameBalanceAlreadyRun = true;
        Plugin.Log.LogInfo($"Running WMS GameBalanceLoad as GameBalance has been loaded.");
        var watch = Stopwatch.StartNew();

        if (Plugin.AllowHandToolDestroy.Value)
        {
            foreach (var itemDef in GameBalance.me.items_data.Where(a => Fields.ToolItems.Contains(a.type)))
            {
                itemDef.player_cant_throw_out = false;
            }
        }

        if (Plugin.EnableToolAndPrayerStacking.Value || Plugin.EnableGraveItemStacking.Value || Plugin.EnablePenPaperInkStacking.Value || Plugin.EnableChiselStacking.Value)
        {
            foreach (var item in GameBalance.me.items_data.Where(item => item.stack_count == 1))
            {
                if (Plugin.EnableToolAndPrayerStacking.Value)
                {
                    if (Fields.ToolItems.Contains(item.type))
                    {
                        item.stack_count = item.stack_count + Plugin.StackSizeForStackables.Value > 999 ? 999 : Plugin.StackSizeForStackables.Value;
                    }
                }

                if (Plugin.EnableGraveItemStacking.Value)
                {
                    if (Fields.GraveItems.Contains(item.type))
                    {
                        item.stack_count = item.stack_count + Plugin.StackSizeForStackables.Value > 999 ? 999 : Plugin.StackSizeForStackables.Value;
                    }
                }

                if (Plugin.EnablePenPaperInkStacking.Value)
                {
                    if (Fields.PenPaperInkItems.Any(item.id.Contains))
                    {
                        item.stack_count = item.stack_count + Plugin.StackSizeForStackables.Value > 999 ? 999 : Plugin.StackSizeForStackables.Value;
                    }
                }

                if (!Plugin.EnableChiselStacking.Value) continue;

                if (Fields.ChiselItems.Any(item.id.Contains))
                {
                    item.stack_count = item.stack_count + Plugin.StackSizeForStackables.Value > 999 ? 999 : Plugin.StackSizeForStackables.Value;
                }
            }
        }

        if (Plugin.ModifyInventorySize.Value)
        {
            foreach (var od in GameBalance.me.objs_data.Where(od =>
                         od.interaction_type == ObjectDefinition.InteractionType.Chest))
            {
                od.inventory_size += Plugin.AdditionalInventorySpace.Value;
            }
        }

        if (!Plugin.ModifyStackSize.Value) return;

        foreach (var id in GameBalance.me.items_data.Where(id => id.stack_count is > 1 and <= 999))
        {
            id.stack_count = id.stack_count + Plugin.StackSizeForStackables.Value > 999 ? 999 : Plugin.StackSizeForStackables.Value;
        }

        watch.Stop();
        Log($"WMS Modifications Loaded! Completed in {watch.ElapsedMilliseconds}ms");
    }


    private static void CollectDrops()
    {
        var watch = Stopwatch.StartNew();
        ReScanDrops();


        MainGame.me.player.PutToAllPossibleInventories(WorldMap._drop_items, out var cantPut);
        Log($"Can't put: {cantPut.Count}");
        foreach (var item in cantPut)
        {
            Log($"Can't put: {item.id}, Qty: {item.value}");
        }

        foreach (var tp in TechPointDrop.all)
        {
            tp.Collect();
            Log($"Attempting to collect TP: {tp.type}");
        }

        Fields.DropsCleaned = true;

        watch.Stop();

        Log($"CollectDrops Complete! Completed in {watch.ElapsedMilliseconds}ms");
        Fields.InvsLoaded = false; // force a refresh just in case
    }

    internal static string GetLocalizedString(string content)
    {
        if (CrossModFields.Culture != null) Thread.CurrentThread.CurrentUICulture = CrossModFields.Culture;
        return content;
    }

    internal static void Log(string message, bool error = false)
    {
        if (error)
        {
            Plugin.Log.LogError($"{message}");
        }
        else
        {
            if (Plugin.Debug.Value)
            {
                Plugin.Log.LogInfo($"{message}");
            }
        }
    }

    private static void ReScanDrops()
    {
        var mapDrops = MainGame.me.world_root.GetComponentsInChildren<DropResGameObject>(true);
        foreach (var drop in mapDrops)
        {
            if (!CanCollectDrop(drop))
            {
                Log($"[Drop]: {drop.res.id} is not collectable.");
                continue;
            }

            Log($"Attempting to collect: {drop.res.id}, Qty: {drop.res.value}");
            drop.CollectDrop(MainGame.me.player);
        }

        WorldMap.RescanDropItemsList();
    }

    private static bool CanCollectDrop(DropResGameObject drop)
    {
        return drop.res.definition is not {item_size: > 1};
    }
}