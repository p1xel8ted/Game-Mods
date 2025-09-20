using CultOfQoL.Core;

namespace CultOfQoL.Patches.Systems;

[HarmonyPatch]
public static class Weather
{
    private static WeatherSystemController WeatherSystemControllerInstance => WeatherSystemController.Instance;

    // Performance optimization: Cache weather strings to avoid repeated switch statements
    private static readonly Dictionary<WeatherSystemController.WeatherType, string> WeatherStringCache = new()
    {
        { WeatherSystemController.WeatherType.None, string.Empty },
        { WeatherSystemController.WeatherType.Raining, "Rain" },
        { WeatherSystemController.WeatherType.Windy, "Wind" },
        { WeatherSystemController.WeatherType.Snowing, "Snow" },
        { WeatherSystemController.WeatherType.Heat, "Heat" }
    };

    [HarmonyPostfix]
    [HarmonyPatch(typeof(TimeManager), nameof(TimeManager.StartNewPhase))]
    public static void TimeManager_StartNewPhase(TimeManager __instance, ref DayPhase phase)
    {
        // Performance optimization: Cache config values to avoid repeated access
        var changeWeatherOnPhaseChange = ConfigCache.GetCachedValue(ConfigCache.Keys.ChangeWeatherOnPhaseChange, () => Plugin.ChangeWeatherOnPhaseChange.Value);
        var showPhaseNotifications = ConfigCache.GetCachedValue(ConfigCache.Keys.ShowPhaseNotifications, () => Plugin.ShowPhaseNotifications.Value);
        var changeWeatherAndShowNotification = changeWeatherOnPhaseChange && showPhaseNotifications;
        
        if (changeWeatherOnPhaseChange)
        {
            if (WeatherSystemControllerInstance)
            {
                var weather = WeatherSystemControllerInstance.weatherData.RandomElement();
                WeatherSystemControllerInstance.SetWeather(weather.WeatherType, weather.WeatherStrength, 3f);

                if (changeWeatherAndShowNotification && phase is not DayPhase.Count or DayPhase.None)
                {
                    // Performance optimization: Use cached string and pre-built message
                    var phaseString = phase.ToString();
                    var weatherString = WeatherStringCache.TryGetValue(weather.WeatherType, out var cached) ? cached : string.Empty;
                    NotificationCentre.Instance.PlayGenericNotification(
                        $"{phaseString} has started and it brings {weather.WeatherStrength} {weatherString}!");
                    return;
                }
            }
        }

        if (showPhaseNotifications && phase is not DayPhase.Count or DayPhase.None && !changeWeatherAndShowNotification)
        {
            NotificationCentre.Instance.PlayGenericNotification($"{phase} has started!");
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
        // Performance optimization: Use cached config value
        if (ConfigCache.GetCachedValue(ConfigCache.Keys.RandomWeatherChangeWhenExitingArea, () => Plugin.RandomWeatherChangeWhenExitingArea.Value))
        {
            var weather = __instance.weatherData.Random();
            __instance.SetWeather(weather.WeatherType, weather.WeatherStrength, 0f);
        }
    }

    private static string GetWeatherString(WeatherSystemController.WeatherType weatherType)
    {
        // Performance optimization: Use cached dictionary lookup instead of switch
        return WeatherStringCache.TryGetValue(weatherType, out var weatherString) ? weatherString : string.Empty;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(WeatherSystemController), nameof(WeatherSystemController.SetWeather))]
    public static void WeatherSystemController_SetWeather_Postfix(WeatherSystemController __instance, WeatherSystemController.WeatherType weatherType, WeatherSystemController.WeatherStrength weatherStrength)
    {
        // Performance optimization: Use cached config value and early return
        if (!ConfigCache.GetCachedValue(ConfigCache.Keys.ShowWeatherChangeNotifications, () => Plugin.ShowWeatherChangeNotifications.Value)) 
            return;

        // Performance optimization: Cache the notifications list check to avoid LINQ overhead
        var notifications = NotificationCentre.Instance.notificationsThisFrame;
        for (var i = 0; i < notifications.Count; i++)
        {
            if (notifications[i].Contains("weather", StringComparison.OrdinalIgnoreCase))
                return;
        }

        // Performance optimization: Use cached weather string and avoid redundant ToString()
        var message = weatherType == WeatherSystemController.WeatherType.None
            ? "Weather cleared!"
            : $"Weather changed to {weatherStrength} {GetWeatherString(weatherType)}!";
            
        NotificationCentre.Instance.PlayGenericNotification(message);
    }
}