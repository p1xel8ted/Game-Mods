namespace CultOfQoL.Patches.Gameplay;

[Harmony]
public static class FishingPatches
{
    private static readonly FieldInfo ReelingCanvasGroupField = AccessTools.Field(typeof(UIFishingOverlayController), nameof(UIFishingOverlayController.reelingCanvasGroup));
    
    [HarmonyTranspiler]
    [HarmonyPatch(typeof(UIFishingOverlayController), nameof(UIFishingOverlayController.SetState))]
    public static IEnumerable<CodeInstruction> UIFishingOverlayController_SetState_Transpiler(IEnumerable<CodeInstruction> instructions, MethodBase originalMethod)
    {
        var original = instructions.ToList();
        if (!Plugin.EasyFishing.Value) return original;

        try
        {
            var codes = new List<CodeInstruction>(original);
            var found = false;

            for (var i = 0; i < codes.Count - 2; i++)
            {
                if (codes[i].LoadsField(ReelingCanvasGroupField) && codes[i + 2].opcode != OpCodes.Callvirt)
                {
                    codes[i + 1].operand = 0f;
                    codes[i + 2].operand = 0f;
                    found = true;
                }
            }

            if (!found)
            {
                Plugin.Log.LogWarning("[Transpiler] UIFishingOverlayController.SetState: Failed to find reelingCanvasGroup field loads.");
                return original;
            }

            Plugin.Log.LogInfo("[Transpiler] UIFishingOverlayController.SetState: Zeroed reeling canvas group values.");
            return codes;
        }
        catch (Exception ex)
        {
            Plugin.Log.LogWarning($"[Transpiler] UIFishingOverlayController.SetState: {ex.Message}");
            return original;
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(UIFishingOverlayController), nameof(UIFishingOverlayController.IsNeedleWithinSection))]
    public static void UIFishingOverlayController_IsNeedleWithinSection(ref bool __result)
    {
        if (!Plugin.EasyFishing.Value) return;
        
        __result = true; 
    }
}