namespace MysticAssistantRedux;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.cotl.mysticassistant";
    private const string PluginName = "Mystic Assistant Redux";
    private const string PluginVer = "0.1.0";

    internal static ManualLogSource Log { get; private set; }

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

    // Overbuy warning flag
    internal static bool ShowOverbuyWarning { get; set; }

    private void Awake()
    {
        Log = Logger;
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        Log.LogInfo($"{PluginName} v{PluginVer} loaded.");
    }

    /// <summary>
    /// Given an item type, returns the corresponding TraderTrackerItems from the shop list.
    /// </summary>
    internal static TraderTrackerItems GetTraderTrackerItemFromItemType(InventoryItem.ITEM_TYPE specifiedType)
    {
        foreach (var shopItem in InventoryInfo.GetShopItemTypeList())
        {
            if (shopItem.itemForTrade == specifiedType)
            {
                return shopItem;
            }
        }
        return null;
    }

    /// <summary>
    /// Clears all post-shop state for a fresh shop session.
    /// </summary>
    internal static void ResetShopState()
    {
        PostShopActions.Clear();
        UnlockedDecorations.Clear();
    }
}
        