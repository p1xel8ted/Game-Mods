namespace UIScales;

[Harmony]
public static class Transpilers
{

    [HarmonyTranspiler]
    [HarmonyPatch(typeof(PlayerSettings), nameof(PlayerSettings.SetZoomLevel))]
    [HarmonyPatch(typeof(PlayerSettings), nameof(PlayerSettings.SetupUI))]
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

}