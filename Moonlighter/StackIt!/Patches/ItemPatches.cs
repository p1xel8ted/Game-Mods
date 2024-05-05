using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;

namespace StackIt;

/// <summary>
/// A class that represents the modifications applied to items in the game.
/// This class is used to alter the properties of items, specifically their stack sizes.
/// </summary>
[HarmonyPatch]
public static class ItemPatches
{

    private readonly static Dictionary<string, int> ItemBackup = new();

    /// <summary>
    /// Applies modifications to all items in the game.
    /// The modifications are applied based on the current game state and the settings of the StackIt plugin.
    /// If the Wanderer DLC is not enabled, an error message is logged and the method returns false.
    /// </summary>
    /// <returns>True if the modifications were applied successfully, false if the Wanderer DLC is not enabled.</returns>
    public static void ApplyModifications()
    {
        var text = new StringBuilder();
        var stackIt = Plugin.StackIt.Value;
        var customStackSizes = Plugin.CustomStackSizes.Value;

        foreach (var itemCollection in ItemDatabase.Instance.itemCollections)
        {
            foreach (var item in itemCollection.items.Concat(itemCollection.createdItems))
            {
                if (!Modify(item)) continue;

                // Backup and modify sizes
                if (!ItemBackup.TryGetValue(item.nameKey, out var original))
                {
                    ItemBackup[item.nameKey] = item.maxStack;
                    if (Plugin.Debug.Value)
                    {
                        Plugin.LOG.LogWarning($"Item backed up: {item.nameKey}");
                    }
                }
                else
                {
                    // Already backed up, now apply modifications
                    text.AppendLine("\n\n------------------------------------------------------------")
                        .AppendLine($"Name: {item.nameKey}\nOriginal Size: {original}");

                    item.maxStack = original;

                    if (stackIt)
                    {
                        DoubleStacks(item, original);
                    }
                    else if (customStackSizes && original < Plugin.MaxStackSize.Value)
                    {
                        CustomStackSizes(item, original);
                    }

                    text.AppendLine($"New Size: {item.maxStack}");
                }
            }
        }
        var finalText = text.ToString();

        if (Plugin.Debug.Value)
        {
            Plugin.LOG.LogInfo("Total Items: " + ItemBackup.Count);
            Plugin.LOG.LogInfo(finalText);
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(ItemDatabase), nameof(ItemDatabase.LoadDLCItems), [])]
    public static void ItemDatabase_LoadDLCItems()
    {
        ApplyModifications();
        Plugin.LOG.LogWarning("ItemDatabase_LoadDLCItems");
    }

    // Patch for LoadDLCItems method with a string parameter
    [HarmonyPostfix]
    [HarmonyPatch(typeof(ItemDatabase), nameof(ItemDatabase.LoadDLCItems), [typeof(string)])]
    public static void ItemDatabase_LoadDLCItems_String()
    {
        ApplyModifications();
        Plugin.LOG.LogWarning("ItemDatabase_LoadDLCItems_String");
    }

    // Patch for ReadFromDLC method
    [HarmonyPostfix]
    [HarmonyPatch(typeof(ItemDatabase), nameof(ItemDatabase.ReadFromDLC))]
    public static void ItemDatabase_ReadFromDLC()
    {
        ApplyModifications();
        Plugin.LOG.LogWarning("ItemDatabase_ReadFromDLC");
    }

    // Patch for ReadDefaultFile method
    [HarmonyPostfix]
    [HarmonyPatch(typeof(ItemDatabase), nameof(ItemDatabase.ReadDefaultFile))]
    public static void ItemDatabase_ReadDefaultFile()
    {
        ApplyModifications();
        Plugin.LOG.LogWarning("ItemDatabase_ReadDefaultFile");
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(ItemDatabase), nameof(ItemDatabase.GetItems))]
    [HarmonyPatch(typeof(ItemDatabase), nameof(ItemDatabase.GetGeneratedItems))]
    [HarmonyPatch(typeof(ItemDatabase), nameof(ItemDatabase.GetDLCItems))]
    public static void ItemDatabase_Get(ref List<ItemMaster> __result)
    {
        var text = new StringBuilder();
        var stackIt = Plugin.StackIt.Value;
        var customStackSizes = Plugin.CustomStackSizes.Value;


        foreach (var item in __result)
        {
            if (!Modify(item)) continue;
            // Backup and modify sizes
            if (!ItemBackup.TryGetValue(item.nameKey, out var original))
            {
                ItemBackup[item.nameKey] = item.maxStack;

                if (Plugin.Debug.Value)
                {
                    Plugin.LOG.LogInfo($"Item backed up: {item.nameKey}");
                }
            }
            else
            {
                // Already backed up, now apply modifications
                text.AppendLine("\n\n------------------------------------------------------------")
                    .AppendLine($"Name: {item.nameKey}\nOriginal Size: {original}");

                item.maxStack = original;

                if (stackIt)
                {
                    DoubleStacks(item, original);
                }
                else if (customStackSizes && original < Plugin.MaxStackSize.Value)
                {
                    CustomStackSizes(item, original);
                }

                text.AppendLine($"New Size: {item.maxStack}");
            }
        }


        var finalText = text.ToString();

        if (Plugin.Debug.Value)
        {
            Plugin.LOG.LogInfo("Total Items (GetMethods): " + __result.Count);
            Plugin.LOG.LogInfo(finalText);
        }
    }


    /// <summary>
    /// Determines whether a given item should be modified.
    /// This method ignores coins and items with a total stack size of 3 or 4.
    /// </summary>
    /// <param name="item">The <see cref="ItemMaster"/> object representing the item to check.</param>
    /// <returns>True if the item should be modified, otherwise false.</returns>
    private static bool Modify(ItemMaster item)
    {
        if (item is null) return false;
        //ignores coin has it has a stack size of 999999999... and returns false for items that have 1 of each stack size as they are not stackable/unique
        if (item.name.Equals("Coin", StringComparison.OrdinalIgnoreCase)) return false;

        var a = item.minChestStack;
        var b = item.maxChestStack;
        var c = item.maxStack;
        var d = item.fixedChestStack;
        var total = a + b + c + d;
        return total switch
        {
            3 or 4 => false,
            _ => true
        };
    }

    /// <summary>
    /// Doubles the maximum stack size of a given item.
    /// If the original stack size is greater than 0, the item's max stack size is set to twice the original value.
    /// </summary>
    /// <param name="item">The <see cref="ItemMaster"/> object representing the item to modify.</param>
    /// <param name="original">The original maximum stack size of the item.</param>
    private static void DoubleStacks(ItemMaster item, int original)
    {
        if (original > 0)
        {
            item.maxStack = original * 2;
        }
    }

    /// <summary>
    /// Applies custom stack sizes to a given item.
    /// If the original stack size is greater than 0 and less than the maximum stack size specified by the StackIt plugin,
    /// the item's max stack size is set to the plugin's maximum stack size.
    /// </summary>
    /// <param name="item">The <see cref="ItemMaster"/> object representing the item to modify.</param>
    /// <param name="original">The original maximum stack size of the item.</param>
    private static void CustomStackSizes(ItemMaster item, int original)
    {
        if (original > 0 && item.maxStack < Plugin.MaxStackSize.Value)
        {
            item.maxStack = Plugin.MaxStackSize.Value;
        }
    }
}