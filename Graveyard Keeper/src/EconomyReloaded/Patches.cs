namespace EconomyReloaded;

[Harmony]
public static class Patches
{
    private const string ProgressCreditKey = "er_progress_credit";

    // Replaces the vanilla buy-price calculation so we can honour the user's
    // Dynamic Buy Pricing toggle and Buy Price Multiplier slider in one place.
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Trading), nameof(Trading.GetSingleItemCostInTraderInventory), typeof(Item), typeof(int))]
    public static bool Trading_GetSingleItemCostInTraderInventory(Trading __instance, Item item, int count_modificator, ref float __result)
    {
        if (__instance?.trader == null || item?.definition == null) return true;
        if (!__instance.trader.CanTradeItem(item.definition))
        {
            __result = 0f;
            return false;
        }

        float price;
        if (Plugin.DynamicBuyPricing.Value)
        {
            var itemsCount = __instance.trader.inventory.GetTotalCount(item.id) + count_modificator;
            price = __instance.trader.GetSingleItemPrice(item, itemsCount);
        }
        else
        {
            price = item.definition.base_price;
        }

        price *= Plugin.BuyPriceMultiplier.Value;
        __result = Mathf.Round(price * 100f) / 100f;
        return false;
    }

    // Replaces the vanilla sell-price calculation. Vanilla bakes in a 0.75
    // markdown and caps at base_price; both are now expressed through the
    // Sell Price Multiplier so the user can freely adjust them.
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Trading), nameof(Trading.GetSingleItemCostInPlayerInventory), typeof(Item), typeof(int))]
    public static bool Trading_GetSingleItemCostInPlayerInventory(Trading __instance, Item item, int count_modificator, ref float __result)
    {
        if (__instance?.trader == null || __instance.player_offer == null || item?.definition == null) return true;
        if (!__instance.trader.CanTradeItem(item.definition))
        {
            __result = 0f;
            return false;
        }

        var multiplier = Plugin.SellPriceMultiplier.Value;
        float price;
        if (Plugin.DynamicSellPricing.Value)
        {
            var itemsCount = __instance.trader.inventory.GetTotalCount(item.id)
                             + __instance.player_offer.GetTotalCount(item.id)
                             + count_modificator;
            price = __instance.trader.GetSingleItemPrice(item, itemsCount) * multiplier;
            var cap = item.definition.base_price * multiplier;
            if (price > cap) price = cap;
        }
        else
        {
            price = item.definition.base_price * multiplier;
        }

        __result = Mathf.Round(price * 100f) / 100f;
        return false;
    }

    // Capture the pre-trade balance difference between vanilla (dynamic) and mod
    // (flat) pricing. The postfix applies this premium to the vendor's tier
    // progression so the level-up bar keeps moving even though the player paid
    // flat prices.
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Trading), nameof(Trading.DoAcceptOffer))]
    public static void Trading_DoAcceptOffer_Prefix(Trading __instance, ref TradeSnapshot __state)
    {
        __state = null;
        if (__instance == null) return;

        var trader = __instance.trader;
        if (trader?.vendor_data == null || trader.inventory == null) return;
        if (__instance.player_offer == null || trader.cur_offer == null) return;

        var modBalance = __instance.GetTotalBalance();
        var vanillaBalance = ComputeVanillaTotalBalance(__instance);

        __state = new TradeSnapshot
        {
            CurOfferCount = trader.cur_offer.inventory.Count,
            Premium = modBalance - vanillaBalance,
        };
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Trading), nameof(Trading.DoAcceptOffer))]
    public static void Trading_DoAcceptOffer_Postfix(Trading __instance, TradeSnapshot __state)
    {
        if (__state == null) return;
        if (__state.Premium <= 0f) return;
        if (__instance == null) return;

        var trader = __instance.trader;
        if (trader?.vendor_data == null || trader.cur_offer == null) return;

        // DoAcceptOffer clears both offer inventories on a successful trade.
        // If the trader offer is still populated, the trade bailed out early.
        if (__state.CurOfferCount > 0 && trader.cur_offer.inventory.Count == __state.CurOfferCount) return;

        var existing = trader.vendor_data.GetParam(ProgressCreditKey);
        trader.vendor_data.SetParam(ProgressCreditKey, existing + __state.Premium);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Vendor), nameof(Vendor.GetTotalGoods))]
    public static void Vendor_GetTotalGoods_Postfix(Vendor __instance, ref float __result)
    {
        if (__instance?.vendor_data == null) return;
        __result += __instance.vendor_data.GetParam(ProgressCreditKey);
    }

    // Mirrors Trading.GetTotalBalance but routes through Vendor.GetSingleItemPrice
    // (which is unpatched) to get the vanilla dynamic price. Used to compute how
    // much the real trade diverged from vanilla so the difference can be credited
    // to the vendor's tier-progress bar.
    private static float ComputeVanillaTotalBalance(Trading t)
    {
        var trader = t.trader;

        var sellValue = 0f;
        var seenSell = new HashSet<string>();
        foreach (Item obj in t.player_offer.inventory)
        {
            if (!seenSell.Add(obj.id)) continue;
            var totalCount = t.player_offer.GetTotalCount(obj.id);
            for (var i = 0; i < totalCount; i++)
            {
                var itemsCount = trader.inventory.GetTotalCount(obj.id) + t.player_offer.GetTotalCount(obj.id) + (-i);
                var dynamic = trader.GetSingleItemPrice(obj, itemsCount) * 0.75f;
                if (dynamic > obj.definition.base_price) dynamic = obj.definition.base_price;
                sellValue += Mathf.Round(dynamic * 100f) / 100f;
            }
        }

        var buyValue = 0f;
        var seenBuy = new HashSet<string>();
        foreach (Item obj in trader.cur_offer.inventory)
        {
            if (!seenBuy.Add(obj.id)) continue;
            var totalCount = trader.cur_offer.GetTotalCount(obj.id);
            for (var i = 0; i < totalCount; i++)
            {
                var itemsCount = trader.inventory.GetTotalCount(obj.id) + (i + 1);
                var dynamic = trader.GetSingleItemPrice(obj, itemsCount);
                buyValue += Mathf.Round(dynamic * 100f) / 100f;
            }
        }

        return sellValue - buyValue;
    }

    public class TradeSnapshot
    {
        public int CurOfferCount;
        public float Premium;
    }
}
