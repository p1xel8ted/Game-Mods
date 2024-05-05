using System;
using System.Collections.Generic;
using HarmonyLib;
using Moonlighter.DLC.WandererDungeon;

namespace QoL.Patches;

[Harmony]
public static class NotebookFix
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(WandererDLCController), nameof(WandererDLCController.GetAvailableItemsToShow))]
    private static void Postfix(ref List<ItemMaster> __result)
    {
        var currentGamePlusLevel = GameManager.Instance.GetCurrentGamePlusLevel();
        var run = currentGamePlusLevel > 0;
        if (!run) return;

        var r = __result;
        var items = ItemDatabase.GetItems(x => r.Exists(y => y.name == x.name), currentGamePlusLevel);
        Plugin.LOG.LogWarning("NotebookFix: " + items.Count + " items found for game plus level " + currentGamePlusLevel);
        __result = items;
    }
}