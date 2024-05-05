namespace BiggerBackpack;

/// <summary>
/// Provides utility functions, including player preferences updates, mod resetting, and content translation.
/// </summary>
public static class Utils
{

    /// <summary>
    /// Resets the mod to its initial state when the user returns to the main menu.
    /// </summary>
    /// <remarks>
    /// Clears the gear slots, destroys the gear panel, and resets related flags.
    /// </remarks>
    internal static void ResetMod()
    {
        Log("Resetting mod as user has returned to the main menu.");
        UI.SlotsCreated = false;
        UI.ActionAttached = false;
        Patches.GearSlots.Clear();
    }

    /// <summary>
    /// Removes null values from a given dictionary and logs each removal.
    /// </summary>
    /// <param name="dictionary">The dictionary to clean up.</param>
    /// <param name="dictionaryName">The name of the dictionary for logging purposes.</param>
    /// <remarks>
    /// This method is used to ensure the integrity of dictionaries storing armor data.
    /// </remarks>
    internal static void RemoveNullValuesAndLogFromDictionary(IDictionary<(ArmorType, int), ArmorData> dictionary, string dictionaryName)
    {
        List<(ArmorType, int)> keysToRemove = [];
        keysToRemove.AddRange(from pair in dictionary where pair.Value == null select pair.Key);

        foreach (var key in keysToRemove)
        {
            dictionary.Remove(key);
            Log($"Removed null value from {dictionaryName}: Key = {key}. This message is expected; it is not an error.");
        }
        if (keysToRemove.Count > 0)
        {
            Log($"Total null values removed from {dictionaryName}: {keysToRemove.Count}. This message is expected; it is not an error.");
        }
    }


    /// <summary>
    /// Determines if a specific inventory slot is filled.
    /// </summary>
    /// <param name="inv">The inventory to check.</param>
    /// <param name="slotIndex">The index of the slot to check in the inventory.</param>
    /// <returns><c>true</c> if the slot at the specified index is filled; otherwise, <c>false</c>.</returns>
    /// <remarks>
    /// A slot is considered filled if the item ID in that slot is greater than 0.
    /// </remarks>
    internal static bool SlotFilled(Inventory inv, int slotIndex)
    {
        return inv.Items[slotIndex].id > 0;
    }
    
    /// <summary>
    /// Logs a message if the debug mode is enabled in the plugin settings.
    /// </summary>
    /// <param name="message">The message to log.</param>
    internal static void Log(string message)
    {
        if (Plugin.Debug.Value)
        {
            Plugin.LOG.LogInfo(message);
        }
    }
}