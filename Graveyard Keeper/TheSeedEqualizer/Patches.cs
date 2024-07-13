namespace TheSeedEqualizer;

[Harmony]
public static class Patches
{
    [HarmonyPrefix]
    [HarmonyBefore("p1xel8ted.GraveyardKeeper.FasterCraftReloaded")]
    [HarmonyPriority(1)]
    [HarmonyPatch(typeof(CraftComponent), nameof(CraftComponent.ReallyUpdateComponent))]
    public static void CraftComponent_ReallyUpdateComponent(CraftComponent __instance, ref float delta_time)
    {
        if (__instance?.current_craft == null) return;
        if (!Plugin.BoostGrowSpeedWhenRaining.Value) return;

        if (!EnvironmentEngine.me.is_rainy) return;
        string[] refugee = ["garden", "planting", "refugee", "grow"];
        if (refugee.All(a => __instance.current_craft.id.Contains(a)) || __instance.current_craft.id.Contains("vineyard") || (__instance.current_craft.id.StartsWith("garden") && __instance.current_craft.id.EndsWith("growing")))
        {
            Plugin.Log.LogInfo($"It's raining! Boosting base grow speed of {__instance.current_craft.id} by 100%!");

            delta_time *= 2f;
        }
    }
}