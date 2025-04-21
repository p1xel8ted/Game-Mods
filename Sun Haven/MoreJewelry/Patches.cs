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

    [HarmonyPostfix]
    [HarmonyPatch(typeof(PlayerInventory), nameof(PlayerInventory.SetUpInventoryData))]
    private static void PlayerInventory_SetUpInventoryData(PlayerInventory __instance)
    {
        foreach (var item in __instance.Items)
        {
            Plugin.LOG.LogWarning($"Item: {item.id}-{item.item.Type}, {item.item.ID()}, {item.slot.slotNumber}, {item.slotNumber}");
        }
    }

    /// <summary>
    /// Harmony prefix patch for PlayerInventory's LoadPlayerInventory method.
    /// </summary>
    /// <param name="__instance">The instance of <see cref="PlayerInventory"/> being patched.</param>
    /// <remarks>
    /// Initializes and sets up custom gear slots and panels if they haven't been created already.
    /// </remarks>
    [HarmonyPrefix]
    [HarmonyPatch(typeof(PlayerInventory), nameof(PlayerInventory.OpenMajorPanel))]
    private static void PlayerInventory_Initialize(PlayerInventory __instance, int panelIndex)
    {
        if (!GameObject.Find(Const.PlayerInventoryPath)) return;
        
        if(panelIndex != 0) return;
        
        if (UI.SlotsCreated && UI.GearPanel != null)
        {
            Utils.Log("Slots already created. Skipping slot creation etc.");
            return;
        }

        UI.InitializeGearPanel();
        
      //UI.CreateSlots(__instance, ArmorType.Ring, 2);
       //  UI.CreateSlots(__instance, ArmorType.Keepsake, 2);
        // UI.CreateSlots(__instance, ArmorType.Amulet, 2);

         __instance.SetUpInventoryData();

        var characterPanelSlots = GameObject.Find(Const.CharacterPanelSlotsPath);
        if (characterPanelSlots != null)
        {
            UI.GearPanel.transform.SetParent(characterPanelSlots.transform, true);
            UI.GearPanel.transform.SetAsLastSibling();
            UI.GearPanel.transform.localPosition = Const.ShowPosition;
        }
        else
        {
            Plugin.LOG.LogError("Character Panel Slots not found. Please report this.");
        }

        UI.UpdatePanelVisibility();
        UI.SlotsCreated = true;
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
        //TODO: Come back when main issue is fixed
        return;
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
        //TODO: Come back when main issue is fixed
        return;
        if (Plugin.MakeSlotsStorageOnly.Value) return;
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
        // //TODO: Come back when main issue is fixed
        // return;
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
        //TODO: Come back when main issue is fixed
        return;
        if (!UI.SlotsCreated) return;
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