namespace CultOfQoL.Patches;

[HarmonyPatch]
internal static class StructurePatches
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(Structures_LumberjackStation), nameof(Structures_LumberjackStation.LifeSpawn), MethodType.Getter)]
    public static void Structures_LumberjackStation_LifeSpawn(Structures_LumberjackStation __instance, ref int __result)
    {
        if (Plugin.LumberAndMiningStationsDontAge.Value)
        {
            return;
        }

        var old = __result;
        int newSpan;

        if (Plugin.DoubleLifespanInstead.Value)
        {
            newSpan = old * 2;
            __result = newSpan;
            Plugin.L($"DOUBLE: Lumber/mining old lifespan {old}, new lifespan: {newSpan}. Current age: {__instance.Data.Age}.");
            return;
        }

        if (!Plugin.FiftyPercentIncreaseToLifespanInstead.Value) return;

        newSpan = (int) (old * 1.5f);
        __result = newSpan;
        Plugin.L($"50%: Lumber/mining old lifespan {old}, new lifespan: {newSpan}. Current age: {__instance.Data.Age}.");
    }


    [HarmonyPatch(typeof(PropagandaSpeaker), nameof(PropagandaSpeaker.Update))]
    public static class PropagandaSpeakerPatches
    {
        [HarmonyPrefix]
        public static bool Prefix()
        {
            if (!Plugin.TurnOffSpeakersAtNight.Value) return true;
            return !TimeManager.IsNight;
        }

        [HarmonyPostfix]
        public static void Postfix(ref PropagandaSpeaker __instance)
        {
            if (!Plugin.DisablePropagandaSpeakerAudio.Value) return;
            if (!__instance.VOPlaying) return;
            AudioManager.Instance.StopLoop(__instance.loopedInstance);
            __instance.VOPlaying = false;
        }
    }

    //stop fuel being taken when speakers are off
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Structures_PropagandaSpeaker), nameof(Structures_PropagandaSpeaker.OnNewPhaseStarted))]
    public static bool Structures_PropagandaSpeaker_OnNewPhaseStarted()
    {
        if (!Plugin.TurnOffSpeakersAtNight.Value) return true;
        return !TimeManager.IsNight;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(TimeManager), nameof(TimeManager.StartNewPhase))]
    public static void TimeManager_StartNewPhase(DayPhase phase)
    {
        if (!Plugin.TurnOffSpeakersAtNight.Value) return;

        if (phase != DayPhase.Night) return;

        var structures = Object.FindObjectsOfType<PropagandaSpeaker>();
        foreach (var structure in structures)
        {
            structure.StopAllCoroutines();
            structure.Spine.AnimationState.SetAnimation(0, "off", true);
            var fireOff = structure.onFireOff;
            fireOff?.Invoke();
            AudioManager.Instance.StopLoop(structure.loopedInstance);
            structure.VOPlaying = false;
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Structures_LumberjackStation), nameof(Structures_LumberjackStation.IncreaseAge))]
    public static void Structures_LumberjackStation_IncreaseAge(ref Structures_LumberjackStation __instance)
    {
        if (!Plugin.LumberAndMiningStationsDontAge.Value) return;

        __instance.Data.Age = 0;
        Plugin.L("Resetting age of lumber/mining station to 0!");
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(StructureBrain), nameof(StructureBrain.SoulMax), MethodType.Getter)]
    public static void Structures_Bed_Constructor(StructureBrain __instance, ref int __result)
    {
        if (__instance is not Structures_Bed) return;
        if (Plugin.UseCustomSoulCapacity.Value)
        {
            __result = Mathf.CeilToInt(__result * Mathf.Abs(Plugin.CustomSoulCapacityMulti.Value));
            return;
        }

        if (Plugin.DoubleSoulCapacity.Value)
        {
            __result *= 2;
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Structures_Shrine), nameof(Structures_Shrine.SoulMax), MethodType.Getter)]
    public static void Structures_Shrine_SoulMax(ref int __result)
    {
        if (Plugin.UseCustomSoulCapacity.Value)
        {
            __result = Mathf.CeilToInt(__result * Mathf.Abs(Plugin.CustomSoulCapacityMulti.Value));
            return;
        }

        if (Plugin.DoubleSoulCapacity.Value)
        {
            __result *= 2;
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Structures_Shrine_Misfit), nameof(Structures_Shrine_Misfit.SoulMax), MethodType.Getter)]
    public static void Structures_Shrine_Misfit_SoulMax(ref int __result)
    {
        if (Plugin.UseCustomSoulCapacity.Value)
        {
            __result = Mathf.CeilToInt(__result * Mathf.Abs(Plugin.CustomSoulCapacityMulti.Value));
            return;
        }

        if (Plugin.DoubleSoulCapacity.Value)
        {
            __result *= 2;
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Structures_Shrine_Ratau), nameof(Structures_Shrine_Ratau.SoulMax), MethodType.Getter)]
    public static void Structures_Shrine_Ratau_SoulMax(ref int __result)
    {
        if (Plugin.UseCustomSoulCapacity.Value)
        {
            __result = Mathf.CeilToInt(__result * Mathf.Abs(Plugin.CustomSoulCapacityMulti.Value));
            return;
        }


        if (Plugin.DoubleSoulCapacity.Value)
        {
            __result *= 2;
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Structures_Shrine_Passive), nameof(Structures_Shrine_Passive.SoulMax), MethodType.Getter)]
    public static void Structures_Shrine_Passive_SoulMax(ref int __result)
    {
        if (Plugin.UseCustomSoulCapacity.Value)
        {
            __result = Mathf.CeilToInt(__result * Mathf.Abs(Plugin.CustomSoulCapacityMulti.Value));
            return;
        }

        if (Plugin.DoubleSoulCapacity.Value)
        {
            __result *= 2;
        }
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(Structures_SiloSeed), nameof(Structures_SiloSeed.Capacity), MethodType.Getter)]
    [HarmonyPatch(typeof(Structures_SiloFertiliser), nameof(Structures_SiloFertiliser.Capacity), MethodType.Getter)]
    public static void Structures_Capacity(ref StructureBrain __instance, ref float __result)
    {
        // public float Capacity => this.Data.Type == StructureBrain.TYPES.SEED_BUCKET ? 250f : 15f;
        //  public float Capacity => this.Data.Type == StructureBrain.TYPES.POOP_BUCKET ? 250f : 15f;

        //250 becomes 256, 15 becomes 32 if Plugin.UseMultiplesOf32.Value is true
        __result = MakeMultipleOf32(__result);

        if (Plugin.DoubleSiloCapacity.Value)
        {
            __result = MakeMultipleOf32(__result *= 2);
            return;
        }

        if (Plugin.UseCustomSiloCapacity.Value)
        {
            __result = MakeMultipleOf32(Mathf.Ceil(__result * Mathf.Abs(Plugin.CustomSiloCapacityMulti.Value)));
        }
    }

    private static float MakeMultipleOf32(float value)
    {
        if (!Plugin.UseMultiplesOf32.Value)
        {
            return value;
        }
        return Mathf.Ceil(value / 32f) * 32f;
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(Structures_Refinery), nameof(Structures_Refinery.GetCost), typeof(InventoryItem.ITEM_TYPE))]
    public static void Structures_Refinery_GetCost(ref List<StructuresData.ItemCost> __result)
    {
        if (!Plugin.AdjustRefineryRequirements.Value) return;
        foreach (var item in __result)
        {
            item.CostValue = Mathf.CeilToInt(item.CostValue / 2f);
        }
    }

}