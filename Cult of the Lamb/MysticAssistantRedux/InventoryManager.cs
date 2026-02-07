namespace MysticAssistantRedux;

/// <summary>
/// Manages the shop inventory for a single shop session.
/// </summary>
internal class InventoryManager
{
    // Item types used for Apple Arcade shop categories (repurposed ITEM_TYPEs for custom labels)
    public const InventoryItem.ITEM_TYPE AppleSkinType = InventoryItem.ITEM_TYPE.FOUND_ITEM_CURSE;
    public const InventoryItem.ITEM_TYPE AppleDecorationType = InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION;
    public const InventoryItem.ITEM_TYPE AppleClothingType = InventoryItem.ITEM_TYPE.FOUND_ITEM_OUTFIT;
    public const InventoryItem.ITEM_TYPE AppleFleeceType = InventoryItem.ITEM_TYPE.SPECIAL_WOOL_6;
    public const InventoryItem.ITEM_TYPE PalworldSkinType = InventoryItem.ITEM_TYPE.FOUND_ITEM_WEAPON;
    public const InventoryItem.ITEM_TYPE BossSkinType = InventoryItem.ITEM_TYPE.SPECIAL_WOOL_7;

    // Existing purchase flags
    public bool BoughtKeyPiece { get; private set; }
    public bool BoughtCrystalDoctrineStone { get; private set; }
    public bool BoughtFollowerSkin { get; private set; }
    public bool BoughtDecoration { get; private set; }
    public bool BoughtTarotCard { get; private set; }
    public bool BoughtRelic { get; private set; }

    // Apple Arcade purchase flags
    public bool BoughtAppleSkin { get; private set; }
    public bool BoughtAppleDecoration { get; private set; }
    public bool BoughtAppleClothing { get; private set; }
    public bool BoughtAppleFleece { get; private set; }

    // Palworld/Boss purchase flags
    public bool BoughtPalworldSkin { get; private set; }
    public bool BoughtBossSkin { get; private set; }

    private readonly List<InventoryItem> _shopInventory = [];
    private readonly List<InventoryItem.ITEM_TYPE> _limitedStockTypes = [];

    // Existing content pools (Mystic)
    private readonly List<string> _followerSkinsAvailable = [];
    private readonly List<StructureBrain.TYPES> _decorationsAvailable = [];
    private readonly List<TarotCards.Card> _tarotCardsAvailable = [];
    private readonly List<RelicType> _relicsAvailable = [];

    // Apple Arcade content pools
    private readonly List<string> _appleSkinsAvailable = [];
    private readonly List<StructureBrain.TYPES> _appleDecorationsAvailable = [];
    private readonly List<FollowerClothingType> _appleClothingAvailable = [];
    private readonly List<int> _appleFleecesAvailable = [];

    // Palworld/Boss content pools
    private readonly List<string> _palworldSkinsAvailable = [];
    private readonly List<string> _bossSkinsAvailable = [];

    private readonly int _maxCrystalDoctrineStones;

    public InventoryManager(Interaction_MysticShop instance)
    {
        // Get max doctrine stones matching vanilla game's DLC-aware calculation
        // Vanilla: 24 + (MAJOR_DLC ? 4 : 0) for the 4 winter/DLC doctrines
        _maxCrystalDoctrineStones = Interaction_MysticShop.maxAmountOfCrystalDoctrines
            + (DataManager.Instance.MAJOR_DLC ? 4 : 0);

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
        var item = _shopInventory.First(s => s.type == (int)itemType);
        item.quantity = Math.Max(0, item.quantity + quantity);
    }

