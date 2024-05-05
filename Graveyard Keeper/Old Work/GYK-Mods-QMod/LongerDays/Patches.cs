using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using Helper;
using LongerDays.lang;
using UnityEngine;
using Tools = Helper.Tools;

namespace LongerDays;

[HarmonyPatch]
public static partial class MainPatcher
{
    //hooks into the time of day update and saves if the K key was pressed
    [HarmonyPrefix]
    [HarmonyPatch(typeof(TimeOfDay), nameof(TimeOfDay.Update))]
    public static void TimeOfDay_Update()
    {
        if (Input.GetKeyUp(_cfg.reloadConfigKeyBind))
        {
            _cfg = Config.GetOptions();
            RefreshConfig();

            if (Time.time - CrossModFields.ConfigReloadTime > CrossModFields.ConfigReloadTimeLength)
            {
                Tools.ShowMessage(GetLocalizedString(strings.ConfigMessage), Vector3.zero);
                CrossModFields.ConfigReloadTime = Time.time;
            }
        }
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
                    operand: typeof(MainPatcher).GetMethod("GetTime"));
            }

            yield return instruction;
        }
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(TimeOfDay), nameof(TimeOfDay.FromTimeKToSeconds))]
    public static void TimeOfDay_FromTimeKToSeconds(ref float time_in_time_k, ref float __result)
    {
        __result = time_in_time_k * _seconds;
    }


    //used by weather systems
    [HarmonyPostfix]
    [HarmonyPatch(typeof(TimeOfDay), nameof(TimeOfDay.FromSecondsToTimeK))]
    public static void TimeOfDay_FromSecondsToTimeK(ref float time_in_secs, ref float __result)
    {
        __result = time_in_secs / _seconds;
    }


    //this is only used by flow canvas?
    [HarmonyPostfix]
    [HarmonyPatch(typeof(TimeOfDay), nameof(TimeOfDay.GetSecondsToTheMidnight))]
    public static void TimeOfDay_GetSecondsToTheMidnight(TimeOfDay __instance, ref float __result)
    {
        __result = (1f - __instance.GetTimeK()) * _seconds;
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
            result = num * -1f * _seconds;
        }
        else
        {
            result = (1f - __instance.GetTimeK() + 0.15f) * _seconds;
        }

        __result = result;
    }
}