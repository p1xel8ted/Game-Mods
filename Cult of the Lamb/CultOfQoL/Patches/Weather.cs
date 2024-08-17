namespace CultOfQoL.Patches;

[HarmonyPatch]
public static class Weather
{
    private static WeatherSystemController WeatherSystemController { get; set; }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(TimeManager), nameof(TimeManager.StartNewPhase))]
    public static void TimeManager_StartNewPhase(TimeManager __instance, ref DayPhase phase)
    {
        var changeWeatherAndShowNotification = Plugin.ChangeWeatherOnPhaseChange.Value && Plugin.ShowPhaseNotifications.Value;
        if (Plugin.ChangeWeatherOnPhaseChange.Value)
        {
            if (WeatherSystemController != null)
            {
                var weather = WeatherSystemController.weatherData.RandomElement();
                WeatherSystemController.SetWeather(weather.WeatherType, weather.WeatherStrength, 3f);
                
                if (changeWeatherAndShowNotification && phase is not DayPhase.Count or DayPhase.None)
                {
                    NotificationCentre.Instance.PlayGenericNotification(
                        $"{phase.ToString()} has started and it brings {weather.WeatherStrength} {GetWeatherString(weather.WeatherType)}!");
                    return;
                }
            }
        }

        if (Plugin.ShowPhaseNotifications.Value && phase is not DayPhase.Count or DayPhase.None &&
            !changeWeatherAndShowNotification)
        {
            NotificationCentre.Instance.PlayGenericNotification($"{phase.ToString()} has started!");
        }
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(WeatherSystemController), nameof(WeatherSystemController.Awake))]
    [HarmonyPatch(typeof(WeatherSystemController), nameof(WeatherSystemController.Start))]
    public static void WeatherSystemController_Assign(ref WeatherSystemController __instance)
    {
        WeatherSystemController = __instance;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(WeatherSystemController), nameof(WeatherSystemController.SetWeather))]
    public static void WeatherSystemController_SetWeather_Prefix(ref WeatherSystemController __instance,
        ref WeatherSystemController.WeatherType weatherType,
        ref WeatherSystemController.WeatherStrength weatherStrength)
    {
        if (Plugin.RandomWeatherChangeWhenExitingArea.Value)
        {
            if (WeatherSystemController != null)
            {
                var weather = WeatherSystemController.weatherData.Random();
                weatherType = weather.WeatherType;
                weatherStrength = weather.WeatherStrength;
            }
        }
    }

    private static string GetWeatherString(WeatherSystemController.WeatherType weatherType)
    {
        var ws = weatherType switch
        {
            WeatherSystemController.WeatherType.None => string.Empty,
            WeatherSystemController.WeatherType.Raining => "Rain",
            WeatherSystemController.WeatherType.Windy => "Wind",
            WeatherSystemController.WeatherType.Snowing => "Snow",
            WeatherSystemController.WeatherType.Heat => "Heat",
            _ => string.Empty
        };

        return ws;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(WeatherSystemController), nameof(WeatherSystemController.SetWeather))]
    public static void WeatherSystemController_SetWeather_Postfix(ref WeatherSystemController.WeatherType weatherType,
        WeatherSystemController.WeatherStrength weatherStrength, float transitionDuration)
    {
        if (!Plugin.ShowWeatherChangeNotifications.Value) return;

        if(NotificationCentre.Instance.notificationsThisFrame.Any(a=>a.Contains("weather") || a.Contains("Weather"))) return;
        
        NotificationCentre.Instance.PlayGenericNotification(weatherType == WeatherSystemController.WeatherType.None
            ? "Weather cleared!"
            : $"Weather changed to {weatherStrength.ToString()} {GetWeatherString(weatherType)}!");
    }
}