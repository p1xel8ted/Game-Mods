using HarmonyLib;

namespace MaxButton
{
    [HarmonyPatch(typeof(InventoryGUI), "OnItemPressedItem", typeof(Item))]
    public class InventoryGuiOnItemPressedItem
    {
        [HarmonyPrefix]
        public static void Prefix() => MaxButtonVendor.SetCaller("InventoryGUI", false, 1, null, null);
    }
}