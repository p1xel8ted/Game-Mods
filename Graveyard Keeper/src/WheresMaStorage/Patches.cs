using Debug = UnityEngine.Debug;

namespace WheresMaStorage;

[Harmony]
[HarmonyPriority(0)]
public static class Patches
{
    // Replaces Actions.GameStartedPlaying += Helpers.RunWmsTasks
    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameSave), nameof(GameSave.GlobalEventsCheck))]
    public static void GameSave_GlobalEventsCheck()
    {
        Helpers.RunWmsTasks();
    }

    // Replaces Actions.GameBalanceLoad += Helpers.GameBalanceLoad
    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameBalance), nameof(GameBalance.LoadGameBalance))]
    public static void GameBalance_LoadGameBalance()
    {
        Helpers.GameBalanceLoad();
    }

    // Replaces GYKHelper Actions.WorldGameObject_Interact — sets interaction state flags
    [HarmonyPrefix]
    [HarmonyPatch(typeof(WorldGameObject), nameof(WorldGameObject.Interact), typeof(WorldGameObject), typeof(bool), typeof(float))]
    public static void WorldGameObject_Interact_Prefix(WorldGameObject __instance, WorldGameObject other_obj)
    {
        if (!MainGame.game_started || __instance == null) return;

        Fields.CurrentWgoInteraction = __instance;
        Fields.IsVendor = __instance.vendor != null;
        Fields.IsCraft = other_obj.is_player && __instance.obj_def.interaction_type != ObjectDefinition.InteractionType.Chest && __instance.obj_def.has_craft;
        Fields.IsChest = __instance.obj_def.interaction_type == ObjectDefinition.InteractionType.Chest;
        Fields.IsBarman = __instance.obj_id.ToLowerInvariant().Contains("barman");
        Fields.IsTavernCellarRack = __instance.obj_id.ToLowerInvariant().Contains("tavern_cellar_rack");
        Fields.IsWritersTable = __instance.obj_id.ToLowerInvariant().Contains("writer");
        Fields.IsSoulBox = __instance.obj_id.ToLowerInvariant().Contains("soul_container");
        Fields.IsChurchPulpit = __instance.obj_id.ToLowerInvariant().Contains("pulpit");

        if (Plugin.DebugEnabled)
        {
            Helpers.Log($"[Interact] obj={__instance.obj_id} zone={__instance.GetMyWorldZoneId()} " +
                        $"type={__instance.obj_def.interaction_type} " +
                        $"flags={{Vendor={Fields.IsVendor},Craft={Fields.IsCraft},Chest={Fields.IsChest}," +
                        $"Barman={Fields.IsBarman},TavernCellar={Fields.IsTavernCellarRack},Writer={Fields.IsWritersTable}," +
                        $"SoulBox={Fields.IsSoulBox},Pulpit={Fields.IsChurchPulpit}}}");
        }

        if (__instance.obj_def.inventory_size > 0)
        {
            __instance.data.sub_name = __instance.obj_id.Length <= 0
                ? "Unknown#" + __instance.GetMyWorldZoneId()
                : __instance.obj_id + "#" + __instance.GetMyWorldZoneId();
        }
    }

    // Replaces GYKHelper Patches.BaseGuiHidePostfix — resets interaction flags when all GUIs close
    [HarmonyPostfix]
    [HarmonyPatch(typeof(BaseGUI), nameof(BaseGUI.Hide), typeof(bool))]
    public static void BaseGUI_Hide_Postfix()
    {
        if (BaseGUI.all_guis_closed)
        {
            if (Plugin.DebugEnabled) Helpers.Log("[ResetFlags] BaseGUI.Hide (all GUIs closed) → resetting interaction flags");
            Helpers.ResetInteractionFlags();
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Debug), nameof(Debug.Log), typeof(object))]
    public static bool Debug_Log(ref object message)
    {
        if (message is not string msg) return true;

        return !msg.Contains("#BAG#");
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(SleepGUI), nameof(SleepGUI.Open), null)]
    public static void SleepGUI_Open()
    {
        Fields.InventoriesLoaded = false;
        if (Plugin.DebugEnabled) Helpers.Log("[ResetFlags] SleepGUI.Open → InventoriesLoaded=false");
    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(WaitingGUI), nameof(WaitingGUI.Open), null)]
    public static void WaitingGUI_Open()
    {
        Fields.InventoriesLoaded = false;
        if (Plugin.DebugEnabled) Helpers.Log("[ResetFlags] WaitingGUI.Open → InventoriesLoaded=false");
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(OrganEnhancerGUI), nameof(OrganEnhancerGUI.Open), null)]
    public static void OrganEnhancerGUI_Open(OrganEnhancerGUI __instance)
    {
        __instance._multi_inventory = MainGame.me.player.GetMultiInventory(exceptions: null, force_world_zone: "",
            player_mi: MultiInventory.PlayerMultiInventory.IncludePlayer, include_toolbelt: false,
            include_bags: false, sortWGOS: false);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(WorldMap), nameof(WorldMap.OnAddNewWGO), typeof(WorldGameObject))]
    public static void WorldMap_OnAddNewWGO(WorldGameObject wgo)
    {
        if (wgo.data.inventory_size > 0)
        {
            if (Plugin.DebugEnabled) Helpers.Log($"[Invalidate] OnAddNewWGO obj={wgo.obj_id} (inventory_size={wgo.data.inventory_size}) → InventoriesLoaded=false");
            Fields.InventoriesLoaded = false;
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(WorldMap), nameof(WorldMap.OnDestroyWGO), typeof(WorldGameObject))]
    public static void WorldMap_OnDestroyWGO(WorldGameObject wgo)
    {
        if (wgo.data.inventory_size > 0)
        {
            if (Plugin.DebugEnabled) Helpers.Log($"[Invalidate] OnDestroyWGO obj={wgo.obj_id} → InventoriesLoaded=false");
            Fields.InventoriesLoaded = false;
        }
    }

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
        if (!Plugin.SharedInventory.Value)
        {
            if (Plugin.DebugEnabled) Helpers.Log($"[GetMultiInventory] SharedInventory disabled, passing through for obj={__instance.obj_id}");
            return true;
        }

        var objId = __instance.obj_id;
        var objDefId = __instance.obj_def.id;
        var worldZoneId = __instance.GetMyWorldZoneId();
        var isQuarry = worldZoneId.Contains("stone_workyard") || worldZoneId.Contains("marble_deposit");
        var isWell = objId.Contains("well");
        var isZombieMill = worldZoneId.Contains("zombie_mill");

        if (Fields.AlwaysSkipInventories.Any(skipItem => objId.Contains(skipItem) || objDefId.Contains(skipItem) || worldZoneId.Contains(skipItem)))
        {
            if (Plugin.DebugEnabled) Helpers.Log($"[GetMultiInventory] skip (AlwaysSkipInventories match) obj={objId} zone={worldZoneId}");
            return true;
        }

        if (Plugin.ExcludeWellsFromSharedInventory.Value && isWell)
        {
            if (Plugin.DebugEnabled) Helpers.Log($"[GetMultiInventory] skip (well exclusion) obj={objId}");
            return true;
        }

        if (Plugin.ExcludeZombieMillFromSharedInventory.Value && isZombieMill)
        {
            if (Plugin.DebugEnabled) Helpers.Log($"[GetMultiInventory] skip (zombie mill exclusion) obj={objId} zone={worldZoneId}");
            return true;
        }

        var isZombieWorker = __instance.has_linked_worker && __instance.linked_worker.obj_id.Contains("zombie") || objDefId.Contains("zombie");
        Fields.ZombieWorker = isZombieWorker;

        if (isZombieWorker && !Plugin.AllowZombiesAccessToSharedInventory.Value)
        {
            if (Plugin.DebugEnabled) Helpers.Log($"[GetMultiInventory] skip (zombie worker, shared disallowed) obj={objId}");
            return true;
        }

        var proceed = objId.Contains("church_pulpit") ||
                      Fields.IsCraft ||
                      isZombieWorker ||
                      objId.Contains("compost") ||
                      __instance.IsWorker() ||
                      __instance.IsInvisibleWorker() ||
                      __instance.is_player ||
                      objId.StartsWith("mf_");

        if (!proceed)
        {
            if (Plugin.DebugEnabled) Helpers.Log($"[GetMultiInventory] no-proceed (no match) obj={objId} zone={worldZoneId}");
            return true;
        }

        var inv = Invents.GetMiInventory(objId, worldZoneId);
        __result = inv;

        if (Plugin.DebugEnabled) Helpers.Log($"[GetMultiInventory] injected shared multi ({inv.all.Count} inventories) for obj={objId} zone={worldZoneId}");
        return false;
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
    public static void CraftDefinition_takes_item_durability(CraftDefinition __instance, ref bool __result)
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
    [HarmonyPatch(typeof(CraftComponent), nameof(CraftComponent.CraftReally))]
    public static void CraftComponent_CraftReally(CraftDefinition craft, bool for_gratitude_points,
        ref bool start_by_player)
    {
        Fields.GratitudeCraft = for_gratitude_points;
        if (Plugin.DebugEnabled) Helpers.Log($"[CraftReally] craft={craft?.id} gratitude={for_gratitude_points}");
        if (Fields.GratitudeCraft)
        {
            start_by_player = false;
        }
    }

    private static void ResetFlags()
    {
        Fields.InventoriesLoaded = false;
        Fields.GameBalanceAlreadyRun = false;
        Fields.DropsCleaned = false;
        Fields.DebugMessageShown = false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(SaveSlotsMenuGUI), nameof(SaveSlotsMenuGUI.Open))]
    public static void SaveSlotsMenuGUI_Open()
    {
        if (Plugin.DebugEnabled) Helpers.Log("[ResetFlags] SaveSlotsMenuGUI.Open → clearing inventory/gamebalance/drops flags");
        ResetFlags();
    }

    // Some crafting objects re-acquire the inventories when starting a craft, overwriting our multi. This stops that.
    [HarmonyPostfix]
    [HarmonyPatch(typeof(BaseCraftGUI), nameof(BaseCraftGUI.multi_inventory), MethodType.Getter)]
    public static void BaseCraftGUI_multi_inventory(BaseCraftGUI __instance, ref MultiInventory __result)
    {
        if (!Plugin.SharedInventory.Value)
        {
            if (Plugin.DebugEnabled) Helpers.Log("[BaseCraftGUI] SharedInventory disabled, passing through");
            return;
        }

        var crafteryWGO = __instance.GetCrafteryWGO();
        var crafteryObjId = crafteryWGO.obj_id;
        var crafteryObjDefId = crafteryWGO.obj_def.id;
        var instanceName = __instance.name;
        var crafteryWzId = crafteryWGO.GetMyWorldZoneId();

        if (Fields.AlwaysSkipInventories.Any(a => crafteryObjId.Contains(a) || crafteryObjDefId.Contains(a) || crafteryWzId.Contains(a)))
        {
            if (Plugin.DebugEnabled) Helpers.Log($"[BaseCraftGUI] skip (AlwaysSkipInventories match) obj={crafteryObjId} zone={crafteryWzId}");
            return;
        }

        var isQuarry = crafteryWzId.Contains("stone_workyard") || crafteryWzId.Contains("marble_deposit");
        var isWell = crafteryObjId.Contains("well") || crafteryObjDefId.Contains("well");
        var isZombieMill = crafteryWzId.Contains("zombie_mill");

        var isZombie = crafteryObjId.Contains("zombie") || crafteryObjDefId.Contains("zombie");
        Fields.ZombieWorker = isZombie;

        if (Plugin.ExcludeWellsFromSharedInventory.Value && isWell)
        {
            if (Plugin.DebugEnabled) Helpers.Log($"[BaseCraftGUI] skip (well exclusion) obj={crafteryObjId}");
            return;
        }

        if (Plugin.ExcludeZombieMillFromSharedInventory.Value && isZombieMill)
        {
            if (Plugin.DebugEnabled) Helpers.Log($"[BaseCraftGUI] skip (zombie mill exclusion) obj={crafteryObjId} zone={crafteryWzId}");
            return;
        }

        if (!Plugin.AllowZombiesAccessToSharedInventory.Value && isZombie)
        {
            if (Plugin.DebugEnabled) Helpers.Log($"[BaseCraftGUI] skip (zombie, shared disallowed) obj={crafteryObjId}");
            return;
        }

        __result = Invents.GetMiInventory($"[BaseCraftGUI.multi_inventory (Getter)]: {instanceName}, Craftery: {crafteryObjId}", crafteryWGO.GetMyWorldZoneId());
        if (Plugin.DebugEnabled) Helpers.Log($"[BaseCraftGUI] injected shared multi ({__result.all.Count} inventories) panel={instanceName} obj={crafteryObjId} zone={crafteryWzId}");
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(WorldGameObject), nameof(WorldGameObject.ReplaceWithObject))]
    public static void WorldGameObject_ReplaceWithObject(WorldGameObject __instance)
    {
        if (__instance.obj_def.interaction_type is ObjectDefinition.InteractionType.Chest
                or ObjectDefinition.InteractionType.Builder or ObjectDefinition.InteractionType.Craft ||
            __instance.obj_id.StartsWith("mf_"))
        {
            if (Plugin.DebugEnabled) Helpers.Log($"[Invalidate] ReplaceWithObject obj={__instance.obj_id} type={__instance.obj_def.interaction_type} → InventoriesLoaded=false");
            Fields.InventoriesLoaded = false;
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
    public static void InventoryPanelGUI_DoOpening_Prefix(InventoryPanelGUI __instance,
        ref MultiInventory multi_inventory)
    {
        if (GUIElements.me.pray_craft.gameObject.activeSelf ||
            GUIElements.me.pray_craft.gameObject.activeInHierarchy)
        {
            if (Plugin.DebugEnabled) Helpers.Log($"[DoOpening:Prefix] skip (pray_craft active) panel={__instance.name}");
            return;
        }

        if (__instance.GetComponentInParent<VendorGUI>() != null)
        {
            if (Plugin.DebugEnabled) Helpers.Log($"[DoOpening:Prefix] skip (VendorGUI parent detected) panel={__instance.name}");
            return;
        }

        if (__instance.name.ToLowerInvariant().Contains(Fields.Vendor))
        {
            if (Plugin.DebugEnabled) Helpers.Log($"[DoOpening:Prefix] skip (panel name contains 'vendor') panel={__instance.name}");
            return;
        }

        // Universal "show hidden items" safety net. Widen-only — never shrinks. Recovers items
        // hidden by any prior bug or other mod that wrote a too-small inventory_size.
        foreach (var inv in multi_inventory.all)
        {
            if (inv?.data?.inventory == null) continue;
            if (inv.data.inventory.Count > inv.data.inventory_size)
            {
                inv.data.SetInventorySize(inv.data.inventory.Count);
            }
        }

        var tools = multi_inventory.all
            .Where(a => a.name == "Tools" || a.data.id is "Tools" or "Toolbelt")
            .ToList();

        if (tools.Any())
        {
            multi_inventory.all.RemoveAll(a => tools.Contains(a));
            multi_inventory.AddInventory(tools[0], 1);
        }

        __instance.dont_show_empty_rows = Plugin.DontShowEmptyRowsInInventory.Value;

        if (Fields.IsCraft || Fields.IsVendor || Fields.IsChurchPulpit)
        {
            if (Plugin.DebugEnabled) Helpers.Log($"[DoOpening:Prefix] skip (flag match Craft={Fields.IsCraft}/Vendor={Fields.IsVendor}/Pulpit={Fields.IsChurchPulpit}) panel={__instance.name}");
            return;
        }

        if (MainGame.me.player.GetMyWorldZoneId().Contains("refugee"))
        {
            if (Plugin.DebugEnabled) Helpers.Log($"[DoOpening:Prefix] skip (player in refugee zone) panel={__instance.name}");
            return;
        }

        if (Plugin.ShowOnlyPersonalInventory.Value ||
            Fields.IsBarman ||
            Fields.IsTavernCellarRack ||
            Fields.IsSoulBox ||
            Fields.IsChest ||
            Fields.IsWritersTable)
        {
            if (__instance.name.Contains("bag"))
            {
                if (Plugin.DebugEnabled) Helpers.Log($"[DoOpening:Prefix] skip (bag panel) panel={__instance.name}");
                return;
            }

            var onlyMineInventory = new MultiInventory();
            var bagCount = multi_inventory.all[0].data.inventory.Count(item => item.is_bag);

            onlyMineInventory.AddInventory(multi_inventory.all[0]);

            if (bagCount > 0)
            {
                for (var i = 0; i < bagCount; i++)
                {
                    onlyMineInventory.AddInventory(multi_inventory.all[i + 1]);
                }
            }
            if (Plugin.DebugEnabled) Helpers.Log($"[DoOpening:Prefix] replaced multi (ShowOnly={Plugin.ShowOnlyPersonalInventory.Value},Barman={Fields.IsBarman},TavernCellar={Fields.IsTavernCellarRack},SoulBox={Fields.IsSoulBox},Chest={Fields.IsChest},Writer={Fields.IsWritersTable}) panel={__instance.name} ({multi_inventory.all.Count} → {onlyMineInventory.all.Count} inventories, {bagCount} bags)");
            multi_inventory = onlyMineInventory;
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(InventoryPanelGUI), nameof(InventoryPanelGUI.DoOpening))]
    public static void InventoryPanelGUI_DoOpening_Postfix(InventoryPanelGUI __instance)
    {
        var panelNameLower = __instance.name.ToLowerInvariant();
        var isChestPanel = panelNameLower.Contains(Fields.Chest);
        var isVendorPanel = panelNameLower.Contains(Fields.Vendor) ||
                            __instance.GetComponentInParent<VendorGUI>() != null;
        var isPlayerPanel = panelNameLower.Contains(Fields.Player) ||
                            panelNameLower.Contains(Fields.Multi) && Fields.CurrentWgoInteraction == null;

        var isResourcePanelProbably = !isChestPanel && !isVendorPanel && !isPlayerPanel;

        if (Plugin.DebugEnabled) Helpers.Log($"[DoOpening:Postfix] panel={__instance.name} classified: chest={isChestPanel} vendor={isVendorPanel} player={isPlayerPanel} resource={isResourcePanelProbably}");

        foreach (var a in __instance._separators)
        {
            if (Plugin.RemoveGapsBetweenSections.Value && isPlayerPanel ||
                Plugin.RemoveGapsBetweenSectionsVendor.Value && isVendorPanel ||
                isResourcePanelProbably)
            {
                a.Hide();
            }
        }

        foreach (var a in __instance._custom_widgets)
        {
            if (isResourcePanelProbably ||
                Fields.AlwaysHidePartials.Any(partial => a.inventory_data.id.Contains(partial)))
            {
                a.Deactivate();
            }

            HideWidgets(a);
        }

        foreach (var a in __instance._widgets)
        {
            if (isResourcePanelProbably || isPlayerPanel || isChestPanel)
            {
                if (a.gameObject.activeSelf)
                {
                    Invents.SetInventorySizeText(a);
                }
            }

            if (Fields.AlwaysHidePartials.Any(partial => a.inventory_data.id.Contains(partial)))
            {
                a.Deactivate();
            }

            HideWidgets(a);
        }

        return;

        void HideWidgets(BaseInventoryWidget a)
        {
            if (isVendorPanel ||
                Fields.IsBarman ||
                Fields.IsTavernCellarRack ||
                Fields.IsSoulBox ||
                Fields.IsChurchPulpit) return;

            var id = a.inventory_data.id;
            if (Plugin.HideSoulWidgets.Value && id.Contains(Fields.Soul) ||
                Plugin.HideStockpileWidgets.Value &&
                Fields.StockpileWidgetsPartials.Any(partial => id.Contains(partial)) ||
                Plugin.HideTavernWidgets.Value && id.Contains(Fields.Tavern) ||
                Plugin.HideWarehouseShopWidgets.Value && id.Contains(Fields.Storage))
            {
                if (!id.Contains(Fields.Writer))
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
    [HarmonyPatch(typeof(CraftResourcesSelectGUI), nameof(CraftResourcesSelectGUI.Open), typeof(WorldGameObject),
        typeof(InventoryWidget.ItemFilterDelegate), typeof(CraftResourcesSelectGUI.ResourceSelectResultDelegate),
        typeof(bool))]
    public static void CraftResourcesSelectGUI_Open(ref bool force_ignore_toolbelt)
    {
        force_ignore_toolbelt = true;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(InventoryPanelGUI), nameof(InventoryPanelGUI.Redraw))]
    public static void InventoryPanelGUI_Redraw(InventoryPanelGUI __instance)
    {
        var panelNameLower = __instance.name.ToLowerInvariant();
        var isChest = panelNameLower.Contains(Fields.Chest);
        var isPlayer = panelNameLower.Contains(Fields.Player) ||
                       panelNameLower.Contains(Fields.Multi) && Fields.CurrentWgoInteraction == null;

        if ((isPlayer || isChest) && Plugin.ShowUsedSpaceInTitles.Value)
        {
            foreach (var widget in __instance._widgets)
            {
                Invents.SetInventorySizeText(widget);
            }
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
    public static void InventoryWidget_FilterItems(InventoryWidget __instance,
        InventoryWidget.ItemFilterDelegate filter_delegate)
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
                case InventoryWidget.ItemFilterResult.Hide:
                    a.Deactivate();
                    break;

                case InventoryWidget.ItemFilterResult.Unknown:
                    a.DrawUnknown();
                    break;
            }
        });

        var activeCount = __instance.items.Count(x => !x.is_inactive_state);

        if (activeCount <= 0)
        {
            __instance.Hide();
        }

        __instance.RecalculateWidgetSize();
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(SoulContainerWidget), nameof(SoulContainerWidget.SoulItemsFilter), typeof(Item))]
    [HarmonyPatch(typeof(SoulHealingWidget), nameof(SoulHealingWidget.SoulItemsFilter), typeof(Item))]
    [HarmonyPatch(typeof(MixedCraftGUI), nameof(MixedCraftGUI.AlchemyItemPickerFilter), typeof(Item),
        typeof(InventoryWidget))]
    public static void SoulHealingWidget_SoulItemsFilter(ref InventoryWidget.ItemFilterResult __result)
    {
        if (!MainGame.game_started) return;
        if (!Plugin.HideInvalidSelections.Value) return;

        if (__result != InventoryWidget.ItemFilterResult.Active)
        {
            __result = InventoryWidget.ItemFilterResult.Inactive;
        }
    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(WorldGameObject), nameof(WorldGameObject.DestroyMe))]
    public static void WorldGameObject_DestroyMe(WorldGameObject __instance)
    {
        if (__instance.obj_def.interaction_type is ObjectDefinition.InteractionType.Chest
                or ObjectDefinition.InteractionType.Builder or ObjectDefinition.InteractionType.Craft ||
            __instance.obj_id.StartsWith("mf_"))
        {
            if (Plugin.DebugEnabled) Helpers.Log($"[Invalidate] DestroyMe obj={__instance.obj_id} type={__instance.obj_def.interaction_type} → InventoriesLoaded=false");
            Fields.InventoriesLoaded = false;
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameSave), nameof(GameSave.InitPlayersInventory))]
    public static void GameSave_InitPlayersInventory(GameSave __instance)
    {
        // Fires on NEW GAME setup before MainGame.me.player is fully wired, so route through
        // the helper which falls back to PlayerVanillaFallback when OriginalInventorySizes
        // doesn't yet have an entry. Clamp to current item count for safety even though new game
        // inventory should be empty.
        var requested = Helpers.GetRequestedPlayerInventorySize();
        var clamped = Math.Max(requested, __instance._inventory.inventory.Count);
        __instance._inventory.SetInventorySize(clamped);
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(WorldGameObject), nameof(WorldGameObject.InitNewObject))]
    public static void WorldGameObject_InitNewObject(WorldGameObject __instance)
    {
        if (__instance.is_player)
        {
            // Don't TryAdd for the player — its data.inventory_size at this moment is the SAVED value
            // (already WMS-modified by a previous session), not the game's true vanilla 20.
            // Helpers.GetRequestedSize special-cases is_player and uses the hardcoded PlayerVanillaSize.
            Helpers.ApplyPlayerInventorySize();
            return;
        }

        Helpers.OriginalInventorySizes.TryAdd(__instance.obj_def.id, __instance.data.inventory_size);

        if (!Plugin.ModifyInventorySize.Value) return;
        if (!string.Equals(__instance.obj_id, Fields.NpcBarman)) return;
        if (!Helpers.OriginalInventorySizes.TryGetValue(__instance.obj_def.id, out var originalSize)) return;

        var requested = originalSize + Plugin.AdditionalContainerInventorySpace.Value;
        var clamped = Math.Max(requested, __instance.data.inventory.Count);
        __instance.data.SetInventorySize(clamped);
    }
}
