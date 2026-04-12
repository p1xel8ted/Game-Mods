namespace MaxButtonsRedux;

[Harmony]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class Patches
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(MainGame), nameof(MainGame.Update))]
    public static void MainGame_Update()
    {
        if (!Plugin.IsUpdateConditionsMet()) return;

        Plugin.HandleGamepadInput();
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(VendorGUI), nameof(VendorGUI.OpenItemCountWidnow))]
    public static void VendorGUI_OpenItemCountWindow(VendorGUI __instance, int can_move, bool from_player)
    {
        MaxButtonVendor.SetCaller(Plugin.VendorGui, from_player, can_move, __instance.trading);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(ItemCountGUI), nameof(ItemCountGUI.Open), typeof(string), typeof(int), typeof(int),
        typeof(Action<int>), typeof(int), typeof(ItemCountGUI.PriceCalculateDelegate))]
    public static void Postfix(ItemCountGUI __instance,
        ItemCountGUI.PriceCalculateDelegate price_calculate_delegate)
    {
        MaxButtonVendor.AddMaxButton(__instance, price_calculate_delegate);
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(InventoryGUI), nameof(InventoryGUI.OnPressedSelect))]
    public static void InventoryGUI_OnPressedSelect()
    {
        MaxButtonVendor.SetCaller(Plugin.InventoryGui, false, 1, null);
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(InventoryGUI), nameof(InventoryGUI.OnItemPressedItem))]
    public static void InventoryGUI_OnItemPressedItem()
    {
        MaxButtonVendor.SetCaller(Plugin.InventoryGui, false, 1, null);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(CraftGUI), nameof(CraftGUI.Open))]
    public static void CraftGUI_Open()
    {
        Plugin._craftGuiOpen = true;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(CraftGUI), nameof(CraftGUI.OnClosePressed))]
    public static void CraftGUI_OnClosePressed()
    {
        Plugin._craftGuiOpen = false;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(CraftItemGUI), nameof(CraftItemGUI.OnOver))]
    public static void CraftItemGUI_OnOver()
    {
        Plugin._craftItemGui = CraftItemGUI.current_overed;
        Plugin._crafteryWgo = GUIElements.me.craft.GetCrafteryWGO();
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(ItemCountGUI), nameof(ItemCountGUI.Open))]
    public static void OpenPostfix(ItemCountGUI __instance)
    {
        Plugin._itemCountGuiOpen = true;
        Plugin._slider = __instance.transform.Find("window/Container/smart slider").GetComponent<SmartSlider>();
    }

    [HarmonyAfter("p1xel8ted.gyk.restinpatches")]
    [HarmonyPostfix]
    [HarmonyPatch(typeof(CraftGUI), nameof(CraftGUI.SwitchTab))]
    public static void CraftGUI_SwitchTab(CraftGUI __instance)
    {
        var componentsInChildren = __instance.GetComponentsInChildren<CraftItemGUI>();
        foreach (var t in componentsInChildren)
        {
            MaxButtonCrafting.AddMinAndMaxButtons(t, "amount btn R", "amount btn max", true,
                __instance.GetCrafteryWGO());
            MaxButtonCrafting.AddMinAndMaxButtons(t, "amount btn L", "amount btn min", false,
                __instance.GetCrafteryWGO());
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(InventoryGUI), nameof(InventoryGUI.OnItemPressedInBag))]
    public static void InventoryGUI_OnItemPressedInBag()
    {
        MaxButtonVendor.SetCaller(Plugin.InventoryGui, false, 1, null);
    }

    [HarmonyAfter("p1xel8ted.gyk.restinpatches")]
    [HarmonyPostfix]
    [HarmonyPatch(typeof(CraftGUI), nameof(CraftGUI.Open), typeof(WorldGameObject), typeof(CraftsInventory),
        typeof(string))]
    public static void CraftGUI_Open(CraftGUI __instance, WorldGameObject craftery_wgo)
    {
        if (LazyInput.gamepad_active)
        {
            return;
        }

        var componentsInChildren = __instance.GetComponentsInChildren<CraftItemGUI>();
        foreach (var t in componentsInChildren)
        {
            MaxButtonCrafting.AddMinAndMaxButtons(t, "amount btn R", "amount btn max", true, craftery_wgo);
            MaxButtonCrafting.AddMinAndMaxButtons(t, "amount btn L", "amount btn min", false, craftery_wgo);
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(ItemCountGUI), nameof(ItemCountGUI.OnPressedBack))]
    [HarmonyPatch(typeof(ItemCountGUI), nameof(ItemCountGUI.OnConfirm))]
    public static void ItemCountGUI_OnPressedBack()
    {
        Plugin._itemCountGuiOpen = false;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(ChestGUI), nameof(ChestGUI.OnItemSelect))]
    public static void ChestGUI_OnItemSelect()
    {
        MaxButtonVendor.SetCaller(Plugin.ChestGui, false, 1, null);
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(WorldGameObject), nameof(WorldGameObject.Interact))]
    private static void WorldGameObject_Interact(WorldGameObject __instance)
    {
        if (Plugin.UnSafeCraftZones.Contains(__instance.GetMyWorldZoneId()) || Plugin.UnSafePartials.Any(__instance.obj_id.Contains) ||
            Plugin.UnSafeCraftObjects.Contains(__instance.obj_id))
        {
            Plugin._unsafeInteraction = true;
        }
        else
        {
            Plugin._unsafeInteraction = false;
        }
    }
}
