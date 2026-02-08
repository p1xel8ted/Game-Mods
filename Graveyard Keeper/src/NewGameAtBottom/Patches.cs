using System.Collections.Generic;
using HarmonyLib;

namespace NewGameAtBottom;

[Harmony]
public static class Patches
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(SaveSlotsMenuGUI), nameof(SaveSlotsMenuGUI.RedrawSlots))]
    private static void SaveSlotsMenuGUI_RedrawSlots_Postfix(SaveSlotsMenuGUI __instance)
    {
        if (__instance._slots.Count < 2) return;

        var newGameSlot = __instance._slots[0];
        __instance._slots.RemoveAt(0);
        __instance._slots.Add(newGameSlot);
        newGameSlot.transform.SetAsLastSibling();
        __instance._slots_table.Reposition();
    }
}
