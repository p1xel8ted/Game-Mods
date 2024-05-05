using System.Collections.Generic;
using HarmonyLib;

namespace WheresMaStorage;

[HarmonyPatch]
public static class Debug
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(MultiInventory), nameof(MultiInventory.TryRemoveSpecificItemNoCheck))]
    public static void _MultiInventoryTryRemoveSpecificItemNoCheck(ref MultiInventory __instance, ref Item item)
    {
        if (__instance.ToString() == "MultiInventory")
        {
            MainPatcher.Log($"MultiInventory.TryRemoveSpecificItemNoCheck: Not sure about this one. Rejecting. Request {__instance}.");
            return;
        }
        MainPatcher.Log($"MultiInventory.TryRemoveSpecificItemNoCheck: {item.id}");
        __instance.SetInventories(MainPatcher.GetMiInventory(__instance.ToString()).all);
    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(MultiInventory), nameof(MultiInventory.IsEmpty))]
    public static void MultiInventory_IsEmpty(ref MultiInventory __instance)
    {
        if (__instance.ToString() == "MultiInventory")
        {
            MainPatcher.Log($"MultiInventory_IsEmpty: Not sure about this one. Rejecting. Request {__instance}.");
            return;
        }
        MainPatcher.Log($"MultiInventory.IsEmpty: Setting Inventories");
        __instance.SetInventories(MainPatcher.GetMiInventory(__instance.ToString()).all);
    }
}