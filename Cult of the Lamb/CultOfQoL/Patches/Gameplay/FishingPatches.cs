namespace CultOfQoL.Patches.Gameplay;

[Harmony]
public static class FishingPatches
{
    private static readonly FieldInfo ReelingCanvasGroupField = AccessTools.Field(typeof(UIFishingOverlayController), nameof(UIFishingOverlayController.reelingCanvasGroup));
    
    [HarmonyTranspiler]
    [HarmonyPatch(typeof(UIFishingOverlayController), nameof(UIFishingOverlayController.SetState))]
    public static IEnumerable<CodeInstruction> UIFishingOverlayController_SetState_Transpiler(IEnumerable<CodeInstruction> instructions, MethodBase originalMethod)
    {
        
        var originalCode = instructions.ToList();
        var modifiedCode = new List<CodeInstruction>(originalCode);
        
        if (!Plugin.EasyFishing.Value) return originalCode;

        try
        {
            for (var i = 0; i < modifiedCode.Count - 2; i++)
            {

                if (modifiedCode[i].LoadsField(ReelingCanvasGroupField) && modifiedCode[i + 2].opcode != OpCodes.Callvirt)
                {
                    modifiedCode[i + 1].operand = 0f;
                    modifiedCode[i + 2].operand = 0f;
                    
                    Plugin.Log.LogInfo($"Patched {nameof(UIFishingOverlayController.reelingCanvasGroup)} at index {i}");   
                }
            }
            
            return modifiedCode.AsEnumerable();
        }
        catch (Exception ex)
        {
            Plugin.Log.LogError($"[Transpiler] Error in FollowerInfo.NewCharacter transpiler: {ex}");
            return originalCode;
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