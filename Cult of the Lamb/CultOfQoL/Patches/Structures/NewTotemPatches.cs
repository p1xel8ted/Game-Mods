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

        if (!Mathf.Approximately(__instance.RangeSprite.size.x, Structures_PropagandaSpeaker.EFFECTIVE_DISTANCE))
        {
            __instance.RangeSprite.size = Vector3.one * Structures_PropagandaSpeaker.EFFECTIVE_DISTANCE;
        }
    }


    [HarmonyPostfix, HarmonyPatch(typeof(FarmStation), nameof(FarmStation.Start))]
    [HarmonyPrefix, HarmonyPatch(typeof(FarmStation), nameof(FarmStation.Update))]
    public static void FarmStation_Patches(FarmStation __instance)
    {
        if (!Mathf.Approximately(__instance.RangeSprite.size.x, Plugin.FarmStationRange.Value))
        {
            __instance.RangeSprite.size = new Vector2(Plugin.FarmStationRange.Value, Plugin.FarmStationRange.Value);
        }
    }

    [HarmonyPostfix, HarmonyPatch(typeof(Interaction_FarmPlotSign), nameof(Interaction_FarmPlotSign.OnEnableInteraction))]
    [HarmonyPrefix, HarmonyPatch(typeof(Interaction_FarmPlotSign), nameof(Interaction_FarmPlotSign.Update))]
    public static void Interaction_FarmPlotSign_Patches(Interaction_FarmPlotSign __instance)
    {
        if (!Mathf.Approximately(__instance.RangeSprite.size.x, Plugin.FarmPlotSignRange.Value))
        {
            __instance.RangeSprite.size = new Vector2(Plugin.FarmPlotSignRange.Value, Plugin.FarmPlotSignRange.Value);
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Interaction_LightningRod), nameof(Interaction_LightningRod.OnEnableInteraction))]
    [HarmonyPatch(typeof(Interaction_LightningRod), nameof(Interaction_LightningRod.Update))]
    public static void Interaction_LightningRod_Patches(Interaction_LightningRod __instance)
    {
        Interaction_LightningRod.EFFECTIVE_DISTANCE_LVL1 = Plugin.LightningRodRangeLvl1.Value;
        Interaction_LightningRod.EFFECTIVE_DISTANCE_LVL2 = Plugin.LightningRodRangeLvl2.Value;

        // Structure is set in OnEnableInteraction, so it may be null during prefix
        if (__instance.Structure == null)
        {
            return;
        }

        // Determine which level this rod is
        var effectiveDistance = __instance.Brain?.Data?.Type == StructureBrain.TYPES.LIGHTNING_ROD
            ? Interaction_LightningRod.EFFECTIVE_DISTANCE_LVL1
            : Interaction_LightningRod.EFFECTIVE_DISTANCE_LVL2;

        if (!Mathf.Approximately(__instance.RangeSprite.size.x, effectiveDistance))
        {
            __instance.RangeSprite.size = new Vector2(effectiveDistance, effectiveDistance);
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

    [HarmonyTranspiler]
    [HarmonyPatch(typeof(Structures_FarmerStation), nameof(Structures_FarmerStation.GetNextUnwateredPlot))]
    [HarmonyPatch(typeof(Structures_FarmerStation), nameof(Structures_FarmerStation.GetNextUnseededPlot))]
    [HarmonyPatch(typeof(Structures_FarmerStation), nameof(Structures_FarmerStation.GetNextUnfertilizedPlot))]
    [HarmonyPatch(typeof(Structures_FarmerStation), nameof(Structures_FarmerStation.GetNextUnpickedPlot))]
    public static IEnumerable<CodeInstruction> Structures_FarmerStation_Range_Transpiler(IEnumerable<CodeInstruction> instructions, MethodBase original)
    {
        var originalCodes = instructions.ToList();
        try
        {
            var codes = new List<CodeInstruction>(originalCodes);
            var getRangeMethod = AccessTools.Method(typeof(NewTotemPatches), nameof(GetFarmStationRange));
            var found = false;

            for (var i = 0; i < codes.Count; i++)
            {
                if (codes[i].opcode == OpCodes.Ldc_R4 && codes[i].operand is float value && Mathf.Approximately(value, 6f))
                {
                    codes[i] = new CodeInstruction(OpCodes.Call, getRangeMethod).WithLabels(codes[i].labels);
                    found = true;
                }
            }

            if (!found)
            {
                Plugin.Log.LogWarning($"[Transpiler] Structures_FarmerStation.{original.Name}: Failed to find hardcoded range 6f.");
                return originalCodes;
            }

            Plugin.Log.LogInfo($"[Transpiler] Structures_FarmerStation.{original.Name}: Replaced hardcoded range with GetFarmStationRange().");
            return codes;
        }
        catch (Exception ex)
        {
            Plugin.Log.LogWarning($"[Transpiler] Structures_FarmerStation.{original.Name}: {ex.Message}");
            return originalCodes;
        }
    }

    [HarmonyTranspiler, HarmonyPatch(typeof(FarmStation), nameof(FarmStation.Update))]
    public static IEnumerable<CodeInstruction> FarmStation_Update_Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var original = instructions.ToList();
        try
        {
            var codes = new List<CodeInstruction>(original);
            var getRangeMethod = AccessTools.Method(typeof(NewTotemPatches), nameof(GetFarmStationRange));
            var found = false;

            for (var i = 0; i < codes.Count - 1; i++)
            {
                if (codes[i].opcode == OpCodes.Ldc_R4 &&
                    codes[i].operand is float value &&
                    Mathf.Approximately(value, 6f) &&
                    codes[i + 1].opcode == OpCodes.Stfld &&
                    codes[i + 1].operand is FieldInfo { Name: nameof(FarmStation.DistanceRadius) })
                {
                    codes[i] = new CodeInstruction(OpCodes.Call, getRangeMethod).WithLabels(codes[i].labels);
                    found = true;
                }
            }

            if (!found)
            {
                Plugin.Log.LogWarning("[Transpiler] FarmStation.Update: Failed to find DistanceRadius assignment.");
                return original;
            }

            Plugin.Log.LogInfo("[Transpiler] FarmStation.Update: Replaced DistanceRadius with GetFarmStationRange().");
            return codes;
        }
        catch (Exception ex)
        {
            Plugin.Log.LogWarning($"[Transpiler] FarmStation.Update: {ex.Message}");
            return original;
        }
    }


    [HarmonyTranspiler, HarmonyPatch(typeof(Interaction_FarmPlotSign), nameof(Interaction_FarmPlotSign.Update))]
    public static IEnumerable<CodeInstruction> Interaction_FarmPlotSign_Update_Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var original = instructions.ToList();
        try
        {
            var codes = new List<CodeInstruction>(original);
            var getRangeMethod = AccessTools.Method(typeof(NewTotemPatches), nameof(GetFarmPlotSignRange));
            var found = false;

            for (var i = 0; i < codes.Count - 1; i++)
            {
                if (codes[i].opcode == OpCodes.Ldc_R4 &&
                    codes[i].operand is float value &&
                    Mathf.Approximately(value, 5f) &&
                    codes[i + 1].opcode == OpCodes.Stfld &&
                    codes[i + 1].operand is FieldInfo { Name: nameof(Interaction_FarmPlotSign.DistanceRadius) })
                {
                    codes[i] = new CodeInstruction(OpCodes.Call, getRangeMethod).WithLabels(codes[i].labels);
                    found = true;
                }
            }

            if (!found)
            {
                Plugin.Log.LogWarning("[Transpiler] Interaction_FarmPlotSign.Update: Failed to find DistanceRadius assignment.");
                return original;
            }

            Plugin.Log.LogInfo("[Transpiler] Interaction_FarmPlotSign.Update: Replaced DistanceRadius with GetFarmPlotSignRange().");
            return codes;
        }
        catch (Exception ex)
        {
            Plugin.Log.LogWarning($"[Transpiler] Interaction_FarmPlotSign.Update: {ex.Message}");
            return original;
        }
    }
}