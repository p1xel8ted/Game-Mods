using PSS;
using UnityEngine.SceneManagement;

namespace MuseumSellPriceRedux;

[Harmony]
public static class Patches
{
    private const string WouldLookGoodInAMuseum = "would look good in a museum";
    private const string LeafieTrinket = "Leafie Trinket";
    private const string FairyWings = "Fairy Wings";
    private const string ManaSap = "Mana Sap";
    private const string MysteriousAntler = "Mysterious Antler";
    private const string MonsterCandy = "Monster Candy";
    private const string DragonFang = "Dragon Fang";
    private const string UnicornHairTuft = "Unicorn Hair Tuft";
    private const string NelVarianRunestone = "Nel'Varian Runestone";
    private const string AncientElvenHeaddress = "Ancient Elven Headdress";
    private const string AncientMagicStaff = "Ancient Magic Staff";
    private const string TentacleMonsterEmblem = "Tentacle Monster Emblem";
    private const string AncientNagaCrook = "Ancient Naga Crook";
    private const string AncientAngelQuill = "Ancient Angel Quill";
    private const string AncientNelVarian = "Ancient Nel'Varian";
    private const string AncientWithergate = "Ancient Withergate";
    private const string AncientSunHaven = "Ancient Sun Haven";
    private const string OriginsOfSunHavenAndElios = "Origins of Sun Haven and Elios";
    private const string OriginsOfTheGrandTree = "Origins of the Grand Tree";
    private const string OriginsOfDynus = "Origins of Dynus";

    private static Dictionary<int, float> BackUpSellPrices { get; } = new();
    private static Dictionary<int, float> BackUpTicketPrices { get; } = new();
    private static Dictionary<int, float> BackUpOrbPrices { get; } = new();

    private readonly static HashSet<string> ExcludedNames = new(StringComparer.OrdinalIgnoreCase)
    {
        AncientNelVarian,
        AncientWithergate,
        AncientSunHaven,
        FairyWings,
        ManaSap,
        MysteriousAntler,
        MonsterCandy,
        DragonFang,
        UnicornHairTuft,
        NelVarianRunestone,
        AncientElvenHeaddress,
        AncientMagicStaff,
        TentacleMonsterEmblem,
        AncientNagaCrook,
        AncientAngelQuill
    };

    private static Dictionary<int, string> DescriptionCache { get; } = new();

    public static void ItemData_Loaded(int itemId)
    {
        var itemSellInfo = Utils.GetItemSellInfo(itemId);
        var item = GetItemFromCache(Database.Instance, itemId);

        DescriptionCache[itemId] = item.description;

        var existsInItemSellList = Utils.GetItemSellInfo(itemId);
        if (existsInItemSellList == null)
        {
            Plugin.Log($"ItemSellInfo for {itemId} does not exist!", error: true);
        }
        
        BackupItemPrices(itemSellInfo, itemId);

        ModifyItem(itemSellInfo, itemId);
    }

    private static bool IsMuseumItem(int id, float price)
    {
        var description = GetDescription(id);
        return description.Contains(WouldLookGoodInAMuseum, StringComparison.OrdinalIgnoreCase) && price <= 11f;
    }

    private static string GetDescription(int itemId)
    {
        return DescriptionCache.TryGetValue(itemId, out var description) ? description : string.Empty;
    }

    private static void BackupItemPrices(ItemSellInfo itemSellInfo, int item)
    {
        if (itemSellInfo is null)
        {
            Plugin.Log($"itemSellInfo for {item} is null!", debug: true);
            return;
        }

        if (BackUpSellPrices.TryAdd(item, itemSellInfo.sellPrice))
        {
            Plugin.Log($"Backed up gold sell price for {itemSellInfo.name} to {itemSellInfo.sellPrice}", debug: true);
        }
        if (BackUpOrbPrices.TryAdd(item, itemSellInfo.orbSellPrice))
        {
            Plugin.Log($"Backed up orb sell price for {itemSellInfo.name} to {itemSellInfo.orbSellPrice}", debug: true);
        }
        if (BackUpTicketPrices.TryAdd(item, itemSellInfo.ticketSellPrice))
        {
            Plugin.Log($"Backed up ticket sell price for {itemSellInfo.name} to {itemSellInfo.ticketSellPrice}", debug: true);
        }
    }

    // Method to get all items from the cache based on itemId
    private static ItemData GetItemFromCache(Database database, int itemId)
    {
        foreach (var typeCache in database.cache.Values)
        {
            if (!typeCache.TryGetValue(itemId, out var cacheNode)) continue;

            var cacheItem = cacheNode.Value;
            return cacheItem.Data as ItemData;
        }
        return null;
    }

