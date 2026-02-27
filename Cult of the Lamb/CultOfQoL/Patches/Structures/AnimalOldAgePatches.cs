namespace CultOfQoL.Patches.Structures;

[Harmony]
public static class AnimalOldAgePatches
{
    /// <summary>
    /// Returns the minimum age threshold for animal old age death.
    /// If disabled entirely, returns int.MaxValue to prevent any animal from meeting the threshold.
    /// </summary>
    public static int GetAnimalOldAgeThreshold()
    {
        if (Plugin.DisableAnimalOldAgeDeath.Value)
        {
            return int.MaxValue;
        }

        return Plugin.AnimalOldAgeDeathThreshold.Value;
    }

    /// <summary>
    /// Returns the guaranteed death age as a float for use in the probability formula.
    /// If disabled entirely, returns float.MaxValue to make the probability effectively zero.
    /// The game uses float (Single) arithmetic for the death probability, not double.
    /// </summary>
    public static float GetAnimalGuaranteedDeathAge()
    {
        if (Plugin.DisableAnimalOldAgeDeath.Value)
        {
            return float.MaxValue;
        }

        return Plugin.AnimalGuaranteedDeathAge.Value;
    }

    /// <summary>
    /// Transpiler for Structures_Ranch.OnNewPhaseStarted:
    /// 1. Replaces the hardcoded age threshold (15) with our configurable value.
    /// 2. Replaces the hardcoded death probability divisor (ldc.r4 100) with our configurable value.
    /// </summary>
    [HarmonyTranspiler]
    [HarmonyPatch(typeof(Structures_Ranch), nameof(Structures_Ranch.OnNewPhaseStarted))]
    public static IEnumerable<CodeInstruction> Structures_Ranch_OnNewPhaseStarted_Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var original = instructions.ToList();
        try
        {
            var codes = new List<CodeInstruction>(original);
            var getThresholdMethod = AccessTools.Method(typeof(AnimalOldAgePatches), nameof(GetAnimalOldAgeThreshold));
            var getDeathAgeMethod = AccessTools.Method(typeof(AnimalOldAgePatches), nameof(GetAnimalGuaranteedDeathAge));
            var ageField = AccessTools.Field(typeof(StructuresData.Ranchable_Animal), nameof(StructuresData.Ranchable_Animal.Age));
            var foundThreshold = false;
            var foundDivisor = false;

            for (var i = 0; i < codes.Count - 1; i++)
            {
                // Pattern 1: ldfld Age, followed by ldc.i4.s 15 → replace 15 with configurable threshold
                if (!foundThreshold && codes[i].LoadsField(ageField) && IsLoadInt(codes[i + 1], 15))
                {
                    codes[i + 1] = new CodeInstruction(OpCodes.Call, getThresholdMethod).WithLabels(codes[i + 1].labels);
                    foundThreshold = true;
                    Plugin.Log.LogInfo("[Transpiler] Structures_Ranch.OnNewPhaseStarted: Replaced animal old age threshold (15) with configurable value.");
                }

                // Pattern 2: ldc.r4 100f → replace with configurable guaranteed death age
                if (!foundDivisor && codes[i].opcode == OpCodes.Ldc_R4 && codes[i].operand is float fv && Math.Abs(fv - 100f) < 0.001f)
                {
                    codes[i] = new CodeInstruction(OpCodes.Call, getDeathAgeMethod).WithLabels(codes[i].labels);
                    foundDivisor = true;
                    Plugin.Log.LogInfo("[Transpiler] Structures_Ranch.OnNewPhaseStarted: Replaced death probability divisor (100f) with configurable value.");
                }

                if (foundThreshold && foundDivisor) break;
            }

            if (!foundThreshold)
            {
                Plugin.Log.LogWarning("[Transpiler] Structures_Ranch.OnNewPhaseStarted: Failed to find Age >= 15 pattern.");
            }

            if (!foundDivisor)
            {
                Plugin.Log.LogWarning("[Transpiler] Structures_Ranch.OnNewPhaseStarted: Failed to find ldc.r4 100 divisor pattern.");
            }

            return !foundThreshold || !foundDivisor ? original : codes;
        }
        catch (Exception ex)
        {
            Plugin.Log.LogWarning($"[Transpiler] Structures_Ranch.OnNewPhaseStarted: {ex.Message}");
            return original;
        }
    }

    /// <summary>
    /// Transpiler for Interaction_Ranchable.OnNewDayStarted:
    /// Replaces the hardcoded age threshold (15) with our configurable value.
    /// This ensures both death paths respect the configurable threshold.
    /// </summary>
    [HarmonyTranspiler]
    [HarmonyPatch(typeof(Interaction_Ranchable), nameof(Interaction_Ranchable.OnNewDayStarted))]
    public static IEnumerable<CodeInstruction> Interaction_Ranchable_OnNewDayStarted_Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var original = instructions.ToList();
        try
        {
            var codes = new List<CodeInstruction>(original);
            var getThresholdMethod = AccessTools.Method(typeof(AnimalOldAgePatches), nameof(GetAnimalOldAgeThreshold));
            var ageField = AccessTools.Field(typeof(StructuresData.Ranchable_Animal), nameof(StructuresData.Ranchable_Animal.Age));
            var found = false;

            for (var i = 0; i < codes.Count - 1; i++)
            {
                if (codes[i].LoadsField(ageField) && IsLoadInt(codes[i + 1], 15))
                {
                    codes[i + 1] = new CodeInstruction(OpCodes.Call, getThresholdMethod).WithLabels(codes[i + 1].labels);
                    found = true;
                    Plugin.Log.LogInfo("[Transpiler] Interaction_Ranchable.OnNewDayStarted: Replaced animal old age threshold (15) with configurable value.");
                    break;
                }
            }

            if (!found)
            {
                Plugin.Log.LogWarning("[Transpiler] Interaction_Ranchable.OnNewDayStarted: Failed to find Age < 15 pattern.");
                return original;
            }

            return codes;
        }
        catch (Exception ex)
        {
            Plugin.Log.LogWarning($"[Transpiler] Interaction_Ranchable.OnNewDayStarted: {ex.Message}");
            return original;
        }
    }

    /// <summary>
    /// Transpiler for Interaction_Ranchable.CheckIfItsTimeToDie:
    /// Fixes the vanilla integer division bug (Age / 100 instead of Age / 100f) and
    /// replaces the hardcoded divisor with our configurable guaranteed death age.
    ///
    /// Original IL: ldfld Age, ldc.i4.s 100, div, conv.r4  (integer division then cast to float)
    /// Patched IL:  ldfld Age, conv.r4, call GetAnimalGuaranteedDeathAge, div  (float division)
    /// </summary>
    [HarmonyTranspiler]
    [HarmonyPatch(typeof(Interaction_Ranchable), nameof(Interaction_Ranchable.CheckIfItsTimeToDie))]
    public static IEnumerable<CodeInstruction> Interaction_Ranchable_CheckIfItsTimeToDie_Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var original = instructions.ToList();
        try
        {
            var codes = new List<CodeInstruction>(original);
            var getDeathAgeMethod = AccessTools.Method(typeof(AnimalOldAgePatches), nameof(GetAnimalGuaranteedDeathAge));
            var ageField = AccessTools.Field(typeof(StructuresData.Ranchable_Animal), nameof(StructuresData.Ranchable_Animal.Age));
            var found = false;

            // Looking for pattern: ldfld Age, ldc.i4(.s) 100, div, conv.r4
            for (var i = 0; i < codes.Count - 3; i++)
            {
                if (codes[i].LoadsField(ageField) &&
                    IsLoadInt(codes[i + 1], 100) &&
                    codes[i + 2].opcode == OpCodes.Div &&
                    codes[i + 3].opcode == OpCodes.Conv_R4)
                {
                    // Transform: ldc.i4.s 100 → conv.r4 (convert Age to float first)
                    codes[i + 1] = new CodeInstruction(OpCodes.Conv_R4).WithLabels(codes[i + 1].labels);
                    // Transform: div → call GetAnimalGuaranteedDeathAge (push float divisor)
                    codes[i + 2] = new CodeInstruction(OpCodes.Call, getDeathAgeMethod).WithLabels(codes[i + 2].labels);
                    // Transform: conv.r4 → div (floating-point division)
                    codes[i + 3] = new CodeInstruction(OpCodes.Div).WithLabels(codes[i + 3].labels);
                    found = true;
                    Plugin.Log.LogInfo("[Transpiler] Interaction_Ranchable.CheckIfItsTimeToDie: Fixed integer division and replaced divisor (100) with configurable value.");
                    break;
                }
            }

            if (!found)
            {
                Plugin.Log.LogWarning("[Transpiler] Interaction_Ranchable.CheckIfItsTimeToDie: Failed to find Age / 100 integer division pattern.");
                return original;
            }

            return codes;
        }
        catch (Exception ex)
        {
            Plugin.Log.LogWarning($"[Transpiler] Interaction_Ranchable.CheckIfItsTimeToDie: {ex.Message}");
            return original;
        }
    }

    private static bool IsLoadInt(CodeInstruction instr, int value)
    {
        if (instr.opcode == OpCodes.Ldc_I4_S && instr.operand is sbyte sb)
        {
            return sb == (sbyte)value;
        }

        if (instr.opcode == OpCodes.Ldc_I4 && instr.operand is int iv)
        {
            return iv == value;
        }

        return false;
    }
}
