namespace CultOfQoL.Patches.Systems.AutoCollect;

[Harmony]
public static class AutoCollectTranspilers
{
    [HarmonyTranspiler]
    [HarmonyPatch(typeof(LumberjackStation), nameof(LumberjackStation.Update))]
    [HarmonyPatch(typeof(Interaction_CollectResourceChest), nameof(Interaction_CollectResourceChest.Update))]
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, MethodBase originalMethod)
    {
        var original = instructions.ToList();
        try
        {
            var codes = new List<CodeInstruction>(original);
            var typeName = originalMethod.GetRealDeclaringType().Name;
            var isResourceChest = typeName.Contains("Interaction_CollectResourceChest");

            if (Plugin.EnableAutoCollect.Value && isResourceChest)
            {
                var activatingBlock = FindActivatingBlock(codes, originalMethod);
                if (activatingBlock.start != -1)
                {
                    RemoveCodeRange(codes, activatingBlock.start, activatingBlock.end - activatingBlock.start);
                }

                var delayBlock = FindDelayCheckBlock(codes, originalMethod);
                if (delayBlock.start != -1)
                {
                    RemoveCodeRange(codes, delayBlock.start, delayBlock.end - delayBlock.start);
                }
            }

            if (Plugin.FastCollecting.Value)
            {
                ReduceSpawnDelay(codes, originalMethod);
            }

            Plugin.Log.LogInfo($"[Transpiler] {typeName}.Update: Applied auto-collect/fast-collect patches.");
            return codes;
        }
        catch (Exception ex)
        {
            Plugin.Log.LogWarning($"[Transpiler] {originalMethod.GetRealDeclaringType().Name}.Update: {ex.Message}");
            return original;
        }
    }

    private static (int start, int end) FindDelayCheckBlock(List<CodeInstruction> codes, MethodBase originalMethod)
    {
        var updateMethod = AccessTools.Method(typeof(Interaction), nameof(Interaction.Update));
        var delayBetweenChecksField = AccessTools.Field(typeof(Interaction_CollectResourceChest), nameof(Interaction_CollectResourceChest.delayBetweenChecks));

        var start = -1;
        var end = -1;

        for (var i = 0; i < codes.Count; i++)
        {
            if (codes[i].Calls(updateMethod) && start == -1)
            {
                start = i + 1;
            }

            if (start != -1 && codes[i].StoresField(delayBetweenChecksField) && i > 0 && codes[i - 1].opcode == OpCodes.Ldc_R4)
            {
                end = i + 1;
                break;
            }
        }

        if (start == -1 || end == -1)
        {
            Plugin.Log.LogWarning($"[Transpiler] {originalMethod.GetRealDeclaringType().Name}: Failed to find delay check block.");
            return (-1, -1);
        }

        Plugin.Log.LogInfo($"[Transpiler] {originalMethod.GetRealDeclaringType().Name}: Found delay check block [{start}, {end}).");
        return (start, end);
    }

    private static (int start, int end) FindActivatingBlock(List<CodeInstruction> codes, MethodBase originalMethod)
    {
        var activatingField = AccessTools.Field(typeof(Interaction_CollectResourceChest), "Activating");

        var start = -1;
        var end = -1;

        for (var i = 0; i < codes.Count; i++)
        {
            if (codes[i].LoadsField(activatingField) && start == -1)
            {
                start = i - 1;
            }

            if (start != -1 && codes[i].StoresField(activatingField))
            {
                end = i + 1;
                break;
            }
        }

        if (start == -1 || end == -1)
        {
            Plugin.Log.LogWarning($"[Transpiler] {originalMethod.GetRealDeclaringType().Name}: Failed to find Activating block.");
            return (-1, -1);
        }

        Plugin.Log.LogInfo($"[Transpiler] {originalMethod.GetRealDeclaringType().Name}: Found Activating block [{start}, {end}).");
        return (start, end);
    }

    private static void ReduceSpawnDelay(List<CodeInstruction> codes, MethodBase originalMethod)
    {
        var delayField = AccessTools.Field(originalMethod.GetRealDeclaringType(), "Delay");
        var newDelay = originalMethod.GetRealDeclaringType().Name.Contains("Lumber") ? 0.025f : 0.01f;

        for (var i = 0; i < codes.Count - 1; i++)
        {
            if (codes[i].opcode == OpCodes.Ldc_R4 && codes[i + 1].StoresField(delayField))
            {
                Plugin.Log.LogInfo($"[Transpiler] {originalMethod.GetRealDeclaringType().Name}: Replacing Delay {codes[i].operand} with {newDelay}.");
                codes[i].operand = newDelay;
            }
        }
    }

    private static void RemoveCodeRange(List<CodeInstruction> codes, int startIndex, int count)
    {
        var labels = new List<Label>();
        for (var i = startIndex; i < startIndex + count; i++)
        {
            labels.AddRange(codes[i].labels);
        }

        codes.RemoveRange(startIndex, count);

        if (labels.Count > 0 && startIndex < codes.Count)
        {
            codes[startIndex].labels.AddRange(labels);
        }
    }
}
