namespace CultOfQoL.Patches.Structures;

[Harmony]
public static class NewTotemPatches
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(HarvestTotem), nameof(HarvestTotem.Start))]
    [HarmonyPatch(typeof(HarvestTotem), nameof(HarvestTotem.Update))]
    public static void HarvestTotem_Patches(HarvestTotem __instance)
    {
        HarvestTotem.EFFECTIVE_DISTANCE = Plugin.HarvestTotemRange.Value;
        __instance.DistanceRadius = Plugin.HarvestTotemRange.Value;

        if (!Mathf.Approximately(__instance.RangeSprite.size.x, HarvestTotem.EFFECTIVE_DISTANCE))
        {
            __instance.RangeSprite.size = new Vector2(HarvestTotem.EFFECTIVE_DISTANCE, HarvestTotem.EFFECTIVE_DISTANCE);
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(PropagandaSpeaker), nameof(PropagandaSpeaker.Start))]
    [HarmonyPatch(typeof(PropagandaSpeaker), nameof(PropagandaSpeaker.Update))]
    public static void PropagandaSpeaker_Patches(PropagandaSpeaker __instance)
    {
        Structures_PropagandaSpeaker.EFFECTIVE_DISTANCE = Plugin.PropagandaSpeakerRange.Value;
        __instance.DistanceRadius = Plugin.PropagandaSpeakerRange.Value;

        if (!Mathf.Approximately(__instance.RangeSprite.size.x, Structures_PropagandaSpeaker.EFFECTIVE_DISTANCE))
        {
            __instance.RangeSprite.size = Vector3.one * Structures_PropagandaSpeaker.EFFECTIVE_DISTANCE;
        }
    }


    [HarmonyPostfix, HarmonyPatch(typeof(FarmStation), nameof(FarmStation.Start))]
    [HarmonyPrefix, HarmonyPatch(typeof(FarmStation), nameof(FarmStation.Update))]
    public static void FarmStation_Patches(FarmStation __instance)
    {
        //transpiler needed in Update to update 6 to Plugin.FarmStationRange.Value
        
        __instance.DistanceRadius = Plugin.FarmStationRange.Value;

        if (!Mathf.Approximately(__instance.RangeSprite.size.x, Plugin.FarmStationRange.Value))
        {
            __instance.RangeSprite.size = new Vector2(Plugin.FarmStationRange.Value, Plugin.FarmStationRange.Value);
        }
    }

    [HarmonyPostfix, HarmonyPatch(typeof(Interaction_FarmPlotSign), nameof(Interaction_FarmPlotSign.OnEnableInteraction))]
    [HarmonyPrefix, HarmonyPatch(typeof(Interaction_FarmPlotSign), nameof(Interaction_FarmPlotSign.Update))]
    public static void Interaction_FarmPlotSign_Patches(Interaction_FarmPlotSign __instance)
    {
        //transpiler needed in Update to update 5 to Plugin.FarmPlotRange.Value
        
        __instance.DistanceRadius = Plugin.FarmPlotSignRange.Value;

        if (!Mathf.Approximately(__instance.RangeSprite.size.x, Plugin.FarmPlotSignRange.Value))
        {
            __instance.RangeSprite.size = new Vector2(Plugin.FarmPlotSignRange.Value, Plugin.FarmPlotSignRange.Value);
        }
    }

    public static float GetFarmPlotSignRange()
    {
        return Plugin.FarmPlotSignRange.Value;
    }

    public static float GetFarmStationRange()
    {
        return Plugin.FarmStationRange.Value;
    }

    [HarmonyTranspiler, HarmonyPatch(typeof(FarmStation), nameof(FarmStation.Update))]
    public static IEnumerable<CodeInstruction> FarmStation_Update_Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var originalCode = instructions.ToList();
        var modifiedCode = new List<CodeInstruction>(originalCode);
   
        try
        {
            var getRangeMethod = AccessTools.Method(typeof(NewTotemPatches), nameof(GetFarmStationRange));

            for (var i = 0; i < modifiedCode.Count - 1; i++)
            {
                if (
                    modifiedCode[i].opcode == OpCodes.Ldc_R4 &&
                    modifiedCode[i].operand is float value &&
                    Mathf.Approximately(value, 6f) &&
                    modifiedCode[i + 1].opcode == OpCodes.Stfld &&
                    modifiedCode[i + 1].operand is FieldInfo { Name: nameof(FarmStation.DistanceRadius) })
                {
                    modifiedCode[i] = new CodeInstruction(OpCodes.Call, getRangeMethod).WithLabels(modifiedCode[i].labels);
                    Plugin.Log.LogInfo($"[Transpiler] Replaced hardcoded {value} in FarmStation.Update with GetFarmStationRange()");
                }
            }

            return modifiedCode;
        }
        catch (Exception ex)
        {
            Plugin.Log.LogError($"[Transpiler] Error in FarmStation transpiler: {ex}");
            return originalCode; 
        }
    }


    [HarmonyTranspiler, HarmonyPatch(typeof(Interaction_FarmPlotSign), nameof(Interaction_FarmPlotSign.Update))]
    public static IEnumerable<CodeInstruction> Interaction_FarmPlotSign_Update_Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var originalCode = instructions.ToList();
        var modifiedCode = new List<CodeInstruction>(originalCode);

        try
        {
            var getRangeMethod = AccessTools.Method(typeof(NewTotemPatches), nameof(GetFarmPlotSignRange));

            for (var i = 0; i < modifiedCode.Count - 1; i++)
            {
                if (
                    modifiedCode[i].opcode == OpCodes.Ldc_R4 &&
                    modifiedCode[i].operand is float value &&
                    Mathf.Approximately(value, 5f) &&
                    modifiedCode[i + 1].opcode == OpCodes.Stfld &&
                    modifiedCode[i + 1].operand is FieldInfo { Name: nameof(Interaction_FarmPlotSign.DistanceRadius) })
                {
                    modifiedCode[i] = new CodeInstruction(OpCodes.Call, getRangeMethod).WithLabels(modifiedCode[i].labels);
                    Plugin.Log.LogInfo($"[Transpiler] Replaced hardcoded {value} in Interaction_FarmPlotSign.Update with GetFarmPlotSignRange()");
                }
            }

            return modifiedCode;
        }
        catch (Exception ex)
        {
            Plugin.Log.LogError($"[Transpiler] Error in Interaction_FarmPlotSign transpiler: {ex}");
            return originalCode;
        }
    }
}