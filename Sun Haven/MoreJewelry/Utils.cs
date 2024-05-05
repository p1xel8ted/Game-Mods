namespace MoreJewelry;

/// <summary>
/// Provides utility functions, including player preferences updates, mod resetting, and content translation.
/// </summary>
public static class Utils
{
    /// <summary>
    /// Updates the player preference for the visibility state of the UI's left arrow instance.
    /// </summary>
    internal static void UpdatePlayerPref()
    {
        PlayerPrefs.SetInt(Const.PlayerPrefKey, !UI.LeftArrowInstance.activeSelf ? 1 : 0);
    }

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
        Object.DestroyImmediate(UI.GearPanel);
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
    /// Gets the content for the left arrow popup based on the current language setting. Translations provided by ChatGPT-4.
    /// </summary>
    /// <returns>The translated content for the left arrow popup.</returns>
    internal static string GetLeftArrowPopupContent()
    {
        if (TranslationLayer.CurrentLanguage == null || string.IsNullOrEmpty(TranslationLayer.CurrentLanguage.LanguageCode))
            return Const.RevealTheContentsOfTheJewelryPouch;

        return TranslationLayer.CurrentLanguage.LanguageCode switch
        {
            "DA" => "Afslør indholdet af smykkeposen",
            "DE" => "Den Inhalt des Schmuckbeutels enthüllen",
            "ES" => "Revelar el contenido de la bolsa de joyas",
            "FR" => "Révéler le contenu de la pochette à bijoux",
            "IT" => "Rivelare il contenuto della borsa di gioielli",
            "JA" => "ジュエリーポーチの中身を明らかにする",
            "KO" => "보석 주머니의 내용을 드러내다",
            "NL" => "De inhoud van het juwelenzakje onthullen",
            "PL" => "Odsłonić zawartość torby z biżuterią",
            "PT-BR" => "Revelar o conteúdo da bolsa de joias",
            "PT" => "Revelar o conteúdo do saquinho de joias",
            "RU" => "Раскрыть содержимое мешочка с украшениями",
            "TR" => "Mücevher kesesinin içeriğini açığa çıkar",
            "UK" => "Виявити вміст кошелька з прикрасами",
            "ZH-CHT" => "揭示珠寶袋的內容",
            "ZH" => "揭示珠宝袋的内容",
            _ => Const.RevealTheContentsOfTheJewelryPouch
        };
    }



    /// <summary>
    /// Gets the content for the right arrow popup based on the current language setting. Translations provided by ChatGPT-4.
    /// </summary>
    /// <returns>The translated content for the right arrow popup.</returns>
    internal static string GetRightArrowPopupContent()
    {
        if (TranslationLayer.CurrentLanguage == null || string.IsNullOrEmpty(TranslationLayer.CurrentLanguage.LanguageCode))
            return Const.PutTheJewelryPouchAway;

        return TranslationLayer.CurrentLanguage.LanguageCode switch
        {
            "DA" => "Læg smykkeposen væk",
            "DE" => "Den Schmuckbeutel weglegen",
            "ES" => "Guardar la bolsa de joyas",
            "FR" => "Ranger la pochette à bijoux",
            "IT" => "Mettere via la borsa di gioielli",
            "JA" => "ジュエリーポーチをしまう",
            "KO" => "보석 주머니를 치우다",
            "NL" => "Het juwelenzakje opbergen",
            "PL" => "Odłożyć torbę z biżuterią",
            "PT-BR" => "Guardar a bolsa de joias",
            "PT" => "Guardar o saquinho de joias",
            "RU" => "Убрать мешочек с украшениями",
            "TR" => "Mücevher kesesini kaldır",
            "UK" => "Прибрати кошельок з прикрасами",
            "ZH-CHT" => "收起珠寶袋",
            "ZH" => "收起珠宝袋",
            _ => Const.PutTheJewelryPouchAway
        };
    }
    
    /// <summary>
    /// Retrieves the name of the inventory slot based on its index.
    /// </summary>
    /// <param name="slotIndex">The index of the slot in the inventory.</param>
    /// <returns>The name of the slot corresponding to the given index.</returns>
    /// <remarks>
    /// This method uses a switch expression to map slot indices to their respective names.
    /// It covers various types of slots including main, secondary, and new slots for rings, keepsakes, and amulets.
    /// Returns "Unknown Slot" for indices that do not match any predefined slot.
    /// </remarks>
    public static string GetSlotName(int slotIndex)
    {
        return slotIndex switch
        {
            Const.MainRingSlot => "Main Ring Slot",
            Const.SecondaryRingSlot => "Secondary Ring Slot",
            Const.MainKeepsakeSlot => "Main Keepsake Slot",
            Const.MainAmuletSlot => "Main Amulet Slot",
            Const.NewRingSlotOne => "New Ring Slot One",
            Const.NewRingSlotTwo => "New Ring Slot Two",
            Const.NewKeepsakeSlotOne => "New Keepsake Slot One",
            Const.NewKeepsakeSlotTwo => "New Keepsake Slot Two",
            Const.NewAmuletSlotOne => "New Amulet Slot One",
            Const.NewAmuletSlotTwo => "New Amulet Slot Two",
            _ => "Unknown Slot"
        };
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
    /// Gets the title text based on the current language setting. Translations provided by ChatGPT-4.
    /// </summary>
    /// <returns>The translated title text.</returns>
    internal static string GetTitle()
    {
        if (TranslationLayer.CurrentLanguage == null || string.IsNullOrEmpty(TranslationLayer.CurrentLanguage.LanguageCode))
            return Const.GearTitleText;

        return TranslationLayer.CurrentLanguage.LanguageCode switch
        {
            "DA" => // Danish
                "Smykkepung",
            "DE" => // German
                "Schmuckbeutel",
            "ES" => // Spanish
                "Bolsa de Joyas",
            "FR" => // French
                "Pochette à Bijoux",
            "IT" => // Italian
                "Borsellino per Gioielli",
            "JA" => // Japanese
                "ジュエリーポーチ",
            "KO" => // Korean
                "쥬얼리 파우치",
            "NL" => // Dutch
                "Sieraden Zakje",
            "PL" => // Polish
                "Saszetka na Biżuterię",
            "PT-BR" => // Portuguese (Brazil)
                "Bolsa de Joias",
            "PT" => // Portuguese (Portugal)
                "Bolsa de Joias",
            "RU" => // Russian
                "Мешочек для Украшений",
            "TR" => // Turkish
                "Takı Kesesi",
            "UK" => // Ukrainian
                "Мішечок для Прикрас",
            "ZH-CHT" => // Chinese Traditional
                "珠寶袋",
            "ZH" => // Chinese Simplified
                "珠宝袋",
            _ => Const.GearTitleText
        };
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