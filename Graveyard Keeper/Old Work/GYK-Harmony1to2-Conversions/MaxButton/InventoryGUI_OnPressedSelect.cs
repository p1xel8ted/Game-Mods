using HarmonyLib;
using System;

namespace MaxButton
{
    [HarmonyPatch(typeof(InventoryGUI), "OnPressedSelect", new Type[] { })]
    public class InventoryGuiOnPressedSelect
    {
        [HarmonyPrefix]
        public static void Prefix() => MaxButtonVendor.SetCaller("InventoryGUI", false, 1, null, null);
    }
}