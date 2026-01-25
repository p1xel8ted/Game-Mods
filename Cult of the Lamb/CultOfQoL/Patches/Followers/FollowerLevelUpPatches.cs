namespace CultOfQoL.Patches.Followers;

[Harmony]
public static class FollowerLevelUpPatches
{
    // CanLevelUp: The game's implementation already allows leveling past 10 (just checks adoration >= max).
    // This patch ensures the behavior is consistent with our uncapped benefits feature.
    [HarmonyPostfix]
    [HarmonyPatch(typeof(FollowerBrain), nameof(FollowerBrain.CanLevelUp))]
    public static void FollowerBrain_CanLevelUp(ref FollowerBrain __instance, ref bool __result)
    {
        if (!Plugin.UncapLevelBenefits.Value) return;
        __result = __instance.Stats.Adoration >= __instance.Stats.MAX_ADORATION;
    }

    // IsMaxLevel: As of 1.5.0, this method is no longer called anywhere in the game code (dead code).
    // Kept for safety in case future updates re-introduce calls to this method or for older game versions.
    [HarmonyPostfix]
    [HarmonyPatch(typeof(FollowerBrain), nameof(FollowerBrain.IsMaxLevel))]
    public static void FollowerBrain_IsMaxLevel(ref bool __result)
    {
        if (!Plugin.UncapLevelBenefits.Value) return;
        __result = false;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(FollowerBrain), nameof(FollowerBrain.GetWillLevelUp))]
    public static void FollowerBrain_GetWillLevelUp(ref FollowerBrain __instance, ref FollowerBrain.AdorationActions Action, ref bool __result)
    {
        if (!Plugin.UncapLevelBenefits.Value) return;
        __result = __instance.Stats.Adoration + __instance.GetAddorationToAdd(Action) >= __instance.Stats.MAX_ADORATION;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(FollowerInformationBox), nameof(FollowerInformationBox.ConfigureImpl))]
    public static void FollowerInformationBox_ConfigureImpl(ref FollowerInformationBox __instance)
    {
        if (!Plugin.UncapLevelBenefits.Value) return;
        __instance._adorationContainer.gameObject.SetActive(true);
    }

    // ProductivityMultiplier: The game clamps XPLevel to 10 in the calculation.
    // This patch recalculates using the actual level for followers above level 10.
    [HarmonyPostfix]
    [HarmonyPatch(typeof(FollowerBrainInfo), nameof(FollowerBrainInfo.ProductivityMultiplier), MethodType.Getter)]
    public static void FollowerBrainInfo_ProductivityMultiplier(FollowerBrainInfo __instance, ref float __result)
    {
        if (!Plugin.UncapLevelBenefits.Value) return;
        if (__instance.XPLevel <= 10) return;

        // Original: (1 + (Clamp(XPLevel, 0.8, 10) - 1) / 5) * traitMultiplier
        // When XPLevel > 10, clamped value is 10, level factor = 2.8
        // Recalculate with actual XPLevel
        const float clampedFactor = (float)(1.0 + (10.0 - 1.0) / 5.0);
        var traitMultiplier = __result / clampedFactor;
        var unclampedFactor = (float)(1.0 + ((double)__instance.XPLevel - 1.0) / 5.0);
        __result = unclampedFactor * traitMultiplier;
    }

    [HarmonyTranspiler]
    [HarmonyPatch(typeof(FollowerTask_Pray), nameof(FollowerTask_Pray.SimDoingBegin))]
    [HarmonyPatch(typeof(FollowerTask_PrayPassive), nameof(FollowerTask_PrayPassive.DepositSoul))]
    public static IEnumerable<CodeInstruction> PrayerClampTranspiler(IEnumerable<CodeInstruction> instructions)
    {
        var original = instructions.ToList();
        if (!Plugin.UncapLevelBenefits.Value) return original;

        try
        {
            var codes = new List<CodeInstruction>(original);
            var clampMethod = AccessTools.Method(typeof(Mathf), nameof(Mathf.Clamp), [typeof(int), typeof(int), typeof(int)]);
            var maxMethod = AccessTools.Method(typeof(Mathf), nameof(Mathf.Max), [typeof(int), typeof(int)]);
            var found = false;

            for (var i = 0; i < codes.Count; i++)
            {
                if (!codes[i].Calls(clampMethod)) continue;

                for (var j = i - 1; j >= 0; j--)
                {
                    if ((codes[j].opcode == OpCodes.Ldc_I4_S && codes[j].operand is sbyte and 10) ||
                        (codes[j].opcode == OpCodes.Ldc_I4 && codes[j].operand is 10))
                    {
                        codes.RemoveAt(j);
                        i--;
                        break;
                    }
                }

                codes[i].operand = maxMethod;
                found = true;
                break;
            }

            if (!found)
            {
                Plugin.Log.LogWarning("[Transpiler] PrayerClampTranspiler: Failed to find Mathf.Clamp call.");
                return original;
            }

            Plugin.Log.LogInfo("[Transpiler] PrayerClampTranspiler: Replaced Mathf.Clamp with Mathf.Max (removed level cap).");
            return codes;
        }
        catch (Exception ex)
        {
            Plugin.Log.LogWarning($"[Transpiler] PrayerClampTranspiler: {ex.Message}");
            return original;
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(RitualSacrifice), nameof(RitualSacrifice.GetDevotionGain))]
    public static void RitualSacrifice_GetDevotionGain(int XPLevel, ref int __result)
    {
        if (!Plugin.UncapLevelBenefits.Value) return;
        __result = 40 + (Mathf.Max(XPLevel, 1) - 1) * 20;
    }

    public static float GetMaxLevel()
    {
        if (Plugin.UncapLevelBenefits.Value)
        {
            return float.PositiveInfinity;
        }
        return 10;
    }

}