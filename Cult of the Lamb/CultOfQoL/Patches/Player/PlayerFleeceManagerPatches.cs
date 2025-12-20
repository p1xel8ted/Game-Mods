namespace CultOfQoL.Patches.Player;

[Harmony]
public static class PlayerFleeceManagerPatches
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(PlayerFleeceManager), nameof(PlayerFleeceManager.IncrementDamageModifier))]
    public static bool PlayerFleeceManager_IncrementDamageModifier()
    {
        // Only patch if we're modifying Golden Fleece behavior
        if (!Plugin.ReverseGoldenFleeceDamageChange.Value && !Helpers.IsMultiplierActive(Plugin.BaseDamageMultiplier.Value)) return true;
    
        var playerFleece = DataManager.Instance.PlayerFleece;
        if (playerFleece != 1) return false; // Golden Fleece

        var baseIncrement =
            // Step 1: Determine base increment based on reverse nerf setting
            // Use old (pre-nerf) value : 0.1f if reversing nerf, else use new (post-nerf) value : 0.05f
            Plugin.ReverseGoldenFleeceDamageChange.Value ? 0.1f : 0.05f;

        // Step 2: Apply custom multiplier if active
        if (Helpers.IsMultiplierActive(Plugin.BaseDamageMultiplier.Value))
        {
            baseIncrement *= Plugin.BaseDamageMultiplier.Value;
        }
        
        PlayerFleeceManager.damageMultiplier += baseIncrement;
        PlayerFleeceManager.OnDamageMultiplierModified?.Invoke(PlayerFleeceManager.damageMultiplier);

        return false;
    }
}