using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using HarmonyLib;
using Helper;
using UnityEngine;
using WheresMaStorage.lang;

namespace WheresMaStorage;

[HarmonyPatch]
public static partial class MainPatcher
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(SleepGUI), nameof(SleepGUI.Open), null)]
    public static void SleepGUI_Open()
    {
        _invsLoaded = false;
    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(WaitingGUI), nameof(WaitingGUI.Open), null)]
    public static void WaitingGUI_Open()
    {
        _invsLoaded = false;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(OrganEnhancerGUI), nameof(OrganEnhancerGUI.Open), null)]
    public static void OrganEnhancerGUI_Open(ref OrganEnhancerGUI __instance)
    {
        __instance._multi_inventory = MainGame.me.player.GetMultiInventory(exceptions: null, force_world_zone: "",
            player_mi: MultiInventory.PlayerMultiInventory.IncludePlayer, include_toolbelt: true,
            include_bags: true, sortWGOS: true);
    }

    // [HarmonyPrefix]
    // [HarmonyPatch(typeof(WorldGameObject), nameof(WorldGameObject.GetMultiInventory))]
    // public static bool WorldGameObject_GetMultiInventory()
    // {
    //     return false;
    // }
    // //private static bool alreadyDone;
    
    
    [HarmonyPrefix]
    [HarmonyPatch(typeof(WorldGameObject), nameof(WorldGameObject.GetMultiInventory))]
    public static bool WorldGameObject_GetMultiInventory(
        WorldGameObject __instance,
        ref MultiInventory __result,
        List<WorldGameObject> exceptions = null,
        string force_world_zone = "",
        MultiInventory.PlayerMultiInventory player_mi = MultiInventory.PlayerMultiInventory.DontChange,
        bool include_toolbelt = false,
        bool sortWGOS = false,
        bool include_bags = false
    )
    {
        if (!_cfg.SharedInventory) return true;
        if (__instance.obj_id.Contains("refugee") || __instance.GetMyWorldZoneId().Contains("refugee")) return true;
        _zombieWorker = (__instance.has_linked_worker && __instance.linked_worker.obj_id.Contains("zombie")) || __instance.obj_def.id.Contains("zombie");
        var proceed = __instance.obj_id.Contains("church_pulpit") || CrossModFields.IsCraft || _zombieWorker || __instance.obj_id.Contains("compost") || __instance.IsWorker() || __instance.IsInvisibleWorker() || __instance.is_player || __instance.obj_id.StartsWith("mf_");
        
        if (proceed)
        {
            Log($"Passed Proceed: GetMulti: {__instance.obj_id}");
            var inv = new MultiInventory();
            inv.SetInventories(GetMiInventory(__instance.name).all);
            _mi = inv;
            __result = inv;
            return false;
        }
        Log($"Did Not Pass Proceed: GetMulti: {__instance.obj_id}");

        return true;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(WorldMap), nameof(WorldMap.GetWorldGameObjectsByComparator))]
    public static void WorldMap_GetWorldGameObjectsByComparator(ref bool log_if_not_found)
    {
        log_if_not_found = false;
    }


    [HarmonyPostfix]
    [HarmonyBefore("p1xel8ted.GraveyardKeeper.MiscBitsAndBobs")]
    [HarmonyPatch(typeof(GameBalance), nameof(GameBalance.LoadGameBalance))]
    public static void GameBalance_LoadGameBalance()
    {
        if (_gameBalanceAlreadyRun) return;
        _gameBalanceAlreadyRun = true;

        var watch = Stopwatch.StartNew();

        if (_cfg.AllowHandToolDestroy)
        {
            foreach (var itemDef in GameBalance.me.items_data.Where(a => ToolItems.Contains(a.type)))
            {
                itemDef.player_cant_throw_out = false;
            }
        }

        if (_cfg.EnableToolAndPrayerStacking || _cfg.EnableGraveItemStacking || _cfg.EnablePenPaperInkStacking || _cfg.EnableChiselStacking)
        {
            foreach (var item in GameBalance.me.items_data.Where(item => item.stack_count == 1))
            {
                if (_cfg.EnableToolAndPrayerStacking)
                {
                    if (ToolItems.Contains(item.type))
                    {
                        item.stack_count = item.stack_count + _cfg.StackSizeForStackables > 999 ? 999 : _cfg.StackSizeForStackables;
                    }
                }

                if (_cfg.EnableGraveItemStacking)
                {
                    if (GraveItems.Contains(item.type))
                    {
                        item.stack_count = item.stack_count + _cfg.StackSizeForStackables > 999 ? 999 : _cfg.StackSizeForStackables;
                    }
                }

                if (_cfg.EnablePenPaperInkStacking)
                {
                    if (PenPaperInkItems.Any(item.id.Contains))
                    {
                        item.stack_count = item.stack_count + _cfg.StackSizeForStackables > 999 ? 999 : _cfg.StackSizeForStackables;
                    }
                }

                if (!_cfg.EnableChiselStacking) continue;

                if (ChiselItems.Any(item.id.Contains))
                {
                    item.stack_count = item.stack_count + _cfg.StackSizeForStackables > 999 ? 999 : _cfg.StackSizeForStackables;
                }
            }
        }

        if (_cfg.ModifyInventorySize)
        {
            foreach (var od in GameBalance.me.objs_data.Where(od =>
                         od.interaction_type == ObjectDefinition.InteractionType.Chest))
            {
                od.inventory_size += _cfg.AdditionalInventorySpace;
            }
        }

        if (!_cfg.ModifyStackSize) return;

        foreach (var id in GameBalance.me.items_data.Where(id => id.stack_count is > 1 and <= 999))
        {
            id.stack_count = id.stack_count + _cfg.StackSizeForStackables > 999 ? 999 : _cfg.StackSizeForStackables;
        }

        watch.Stop();
        Log($"WMS Modifications Loaded! Completed in {watch.ElapsedMilliseconds}ms");
    }

    [HarmonyPostfix]
    [HarmonyBefore("p1xel8ted.GraveyardKeeper.MiscBitsAndBobs")]
    [HarmonyPatch(typeof(CraftDefinition), nameof(CraftDefinition.takes_item_durability), MethodType.Getter)]
    public static void CraftDefinition_takes_item_durability(ref CraftDefinition __instance, ref bool __result)
    {
        if (__instance == null) return;

        if (_cfg.EnablePenPaperInkStacking)
        {
            if (__instance.needs.Exists(item => item.id.Equals("pen:ink_pen")) && __instance.dur_needs_item > 0)
            {
                __result = false;
            }
        }

        if (_cfg.EnableChiselStacking)
        {
            if (__instance.needs.Exists(item => item.id.Contains("chisel")) && __instance.dur_needs_item > 0)
            {
                __result = false;
            }
        }
    }

    [HarmonyPrefix]
    [HarmonyPriority(1)]
    [HarmonyPatch(typeof(CraftComponent), nameof(CraftComponent.CraftReally))]
    public static void CraftComponent_CraftReally(CraftDefinition craft, bool for_gratitude_points, ref bool start_by_player)
    {
        _gratitudeCraft = for_gratitude_points;
        if (_gratitudeCraft)
        {
            start_by_player = false;
        }
        // Log($"[Gratitude]: Craft: {craft.id}, CraftGratCost: {craft.gratitude_points_craft_cost?.EvaluateFloat(MainGame.me.player,null)}, ForGrat: {for_gratitude_points}, StartedByPlayer: {start_by_player}");
    }

    private static void ResetFlags()
    {
        _invsLoaded = false;
        _gameBalanceAlreadyRun = false;
        MainGame.game_started = false;
        _dropsCleaned = false;
        _debugMessageShown = false;
    }

    [HarmonyPrefix]
    [HarmonyPriority(1)]
    [HarmonyPatch(typeof(InGameMenuGUI), nameof(InGameMenuGUI.OnPressedSaveAndExit))]
    public static void InGameMenuGUI_OnPressedSaveAndExit()
    {
        ResetFlags();
    }


    [HarmonyPrefix]
    [HarmonyPriority(1)]
    [HarmonyPatch(typeof(SaveSlotsMenuGUI), nameof(SaveSlotsMenuGUI.Open))]
    public static void SaveSlotsMenuGUI_Open()
    {
        ResetFlags();
    }

    private static bool _refugeeCraft;

