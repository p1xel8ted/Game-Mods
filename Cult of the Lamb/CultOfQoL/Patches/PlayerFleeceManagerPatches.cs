namespace CultOfQoL.Patches;

[HarmonyPatch]
public static class PlayerFleeceManagerPatches
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(PlayerFleeceManager), nameof(PlayerFleeceManager.IncrementDamageModifier))]
    public static bool PlayerFleeceManager_IncrementDamageModifier()
    {
        if (!Plugin.ReverseGoldenFleeceDamageChange.Value && !Plugin.IncreaseGoldenFleeceDamageRate.Value && !Plugin.UseCustomDamageValue.Value) return true;

        var playerFleece = DataManager.Instance.PlayerFleece;
        if (playerFleece == 1)
        {
            if (Plugin.UseCustomDamageValue.Value)
            {
                PlayerFleeceManager.damageMultiplier += Mathf.Ceil(0.05f * Mathf.Abs(Plugin.CustomDamageMulti.Value));
            }
            else
            {
                if (Plugin.ReverseGoldenFleeceDamageChange.Value)
                    PlayerFleeceManager.damageMultiplier += Plugin.IncreaseGoldenFleeceDamageRate.Value ? 0.2f : 0.1f;
                else
                    PlayerFleeceManager.damageMultiplier += Plugin.IncreaseGoldenFleeceDamageRate.Value ? 0.1f : 0.05f;
            }
        }

        PlayerFleeceManager.OnDamageMultiplierModified?.Invoke(PlayerFleeceManager.damageMultiplier);
        return false;
    }
}