    private static float GetOriginalSellPrice(ItemSellInfo item, int id)
    {
        if (!BackUpSellPrices.TryGetValue(id, out var sellPrice))
        {
            Plugin.Log($"Failed to get original sell price for {item.name}!", error: true);
            return 0;
        }

        return sellPrice;
    }

    private static float GetOriginalOrbSellPrice(ItemSellInfo item, int id)
    {
        if (!BackUpOrbPrices.TryGetValue(id, out var sellPrice))
        {
            Plugin.Log($"Failed to get original orb price for {item.name}!", error: true);
            return 0;
        }

        return sellPrice;
    }

    private static float GetOriginalTicketSellPrice(ItemSellInfo item, int id)
    {
        if (!BackUpTicketPrices.TryGetValue(id, out var sellPrice))
        {
            Plugin.Log($"Failed to get original ticket price for {item.name}!", error: true);
            return 0;
        }

        return sellPrice;
    }

    private static void ModifyItem(ItemSellInfo item, int id)
    {
        if (!Plugin.Enabled.Value) return;
        if (IsMuseumItem(id, item.sellPrice))
        {
            var originalSellPrice = GetOriginalSellPrice(item, id);
            if (ExcludedNames.Contains(item.name))
            {
                AdjustSellPriceForExcludedNames(item, id);
            }
            else if (item.name.Equals(LeafieTrinket, StringComparison.OrdinalIgnoreCase))
            {
                item.sellPrice = originalSellPrice * Plugin.Multiplier.Value / 3;
                Plugin.Log($"Adjusted gold sell price for {item.name} to {item.sellPrice}", debug: true);
            }
            else
            {
                item.sellPrice = originalSellPrice * Plugin.Multiplier.Value;
                Plugin.Log($"Adjusted gold sell price for {item.name} to {item.sellPrice}", debug: true);
            }
        }

        AdjustOtherConditions(item, id);
    }

    internal static void RestorePrices(Action onComplete = null)
    {
        Plugin.Log("Restoring prices...");
        foreach (var item in SingletonBehaviour<ItemInfoDatabase>.Instance.allItemSellInfos)
        {
            var itemName = Utils.GetNameByID(item.Key);
            if (BackUpSellPrices.TryGetValue(item.Key, out var sellPrice))
            {
                item.Value.sellPrice = sellPrice;
                Plugin.Log($"Restored gold sell price for {itemName} to {sellPrice}", debug: true);
            }
            if (BackUpOrbPrices.TryGetValue(item.Key, out var orbPrice))
            {
                item.Value.orbSellPrice = orbPrice;
                Plugin.Log($"Restored orb sell price for {itemName} to {orbPrice}", debug: true);
            }
            if (BackUpTicketPrices.TryGetValue(item.Key, out var ticketPrice))
            {
                item.Value.ticketSellPrice = ticketPrice;
                Plugin.Log($"Restored ticket sell price for {itemName} to {ticketPrice}", debug: true);
            }
        }

        if (onComplete == null)
        {
            Plugin.Log("Restoring completed...");
            Plugin.SendNotification("Prices restored to default!");
            return;
        }

        onComplete.Invoke();
    }

