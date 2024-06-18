namespace NoTimeForFishing;

[Harmony]
public static class Transpilers
{
    [HarmonyTranspiler]
    [HarmonyPatch(typeof(Fish), nameof(Fish.BiteRoutine), MethodType.Enumerator)]
    public static IEnumerable<CodeInstruction> Fish_BiteRoutine_Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var chanceMethod = AccessTools.Method(typeof(Wish.Utilities), nameof(Wish.Utilities.Chance));
        var codes = new List<CodeInstruction>(instructions);
        var foundMatchingSequence = false;

        foreach (var t in codes.Where(t => t.Calls(chanceMethod)))
        {
            foundMatchingSequence = true;

            t.operand = AccessTools.Method(typeof(TranspilerMethods), nameof(TranspilerMethods.ModifyNibbleChance));

            break;
        }

        if (!foundMatchingSequence)
        {
            Plugin.LOG.LogError($"Failed to find the matching opcode sequence in {nameof(Fish.BiteRoutine)}. Fish bite chance will not be modified.");
        }
        else
        {
            Plugin.LOG.LogInfo($"Found the matching opcode sequence in {nameof(Fish.BiteRoutine)}. Fish bite chance will be modified.");
        }

        return codes;
    }



    [HarmonyTranspiler]
    [HarmonyPatch(typeof(FishingRod), nameof(FishingRod.WinMiniGame))]
    public static IEnumerable<CodeInstruction> FishingRod_WinMiniGame_Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var chanceMethod = AccessTools.Method(typeof(Wish.Utilities), nameof(Wish.Utilities.Chance));
        var codes = new List<CodeInstruction>(instructions);
        var foundMatchingSequence = false;
        for (var i = 1; i < codes.Count; i++) // Start from index 1 since we check i-1
        {
            if (codes[i].Calls(chanceMethod))
            {
                foundMatchingSequence = true;
                codes[i].operand = AccessTools.Method(typeof(TranspilerMethods), nameof(TranspilerMethods.MuseumChance));
                break;
            }
        }

        if (!foundMatchingSequence)
        {
            Plugin.LOG.LogError($"Failed to find the matching opcode sequence in {nameof(FishingRod.WinMiniGame)}. Fish museum chance will not be modified.");
        }
        else
        {
            Plugin.LOG.LogInfo($"Found the matching opcode sequence in {nameof(FishingRod.WinMiniGame)}. Fish museum chance will modified.");
        }
        return codes;
    }


    [HarmonyTranspiler]
    [HarmonyPatch(typeof(Fish), nameof(Fish.TargetBobber))]
    public static IEnumerable<CodeInstruction> Fish_TargetBobber_Transpiler(IEnumerable<CodeInstruction> instructions,
        MethodBase originalMethod)
    {
        List<FieldInfo> colliders = [];
        colliders.Clear();
        var innerTypes = typeof(Fish).GetNestedTypes(AccessTools.all);
        var innerColliders = innerTypes.Where(type =>
            type.GetFields(AccessTools.all).Any(field => field.FieldType == typeof(Collider2D))).ToList();
        colliders.AddRange(innerColliders.SelectMany(type =>
            type.GetFields(AccessTools.all).Where(field => field.FieldType == typeof(Collider2D))));

        if (colliders.Count == 0)
        {
            Plugin.LOG.LogError(
                $"Failed to find any colliders in {originalMethod.Name}. Fish swim speed will not be modified.");
            return instructions.AsEnumerable();
        }

        var field = colliders[0];

        var codes = new List<CodeInstruction>(instructions);
        var foundMatchingSequence = false;

        for (var i = 0; i < codes.Count - 2; i++)
        {
            if (codes[i].opcode == OpCodes.Stfld &&
                (FieldInfo) codes[i].operand ==
                AccessTools.Field(typeof(Fish), nameof(Fish._targetBobber)) &&
                codes[i + 1].opcode == OpCodes.Ldarg_2)
            {
                foundMatchingSequence = true;

                var insertInstructions = new List<CodeInstruction>
                {
                    new(OpCodes.Ldarg_0),
                    new(OpCodes.Ldarg_0),
                    new(OpCodes.Ldfld, AccessTools.Field(typeof(Fish), nameof(Fish._pathMoveSpeed))),
                    new(OpCodes.Ldloc_0),
                    new(OpCodes.Ldfld, field),
                    new(OpCodes.Ldarg_2),
                    new(OpCodes.Call, AccessTools.Method(typeof(TranspilerMethods), nameof(TranspilerMethods.GetPathMoveSpeed))),
                    new(OpCodes.Stfld, AccessTools.Field(typeof(Fish), nameof(Fish._pathMoveSpeed))),
                };

                codes.InsertRange(i + 2, insertInstructions);
                break;
            }
        }

        if (foundMatchingSequence)
        {
            Plugin.LOG.LogInfo(
                $"Found the matching opcode sequence in {originalMethod.Name}. Fish swim speed will modified.");
        }
        else
        {
            Plugin.LOG.LogError(
                $"Failed to find the matching opcode sequence in {originalMethod.Name}. Fish swim speed will not be modified.");
        }


        return codes.AsEnumerable();
    }
}