namespace WheresMaStorage;

public static class Helpers
{
    private readonly static Dictionary<string, bool> OriginalHandToolDestroy = new();
    internal readonly static Dictionary<string, int> OriginalInventorySizes = new();
    private readonly static Dictionary<string, int> OriginalStackSizes = new();

    internal static void RunWmsTasks()
    {
        Plugin.Log.LogInfo("Running WMS Tasks as the Player has spawned in or a new chest/craft has been built.");
        if (!MainGame.game_started) return;

        if (Plugin.Debug.Value && !Fields.DebugMessageShown)
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
        Plugin.Log.LogInfo("Running WMS GameBalanceLoad as GameBalance has been loaded.");
        var watch = Stopwatch.StartNew();

        BackupOriginalData();

        UpdateToolDestroy();
        UpdateInventorySizes();
        UpdateStackSizes();

        watch.Stop();
        Log($"WMS Modifications Loaded! Completed in {watch.ElapsedMilliseconds}ms");
    }

    private static void BackupOriginalData()
    {
        foreach (var itemDefinition in GameBalance.me.items_data)
        {
            Log($"Backing up stack size for: {itemDefinition.id}, {itemDefinition.stack_count}");
            OriginalStackSizes.TryAdd(itemDefinition.id, itemDefinition.stack_count);
        }

        foreach (var objectDefinition in GameBalance.me.objs_data)
        {
            Log($"Backing up inventory size for: {objectDefinition.id}, {objectDefinition.inventory_size}");
            OriginalInventorySizes.TryAdd(objectDefinition.id, objectDefinition.inventory_size);
        }

        foreach (var itemDefinition in GameBalance.me.items_data.Where(a => a.is_tool))
        {
            Log($"Backing up hand tool destroy for: {itemDefinition.id}, {itemDefinition.player_cant_throw_out}");
            OriginalHandToolDestroy.TryAdd(itemDefinition.id, itemDefinition.player_cant_throw_out);
        }
    }

    internal static void UpdateStackSizes()
    {
        RestoreStackSizes();
        if (!Plugin.ModifyStackSize.Value) return;

        foreach (var id in GameBalance.me.items_data.Where(id => id.stack_count is > 1 and <= 999))
        {
            if (id.is_tool || id.IsWeapon() || id.IsEquipment() || id.type == ItemDefinition.ItemType.Preach || Fields.PenPaperInkItems.Contains(id.id) || Fields.ChiselItems.Contains(id.id) || Fields.GraveItems.Contains(id.type)) continue;
            if (!OriginalStackSizes.TryGetValue(id.id, out var originalSize)) continue;

            var newSize = Mathf.Min(originalSize + Plugin.StackSizeForStackables.Value, 999);
            Log($"Modifying stack size for: {id.id}, {id.stack_count} -> {newSize}");
            id.stack_count = newSize;
        }

        if (Plugin.EnableEquipmentStacking.Value)
        {
            foreach (var item in GameBalance.me.items_data.Where(item => item.IsEquipment() && !item.is_tool))
            {
                item.stack_count = Mathf.Min(item.stack_count + Plugin.StackSizeForStackables.Value, 999);
                Log($"EquipmentStacking: Modifying stack size for: {item.id}, {item.stack_count}");
            }
        }

        if (Plugin.EnableWeaponStacking.Value)
        {
            foreach (var item in GameBalance.me.items_data.Where(item => item.IsWeapon()))
            {
                item.stack_count = Mathf.Min(item.stack_count + Plugin.StackSizeForStackables.Value, 999);
                Log($"WeaponStacking: Modifying stack size for: {item.id}, {item.stack_count}");
            }
        }


        if (Plugin.EnableToolStacking.Value)
        {
            foreach (var item in GameBalance.me.items_data.Where(item => item.is_tool && !item.IsWeapon()))
            {
                item.stack_count = Mathf.Min(item.stack_count + Plugin.StackSizeForStackables.Value, 999);
                Log($"ToolStacking: Modifying stack size for: {item.id}, {item.stack_count}");
            }
        }

        if (Plugin.EnablePrayerStacking.Value)
        {
            foreach (var item in GameBalance.me.items_data.Where(item => item.type == ItemDefinition.ItemType.Preach))
            {
                item.stack_count = Mathf.Min(item.stack_count + Plugin.StackSizeForStackables.Value, 999);
                Log($"PrayerStacking: Modifying stack size for: {item.id}, {item.stack_count}");
            }
        }

        if (Plugin.EnablePenPaperInkStacking.Value)
        {
            foreach (var item in GameBalance.me.items_data.Where(item => Fields.PenPaperInkItems.Any(item.id.Contains)))
            {
                item.stack_count = Mathf.Min(item.stack_count + Plugin.StackSizeForStackables.Value, 999);
                Log($"PenPaperInkStacking: Modifying stack size for: {item.id}, {item.stack_count}");
            }
        }

        if (Plugin.EnableChiselStacking.Value)
        {
            foreach (var item in GameBalance.me.items_data.Where(item => Fields.ChiselItems.Any(item.id.Contains)))
            {
                item.stack_count = Mathf.Min(item.stack_count + Plugin.StackSizeForStackables.Value, 999);
                Log($"ChiselStacking: Modifying stack size for: {item.id}, {item.stack_count}");
            }
        }

        if (Plugin.EnableGraveItemStacking.Value)
        {
            foreach (var item in GameBalance.me.items_data.Where(item => Fields.GraveItems.Contains(item.type)))
            {
                item.stack_count = Mathf.Min(item.stack_count + Plugin.StackSizeForStackables.Value, 999);
                Log($"GraveItemStacking: Modifying stack size for: {item.id}, {item.stack_count}");
            }
        }
    }

