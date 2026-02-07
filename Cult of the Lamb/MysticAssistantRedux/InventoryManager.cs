namespace MysticAssistantRedux;

/// <summary>
/// Manages the shop inventory for a single shop session.
/// </summary>
internal class InventoryManager
{
    public bool BoughtKeyPiece { get; private set; }
    public bool BoughtCrystalDoctrineStone { get; private set; }
    public bool BoughtFollowerSkin { get; private set; }
    public bool BoughtDecoration { get; private set; }
    public bool BoughtTarotCard { get; private set; }
    public bool BoughtRelic { get; private set; }

    private readonly List<InventoryItem> _shopInventory = [];
    private readonly List<InventoryItem.ITEM_TYPE> _limitedStockTypes = [];
    private readonly List<string> _followerSkinsAvailable = [];
    private readonly List<StructureBrain.TYPES> _decorationsAvailable = [];
    private readonly List<TarotCards.Card> _tarotCardsAvailable = [];
    private readonly List<RelicType> _relicsAvailable = [];

    private readonly int _maxCrystalDoctrineStones;

    public InventoryManager(Interaction_MysticShop instance)
    {
        // Get max doctrine stones from the static field (assemblies are publicized)
        _maxCrystalDoctrineStones = Interaction_MysticShop.maxAmountOfCrystalDoctrines;

        // Fix Aym/Baal flags for saves from older mod versions
        FixAymBaalFlags();

        PopulateShopInventory();
    }

    private static void FixAymBaalFlags()
    {
        // Check if player has ever had Aym or Baal
        var playerHasAym = DataManager.Instance.Followers.Exists(f => f.Name == "Aym") ||
                           DataManager.Instance.Followers_Dead.Exists(f => f.Name == "Aym") ||
                           DataManager.Instance.HasReturnedAym ||
                           DataManager.Instance.HasReturnedBoth;
        DataManager.Instance.HasAymSkin = playerHasAym;

        var playerHasBaal = DataManager.Instance.Followers.Exists(f => f.Name == "Baal") ||
                            DataManager.Instance.Followers_Dead.Exists(f => f.Name == "Baal") ||
                            DataManager.Instance.HasReturnedBaal ||
                            DataManager.Instance.HasReturnedBoth;
        DataManager.Instance.HasBaalSkin = playerHasBaal;
    }

    public List<InventoryItem> GetShopInventory() => _shopInventory;

    public void ChangeShopStockByQuantity(InventoryItem.ITEM_TYPE itemType, int quantity)
    {
        _shopInventory.First(s => s.type == (int)itemType).quantity += quantity;
    }

    public int GetItemListCountByItemType(InventoryItem.ITEM_TYPE itemType)
    {
        return itemType switch
        {
            InventoryItem.ITEM_TYPE.FOUND_ITEM_FOLLOWERSKIN => _followerSkinsAvailable.Count,
            InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION_ALT => _decorationsAvailable.Count,
            InventoryItem.ITEM_TYPE.TRINKET_CARD => _tarotCardsAvailable.Count,
            InventoryItem.ITEM_TYPE.SOUL_FRAGMENT => _relicsAvailable.Count, // For relics
            _ => 0
        };
    }

    public void RemoveItemFromListByTypeAndIndex(InventoryItem.ITEM_TYPE itemType, int index)
    {
        switch (itemType)
        {
            case InventoryItem.ITEM_TYPE.FOUND_ITEM_FOLLOWERSKIN:
                _followerSkinsAvailable.RemoveAt(index);
                break;
            case InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION_ALT:
                _decorationsAvailable.RemoveAt(index);
                break;
            case InventoryItem.ITEM_TYPE.TRINKET_CARD:
                _tarotCardsAvailable.RemoveAt(index);
                break;
            case InventoryItem.ITEM_TYPE.SOUL_FRAGMENT:
                _relicsAvailable.RemoveAt(index);
                break;
        }
    }

    public string GetFollowerSkinNameByIndex(int index) => _followerSkinsAvailable[index];
    public StructureBrain.TYPES GetDecorationByIndex(int index) => _decorationsAvailable[index];
    public TarotCards.Card GetTarotCardByIndex(int index) => _tarotCardsAvailable[index];
    public RelicType GetRelicByIndex(int index) => _relicsAvailable[index];

    public void SetBoughtKeyPieceFlag(bool value) => BoughtKeyPiece = value;
    public void SetBoughtCrystalDoctrineStoneFlag(bool value) => BoughtCrystalDoctrineStone = value;
    public void SetBoughtFollowerSkinFlag(bool value) => BoughtFollowerSkin = value;
    public void SetBoughtDecorationFlag(bool value) => BoughtDecoration = value;
    public void SetBoughtTarotCardFlag(bool value) => BoughtTarotCard = value;
    public void SetBoughtRelicFlag(bool value) => BoughtRelic = value;

