using System.Collections.Generic;
using System.Linq;
using GYKHelper;
using HarmonyLib;
using UnityEngine;

namespace WheresMaStorage;

[HarmonyPatch]
public static class Patches
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(SleepGUI), nameof(SleepGUI.Open), null)]
    public static void SleepGUI_Open()
    {
        Fields.InvsLoaded = false;
    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(WaitingGUI), nameof(WaitingGUI.Open), null)]
    public static void WaitingGUI_Open()
    {
        Fields.InvsLoaded = false;
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
        if (!Plugin.SharedInventory.Value) return true;
        if (__instance.obj_id.Contains("refugee") || __instance.GetMyWorldZoneId().Contains("refugee")) return true;
        Fields.ZombieWorker = (__instance.has_linked_worker && __instance.linked_worker.obj_id.Contains("zombie")) || __instance.obj_def.id.Contains("zombie");
        var proceed = __instance.obj_id.Contains("church_pulpit") || CrossModFields.IsCraft || Fields.ZombieWorker || __instance.obj_id.Contains("compost") || __instance.IsWorker() || __instance.IsInvisibleWorker() || __instance.is_player || __instance.obj_id.StartsWith("mf_");

        if (proceed)
        {
            Helpers.Log($"Passed Proceed: GetMulti: {__instance.obj_id}");
            var inv = new MultiInventory();
            inv.SetInventories(Invents.GetMiInventory(__instance.name).all);
            Fields.Mi = inv;
            __result = inv;
            return false;
        }

        Helpers.Log($"Did Not Pass Proceed: GetMulti: {__instance.obj_id}");

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
    [HarmonyPatch(typeof(CraftDefinition), nameof(CraftDefinition.takes_item_durability), MethodType.Getter)]
    public static void CraftDefinition_takes_item_durability(ref CraftDefinition __instance, ref bool __result)
    {
        if (__instance == null) return;

        if (Plugin.EnablePenPaperInkStacking.Value)
        {
            if (__instance.needs.Exists(item => item.id.Equals("pen:ink_pen")) && __instance.dur_needs_item > 0)
            {
                __result = false;
            }
        }

        if (Plugin.EnableChiselStacking.Value)
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
        Fields.GratitudeCraft = for_gratitude_points;
        if (Fields.GratitudeCraft)
        {
            start_by_player = false;
        }
        // Helpers.Log($"[Gratitude]: Craft: {craft.id}, CraftGratCost: {craft.gratitude_points_craft_cost?.EvaluateFloat(MainGame.me.player,null)}, ForGrat: {for_gratitude_points}, StartedByPlayer: {start_by_player}");
    }

    private static void ResetFlags()
    {
        Fields.InvsLoaded = false;
        Fields.GameBalanceAlreadyRun = false;
        MainGame.game_started = false;
        Fields.DropsCleaned = false;
        Fields.DebugMessageShown = false;
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
        if (!Plugin.SharedInventory.Value) return;
        if (__instance.GetCrafteryWGO().obj_id.Contains("refugee") || __instance.name.Contains("refugee"))
        {
            return;
        }

        if (!Fields.ZombieWorker)
        {
            if (Time.time - Fields.TimeSix > Fields.LogGap)
            {
                Fields.TimeSix = Time.time;

                Helpers.Log(
                    $"[BaseCraftGUI.multi_inventory (Getter)]: {__instance.name}, Craftery: {__instance.GetCrafteryWGO().obj_id}");
            }
        }

        // if (__instance.GetCrafteryWGO().obj_id.Contains("refugee") || __instance.name.Contains("refugee"))
        // {
        //     Helpers.Log($"[BaseCraftGUI.multi_inventory (Getter)]: Refugee request - sending back refugee inventory. {__instance.name}, Craftery: {__instance.GetCrafteryWGO().obj_id}");
        //     __result = _refugeeMi;
        //     return;
        // }

        __result = Invents.GetMiInventory($"[BaseCraftGUI.multi_inventory (Getter)]: {__instance.name}, Craftery: {__instance.GetCrafteryWGO().obj_id}");
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(WorldGameObject), nameof(WorldGameObject.ReplaceWithObject))]
    public static void WorldGameObject_ReplaceWithObject(ref WorldGameObject __instance)
    {
        if (__instance.obj_def.interaction_type is ObjectDefinition.InteractionType.Chest or ObjectDefinition.InteractionType.Builder or ObjectDefinition.InteractionType.Craft || __instance.obj_id.StartsWith("mf_"))
        {
            Helpers.Log($"Chest or MF object built! Object: {__instance.obj_id}, Def: {__instance.obj_def.id}, InteractionType: {__instance.obj_def.interaction_type}");
            Fields.InvsLoaded = false;
            
        }
    }

