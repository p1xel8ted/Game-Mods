namespace LongerDays;

[Harmony]
public static class Patches
{
    internal static float GetTimeMulti()
    {
        return Plugin.Seconds switch
        {
            Plugin.DefaultIncreaseSeconds => 1.5f,
            Plugin.DoubleLengthSeconds => 2f,
            Plugin.EvenLongerSeconds => 2.5f,
            Plugin.MadnessSeconds => 3f,
            _ => 1f
        };
    }

    public static float GetTime()
    {
        return Time.deltaTime / GetTimeMulti();
    }

    [HarmonyTranspiler]
    [HarmonyPatch(typeof(EnvironmentEngine), nameof(EnvironmentEngine.Update))]
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var time = AccessTools.Property(typeof(Time), nameof(Time.deltaTime)).GetGetMethod();

        foreach (var instruction in instructions)
        {
            if (instruction.opcode == OpCodes.Call && instruction.OperandIs(time))
            {
                instruction.operand = AccessTools.Method(typeof(Patches), nameof(GetTime));
            }
            yield return instruction;
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(TimeOfDay), nameof(TimeOfDay.FromTimeKToSeconds))]
    public static void TimeOfDay_FromTimeKToSeconds(float time_in_time_k, ref float __result)
    {
        __result = time_in_time_k * Plugin.Seconds;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(TimeOfDay), nameof(TimeOfDay.FromSecondsToTimeK))]
    public static void TimeOfDay_FromSecondsToTimeK(float time_in_secs, ref float __result)
    {
        __result = time_in_secs / Plugin.Seconds;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(TimeOfDay), nameof(TimeOfDay.GetSecondsToTheMidnight))]
    public static void TimeOfDay_GetSecondsToTheMidnight(TimeOfDay __instance, ref float __result)
    {
        __result = (1f - __instance.GetTimeK()) * Plugin.Seconds;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(TimeOfDay), nameof(TimeOfDay.GetSecondsToTheMorning))]
    public static void TimeOfDay_GetSecondsToTheMorning(TimeOfDay __instance, ref float __result)
    {
        var num = __instance.GetTimeK() - 0.15f;
        if (num < 0f)
        {
            __result = num * -1f * Plugin.Seconds;
        }
        else
        {
            __result = (1f - __instance.GetTimeK() + 0.15f) * Plugin.Seconds;
        }
    }
}