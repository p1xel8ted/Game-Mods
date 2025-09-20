namespace CultOfQoL.Patches.Gameplay;

[HarmonyPatch]
public static class TarotCardPatches
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(TarotCards.TarotCard), MethodType.Constructor, typeof(TarotCards.Card), typeof(int))]
    private static void TarotCard_Constructor(ref TarotCards.TarotCard __instance)
    {
        if (!Plugin.RareTarotCardsOnly.Value) return;
        
        // If luck multiplier is active and the card already has upgrades, only maximize if it's not already upgraded
        if (Plugin.ThriceMultiplyTarotCardLuck.Value && __instance.UpgradeIndex > 0) return;
        
        __instance.UpgradeIndex = TarotCards.GetMaxTarotCardLevel(__instance.CardType);
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.DrawRandomCard))]
    public static bool TarotCards_DrawRandomCard(PlayerFarming playerFarming, bool canBeCorrupted, ref TarotCards.TarotCard __result)
    {
        if (!Plugin.ThriceMultiplyTarotCardLuck.Value) return true;
        
        var unusedFoundTrinkets = TarotCards.GetUnusedFoundTrinkets(playerFarming, canBeCorrupted);
        if (unusedFoundTrinkets.Count <= 0) return true;
        
        var card = unusedFoundTrinkets[Random.Range(0, unusedFoundTrinkets.Count)];
        var upgradeLevel = 0;
        
        if (DataManager.Instance.dungeonRun >= 5)
        {
            var luckChance = 0.275f * DataManager.Instance.GetLuckMultiplier() * 3f;
            while (Random.Range(0f, 1f) < luckChance)
            {
                upgradeLevel++;
                // Early exit if we've hit max level
                if (upgradeLevel >= TarotCards.GetMaxTarotCardLevel(card)) break;
            }
        }
        
        __result = new TarotCards.TarotCard(card, upgradeLevel);
        return false;
    }
}