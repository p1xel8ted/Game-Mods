namespace TheSeedEqualizer;

[Harmony]
public static class Patches
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameBalance), nameof(GameBalance.LoadGameBalance))]
    public static void GameBalance_LoadGameBalance()
    {
        Helpers.GameBalancePostfix();
    }

    [HarmonyPrefix]
    [HarmonyBefore("p1xel8ted.GraveyardKeeper.FasterCraftReloaded")]
    [HarmonyPriority(1)]
    [HarmonyPatch(typeof(CraftComponent), nameof(CraftComponent.ReallyUpdateComponent))]
    public static void CraftComponent_ReallyUpdateComponent(CraftComponent __instance, ref float delta_time)
    {
        if (__instance?.current_craft == null) return;
        if (!Plugin.BoostGrowSpeedWhenRaining.Value)
        {
            return;
        }
        if (!EnvironmentEngine.me.is_rainy)
        {
            return;
        }

        var craftId = __instance.current_craft.id;
        string[] refugee = ["garden", "planting", "refugee", "grow"];
        var isRefugeePlanting = refugee.All(craftId.Contains);
        var isVineyard = craftId.Contains("vineyard");
        var isPlayerGarden = craftId.StartsWith("garden") && craftId.EndsWith("growing");

        if (isRefugeePlanting || isVineyard || isPlayerGarden)
        {
            if (Plugin.DebugEnabled)
            {
                Helpers.Log($"[RainBoost] doubling delta_time for craft '{craftId}' (refugee={isRefugeePlanting}, vineyard={isVineyard}, playerGarden={isPlayerGarden}, before={delta_time:F3})");
            }
            delta_time *= 2f;
        }
        else
        {
            if (Plugin.DebugEnabled)
            {
                Helpers.Log($"[RainBoost] skip craft '{craftId}': not a recognised garden/vineyard/refugee planting craft");
            }
        }
    }
}
