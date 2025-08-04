namespace CultOfQoL.Patches;

[Harmony]
public static class RitualEnrichmentNerf
{
    private static void AddCoins(int type, int quantity, bool forceNormalInventory = false, int myQty = 0)
    {
        if (!Plugin.ReverseEnrichmentNerf.Value || myQty == 0)
        {
            Inventory.AddItem(type, quantity, forceNormalInventory);
            return;
        }
        Inventory.AddItem(type, myQty, forceNormalInventory);
        Plugin.L($"RitualEnrichmentNerf: Adding {quantity} coins to inventory.");
    }

    [HarmonyTranspiler]
    [HarmonyPatch(typeof(RitualDonation), nameof(RitualDonation.RitualRoutine), MethodType.Enumerator)]
    private static IEnumerable<CodeInstruction> RitualDonation_RitualRoutine(IEnumerable<CodeInstruction> instructions)
    {
        var codes = new List<CodeInstruction>(instructions);
        var addItemMethod = AccessTools.Method(typeof(Inventory), nameof(Inventory.AddItem), [typeof(int), typeof(int), typeof(bool)]);
        var addCoinsMethod = AccessTools.Method(typeof(RitualEnrichmentNerf), nameof(AddCoins));

        for (var i = 0; i < codes.Count; i++)
        {
            if (!codes[i].Calls(addItemMethod)) continue;

            // Inject the myQty value before calling the new AddCoins method
            codes.Insert(i, new CodeInstruction(OpCodes.Ldc_I4, 0)); // Push the default value of 0 for myQty onto the stack

            // Replace the method call with the updated AddCoins method
            codes[i + 1].operand = addCoinsMethod;
        }

        return codes.AsEnumerable();
    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(RitualDonation), nameof(RitualDonation.GiveCoins))]
    private static bool RitualDonation_GiveCoins(ref IEnumerator __result, Follower follower, float totalTime, float delay)
    {
        if (!Plugin.ReverseEnrichmentNerf.Value)
        {
            return true;
        }
        // Replace the original method with custom logic
        __result = CustomGiveCoins(follower, totalTime, delay);

        // Skip the original method
        return false;
    }

    // Custom implementation of GiveCoins, replicating the old logic.
    private static IEnumerator CustomGiveCoins(Follower follower, float totalTime, float delay)
    {
        if (!Plugin.ReverseEnrichmentNerf.Value)
        {
            yield break;
        }

        if (!follower)
        {
            yield break;
        }

        var randomCoins = follower.Brain.Info.XPLevel * 10;

        yield return new WaitForSeconds(delay);

        var increment = (totalTime - delay) / randomCoins;
        int num;
        for (var i = 0; i < randomCoins; i = num + 1)
        {
            AudioManager.Instance.PlayOneShot("event:/followers/pop_in", follower.transform.position);
            ResourceCustomTarget.Create(PlayerFarming.Instance.gameObject, follower.transform.position, InventoryItem.ITEM_TYPE.BLACK_GOLD, null);
            yield return new WaitForSeconds(increment);
            num = i;
        }
        
        Inventory.AddItem(InventoryItem.ITEM_TYPE.BLACK_GOLD, randomCoins * 2);
    }
}