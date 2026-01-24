namespace CultOfQoL.Patches.Gameplay;

[Harmony]
public static class RitualEnrichmentNerf
{
    private static void AddCoins(int type, int quantity, bool forceNormalInventory = false)
    {
        if (Plugin.ReverseEnrichmentNerf.Value)
        {
            // CustomGiveCoins handles coin addition per-follower
            Plugin.L($"RitualEnrichmentNerf: Skipping RitualRoutine's AddItem({quantity} coins) - CustomGiveCoins handles it.");
            return;
        }
        Inventory.AddItem(type, quantity, forceNormalInventory);
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
    private static bool RitualDonation_GiveCoins(ref IEnumerator __result, Follower follower, float totalTime, float delay)
    {
        if (!Plugin.ReverseEnrichmentNerf.Value)
        {
            return true;
        }

        __result = CustomGiveCoins(follower, totalTime, delay);
        return false;
    }

    private static IEnumerator CustomGiveCoins(Follower follower, float totalTime, float delay)
    {
        if (!follower)
        {
            yield break;
        }

        var coinsPerFollower = follower.Brain.Info.XPLevel * 10;

        yield return new WaitForSeconds(delay);

        var increment = (totalTime - delay) / coinsPerFollower;
        for (var i = 0; i < coinsPerFollower; i++)
        {
            AudioManager.Instance.PlayOneShot("event:/followers/pop_in", follower.transform.position);
            ResourceCustomTarget.Create(PlayerFarming.Instance.gameObject, follower.transform.position, InventoryItem.ITEM_TYPE.BLACK_GOLD, null);
            yield return new WaitForSeconds(increment);
        }

        Inventory.AddItem(InventoryItem.ITEM_TYPE.BLACK_GOLD, coinsPerFollower * 2);
        Plugin.L($"RitualEnrichmentNerf: {follower.Brain.Info.Name} donated {coinsPerFollower * 2} coins (level {follower.Brain.Info.XPLevel}).");
    }
}