    public int GetItemListCountByItemType(InventoryItem.ITEM_TYPE itemType)
    {
        return itemType switch
        {
            // Existing (Mystic)
            InventoryItem.ITEM_TYPE.FOUND_ITEM_FOLLOWERSKIN => _followerSkinsAvailable.Count,
            InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION_ALT => _decorationsAvailable.Count,
            InventoryItem.ITEM_TYPE.TRINKET_CARD => _tarotCardsAvailable.Count,
            InventoryItem.ITEM_TYPE.SOUL_FRAGMENT => _relicsAvailable.Count,

            // Apple Arcade
            AppleSkinType => _appleSkinsAvailable.Count,
            AppleDecorationType => _appleDecorationsAvailable.Count,
            AppleClothingType => _appleClothingAvailable.Count,
            AppleFleeceType => _appleFleecesAvailable.Count,

            // Palworld/Boss
            // PalworldSkinType => _palworldSkinsAvailable.Count,
            BossSkinType => _bossSkinsAvailable.Count,

            _ => 0
        };
    }

    public void RemoveItemFromListByTypeAndIndex(InventoryItem.ITEM_TYPE itemType, int index)
    {
        switch (itemType)
        {
            // Existing (Mystic)
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

            // Apple Arcade
            case AppleSkinType:
                _appleSkinsAvailable.RemoveAt(index);
                break;
            case AppleDecorationType:
                _appleDecorationsAvailable.RemoveAt(index);
                break;
            case AppleClothingType:
                _appleClothingAvailable.RemoveAt(index);
                break;
            case AppleFleeceType:
                _appleFleecesAvailable.RemoveAt(index);
                break;

            // Palworld/Boss
            // case PalworldSkinType:
            //     _palworldSkinsAvailable.RemoveAt(index);
            //     break;
            case BossSkinType:
                _bossSkinsAvailable.RemoveAt(index);
                break;
        }
    }

    // Existing getters
    public string GetFollowerSkinNameByIndex(int index) => _followerSkinsAvailable[index];
    public StructureBrain.TYPES GetDecorationByIndex(int index) => _decorationsAvailable[index];
    public TarotCards.Card GetTarotCardByIndex(int index) => _tarotCardsAvailable[index];
    public RelicType GetRelicByIndex(int index) => _relicsAvailable[index];

    // Apple Arcade getters
    public string GetAppleSkinNameByIndex(int index) => _appleSkinsAvailable[index];
    public StructureBrain.TYPES GetAppleDecorationByIndex(int index) => _appleDecorationsAvailable[index];
    public FollowerClothingType GetAppleClothingByIndex(int index) => _appleClothingAvailable[index];
    public int GetAppleFleeceByIndex(int index) => _appleFleecesAvailable[index];

    // Existing flag setters
    public void SetBoughtKeyPieceFlag(bool value) => BoughtKeyPiece = value;
    public void SetBoughtCrystalDoctrineStoneFlag(bool value) => BoughtCrystalDoctrineStone = value;
    public void SetBoughtFollowerSkinFlag(bool value) => BoughtFollowerSkin = value;
    public void SetBoughtDecorationFlag(bool value) => BoughtDecoration = value;
    public void SetBoughtTarotCardFlag(bool value) => BoughtTarotCard = value;
    public void SetBoughtRelicFlag(bool value) => BoughtRelic = value;

    // Apple Arcade flag setters
    public void SetBoughtAppleSkinFlag(bool value) => BoughtAppleSkin = value;
    public void SetBoughtAppleDecorationFlag(bool value) => BoughtAppleDecoration = value;
    public void SetBoughtAppleClothingFlag(bool value) => BoughtAppleClothing = value;
    public void SetBoughtAppleFleeceFlag(bool value) => BoughtAppleFleece = value;

    // Palworld/Boss getters
    public string GetPalworldSkinNameByIndex(int index) => _palworldSkinsAvailable[index];
    public string GetBossSkinNameByIndex(int index) => _bossSkinsAvailable[index];

    // Palworld/Boss flag setters
    public void SetBoughtPalworldSkinFlag(bool value) => BoughtPalworldSkin = value;
    public void SetBoughtBossSkinFlag(bool value) => BoughtBossSkin = value;

