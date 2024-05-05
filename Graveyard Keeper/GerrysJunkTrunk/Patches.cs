using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using GerrysJunkTrunk.lang;
using GYKHelper;
using HarmonyLib;
using UnityEngine;

namespace GerrysJunkTrunk;

[HarmonyPatch]
public partial class Plugin
{
    //should never need these, but will stop a 2nd being built
    [HarmonyPostfix]
    [HarmonyPatch(typeof(BuildModeLogics), nameof(BuildModeLogics.CanBuild))]
    public static void BuildModeLogics_CanBuild(ref bool __result, ref CraftDefinition cd)
    {
        //
        if (InternalShippingBoxBuilt.Value && _shippingBox != null)
        {
            if (cd.id.Contains(ShippingItem))
            {
                __result = false;
            }
        }
    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(InventoryPanelGUI), nameof(InventoryPanelGUI.Open))]
    public static void InventoryPanelGUI_Open()
    {
        AlreadyDone.Clear();
    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(BuildModeLogics), nameof(BuildModeLogics.OnBuildCraftSelected))]
    public static void BuildModeLogics_OnBuildCraftSelected(ref ObjectCraftDefinition cd)
    {
        //
        if (cd.id.Contains(ShippingItem))
        {
            _shippingBuild = true;
            var ocd = GameBalance.me.GetData<ObjectCraftDefinition>("mf_wood_builddesk:p:mf_box_stuff_place");
            cd = ocd;
        }
    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(ChestGUI), nameof(ChestGUI.Hide))]
    public static void ChestGUI_Hide(ref ChestGUI __instance)
    {
        //
        ClearGerryFlag(ref __instance);
    }


    [HarmonyPostfix]
    [HarmonyAfter("p1xel8ted.gyk.miscbitsandbobs", "p1xel8ted.gyk.wheresmastorage")]
    [HarmonyPatch(typeof(ChestGUI), nameof(ChestGUI.MoveItem), typeof(Item), typeof(int), typeof(bool), typeof(Item),
        typeof(bool))]
    public static void ChestGUI_MoveItem(ref ChestGUI __instance)
    {
        //
        if (__instance == null || !_usingShippingBox) return;

        UpdateItemStates(ref __instance);
    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(ChestGUI), nameof(ChestGUI.OnPressedBack))]
    public static void ChestGUI_OnPressedBack(ref ChestGUI __instance)
    {
        //
        ClearGerryFlag(ref __instance);
    }


    [HarmonyPostfix]
    [HarmonyAfter("p1xel8ted.gyk.miscbitsandbobs", "p1xel8ted.gyk.wheresmastorage")]
    [HarmonyPatch(typeof(ChestGUI), nameof(ChestGUI.Open))]
    public static void ChestGUI_Open(ref ChestGUI __instance)
    {
        if (__instance == null || !_usingShippingBox) return;

        var maxItemCount = UnlockedFullPrice() ? LargeMaxItemCount : SmallMaxItemCount;

        // Combined loops for player_panel and chest_panel
        foreach (var panel in new[] {__instance.player_panel, __instance.chest_panel})
        {
            foreach (var item in panel.multi_inventory.all[0].data.inventory
                         .Where(item => item.definition.stack_count > 1))
            {
                TryAdd(StackSizeBackups, item.id, item.definition.stack_count);
                item.definition.stack_count = maxItemCount;
            }
        }

        UpdateItemStates(ref __instance);
    }


    [HarmonyPrefix]
    [HarmonyAfter("p1xel8ted.gyk.miscbitsandbobs", "p1xel8ted.gyk.wheresmastorage")]
    [HarmonyPatch(typeof(InventoryPanelGUI), nameof(ChestGUI.OnItemOver), typeof(InventoryWidget),
        typeof(BaseItemCellGUI))]
    public static void InventoryPanelGUI_OnItemOver_Prefix(ref InventoryPanelGUI __instance,
        ref BaseItemCellGUI item_gui)
    {
        Thread.CurrentThread.CurrentUICulture = CrossModFields.Culture;

        if (!ShowItemPriceTooltips.Value || !UnlockedShippingBox() || __instance == null || item_gui == null ||
            AlreadyDone.Contains(item_gui) || item_gui.id_empty)
            return;

        foreach (var tooltip in new[] {item_gui.x1, item_gui.x2})
        {
            if (tooltip != null && tooltip.tooltip != null && tooltip.tooltip.has_info)
            {
                var itemEarnings = GetItemEarnings(item_gui.item);
                tooltip.tooltip.AddData(new BubbleWidgetSeparatorData());

                tooltip.tooltip.AddData(new BubbleWidgetTextData(
                    strings.GerrysPrice,
                    UITextStyles.TextStyle.Usual, NGUIText.Alignment.Left));
                tooltip.tooltip.AddData(new BubbleWidgetSeparatorData());
                    tooltip.tooltip.AddData(new BubbleWidgetTextData(
                        $"{Trading.FormatMoney(itemEarnings, true)}",
                        UITextStyles.TextStyle.TinyDescription, NGUIText.Alignment.Left));
          
            }
        }

        AlreadyDone.Add(item_gui);
    }


    [HarmonyFinalizer]
    [HarmonyPatch(typeof(InventoryPanelGUI), nameof(ChestGUI.OnItemOver), typeof(InventoryWidget),
        typeof(BaseItemCellGUI))]
    private static Exception InventoryPanelGUI_OnItemOver_Finalizer()
    {
        return null;
    }

    private static void EnvironmentEngine_OnEndOfDay()
    {
        Thread.CurrentThread.CurrentUICulture = CrossModFields.Culture;

        if (!UnlockedShippingBox() || !InternalShippingBoxBuilt.Value || _shippingBox == null) return;
        
        var earnings = GetBoxEarnings(_shippingBox);

        if (earnings > 0)
        {
            Stats.PlayerAddMoney(earnings, strings.Header);
        }

        MainGame.me.player.data.money += earnings;
        _shippingBox.data.inventory.Clear();
        var money = Trading.FormatMoney(earnings, true);

        if (EnableGerry.Value)
        {
            StartGerryRoutine(earnings);
        }
        else
        {
            PlayCoinsSoundAndShowMessage(earnings, money);
        }

        // if (ShowSummaryMessage.Value && earnings > 0)
        // {
        //     ShowSummary(money);
        // }
    }

    private static void GameBalance_LoadGameBalance()
    {
        if (GameBalance.me.craft_data.Exists(a => a == NewItem)) return;

        //Thread.CurrentThread.CurrentUICulture = CrossModFields.Culture;
        var newCd = new ObjectCraftDefinition();
        var cd = GameBalance.me.GetData<ObjectCraftDefinition>("mf_wood_builddesk:p:mf_box_stuff_place");
        newCd.craft_in = cd.craft_in;
        newCd.builder_ids = cd.builder_ids;
        newCd.out_obj = "mf_shipping_box_place";
        newCd.needs = cd.needs;
        newCd.needs_from_wgo = cd.needs_from_wgo;
        newCd.output = cd.output;
        newCd.out_items_expressions = cd.out_items_expressions;
        newCd.output_res_wgo = cd.output_res_wgo;
        newCd.output_set_res_wgo = cd.output_set_res_wgo;
        newCd.set_when_cancelled = cd.set_when_cancelled;
        newCd.output_to_wgo = cd.output_to_wgo;
        newCd.output_to_wgo_on_start = cd.output_to_wgo_on_start;
        newCd.tool_actions = cd.tool_actions;
        newCd.condition = cd.condition;
        newCd.end_script = cd.end_script;
        newCd.end_event = cd.end_event;
        newCd.flag = cd.flag;
        newCd.craft_time = cd.craft_time;
        newCd.energy = cd.energy;
        newCd.gratitude_points_craft_cost = cd.gratitude_points_craft_cost;
        newCd.sanity = cd.sanity;
        newCd.hidden = false;
        newCd.needs_unlock = true;
        newCd.icon = cd.icon;
        newCd.craft_type = cd.craft_type;
        newCd.is_auto = cd.is_auto;
        newCd.not_hide_gui = cd.not_hide_gui;
        newCd.can_craft_always = cd.can_craft_always;
        newCd.game_res_to_mirror_name = cd.game_res_to_mirror_name;
        newCd.game_res_to_mirror_max = cd.game_res_to_mirror_max;
        newCd.change_wgo = cd.change_wgo;
        newCd.use_variations = cd.use_variations;
        newCd.variation_index = cd.variation_index;
        newCd.craft_after_finish = cd.craft_after_finish;
        newCd.one_time_craft = true;
        newCd.force_multi_craft = cd.force_multi_craft;
        newCd.disable_multi_craft = cd.disable_multi_craft;
        newCd.sub_type = cd.sub_type;
        newCd.transfer_needs_to_wgo = cd.transfer_needs_to_wgo;
        newCd.set_out_wgo_params_on_start = cd.set_out_wgo_params_on_start;
        newCd.itempars_add = cd.itempars_add;
        newCd.itempars_set = cd.itempars_set;
        newCd.item_output = cd.item_output;
        newCd.item_needs = cd.item_needs;
        newCd.item_needs_leave = cd.item_needs_leave;
        newCd.dur_needs_item = cd.dur_needs_item;
        newCd.dur_needs_item_index = cd.dur_needs_item_index;
        newCd.difficulty = cd.difficulty;
        newCd.linked_perks = cd.linked_perks;
        newCd.linked_buffs = cd.linked_buffs;
        newCd.custom_name = strings.Header;
        newCd.tab_id = cd.tab_id;
        newCd.buff = cd.buff;
        newCd.needs_quality = cd.needs_quality;
        newCd.k_money = cd.k_money;
        newCd.k_faith = cd.k_faith;
        newCd.linked_sub_id = cd.linked_sub_id;
        newCd.dont_close_window_on_craft = cd.dont_close_window_on_craft;
        newCd.dur_parameter = cd.dur_parameter;
        newCd.dont_show_in_hint = cd.dont_show_in_hint;
        newCd.ach_key = cd.ach_key;
        newCd.craft_time_is_zero = cd.craft_time_is_zero;
        newCd.puff_when_replaced = cd.puff_when_replaced;
        newCd.is_item_crating_craft = cd.is_item_crating_craft;
        newCd.store_last_craft_slot = cd.store_last_craft_slot;
        newCd.hide_quality_icon = cd.hide_quality_icon;
        newCd.enqueue_type = cd.enqueue_type;
        newCd.id = ShippingBoxId;

        NewItem = newCd;

        GameBalance.me.craft_data.Add(NewItem);
        GameBalance.me.craft_obj_data.Add(NewItem);
        GameBalance.me.AddDataUniversal(NewItem);
        GameBalance.me.AddData(NewItem);
    }


    [HarmonyPostfix]
    [HarmonyAfter("p1xel8ted.gyk.wheresmastorage")]
    [HarmonyPatch(typeof(InventoryPanelGUI), nameof(InventoryPanelGUI.DoOpening))]
    public static void InventoryPanelGUI_DoOpening(ref InventoryPanelGUI __instance)
    {
        //
        if (!UnlockedShippingBox()) return;
        if (__instance == null) return;
        Thread.CurrentThread.CurrentUICulture = CrossModFields.Culture;
        var isChest = __instance.name.ToLowerInvariant().Contains("chest");
        var isPlayer = __instance.name.ToLowerInvariant().Contains("player");
        if (_usingShippingBox && isChest && !isPlayer)
        {
            foreach (var inventoryWidget in __instance._widgets)
            {
                var tier = GetTrunkTier();
                inventoryWidget.header_label.text = $"{strings.Header} (T{tier})";
                inventoryWidget.dont_show_empty_rows = true;
                inventoryWidget.SetInactiveStateToEmptyCells();
            }
        }
    }


    [HarmonyPostfix]
    [HarmonyAfter("p1xel8ted.gyk.wheresmastorage")]
    [HarmonyPatch(typeof(InventoryPanelGUI), nameof(InventoryPanelGUI.Redraw))]
    public static void InventoryPanelGUI_Redraw(ref InventoryPanelGUI __instance)
    {
        //
        if (!UnlockedShippingBox()) return;
        if (__instance == null) return;
        Thread.CurrentThread.CurrentUICulture = CrossModFields.Culture;
        var isChest = __instance.name.ToLowerInvariant().Contains("chest");
        var isPlayer = __instance.name.ToLowerInvariant().Contains("player");
        if (_usingShippingBox && isChest && !isPlayer)
        {
            foreach (var inventoryWidget in __instance._widgets)
            {
                var tier = GetTrunkTier();
                inventoryWidget.header_label.text = $"{strings.Header} (T{tier})";
                inventoryWidget.dont_show_empty_rows = true;
                inventoryWidget.SetInactiveStateToEmptyCells();
            }

            var money = GetBoxEarnings(_shippingBox);
            __instance.money_label.text = Trading.FormatMoney(money, true);
        }
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(TechTreeGUIItem), nameof(TechTreeGUIItem.InitGamepadTooltip))]
    public static void TechTreeGUIItem_InitGamepadTooltip(ref TechTreeGUIItem __instance)
    {
        //
        if (__instance == null) return;
        {
            Thread.CurrentThread.CurrentUICulture = CrossModFields.Culture;
            var component = __instance.GetComponent<Tooltip>();
            if (__instance.tech_id.ToLowerInvariant().Contains("Wood processing".ToLowerInvariant()))
            {
                component.AddData(new BubbleWidgetSeparatorData());
                component.AddData(new BubbleWidgetTextData(strings.Stage1Header, UITextStyles.TextStyle.HintTitle));
                component.AddData(new BubbleWidgetTextData(strings.Stage1Des, UITextStyles.TextStyle.TinyDescription,
                    NGUIText.Alignment.Left));
            }

            if (__instance.tech_id.ToLowerInvariant().Contains("Engineer".ToLowerInvariant()))
            {
                component.AddData(new BubbleWidgetSeparatorData());
                component.AddData(new BubbleWidgetTextData(strings.Stage2Header, UITextStyles.TextStyle.HintTitle));
                component.AddData(new BubbleWidgetTextData(strings.Stage2Des, UITextStyles.TextStyle.TinyDescription,
                    NGUIText.Alignment.Left));
            }

            if (__instance.tech_id.ToLowerInvariant().Contains("Best friend".ToLowerInvariant()))
            {
                component.AddData(new BubbleWidgetSeparatorData());
                component.AddData(new BubbleWidgetTextData(strings.Stage3Header, UITextStyles.TextStyle.HintTitle));
                component.AddData(new BubbleWidgetTextData(strings.Stage3Des, UITextStyles.TextStyle.TinyDescription,
                    NGUIText.Alignment.Left));
            }
        }
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(TechUnlock), nameof(TechUnlock.GetTooltip), typeof(Tooltip))]
    public static void TechUnlock_GetTooltip(ref TechUnlock __instance, ref Tooltip tooltip)
    {
        //
        if (__instance != null)
        {
            Thread.CurrentThread.CurrentUICulture = CrossModFields.Culture;
            if (LazyInput.gamepad_active) return;
            var name = __instance.GetData().name;

            if (name.ToLowerInvariant().Contains("Wooden plank".ToLowerInvariant()))
            {
                tooltip.AddData(new BubbleWidgetBlankSeparatorData());
                tooltip.AddData(new BubbleWidgetTextData(strings.Stage1Header, UITextStyles.TextStyle.HintTitle,
                    NGUIText.Alignment.Left));
                tooltip.AddData(new BubbleWidgetTextData(strings.Stage1Des, UITextStyles.TextStyle.TinyDescription,
                    NGUIText.Alignment.Left));
            }

            if (name.ToLowerInvariant().Contains("Engineer".ToLowerInvariant()))
            {
                tooltip.AddData(new BubbleWidgetBlankSeparatorData());
                tooltip.AddData(new BubbleWidgetTextData(strings.Stage2Header, UITextStyles.TextStyle.HintTitle,
                    NGUIText.Alignment.Left));
                tooltip.AddData(new BubbleWidgetTextData(strings.Stage2Des, UITextStyles.TextStyle.TinyDescription,
                    NGUIText.Alignment.Left));
            }

            if (name.ToLowerInvariant().Contains("Jeweler".ToLowerInvariant()))
            {
                tooltip.AddData(new BubbleWidgetBlankSeparatorData());
                tooltip.AddData(new BubbleWidgetTextData(strings.Stage3Header, UITextStyles.TextStyle.HintTitle,
                    NGUIText.Alignment.Left));
                tooltip.AddData(new BubbleWidgetTextData(strings.Stage3Des, UITextStyles.TextStyle.TinyDescription,
                    NGUIText.Alignment.Left));
            }
        }
    }


    // [HarmonyPostfix]
    // [HarmonyPatch(typeof(Vendor), nameof(Vendor.CanTradeItem))]
    // public static void Vendor_CanTradeItem(ref Vendor __instance, ref bool __result)
    // {
    //     
    //     if (!UnlockedShippingBox()) return;
    //     if (__instance == null || _myVendor == null || _myVendor.vendor == null) return;
    //     if (__instance.Equals(_myVendor.vendor))
    //     {
    //         __result = true;
    //     }
    // }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(WorldGameObject), nameof(WorldGameObject.DestroyMe))]
    public static void WorldGameObject_DestroyMe(ref WorldGameObject __instance)
    {
        //
        if (!UnlockedShippingBox()) return;
        if (__instance == null) return;
        if (string.Equals(__instance.custom_tag, ShippingBoxTag))
        {
            WriteLog($"Removed Shipping Box!");
            _shippingBox = null;
            InternalShippingBoxBuilt.Value = false;
            var sbCraft = GameBalance.me.GetData<ObjectCraftDefinition>(ShippingBoxId);
            sbCraft.hidden = false;
        }
    }

    private static void WorldGameObject_Interact(WorldGameObject __instance, WorldGameObject other_obj)
    {
        if (!UnlockedShippingBox() || __instance == null ||
            !string.Equals(__instance.custom_tag, ShippingBoxTag)) return;

        WriteLog($"Found Shipping Box! {__instance.data.drop_zone_id}, Other: {other_obj.obj_id}");

        _interactedObject = __instance;
        _usingShippingBox = true;
        __instance.data.drop_zone_id = ShippingBoxTag;
        __instance.custom_tag = ShippingBoxTag;

        var invSize = UnlockedShippingBoxExpansion() ? LargeInvSize : SmallInvSize;
        __instance.data.SetInventorySize(invSize);
        __instance.data.money = GetBoxEarnings(__instance);

        _shippingBox = __instance;
    }

    [HarmonyFinalizer]
    [HarmonyPatch(typeof(WorldGameObject), nameof(WorldGameObject.Interact))]
    private static Exception Finalizer()
    {
        return null;
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(WorldGameObject), nameof(WorldGameObject.ReplaceWithObject))]
    public static void WorldGameObject_ReplaceWithObject(ref WorldGameObject __instance, ref string new_obj_id)
    {
        if (!UnlockedShippingBox() || __instance == null ||
            InternalShippingBoxBuilt.Value && _shippingBox != null) return;

        if (string.Equals(new_obj_id, "mf_box_stuff") && _shippingBuild)
        {
            WriteLog($"Built Shipping Box!");
            var sbCraft = GameBalance.me.GetData<ObjectCraftDefinition>(ShippingBoxId);
            sbCraft.hidden = true;
            __instance.custom_tag = ShippingBoxTag;

            _shippingBuild = false;
            _shippingBox = __instance;

            InternalShippingBoxBuilt.Value = true;

            UpdateShippingBox(sbCraft, __instance);
        }
    }

    // [HarmonyPostfix]
    // [HarmonyPatch(typeof(WorldMap), nameof(WorldMap.RescanWGOsList))]
    // public static void WorldMap_RescanWGOsList()
    // {
    //     //
    //     if (CrossModFields.WorldNpcs == null) return;
    //     foreach (var npc in CrossModFields.WorldNpcs.Where(npc => npc.vendor != null))
    //     {
    //         var known =
    //             MainGame.me.save.known_npcs.npcs.Exists(a => string.Equals(a.npc_id, npc.vendor.id));
    //         if (known)
    //         {
    //             var newVendor = Instantiate(npc, npc.transform.parent, instantiateInWorldSpace: false) as WorldGameObject;
    //             if (newVendor != null)
    //             {
    //                 newVendor.name = $"{npc.name}_GERRY";
    //                 newVendor.vendor.id = $"{npc.vendor.id}_GERRY";
    //                 newVendor.obj_id = $"{npc.name}_GERRY";
    //                 Log.LogWarning(
    //                     $"[WorldMap.RescanWGOsList] Found Vendor: {npc.name} - created GERRY version: {newVendor.name} // {newVendor.vendor.id}");
    //                 KnownVendors.Add(newVendor);
    //             }
    //         }
    //
    //         KnownVendors.RemoveAll(a => a.vendor.id.ToLowerInvariant().Contains("hunchback"));
    //     }
    // }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(WorldZone), nameof(WorldZone.GetZoneWGOs))]
    public static void WorldZone_GetZoneWGOs(ref List<WorldGameObject> __result)
    {
        if (__result == null) return;
        if (_interactedObject != null &&
            _interactedObject.obj_def.interaction_type == ObjectDefinition.InteractionType.Builder) return;

        var count = __result.RemoveAll(a => a.custom_tag == ShippingBoxTag || a.data.drop_zone_id == ShippingBoxTag);
        if (count > 0)
        {
            WriteLog($"[WorldZone.GetZoneWGOs] Removed Shipping Box From WorldMap Objects");
        }

        _interactedObject = null;
    }
}