//set stack size back up before collecting
    [HarmonyPrefix]
    [HarmonyBefore("p1xel8ted.GraveyardKeeper.MiscBitsAndBobs")]
    [HarmonyPatch(typeof(DropResGameObject), nameof(DropResGameObject.CollectDrop))]
    public static void DropResGameObject_CollectDrop(ref DropResGameObject __instance)
    {
        if (!Plugin.EnableGraveItemStacking.Value) return;
        if (!Fields.GraveItems.Contains(__instance.res.definition.type)) return;

        __instance.res.definition.stack_count = Plugin.StackSizeForStackables.Value;
    }


//needed for grave removals to work
    [HarmonyPostfix]
    [HarmonyBefore("p1xel8ted.GraveyardKeeper.MiscBitsAndBobs")]
    [HarmonyPatch(typeof(GameBalance), nameof(GameBalance.GetRemoveCraftForItem))]
    public static void GameBalance_GetRemoveCraftForItem(ref CraftDefinition __result)
    {
        foreach (var item in __result.output.Where(a => Fields.GraveItems.Contains(a.definition.type)))
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

        var isVendorPanel = __instance.name.ToLowerInvariant().Contains(Fields.Vendor);

        if (isVendorPanel) return;

        var tools = multi_inventory.all.FindAll(a => a.name == "Tools" || a.data.id is "Tools" or "Toolbelt");
        multi_inventory.all.RemoveAll(a => a.name == "Tools" || a.data.id is "Tools" or "Toolbelt");
        if (tools.Count > 0)
        {
            multi_inventory.AddInventory(tools[0], 1);
        }

        Helpers.Log($"This panel is: {__instance.name}, MultiInventory: {multi_inventory.all.Count}");
        // multi_inventory = GetMiInventory(__instance.name);

        if (Plugin.DontShowEmptyRowsInInventory.Value)
        {
            __instance.dont_show_empty_rows = true;
        }

        if (CrossModFields.IsCraft || CrossModFields.IsVendor || CrossModFields.IsChurchPulpit) return;

        if (MainGame.me.player.GetMyWorldZoneId().Contains("refugee")) return;

        if (Plugin.ShowOnlyPersonalInventory.Value || CrossModFields.IsBarman || CrossModFields.IsTavernCellarRack || CrossModFields.IsSoulBox || CrossModFields.IsChest || CrossModFields.IsWritersTable)
        {
            if (__instance.name.Contains("bag")) return;
            var onlyMineInventory = new MultiInventory();
            var bagCount = 0;
            foreach (var item in multi_inventory.all[0].data.inventory)
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
                    onlyMineInventory.AddInventory(multi_inventory.all[i + 1]);
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
        var isChestPanel = __instance.name.ToLowerInvariant().Contains(Fields.Chest);
        var isVendorPanel = __instance.name.ToLowerInvariant().Contains(Fields.Vendor);
        var isPlayerPanel = __instance.name.ToLowerInvariant().Contains(Fields.Player) || (__instance.name.ToLowerInvariant().Contains(Fields.Multi) && CrossModFields.CurrentWgoInteraction == null);

        var isResourcePanelProbably = !isChestPanel && !isVendorPanel && !isPlayerPanel;

        __instance._separators.ForEach(a =>
        {
            if ((Plugin.RemoveGapsBetweenSections.Value && isPlayerPanel) || (Plugin.RemoveGapsBetweenSectionsVendor.Value && isVendorPanel) || isResourcePanelProbably)
            {
                a.Hide();
            }
        });

        __instance._custom_widgets.ForEach(a =>
        {
            Helpers.Log($"CustomWidget: InventoryID {a.inventory_data.id}");
            if (isResourcePanelProbably)
            {
                a.Deactivate();
            }

            if (Fields.AlwaysHidePartials.Any(a.inventory_data.id.Contains))
            {
                a.Deactivate();
            }

            HideWidgets(a);
        });

        __instance._widgets.ForEach(a =>
        {
            Helpers.Log($"Widget: InventoryID {a.inventory_data.id}");
            if (isResourcePanelProbably || isPlayerPanel || isChestPanel)
            {
                if (a.gameObject.activeSelf)
                {
                    Invents.SetInventorySizeText(a);
                }
            }

            if (Fields.AlwaysHidePartials.Any(a.inventory_data.id.Contains))
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

            Helpers.Log($"HideWidgets: InventoryID {a.inventory_data.id}");
            var id = a.inventory_data.id;
            if ((Plugin.HideSoulWidgets.Value && id.Contains(Fields.Soul)) || (Plugin.HideStockpileWidgets.Value && Fields.StockpileWidgetsPartials.Any(id.Contains)) || (Plugin.HideTavernWidgets.Value && id.Contains(Fields.Tavern)) || (Plugin.HideWarehouseShopWidgets.Value && id.Contains(Fields.Storage)))
            {
                if (!a.inventory_data.id.Contains(Fields.Writer))
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
        Fields.UsingBag = false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(InventoryGUI), nameof(InventoryGUI.OpenBag))]
    public static void InventoryGUI_OpenBag()
    {
        Fields.UsingBag = true;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(GraveGUI), nameof(GraveGUI.GravePartsFilter), typeof(Item), typeof(ItemDefinition.ItemType))]
    public static void GraveGUI_GravePartsFilter(ref InventoryWidget.ItemFilterResult __result)
    {
        if (!MainGame.game_started) return;
        if (!Plugin.HideInvalidSelections.Value) return;

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
        var isChest = __instance.name.ToLowerInvariant().Contains(Fields.Chest);
        var isPlayer = __instance.name.ToLowerInvariant().Contains(Fields.Player) || (__instance.name.ToLowerInvariant().Contains(Fields.Multi) && CrossModFields.CurrentWgoInteraction == null);

        if ((isPlayer || isChest) && Plugin.ShowUsedSpaceInTitles.Value)
        {
            __instance._widgets.ForEach(Invents.SetInventorySizeText);
        }
    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(InventoryPanelGUI), nameof(InventoryPanelGUI.SetGrayToNotMainWidgets))]
    public static bool InventoryPanelGUI_SetGrayToNotMainWidgets()
    {
        return !Plugin.DisableInventoryDimming.Value;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(InventoryWidget), nameof(InventoryWidget.FilterItems))]
    public static void InventoryWidget_FilterItems(ref InventoryWidget __instance,
        ref InventoryWidget.ItemFilterDelegate filter_delegate)
    {
        if (__instance.gameObject.transform.parent.transform.parent.transform.parent.name.ToLowerInvariant()
            .Contains(Fields.Vendor))
            return;

        if (!Plugin.HideInvalidSelections.Value) return;

        if (Fields.UsingBag) return;

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
        // Helpers.Log($"[InvDataID]: {__instance.inventory_data.id}");
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
        if (!Plugin.HideInvalidSelections.Value) return;

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
        if (!Plugin.HideInvalidSelections.Value) return;

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
        if (!Plugin.HideInvalidSelections.Value) return;

        if (__result != InventoryWidget.ItemFilterResult.Active)
        {
            __result = InventoryWidget.ItemFilterResult.Inactive;
        }
    }

    private static bool CanCollectDrop(DropResGameObject drop)
    {
        return drop.res.definition is not {item_size: > 1};
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(WorldMap), nameof(WorldMap.RescanDropItemsList))]
    public static void WorldMap_RescanDropItemsList(List<Item> ____drop_items)
    {
        Fields.OldDrops = ____drop_items;
    }
    
    [HarmonyPrefix]
    [HarmonyPatch(typeof(WorldGameObject), nameof(WorldGameObject.DestroyMe))]
    public static void WorldGameObject_DestroyMe(WorldGameObject __instance)
    {
        if (__instance.obj_def.interaction_type is ObjectDefinition.InteractionType.Chest or ObjectDefinition.InteractionType.Builder or ObjectDefinition.InteractionType.Craft || __instance.obj_id.StartsWith("mf_"))
        {
            Helpers.Log($"Chest or MF object destroyed! Object: {__instance.obj_id}, Def: {__instance.obj_def.id}, InteractionType: {__instance.obj_def.interaction_type}");
            Fields.InvsLoaded = false;
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(WorldGameObject), nameof(WorldGameObject.InitNewObject))]
    public static void WorldGameObject_InitNewObject(WorldGameObject __instance)
    {
        if (!Plugin.ModifyInventorySize.Value) return;

        if (__instance.is_player)
        {
            __instance.data.SetInventorySize(Fields.InvSize);
        }

        if (string.Equals(__instance.obj_id, Fields.NpcBarman))
        {
            __instance.data.SetInventorySize(__instance.obj_def.inventory_size +
                                             Plugin.AdditionalInventorySpace.Value);
        }
    }
}