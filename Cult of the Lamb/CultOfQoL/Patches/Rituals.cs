namespace CultOfQoL.Patches;

[Harmony]
public static class Rituals
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(UpgradeSystem), nameof(UpgradeSystem.AddCooldown))]
    public static void UpgradeSystem_AddCooldown_Prefix(UpgradeSystem.Type type, ref float duration)
    {
        var multiplier = Plugin.RitualCooldownTime.Value;
        var originalDuration = duration;
        var scaledDuration = originalDuration * multiplier;

        Plugin.L($"[UpgradeSystem_AddCooldown] Type: {type} | Original: {originalDuration:F2}s | Multiplier: {multiplier:F2} | New: {scaledDuration:F2}s");

        duration = scaledDuration;
    }


    public static int GetBossLimit()
    {
        return Plugin.SinBossLimit.Value;
    }

    [HarmonyTranspiler]
    [HarmonyPatch(typeof(Interaction_TempleAltar), nameof(Interaction_TempleAltar.TryOnboardSin))]
    [HarmonyPatch(typeof(Interaction_TempleAltar), nameof(Interaction_TempleAltar.TryOnboardSin), MethodType.Enumerator)]
    public static IEnumerable<CodeInstruction> Interaction_TempleAltar_TryOnboardSin_Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var originalCode = instructions.ToList();
        var modifiedCode = new List<CodeInstruction>(originalCode);

        try
        {
            var callMethod = AccessTools.Method(typeof(Rituals), nameof(GetBossLimit));

            for (var index = 0; index < modifiedCode.Count; index++)
            {
                if (modifiedCode[index].opcode == OpCodes.Ldc_I4_3)
                {
                    modifiedCode[index] = new CodeInstruction(OpCodes.Call, callMethod).WithLabels(modifiedCode[index].labels);
                    Plugin.Log.LogInfo("[Transpiler] Replaced hardcoded 3 in TryOnboardSin with GetBossLimit()");
                }
            }

            return modifiedCode;
        }
        catch (Exception ex)
        {
            Plugin.Log.LogError($"[Transpiler] Error in TryOnboardSin transpiler: {ex}");
            return originalCode;
        }
    }
}