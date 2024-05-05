namespace CultOfQoL.Patches;

[Harmony]
public static class FollowerLevelUpPatches
{

    [HarmonyPostfix]
    [HarmonyPatch(typeof(FollowerBrain), nameof(FollowerBrain.CanLevelUp))]
    public static void FollowerBrain_CanLevelUp(ref FollowerBrain __instance, ref bool __result)
    {
        if (!Plugin.RemoveLevelLimit.Value) return;
        __result = __instance.Stats.Adoration >= __instance.Stats.MAX_ADORATION;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(FollowerBrain), nameof(FollowerBrain.IsMaxLevel))]
    public static void FollowerBrain_IsMaxLevel(ref bool __result)
    {
        if (!Plugin.RemoveLevelLimit.Value) return;
        __result = false;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(FollowerBrainInfo), nameof(FollowerBrainInfo.MaxLevelReached), MethodType.Getter)]
    public static void FollowerBrainInfo_MaxLevelReached_Getter(ref bool __result)
    {
        if (!Plugin.RemoveLevelLimit.Value) return;
        __result = false;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(FollowerBrainInfo), nameof(FollowerBrainInfo.MaxLevelReached), MethodType.Setter)]
    public static void FollowerBrainInfo_MaxLevelReached_Setter(ref bool value)
    {
        if (!Plugin.RemoveLevelLimit.Value) return;
        value = false;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(FollowerBrain), nameof(FollowerBrain.GetWillLevelUp))]
    public static void FollowerBrain_GetWillLevelUp(ref FollowerBrain __instance, ref FollowerBrain.AdorationActions Action, ref bool __result)
    {
        if (!Plugin.RemoveLevelLimit.Value) return;
        __result = __instance.Stats.Adoration + __instance.GetAddorationToAdd(Action) >= __instance.Stats.MAX_ADORATION;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(FollowerInformationBox), nameof(FollowerInformationBox.ConfigureImpl))]
    public static void FollowerInformationBox_ConfigureImpl(ref FollowerInformationBox __instance)
    {
        if (!Plugin.RemoveLevelLimit.Value) return;
        __instance._adorationContainer.gameObject.SetActive(true);
    }

    public static float GetMaxLevel()
    {
        if (Plugin.RemoveLevelLimit.Value)
        {
            return float.PositiveInfinity;
        }
        return 10;
    }

    [HarmonyTranspiler]
    [HarmonyPatch(typeof(FollowerBrain), nameof(FollowerBrain.AddAdoration), typeof(Follower), typeof(FollowerBrain.AdorationActions), typeof(Action))]
    public static IEnumerable<CodeInstruction> FollowerBrain_AddAdoration(IEnumerable<CodeInstruction> instructions, MethodBase originalMethod)
    {
        if (!Plugin.RemoveLevelLimit.Value) return instructions;

        var getXpLevel = AccessTools.Property(typeof(FollowerBrainInfo), nameof(FollowerBrainInfo.XPLevel)).GetGetMethod();
        var codes = new List<CodeInstruction>(instructions);

        for (var i = 0; i < codes.Count; i++)
        {
            if (!codes[i].Calls(getXpLevel)) continue;

            Plugin.L($"{originalMethod.Name}: Found XPLevel property at {i}. Replacing 10 with call to GetMaxLevel().");
            codes.Insert(i + 1, new CodeInstruction(OpCodes.Conv_R4));
            codes[i + 2].opcode = OpCodes.Call;
            codes[i + 2].operand = AccessTools.Method(typeof(FollowerLevelUpPatches), nameof(GetMaxLevel));
        }

        return codes;
    }
}