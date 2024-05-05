using HarmonyLib;
using System;

namespace MaxButton
{
    [HarmonyPatch(typeof(InventoryGUI), "OnItemPressedInBag", new Type[] { })]
    public class InventoryGuiOnItemPressedInBag
    {
        [HarmonyPrefix]
        public static void Prefix() => MaxButtonVendor.SetCaller("InventoryGUI", false, 1, null, null);
    }
}