    private void PopulateShopInventory()
    {
        // Existing (Mystic) content
        PopulateFollowerSkins();
        PopulateDecorations();
        PopulateTarotCards();
        PopulateRelics();

        // Apple Arcade content (if enabled)
        PopulateAppleSkins();
        PopulateAppleDecorations();
        PopulateAppleClothing();
        PopulateAppleFleeces();

        // Palworld/Boss content (if enabled)
        // PopulatePalworldSkins(); // Palworld skins commented out — missing atlas textures
        PopulateBossSkins();

        Plugin.Log.LogInfo($"[InventoryManager] Shop stock: Skins={_followerSkinsAvailable.Count}, Decorations={_decorationsAvailable.Count}, " +
                           $"TarotCards={_tarotCardsAvailable.Count}, Relics={_relicsAvailable.Count}, " +
                           $"AppleSkins={_appleSkinsAvailable.Count}, AppleDecos={_appleDecorationsAvailable.Count}, " +
                           $"AppleClothing={_appleClothingAvailable.Count}, AppleFleeces={_appleFleecesAvailable.Count}, " +
                           $"BossSkins={_bossSkinsAvailable.Count}");

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
                if (shopStock <= 0)
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

    #region Apple Arcade Population Methods

    private void PopulateAppleSkins()
    {


        Patches.MysticShopPatches.ValidateAppleSkins();

        foreach (var (skinName, hasPcAssets) in ExclusiveContent.AppleSkins)
        {
            if (!hasPcAssets)
            {
                Plugin.Log.LogInfo($"[InventoryManager] Skipping Apple skin '{skinName}' - no PC assets");
                continue;
            }

            if (!DataManager.GetFollowerSkinUnlocked(skinName))
            {
                _appleSkinsAvailable.Add(skinName);
            }
        }

        // Always add to limited stock types so 0-stock handling works correctly
        _limitedStockTypes.Add(AppleSkinType);
    }

    private void PopulateAppleDecorations()
    {


        Plugin.Log.LogInfo($"[InventoryManager] Checking {ExclusiveContent.AppleDecorations.Length} Apple decorations");
        foreach (var deco in ExclusiveContent.AppleDecorations)
        {
            var isUnlocked = DataManager.Instance.UnlockedStructures.Contains(deco);
            Plugin.Log.LogInfo($"[InventoryManager]   Apple Decoration '{deco}': unlocked={isUnlocked}");
            if (!isUnlocked)
            {
                _appleDecorationsAvailable.Add(deco);
            }
        }

        _limitedStockTypes.Add(AppleDecorationType);
    }

    private void PopulateAppleClothing()
    {


        foreach (var clothing in ExclusiveContent.AppleClothing)
        {
            if (!DataManager.Instance.UnlockedClothing.Contains(clothing))
            {
                _appleClothingAvailable.Add(clothing);
            }
        }

        _limitedStockTypes.Add(AppleClothingType);
    }

    private void PopulateAppleFleeces()
    {


        if (!DataManager.Instance.UnlockedFleeces.Contains(ExclusiveContent.AppleFleece))
        {
            _appleFleecesAvailable.Add(ExclusiveContent.AppleFleece);
        }

        _limitedStockTypes.Add(AppleFleeceType);
    }

    #endregion

    #region Palworld/Boss Population Methods

    // private void PopulatePalworldSkins()
    // {
    //     if (!Plugin.EnableAppleArcadeContent.Value)
    //     {
    //         return;
    //     }
    //
    //     foreach (var skinName in ExclusiveContent.PalworldSkins)
    //     {
    //         if (!DataManager.GetFollowerSkinUnlocked(skinName))
    //         {
    //             _palworldSkinsAvailable.Add(skinName);
    //         }
    //     }
    //
    //     _limitedStockTypes.Add(PalworldSkinType);
    // }

    private void PopulateBossSkins()
    {
        if (!Plugin.EnableBossSkins.Value)
        {
            return;
        }

        foreach (var skinName in ExclusiveContent.BossSkins)
        {
            if (WorshipperData.Instance.GetCharacters(skinName) == null)
            {
                Plugin.Log.LogWarning($"[BossSkins] '{skinName}': NOT FOUND in WorshipperData");
            }

            if (!DataManager.GetFollowerSkinUnlocked(skinName))
            {
                _bossSkinsAvailable.Add(skinName);
            }
        }

        _limitedStockTypes.Add(BossSkinType);
    }

    #endregion
}