    private static void RestoreStackSizes()
    {
        foreach (var id in GameBalance.me.items_data)
        {
            if (!OriginalStackSizes.TryGetValue(id.id, out var originalSize)) continue;

            Log($"Restoring stack size for: {id.id}, {id.stack_count} -> {originalSize}");
            id.stack_count = originalSize;
        }
    }

    private static void RestoreToolDestroy()
    {
        foreach (var itemDef in GameBalance.me.items_data.Where(a => a.is_tool))
        {
            if (!OriginalHandToolDestroy.TryGetValue(itemDef.id, out var originalValue)) continue;

            Log($"Restoring hand tool destroy for: {itemDef.id}, {itemDef.player_cant_throw_out} -> {originalValue}");
            itemDef.player_cant_throw_out = originalValue;
        }
    }

    private static void RestoreInventorySizes()
    {
        if (MainGame.me.player && MainGame.game_started)
        {
            Log("Restoring Player Inventory Size to 20");
            MainGame.me.player.data.SetInventorySize(20);
        }

        foreach (var od in GameBalance.me.objs_data.Where(a => a.interaction_type == ObjectDefinition.InteractionType.Chest))
        {
            if (!OriginalInventorySizes.TryGetValue(od.id, out var originalSize)) continue;

            Log($"Restoring Inventory Size for: {od.id}, {od.inventory_size} -> {originalSize}");
            od.inventory_size = originalSize;
        }
    }

    internal static void UpdateToolDestroy()
    {
        RestoreToolDestroy();
        if (!Plugin.AllowHandToolDestroy.Value) return;

        foreach (var itemDef in GameBalance.me.items_data.Where(a => a.is_tool))
        {
            Log($"Modifying hand tool destroy for: {itemDef.id}");
            itemDef.player_cant_throw_out = false;
        }
    }

    internal static void UpdateInventorySizes()
    {
        RestoreInventorySizes();
        if (!Plugin.ModifyInventorySize.Value) return;

        if (MainGame.me.player && MainGame.game_started)
        {
            Log($"Modifying Player Inventory Size from 20 to {Fields.PlayerInventorySize}");
            MainGame.me.player.data.SetInventorySize(Fields.PlayerInventorySize);
        }

        foreach (var od in GameBalance.me.objs_data.Where(od => od.interaction_type == ObjectDefinition.InteractionType.Chest && IsValidStorage(od.inventory_size)))
        {
            if (!OriginalInventorySizes.TryGetValue(od.id, out var originalSize)) continue;

            od.inventory_size = originalSize + Plugin.AdditionalInventorySpace.Value;
            Log($"Modifying Inventory Size for: {od.id}, {originalSize} -> {od.inventory_size}");
        }
    }

    private static bool IsValidStorage(int size)
    {
        return size is 20 or 25 or 5;
    }

    private static void CollectDrops()
    {
        var watch = Stopwatch.StartNew();
        var player = MainGame.me.player;

        WorldMap.RescanDropItemsList();

        var cantCollectList = new List<DropResGameObject>();
        
        foreach (var drop in Tools.GetWorldDrops().Where(Tools.CanCollectDrop))
        {
            if (drop.is_collected)
            {
                Plugin.Log.LogInfo($"Drop already collected: {drop.res.id}");
                continue;
            }

            var amount = player.CanCollectDrop(drop);
            if (amount == 0)
            {
                drop.UnsuccessfullPickup(player);
                cantCollectList.Add(drop);
                Plugin.Log.LogInfo($"Can't collect: {drop.res.id}. Not enough space.");
                continue;
            }

            if (drop.res.value == amount)
            {
                drop.CollectDrop(player);
                Plugin.Log.LogInfo($"Collected: {drop.res.id}, Qty: {amount}");
                continue;
            }

            drop.res.value -= amount;
            drop.RedrawStackCounter();
            player.data.AddItem(drop.res.id, amount);
            cantCollectList.Add(drop);
            Plugin.Log.LogInfo($"Partially Collected: {drop.res.id}, Qty: {amount}");
        }

        foreach (var drop in cantCollectList)
        {
            drop.transform.position = new Vector3(3708.6f, 214.6f, 0.0f);
            drop.DoTryMerging();
            drop.UpdateMe();
            Plugin.Log.LogInfo($"Moving {drop.res.id} next to the meditation spot.");
        }

        foreach (var tp in TechPointDrop.all)
        {
            tp.Collect();
            Plugin.Log.LogInfo($"Attempting to collect TP: {tp.type}");
        }

        Fields.DropsCleaned = true;

        watch.Stop();

        Plugin.Log.LogInfo($"CollectDrops Complete! Completed in {watch.ElapsedMilliseconds}ms");
        Fields.InventoriesLoaded = false; // force a refresh just in case
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
            Plugin.Log.LogError(message);
        }
        else if (Plugin.Debug.Value)
        {
            Plugin.Log.LogInfo(message);
        }
    }

}