//public bool Interact(bool interaction_start)

    [HarmonyPostfix]
    [HarmonyPriority(1)]
    [HarmonyPatch(typeof(Stats), nameof(Stats.DesignEvent), typeof(string))]
    public static void Stats_DesignEvent(string event_name)
    {
        _refugeeCraft = event_name.Contains("refugee");
    }

//some crafting objects re-acquire the inventories when starting a craft, overwriting our multi.This stops that.
    [HarmonyPostfix]
    [HarmonyPatch(typeof(BaseCraftGUI), nameof(BaseCraftGUI.multi_inventory), MethodType.Getter)]
    public static void BaseCraftGUI_multi_inventory(ref BaseCraftGUI __instance, ref MultiInventory __result)
    {
        if (!_cfg.SharedInventory) return;
        if (__instance.GetCrafteryWGO().obj_id.Contains("refugee") || __instance.name.Contains("refugee"))
        {
            return;
        }

        if (!_zombieWorker)
        {
            if (Time.time - _timeSix > LogGap)
            {
                _timeSix = Time.time;

                Log(
                    $"[BaseCraftGUI.multi_inventory (Getter)]: {__instance.name}, Craftery: {__instance.GetCrafteryWGO().obj_id}");
            }
        }

        // if (__instance.GetCrafteryWGO().obj_id.Contains("refugee") || __instance.name.Contains("refugee"))
        // {
        //     Log($"[BaseCraftGUI.multi_inventory (Getter)]: Refugee request - sending back refugee inventory. {__instance.name}, Craftery: {__instance.GetCrafteryWGO().obj_id}");
        //     __result = _refugeeMi;
        //     return;
        // }

        __result = GetMiInventory($"[BaseCraftGUI.multi_inventory (Getter)]: {__instance.name}, Craftery: {__instance.GetCrafteryWGO().obj_id}");
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(WorldGameObject), nameof(WorldGameObject.ReplaceWithObject))]
    public static void WorldGameObject_ReplaceWithObject(ref WorldGameObject __instance)
    {
        if (__instance.obj_def.interaction_type is ObjectDefinition.InteractionType.Chest or ObjectDefinition.InteractionType.Builder or ObjectDefinition.InteractionType.Craft || __instance.obj_id.StartsWith("mf_"))
        {
            Log($"Chest or MF object built! Object: {__instance.obj_id}, Def: {__instance.obj_def.id}, InteractionType: {__instance.obj_def.interaction_type}");
            _invsLoaded = false;
        }
    }

