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
    /// Transpiler to replace the hardcoded age threshold (15) in the old age death check
    /// with our configurable value.
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
            var ageField = AccessTools.Field(typeof(StructuresData.Ranchable_Animal), nameof(StructuresData.Ranchable_Animal.Age));
            var found = false;

            // Looking for pattern: ldfld Age, followed by ldc.i4.s 15
            for (var i = 0; i < codes.Count - 1; i++)
            {
                if (codes[i].LoadsField(ageField) && IsLoadInt(codes[i + 1], 15))
                {
                    codes[i + 1] = new CodeInstruction(OpCodes.Call, getThresholdMethod).WithLabels(codes[i + 1].labels);
                    found = true;
                    Plugin.Log.LogInfo("[Transpiler] Structures_Ranch.OnNewPhaseStarted: Replaced animal old age threshold (15) with configurable value.");
                    break;
                }
            }

            if (!found)
            {
                Plugin.Log.LogWarning("[Transpiler] Structures_Ranch.OnNewPhaseStarted: Failed to find Age >= 15 pattern.");
                return original;
            }

            return codes;

            bool IsLoadInt(CodeInstruction instr, int value)
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
        catch (Exception ex)
        {
            Plugin.Log.LogWarning($"[Transpiler] Structures_Ranch.OnNewPhaseStarted: {ex.Message}");
            return original;
        }
    }
}
