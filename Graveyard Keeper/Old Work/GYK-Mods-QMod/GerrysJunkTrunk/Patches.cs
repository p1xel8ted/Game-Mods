using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using GerrysJunkTrunk.lang;
using HarmonyLib;
using Helper;
using UnityEngine;
using Object = UnityEngine.Object;
using Tools = Helper.Tools;

namespace GerrysJunkTrunk;

[HarmonyPatch]
public static partial class MainPatcher
{
    //should never need these, but will stop a 2nd being built
    [HarmonyPostfix]
    [HarmonyPatch(typeof(BuildModeLogics), nameof(BuildModeLogics.CanBuild))]
    public static void BuildModeLogics_CanBuild(ref bool __result, ref CraftDefinition cd)
    {
        //
        if (_internalCfg.shippingBoxBuilt && _shippingBox != null)
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
        ClearGerryFlag(__instance);
    }


    [HarmonyPostfix]
    [HarmonyAfter("p1xel8ted.GraveyardKeeper.MiscBitsAndBobs", "p1xel8ted.GraveyardKeeper.WheresMaStorage")]
    [HarmonyPatch(typeof(ChestGUI), nameof(ChestGUI.MoveItem), typeof(Item), typeof(int), typeof(bool), typeof(Item), typeof(bool))]
    public static void ChestGUI_MoveItem(ref ChestGUI __instance)
    {
        //
        if (__instance == null || !_usingShippingBox) return;

        UpdateItemStates(__instance: ref __instance);
    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(ChestGUI), nameof(ChestGUI.OnPressedBack))]
    public static void ChestGUI_OnPressedBack(ref ChestGUI __instance)
    {
        //
        ClearGerryFlag(__instance);
    }


    [HarmonyPostfix]
    [HarmonyAfter("p1xel8ted.GraveyardKeeper.MiscBitsAndBobs", "p1xel8ted.GraveyardKeeper.WheresMaStorage")]
    [HarmonyPatch(typeof(ChestGUI), nameof(ChestGUI.Open))]
    public static void ChestGUI_Open(ref ChestGUI __instance)
    {
        //

        if (__instance == null || !_usingShippingBox) return;

        var maxItemCount = SmallMaxItemCount;

        if (UnlockedFullPrice())
        {
            maxItemCount = LargeMaxItemCount;
        }

        foreach (var item in __instance.player_panel.multi_inventory.all[0].data.inventory.Where(item => item.definition.stack_count > 1))
        {
            TryAdd(StackSizeBackups, item.id, item.definition.stack_count);

            item.definition.stack_count = maxItemCount;
        }

        foreach (var item in __instance.chest_panel.multi_inventory.all[0].data.inventory.Where(item => item.definition.stack_count > 1))
        {
            TryAdd(StackSizeBackups, item.id, item.definition.stack_count);

            item.definition.stack_count = maxItemCount;
        }

        UpdateItemStates(__instance: ref __instance);
    }

    [HarmonyPrefix]
    [HarmonyAfter("p1xel8ted.GraveyardKeeper.MiscBitsAndBobs", "p1xel8ted.GraveyardKeeper.WheresMaStorage")]
    [HarmonyPatch(typeof(InventoryPanelGUI), nameof(ChestGUI.OnItemOver), typeof(InventoryWidget), typeof(BaseItemCellGUI))]
    public static void InventoryPanelGUI_OnItemOver_Prefix(ref InventoryPanelGUI __instance, ref BaseItemCellGUI item_gui)
    {
        Thread.CurrentThread.CurrentUICulture = CrossModFields.Culture;
        //
        //if (!_usingShippingBox) return;
        if (!_cfg.showItemPriceTooltips) return;
        if (!UnlockedShippingBox()) return;
        if (__instance == null || item_gui == null) return;

        //Log($"[ItemGUI]: {item_gui.item.id}");

        if (AlreadyDone.Contains(item_gui)) return;

        if (item_gui.id_empty) return;

        if (item_gui.x1 != null && item_gui.x1.tooltip != null && item_gui.x1.tooltip.has_info)
        {
            item_gui.x1.tooltip.AddData(new BubbleWidgetSeparatorData());
            item_gui.x1.tooltip.AddData(new BubbleWidgetTextData(
                $"{strings.GerrysPrice} {Trading.FormatMoney(GetItemEarnings(item_gui.item), true)}",
                UITextStyles.TextStyle.TinyDescription, NGUIText.Alignment.Left));
            AlreadyDone.Add(item_gui);
        }

        if (item_gui.x2 != null && item_gui.x2.tooltip != null && item_gui.x2.tooltip.has_info)
        {
            item_gui.x2.tooltip.AddData(new BubbleWidgetSeparatorData());
            item_gui.x2.tooltip.AddData(new BubbleWidgetTextData(
                $"{strings.GerrysPrice} {Trading.FormatMoney(GetItemEarnings(item_gui.item), true)}",
                UITextStyles.TextStyle.TinyDescription, NGUIText.Alignment.Left));
            AlreadyDone.Add(item_gui);
        }
    }

    [HarmonyFinalizer]
    [HarmonyPatch(typeof(InventoryPanelGUI), nameof(ChestGUI.OnItemOver), typeof(InventoryWidget), typeof(BaseItemCellGUI))]
    private static Exception InventoryPanelGUI_OnItemOver_Finalizer()
    {
        return null;
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(EnvironmentEngine), nameof(EnvironmentEngine.OnEndOfDay))]
    public static void EnvironmentEngine_OnEndOfDay()
    {
        //
        if (!UnlockedShippingBox()) return;
        Thread.CurrentThread.CurrentUICulture = CrossModFields.Culture;
        if (_internalCfg.shippingBoxBuilt && _shippingBox != null)
        {
            foreach (var item in _shippingBox.data.inventory)
            {
                for (var i = 0; i < item.value; i++)
                {
                    item.OnTraded();
                }
            }

            var earnings = GetBoxEarnings(_shippingBox);

            if (earnings > 0)
            {
                Stats.PlayerAddMoney(earnings, strings.Header);
            }

            MainGame.me.player.data.money += earnings;
            var money = Trading.FormatMoney(earnings, true);

            Vector3 position;
            float time;
            if (_cfg.showSoldMessagesOnPlayer)
            {
                position = MainGame.me.player_pos;
                position.y += 125f;
                time = 4f;
            }
            else
            {
                position = _shippingBox.pos3;
                position.y += 100f;
                time = 7f;
            }

            if (_cfg.enableGerry)
            {
                StartGerryRoutine(earnings);
            }
            else
            {
                Sounds.PlaySound("coins_sound", position, true);
                _shippingBox.data.inventory.Clear();

                if (_cfg.disableSoldMessageWhenNoSale) return;

                EffectBubblesManager.ShowImmediately(position, $"{money}",
                    earnings > 0 ? EffectBubblesManager.BubbleColor.Green : EffectBubblesManager.BubbleColor.Red,
                    true, time);
            }

            if (_cfg.showSummary && !_cfg.enableGerry && earnings > 0)
            {
                ShowSummary(money);
            }
        }
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameBalance),nameof(GameBalance.LoadGameBalance))]
    [HarmonyBefore("p1xel8ted.GraveyardKeeper.QueueEverything")]
    public static void GameBalance_LoadGameBalance()
    {
        if (GameBalance.me.craft_data.Exists(a => a == _newItem)) return;

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

        _newItem = newCd;

        GameBalance.me.craft_data.Add(_newItem);
        GameBalance.me.craft_obj_data.Add(_newItem);
        GameBalance.me.AddDataUniversal(_newItem);
        GameBalance.me.AddData(_newItem);
    }


    [HarmonyPostfix]
    [HarmonyAfter("p1xel8ted.GraveyardKeeper.WheresMaStorage")]
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
                var vendorCount = KnownVendors.Count;
                var tier = GetTrunkTier();
                var header = vendorCount switch
                {
                    > 1 => $"{strings.Header} (T{tier}) - {vendorCount} {strings.Vendors}",
                    1 => $"{strings.Header} (T{tier}) - {vendorCount} {strings.Vendor}",
                    _ => $"{strings.Header} (T{tier})"
                };
                inventoryWidget.header_label.text = _cfg.showKnownVendorCount ? header : $"{strings.Header} (T{tier})";
                inventoryWidget.dont_show_empty_rows = true;
                inventoryWidget.SetInactiveStateToEmptyCells();
            }
        }
    }


    [HarmonyPostfix]
    [HarmonyAfter("p1xel8ted.GraveyardKeeper.WheresMaStorage")]
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
                var vendorCount = KnownVendors.Count;
                var tier = GetTrunkTier();
                var header = vendorCount switch
                {
                    > 1 => $"{strings.Header} (T{tier}) - {vendorCount} {strings.Vendors}",
                    1 => $"{strings.Header} (T{tier}) - {vendorCount} {strings.Vendor}",
                    _ => $"{strings.Header} (T{tier})"
                };
                inventoryWidget.header_label.text = _cfg.showKnownVendorCount ? header : $"{strings.Header} (T{tier})";
                inventoryWidget.dont_show_empty_rows = true;
                inventoryWidget.SetInactiveStateToEmptyCells();
            }


            var money = GetBoxEarnings(_shippingBox);
            __instance.money_label.text = Trading.FormatMoney(money, true);
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(MainGame), nameof(MainGame.Update))]
    public static void MainGame_Update()
    {
        if (!MainGame.game_started) return;

        _techCount = MainGame.me.save.unlocked_techs.Count;
        if (_techCount > _oldTechCount)
        {
            _oldTechCount = _techCount;
            CheckShippingBox();
        }

        if (_internalCfg.showIntroMessage)
        {
            ShowIntroMessage();
            _internalCfg.showIntroMessage = false;
            UpdateInternalConfig();
        }

        if (!UnlockedShippingBox()) return;
        var sbCraft = GameBalance.me.GetData<ObjectCraftDefinition>(ShippingBoxId);
        if (_internalCfg.shippingBoxBuilt && _shippingBox == null)
        {
            _shippingBox = Object.FindObjectsOfType<WorldGameObject>(true)
                .FirstOrDefault(x => string.Equals(x.custom_tag, ShippingBoxTag));
            if (_shippingBox == null)
            {
                Log("No Shipping Box Found!");
                _internalCfg.shippingBoxBuilt = false;
                sbCraft.hidden = false;
            }
            else
            {
                Log($"Found Shipping Box at {_shippingBox.pos3}");
                _internalCfg.shippingBoxBuilt = true;
                _shippingBox.data.drop_zone_id = ShippingBoxTag;

                var invSize = SmallInvSize;
                if (UnlockedShippingBoxExpansion())
                {
                    invSize = LargeInvSize;
                }

                _shippingBox.data.SetInventorySize(invSize);


                sbCraft.hidden = true;
            }

            UpdateInternalConfig();
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
                component.AddData(new BubbleWidgetTextData(strings.Stage1Des, UITextStyles.TextStyle.TinyDescription, NGUIText.Alignment.Left));
            }

            if (__instance.tech_id.ToLowerInvariant().Contains("Engineer".ToLowerInvariant()))
            {
                component.AddData(new BubbleWidgetSeparatorData());
                component.AddData(new BubbleWidgetTextData(strings.Stage2Header, UITextStyles.TextStyle.HintTitle));
                component.AddData(new BubbleWidgetTextData(strings.Stage2Des, UITextStyles.TextStyle.TinyDescription, NGUIText.Alignment.Left));
            }

            if (__instance.tech_id.ToLowerInvariant().Contains("Best friend".ToLowerInvariant()))
            {
                component.AddData(new BubbleWidgetSeparatorData());
                component.AddData(new BubbleWidgetTextData(strings.Stage3Header, UITextStyles.TextStyle.HintTitle));
                component.AddData(new BubbleWidgetTextData(strings.Stage3Des, UITextStyles.TextStyle.TinyDescription, NGUIText.Alignment.Left));
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
                tooltip.AddData(new BubbleWidgetTextData(strings.Stage1Header, UITextStyles.TextStyle.HintTitle, NGUIText.Alignment.Left));
                tooltip.AddData(new BubbleWidgetTextData(strings.Stage1Des, UITextStyles.TextStyle.TinyDescription, NGUIText.Alignment.Left));
            }

            if (name.ToLowerInvariant().Contains("Engineer".ToLowerInvariant()))
            {
                tooltip.AddData(new BubbleWidgetBlankSeparatorData());
                tooltip.AddData(new BubbleWidgetTextData(strings.Stage2Header, UITextStyles.TextStyle.HintTitle, NGUIText.Alignment.Left));
                tooltip.AddData(new BubbleWidgetTextData(strings.Stage2Des, UITextStyles.TextStyle.TinyDescription, NGUIText.Alignment.Left));
            }

            if (name.ToLowerInvariant().Contains("Jeweler".ToLowerInvariant()))
            {
                tooltip.AddData(new BubbleWidgetBlankSeparatorData());
                tooltip.AddData(new BubbleWidgetTextData(strings.Stage3Header, UITextStyles.TextStyle.HintTitle, NGUIText.Alignment.Left));
                tooltip.AddData(new BubbleWidgetTextData(strings.Stage3Des, UITextStyles.TextStyle.TinyDescription, NGUIText.Alignment.Left));
            }
        }
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(Vendor), nameof(Vendor.CanTradeItem))]
    public static void Vendor_CanTradeItem(ref Vendor __instance, ref bool __result)
    {
        //
        if (!UnlockedShippingBox()) return;
        if (__instance == null || _myVendor == null || _myVendor.vendor == null) return;
        if (__instance.Equals(_myVendor.vendor))
        {
            __result = true;
        }
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(WorldGameObject), nameof(WorldGameObject.DestroyMe))]
    public static void WorldGameObject_DestroyMe(ref WorldGameObject __instance)
    {
        //
        if (!UnlockedShippingBox()) return;
        if (__instance == null) return;
        if (string.Equals(__instance.custom_tag, ShippingBoxTag))
        {
            Log($"Removed Shipping Box!");
            _shippingBox = null;
            _internalCfg.shippingBoxBuilt = false;
            var sbCraft = GameBalance.me.GetData<ObjectCraftDefinition>(ShippingBoxId);
            sbCraft.hidden = false;

            UpdateInternalConfig();
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(WorldGameObject), nameof(WorldGameObject.Interact))]
    public static void WorldGameObject_Interact(ref WorldGameObject __instance, ref WorldGameObject other_obj)
    {
        //
        if (!UnlockedShippingBox()) return;
        if (__instance == null) return;
        _interactedObject = __instance;
        if (string.Equals(__instance.custom_tag, ShippingBoxTag))
        {
            Log($"Found Shipping Box! {__instance.data.drop_zone_id}, Other: {other_obj.obj_id}");
            _internalCfg.shippingBoxBuilt = true;
            UpdateInternalConfig();
            _usingShippingBox = true;
            __instance.data.drop_zone_id = ShippingBoxTag;
            __instance.custom_tag = ShippingBoxTag;
            var invSize = SmallInvSize;
            if (UnlockedShippingBoxExpansion())
            {
                invSize = LargeInvSize;
            }

            __instance.data.SetInventorySize(invSize);
            __instance.data.money = GetBoxEarnings(__instance);
            _shippingBox = __instance;
        }
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
        //
        if (!UnlockedShippingBox()) return;
        if (__instance == null) return;
        if (_internalCfg.shippingBoxBuilt && _shippingBox != null) return;
        if (string.Equals(new_obj_id, "mf_box_stuff") && _shippingBuild)
        {
            Log($"Built Shipping Box!");
            var sbCraft = GameBalance.me.GetData<ObjectCraftDefinition>(ShippingBoxId);
            sbCraft.hidden = true;
            __instance.custom_tag = ShippingBoxTag;

            _shippingBuild = false;
            var invSize = SmallInvSize;
            if (UnlockedShippingBoxExpansion())
            {
                invSize = LargeInvSize;
            }

            __instance.data.SetInventorySize(invSize);
            __instance.data.drop_zone_id = ShippingBoxTag;
            _shippingBox = __instance;

            _internalCfg.shippingBoxBuilt = true;
            UpdateInternalConfig();
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(WorldMap), nameof(WorldMap.RescanWGOsList))]
    public static void WorldMap_RescanWGOsList()
    {
        //
        if (CrossModFields.WorldNpcs == null) return;
        foreach (var npc in CrossModFields.WorldNpcs.Where(npc => npc.vendor != null))
        {
            var known =
                MainGame.me.save.known_npcs.npcs.Exists(a => string.Equals(a.npc_id, npc.vendor.id));
            if (known)
            {
                KnownVendors.Add(npc);
            }
        }
    }
    
    //hooks into the time of day update and saves if the K key was pressed
    [HarmonyPrefix]
    [HarmonyPatch(typeof(TimeOfDay), nameof(TimeOfDay.Update))]
    public static void TimeOfDay_Update()
    {
        if (Input.GetKeyUp(_cfg.reloadConfigKeyBind))
        {
            _cfg = Config.GetOptions();

            if (Time.time - CrossModFields.ConfigReloadTime > CrossModFields.ConfigReloadTimeLength)
            {
                Tools.ShowMessage(GetLocalizedString(strings.ConfigMessage), Vector3.zero);
                CrossModFields.ConfigReloadTime = Time.time;
            }
        }
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(WorldZone), nameof(WorldZone.GetZoneWGOs))]
    public static void WorldZone_GetZoneWGOs(ref List<WorldGameObject> __result)
    {
        if (__result == null) return;
        if (_interactedObject != null && _interactedObject.obj_def.interaction_type == ObjectDefinition.InteractionType.Builder) return;

        var count = __result.RemoveAll(a => a.custom_tag == ShippingBoxTag || a.data.drop_zone_id == ShippingBoxTag);
        if (count > 0)
        {
            Log($"[WorldZone.GetZoneWGOs] Removed Shipping Box From WorldMap Objects");
        }

        _interactedObject = null;
    }
}