//set stack size back up before collecting
    [HarmonyPrefix]
    [HarmonyBefore("p1xel8ted.GraveyardKeeper.MiscBitsAndBobs")]
    [HarmonyPatch(typeof(DropResGameObject), nameof(DropResGameObject.CollectDrop))]
    public static void DropResGameObject_CollectDrop(ref DropResGameObject __instance)
    {
        if (!_cfg.EnableGraveItemStacking) return;
        if (!GraveItems.Contains(__instance.res.definition.type)) return;

        __instance.res.definition.stack_count = _cfg.StackSizeForStackables;
    }


//needed for grave removals to work
    [HarmonyPostfix]
    [HarmonyBefore("p1xel8ted.GraveyardKeeper.MiscBitsAndBobs")]
    [HarmonyPatch(typeof(GameBalance), nameof(GameBalance.GetRemoveCraftForItem))]
    public static void GameBalance_GetRemoveCraftForItem(ref CraftDefinition __result)
    {
        foreach (var item in __result.output.Where(a => GraveItems.Contains(a.definition.type)))
        {
            item.definition.stack_count = 1;
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(InventoryPanelGUI), nameof(InventoryPanelGUI.DoOpening))]
    public static void InventoryPanelGUI_DoOpening_Prefix(ref InventoryPanelGUI __instance, ref MultiInventory multi_inventory)
    {
        //if (GUIElements.me.pray_craft.CanClose()) return;
        if (GUIElements.me.pray_craft.gameObject.activeSelf || GUIElements.me.pray_craft.gameObject.activeInHierarchy) return;

        var isVendorPanel = __instance.name.ToLowerInvariant().Contains(Vendor);

        if (isVendorPanel) return;

        var tools = multi_inventory.all.FindAll(a => a.name == "Tools" || a.data.id is "Tools" or "Toolbelt");
        multi_inventory.all.RemoveAll(a => a.name == "Tools" || a.data.id is "Tools" or "Toolbelt");
        if (tools.Count > 0)
        {
            multi_inventory.AddInventory(tools[0], 1);
        }

        Log($"This panel is: {__instance.name}, MultiInventory: {multi_inventory.all.Count}");
        // multi_inventory = GetMiInventory(__instance.name);

        if (_cfg.DontShowEmptyRowsInInventory)
        {
            __instance.dont_show_empty_rows = true;
        }

        if (CrossModFields.IsCraft || CrossModFields.IsVendor || CrossModFields.IsChurchPulpit) return;

        if (MainGame.me.player.GetMyWorldZoneId().Contains("refugee")) return;

        if (_cfg.ShowOnlyPersonalInventory || CrossModFields.IsBarman || CrossModFields.IsTavernCellarRack || CrossModFields.IsSoulBox || CrossModFields.IsChest || CrossModFields.IsWritersTable)
        {
            if (__instance.name.Contains("bag")) return;
            var onlyMineInventory = new MultiInventory();
            var bagCount = 0;
            foreach(var item in multi_inventory.all[0].data.inventory)
            {
                if (item.is_bag)
                {
                    bagCount++;
                }
            }
            
            onlyMineInventory.AddInventory(multi_inventory.all[0]);
            
            if (bagCount > 0)
            {
                for (var i = 0; i < bagCount; i++)
                {
                    onlyMineInventory.AddInventory(multi_inventory.all[i+1]);  
                }
               
            }
            
            // if (multi_inventory.all[1].name.Contains("bag") || multi_inventory.all[1].name.Contains("universal"))
            // {
            //     onlyMineInventory.AddInventory(multi_inventory.all[1]);   
            // }
            multi_inventory = onlyMineInventory;
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(InventoryPanelGUI), nameof(InventoryPanelGUI.DoOpening))]
    public static void InventoryPanelGUI_DoOpening_Postfix(ref InventoryPanelGUI __instance)
    {
        var isChestPanel = __instance.name.ToLowerInvariant().Contains(Chest);
        var isVendorPanel = __instance.name.ToLowerInvariant().Contains(Vendor);
        var isPlayerPanel = __instance.name.ToLowerInvariant().Contains(Player) || (__instance.name.ToLowerInvariant().Contains(Multi) && CrossModFields.CurrentWgoInteraction == null);

        var isResourcePanelProbably = !isChestPanel && !isVendorPanel && !isPlayerPanel;

        __instance._separators.ForEach(a =>
        {
            if ((_cfg.RemoveGapsBetweenSections && isPlayerPanel) || (_cfg.RemoveGapsBetweenSectionsVendor && isVendorPanel) || isResourcePanelProbably)
            {
                a.Hide();
            }
        });

        __instance._custom_widgets.ForEach(a =>
        {
            Log($"CustomWidget: InventoryID {a.inventory_data.id}");
            if (isResourcePanelProbably)
            {
                a.Deactivate();
            }

            if (AlwaysHidePartials.Any(a.inventory_data.id.Contains))
            {
                a.Deactivate();
            }

            HideWidgets(a);
        });

        __instance._widgets.ForEach(a =>
        {
            Log($"Widget: InventoryID {a.inventory_data.id}");
            if (isResourcePanelProbably || isPlayerPanel || isChestPanel)
            {
                if (a.gameObject.activeSelf)
                {
                    SetInventorySizeText(a);
                }
            }

            if (AlwaysHidePartials.Any(a.inventory_data.id.Contains))
            {
                a.Deactivate();
            }

            HideWidgets(a);

            // if (!isResourcePanelProbably && a.name == "Tools" || a.inventory_data.id is "Tools" or "Toolbelt")
            // {
            //     a.Deactivate();
            // }
        });


        void HideWidgets(BaseInventoryWidget a)
        {
            if (isVendorPanel || CrossModFields.IsBarman || CrossModFields.IsTavernCellarRack || CrossModFields.IsSoulBox || CrossModFields.IsChurchPulpit) return;

            Log($"HideWidgets: InventoryID {a.inventory_data.id}");
            var id = a.inventory_data.id;
            if ((_cfg.HideSoulWidgets && id.Contains(Soul)) || (_cfg.HideStockpileWidgets && StockpileWidgetsPartials.Any(id.Contains)) || (_cfg.HideTavernWidgets && id.Contains(Tavern)) || (_cfg.HideWarehouseShopWidgets && id.Contains(Storage)))
            {
                if (!a.inventory_data.id.Contains(Writer))
                {
                    a.Deactivate();
                }
            }
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(InventoryGUI), nameof(InventoryGUI.CloseBag))]
    public static void InventoryGUI_CloseBag()
    {
        _usingBag = false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(InventoryGUI), nameof(InventoryGUI.OpenBag))]
    public static void InventoryGUI_OpenBag()
    {
        _usingBag = true;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(GraveGUI), nameof(GraveGUI.GravePartsFilter), typeof(Item), typeof(ItemDefinition.ItemType))]
    public static void GraveGUI_GravePartsFilter(ref InventoryWidget.ItemFilterResult __result)
    {
        if (!MainGame.game_started) return;
        if (!_cfg.HideInvalidSelections) return;

        if (__result != InventoryWidget.ItemFilterResult.Active)
        {
            __result = InventoryWidget.ItemFilterResult.Inactive;
        }
    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(CraftResourcesSelectGUI), nameof(CraftResourcesSelectGUI.Open), typeof(WorldGameObject), typeof(InventoryWidget.ItemFilterDelegate), typeof(CraftResourcesSelectGUI.ResourceSelectResultDelegate), typeof(bool))]
    public static void CraftResourcesSelectGUI_Open(ref bool force_ignore_toolbelt)
    {
        force_ignore_toolbelt = true;
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(InventoryPanelGUI), nameof(InventoryPanelGUI.Redraw))]
    public static void InventoryPanelGUI_Redraw(ref InventoryPanelGUI __instance)
    {
        //if (MainGame.me.save.IsInTutorial()) return;
        var isChest = __instance.name.ToLowerInvariant().Contains(Chest);
        var isPlayer = __instance.name.ToLowerInvariant().Contains(Player) || (__instance.name.ToLowerInvariant().Contains(Multi) && CrossModFields.CurrentWgoInteraction == null);

        if ((isPlayer || isChest) && _cfg.ShowUsedSpaceInTitles)
        {
            __instance._widgets.ForEach(SetInventorySizeText);
        }
    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(InventoryPanelGUI), nameof(InventoryPanelGUI.SetGrayToNotMainWidgets))]
    public static bool InventoryPanelGUI_SetGrayToNotMainWidgets()
    {
        return !_cfg.DisableInventoryDimming;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(InventoryWidget), nameof(InventoryWidget.FilterItems))]
    public static void InventoryWidget_FilterItems(ref InventoryWidget __instance,
        ref InventoryWidget.ItemFilterDelegate filter_delegate)
    {
        if (__instance.gameObject.transform.parent.transform.parent.transform.parent.name.ToLowerInvariant()
            .Contains(Vendor))
            return;

        if (!_cfg.HideInvalidSelections) return;

        if (_usingBag) return;

        var @delegate = filter_delegate;
        var widget = __instance;
        __instance.items.ForEach(a =>
        {
            switch (@delegate(a.item, widget))
            {
                case InventoryWidget.ItemFilterResult.Active:
                    a.SetGrayState(false);
                    break;

                case InventoryWidget.ItemFilterResult.Inactive:
                    a.Deactivate();
                    break;

                case InventoryWidget.ItemFilterResult.Hide:
                    a.Deactivate();
                    break;

                case InventoryWidget.ItemFilterResult.Unknown:
                    a.DrawUnknown();
                    break;
            }
        });

        var activeCount = __instance.items.Count(x => !x.is_inactive_state);
        // Log($"[InvDataID]: {__instance.inventory_data.id}");
        if (activeCount <= 0)
        {
            __instance.Hide();
        }
        __instance.RecalculateWidgetSize();
        // typeof(InventoryWidget).GetMethod("RecalculateWidgetSize", AccessTools.all)
        //     ?.Invoke(__instance, new object[]
        //     {
        //     });
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(MixedCraftGUI), nameof(MixedCraftGUI.AlchemyItemPickerFilter), typeof(Item), typeof(InventoryWidget))]
    public static void MixedCraftGUI_AlchemyItemPickerFilter(ref InventoryWidget.ItemFilterResult __result)
    {
        if (!MainGame.game_started) return;
        if (!_cfg.HideInvalidSelections) return;

        if (__result != InventoryWidget.ItemFilterResult.Active)
        {
            __result = InventoryWidget.ItemFilterResult.Inactive;
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(SoulContainerWidget), nameof(SoulContainerWidget.SoulItemsFilter), typeof(Item))]
    public static void SoulContainerWidget_SoulItemsFilter(ref InventoryWidget.ItemFilterResult __result)
    {
        if (!MainGame.game_started) return;
        if (!_cfg.HideInvalidSelections) return;

        if (__result != InventoryWidget.ItemFilterResult.Active)
        {
            __result = InventoryWidget.ItemFilterResult.Inactive;
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(SoulHealingWidget), nameof(SoulHealingWidget.SoulItemsFilter), typeof(Item))]
    public static void SoulHealingWidget_SoulItemsFilter(ref InventoryWidget.ItemFilterResult __result)
    {
        if (!MainGame.game_started) return;
        if (!_cfg.HideInvalidSelections) return;

        if (__result != InventoryWidget.ItemFilterResult.Active)
        {
            __result = InventoryWidget.ItemFilterResult.Inactive;
        }
    }

    private static bool _dropsCleaned;
    private static bool _debugMessageShown;

    [HarmonyPrefix]
    [HarmonyPatch(typeof(TimeOfDay), nameof(TimeOfDay.Update))]
    public static void TimeOfDay_Update()
    {
        if (Input.GetKeyUp(_cfg.ReloadConfigKeyBind))
        {
            _cfg = Config.GetOptions();

            if (Time.time - CrossModFields.ConfigReloadTime > CrossModFields.ConfigReloadTimeLength)
            {
                Tools.ShowMessage(GetLocalizedString(strings.ConfigMessage), Vector3.zero);
                CrossModFields.ConfigReloadTime = Time.time;
            }
        }

        if (!MainGame.game_started) return;


        if (_cfg.Debug & !_debugMessageShown)
        {
            Tools.ShowAlertDialog("Where's Ma Storage!", "Please note you have debug mode turned on. It isn't recommended to leave this on unless you have a purpose for it.", string.Empty, true);
            _debugMessageShown = true;
        }

        {
        }

        if (!_invsLoaded)
        {
            MainGame.me.StartCoroutine(LoadWildernessInventories());
            MainGame.me.StartCoroutine(LoadInventories());
        }

        if (_cfg.CollectDropsOnGameLoad)
        {
            if (!_dropsCleaned)
            {
                CollectDrops();
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

    [HarmonyPostfix]
    [HarmonyPatch(typeof(WorldMap), nameof(WorldMap.RescanDropItemsList))]
    public static void WorldMap_RescanDropItemsList(List<Item> ____drop_items)
    {
        _oldDrops = ____drop_items;
    }

    private static List<Item> _oldDrops = new();

    private static void CollectDrops()
    {
        var watch = Stopwatch.StartNew();
        ReScanDrops();


        MainGame.me.player.PutToAllPossibleInventories(_oldDrops, out var cantPut);
        Log($"Can't put: {cantPut.Count}");
        foreach (var item in cantPut)
        {
            Log($"Can't put: {item.id}, Qty: {item.value}");
        }

        foreach (var tp in TechPointDrop.all)
        {
            AccessTools.Method(typeof(TechPointDrop), "Collect")?.Invoke(tp, null);
            Log($"Attempting to collect TP: {tp.type}");
        }

        _dropsCleaned = true;

        watch.Stop();

        Log($"CollectDrops Complete! Completed in {watch.ElapsedMilliseconds}ms");
        _invsLoaded = false; // force a refresh just in case
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(WorldGameObject), nameof(WorldGameObject.DestroyMe))]
    public static void WorldGameObject_DestroyMe(WorldGameObject __instance)
    {
        if (__instance.obj_def.interaction_type is ObjectDefinition.InteractionType.Chest or ObjectDefinition.InteractionType.Builder or ObjectDefinition.InteractionType.Craft || __instance.obj_id.StartsWith("mf_"))
        {
            Log($"Chest or MF object destroyed! Object: {__instance.obj_id}, Def: {__instance.obj_def.id}, InteractionType: {__instance.obj_def.interaction_type}");
            _invsLoaded = false;
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(WorldGameObject), nameof(WorldGameObject.InitNewObject))]
    public static void WorldGameObject_InitNewObject(WorldGameObject __instance)
    {
        if (!_cfg.ModifyInventorySize) return;

        if (__instance.is_player)
        {
            __instance.data.SetInventorySize(_invSize);
        }

        if (string.Equals(__instance.obj_id, NpcBarman))
        {
            __instance.data.SetInventorySize(__instance.obj_def.inventory_size +
                                             _cfg.AdditionalInventorySpace);
        }
    }
}