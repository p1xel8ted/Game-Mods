namespace CultOfQoL.Patches.Gameplay;

[Harmony]
public static class RitualEnrichmentNerf
{
    private static void AddCoins(int type, int quantity, bool forceNormalInventory = false)
    {
        if (!Plugin.ReverseEnrichmentNerf.Value)
        {
            Inventory.AddItem(type, quantity, forceNormalInventory);
            return;
        }

        // Pre-nerf formula: level * 20 per follower (verified from pre-1.3.0 game code)
        // Original used ResourceCustomTarget callbacks (level*10 visual coins × 2 gold each)
        // We add the total synchronously for reliability
        var followers = Ritual.GetFollowersAvailableToAttendSermon();
        var enhancedTotal = 0;
        foreach (var brain in followers)
        {
            enhancedTotal += brain.Info.XPLevel * 20;
        }

        Plugin.WriteLog($"[EnrichmentNerf] Enhanced: {enhancedTotal} coins for {followers.Count} followers (vanilla would be {quantity})");
        Inventory.AddItem(type, enhancedTotal, forceNormalInventory);
    }

    [HarmonyTranspiler]
    [HarmonyPatch(typeof(RitualDonation), nameof(RitualDonation.RitualRoutine), MethodType.Enumerator)]
    private static IEnumerable<CodeInstruction> RitualDonation_RitualRoutine(IEnumerable<CodeInstruction> instructions)
    {
        var original = instructions.ToList();
        try
        {
            var codes = new List<CodeInstruction>(original);
            var addItemMethod = AccessTools.Method(typeof(Inventory), nameof(Inventory.AddItem), [typeof(int), typeof(int), typeof(bool)]);
            var addCoinsMethod = AccessTools.Method(typeof(RitualEnrichmentNerf), nameof(AddCoins));
            var found = false;

            for (var i = 0; i < codes.Count; i++)
            {
                if (!codes[i].Calls(addItemMethod)) continue;
                codes[i].operand = addCoinsMethod;
                found = true;
            }

            if (!found)
            {
                Plugin.Log.LogWarning("[Transpiler] RitualDonation.RitualRoutine: Failed to find Inventory.AddItem call.");
                return original;
            }

            Plugin.Log.LogInfo("[Transpiler] RitualDonation.RitualRoutine: Redirected Inventory.AddItem to AddCoins.");
            return codes;
        }
        catch (Exception ex)
        {
            Plugin.Log.LogWarning($"[Transpiler] RitualDonation.RitualRoutine: {ex.Message}");
            return original;
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(RitualDonation), nameof(RitualDonation.GiveCoins))]
    private static bool RitualDonation_GiveCoins(ref IEnumerator __result, Follower follower, float totalTime, float delay, int coinPerFollower, int totalFollowers)
    {
        if (!Plugin.ReverseEnrichmentNerf.Value)
        {
            return true;
        }

        Plugin.WriteLog($"[EnrichmentNerf] Processing follower, vanilla would give {coinPerFollower} coins (total followers: {totalFollowers})");
        __result = CustomGiveCoins(follower, totalTime, delay, coinPerFollower);
        return false;
    }

    private static IEnumerator CustomGiveCoins(Follower follower, float totalTime, float delay, int vanillaCoinPerFollower)
    {
        if (!follower)
        {
            yield break;
        }

        // Visual-only animation — actual coins are added synchronously in AddCoins
        var level = follower.Brain.Info.XPLevel;
        var visualCoins = Math.Min(level * 10, 20);

        yield return new WaitForSeconds(delay);

        var increment = (totalTime - delay) / visualCoins;
        for (var i = 0; i < visualCoins; i++)
        {
            AudioManager.Instance.PlayOneShot("event:/followers/pop_in", follower.transform.position);
            ResourceCustomTarget.Create(PlayerFarming.Instance.gameObject, follower.transform.position, InventoryItem.ITEM_TYPE.BLACK_GOLD, null);
            yield return new WaitForSeconds(increment);
        }
    }
}
