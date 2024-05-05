using HarmonyLib;
using System;

namespace MaxButton
{
    [HarmonyPatch(typeof(ChestGUI), "OnItemSelect", new Type[] { })]
    public class ChestGuiOnItemSelect
    {
        [HarmonyPostfix]
        public static void Prefix() => MaxButtonVendor.SetCaller("ChestGUI", false, 1, null, null);
    }
}