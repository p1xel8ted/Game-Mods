#if DEBUG
using UnityEngine.Windows;
using Input = UnityEngine.Input;
#endif

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

        GodTearCost = Config.Bind("02. Shop", "GodTearCost", 3,
            new ConfigDescription(Localization.GodTearCostDesc, new AcceptableValueRange<int>(1, 10),
                new ConfigurationManagerAttributes { DispName = Localization.GodTearCostName, Order = 2 }));

        Config.Bind("02. Shop", "LockCost", "",
            new ConfigDescription("Lock the current cost for this save. Cost can only be increased, not decreased.",
                null, new ConfigurationManagerAttributes
                {
                    DispName = "Lock Cost for Save",
                    Order = 0,
                    HideDefaultButton = true,
                    CustomDrawer = DrawLockCostButton
                }));

        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        Log.LogInfo($"{PluginName} v{PluginVer} loaded.");
    }

    private static void DrawLockCostButton(ConfigEntryBase entry)
    {
        var costKey = GetCostKey(SaveAndLoad.SAVE_SLOT);
        var lockedCost = PlayerPrefs.HasKey(costKey) ? PlayerPrefs.GetInt(costKey) : -1;
        var configCost = GodTearCost.Value;

        if (lockedCost > 0)
        {
            GUILayout.Label($"Locked at {lockedCost} God Tears (slot {SaveAndLoad.SAVE_SLOT})");
            if (configCost > lockedCost)
            {
                if (GUILayout.Button($"Increase to {configCost}"))
                {
                    _popupManager.ShowConfirmation(PluginName,
                        $"Increase the locked cost from {lockedCost} to {configCost} God Tears per item for save slot {SaveAndLoad.SAVE_SLOT}?\n\nThis cannot be reversed.",
                        () =>
                        {
                            PlayerPrefs.SetInt(costKey, configCost);
                            PlayerPrefs.Save();
                            Log.LogInfo($"[MysticShop] Cost increased to {configCost} for slot {SaveAndLoad.SAVE_SLOT}");
                        });
                }
            }
        }
        else
        {
            if (GUILayout.Button($"Lock at {configCost} God Tears"))
            {
                _popupManager.ShowConfirmation(PluginName,
                    $"Lock the cost at {configCost} God Tears per item for save slot {SaveAndLoad.SAVE_SLOT}?\n\nCost can only be increased after locking, not decreased.",
                    () =>
                    {
                        PlayerPrefs.SetInt(costKey, configCost);
                        PlayerPrefs.Save();
                        Log.LogInfo($"[MysticShop] Cost locked at {configCost} for slot {SaveAndLoad.SAVE_SLOT}");
                    });
            }
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

    #if DEBUG
    private bool _decoProbeRan;

    private void Update()
    {
        if (Input.GetKey(KeyCode.F5))
        {
            WorshipperData.Instance?.ExportCSV();
        }

        if (Input.GetKeyDown(KeyCode.F6) && !_decoProbeRan)
        {
            _decoProbeRan = true;
            ProbeAppleDecorations();
        }
    }

    private void ProbeAppleDecorations()
    {
        Log.LogInfo("[AppleDecoProbe] Starting probe of Apple decoration addressables...");
        foreach (var type in ExclusiveContent.AppleDecorations)
        {
            var info = StructuresData.GetInfoByType(type, 0);
            var path = info.PrefabPath;
            if (!path.Contains("Assets"))
            {
                path = $"Assets/{path}.prefab";
            }

            var capturedType = type;
            var capturedPath = path;

            UnityEngine.AddressableAssets.Addressables.LoadResourceLocationsAsync(path).Completed += locHandle =>
            {
                var locs = locHandle.Result;
                Log.LogInfo($"[AppleDecoProbe] {capturedType}: path='{capturedPath}', locations={locs?.Count ?? 0}");

                if (locs == null || locs.Count == 0)
                {
                    Log.LogError($"[AppleDecoProbe] {capturedType}: NO LOCATIONS FOUND — key not in catalog");
                    return;
                }

                foreach (var loc in locs)
                {
                    Log.LogInfo($"[AppleDecoProbe]   Location: InternalId={loc.InternalId}, Provider={loc.ProviderId}, Type={loc.ResourceType}");
                    if (loc.Dependencies != null)
                    {
                        foreach (var dep in loc.Dependencies)
                        {
                            Log.LogInfo($"[AppleDecoProbe]     Dep: InternalId={dep.InternalId}, Provider={dep.ProviderId}");
                        }
                    }
                }

                UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<GameObject>(capturedPath).Completed += assetHandle =>
                {
                    if (assetHandle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded && assetHandle.Result != null)
                    {
                        Log.LogInfo($"[AppleDecoProbe] {capturedType}: LOADED OK — name='{assetHandle.Result.name}', components={assetHandle.Result.GetComponents<Component>().Length}");
                        UnityEngine.AddressableAssets.Addressables.Release(assetHandle);
                    }
                    else
                    {
                        Log.LogError($"[AppleDecoProbe] {capturedType}: LOAD FAILED — {assetHandle.OperationException?.Message}");
                    }
                };
            };
        }
    }
    #endif
}
        