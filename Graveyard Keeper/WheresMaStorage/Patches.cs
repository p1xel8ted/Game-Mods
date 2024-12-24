using Debug = UnityEngine.Debug;

namespace WheresMaStorage;

[Harmony]
[HarmonyPriority(0)]
public static class Patches
{

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
    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(WaitingGUI), nameof(WaitingGUI.Open), null)]
    public static void WaitingGUI_Open()
    {
        Fields.InventoriesLoaded = false;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(OrganEnhancerGUI), nameof(OrganEnhancerGUI.Open), null)]
    public static void OrganEnhancerGUI_Open(ref OrganEnhancerGUI __instance)
    {
        __instance._multi_inventory = MainGame.me.player.GetMultiInventory(exceptions: null, force_world_zone: "",
            player_mi: MultiInventory.PlayerMultiInventory.IncludePlayer, include_toolbelt: true,
            include_bags: true, sortWGOS: true);
    }

    private static readonly string[] AlwaysSkip = ["slime", "bat", "refugees", "refugee", "bush_berry", "tree_apple", "bee"];

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

        var objId = __instance.obj_id;
        var objDefId = __instance.obj_def.id;
        var worldZoneId = __instance.GetMyWorldZoneId();
        var isQuarry = worldZoneId.Contains("stone_workyard") || worldZoneId.Contains("marble_deposit");
        var isWell = objId.Contains("well");
        var isZombieMill = worldZoneId.Contains("zombie_mill");

        if (AlwaysSkip.Any(skipItem => objId.Contains(skipItem) || objDefId.Contains(skipItem) || worldZoneId.Contains(skipItem))) return true;

        if (Plugin.ExcludeWellsFromSharedInventory.Value && isWell) return true;

        if (Plugin.ExcludeQuarryFromSharedInventory.Value && isQuarry) return true;

        if (Plugin.ExcludeZombieMillFromSharedInventory.Value && isZombieMill) return true;

        var isZombieWorker = __instance.has_linked_worker && __instance.linked_worker.obj_id.Contains("zombie") || objDefId.Contains("zombie");
        Fields.ZombieWorker = isZombieWorker;

        if (isZombieWorker && !Plugin.AllowZombiesAccessToSharedInventory.Value) return true;

        var proceed = objId.Contains("church_pulpit") ||
                      CrossModFields.IsCraft ||
                      isZombieWorker ||
                      objId.Contains("compost") ||
                      __instance.IsWorker() ||
                      __instance.IsInvisibleWorker() ||
                      __instance.is_player ||
                      objId.StartsWith("mf_");

        if (!proceed) return true;

        var inv = new MultiInventory();
        inv.SetInventories(Invents.GetMiInventory(objId, worldZoneId).all);

        Fields.Mi = inv;
        __result = inv;

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
    [HarmonyPatch(typeof(CraftComponent), nameof(CraftComponent.CraftReally))]
    public static void CraftComponent_CraftReally(CraftDefinition craft, bool for_gratitude_points,
        ref bool start_by_player)
    {
        Fields.GratitudeCraft = for_gratitude_points;
        if (Fields.GratitudeCraft)
        {
            start_by_player = false;
        }
    }

    private static void ResetFlags()
    {
        Fields.InventoriesLoaded = false;
        Fields.GameBalanceAlreadyRun = false;
        MainGame.game_started = false;
        Fields.DropsCleaned = false;
        Fields.DebugMessageShown = false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(InGameMenuGUI), nameof(InGameMenuGUI.OnPressedSaveAndExit))]
    public static void InGameMenuGUI_OnPressedSaveAndExit()
    {
        ResetFlags();
    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(SaveSlotsMenuGUI), nameof(SaveSlotsMenuGUI.Open))]
    public static void SaveSlotsMenuGUI_Open()
    {
        ResetFlags();
    }

    // Some crafting objects re-acquire the inventories when starting a craft, overwriting our multi. This stops that.
    [HarmonyPostfix]
    [HarmonyPatch(typeof(BaseCraftGUI), nameof(BaseCraftGUI.multi_inventory), MethodType.Getter)]
    public static void BaseCraftGUI_multi_inventory(ref BaseCraftGUI __instance, ref MultiInventory __result)
    {
        if (!Plugin.SharedInventory.Value) return;

        var crafteryWGO = __instance.GetCrafteryWGO();
        var crafteryObjId = crafteryWGO.obj_id;
        var crafteryObjDefId = crafteryWGO.obj_def.id;
        var instanceName = __instance.name;
        var crafteryWzId = crafteryWGO.GetMyWorldZoneId();

        if (AlwaysSkip.Any(a => crafteryObjId.Contains(a) || crafteryObjDefId.Contains(a) || crafteryWzId.Contains(a))) return;

        var isQuarry = crafteryWzId.Contains("stone_workyard") || crafteryWzId.Contains("marble_deposit");
        var isWell = crafteryObjId.Contains("well") || crafteryObjDefId.Contains("well");
        var isZombieMill = crafteryWzId.Contains("zombie_mill");

        var isZombie = crafteryObjId.Contains("zombie") || crafteryObjDefId.Contains("zombie");
        Fields.ZombieWorker = isZombie;

        if (AlwaysSkip.Any(a => crafteryObjId.Contains(a) || crafteryObjDefId.Contains(a) || crafteryWzId.Contains(a))) return;

        if (Plugin.ExcludeWellsFromSharedInventory.Value && isWell) return;

        if (Plugin.ExcludeQuarryFromSharedInventory.Value && isQuarry) return;

        if (Plugin.ExcludeZombieMillFromSharedInventory.Value && isZombieMill) return;

        if (!Plugin.AllowZombiesAccessToSharedInventory.Value && isZombie) return;

        __result = Invents.GetMiInventory($"[BaseCraftGUI.multi_inventory (Getter)]: {instanceName}, Craftery: {crafteryObjId}", crafteryWGO.GetMyWorldZoneId());
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(WorldGameObject), nameof(WorldGameObject.ReplaceWithObject))]
    public static void WorldGameObject_ReplaceWithObject(ref WorldGameObject __instance)
    {
        if (__instance.obj_def.interaction_type is ObjectDefinition.InteractionType.Chest
                or ObjectDefinition.InteractionType.Builder or ObjectDefinition.InteractionType.Craft ||
            __instance.obj_id.StartsWith("mf_"))
        {
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
    public static void InventoryPanelGUI_DoOpening_Prefix(ref InventoryPanelGUI __instance,
        ref MultiInventory multi_inventory)
    {
        if (GUIElements.me.pray_craft.gameObject.activeSelf ||
            GUIElements.me.pray_craft.gameObject.activeInHierarchy) return;

        if (__instance.name.ToLowerInvariant().Contains(Fields.Vendor)) return;

        var tools = multi_inventory.all
            .Where(a => a.name == "Tools" || a.data.id is "Tools" or "Toolbelt")
            .ToList();

        if (tools.Any())
        {
            multi_inventory.all.RemoveAll(a => tools.Contains(a));
            multi_inventory.AddInventory(tools[0], 1);
        }

        if (Plugin.DontShowEmptyRowsInInventory.Value)
        {
            __instance.dont_show_empty_rows = true;
        }

        if (CrossModFields.IsCraft || CrossModFields.IsVendor || CrossModFields.IsChurchPulpit) return;

        if (MainGame.me.player.GetMyWorldZoneId().Contains("refugee")) return;

        if (Plugin.ShowOnlyPersonalInventory.Value ||
            CrossModFields.IsBarman ||
            CrossModFields.IsTavernCellarRack ||
            CrossModFields.IsSoulBox ||
            CrossModFields.IsChest ||
            CrossModFields.IsWritersTable)
        {
            if (__instance.name.Contains("bag")) return;

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
            multi_inventory = onlyMineInventory;
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(InventoryPanelGUI), nameof(InventoryPanelGUI.DoOpening))]
    public static void InventoryPanelGUI_DoOpening_Postfix(ref InventoryPanelGUI __instance)
    {
        var panelNameLower = __instance.name.ToLowerInvariant();
        var isChestPanel = panelNameLower.Contains(Fields.Chest);
        var isVendorPanel = panelNameLower.Contains(Fields.Vendor);
        var isPlayerPanel = panelNameLower.Contains(Fields.Player) ||
                            panelNameLower.Contains(Fields.Multi) && CrossModFields.CurrentWgoInteraction == null;

        var isResourcePanelProbably = !isChestPanel && !isVendorPanel && !isPlayerPanel;

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
                CrossModFields.IsBarman ||
                CrossModFields.IsTavernCellarRack ||
                CrossModFields.IsSoulBox ||
                CrossModFields.IsChurchPulpit) return;

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
    public static void InventoryPanelGUI_Redraw(ref InventoryPanelGUI __instance)
    {
        var panelNameLower = __instance.name.ToLowerInvariant();
        var isChest = panelNameLower.Contains(Fields.Chest);
        var isPlayer = panelNameLower.Contains(Fields.Player) ||
                       panelNameLower.Contains(Fields.Multi) && CrossModFields.CurrentWgoInteraction == null;

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
        if (__instance.obj_def.interaction_type is ObjectDefinition.InteractionType.Chest
                or ObjectDefinition.InteractionType.Builder or ObjectDefinition.InteractionType.Craft ||
            __instance.obj_id.StartsWith("mf_"))
        {
            Fields.InventoriesLoaded = false;
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameSave), nameof(GameSave.InitPlayersInventory))]
    public static void GameSave_InitPlayersInventory(GameSave __instance)
    {
        __instance._inventory.inventory_size = Fields.PlayerInventorySize;
        __instance._inventory.SetInventorySize(Fields.PlayerInventorySize);
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(WorldGameObject), nameof(WorldGameObject.InitNewObject))]
    public static void WorldGameObject_InitNewObject(WorldGameObject __instance)
    {
        Helpers.OriginalInventorySizes.TryAdd(__instance.obj_def.id, __instance.data.inventory_size);

        if (!Plugin.ModifyInventorySize.Value) return;

        if (__instance.is_player)
        {
            __instance.data.SetInventorySize(Fields.PlayerInventorySize);
        }

        if (!string.Equals(__instance.obj_id, Fields.NpcBarman)) return;
        if (!Helpers.OriginalInventorySizes.TryGetValue(__instance.obj_def.id, out var originalSize)) return;

        __instance.data.SetInventorySize(originalSize + Plugin.AdditionalInventorySpace.Value);
    }
}