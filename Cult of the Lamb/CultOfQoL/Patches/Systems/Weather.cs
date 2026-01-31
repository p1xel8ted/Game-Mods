using CultOfQoL.Core;
using CultOfQoL.Patches.UI;

namespace CultOfQoL.Patches.Systems;

public enum WeatherChangeTrigger
{
    Disabled,
    OnLocationChange,
    OnPhaseChange,
    Both
}

[Harmony]
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
        var trigger = Plugin.WeatherChangeTrigger.Value;
        var changeWeatherOnPhaseChange = trigger == WeatherChangeTrigger.OnPhaseChange || trigger == WeatherChangeTrigger.Both;
        var showPhaseNotifications = ConfigCache.GetCachedValue(ConfigCache.Keys.ShowPhaseNotifications, () => Plugin.ShowPhaseNotifications.Value);
        var changeWeatherAndShowNotification = changeWeatherOnPhaseChange && showPhaseNotifications;

        if (changeWeatherOnPhaseChange)
        {
            if (WeatherSystemControllerInstance)
            {
                var weather = WeatherSystemControllerInstance.weatherData.RandomElement();
                WeatherSystemControllerInstance.SetWeather(weather.WeatherType, weather.WeatherStrength, 3f);

                if (changeWeatherAndShowNotification && phase is not DayPhase.Count and not DayPhase.None)
                {
                    if (Notifications.ShouldSuppressNotification()) return;

                    var phaseString = phase.ToString();
                    var weatherString = WeatherStringCache.TryGetValue(weather.WeatherType, out var cached) ? cached : string.Empty;
                    var message = $"{phaseString} has started and it brings {weather.WeatherStrength} {weatherString}!";
                    if (Notifications.IsDuplicateNotification(message)) return;

                    NotificationCentre.Instance.PlayGenericNotification(message);
                    return;
                }
            }
        }

        if (showPhaseNotifications && phase is not DayPhase.Count and not DayPhase.None && !changeWeatherAndShowNotification)
        {
            if (Notifications.ShouldSuppressNotification()) return;

            var message = $"{phase} has started!";
            if (Notifications.IsDuplicateNotification(message)) return;

            NotificationCentre.Instance.PlayGenericNotification(message);
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(WeatherSystemController), nameof(WeatherSystemController.Start))]
    public static void WeatherSystemController_Start_Postfix()
    {
        LocationManager.OnPlayerLocationSet -= OnLocationChanged;
        LocationManager.OnPlayerLocationSet += OnLocationChanged;
    }

    private static void OnLocationChanged()
    {
        var trigger = Plugin.WeatherChangeTrigger.Value;
        if (trigger != WeatherChangeTrigger.OnLocationChange && trigger != WeatherChangeTrigger.Both) return;
        if (!WeatherSystemControllerInstance) return;

        var weather = WeatherSystemControllerInstance.weatherData.RandomElement();
        WeatherSystemControllerInstance.SetWeather(weather.WeatherType, weather.WeatherStrength, 3f);
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(WeatherSystemController), nameof(WeatherSystemController.ChooseWeather))]
    public static bool WeatherSystemController_ChooseWeather_Prefix(WeatherSystemController __instance)
    {
        if (!Plugin.UnlockAllWeatherTypes.Value) return true;

        if (__instance.currentWeatherType != WeatherSystemController.WeatherType.None) return false;

        var weatherData = __instance.ChooseRandomWeather();
        if (weatherData == null) return false;

        if (!LocationManager.IndoorLocations.Contains(PlayerFarming.Location))
        {
            __instance.SetWeather(weatherData.WeatherType, weatherData.WeatherStrength, __instance.RandomWeatherTransitionLength);
        }
        else
        {
            __instance.SetWeather(weatherData.WeatherType, weatherData.WeatherStrength, 0f);
        }

        return false;
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
            __result.WeatherTint = color.Value;
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

        if (Notifications.ShouldSuppressNotification())
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

        if (Notifications.IsDuplicateNotification(message))
            return;

        NotificationCentre.Instance.PlayGenericNotification(message);
    }
}