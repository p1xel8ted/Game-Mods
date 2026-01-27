using System;
using System.Collections.Generic;

namespace CultOfQoL.Core;

/// <summary>
/// Performance-optimized configuration caching system to reduce repeated config access
/// </summary>
internal static class ConfigCache
{
    private static readonly Dictionary<string, object> Cache = new();
    private static readonly HashSet<string> DirtyKeys = [];

    /// <summary>
    /// Get a cached config value or cache it if not present
    /// </summary>
    public static T GetCachedValue<T>(string key, Func<T> valueGetter)
    {
        if (DirtyKeys.Contains(key) || !Cache.TryGetValue(key, out var cachedValue))
        {
            var newValue = valueGetter();
            Cache[key] = newValue;
            DirtyKeys.Remove(key);
            return newValue;
        }

        return (T)cachedValue;
    }

    /// <summary>
    /// Mark a config key as dirty, forcing a refresh on next access
    /// </summary>
    public static void MarkDirty(string key)
    {
        DirtyKeys.Add(key);
    }


    /// <summary>
    /// Commonly accessed config values with static keys for performance
    /// </summary>
    public static class Keys
    {
        public const string EnableLogging = nameof(EnableLogging);
        public const string CookedMeatMealsContainBone = nameof(CookedMeatMealsContainBone);
        public const string ChangeWeatherOnPhaseChange = nameof(ChangeWeatherOnPhaseChange);
        public const string ShowPhaseNotifications = nameof(ShowPhaseNotifications);
        public const string ShowWeatherChangeNotifications = nameof(ShowWeatherChangeNotifications);
        public const string RandomWeatherChangeWhenExitingArea = nameof(RandomWeatherChangeWhenExitingArea);
        public const string LumberAndMiningStationsDontAge = nameof(LumberAndMiningStationsDontAge);
        public const string LumberAndMiningStationsAgeMultiplier = nameof(LumberAndMiningStationsAgeMultiplier);
        public const string TurnOffSpeakersAtNight = nameof(TurnOffSpeakersAtNight);
        public const string DisablePropagandaSpeakerAudio = nameof(DisablePropagandaSpeakerAudio);
        public const string SoulCapacityMulti = nameof(SoulCapacityMulti);
        public const string SiloCapacityMulti = nameof(SiloCapacityMulti);
        public const string UseMultiplesOf32 = nameof(UseMultiplesOf32);
        public const string AdjustRefineryRequirements = nameof(AdjustRefineryRequirements);
        public const string AddSpiderWebsToOfferings = nameof(AddSpiderWebsToOfferings);
        public const string AddCrystalShardsToOfferings = nameof(AddCrystalShardsToOfferings);
        public const string ProduceSpiderWebsFromLumber = nameof(ProduceSpiderWebsFromLumber);
        public const string SpiderWebsPerLogs = nameof(SpiderWebsPerLogs);
        public const string ProduceCrystalShardsFromStone = nameof(ProduceCrystalShardsFromStone);
        public const string CrystalShardsPerStone = nameof(CrystalShardsPerStone);
        public const string ResourceChestDepositSounds = nameof(ResourceChestDepositSounds);
        public const string ResourceChestCollectSounds = nameof(ResourceChestCollectSounds);
    }
}