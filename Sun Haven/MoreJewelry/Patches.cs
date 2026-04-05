namespace MoreJewelry;

/// <summary>
/// Contains Harmony patches for modifying and extending the functionality of various game methods.
/// </summary>
[Harmony]
public static class Patches
{

    /// <summary>
    /// A list of custom gear slots added by the mod.
    /// </summary>
    [UsedImplicitly] public static List<Slot> GearSlots = [];

    /// <summary>
    /// After the game loads inventory data, check if saved items exist for custom slots
    /// and restore them. The game's LoadInventory skips keys >= Items.Count, so we
    /// extend Items and manually load the custom slot data from the save.
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(PlayerInventory), nameof(PlayerInventory.LoadPlayerInventory))]
    private static void PlayerInventory_LoadPlayerInventory(PlayerInventory __instance)
    {
        var savedItems = SingletonBehaviour<GameSave>.Instance?.CurrentSave?.characterData?.Items;
        if (savedItems == null) return;

        // Extend Items list if needed
        while (__instance.Items.Count <= Const.NewAmuletSlotTwo)
        {
            __instance.Items.Add(new SlotItemData(new NormalItem(0), 0, __instance.Items.Count, null));
        }

        // Manually restore items the game skipped due to bounds check
        foreach (var kvp in savedItems)
        {
            if (kvp.Key < Const.NewRingSlotOne || kvp.Key > Const.NewAmuletSlotTwo) continue;
            if (kvp.Value?.Item == null || kvp.Value.Item.ID() == 0) continue;
            if (!PSS.Database.ValidID(kvp.Value.Item.ID())) continue;

            __instance.Items[kvp.Key].item = kvp.Value.Item;
            __instance.Items[kvp.Key].id = kvp.Value.Item.ID();
            __instance.Items[kvp.Key].amount = kvp.Value.Amount;
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(PlayerInventory), nameof(PlayerInventory.OpenMajorPanel))]
    private static void PlayerInventory_Initialize(PlayerInventory __instance, int panelIndex)
    {
        if (panelIndex != 0) return;

        if (UI.SlotsCreated && UI.GearPanel != null)
        {
            Utils.Log("Slots already created. Skipping slot creation etc.");
            return;
        }

        // Cache inventory root transform for dynamic child lookups (no hardcoded paths).
        // PlayerInventory may be mounted at any level, so search from the hierarchy root.
        var playerRoot = __instance.transform.root;
        var uiInventory = playerRoot.FindFirstChildByName("UI_Inventory");
        UI.InventoryTransform = uiInventory?.Find("Inventory");
        if (UI.InventoryTransform == null || !UI.InventoryTransform.gameObject.activeInHierarchy)
        {
            // Hierarchy isn't fully active yet — bail out and let the next OpenMajorPanel call retry.
            return;
        }

        // Cache original keepsake/amulet slot references before creating custom slots.
        // Find by ArmorType instead of hardcoded paths to survive game hierarchy changes.
        var originalKeepsake = __instance._slots.FirstOrDefault(s =>
            s.acceptableArmorType == ArmorType.Keepsake);
        var originalAmulet = __instance._slots.FirstOrDefault(s =>
            s.acceptableArmorType == ArmorType.Amulet);

        if (originalKeepsake != null && originalAmulet != null)
        {
            UI.OriginalKeepsakeSlot = originalKeepsake.gameObject;
            UI.OriginalAmuletSlot = originalAmulet.gameObject;
        }
        else
        {
            Plugin.LOG.LogWarning("Could not find original keepsake/amulet slots by ArmorType. Controller navigation may not work.");
        }

        UI.InitializeGearPanel();

        UI.CreateSlots(__instance, ArmorType.Ring, 2);
        UI.CreateSlots(__instance, ArmorType.Keepsake, 2);
        UI.CreateSlots(__instance, ArmorType.Amulet, 2);

        // Register custom slots in Items list and _slots array
        foreach (var gearSlot in GearSlots)
        {
            while (__instance.Items.Count <= gearSlot.slotNumber)
            {
                __instance.Items.Add(new SlotItemData(new NormalItem(0), 0, __instance.Items.Count, null));
            }

            __instance.Items[gearSlot.slotNumber] = new SlotItemData(new NormalItem(0), 0, gearSlot.slotNumber, gearSlot);
            gearSlot.inventory = __instance;
        }

        // Add to _slots array so SetupItemIcon can find them
        __instance._slots = __instance._slots.Concat(GearSlots.Where(s => !__instance._slots.Contains(s))).ToArray();
        __instance.maxSlots = __instance._slots.Length;

        // Restore saved items into custom slots and create their icons
        var savedItems = SingletonBehaviour<GameSave>.Instance?.CurrentSave?.characterData?.Items;
        if (savedItems != null)
        {
            foreach (var kvp in savedItems)
            {
                if (kvp.Key < Const.NewRingSlotOne || kvp.Key > Const.NewAmuletSlotTwo) continue;
                if (kvp.Value?.Item == null || kvp.Value.Item.ID() == 0) continue;
                if (!PSS.Database.ValidID(kvp.Value.Item.ID())) continue;

                __instance.Items[kvp.Key].item = kvp.Value.Item;
                __instance.Items[kvp.Key].id = kvp.Value.Item.ID();
                __instance.Items[kvp.Key].amount = kvp.Value.Amount;
                __instance.SetupItemIcon(kvp.Key);
            }
        }

        // Parent GearPanel to CharacterPanel/Slots — derived from original slot's parent.
        var characterPanelSlots = UI.OriginalKeepsakeSlot != null ? UI.OriginalKeepsakeSlot.transform.parent : null;
        if (characterPanelSlots != null)
        {
            UI.GearPanel.transform.SetParent(characterPanelSlots, true);
            UI.GearPanel.transform.SetAsLastSibling();
            UI.GearPanel.transform.localPosition = Const.ShowPosition;
        }
        else
        {
            Plugin.LOG.LogError("Character Panel Slots not found. Please report this.");
        }

        UI.UpdatePanelVisibility();
        UI.UpdateNavigationElements();
        UI.SlotsCreated = true;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(MouseAndControllerInputModule), nameof(MouseAndControllerInputModule.GetMousePointerEventData))]
    private static void MouseAndControllerInputModule_GetMousePointerEventData(ref PointerInputModule.MouseState __result)
    {
        if (!MouseVisualManager.UsingController) return;
        if (EventSystem.current == null) return;

        var selected = EventSystem.current.currentSelectedGameObject;
        if (!ShouldUseSelectedObjectForControllerPress(selected)) return;

        var leftButtonState = __result.GetButtonState(PointerEventData.InputButton.Left);
        var buttonData = leftButtonState?.eventData?.buttonData;
        if (buttonData == null) return;

        var raycast = buttonData.pointerCurrentRaycast;
        raycast.gameObject = selected;
        buttonData.pointerCurrentRaycast = raycast;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(MouseAndControllerInputModule), nameof(MouseAndControllerInputModule.ProcessMousePress))]
    private static void MouseAndControllerInputModule_ProcessMousePress(PointerInputModule.MouseButtonEventData data)
    {
        if (!MouseVisualManager.UsingController) return;
        if (EventSystem.current == null) return;
        if (data?.buttonData == null || !data.PressedThisFrame()) return;

        var selected = EventSystem.current.currentSelectedGameObject;
        if (!ShouldUseSelectedObjectForControllerPress(selected)) return;

        var buttonData = data.buttonData;
        var originalTarget = buttonData.pointerCurrentRaycast.gameObject;
        if (originalTarget != selected)
        {
            Plugin.LOG.LogInfo($"[MoreJewelry] Controller press remap: selected='{selected.name}', raycast='{originalTarget?.name ?? "null"}'");
        }
        else
        {
            Plugin.LOG.LogInfo($"[MoreJewelry] Controller press on selected='{selected.name}'");
        }

        var raycast = buttonData.pointerCurrentRaycast;
        raycast.gameObject = selected;
        buttonData.pointerCurrentRaycast = raycast;
    }

    // === Debug logging for ALL Slot interaction methods on custom slots ===

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Slot), nameof(Slot.OnPointerDown))]
    private static void Slot_OnPointerDown(Slot __instance, PointerEventData eventData)
    {
        var tag = GearSlots.Contains(__instance) ? "CUSTOM" : "NATIVE";
        Plugin.LOG.LogInfo($"[Slot:{tag}] OnPointerDown: '{__instance.name}' slot#{__instance.slotNumber} | selected='{EventSystem.current?.currentSelectedGameObject?.name ?? "null"}' | currentItemIcon={ItemIconName()} | pointer={eventData?.pointerId}");
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Slot), nameof(Slot.OnSubmit), new[] { typeof(BaseEventData) })]
    private static void Slot_OnSubmit(Slot __instance, BaseEventData eventData)
    {
        var tag = GearSlots.Contains(__instance) ? "CUSTOM" : "NATIVE";
        var itemId = __instance.inventory?.Items != null && __instance.slotNumber < __instance.inventory.Items.Count
            ? __instance.inventory.Items[__instance.slotNumber].id : -1;
        Plugin.LOG.LogInfo($"[Slot:{tag}] OnSubmit: '{__instance.name}' slot#{__instance.slotNumber} | currentItemIcon={ItemIconName()} | slotItemId={itemId} | interactable={__instance.GetComponent<Selectable>()?.interactable}");

        // When controller Submit reaches the Slot but bypasses the ItemIcon (no CurrentItemIcon),
        // and the slot has an item, forward to the ItemIcon so it can pick up the item.
        // This applies to all slots — native slots have the same issue when navigating from the custom panel.
        if (!MouseVisualManager.UsingController) return;
        if (Inventory.CurrentItemIcon != null) return;

        var itemIcon = __instance.GetComponentInChildren<ItemIcon>();
        if (itemIcon == null || itemIcon.item == null || itemIcon.item.ID() == 0) return;

        Plugin.LOG.LogInfo($"[Slot:CUSTOM] Forwarding Submit to ItemIcon '{itemIcon.name}' for pickup");
        itemIcon.ClickIcon(UIMoveType.Submit);
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Slot), nameof(Slot.OnSelect))]
    private static void Slot_OnSelect(Slot __instance)
    {
        var tag = GearSlots.Contains(__instance) ? "CUSTOM" : "NATIVE";
        Plugin.LOG.LogInfo($"[Slot:{tag}] OnSelect: '{__instance.name}' slot#{__instance.slotNumber}");
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Slot), nameof(Slot.OnDeselect))]
    private static void Slot_OnDeselect(Slot __instance)
    {
        var tag = GearSlots.Contains(__instance) ? "CUSTOM" : "NATIVE";
        Plugin.LOG.LogInfo($"[Slot:{tag}] OnDeselect: '{__instance.name}' slot#{__instance.slotNumber}");
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Slot), nameof(Slot.ValidateItem), new[] { typeof(int) })]
    private static void Slot_ValidateItem(Slot __instance, int id)
    {
        var tag = GearSlots.Contains(__instance) ? "CUSTOM" : "NATIVE";
        Plugin.LOG.LogInfo($"[Slot:{tag}] ValidateItem: '{__instance.name}' slot#{__instance.slotNumber} | itemId={id} | requireArmorType={__instance.requireArmorType} | acceptableType={__instance.acceptableArmorType}");
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Slot), nameof(Slot.ValidateItem), new[] { typeof(int) })]
    private static void Slot_ValidateItem_Post(Slot __instance, int id, bool __result)
    {
        var tag = GearSlots.Contains(__instance) ? "CUSTOM" : "NATIVE";
        Plugin.LOG.LogInfo($"[Slot:{tag}] ValidateItem result: '{__instance.name}' | itemId={id} | result={__result}");
    }

    // === ItemIcon logging to track pickup/drop state ===

    [HarmonyPostfix]
    [HarmonyPatch(typeof(ItemIcon), nameof(ItemIcon.PickupIcon))]
    private static void ItemIcon_PickupIcon(ItemIcon __instance)
    {
        Plugin.LOG.LogInfo($"[ItemIcon] PickupIcon: '{__instance.name}' | slotIndex={__instance.slotIndex} | itemId={__instance.item?.ID()} | CurrentItemIcon={ItemIconName()}");
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(ItemIcon), nameof(ItemIcon.DropIcon))]
    private static void ItemIcon_DropIcon(ItemIcon __instance)
    {
        Plugin.LOG.LogInfo($"[ItemIcon] DropIcon: '{__instance.name}' | slotIndex={__instance.slotIndex} | CurrentItemIcon={ItemIconName()}");
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(ItemIcon), nameof(ItemIcon.ClickIcon))]
    private static void ItemIcon_ClickIcon(ItemIcon __instance, UIMoveType uiMoveType)
    {
        Plugin.LOG.LogInfo($"[ItemIcon] ClickIcon: '{__instance.name}' | slotIndex={__instance.slotIndex} | moveType={uiMoveType} | CurrentItemIcon={ItemIconName()}");
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(ItemIcon), nameof(ItemIcon.OnSubmit))]
    private static void ItemIcon_OnSubmit(ItemIcon __instance)
    {
        Plugin.LOG.LogInfo($"[ItemIcon] OnSubmit: '{__instance.name}' | slotIndex={__instance.slotIndex} | itemId={__instance.item?.ID()} | CurrentItemIcon={ItemIconName()}");
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(ItemIcon), nameof(ItemIcon.OnPointerDown))]
    private static void ItemIcon_OnPointerDown(ItemIcon __instance)
    {
        Plugin.LOG.LogInfo($"[ItemIcon] OnPointerDown: '{__instance.name}' | slotIndex={__instance.slotIndex} | CurrentItemIcon={ItemIconName()}");
    }

    // === PSS.UI.Selectable logging on custom slots ===

    [HarmonyPrefix]
    [HarmonyPatch(typeof(PSS.UI.Selectable), nameof(PSS.UI.Selectable.Select))]
    private static void PSSSelectable_Select(PSS.UI.Selectable __instance)
    {
        Plugin.LOG.LogInfo($"[PSS.Selectable] Select: '{__instance.name}'");
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(PSS.UI.Selectable), nameof(PSS.UI.Selectable.Deselect))]
    private static void PSSSelectable_Deselect(PSS.UI.Selectable __instance)
    {
        Plugin.LOG.LogInfo($"[PSS.Selectable] Deselect: '{__instance.name}'");
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(PSS.UI.Selectable), nameof(PSS.UI.Selectable.OnPointerEnter))]
    private static void PSSSelectable_OnPointerEnter(PSS.UI.Selectable __instance)
    {
        Plugin.LOG.LogInfo($"[PSS.Selectable] OnPointerEnter: '{__instance.name}' | selected='{EventSystem.current?.currentSelectedGameObject?.name ?? "null"}'");
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(PSS.UI.Selectable), nameof(PSS.UI.Selectable.OnPointerExit))]
    private static void PSSSelectable_OnPointerExit(PSS.UI.Selectable __instance)
    {
        Plugin.LOG.LogInfo($"[PSS.Selectable] OnPointerExit: '{__instance.name}'");
    }

    private static string ItemIconName()
    {
        var icon = Inventory.CurrentItemIcon;
        return icon != null ? $"'{icon.name}' (slot#{icon.slotIndex}, item={icon.item?.ID()})" : "null";
    }

    private static bool ShouldUseSelectedObjectForControllerPress(GameObject selected)
    {
        if (selected == null || !selected.activeInHierarchy) return false;

        if (UI.ToggleArrowInstance != null && (selected == UI.ToggleArrowInstance || selected.transform.IsChildOf(UI.ToggleArrowInstance.transform)))
        {
            return true;
        }

        if (Patches.GearSlots.Count == 0) return false;

        var slot = selected.GetComponentInParent<Slot>();
        return slot != null && Patches.GearSlots.Contains(slot);
    }
    
    /// <summary>
    /// Harmony prefix patch for the SwapItems method of the Inventory class.
    /// </summary>
    /// <param name="__instance">The instance of the Inventory being patched.</param>
    /// <param name="slot1">The first slot involved in the swap operation.</param>
    /// <remarks>
    /// This method modifies the behavior of the item swapping process in the inventory.
    /// It includes custom logic for handling the swapping of items in keepsake, amulet, and ring slots.
    /// <para>The method checks if the main slot for each item type is filled and attempts to swap with an alternate empty slot if available.</para>
    /// <para>Uses <see cref="Utils.SlotFilled"/> to check if a slot is filled and <see cref="Utils.GetSlotName"/> for logging purposes.</para>
    /// </remarks>
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Inventory), nameof(Inventory.SwapItems))]
    private static void Inventory_SwapItems(ref Inventory __instance, ref int slot1)
    {
        if (!Plugin.UseAdjustedEquipping.Value)
        {
            Utils.Log("Adjusted equipping disabled. Skipping slot swapping logic.");
            return;
        }
        
        // Helper method to handle slot swapping
        var inv = __instance;

        // Keepsake slot handling
        TrySwapSlot(ref slot1, Const.MainKeepsakeSlot, Const.NewKeepsakeSlotOne, Const.NewKeepsakeSlotTwo);

        // Amulet slot handling
        TrySwapSlot(ref slot1, Const.MainAmuletSlot, Const.NewAmuletSlotOne, Const.NewAmuletSlotTwo);

        // Ring slot handling
        TrySwapSlot(ref slot1, Const.MainRingSlot, Const.SecondaryRingSlot, Const.NewRingSlotOne, Const.NewRingSlotTwo);
        return;

        void TrySwapSlot(ref int slot, int mainSlot, params int[] alternateSlots)
        {
            if (slot != mainSlot || !Utils.SlotFilled(inv, mainSlot)) return;
            foreach (var altSlot in alternateSlots)
            {
                if (Utils.SlotFilled(inv, altSlot)) continue;
                slot = altSlot;
                Utils.Log($"Redirecting to empty slot: {Utils.GetSlotName(altSlot)}");
                return;
            }
            Utils.Log($"No empty slots found for {Utils.GetSlotName(mainSlot)}. Redirect aborted.");
        }
    }
    
    /// <summary>
    /// Harmony postfix patch for PlayerInventory's GetStat method.
    /// </summary>
    /// <param name="__instance">The instance of <see cref="PlayerInventory"/> being patched.</param>
    /// <param name="stat">The type of stat being retrieved.</param>
    /// <param name="__result">The result value of the original GetStat method.</param>
    /// <remarks>
    /// Modifies the stat calculation to include custom gear slots. This method is called every frame.
    /// </remarks>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(PlayerInventory), nameof(PlayerInventory.GetStat))]
    public static void PlayerInventory_GetStat(ref PlayerInventory __instance, StatType stat, ref float __result)
    {
        if (Plugin.MakeSlotsStorageOnly.Value) return;
        if (__instance.Items.Count <= Const.NewAmuletSlotTwo) return;
        __result += __instance.GetStatValueFromSlot(ArmorType.Ring, Const.NewRingSlotOne, 2, stat);
        __result += __instance.GetStatValueFromSlot(ArmorType.Ring, Const.NewRingSlotTwo, 3, stat);
        __result += __instance.GetStatValueFromSlot(ArmorType.Keepsake, Const.NewKeepsakeSlotOne, 1, stat);
        __result += __instance.GetStatValueFromSlot(ArmorType.Keepsake, Const.NewKeepsakeSlotTwo, 2, stat);
        __result += __instance.GetStatValueFromSlot(ArmorType.Amulet, Const.NewAmuletSlotOne, 1, stat);
        __result += __instance.GetStatValueFromSlot(ArmorType.Amulet, Const.NewAmuletSlotTwo, 2, stat);
    }

    /// <summary>
    /// Harmony postfix patch for PlayerInventory's Awake method.
    /// </summary>
    /// <param name="__instance">The instance of <see cref="PlayerInventory"/> being patched.</param>
    /// <remarks>
    /// Attaches additional actions to the OnInventoryUpdated event for cleaning up armor dictionaries. This event is triggered when
    /// the player opens or closes their inventory.
    /// </remarks>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(PlayerInventory), nameof(PlayerInventory.Awake))]
    public static void PlayerInventory_Awake(ref PlayerInventory __instance)
    {
        if (UI.ActionAttached) return;
        UI.ActionAttached = true;

        Utils.Log("Attaching RemoveNullValuesAndLogFromDictionary action to OnInventoryUpdated event.");
        var instance = __instance;
        __instance.OnInventoryUpdated += () =>
        {
            if (instance == null || instance.currentRealArmor == null || instance.currentArmor == null)
            {
                Plugin.LOG.LogError("OnInventoryUpdated: PlayerInventory instance is null. It is advised to restart your game.");
                return;
            }
            Utils.Log("OnInventoryUpdated: Cleaning armor dictionaries.");
            Utils.RemoveNullValuesAndLogFromDictionary(instance.currentRealArmor, "CurrentRealArmor");
            Utils.RemoveNullValuesAndLogFromDictionary(instance.currentArmor, "CurrentArmor");
        };
    }


    /// <summary>
    /// Harmony postfix patch for PlayerInventory's LateUpdate method.
    /// </summary>
    /// <param name="__instance">The instance of <see cref="PlayerInventory"/> being patched.</param>
    /// <remarks>
    /// Ensures custom gear slots are equipped with non-visual armor items. This is where the game begins to calculate stats the gear provides.
    /// </remarks>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(PlayerInventory), nameof(PlayerInventory.LateUpdate))]
    private static void PlayerInventory_EquipNonVisualArmor(ref PlayerInventory __instance)
    {
        if (__instance.Items.Count <= Const.NewAmuletSlotTwo) return;
        __instance.EquipNonVisualArmor(Const.NewRingSlotOne, ArmorType.Ring, 2);
        __instance.EquipNonVisualArmor(Const.NewRingSlotTwo, ArmorType.Ring, 3);
        __instance.EquipNonVisualArmor(Const.NewKeepsakeSlotOne, ArmorType.Keepsake, 1);
        __instance.EquipNonVisualArmor(Const.NewKeepsakeSlotTwo, ArmorType.Keepsake, 2);
        __instance.EquipNonVisualArmor(Const.NewAmuletSlotOne, ArmorType.Amulet, 1);
        __instance.EquipNonVisualArmor(Const.NewAmuletSlotTwo, ArmorType.Amulet, 2);
    }

    /// <summary>
    /// Harmony postfix patch for MainMenuController's EnableMenu method.
    /// </summary>
    /// <param name="__instance">The instance of <see cref="MainMenuController"/> being patched.</param>
    /// <param name="menu">The menu GameObject being enabled.</param>
    /// <remarks>
    /// Resets the mod to its initial state when certain menus are enabled.
    /// </remarks>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(MainMenuController), nameof(MainMenuController.EnableMenu))]
    private static void MainMenuController_EnableMenu(ref MainMenuController __instance, ref GameObject menu)
    {
        if (menu == __instance.homeMenu || menu == __instance.newCharacterMenu || menu == __instance.loadCharacterMenu || menu == __instance.singlePlayerMenu || menu == __instance.multiplayerMenu)
        {
            Utils.ResetMod();
        }
    }


}