    private static void AdjustSellPriceForExcludedNames(ItemSellInfo item, int id)
    {
        if (!Plugin.Enabled.Value) return;
        var originalSellPrice = GetOriginalSellPrice(item, id);
        var originalOrbPrice = GetOriginalOrbSellPrice(item, id);
        var originalTicketPrice = GetOriginalTicketSellPrice(item, id);

        if (item.name.Contains(FairyWings, StringComparison.OrdinalIgnoreCase) || item.name.Contains(ManaSap, StringComparison.OrdinalIgnoreCase) || item.name.Contains(MysteriousAntler, StringComparison.OrdinalIgnoreCase))
        {
            item.sellPrice = 0f;
            item.orbSellPrice = Plugin.Multiplier.Value;
            Plugin.Log($"Adjusted orb sell price for {item.name} to {item.orbSellPrice}", debug: true);
        }
        else if (item.name.Contains(MonsterCandy, StringComparison.OrdinalIgnoreCase) || item.name.Contains(DragonFang, StringComparison.OrdinalIgnoreCase) || item.name.Contains(UnicornHairTuft, StringComparison.OrdinalIgnoreCase))
        {
            item.sellPrice = 0f;
            item.ticketSellPrice = Plugin.Multiplier.Value;
            Plugin.Log($"Adjusted ticket sell price for {item.name} to {item.ticketSellPrice}", debug: true);
        }
        else if (item.name.Contains(NelVarianRunestone, StringComparison.OrdinalIgnoreCase) || item.name.Contains(AncientElvenHeaddress, StringComparison.OrdinalIgnoreCase) || item.name.Contains(AncientMagicStaff, StringComparison.OrdinalIgnoreCase))
        {
            item.sellPrice = 0f;
            item.orbSellPrice = Plugin.Multiplier.Value;
            Plugin.Log($"Adjusted orb sell price for {item.name} to {item.orbSellPrice}", debug: true);
        }
        else if (item.name.Contains(TentacleMonsterEmblem, StringComparison.OrdinalIgnoreCase) || item.name.Contains(AncientNagaCrook, StringComparison.OrdinalIgnoreCase) || item.name.Contains(AncientAngelQuill, StringComparison.OrdinalIgnoreCase))
        {
            item.sellPrice = 0f;
            item.ticketSellPrice = Plugin.Multiplier.Value;
            Plugin.Log($"Adjusted ticket sell price for {item.name} to {item.ticketSellPrice}", debug: true);
        }
        else if (item.name.Contains(AncientNelVarian, StringComparison.OrdinalIgnoreCase) && item.sellPrice <= 101f)
        {
            item.orbSellPrice = originalOrbPrice * Plugin.Multiplier.Value / 2;
            Plugin.Log($"Adjusted orb sell price for {item.name} to {item.orbSellPrice}", debug: true);
        }
        else if (item.name.Contains(AncientWithergate, StringComparison.OrdinalIgnoreCase) && item.sellPrice <= 101f)
        {
            item.ticketSellPrice = originalTicketPrice * Plugin.Multiplier.Value / 2;
            Plugin.Log($"Adjusted ticket sell price for {item.name} to {item.ticketSellPrice}", debug: true);
        }
        else if (item.name.Contains(AncientSunHaven, StringComparison.OrdinalIgnoreCase) && item.sellPrice <= 101f)
        {
            item.sellPrice = originalSellPrice * Plugin.Multiplier.Value / 3;
            Plugin.Log($"Adjusted gold sell price for {item.name} to {item.sellPrice}", debug: true);
        }
    }

    private static void AdjustOtherConditions(ItemSellInfo item, int id)
    {
        if (!Plugin.Enabled.Value) return;
        var originalSellPrice = GetOriginalSellPrice(item, id);
        
        if (item.name.Contains(OriginsOfSunHavenAndElios, StringComparison.OrdinalIgnoreCase) && item.sellPrice <= 2f)
        {
            item.sellPrice = originalSellPrice * 10 * Plugin.Multiplier.Value / 2;
            Plugin.Log($"Adjusted gold sell price for {item.name} to {item.sellPrice}", debug: true);
        }
        else if (item.name.Contains(OriginsOfTheGrandTree, StringComparison.OrdinalIgnoreCase) && item.sellPrice <= 2f)
        {
            item.sellPrice = originalSellPrice * 10 * Plugin.Multiplier.Value / 2;
            Plugin.Log($"Adjusted gold sell price for {item.name} to {item.sellPrice}", debug: true);
        }
        else if (item.name.Contains(OriginsOfDynus, StringComparison.OrdinalIgnoreCase) && item.sellPrice <= 2f)
        {
            item.sellPrice = originalSellPrice * 10 * Plugin.Multiplier.Value / 2;
            Plugin.Log($"Adjusted gold sell price for {item.name} to {item.sellPrice}", debug: true);
        }
    }

    public static void BackUpPrices()
    {
        Plugin.Log("Backing up prices...");
        foreach (var item in SingletonBehaviour<ItemInfoDatabase>.Instance.allItemSellInfos)
        {
            BackupItemPrices(item.Value, item.Key);
        }
        Plugin.Log("Prices backed up...");
    }


    public static void ApplyPriceChanges()
    {
        if (!Plugin.Enabled.Value) return;

        Plugin.Log("Applying price changes...");
        foreach (var item in SingletonBehaviour<ItemInfoDatabase>.Instance.allItemSellInfos)
        {
            ModifyItem(item.Value, item.Key);
        }
        Plugin.Log("Price changes applied...");

        Plugin.SendNotification("Prices updated!");
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(PlayerInventory), nameof(PlayerInventory.LoadPlayerInventory))]
    private static void Player_Initialize(PlayerInventory __instance)
    {
        Plugin.LOG.LogWarning("PlayerInventory.LoadPlayerInventory");

        BackUpPrices();
        ApplyPriceChanges();
    }

}