using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;


namespace LongerDays;

[HarmonyPatch]
public static class Patches
{

    internal static float GetTimeMulti()
    {
        var num = Plugin.Seconds switch
        {
            Plugin.DefaultIncreaseSeconds => 1.5f,
            Plugin.DoubleLengthSeconds => 2f,
            Plugin.EvenLongerSeconds => 2.5f,
            Plugin.MadnessSeconds => 3f,
            _ => 1f
        };
        return num;
    }
    public static float GetTime()
    {
        var adj = GetTimeMulti();
        var time = Time.deltaTime;
        var newTime = time / adj;
        return newTime;
    }
    
    [HarmonyTranspiler]
    [HarmonyPatch(typeof(EnvironmentEngine), nameof(EnvironmentEngine.Update))]
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var instructionsList = new List<CodeInstruction>(instructions);
        var time = AccessTools.Property(typeof(Time), nameof(Time.deltaTime)).GetGetMethod();

        foreach (var t in instructionsList)
        {
            var instruction = t;
            if (instruction.opcode == OpCodes.Call && instruction.OperandIs(time))
            {
                yield return instruction;
                instruction = new CodeInstruction(opcode: OpCodes.Call,
                    operand: AccessTools.Method(typeof(Patches), nameof(GetTime)));
            }

            yield return instruction;
        }
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(TimeOfDay), nameof(TimeOfDay.FromTimeKToSeconds))]
    public static void TimeOfDay_FromTimeKToSeconds(ref float time_in_time_k, ref float __result)
    {
        __result = time_in_time_k * Plugin.Seconds;
    }


    //used by weather systems
    [HarmonyPostfix]
    [HarmonyPatch(typeof(TimeOfDay), nameof(TimeOfDay.FromSecondsToTimeK))]
    public static void TimeOfDay_FromSecondsToTimeK(ref float time_in_secs, ref float __result)
    {
        __result = time_in_secs / Plugin.Seconds;
    }


    //this is only used by flow canvas?
    [HarmonyPostfix]
    [HarmonyPatch(typeof(TimeOfDay), nameof(TimeOfDay.GetSecondsToTheMidnight))]
    public static void TimeOfDay_GetSecondsToTheMidnight(TimeOfDay __instance, ref float __result)
    {
        __result = (1f - __instance.GetTimeK()) * Plugin.Seconds;
    }


    //only used by refugee cooking
    [HarmonyPostfix]
    [HarmonyPatch(typeof(TimeOfDay), nameof(TimeOfDay.GetSecondsToTheMorning))]
    public static void TimeOfDay_GetSecondsToTheMorning(TimeOfDay __instance, ref float __result)
    {
        var num = __instance.GetTimeK() - 0.15f;
        float result;
        if (num < 0f)
        {
            result = num * -1f * Plugin.Seconds;
        }
        else
        {
            result = (1f - __instance.GetTimeK() + 0.15f) * Plugin.Seconds;
        }

        __result = result;
    }
}