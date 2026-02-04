namespace MysticAssistantRedux;

/// <summary>
/// Defines shop items and handles overbuy warning logic.
/// </summary>
internal static class InventoryInfo
{
    private const int MaxCountDarkNecklace = 1;
    private const int MaxCountLightNecklace = 1;
    private const int MaxCountTalismanPieces = 12; // Max as of Unholy Alliance update

    /// <summary>
    /// Gets the localized shop label for an item type, with custom handling for some items.
    /// </summary>
    public static string GetShopLabelByItemType(InventoryItem.ITEM_TYPE itemType)
    {
        return itemType switch
        {
            InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION_ALT =>
                LocalizationManager.GetTranslation($"Inventory/{InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION}"),
            // SOUL_FRAGMENT is used for relics in the shop
            InventoryItem.ITEM_TYPE.SOUL_FRAGMENT => "Relic",
            _ => LocalizationManager.GetTranslation($"Inventory/{itemType}")
        };
    }

    /// <summary>
    /// Checks if the player should be warned about buying more of an item than normally allowed.
    /// </summary>
    public static bool CheckForBoughtQuantityWarning(TraderTrackerItems chosenItem)
    {
        switch (chosenItem.itemForTrade)
        {
            case InventoryItem.ITEM_TYPE.Necklace_Dark:
                // Warn if player already has Aym, has the necklace, or a follower is wearing it
                if (DataManager.Instance.HasAymSkin ||
                    Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.Necklace_Dark) >= MaxCountDarkNecklace ||
                    DataManager.Instance.Followers.Exists(f => f.Necklace == InventoryItem.ITEM_TYPE.Necklace_Dark))
                {
                    return true;
                }
                break;

            case InventoryItem.ITEM_TYPE.Necklace_Light:
                // Warn if player already has Baal, has the necklace, or a follower is wearing it
                if (DataManager.Instance.HasBaalSkin ||
                    Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.Necklace_Light) >= MaxCountLightNecklace ||
                    DataManager.Instance.Followers.Exists(f => f.Necklace == InventoryItem.ITEM_TYPE.Necklace_Light))
                {
                    return true;
                }
                break;

            case InventoryItem.ITEM_TYPE.TALISMAN:
                if (DataManager.Instance.TalismanPiecesReceivedFromMysticShop >= MaxCountTalismanPieces)
                {
                    return true;
                }
                break;
        }

        return false;
    }

    /// <summary>
    /// Creates the list of all items available in the Mystic Assistant shop.
    /// All items cost 1 God Tear.
    /// </summary>
    public static List<TraderTrackerItems> GetShopItemTypeList()
    {
        return
        [
            CreateTraderItem(InventoryItem.ITEM_TYPE.Necklace_Gold_Skull),
            CreateTraderItem(InventoryItem.ITEM_TYPE.Necklace_Demonic),
            CreateTraderItem(InventoryItem.ITEM_TYPE.Necklace_Loyalty),
            CreateTraderItem(InventoryItem.ITEM_TYPE.Necklace_Missionary),
            CreateTraderItem(InventoryItem.ITEM_TYPE.Necklace_Light),
            CreateTraderItem(InventoryItem.ITEM_TYPE.Necklace_Dark),
            CreateTraderItem(InventoryItem.ITEM_TYPE.CRYSTAL_DOCTRINE_STONE),
            CreateTraderItem(InventoryItem.ITEM_TYPE.TALISMAN),
            CreateTraderItem(InventoryItem.ITEM_TYPE.FOUND_ITEM_FOLLOWERSKIN),
            CreateTraderItem(InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION_ALT),
            CreateTraderItem(InventoryItem.ITEM_TYPE.TRINKET_CARD),
            CreateTraderItem(InventoryItem.ITEM_TYPE.SOUL_FRAGMENT), // Used for relics
            CreateTraderItem(InventoryItem.ITEM_TYPE.BLACK_GOLD)
        ];
    }

    private static TraderTrackerItems CreateTraderItem(InventoryItem.ITEM_TYPE itemType)
    {
        return new TraderTrackerItems
        {
            itemForTrade = itemType,
            BuyPrice = 1,
            BuyOffset = 0,
            SellPrice = 1,
            SellOffset = 0,
            LastDayChecked = TimeManager.CurrentDay
        };
    }
}
