namespace MaxButtonControllerSupport;

[Harmony]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public partial class Plugin
{
    internal const string VendorGui = "VendorGUI";
    private const string InventoryGui = "InventoryGUI";
    private const string ChestGui = "ChestGUI";
    private static bool _craftGuiOpen;
    private static bool _itemCountGuiOpen;
    private static CraftItemGUI _craftItemGui;
    private static WorldGameObject _crafteryWgo;
    private static SmartSlider _slider;

    private static bool _unsafeInteraction;

    private static readonly string[] UnSafeCraftObjects =
    [
        "mf_crematorium_corp", "garden_builddesk", "tree_garden_builddesk", "mf_crematorium", "grave_ground",
        "tile_church_semicircle_2floors", "mf_grindstone_1", "zombie_garden_desk_1", "zombie_garden_desk_2",
        "zombie_garden_desk_3",
        "zombie_vineyard_desk_1", "zombie_vineyard_desk_2", "zombie_vineyard_desk_3", "graveyard_builddesk",
        "blockage_H_low", "blockage_V_low",
        "blockage_H_high", "blockage_V_high", "wood_obstacle_v", "refugee_camp_garden_bed", "refugee_camp_garden_bed_1",
        "refugee_camp_garden_bed_2",
        "refugee_camp_garden_bed_3"
    ];

    private static readonly string[] UnSafeCraftZones =
    [
        "church"
    ];

    private static readonly string[] UnSafePartials =
    [
        "blockage", "obstacle", "builddesk", "fix", "broken"
    ];

    [HarmonyPrefix]
    [HarmonyPatch(typeof(VendorGUI), nameof(VendorGUI.OpenItemCountWidnow))]
    public static void VendorGUI_OpenItemCountWindow(ref VendorGUI __instance, ref int can_move, ref bool from_player)
    {
        MaxButtonVendor.SetCaller(VendorGui, from_player, can_move, __instance.trading);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(ItemCountGUI), nameof(ItemCountGUI.Open), typeof(string), typeof(int), typeof(int),
        typeof(Action<int>), typeof(int), typeof(ItemCountGUI.PriceCalculateDelegate))]
    public static void Postfix(ref ItemCountGUI __instance,
        ref ItemCountGUI.PriceCalculateDelegate price_calculate_delegate)
    {
        MaxButtonVendor.AddMaxButton(ref __instance, ref price_calculate_delegate);
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(InventoryGUI), nameof(InventoryGUI.OnPressedSelect))]
    public static void InventoryGUI_OnPressedSelect()
    {
        MaxButtonVendor.SetCaller(InventoryGui, false, 1, null);
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(InventoryGUI), nameof(InventoryGUI.OnItemPressedItem))]
    public static void InventoryGUI_OnItemPressedItem()
    {
        MaxButtonVendor.SetCaller(InventoryGui, false, 1, null);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(CraftGUI), nameof(CraftGUI.Open))]
    public static void CraftGUI_Open()
    {
        _craftGuiOpen = true;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(CraftGUI), nameof(CraftGUI.OnClosePressed))]
    public static void CraftGUI_OnClosePressed()
    {
        _craftGuiOpen = false;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(CraftItemGUI), nameof(CraftItemGUI.OnOver))]
    public static void CraftItemGUI_OnOver()
    {
        _craftItemGui = CraftItemGUI.current_overed;
        _crafteryWgo = GUIElements.me.craft.GetCrafteryWGO();
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(ItemCountGUI), nameof(ItemCountGUI.Open))]
    public static void OpenPostfix(ref ItemCountGUI __instance)
    {
        _itemCountGuiOpen = true;
        _slider = __instance.transform.Find("window/Container/smart slider").GetComponent<SmartSlider>();
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(CraftGUI), nameof(CraftGUI.SwitchTab))]
    public static void CraftGUI_SwitchTab(ref CraftGUI __instance)
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
        MaxButtonVendor.SetCaller(InventoryGui, false, 1, null);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(CraftGUI), nameof(CraftGUI.Open), typeof(WorldGameObject), typeof(CraftsInventory),
        typeof(string))]
    public static void CraftGUI_Open(ref CraftGUI __instance, ref WorldGameObject craftery_wgo)
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
        _itemCountGuiOpen = false;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(ChestGUI), nameof(ChestGUI.OnItemSelect))]
    public static void ChestGUI_OnItemSelect()
    {
        MaxButtonVendor.SetCaller(ChestGui, false, 1, null);
    }

    private static void WorldGameObject_Interact(WorldGameObject instance, WorldGameObject other)
    {
        if (UnSafeCraftZones.Contains(instance.GetMyWorldZoneId()) || UnSafePartials.Any(instance.obj_id.Contains) ||
            UnSafeCraftObjects.Contains(instance.obj_id))
        {
            _unsafeInteraction = true;
        }
        else
        {
            _unsafeInteraction = false;
        }
    }
}