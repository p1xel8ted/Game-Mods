namespace CultOfQoL.Patches.Gameplay;

[HarmonyPatch]
public static class FishingPatches
{
    [HarmonyTranspiler]
    [HarmonyPatch(typeof(UIFishingOverlayController), nameof(UIFishingOverlayController.SetState))]
    public static IEnumerable<CodeInstruction> TranspilerOne(IEnumerable<CodeInstruction> instructions, MethodBase originalMethod)
    {
        if (Plugin.EasyFishing is {Value: false}) return instructions;

        var codes = new List<CodeInstruction>(instructions);
        for (var i = 0; i < codes.Count; i++)
        {
            if (!codes[i].LoadsField(AccessTools.Field(typeof(UIFishingOverlayController), nameof(UIFishingOverlayController.reelingCanvasGroup)))) continue;
            if (codes[i + 2].opcode == OpCodes.Callvirt) continue;
           Plugin.L($"Found {nameof(UIFishingOverlayController.reelingCanvasGroup)} at {i}");
            codes[i + 1].operand = 0f;
            codes[i + 2].operand = 0f;
        }
        return codes.AsEnumerable();
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(UIFishingOverlayController), nameof(UIFishingOverlayController.IsNeedleWithinSection))]
    public static void UIFishingOverlayController_IsNeedleWithinSection(ref bool __result)
    {
        if (Plugin.EasyFishing is {Value: false}) return;
        __result = true;
    }
}