    private void PopulateShopInventory()
    {
        PopulateFollowerSkins();
        PopulateDecorations();
        PopulateTarotCards();
        PopulateRelics();

        Plugin.Log.LogInfo($"[InventoryManager] Shop stock: Skins={_followerSkinsAvailable.Count}, Decorations={_decorationsAvailable.Count}, TarotCards={_tarotCardsAvailable.Count}, Relics={_relicsAvailable.Count}");

        var outOfStockItems = new List<InventoryItem.ITEM_TYPE>();

        foreach (var item in InventoryInfo.GetShopItemTypeList())
        {
            int shopStock = 99; // Default for unlimited items

            if (_limitedStockTypes.Contains(item.itemForTrade))
            {
                shopStock = GetItemListCountByItemType(item.itemForTrade);
                if (shopStock == 0)
                {
                    outOfStockItems.Add(item.itemForTrade);
                    continue;
                }
            }
            else if (item.itemForTrade == InventoryItem.ITEM_TYPE.CRYSTAL_DOCTRINE_STONE)
            {
                shopStock = _maxCrystalDoctrineStones - DataManager.Instance.CrystalDoctrinesReceivedFromMysticShop;
                if (shopStock == 0)
                {
                    outOfStockItems.Add(item.itemForTrade);
                    continue;
                }
            }
            else if (item.itemForTrade == InventoryItem.ITEM_TYPE.BLACK_GOLD)
            {
                shopStock = 100;
            }

            _shopInventory.Add(new InventoryItem(item.itemForTrade, shopStock));
        }

        // Add out-of-stock items at the end
        foreach (var itemType in outOfStockItems)
        {
            _shopInventory.Add(new InventoryItem(itemType, 0));
        }
    }

    private void PopulateFollowerSkins()
    {
        Plugin.Log.LogInfo($"[InventoryManager] Checking {DataManager.MysticShopKeeperSkins.Length} skins from MysticShopKeeperSkins");
        foreach (var skinString in DataManager.MysticShopKeeperSkins.ToList())
        {
            var isUnlocked = DataManager.GetFollowerSkinUnlocked(skinString);
            Plugin.Log.LogInfo($"[InventoryManager]   Skin '{skinString}': unlocked={isUnlocked}");
            if (!isUnlocked)
            {
                _followerSkinsAvailable.Add(skinString);
            }
        }
        _limitedStockTypes.Add(InventoryItem.ITEM_TYPE.FOUND_ITEM_FOLLOWERSKIN);
    }

    private void PopulateDecorations()
    {
        Plugin.Log.LogInfo($"[InventoryManager] Checking {DataManager.MysticShopKeeperDecorations.Length} decorations from MysticShopKeeperDecorations");
        foreach (var deco in DataManager.MysticShopKeeperDecorations.ToList())
        {
            var isUnlocked = DataManager.Instance.UnlockedStructures.Contains(deco);
            Plugin.Log.LogInfo($"[InventoryManager]   Decoration '{deco}': unlocked={isUnlocked}");
            if (!isUnlocked)
            {
                _decorationsAvailable.Add(deco);
            }
        }
        _limitedStockTypes.Add(InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION_ALT);
    }

    private void PopulateTarotCards()
    {
        Plugin.Log.LogInfo($"[InventoryManager] Checking {TarotCards.MysticCards.Length} cards from TarotCards.MysticCards");
        foreach (var card in TarotCards.MysticCards)
        {
            var isUnlocked = DataManager.Instance.PlayerFoundTrinkets.Contains(card);
            Plugin.Log.LogInfo($"[InventoryManager]   Tarot card '{card}': unlocked={isUnlocked}");
            if (!isUnlocked)
            {
                _tarotCardsAvailable.Add(card);
            }
        }
        _limitedStockTypes.Add(InventoryItem.ITEM_TYPE.TRINKET_CARD);
    }

    private void PopulateRelics()
    {
        Plugin.Log.LogInfo("[InventoryManager] Checking 2 hardcoded relics");

        // Relics available from Mystic Shop are hardcoded in the original game
        var goopUnlocked = DataManager.Instance.PlayerFoundRelics.Contains(RelicType.SpawnBlackGoop);
        Plugin.Log.LogInfo($"[InventoryManager]   Relic 'SpawnBlackGoop': unlocked={goopUnlocked}");
        if (!goopUnlocked)
        {
            _relicsAvailable.Add(RelicType.SpawnBlackGoop);
        }

        var fervourUnlocked = DataManager.Instance.PlayerFoundRelics.Contains(RelicType.UnlimitedFervour);
        Plugin.Log.LogInfo($"[InventoryManager]   Relic 'UnlimitedFervour': unlocked={fervourUnlocked}");
        if (!fervourUnlocked)
        {
            _relicsAvailable.Add(RelicType.UnlimitedFervour);
        }

        _limitedStockTypes.Add(InventoryItem.ITEM_TYPE.SOUL_FRAGMENT);
    }
}
