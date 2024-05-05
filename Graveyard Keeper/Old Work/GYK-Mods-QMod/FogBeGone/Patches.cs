using FlowCanvas;
using FogBeGone.lang;
using HarmonyLib;
using Helper;
using UnityEngine;
using Tools = Helper.Tools;

namespace FogBeGone;

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

            if (Time.time - CrossModFields.ConfigReloadTime > CrossModFields.ConfigReloadTimeLength)
            {
                Tools.ShowMessage(GetLocalizedString(strings.ConfigMessage), Vector3.zero);
                CrossModFields.ConfigReloadTime = Time.time;
            }
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(CustomFlowScript), nameof(CustomFlowScript.Create), typeof(GameObject), typeof(FlowGraph), typeof(bool), typeof(CustomFlowScript.OnFinishedDelegate), typeof(string))]
    public static void CustomFlowScript_Create(ref FlowGraph g)
    {
        _introPlaying = string.Equals(g.name, "red_eye_talk_1");
    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(SmartWeatherState), nameof(SmartWeatherState.Update))]
    public static void SmartWeatherState_Update(SmartWeatherState __instance)
    {
        if (_cfg.enableFog) return;
        if (!MainGame.game_started) return;
        if (__instance == null) return;
        if (_introPlaying) return;
        switch (__instance.type)
        {
            case SmartWeatherState.WeatherType.Fog:
                __instance._previously_enabled = true;
                __instance._enabled = false;
                __instance._cur_amount = 0;
                __instance.value = 0;
                break;

            case SmartWeatherState.WeatherType.Wind:
                __instance._previously_enabled = true;
                __instance._enabled = false;
                __instance._cur_amount = 0;
                __instance.value = 0;
                break;

            case SmartWeatherState.WeatherType.Rain:
                __instance._previously_enabled = false;
                break;

            case SmartWeatherState.WeatherType.LUT:
                break;
        }
    }
}