namespace CultOfQoL.Patches;

[HarmonyPatch]
public static class Weather
{
    private static WeatherSystemController WeatherSystemControllerInstance => WeatherSystemController.Instance;

    [HarmonyPostfix]
    [HarmonyPatch(typeof(TimeManager), nameof(TimeManager.StartNewPhase))]
    public static void TimeManager_StartNewPhase(TimeManager __instance, ref DayPhase phase)
    {
        var changeWeatherAndShowNotification = Plugin.ChangeWeatherOnPhaseChange.Value && Plugin.ShowPhaseNotifications.Value;
        if (Plugin.ChangeWeatherOnPhaseChange.Value)
        {
            if (WeatherSystemControllerInstance)
            {
                var weather = WeatherSystemControllerInstance.weatherData.RandomElement();
                WeatherSystemControllerInstance.SetWeather(weather.WeatherType, weather.WeatherStrength, 3f);

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

    private static Color? GetWeatherColor(WeatherSystemController.WeatherType weatherType, WeatherSystemController.WeatherStrength weatherStrength)
    {
        Color? newColor = (weatherStrength, weatherType) switch
        {
            (WeatherSystemController.WeatherStrength.Light, WeatherSystemController.WeatherType.Raining) => Plugin.LightRainColor.Value,
            (WeatherSystemController.WeatherStrength.Light, WeatherSystemController.WeatherType.Snowing) => Plugin.LightSnowColor.Value,
            (WeatherSystemController.WeatherStrength.Light, WeatherSystemController.WeatherType.Windy) => Plugin.LightWindColor.Value,
            (WeatherSystemController.WeatherStrength.Medium, WeatherSystemController.WeatherType.Raining) => Plugin.MediumRainColor.Value,
            (WeatherSystemController.WeatherStrength.Heavy, WeatherSystemController.WeatherType.Raining) => Plugin.HeavyRainColor.Value,
            _ => null
        };

        return newColor;
    }

    internal enum WeatherCombo
    {
        LightRain,
        MediumRain,
        HeavyRain,
        LightSnow,
        LightWind
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(WeatherSystemController), nameof(WeatherSystemController.GetWeatherData))]
    public static void WeatherSystemController_GetWeatherData(WeatherSystemController __instance, WeatherSystemController.WeatherType weatherType, WeatherSystemController.WeatherStrength weatherStrength, WeatherSystemController.WeatherData __result)
    {
        if (__result == null) return;

        var color = GetWeatherColor(weatherType, weatherStrength);

        if (color.HasValue)
        {
            __result.OverlayOpacity = color.Value.a;
            __result.Overlay.color = color.Value with { a = 0f };
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(WeatherSystemController), nameof(WeatherSystemController.ExitedBuilding))]
    public static void WeatherSystemController_ExitedBuilding(WeatherSystemController __instance)
    {
        if (Plugin.RandomWeatherChangeWhenExitingArea.Value)
        {
            var weather = __instance.weatherData.Random();
            __instance.SetWeather(weather.WeatherType, weather.WeatherStrength, 0f);
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
    public static void WeatherSystemController_SetWeather_Postfix(WeatherSystemController __instance, WeatherSystemController.WeatherType weatherType, WeatherSystemController.WeatherStrength weatherStrength)
    {
        if (!Plugin.ShowWeatherChangeNotifications.Value) return;

        if (NotificationCentre.Instance.notificationsThisFrame.Any(a => a.Contains("weather", StringComparison.OrdinalIgnoreCase))) return;

        NotificationCentre.Instance.PlayGenericNotification(weatherType == WeatherSystemController.WeatherType.None
            ? "Weather cleared!"
            : $"Weather changed to {weatherStrength.ToString()} {GetWeatherString(weatherType)}!");
    }
}