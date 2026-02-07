namespace MysticAssistantRedux;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.cotl.mysticassistant";
    private const string PluginName = "Mystic Assistant Redux";
    private const string PluginVer = "0.1.1";

    internal static ManualLogSource Log { get; private set; }

    // Config entries for optional content
    internal static ConfigEntry<bool> EnableDlcNecklaces { get; private set; }
    internal static ConfigEntry<bool> EnableBossSkins { get; private set; }
    internal static ConfigEntry<int> GodTearCost { get; private set; }

    // Shop context key used to identify our custom shop
    internal const string ShopContextKey = "mystic_assistant_shop";

    // Inventory manager instance, reset each time the shop is accessed
    internal static InventoryManager CurrentInventoryManager { get; set; }

    // Post-shop action queue for displaying unlock screens
    internal static List<Action> PostShopActions { get; } = [];

    // Lists of unlocked items to show after shop closes
    internal static List<StructureBrain.TYPES> UnlockedDecorations { get; } = [];
    internal static List<TarotCards.Card> UnlockedTarotCards { get; } = [];
    internal static List<RelicType> UnlockedRelics { get; } = [];
    internal static List<FollowerClothingType> UnlockedClothing { get; } = [];
    internal static List<int> UnlockedFleeces { get; } = [];

    // Overbuy warning flag
    internal static bool ShowOverbuyWarning { get; set; }

    private static PopupManager _popupManager;
    private static bool _changingCost;

    private void Awake()
    {
        Log = Logger;

        var go = new GameObject($"{PluginName}_PopupManager");
        DontDestroyOnLoad(go);
        _popupManager = go.AddComponent<PopupManager>();
        _popupManager.Title = PluginName;

        EnableDlcNecklaces = Config.Bind("01. Extra Content", "EnableDLCNecklaces", false,
            new ConfigDescription(Localization.DLCNecklacesDesc, null,
                new ConfigurationManagerAttributes { DispName = Localization.DLCNecklacesName, Order = 2 }));
        EnableBossSkins = Config.Bind("01. Extra Content", "EnableBossSkins", false,
            new ConfigDescription(Localization.BossSkinsDesc, null,
                new ConfigurationManagerAttributes { DispName = Localization.BossSkinsName, Order = 1 }));

        GodTearCost = Config.Bind("02. Shop", "GodTearCost", 1,
            new ConfigDescription(Localization.GodTearCostDesc, new AcceptableValueRange<int>(1, 10),
                new ConfigurationManagerAttributes { DispName = Localization.GodTearCostName, Order = 2 }));
        GodTearCost.SettingChanged += OnGodTearCostChanged;

        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        Log.LogInfo($"{PluginName} v{PluginVer} loaded.");
    }

    private static void OnGodTearCostChanged(object sender, EventArgs e)
    {
        if (_changingCost)
        {
            return;
        }

        _changingCost = true;
        try
        {
            var newVal = GodTearCost.Value;

            if (!SaveAndLoad.Loaded)
            {
                GodTearCost.Value = 1;
                _popupManager.ShowPopupDlg(PluginName, Localization.CostRequiresSave, false);
                return;
            }

            var key = GetCostKey(SaveAndLoad.SAVE_SLOT);
            var locked = PlayerPrefs.HasKey(key) ? PlayerPrefs.GetInt(key) : 0;

            if (newVal <= locked)
            {
                GodTearCost.Value = locked;
                return;
            }

            if (newVal <= 1 && locked == 0)
            {
                return;
            }

            var revertTo = locked > 0 ? locked : 1;
            GodTearCost.Value = revertTo;

            var message = locked > 0
                ? Localization.CostIncreaseConfirm(locked, newVal, SaveAndLoad.SAVE_SLOT)
                : Localization.CostSetConfirm(newVal, SaveAndLoad.SAVE_SLOT);

            _popupManager.ShowConfirmation(PluginName, message, () =>
            {
                _changingCost = true;
                try
                {
                    PlayerPrefs.SetInt(key, newVal);
                    PlayerPrefs.Save();
                    GodTearCost.Value = newVal;
                    Log.LogInfo($"[MysticShop] Cost set to {newVal} for slot {SaveAndLoad.SAVE_SLOT}");
                }
                finally
                {
                    _changingCost = false;
                }
            });
        }
        finally
        {
            _changingCost = false;
        }
    }

    /// <summary>
    /// Returns the effective God Tear cost for the current save slot.
    /// If a locked cost exists, returns max(config, locked) to enforce the one-way ratchet.
    /// If no locked cost, returns the config value directly.
    /// </summary>
    internal static int GetEffectiveCost()
    {
        var key = GetCostKey(SaveAndLoad.SAVE_SLOT);
        if (!PlayerPrefs.HasKey(key))
        {
            return GodTearCost.Value;
        }
        return Math.Max(GodTearCost.Value, PlayerPrefs.GetInt(key));
    }

    internal static string GetCostKey(int saveSlot) => $"MysticAssistant_Cost_{saveSlot}";

    /// <summary>
    /// Clears the locked God Tear cost when a save slot is deleted or overwritten.
    /// </summary>
    internal static void ClearCostForSlot(int saveSlot)
    {
        var key = GetCostKey(saveSlot);
        if (PlayerPrefs.HasKey(key))
        {
            PlayerPrefs.DeleteKey(key);
            PlayerPrefs.Save();
            Log.LogInfo($"[MysticShop] Cleared locked cost for save slot {saveSlot}");
        }
    }

    /// <summary>
    /// Given an item type, returns the corresponding TraderTrackerItems from the shop list.
    /// </summary>
    internal static TraderTrackerItems GetTraderTrackerItemFromItemType(InventoryItem.ITEM_TYPE specifiedType)
    {
        return InventoryInfo.GetShopItemTypeList().FirstOrDefault(shopItem => shopItem.itemForTrade == specifiedType);
    }

    /// <summary>
    /// Clears all post-shop state for a fresh shop session.
    /// </summary>
    internal static void ResetShopState()
    {
        PostShopActions.Clear();
        UnlockedDecorations.Clear();
        UnlockedTarotCards.Clear();
        UnlockedRelics.Clear();
        UnlockedClothing.Clear();
        UnlockedFleeces.Clear();
    }

}
        