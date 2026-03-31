namespace UIScales;

[Harmony]
public static class Transpilers
{
    [HarmonyTranspiler]
    [HarmonyPatch(typeof(PlayerSettings), nameof(PlayerSettings.SetZoomLevel))]
    public static IEnumerable<CodeInstruction> PlayerSettings_SetZoomLevel_Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var codes = new List<CodeInstruction>(instructions);
        foreach (var code in codes)
        {
            if (code.operand is 2f)
            {
                code.operand = 0.5f;
            }
            if (code.operand is 4f)
            {
                code.operand = 10f;
            }
        }
        return codes;
    }

    [HarmonyTranspiler]
    [HarmonyPatch(typeof(PlayerSettings), nameof(PlayerSettings.SetupUI))]
    public static IEnumerable<CodeInstruction> PlayerSettings_SetupUI_Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var codes = new List<CodeInstruction>(instructions);

        // Only replace the 4f/2f pair that appears together for the zoom InverseLerp.
        // The pattern is: ldc.r4 4f, ldc.r4 2f (consecutive zoom bounds for InverseLerp(4f, 2f, zoom))
        // We must NOT touch the day speed InverseLerp(1f, 4f, daySpeed) where 4f appears alone.
        for (var i = 0; i < codes.Count - 1; i++)
        {
            if (codes[i].operand is 4f && codes[i + 1].operand is 2f)
            {
                codes[i].operand = 10f;
                codes[i + 1].operand = 0.5f;
                break;
            }
        }

        return codes;
    }
}
