namespace GerrysJunkTrunk;

[Harmony]
public static class Patches
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(MainGame), nameof(MainGame.Update))]
    public static void MainGame_Update()
    {
        if (!MainGame.game_started) return;

        // Cinematic watchdog. The 20s GJTimer safety-net inside StartGerryRoutine can be
        // destroyed by scene unloads, paused indefinitely by Time.timeScale=0, or skipped
        // entirely if a Say callback never fires. If _cinematicPlaying gets stranded true,
        // HUD/control stay disabled forever (or until fast-travel coincidentally re-enables
        // them via the load sequence). Force a recovery once we exceed the routine's
        // expected upper bound.
        if (Plugin._cinematicPlaying && Time.time - Plugin._cinematicStartedAt > Plugin.CinematicMaxDurationSeconds)
        {
            if (Plugin.DebugEnabled) Plugin.WriteLog($"[Watchdog] cinematic exceeded {Plugin.CinematicMaxDurationSeconds}s — forcing HideCinematic()");
            Plugin.HideCinematic();
        }

        Plugin._techCount = MainGame.me.save.unlocked_techs.Count;
        if (Plugin._techCount > Plugin._oldTechCount)
        {
            Plugin._oldTechCount = Plugin._techCount;
            Plugin.CheckShippingBox();
        }

        if (Plugin.InternalShowIntroMessage.Value)
        {
            Plugin.ShowIntroMessage();
            Plugin.InternalShowIntroMessage.Value = false;
        }

        if (!Plugin.UnlockedShippingBox()) return;
        var sbCraft = GameBalance.me.GetData<ObjectCraftDefinition>(Plugin.ShippingBoxId);
        if (Plugin.InternalShippingBoxBuilt.Value && Plugin._shippingBox == null)
        {
            Plugin._shippingBox = UnityEngine.Object.FindObjectsOfType<WorldGameObject>(true)
                .FirstOrDefault(x => string.Equals(x.custom_tag, Plugin.ShippingBoxTag));
            if (Plugin._shippingBox == null)
            {
                if (Plugin.DebugEnabled)
                {
                    Plugin.Log.LogInfo("Update: No Shipping Box Found!");
                }
                Plugin.InternalShippingBoxBuilt.Value = false;
                sbCraft.hidden = false;
            }
            else
            {
                if (Plugin.DebugEnabled)
                {
                    Plugin.Log.LogInfo($"Update: Found Shipping Box at {Plugin._shippingBox.pos3}");
                }
                Plugin.InternalShippingBoxBuilt.Value = true;
                Plugin._shippingBox.data.drop_zone_id = Plugin.ShippingBoxTag;

                var invSize = Plugin.SmallInvSize;
                if (Plugin.UnlockedShippingBoxExpansion())
                {
                    invSize = Plugin.LargeInvSize;
                }

                Plugin._shippingBox.data.SetInventorySize(invSize);

                sbCraft.hidden = true;
            }
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(BuildModeLogics), nameof(BuildModeLogics.CanBuild))]
    public static void BuildModeLogics_CanBuild(ref bool __result, CraftDefinition cd)
    {
        if (Plugin.InternalShippingBoxBuilt.Value && Plugin._shippingBox != null)
        {
            if (cd.id.Contains(Plugin.ShippingItem))
            {
                __result = false;
            }
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(InventoryPanelGUI), nameof(InventoryPanelGUI.Open))]
    public static void InventoryPanelGUI_Open()
    {
        Plugin.AlreadyDone.Clear();
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(BuildModeLogics), nameof(BuildModeLogics.OnBuildCraftSelected))]
    public static void BuildModeLogics_OnBuildCraftSelected(ref ObjectCraftDefinition cd)
    {
        if (cd.id.Contains(Plugin.ShippingItem))
        {
            Plugin._shippingBuild = true;
            var ocd = GameBalance.me.GetData<ObjectCraftDefinition>("mf_wood_builddesk:p:mf_box_stuff_place");
            cd = ocd;
        }
    }

    // Tag the floating placeholder's custom_tag directly so the marker travels with the
    // wobj through placement (StopCurrentFloating keeps the gameObject alive) and craft
    // completion (ReplaceWithObject changes obj_id but leaves custom_tag untouched). If
    // the player cancels without placing, StopCurrentFloating destroys the gameObject and
    // the tag disappears with it — no leaked flag to falsely tag the next wooden storage.
    [HarmonyPostfix]
    [HarmonyPatch(typeof(BuildModeLogics), nameof(BuildModeLogics.OnBuildCraftSelected))]
    public static void BuildModeLogics_OnBuildCraftSelected_Postfix()
    {
        if (!Plugin._shippingBuild) return;
        Plugin._shippingBuild = false;

        var floating = FloatingWorldGameObject.cur_floating;
        if (floating == null || floating.wobj == null) return;

        floating.wobj.custom_tag = Plugin.ShippingBoxTag;
        if (Plugin.DebugEnabled) Plugin.WriteLog($"[OnBuildCraftSelected.Postfix] tagged floating shipping box placeholder");
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(ChestGUI), nameof(ChestGUI.Hide))]
    public static void ChestGUI_Hide(ChestGUI __instance)
    {
        Plugin.ClearGerryFlag(__instance);
    }

    [HarmonyPostfix]
    [HarmonyAfter("p1xel8ted.gyk.miscbitsandbobs", "p1xel8ted.gyk.wheresmastorage")]
    [HarmonyPatch(typeof(ChestGUI), nameof(ChestGUI.MoveItem), typeof(Item), typeof(int), typeof(bool), typeof(Item),
        typeof(bool))]
    public static void ChestGUI_MoveItem(ChestGUI __instance)
    {
        if (__instance == null || !Plugin._usingShippingBox) return;

        Plugin.UpdateItemStates(__instance);
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(ChestGUI), nameof(ChestGUI.OnPressedBack))]
    public static void ChestGUI_OnPressedBack(ChestGUI __instance)
    {
        Plugin.ClearGerryFlag(__instance);
    }

    [HarmonyPostfix]
    [HarmonyAfter("p1xel8ted.gyk.miscbitsandbobs", "p1xel8ted.gyk.wheresmastorage")]
    [HarmonyPatch(typeof(ChestGUI), nameof(ChestGUI.Open))]
    public static void ChestGUI_Open(ChestGUI __instance)
    {
        if (__instance == null || !Plugin._usingShippingBox) return;

        var maxItemCount = Plugin.UnlockedFullPrice() ? Plugin.LargeMaxItemCount : Plugin.SmallMaxItemCount;

        foreach (var panel in new[] { __instance.player_panel, __instance.chest_panel })
        {
            foreach (var item in panel.multi_inventory.all[0].data.inventory
                         .Where(item => item.definition.stack_count > 1))
            {
                Plugin.TryAdd(Plugin.StackSizeBackups, item.id, item.definition.stack_count);
                item.definition.stack_count = maxItemCount;
            }
        }

        Plugin.UpdateItemStates(__instance);
    }

    [HarmonyPrefix]
    [HarmonyAfter("p1xel8ted.gyk.miscbitsandbobs", "p1xel8ted.gyk.wheresmastorage")]
    [HarmonyPatch(typeof(InventoryPanelGUI), nameof(InventoryPanelGUI.OnItemOver), typeof(InventoryWidget),
        typeof(BaseItemCellGUI))]
    public static void InventoryPanelGUI_OnItemOver_Prefix(InventoryPanelGUI __instance,
        BaseItemCellGUI item_gui)
    {
        Lang.Reload();

        if (!Plugin.ShowItemPriceTooltips.Value || !Plugin.UnlockedShippingBox() || __instance == null || item_gui == null ||
            Plugin.AlreadyDone.Contains(item_gui) || item_gui.id_empty)
            return;

        foreach (var tooltip in new[] { item_gui.x1, item_gui.x2 })
        {
            if (tooltip != null && tooltip.tooltip != null && tooltip.tooltip.has_info)
            {
                var itemEarnings = Plugin.GetItemEarnings(item_gui.item);
                tooltip.tooltip.AddData(new BubbleWidgetSeparatorData());

                tooltip.tooltip.AddData(new BubbleWidgetTextData(
                    Lang.Get("GerrysPrice"),
                    UITextStyles.TextStyle.Usual, NGUIText.Alignment.Left));
                tooltip.tooltip.AddData(new BubbleWidgetSeparatorData());
                tooltip.tooltip.AddData(new BubbleWidgetTextData(
                    $"{Trading.FormatMoney(itemEarnings, true)}",
                    UITextStyles.TextStyle.TinyDescription, NGUIText.Alignment.Left));
            }
        }

        Plugin.AlreadyDone.Add(item_gui);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(EnvironmentEngine), nameof(EnvironmentEngine.OnEndOfDay))]
    private static void EnvironmentEngine_OnEndOfDay()
    {
        Lang.Reload();

        if (!Plugin.UnlockedShippingBox() || !Plugin.InternalShippingBoxBuilt.Value || Plugin._shippingBox == null) return;

        var earnings = Plugin.GetBoxEarnings(Plugin._shippingBox);

        MainGame.me.player.data.money += earnings;
        Plugin._shippingBox.data.inventory.Clear();
        var money = Trading.FormatMoney(earnings, true);

        if (Plugin.EnableGerry.Value && !GUIElements.me.dialog.is_shown && !EnvironmentEngine.me.IsTimeStopped())
        {
            Plugin.StartGerryRoutine(earnings);
        }
        else
        {
            Plugin.PlayCoinsSoundAndShowMessage(earnings, money);
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameBalance), nameof(GameBalance.LoadGameBalance))]
    private static void GameBalance_LoadGameBalance()
    {
        if (GameBalance.me.craft_data.Exists(a => a == Plugin.NewItem)) return;

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
        newCd.custom_name = Lang.Get("Header");
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
        newCd.id = Plugin.ShippingBoxId;

        Plugin.SetNewItem(newCd);

        GameBalance.me.craft_data.Add(Plugin.NewItem);
        GameBalance.me.craft_obj_data.Add(Plugin.NewItem);
        GameBalance.me.AddDataUniversal(Plugin.NewItem);
        GameBalance.me.AddData(Plugin.NewItem);
    }

    [HarmonyPostfix]
    [HarmonyAfter("p1xel8ted.gyk.wheresmastorage")]
    [HarmonyPatch(typeof(InventoryPanelGUI), nameof(InventoryPanelGUI.DoOpening))]
    public static void InventoryPanelGUI_DoOpening(InventoryPanelGUI __instance)
    {
        if (!Plugin.UnlockedShippingBox()) return;
        if (__instance == null) return;
        Lang.Reload();
        var isChest = __instance.name.ToLowerInvariant().Contains("chest");
        var isPlayer = __instance.name.ToLowerInvariant().Contains("player");
        if (Plugin._usingShippingBox && isChest && !isPlayer)
        {
            foreach (var inventoryWidget in __instance._widgets)
            {
                var tier = Plugin.GetTrunkTier();
                inventoryWidget.header_label.text = $"{Lang.Get("Header")} (T{tier})";
                inventoryWidget.dont_show_empty_rows = true;
                inventoryWidget.SetInactiveStateToEmptyCells();
            }
        }
    }

    [HarmonyPostfix]
    [HarmonyAfter("p1xel8ted.gyk.wheresmastorage")]
    [HarmonyPatch(typeof(InventoryPanelGUI), nameof(InventoryPanelGUI.Redraw))]
    public static void InventoryPanelGUI_Redraw(InventoryPanelGUI __instance)
    {
        if (!Plugin.UnlockedShippingBox()) return;
        if (__instance == null) return;
        Lang.Reload();
        var isChest = __instance.name.ToLowerInvariant().Contains("chest");
        var isPlayer = __instance.name.ToLowerInvariant().Contains("player");
        if (Plugin._usingShippingBox && isChest && !isPlayer)
        {
            foreach (var inventoryWidget in __instance._widgets)
            {
                var tier = Plugin.GetTrunkTier();
                inventoryWidget.header_label.text = $"{Lang.Get("Header")} (T{tier})";
                inventoryWidget.dont_show_empty_rows = true;
                inventoryWidget.SetInactiveStateToEmptyCells();
            }

            var money = Plugin.GetBoxEarnings(Plugin._shippingBox);
            __instance.money_label.text = Trading.FormatMoney(money, true);
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(TechTreeGUIItem), nameof(TechTreeGUIItem.InitGamepadTooltip))]
    public static void TechTreeGUIItem_InitGamepadTooltip(TechTreeGUIItem __instance)
    {
        if (__instance == null) return;

        Lang.Reload();
        var component = __instance.GetComponent<Tooltip>();
        if (__instance.tech_id.ToLowerInvariant().Contains("Wood processing".ToLowerInvariant()))
        {
            component.AddData(new BubbleWidgetSeparatorData());
            component.AddData(new BubbleWidgetTextData(Lang.Get("Stage1Header"), UITextStyles.TextStyle.HintTitle));
            component.AddData(new BubbleWidgetTextData(Lang.Get("Stage1Des"), UITextStyles.TextStyle.TinyDescription,
                NGUIText.Alignment.Left));
        }

        if (__instance.tech_id.ToLowerInvariant().Contains("Engineer".ToLowerInvariant()))
        {
            component.AddData(new BubbleWidgetSeparatorData());
            component.AddData(new BubbleWidgetTextData(Lang.Get("Stage2Header"), UITextStyles.TextStyle.HintTitle));
            component.AddData(new BubbleWidgetTextData(Lang.Get("Stage2Des"), UITextStyles.TextStyle.TinyDescription,
                NGUIText.Alignment.Left));
        }

        if (__instance.tech_id.ToLowerInvariant().Contains("Best friend".ToLowerInvariant()))
        {
            component.AddData(new BubbleWidgetSeparatorData());
            component.AddData(new BubbleWidgetTextData(Lang.Get("Stage3Header"), UITextStyles.TextStyle.HintTitle));
            component.AddData(new BubbleWidgetTextData(Lang.Get("Stage3Des"), UITextStyles.TextStyle.TinyDescription,
                NGUIText.Alignment.Left));
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(TechUnlock), nameof(TechUnlock.GetTooltip), typeof(Tooltip))]
    public static void TechUnlock_GetTooltip(TechUnlock __instance, Tooltip tooltip)
    {
        if (__instance == null) return;

        Lang.Reload();
        if (LazyInput.gamepad_active) return;
        var name = __instance.GetData().name;

        if (name.ToLowerInvariant().Contains("Wooden plank".ToLowerInvariant()))
        {
            tooltip.AddData(new BubbleWidgetBlankSeparatorData());
            tooltip.AddData(new BubbleWidgetTextData(Lang.Get("Stage1Header"), UITextStyles.TextStyle.HintTitle,
                NGUIText.Alignment.Left));
            tooltip.AddData(new BubbleWidgetTextData(Lang.Get("Stage1Des"), UITextStyles.TextStyle.TinyDescription,
                NGUIText.Alignment.Left));
        }

        if (name.ToLowerInvariant().Contains("Engineer".ToLowerInvariant()))
        {
            tooltip.AddData(new BubbleWidgetBlankSeparatorData());
            tooltip.AddData(new BubbleWidgetTextData(Lang.Get("Stage2Header"), UITextStyles.TextStyle.HintTitle,
                NGUIText.Alignment.Left));
            tooltip.AddData(new BubbleWidgetTextData(Lang.Get("Stage2Des"), UITextStyles.TextStyle.TinyDescription,
                NGUIText.Alignment.Left));
        }

        if (name.ToLowerInvariant().Contains("Jeweler".ToLowerInvariant()))
        {
            tooltip.AddData(new BubbleWidgetBlankSeparatorData());
            tooltip.AddData(new BubbleWidgetTextData(Lang.Get("Stage3Header"), UITextStyles.TextStyle.HintTitle,
                NGUIText.Alignment.Left));
            tooltip.AddData(new BubbleWidgetTextData(Lang.Get("Stage3Des"), UITextStyles.TextStyle.TinyDescription,
                NGUIText.Alignment.Left));
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(WorldGameObject), nameof(WorldGameObject.DestroyMe))]
    public static void WorldGameObject_DestroyMe(WorldGameObject __instance)
    {
        if (!Plugin.UnlockedShippingBox()) return;
        if (__instance == null) return;
        if (string.Equals(__instance.custom_tag, Plugin.ShippingBoxTag))
        {
            if (Plugin.DebugEnabled) Plugin.WriteLog($"[DestroyMe] shipping box removed at {__instance.pos3}");
            Plugin._shippingBox = null;
            Plugin.InternalShippingBoxBuilt.Value = false;
            var sbCraft = GameBalance.me.GetData<ObjectCraftDefinition>(Plugin.ShippingBoxId);
            sbCraft.hidden = false;
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(WorldGameObject), nameof(WorldGameObject.Interact))]
    private static void WorldGameObject_Interact(WorldGameObject __instance, WorldGameObject other_obj)
    {
        if (!Plugin.UnlockedShippingBox() || __instance == null ||
            !string.Equals(__instance.custom_tag, Plugin.ShippingBoxTag)) return;

        if (Plugin.DebugEnabled) Plugin.WriteLog($"[Interact] shipping box interaction: drop_zone={__instance.data.drop_zone_id}, other={other_obj.obj_id}");

        Plugin._interactedObject = __instance;
        Plugin._usingShippingBox = true;
        __instance.data.drop_zone_id = Plugin.ShippingBoxTag;
        __instance.custom_tag = Plugin.ShippingBoxTag;

        var sbCraft = GameBalance.me.GetData<ObjectCraftDefinition>(Plugin.ShippingBoxId);
        sbCraft.hidden = true;

        var invSize = Plugin.UnlockedShippingBoxExpansion() ? Plugin.LargeInvSize : Plugin.SmallInvSize;
        __instance.data.SetInventorySize(invSize);
        __instance.data.money = Plugin.GetBoxEarnings(__instance);

        Plugin._shippingBox = __instance;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(WorldGameObject), nameof(WorldGameObject.ReplaceWithObject))]
    public static void WorldGameObject_ReplaceWithObject(WorldGameObject __instance, string new_obj_id)
    {
        if (!Plugin.UnlockedShippingBox() || __instance == null ||
            Plugin.InternalShippingBoxBuilt.Value && Plugin._shippingBox != null) return;

        if (string.Equals(new_obj_id, "mf_box_stuff") && string.Equals(__instance.custom_tag, Plugin.ShippingBoxTag))
        {
            if (Plugin.DebugEnabled) Plugin.WriteLog($"[ReplaceWithObject] shipping box built at {__instance.pos3}");
            var sbCraft = GameBalance.me.GetData<ObjectCraftDefinition>(Plugin.ShippingBoxId);
            sbCraft.hidden = true;

            Plugin._shippingBox = __instance;
            Plugin.InternalShippingBoxBuilt.Value = true;

            Plugin.UpdateShippingBox(sbCraft, __instance);
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(WorldZone), nameof(WorldZone.GetZoneWGOs))]
    public static void WorldZone_GetZoneWGOs(List<WorldGameObject> __result)
    {
        if (__result == null) return;
        if (Plugin._interactedObject != null &&
            Plugin._interactedObject.obj_def.interaction_type == ObjectDefinition.InteractionType.Builder) return;

        var count = __result.RemoveAll(a => a.custom_tag == Plugin.ShippingBoxTag || a.data.drop_zone_id == Plugin.ShippingBoxTag);
        if (count > 0)
        {
            var sbCraft = GameBalance.me.GetData<ObjectCraftDefinition>(Plugin.ShippingBoxId);
            sbCraft.hidden = true;

            if (Plugin.DebugEnabled) Plugin.WriteLog($"[WorldZone.GetZoneWGOs] Removed Shipping Box From WorldMap Objects");
        }

        Plugin._interactedObject = null;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameSave), nameof(GameSave.GlobalEventsCheck))]
    private static void FindJunkTrunk()
    {
        // _cinematicPlaying is a static; if a previous play session ended (save-load) mid-
        // cinematic the flag persists and the next ShowCinematic returns early forever.
        // Force a recovery on every load so HUD/control are guaranteed restored.
        if (Plugin._cinematicPlaying)
        {
            if (Plugin.DebugEnabled) Plugin.WriteLog("[FindJunkTrunk] _cinematicPlaying=true on load → forcing HideCinematic()");
            Plugin.HideCinematic();
        }

        var trunks = WorldMap.GetWorldGameObjectsByCustomTag(Plugin.ShippingBoxTag);
        if (trunks.Count > 0)
        {
            Plugin._shippingBox = trunks[0];
            Plugin.InternalShippingBoxBuilt.Value = true;
            var sbCraft = GameBalance.me.GetData<ObjectCraftDefinition>(Plugin.ShippingBoxId);
            sbCraft.hidden = true;

            if (Plugin.DebugEnabled) Plugin.WriteLog($"[FindJunkTrunk] found existing shipping box at {Plugin._shippingBox.pos3} on save load");
        }
        else if (Plugin.DebugEnabled)
        {
            Plugin.WriteLog("[FindJunkTrunk] no shipping box in save");
        }
    }
}
