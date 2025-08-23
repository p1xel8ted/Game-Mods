namespace CultOfQoL.Patches.Gameplay;

[HarmonyPatch]
public static class TarotCardPatches
{

    [HarmonyPostfix]
    [HarmonyPatch(typeof(TarotCards.TarotCard), MethodType.Constructor, [typeof(TarotCards.Card), typeof(int)])]
    private static void TarotCard_Constructor(ref TarotCards.TarotCard __instance)
    {
        if (!Plugin.RareTarotCardsOnly.Value) return;
        __instance.UpgradeIndex = TarotCards.GetMaxTarotCardLevel(__instance.CardType);
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(TarotCards), nameof(TarotCards.DrawRandomCard))]
    public static bool TarotCards_DrawRandomCard(PlayerFarming playerFarming, bool canBeCorrupted, ref TarotCards.TarotCard __result)
    {
        if (!Plugin.ThriceMultiplyTarotCardLuck.Value) return true;
        
        var unusedFoundTrinkets = TarotCards.GetUnusedFoundTrinkets(playerFarming, canBeCorrupted);
        if (unusedFoundTrinkets.Count <= 0)
        {
            return true;
        }
        var card = unusedFoundTrinkets[Random.Range(0, unusedFoundTrinkets.Count)];
        var num = 0;
        if (DataManager.Instance.dungeonRun >= 5)
        {
            while (Random.Range(0f, 1f) < 0.275f * DataManager.Instance.GetLuckMultiplier() * 3)
            {
                num++;
            }
        }
        num = Mathf.Min(num, TarotCards.GetMaxTarotCardLevel(card));
        __result = new TarotCards.TarotCard(card, num);
        return false;
    }
}