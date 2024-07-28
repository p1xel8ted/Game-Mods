namespace ShowMeMoar;

[HarmonyPatch]
[HarmonyPriority(0)]
public static class FullscreenPatch
{
    public static int GetMaxRefreshRate()
    {
        if (Plugin.SetVsyncLimitToMaxRefreshRate.Value)
        {
            return (int) Plugin.MaxRefreshRate;   
        }
        
        return 60;
    }

    [HarmonyTranspiler]
    [HarmonyPatch(typeof(PlatformSpecific), nameof(PlatformSpecific.ApplyFullScreenMode))]
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var originalInstructions = instructions.ToList();
        foreach (var t in originalInstructions.Where(t => t.opcode == OpCodes.Ldc_I4_S && t.operand is sbyte and 60))
        {
            t.opcode = OpCodes.Call;
            t.operand = typeof(FullscreenPatch).GetMethod(nameof(GetMaxRefreshRate));
        }
        return originalInstructions.AsEnumerable();
    }
}