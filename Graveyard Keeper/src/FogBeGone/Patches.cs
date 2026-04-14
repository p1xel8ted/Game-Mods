namespace FogBeGone;

[Harmony]
public static class Patches
{
    private static bool IntroPlaying { get; set; }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(CustomFlowScript), nameof(CustomFlowScript.Create), typeof(GameObject), typeof(FlowGraph), typeof(bool), typeof(CustomFlowScript.OnFinishedDelegate), typeof(string))]
    public static void CustomFlowScript_Create(ref FlowGraph g)
    {
        IntroPlaying = string.Equals(g.name, "red_eye_talk_1");
    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(SmartWeatherState), nameof(SmartWeatherState.Update))]
    public static void SmartWeatherState_Update(SmartWeatherState __instance)
    {
        if (!MainGame.game_started || !__instance || IntroPlaying) return;
        if (MainGame.me.player && MainGame.me.player.components.character.player_controlled_by_script) return;
        switch (__instance.type)
        {
            case SmartWeatherState.WeatherType.Fog:
                if (!Plugin.DisableFogCached) return;
                __instance._previously_enabled = false;
                __instance._enabled = false;
                __instance._cur_amount = 0f;
                __instance.value = 0f;
                break;

            case SmartWeatherState.WeatherType.Wind:
                if (!Plugin.DisableWindCached) return;
                __instance._previously_enabled = false;
                __instance._enabled = false;
                __instance._cur_amount = 0f;
                __instance.value = 0f;
                break;

            case SmartWeatherState.WeatherType.Rain:
                if (!Plugin.DisableRainCached) return;
                __instance._previously_enabled = false;
                __instance._enabled = false;
                __instance._cur_amount = 0f;
                __instance.value = 0f;
                break;
        }
    }
}